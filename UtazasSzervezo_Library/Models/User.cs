using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class User : IdentityUser
    {
        public int User_id { get; set; }
        [Required]
        public string? Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Phone_number { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
