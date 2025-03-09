using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Booking
    {
        [Key]
        public int id { get; set; }
        public int? accommodation_id { get; set; }
        public Accommodation? Accommodation { get; set; }
        public int? flight_id { get; set; }
        public Flight? Flight { get; set; }
        [Required]
        public DateTime start_date { get; set; }
        [Required]
        public DateTime end_date { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string status { get; set; }
        public string? special_request { get; set; }
        [Required]
        public decimal total_price { get; set; }
    }
}
