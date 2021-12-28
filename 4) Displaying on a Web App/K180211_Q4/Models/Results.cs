using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace K180211_Q4.Models
{
    public class Results
    {
        public string Name { get; set; }

        public int VotersCount { get; set; }
        public Results()
        {

        }
        public Results(string name, int voterscount)
        {
            this.Name = name;

            this.VotersCount = voterscount;
        }
    }
}