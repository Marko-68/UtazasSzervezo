using Microsoft.AspNetCore.Mvc;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/Booking")]
    [ApiController]
    public class BookingAPIController : ControllerBase
    {
        private readonly BookingService _bookingService;
        private readonly FlightService _flightService;
        public BookingAPIController(BookingService bookingService, FlightService flightService)
        {
            _bookingService = bookingService;
            _flightService = flightService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bookings = await _bookingService.GetAllBookings();
            return Ok(bookings);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bookings = await _bookingService.GetBookingById(id);
            if (bookings == null)
                return NotFound();
            return Ok(bookings);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Booking booking)
        {
            await _bookingService.CreateBooking(booking);
            return Ok();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Booking booking)
        {
            var success = await _bookingService.UpdateBooking(id, booking);
            if (!success)
                return NotFound(new { message = "Booking not found" });

            return Ok(booking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var bookingToDelete = await _bookingService.GetBookingById(id);
            if (bookingToDelete == null)
            {
                return NotFound();
            }

            //Növeljük a szabad helyeket
            if (bookingToDelete.flight_id.HasValue)
            {
                var flight = await _flightService.GetFlightById(bookingToDelete.flight_id.Value);
                if (flight != null)
                {
                    await _flightService.FlightSeatIncrement(flight.id);
                }
            }

            var success = await _bookingService.DeleteBooking(id);
            if (!success)
            {
                return StatusCode(500, "Failed to delete booking.");
            }

            return NoContent();
        }

        [HttpGet("CheckAvailability")]
        public async Task<IActionResult> CheckAccommodationAvailability([FromQuery] int accommodationId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                bool available = await _bookingService.CheckAccommodationAvailability(accommodationId, startDate, endDate);
                if (available)
                    return Ok(new { available = true});
                else
                    return Ok(new { available = false, message = "No rooms available at this time" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
