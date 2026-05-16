using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using review_microservice.Dtos;
using review_microservice.Interfaces;

namespace review_microservice.Services
{
    public class DiscogsService : IDiscogsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public DiscogsService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<PagedResponseDto<DiscogsSearchItemDto>> SearchAsync(string query, int page, int pageSize)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                throw new ArgumentException("Query cannot be empty.");
            }

            int defaultPageSize = _configuration.GetValue<int>("Discogs:DefaultPageSize", 10);
            int maxPageSize = _configuration.GetValue<int>("Discogs:MaxPageSize", 50);

            if (page < 1)
            {
                page = 1;
            }

            if (pageSize <= 0)
            {
                pageSize = defaultPageSize;
            }

            if (pageSize > maxPageSize)
            {
                pageSize = maxPageSize;
            }

            string consumerKey = _configuration["Discogs:ConsumerKey"] ?? string.Empty;
            string consumerSecret = _configuration["Discogs:ConsumerSecret"] ?? string.Empty;

           string url = $"/database/search?type=master&q={Uri.EscapeDataString(query)}&page={page}&per_page={pageSize}";

            if (!string.IsNullOrWhiteSpace(consumerKey) && !string.IsNullOrWhiteSpace(consumerSecret))
            {
                url += $"&key={Uri.EscapeDataString(consumerKey)}&secret={Uri.EscapeDataString(consumerSecret)}";
            }

            using var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Discogs API request failed. StatusCode: {(int)response.StatusCode}, Response: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var discogsResponse = JsonSerializer.Deserialize<DiscogsSearchResponseDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (discogsResponse == null)
            {
                throw new InvalidOperationException("Failed to deserialize Discogs API response.");
            }

            return new PagedResponseDto<DiscogsSearchItemDto>
            {
                CurrentPage = discogsResponse.Pagination.Page,
                PageSize = discogsResponse.Pagination.PerPage,
                TotalItems = discogsResponse.Pagination.Items,
                TotalPages = discogsResponse.Pagination.Pages,
                Items = discogsResponse.Results.Select(x => new DiscogsSearchItemDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Type = x.Type,
                    Year = int.TryParse(x.Year, out int parsedYear) ? parsedYear : null,
                    Country = x.Country,
                    Genre = x.Genre ?? new List<string>(),
                    CoverImage = x.CoverImage
                }).ToArray()
            };
        }

        public async Task<DiscogsMasterDetailDto?> GetMasterByIdAsync(int masterId)
        {
            if (masterId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(masterId));
            }

            string consumerKey = _configuration["Discogs:ConsumerKey"] ?? string.Empty;
            string consumerSecret = _configuration["Discogs:ConsumerSecret"] ?? string.Empty;

            string url = $"/masters/{masterId}";
            if (!string.IsNullOrWhiteSpace(consumerKey) && !string.IsNullOrWhiteSpace(consumerSecret))
            {
                url += $"?key={Uri.EscapeDataString(consumerKey)}&secret={Uri.EscapeDataString(consumerSecret)}";
            }

            using var response = await _httpClient.GetAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"Discogs API request failed. StatusCode: {(int)response.StatusCode}, Response: {errorContent}");
            }

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<DiscogsMasterApiJson>(json, options);
            if (data == null)
            {
                throw new InvalidOperationException("Failed to deserialize Discogs master response.");
            }

            string? cover = null;
            if (data.Images is { Count: > 0 })
            {
                var primary = data.Images.FirstOrDefault(i =>
                    string.Equals(i.Type, "primary", StringComparison.OrdinalIgnoreCase));
                cover = primary?.Uri ?? data.Images[0].Uri;
            }

            var tracks = new List<DiscogsTrackItemDto>();
            if (data.Tracklist != null)
            {
                foreach (var t in data.Tracklist)
                {
                    if (string.IsNullOrWhiteSpace(t.Title))
                    {
                        continue;
                    }

                    var isHeading = string.Equals(t.Type, "heading", StringComparison.OrdinalIgnoreCase);
                    var title = isHeading ? $"— {t.Title.Trim()}" : t.Title.Trim();
                    tracks.Add(new DiscogsTrackItemDto
                    {
                        Position = t.Position?.Trim() ?? string.Empty,
                        Title = title,
                        Duration = string.IsNullOrWhiteSpace(t.Duration) ? null : t.Duration.Trim()
                    });
                }
            }

            return new DiscogsMasterDetailDto
            {
                Id = data.Id,
                Title = data.Title?.Trim() ?? string.Empty,
                Year = data.Year,
                Artists = data.Artists?
                    .Select(a => a.Name?.Trim())
                    .Where(n => !string.IsNullOrEmpty(n))
                    .Select(n => n!)
                    .ToArray() ?? Array.Empty<string>(),
                Genres = data.Genres?.Where(g => !string.IsNullOrWhiteSpace(g)).Select(g => g.Trim()).ToArray()
                    ?? Array.Empty<string>(),
                Styles = data.Styles?.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray()
                    ?? Array.Empty<string>(),
                Notes = StripDiscogsNotes(data.Notes),
                CoverImage = cover,
                Tracklist = tracks
            };
        }

        private static string? StripDiscogsNotes(string? html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return null;
            }

            var text = Regex.Replace(html, "<[^>]+>", " ");
            text = System.Net.WebUtility.HtmlDecode(text);
            return Regex.Replace(text, @"\s+", " ").Trim();
        }
    }
}