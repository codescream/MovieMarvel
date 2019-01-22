using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieMarvel.Models
{
    public class User
    {
        public int id { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        [MaxLength(100)]
        public string password { get; set; }

        [Required]
        [StringLength(1, MinimumLength = 1)]
        public string status { get; set; }

        public ICollection<Cart> Carts { get; set; }

        public ICollection<Item> Items { get; set; }
    }
}
