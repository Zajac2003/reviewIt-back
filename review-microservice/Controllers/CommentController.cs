using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using review_microservice.Dtos;
using review_microservice.Interfaces;
using review_microservice.Models;

namespace review_microservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IReviewRepository _reviewRepository;

        public CommentController(ICommentRepository repository, IReviewRepository reviewRepository)
        {
            _commentRepository = repository;
            _reviewRepository = reviewRepository;
        }

        [HttpGet("review/{reviewId}", Name = "GetCommentsByReview")]
        public async Task<ActionResult<IReadOnlyCollection<ReviewReadDto>>> GetCommentsByReview(int reviewId)
        {
            var review = await _reviewRepository.GetByIdAsync(reviewId);

            if (review == null)
            {
                return NotFound($"Review with id {reviewId} not found.");
            }

            var comments = await _commentRepository.GetByReviewAsync(reviewId);

            var commentDtos = comments.Select(c => new CommentReadDto
            {
                AppUserId = c.AppUserId,
                Content = c.Content,
                CreatedDate = c.CreatedDate,
                Id = c.Id
            });

            return Ok(commentDtos);
        }

        [HttpPost]
        public async Task<ActionResult<CommentReadDto>> CreateComment([FromBody] CommentCreateDto dto)
        {
            var review = await _reviewRepository.GetByIdAsync(dto.ReviewId);

            if (review == null)
            {
                return BadRequest($"Review with id {dto.ReviewId} not found.");
            }

            var comment = new Comment()
            {
                Content = dto.Content,
                CreatedDate = DateTime.UtcNow,
                AppUserId = dto.AppUserId //na razie tak 
            };

            var success = await _commentRepository.AddAsync(comment);

            if (success)
            {
                var commentRead = new CommentReadDto()
                {
                    AppUserId = comment.AppUserId,
                    Content = comment.Content,
                    CreatedDate = comment.CreatedDate,
                    Id = comment.Id
                };
                return CreatedAtRoute(nameof(GetCommentsByReview), new { reviewId = review.Id }, commentRead);
            }
            else return BadRequest();
        }
    }
}