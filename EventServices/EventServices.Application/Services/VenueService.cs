using AutoMapper;
using EventServices.Application.DTO.Venue;
using EventServices.Application.IRepository;
using EventServices.Application.IServices;
using EventServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventServices.Application.Services
{
    public class VenueService : IVenueService
    {
        private readonly IVenueRepository _repository;        
        private readonly IMapper _mapper;

        public VenueService(IVenueRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ScreenResponseDto> CreateScreenAsync(CreateScreenDto createScreen, Guid venueId)
        {
            if (createScreen == null)
            {
                throw new ArgumentNullException(nameof(createScreen), "Screen cannot be null");
            }
            if (venueId == Guid.Empty)
            {
                throw new ArgumentException("VenueId cannot be empty", nameof(venueId));
            }

            var venue = await _repository.GetByIdAsync(venueId);
            if (venue == null)
            {
                throw new KeyNotFoundException($"Venue with ID {venueId} was not found.");
            }

            var screen = _mapper.Map<Screen>(createScreen);
            screen.Id = Guid.NewGuid();
            screen.VenueId = venueId;

            venue.Screens.Add(screen);

            await _repository.SaveChangesAsync();

            return _mapper.Map<ScreenResponseDto>(screen);
        }

        public async Task<VenueResponseDto> CreateVenueAsync(CreateVenueDto createVenueDto)
        {
            if(createVenueDto == null)
            {
                throw new ArgumentNullException(nameof(createVenueDto), "venus details cannot be blank");
            }

            var venue = _mapper.Map<Venue>(createVenueDto);
            venue.Id = Guid.NewGuid();
            await _repository.AddAsync(venue);
            await _repository.SaveChangesAsync();

            return _mapper.Map<VenueResponseDto>(venue);
        }

        public async Task<bool> DeactivateVenueAsync(Guid Id)
        {
            if (Id == Guid.Empty)
            {
                throw new ArgumentException("ID cannot be empty", nameof(Id));
            }

            var venue = await _repository.GetByIdAsync(Id);
            if (venue == null)
            {
                return false;
            }

            venue.IsActive = false;
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<List<VenueResponseDto>> GetVenueAsync()
        {
            var venues= await _repository.GetAllAsync();
            return _mapper.Map<List<VenueResponseDto>>(venues);
        }

        public async Task<List<VenueResponseDto>> GetVenueByCityAsync(string city)
        {
            if(city == null)
            {
                throw new ArgumentNullException(nameof(city),"city name cannot be null");
            }

            var venues=await _repository.GetByCityAsync(city);

            return _mapper.Map<List<VenueResponseDto>>(venues);
        }

        public async Task<VenueResponseDto> GetVenueByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id), "Id cannot be null");
            }

            var venue = await _repository.GetByIdAsync(id);

            return _mapper.Map<VenueResponseDto>(venue);
        }
    }
}
