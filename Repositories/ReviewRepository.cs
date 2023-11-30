using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CoolMate.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DBContext _context;
        public ReviewRepository(DBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserReview>> GetReviewsAsync()
        {
            return await _context.UserReviews
                .Include(x=>x.User)
                .Include(x=>x.OrderedProduct)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserReview>> GetReviewsOfProductAsync(int productId)
        {
            return await _context.UserReviews
                .Include(x => x.User)
                .Include(x => x.OrderedProduct)
                .Where(r => r.OrderedProduct.ProductId == productId)
                .ToListAsync();
        }

        public async Task<UserReview> GetReviewAsync(int reviewId)
        {
            return await _context.UserReviews
                .Include(x => x.User)
                .Include(x => x.OrderedProduct)
                .FirstOrDefaultAsync(x=>x.Id == reviewId);
        }

        public async Task<bool> AddReviewAsync(UserReview review)
        {
            _context.UserReviews.Add(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateReviewAsync(UserReview review)
        {
            _context.UserReviews.Update(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReviewAsync(int reviewId)
        {
            var review = await _context.UserReviews.FindAsync(reviewId);
            if (review == null) return false;
            _context.UserReviews.Remove(review);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> IsReviewedAsync(string? userId, int? productItemId)
        {
            var review = await _context.UserReviews.FirstOrDefaultAsync(x => x.UserId == userId && x.OrderedProductId == productItemId);
            return review != null;
        }
    }
}
