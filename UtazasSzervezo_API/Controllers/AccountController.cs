using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User { UserName = model.Email, Email = model.Email, Name = model.Name };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok("User registered successfully!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

            if (!result.Succeeded)
                return Unauthorized("Invalid email or password.");

            return Ok("Login successful!");
        }



        

        public class LoginModel
        {
            [Required, EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
        }
    }

}

