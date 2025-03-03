using Microsoft.AspNetCore.Mvc;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationController : ControllerBase
    {
        private readonly AccommodationService _accommodationService;
        public AccommodationController(AccommodationService accommodationService)
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
        public async Task<IActionResult> Create(Accommodation accommodation)
        {
            await _accommodationService.CreateAccommodation(accommodation);
            return Ok();
        }


       /* [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Accommodation accommodation)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _accommodationService.UpdateAccommodation(id, accommodation);
            if (!success)
                return NotFound();

            return NoContent();
        }*/

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
