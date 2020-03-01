using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClubBaistSystem.Domain
{
    public class Scorecard
    {
        public Golfer Golfer { get; set; }
        public string Course { get; set; }
        public DateTime Date { get; set; }
        public List<Round> Rounds { get; set; }
    }
}