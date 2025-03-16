using Microsoft.AspNetCore.Mvc;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;
        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
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
            var success = await _bookingService.DeleteBooking(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
