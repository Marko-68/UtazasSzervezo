using Microsoft.AspNetCore.Mvc;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly FlightService _flightService;
        public FlightController(FlightService flightService)
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

        [HttpPost]
        public async Task<IActionResult> Create(Flight flight)
        {
            await _flightService.CreateFlight(flight);
            return Ok();
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
