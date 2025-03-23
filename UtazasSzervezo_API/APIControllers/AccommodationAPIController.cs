using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationAPIController : ControllerBase
    {
        private readonly AccommodationService _accommodationService;
        public AccommodationAPIController(AccommodationService accommodationService)
        {
            _accommodationService = accommodationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var accommodations = await _accommodationService.GetAllAccommodations();
            return Ok(accommodations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var accommodation = await _accommodationService.GetAccommodationById(id);
            if (accommodation == null)
                return NotFound();
            return Ok(accommodation);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Accommodation accommodation)
        {
            if (accommodation == null)
                return BadRequest(new { message = "Invalid accommodation data" });

            var createdAccommodation = await _accommodationService.CreateAccommodation(accommodation);
            return CreatedAtAction(nameof(GetById), new { id = createdAccommodation.id }, createdAccommodation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Accommodation accommodation)
        {
            var success = await _accommodationService.UpdateAccommodation(id, accommodation);
            if (!success)
                return NotFound(new { message = "Accommodation not found" });

            return Ok(accommodation);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _accommodationService.DeleteAccommodation(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
