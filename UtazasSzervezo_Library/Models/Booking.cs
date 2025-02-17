using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Booking
    {
        public int ID { get; set; }
        public int Accommodation_id { get; set; }
        public Accommodation Accommodation { get; set; }
        public int Flight_id { get; set; }
        public Flight Flight { get; set; }
        public DateTime Start_date { get; set; }
        public DateTime End_date { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string? Special_request { get; set; }
    }
}
