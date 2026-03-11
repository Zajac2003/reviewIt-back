using Microsoft.EntityFrameworkCore;
using review_microservice.Data;
using review_microservice.Interfaces;
using review_microservice.Models;

namespace review_microservice.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepository(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<Rating> GetByIdAsync(int id)
        {
            var rating = await _context.Ratings.FirstOrDefaultAsync(x => x.Id == id);
            return rating;
        }

        public async Task<bool> AddAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            return await SaveAsync();
        }

        public async Task<bool> UpdateAsync(Rating rating)
        {
            _context.Ratings.Update(rating);
            return await SaveAsync();
        }

        public async Task<bool> DeleteAsync(Rating rating)
        {
            _context.Ratings.Remove(rating);
            return await SaveAsync();
        }

        public async Task<bool> SaveAsync()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }
    }
}
