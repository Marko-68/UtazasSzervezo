using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class AccommodationAmenities
    {
        public int Accommodation_id { get; set; }
        public Accommodation Accommodation { get; set; }
        public int Amentry_id { get; set; }
        public Amenity Amenity { get; set; }
    }
}
