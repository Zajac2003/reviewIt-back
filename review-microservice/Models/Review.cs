using System.ComponentModel.DataAnnotations;

namespace review_microservice.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int Value { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int AlbumId { get; set; }
        public string AppUserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
    }
}
