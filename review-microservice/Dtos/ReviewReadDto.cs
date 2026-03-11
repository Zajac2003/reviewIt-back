using review_microservice.Models;
using System.ComponentModel.DataAnnotations;

namespace review_microservice.Dtos
{
    public class ReviewReadDto
    {
        public int Id { get; set; }
        public int Value { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AlbumId { get; set; }
        public string AppUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
