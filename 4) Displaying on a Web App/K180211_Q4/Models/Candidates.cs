using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace K180211_Q4.Models
{
    public class Candidates
    {
        public string Name { get; set; }
        public string CandidateId { get; set; }
        public string Position { get; set; }

        public Candidates()
        {

        }
        public Candidates(string name, string candidateid, string position)
        {
            this.Name = name;
            this.CandidateId = candidateid;
            this.Position = position;
        }
    }
}