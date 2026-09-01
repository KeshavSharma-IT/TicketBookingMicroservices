using AutoMapper;
using EventServices.Application.DTO.Event;
using EventServices.Application.IRepository;
using EventServices.Application.IServices;
using EventServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application.Services
{
    public class EventService : IEventService
    {

        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
        }

        public async Task<EventResponseDto> CreateEventAsync(CreateEventDto eventDto)
        {
            if (eventDto == null)
            {
                throw new ArgumentNullException(nameof(eventDto));
            }

            // 1. Map DTO to Event entity
            var eventEntity = _mapper.Map<Event>(eventDto);

            // 2. Generate a new Guid and assign it to the entity
            eventEntity.Id = Guid.NewGuid();

            // 3. Add to repository
            await _eventRepository.AddAsync(eventEntity);

            // 4. Save changes to DB
            await _eventRepository.SaveAsync();

            // 5. Map the saved entity back to EventResponseDto
            return _mapper.Map<EventResponseDto>(eventEntity);
        }

        public async Task<bool> DeactivateEventAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty.", nameof(Id));
            }

            var existingEvent = await _eventRepository.GetByIdAsync(Id);
            if (existingEvent != null)
            {
                existingEvent.IsActive = false;
                await _eventRepository.SaveAsync();
                return true;
            }

            return false;
        }

        public async Task<List<EventResponseDto>> GetAllEventsAsync()
        {
            var events = await _eventRepository.GetAllAsync();
            
            // Map List<Event> to List<EventResponseDto> using AutoMapper
            return _mapper.Map<List<EventResponseDto>>(events);
        }

        public async Task<EventResponseDto?> GetEventByIdAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("Id cannot be empty.", nameof(Id));
            }

            var eventEntity = await _eventRepository.GetByIdAsync(Id);
            
            // Map Event to EventResponseDto (returns null if eventEntity is null)
            return _mapper.Map<EventResponseDto>(eventEntity);
        }
    }
}
