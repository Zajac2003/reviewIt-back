using user_microservice.Models;

namespace user_microservice.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user);
    }
}
