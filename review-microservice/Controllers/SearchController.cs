using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using review_microservice.Dtos;
using review_microservice.Interfaces;

namespace review_microservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly IDiscogsService _discogsService;

        public SearchController(IDiscogsService discogsService)
        {
            _discogsService = discogsService;
        }

        [HttpGet("discogs")]
        public async Task<ActionResult<PagedResponseDto<DiscogsSearchItemDto>>> SearchDiscogs(
            [FromQuery] string q,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest("Query parameter 'q' is required.");
            }

            try
            {
                var result = await _discogsService.SearchAsync(q, page, pageSize);
                return Ok(result);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, new
                {
                    message = "Error while calling Discogs API.",
                    details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected server error.",
                    details = ex.Message
                });
            }
        }
    }
}