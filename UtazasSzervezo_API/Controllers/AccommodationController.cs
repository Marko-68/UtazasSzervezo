using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var accommodation = await _accommodationService.GetAccommodationById(id);
            if (accommodation == null)
                return NotFound();
            return Ok(accommodation);
        }

    }
}
