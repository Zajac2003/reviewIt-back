namespace review_microservice.Dtos
{
    public class DiscogsMasterDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? Year { get; set; }
        public IReadOnlyList<string> Artists { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Genres { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Styles { get; set; } = Array.Empty<string>();
        /// <summary>Oczyszczony tekst (bez HTML z Discogs).</summary>
        public string? Notes { get; set; }
        public string? CoverImage { get; set; }
        public IReadOnlyList<DiscogsTrackItemDto> Tracklist { get; set; } = Array.Empty<DiscogsTrackItemDto>();
    }

    public class DiscogsTrackItemDto
    {
        public string Position { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Duration { get; set; }
    }
}
