

using AutoMapper;
using EventServices.Application.DTO.Show;
using EventServices.Application.IRepository;
using EventServices.Application.IServices;
using EventServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EventServices.Application.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public ShowService(IShowRepository showRepository, IEventRepository eventRepository, IMapper mapper)
        {
            _mapper = mapper;
            _showRepository = showRepository;
            _eventRepository = eventRepository;
        }

        public async Task<ShowResponseDto> CreateShowAsync(CreateShowDto showDto)
        {
            if (showDto == null)
            {
                throw new ArgumentNullException(nameof(showDto));
            }

            // 1. Fetch event to get duration
            var eventObj = await _eventRepository.GetByIdAsync(showDto.EventId);
            if (eventObj == null)
            {
                throw new KeyNotFoundException($"Event with ID {showDto.EventId} was not found.");
            }

            // 2. Map CreateShowDto to Show entity
            var show = _mapper.Map<Show>(showDto);
            show.Id = Guid.NewGuid();

            // 3. Calculate EndTime dynamically
            show.EndTime = showDto.StartTime.AddMinutes(eventObj.DurationInMinutes);

            // 4. Overlap Validation Check
            var allShows = await _showRepository.GetAllAsync();
            var screenShows = allShows.Where(s => s.ScreenId == showDto.ScreenId && s.IsActive).ToList();
            
            bool hasOverlap = screenShows.Any(s =>
                showDto.StartTime < s.EndTime &&
                show.EndTime > s.StartTime);

            if (hasOverlap)
            {
                throw new ArgumentException("Show timing conflicts with an existing show on this screen.");
            }

            // 5. Persist to Database
            await _showRepository.AddAsync(show);
            await _showRepository.SaveChangesAsync();

            return _mapper.Map<ShowResponseDto>(show);
        }

        public async Task<bool> DeactivateShowAsync(Guid id)
        {
            if (id != Guid.Empty)
            {
                var existingShow =await _showRepository.GetByIdAsync(id);
                if (existingShow != null)
                {
                    existingShow.IsActive = false;

                    await _showRepository.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<List<ShowResponseDto>> GetAllShowAsync()
        {
            var shows=await _showRepository.GetAllAsync();
            return _mapper.Map<List<ShowResponseDto>>(shows);
        }

        public async Task<ShowResponseDto?> GetShowByIdAsync(Guid id)
        {
            if(id!= Guid.Empty)
            {
                var show=await _showRepository.GetByIdAsync(id);
                return _mapper.Map<ShowResponseDto>(show);
            }
            else
            {
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            }

        }

        public async Task<List<ShowResponseDto>> GetShowsByEventAndCityAsync(Guid id, string city)
        {
            var shows= await _showRepository.GetShowsByEventAndCityAsync(id, city);

            return _mapper.Map<List<ShowResponseDto>>(shows);
        }
    }
}
