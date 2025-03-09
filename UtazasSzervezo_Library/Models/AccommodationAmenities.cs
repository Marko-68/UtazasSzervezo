using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class AccommodationAmenities
    {
        [Key]
        public int id { get; set; }
        public int? accommodation_id { get; set; }
        public Accommodation? Accommodation { get; set; }
        public int? amentry_id { get; set; }
        public Amenity? Amenity { get; set; }
    }
}
