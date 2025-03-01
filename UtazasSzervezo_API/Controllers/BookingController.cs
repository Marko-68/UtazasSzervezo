using Microsoft.AspNetCore.Mvc;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
