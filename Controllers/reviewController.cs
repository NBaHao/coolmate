using CoolMate.DTO;
using CoolMate.Models;
using CoolMate.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoolMate.Controllers
{
    [Route("api/[controller]")]
    public class reviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IShopOrderRepository _shopOrderRepository;
        public reviewController(IReviewRepository reviewRepository, IShopOrderRepository shopOrderRepository)
        {
            _reviewRepository = reviewRepository;
            _shopOrderRepository = shopOrderRepository;
        }

        [HttpGet("getAll")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> GetAllReviews()
        {
            var reviews = await _reviewRepository.GetReviewsAsync();
            var response = reviews.Select(r => new UserReviewDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                Name = r.User.Name,
                OrderedProductId = r.OrderedProductId,
                RatingValue = r.RatingValue,
                Comment = r.Comment,
                Color = r.OrderedProduct.Color,
                Size = r.OrderedProduct.Size,
                CreatedDate = r.CreatedDate
            });
            return Ok(response);
        }

        [HttpGet("getOfProduct")]
        public async Task<ActionResult> GetReviewsOfProduct(int productId)
        {
            var reviews = await _reviewRepository.GetReviewsOfProductAsync(productId);
            var response = reviews.Select(r => new UserReviewDTO
            {
                Id = r.Id,
                UserId = r.UserId,
                Name = r.User.Name,
                OrderedProductId = r.OrderedProductId,
                RatingValue = r.RatingValue,
                Comment = r.Comment,
                Color = r.OrderedProduct.Color,
                Size = r.OrderedProduct.Size,
                CreatedDate = r.CreatedDate
            });
            return Ok(new
            {
                total = response.Count(),
                rating = response.Average(r => r.RatingValue),
                reviews = response
            });
        }

        [HttpGet("getOne")]
        public async Task<ActionResult> GetReview(int reviewId)
        {
            var review = await _reviewRepository.GetReviewAsync(reviewId);
            if (review == null) return NotFound();
            var response = new UserReviewDTO
            {
                Id = review.Id,
                UserId = review.UserId,
                Name = review.User.Name,
                OrderedProductId = review.OrderedProductId,
                RatingValue = review.RatingValue,
                Comment = review.Comment,
                Color = review.OrderedProduct.Color,
                Size = review.OrderedProduct.Size,
                CreatedDate = review.CreatedDate
            };
            return Ok(response);
        }

        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult> AddReview([FromBody] AddReviewDTO userReviewDTO)
        {
            if (userReviewDTO.RatingValue < 1 || userReviewDTO.RatingValue > 5) return BadRequest("Rating value must be between 1 and 5");
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var isReviewed = await _reviewRepository.IsReviewedAsync(userId, userReviewDTO.ProductItemId);
            if (isReviewed) return BadRequest("You have reviewed this product");

            var isOrdered = await _shopOrderRepository.IsProductItemOrderedAsync(userId, userReviewDTO.ProductItemId);
            if (!isOrdered) return BadRequest("You haven't ordered this product yet");
            var review = new UserReview
            {
                UserId = userId,
                OrderedProductId = userReviewDTO.ProductItemId,
                RatingValue = userReviewDTO.RatingValue,
                Comment = userReviewDTO.Comment,
                CreatedDate = DateTime.Now
            };
            var res = await _reviewRepository.AddReviewAsync(review);
            if (res) return Ok(res);
            return BadRequest(res);
        }

        [HttpDelete("delete")]
        [Authorize]
        public async Task<ActionResult> DeleteReview(int reviewId)
        {
            var review = await _reviewRepository.GetReviewAsync(reviewId);
            if (review == null) return NotFound();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (review.UserId != userId) return BadRequest("You don't have permission to delete this review");
            var res = await _reviewRepository.DeleteReviewAsync(reviewId);
            if (res) return Ok(res);
            return BadRequest(res);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<ActionResult> UpdateReview([FromBody] UpdateReviewDTO userReviewDTO)
        {
            if (userReviewDTO.RatingValue < 1 || userReviewDTO.RatingValue > 5) return BadRequest("Rating value must be between 1 and 5");
            var review = await _reviewRepository.GetReviewAsync(userReviewDTO.Id);
            if (review == null) return NotFound();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (review.UserId != userId) return BadRequest("You don't have permission to update this review");
            review.RatingValue = userReviewDTO.RatingValue;
            review.Comment = userReviewDTO.Comment;
            var res = await _reviewRepository.UpdateReviewAsync(review);
            if (res) return Ok(res);
            return BadRequest(res);
        }
    }
}
