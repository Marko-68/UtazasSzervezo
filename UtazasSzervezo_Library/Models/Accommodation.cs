using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Accommodation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int Number_of_rooms { get; set; }
        public int Max_person { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Price_per_night { get; set; }
        public int? Star_rating { get; set; }
        public int Available_rooms { get; set; }
    }
}
