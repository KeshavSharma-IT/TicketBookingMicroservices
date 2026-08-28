using EventServices.Application.DTO.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application.IServices
{
    public interface IEventService
    {
        Task<EventResponseDto> CreateEventAsync(CreateEventDto eventDto);
        Task<EventResponseDto> GetEventByIdAsync(Guid Id);
        Task<List<EventResponseDto>> GetAllEventsAsync();
        Task<bool> DeactivateEventAsync(Guid Id);
    }
}
