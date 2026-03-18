using System.ComponentModel.DataAnnotations;

namespace user_microservice.Dtos
{
    public class LoginInputDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}