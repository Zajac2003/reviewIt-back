using review_microservice.Models;

namespace review_microservice.Interfaces
{
    public interface ICommentRepository
    {
        Task<bool> AddAsync(Comment comment);
        Task<bool> UpdateAsync(Comment comment);
        Task<bool> DeleteAsync(Comment comment);
        Task<bool> SaveAsync();
        Task<IReadOnlyCollection<Comment>> GetByReviewAsync(int reviewId);
    }
}
