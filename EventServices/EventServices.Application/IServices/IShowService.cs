using EventServices.Application.DTO.Show;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application.IServices
{
    public interface IShowService
    {
        Task<ShowResponseDto> CreateShowAsync(CreateShowDto showDto);
        Task<List<ShowResponseDto>> GetAllShowAsync();

        Task<List<ShowResponseDto>> GetShowsByEventAndCityAsync(Guid id, string city);

        Task<ShowResponseDto?> GetShowByIdAsync(Guid id);
        Task<bool> DeactivateShowAsync(Guid id);
    }
}
