using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Flight
    {
        public int Id { get; set; }
        [Required]
        public string Airline { get; set; }
        [Required]
        public DateTime Departure_time { get; set; }
        [Required]
        public DateTime Arrival_time { get; set; }
        [Required]
        public string Departure_airport { get; set; }
        [Required]
        public string Destination_airport { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int Available_seats { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
