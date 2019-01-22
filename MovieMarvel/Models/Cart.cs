using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMarvel.Models
{
    public class Cart
    {
        public int id { get; set; }

        [Required]
        public DateTime Opened { get; set; }

        [Required]
        public DateTime Closed { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string Status { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

    }
}
