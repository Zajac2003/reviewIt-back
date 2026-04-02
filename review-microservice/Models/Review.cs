using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace review_microservice.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int Value { get; set; }
        [MaxLength(100)]
        public string? Title { get; set; }
        [MaxLength(10000)]
        public string? Content { get; set; }
        [Required]
        public int AlbumId { get; set; }
        [Required]
        [StringLength(36)]
        public string AppUserId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public IEnumerable<Comment>? Comments { get; set; }
    }
}
