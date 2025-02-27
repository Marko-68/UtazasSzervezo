using Microsoft.AspNetCore.Mvc;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccommodationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
