using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieMarvel.Models
{
    public class Vote
    {
        public int VoteID { get; set; }
        public string MovieID { get; set; }
        public int VoteRating { get; set; }
    }
}
