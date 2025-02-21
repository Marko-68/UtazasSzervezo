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
        public int ID { get; set; }
        public int? Accommodation_id { get; set; }
        public Accommodation? Accommodation { get; set; }
        public int? Flight_id { get; set; }
        public Flight? Flight { get; set; }
        [Required]
        public DateTime Start_date { get; set; }
        [Required]
        public DateTime End_date { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }
        public string? Special_request { get; set; }
        [Required]
        public decimal total_price { get; set; }
    }
}
