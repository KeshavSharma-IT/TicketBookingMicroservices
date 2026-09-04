using EventServices.Application.DTO.Show;
using EventServices.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventServices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;
        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        [HttpPost]
        public async Task<ActionResult<ShowResponseDto>> CreateShowAsync([FromBody] CreateShowDto createShow)
        {
            if (createShow == null)
            {
                return BadRequest("Form data is null");
            }

            var res = await _showService.CreateShowAsync(createShow);
            return CreatedAtAction(nameof(GetShowById), new { id = res.Id }, res);
        }

        [HttpGet]
        public async Task<ActionResult<List<ShowResponseDto>>> GetAllShowAsync()
        {
            var res = await _showService.GetAllShowAsync();
            return Ok(res);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ShowResponseDto>> GetShowById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id cannot be empty");
            }

            var res = await _showService.GetShowByIdAsync(id);
            if (res == null)
            {
                return NotFound($"Show with ID {id} was not found.");
            }

            return Ok(res);
        }

        [HttpGet("event/{id:guid}/city/{city}")]
        public async Task<ActionResult<List<ShowResponseDto>>> GetShowsByEventAndCityAsync(Guid id, string city)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id cannot be empty");
            }
            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest("City name cannot be blank");
            }

            var res = await _showService.GetShowsByEventAndCityAsync(id, city);
            return Ok(res);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Id is invalid");
            }

            var success = await _showService.DeactivateShowAsync(id);
            if (!success)
            {
                return NotFound($"Show with ID {id} was not found.");
            }

            return NoContent();
        }
    }
}
