using Microsoft.EntityFrameworkCore;
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
        [Key]
        public int id { get; set; }
        [Required]
        public string airline { get; set; }
        [Required]
        public DateTime departure_time { get; set; }
        [Required]
        public DateTime arrival_time { get; set; }
        [Required]
        [StringLength(3)]
        public string departure_airport { get; set; }
        [Required]
        [StringLength(3)]
        public string destination_airport { get; set; }
        [Required]
        public int duration { get; set; }
        [Required]
        public int available_seats { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal price { get; set; }
    }
}
