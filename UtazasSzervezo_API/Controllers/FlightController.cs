using Microsoft.AspNetCore.Mvc;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly FlighService _flightService;
        public FlightController(FlighService flightService)
        {
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var flights = await _flightService.GetAllFlights();
            return Ok(flights);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var flights = await _flightService.GetFlightById(id);
            if (flights == null)
                return NotFound();
            return Ok(flights);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _flightService.DeleteFlight(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
