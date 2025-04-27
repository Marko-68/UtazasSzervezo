// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using UtazasSzervezo_Library.Models;

namespace UtazasSzervezo_UI.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PersonalDataModel(
            UserManager<User> userManager,
            ILogger<PersonalDataModel> logger,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
        }

        public bool IsAdmin { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            //Ha admin vagy akkor nem tudod kitörölni az accountodat
            IsAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (IsAdmin)
            {
                ModelState.AddModelError(string.Empty, "You cannot delete your account as an admin.");
                return Page();
            }

            return Page();
        }
    }
}
