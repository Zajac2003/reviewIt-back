using review_microservice.Models;
using System.ComponentModel.DataAnnotations;

namespace review_microservice.Dtos
{
    public class ReviewCreateDto
    {
        [Required(ErrorMessage = "Value is required.")]
        [Range(1, 10, ErrorMessage = "Value must be in range (1, 10)")]
        public int Value { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Title cannot be empty.")]
        [MaxLength(100, ErrorMessage = "Title is too long. Use up to 100 characters.")]
        public string Title { get; set; }


        [MinLength(30, ErrorMessage = "Review content is too short. Use at least 30 characters.")]
        [MaxLength(10000, ErrorMessage = "Content is too long. Use up to 10000 characters.")]
        public string Content { get; set; }


        [Required(ErrorMessage = "AlbumId is required.")]
        public int AlbumId { get; set; }


        public string AppUserId { get; set; }
    }
}
