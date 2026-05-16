namespace user_microservice.Dtos
{
    public class AppUserReadDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsBanned { get; set; }
    }
}
