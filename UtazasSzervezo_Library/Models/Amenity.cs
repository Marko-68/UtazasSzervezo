using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Amenity
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string name { get; set; }

        public ICollection<AccommodationAmenities>? AccommodationAmenities { get; set; }
    }
}
