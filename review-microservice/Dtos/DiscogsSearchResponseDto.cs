using System.Text.Json.Serialization;

namespace review_microservice.Dtos
{
    public class DiscogsSearchResponseDto
    {
        [JsonPropertyName("results")]
        public List<DiscogsSearchResultDto> Results { get; set; } = new();

        [JsonPropertyName("pagination")]
        public DiscogsPaginationDto Pagination { get; set; } = new();
    }

    public class DiscogsSearchResultDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("year")]
        public string? Year { get; set; }

        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("genre")]
        public List<string>? Genre { get; set; }

        [JsonPropertyName("cover_image")]
        public string? CoverImage { get; set; }
    }

    public class DiscogsPaginationDto
    {
        [JsonPropertyName("page")]
        public int Page { get; set; }

        [JsonPropertyName("pages")]
        public int Pages { get; set; }

        [JsonPropertyName("per_page")]
        public int PerPage { get; set; }

        [JsonPropertyName("items")]
        public int Items { get; set; }
    }
}