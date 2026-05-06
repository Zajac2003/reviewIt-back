using review_microservice.Dtos;

namespace review_microservice.Interfaces
{
    public interface IDiscogsService
    {
        Task<PagedResponseDto<DiscogsSearchItemDto>> SearchAsync(string query, int page, int pageSize);

        /// <summary>Master release (Discogs) — <c>albumId</c> w aplikacji to ID mastera.</summary>
        Task<DiscogsMasterDetailDto?> GetMasterByIdAsync(int masterId);
    }
}