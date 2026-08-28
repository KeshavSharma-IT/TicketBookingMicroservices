using EventServices.Application.DTO.Venue;

namespace EventServices.Application.IServices
{
    public interface IVenueService
    {
        Task<VenueResponseDto> CreateVenueAsync(CreateVenueDto createVenueDto);

        Task<ScreenResponseDto> CreateScreenAsync(CreateScreenDto createScreen,Guid venueId);
        Task<List<VenueResponseDto>> GetVenueAsync();
        Task<List<VenueResponseDto>> GetVenueByCityAsync(string city);

        Task<VenueResponseDto> GetVenueByIdAsync(Guid id);
        Task<bool> DeactivateVenueAsync(Guid Id);
    }
}
