using System.ComponentModel.DataAnnotations;

namespace user_microservice.Dtos
{
    public class BanInputDto
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool ShouldBeBanned { get; set; }
    }
}
