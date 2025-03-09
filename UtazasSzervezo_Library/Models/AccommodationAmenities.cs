using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class AccommodationAmenities
    {
        [Key]
        public int id { get; set; }
        [Required]
        public int? accommodation_id { get; set; }

        [JsonIgnore]
        public Accommodation? Accommodation { get; set; }
        [Required]
        public int? amenity_id { get; set; }

        [JsonIgnore]
        public Amenity? Amenity { get; set; }
    }
}
