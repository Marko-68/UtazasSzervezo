using Microsoft.AspNetCore.Mvc;
using UtazasSzervezo_Library.Models;
using UtazasSzervezo_Library.Services;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/Review")]
    [ApiController]
    public class ReviewAPIController : ControllerBase
    {
        private readonly ReviewService _reviewService;
        public ReviewAPIController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reviews = await _reviewService.GetAllReviews();
            return Ok(reviews);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reviews = await _reviewService.GetReviewById(id);
            if (reviews == null)
                return NotFound();
            return Ok(reviews);
        }

        //ez nem kell az WPF-hez
        [HttpGet("byaccommodation/{accommodationId}")]
        public async Task<IActionResult> GetByAccommodationId(int accommodationId)
        {
            var reviews = await _reviewService.GetReviewsByAccommodationId(accommodationId);
            return Ok(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Review review)
        {
            await _reviewService.CreateReview(review);
            return Ok();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _reviewService.DeleteReview(id);
            if (!success)
                return NotFound();

            return NoContent();
        }

    }
}
