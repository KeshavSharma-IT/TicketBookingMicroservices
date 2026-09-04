using EventServices.Application.DTO.Venue;
using EventServices.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventServices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpPost]
        public async Task<ActionResult<VenueResponseDto>> CreateVenueAsync([FromBody] CreateVenueDto createVenueDto)
        {
            if (createVenueDto == null)
            {
                return BadRequest("Venue data cannot be null.");
            }

            var venue = await _venueService.CreateVenueAsync(createVenueDto);
            return CreatedAtAction(nameof(GetVenueById), new { id = venue.Id }, venue);
        }

        [HttpPost("{venueId:guid}/screens")]
        public async Task<ActionResult<ScreenResponseDto>> CreateScreenAsync(Guid venueId, [FromBody] CreateScreenDto createScreenDto)
        {
            if (createScreenDto == null)
            {
                return BadRequest("Screen data cannot be null.");
            }
            if (venueId == Guid.Empty)
            {
                return BadRequest("Venue ID cannot be empty.");
            }

            var screen = await _venueService.CreateScreenAsync(createScreenDto, venueId);
            return Ok(screen);
        }

        [HttpGet]
        public async Task<ActionResult<List<VenueResponseDto>>> GetAllVenuesAsync()
        {
            var venues = await _venueService.GetVenueAsync();
            return Ok(venues);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<VenueResponseDto>> GetVenueById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Venue ID cannot be empty.");
            }

            var venue = await _venueService.GetVenueByIdAsync(id);
            if (venue == null)
            {
                return NotFound($"Venue with ID {id} was not found.");
            }

            return Ok(venue);
        }

        [HttpGet("city/{city}")]
        public async Task<ActionResult<List<VenueResponseDto>>> GetVenuesByCityAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City name cannot be blank.");
            }

            var venues = await _venueService.GetVenueByCityAsync(city);
            return Ok(venues);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeactivateVenueAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Venue ID is invalid.");
            }

            var success = await _venueService.DeactivateVenueAsync(id);
            if (!success)
            {
                return NotFound($"Venue with ID {id} was not found.");
            }

            return NoContent();
        }
    }
}
