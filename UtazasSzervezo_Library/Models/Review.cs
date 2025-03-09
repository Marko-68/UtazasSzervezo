using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Review
    {
        [Key]
        public int id { get; set; }
        public int user_id { get; set; }
        public User User { get; set; }
        public int? accommodation_id { get; set; }
        public Accommodation Accommodation { get; set; }
        public int? flight_id { get; set; }
        public Flight Flight { get; set; }
        //Rating 1-10
        [Required]
        [Range(1, 10)]
        public int rating { get; set; }
        public string comment { get; set; }
        public DateTime created_at { get; set; }
    }
}
