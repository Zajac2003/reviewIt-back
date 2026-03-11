using System.ComponentModel.DataAnnotations;

namespace review_microservice.Dtos
{
    public class RatingReadDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public int AlbumId { get; set; }
        public string AppUserId { get; set; }
    }
}
