using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public User User { get; set; }
        public int? Accommodation_id { get; set; }
        public Accommodation Accommodation { get; set; }
        public int? Flight_id { get; set; }
        public Flight Flight { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime Created_at { get; set; }
    }
}
