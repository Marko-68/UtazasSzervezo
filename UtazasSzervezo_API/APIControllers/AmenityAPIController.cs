using Microsoft.AspNetCore.Mvc;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/Amenity")]
    [ApiController]
    public class AmenityAPIController : ControllerBase
    {
        private readonly AmenityService _amenityService;
        public AmenityAPIController(AmenityService amenityService)
        {
            _amenityService = amenityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var amenities = await _amenityService.GetAllAmenities();
            return Ok(amenities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var amenity = await _amenityService.GetAmenityById(id);
            if (amenity == null)
                return NotFound();
            return Ok(amenity);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Amenity amenities)
        {
            await _amenityService.CreateAmenity(amenities);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Amenity amenity)
        {
            var success = await _amenityService.UpdateAmenity(id, amenity);
            if (!success)
                return NotFound(new { message = "Amenity not found" });

            return Ok(amenity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _amenityService.DeleteAmenity(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
