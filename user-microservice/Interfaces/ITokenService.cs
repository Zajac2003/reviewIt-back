using user_microservice.Models;

namespace user_microservice.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
