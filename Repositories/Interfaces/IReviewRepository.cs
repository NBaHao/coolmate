using CoolMate.Models;

namespace CoolMate.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<UserReview>> GetReviewsAsync();
        Task<IEnumerable<UserReview>> GetReviewsOfProductAsync(int productId);
        Task<UserReview> GetReviewAsync(int reviewId);
        Task<bool> AddReviewAsync(UserReview review);
        Task<bool> UpdateReviewAsync(UserReview review);
        Task<bool> DeleteReviewAsync(int reviewId);
        Task<bool> IsReviewedAsync(string? userId, int? productItemId);
    }
}
