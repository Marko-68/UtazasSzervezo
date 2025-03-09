using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationAmenitiesController : ControllerBase
    {
        private readonly AccommodationAmenitiesService _service;

        public AccommodationAmenitiesController(AccommodationAmenitiesService service)
        {
            _service = service;
        }

        // GET ALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var amenities = await _service.GetAllAmenities();
            return Ok(amenities);
        }

        // GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var amenity = await _service.GetAmenityById(id);
            if (amenity == null)
                return NotFound();
            return Ok(amenity);
        }

        // CREATE
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AccommodationAmenities amenity)
        {
            if (amenity == null)
                return BadRequest("Amenity data is required.");

            var createdAmenity = await _service.CreateAmenity(amenity);
            return CreatedAtAction(nameof(GetById), new { id = createdAmenity.id }, createdAmenity);
        }

        // DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAmenity(id);
            if (!success)
                return NotFound();
            return NoContent();
        }
    }
}
