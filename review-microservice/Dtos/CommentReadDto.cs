using review_microservice.Models;
using System.ComponentModel.DataAnnotations;

namespace review_microservice.Dtos
{
    public class CommentReadDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
        public string AppUserId { get; set; }
        public int ReviewId { get; set; }
    }
}
