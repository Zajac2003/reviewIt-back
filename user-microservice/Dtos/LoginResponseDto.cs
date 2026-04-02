namespace user_microservice.Dtos
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public string? Username { get; set; }
        public int ExpiresIn { get; set; } //seconds
    }
}
