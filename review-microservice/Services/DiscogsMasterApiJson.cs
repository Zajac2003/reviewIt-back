using System.Text.Json.Serialization;

namespace review_microservice.Services
{
    internal sealed class DiscogsMasterApiJson
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("year")]
        public int? Year { get; set; }

        [JsonPropertyName("notes")]
        public string? Notes { get; set; }

        [JsonPropertyName("genres")]
        public List<string>? Genres { get; set; }

        [JsonPropertyName("styles")]
        public List<string>? Styles { get; set; }

        [JsonPropertyName("artists")]
        public List<DiscogsArtistApiJson>? Artists { get; set; }

        [JsonPropertyName("tracklist")]
        public List<DiscogsTrackApiJson>? Tracklist { get; set; }

        [JsonPropertyName("images")]
        public List<DiscogsImageApiJson>? Images { get; set; }
    }

    internal sealed class DiscogsArtistApiJson
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }

    internal sealed class DiscogsTrackApiJson
    {
        [JsonPropertyName("position")]
        public string? Position { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("duration")]
        public string? Duration { get; set; }

        [JsonPropertyName("type_")]
        public string? Type { get; set; }
    }

    internal sealed class DiscogsImageApiJson
    {
        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("uri")]
        public string? Uri { get; set; }
    }
}
