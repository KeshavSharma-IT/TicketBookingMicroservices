using EventServices.Application.DTO.Event;
using EventServices.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventServices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<List<EventResponseDto>>> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EventResponseDto>> GetEventById(Guid id)
        {
            var eventDto = await _eventService.GetEventByIdAsync(id);
            if (eventDto == null)
            {
                return NotFound($"Event with ID {id} was not found.");
            }
            return Ok(eventDto);
        }

        [HttpPost]
        public async Task<ActionResult<EventResponseDto>> CreateEvent([FromBody] CreateEventDto createEventDto)
        {
            if (createEventDto == null)
            {
                return BadRequest("Event data cannot be null.");
            }

            var createdEvent = await _eventService.CreateEventAsync(createEventDto);

            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeactivateEvent(Guid id)
        {
            var success = await _eventService.DeactivateEventAsync(id);
            if (!success)
            {
                return NotFound($"Event with ID {id} was not found.");
            }
            return NoContent();
        }
    }
}
