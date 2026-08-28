using AutoMapper;
using EventServices.Application.DTO.Event;
using EventServices.Application.DTO.Show;
using EventServices.Application.DTO.Venue;
using EventServices.Domain.Entities;

namespace EventServices.Application.Mappings
{
    public class EventMappingProfile : Profile
    {
        public EventMappingProfile()
        {
            // Map incoming CreateEventDto to our Event Domain Entity
            CreateMap<CreateEventDto, Event>();

            // Map our Event Domain Entity to outgoing EventResponseDto
            CreateMap<Event, EventResponseDto>();

            //map CreateShowDto to  Entity show
            CreateMap<CreateShowDto, Show>();

            //map Entity show to responce show Dto
             CreateMap<Show, ShowResponseDto>();

            CreateMap<CreateScreenDto, Screen>();
            CreateMap<Screen, ScreenResponseDto>();
            CreateMap<CreateVenueDto,Venue>();  
            CreateMap<Venue, VenueResponseDto>();
        }
    }
}
