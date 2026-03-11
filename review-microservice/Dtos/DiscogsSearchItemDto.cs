namespace review_microservice.Dtos
{
    public class DiscogsSearchItemDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public int? Year { get; set; }

        public string? Country { get; set; }

        public IReadOnlyCollection<string> Genre { get; set; } = Array.Empty<string>();

        public string? CoverImage { get; set; }
    }
}