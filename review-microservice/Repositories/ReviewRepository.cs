using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using review_microservice.Data;
using review_microservice.Interfaces;
using review_microservice.Models;

namespace review_microservice.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Review review)
        {
            _context.Reviews.Remove(review);
            return await SaveAsync();
        }

        public async Task<IReadOnlyCollection<Review>> GetAllAsync()
        {
            IReadOnlyCollection<Review> reviews = await _context.Reviews
                .AsNoTracking() // podobno przyspiesza działanie, zwłaszcza gdy nie zamierzamy modyfikować tego
                .ToArrayAsync();

            return reviews;
        }

        public async Task<bool> UserHasReviewForAlbum(string userId, int albumId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.AppUserId == userId && r.AlbumId == albumId);
        }

        public async Task<Review> GetByIdAsync(int id)
        {
            Review review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.Id == id);
            return review;
        }

        public async Task<Review> GetByIdWithCommentsAsync(int id)
        {
            Review review = await _context.Reviews
                 .Include(b => b.Comments)
                .FirstOrDefaultAsync(r => r.Id == id);
            return review;
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Review review)
        {
            _context.Reviews.Update(review);
            return await SaveAsync();
        }
    }
}
