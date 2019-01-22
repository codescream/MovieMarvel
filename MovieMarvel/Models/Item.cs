using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMarvel.Models
{
    public class Item
    {
        public int id { get; set; }

        [Required]
        public string MovieID { get; set; }

        [Required]
        public string MovieTitle { get; set; }

        [Required]
        public string MoviePoster { get; set; }

        [Required]
        public int Duration { get; set; }

        [Required]
        public float Cost { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string Status { get; set; }

        public int CartID { get; set; }
        public Cart Cart { get; set; }
    }
}
