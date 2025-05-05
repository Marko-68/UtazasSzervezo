using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UtazasSzervezo_Library.Models
{
    public class Accommodation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        public int number_of_rooms { get; set; }
        [Required]
        public int guests { get; set; }
        [Required]
        public string address { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        [Precision(10, 2)]
        public decimal price_per_night { get; set; }
        [Range(1,5)]
        public int? star_rating { get; set; }
        [Required]
        public int available_rooms { get; set; }
        //breakfast,half-board,all-inclusive
        //selectable 
        public string? dinning { get; set; }

        public string? cover_img { get; set; }
        public List<string>? images_url { get; set; }
        public ICollection<AccommodationAmenities>? AccommodationAmenities { get; set; }
    }
}
