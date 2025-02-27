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
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<AccommodationAmenities> AccommodationAmenities { get; set; }
    }
}
