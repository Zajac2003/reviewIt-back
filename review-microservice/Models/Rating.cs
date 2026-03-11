using System.ComponentModel.DataAnnotations;

namespace review_microservice.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        [Range(1, 10)]
        [Required]
        public int Value { get; set; }
        [Required]
        public int AlbumId { get; set; }
        [Required]
        public string AppUserId { get; set; }
       
    }
}
