using review_microservice.Models;

namespace review_microservice.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review> GetByIdAsync(int id);
        Task<bool> AddAsync(Review review);
        Task<bool> UpdateAsync(Review review);
        Task<bool> DeleteAsync(Review review);
        Task<bool> SaveAsync();
        Task<IReadOnlyCollection<Review>> GetAllAsync();
        Task<Review> GetByIdWithCommentsAsync(int id);
        Task<bool> UserHasReviewForAlbum(string userId, int albumId);
    }
}
