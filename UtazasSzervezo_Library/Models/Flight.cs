using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string Airline { get; set; }
        public DateTime Departure_time { get; set; }
        public DateTime Arrival_time { get; set; }
        public string Departure_airport { get; set; }
        public string Destination_airport { get; set; }
        public int Duration { get; set; }
        public int Available_seats { get; set; }
        public int Price { get; set; }
    }
}
