using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Booking
    {
        [Key]
        public int id { get; set; }
        public int? accommodation_id { get; set; }
        [JsonIgnore]
        public Accommodation? Accommodation { get; set; }
        public int? flight_id { get; set; }
        [JsonIgnore]
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
        [Precision(10, 2)]
        public decimal total_price { get; set; }

        public string user_id { get; set; }

        [JsonIgnore]
        public User User { get; set; }

        //TODO: képek
    }
}
