using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Accommodation
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Type { get; set; }
        [Required]
        public int Number_of_rooms { get; set; }
        [Required]
        public int Max_person { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public decimal Price_per_night { get; set; }
        public int? Star_rating { get; set; }
        [Required]
        public int Available_rooms { get; set; }
    }
}
