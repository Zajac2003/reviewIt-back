using review_microservice.Models;
using System.ComponentModel.DataAnnotations;

namespace review_microservice.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Content { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [Required]
        [MaxLength(36)]
        public string AppUserId { get; set; }
        [Required]
        public int ReviewId { get; set; }
        public Review Review { get; set; }
    }
}