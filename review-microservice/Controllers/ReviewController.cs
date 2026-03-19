using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using review_microservice.Dtos;
using review_microservice.Interfaces;
using review_microservice.Models;

namespace review_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository repository)
        {
            _reviewRepository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<ReviewReadDto>>> Index()
        {
            var reviews = await _reviewRepository.GetAllAsync();

            var reviewDtos = reviews.Select(review => new ReviewReadDto
            {
                AlbumId = review.AlbumId,
                AppUserId = review.AppUserId,
                Content = review.Content,
                CreatedAt = review.CreatedAt,
                Id = review.Id,
                Title = review.Title,
                Value = review.Value
            });

            return Ok(reviewDtos);
        }

        [HttpGet("{id}", Name = "GetReviewById")]
        public async Task<ActionResult<ReviewReadDto>> GetReviewById(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);

            if (review != null)
            {
                var reviewDto = new ReviewReadDto
                {
                    AlbumId = review.AlbumId,
                    AppUserId = review.AppUserId,
                    Content = review.Content,
                    CreatedAt = review.CreatedAt,
                    Id = review.Id,
                    Title = review.Title,
                    Value = review.Value
                };
                return Ok(reviewDto);
            }
            else return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<ReviewReadDto>> CreateReview([FromBody] ReviewCreateDto dto)
        {
            var review = new Review()
            {
                Title = dto.Title,
                Value = dto.Value,
                AlbumId = dto.AlbumId,
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                AppUserId = dto.AppUserId //na razie tak 
            };

            var success = await _reviewRepository.AddAsync(review);

            if (success)
            {
                var reviewRead = new ReviewReadDto()
                {
                    AlbumId = review.AlbumId,
                    AppUserId = review.AppUserId,
                    Content = review.Content,
                    CreatedAt = review.CreatedAt,
                    Id = review.Id,
                    Title = review.Title,
                    Value = review.Value
                };
                return CreatedAtRoute(nameof(GetReviewById), new { id = reviewRead.Id }, reviewRead);
            }
            else return BadRequest();
        }
    }
}