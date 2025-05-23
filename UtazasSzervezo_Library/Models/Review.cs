﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Review
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string user_id { get; set; }
        [JsonIgnore]
        public virtual User? User { get; set; }

        public int? accommodation_id { get; set; }

        [JsonIgnore]
        public Accommodation? Accommodation { get; set; }

        public int? flight_id { get; set; }

        [JsonIgnore]
        public Flight? Flight { get; set; }

        [Required]
        [Range(1, 10)]
        public int rating { get; set; }

        [Required]
        public string comment { get; set; }
        [Required]
        public DateTime created_at { get; set; }
    }
}
