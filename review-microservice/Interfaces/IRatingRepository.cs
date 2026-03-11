using review_microservice.Models;

namespace review_microservice.Interfaces
{
    public interface IRatingRepository
    {
        Task<Rating> GetByIdAsync(int id);
        Task<bool> AddAsync(Rating rating);
        Task<bool> UpdateAsync(Rating rating);
        Task<bool> DeleteAsync(Rating rating);
        Task<bool> SaveAsync();

    }
}
