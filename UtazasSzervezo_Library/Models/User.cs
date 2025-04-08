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
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Review>? Reviews { get; set; }

        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public int? postalcode { get; set; }
        public int? phone_number { get; set; }
        public string? country { get; set; }

    }
}