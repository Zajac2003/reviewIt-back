using Microsoft.EntityFrameworkCore;
using review_microservice.Data;
using review_microservice.Interfaces;
using review_microservice.Models;

namespace review_microservice.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> AddAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            return await SaveAsync();
        }

        public async Task<IReadOnlyCollection<Comment>> GetByReviewAsync(int reviewId)
        {
            return await _context.Comments
                .Where(c => c.ReviewId == reviewId)
                .ToArrayAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
