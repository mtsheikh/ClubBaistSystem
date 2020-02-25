using System;

namespace ClubBaistSystem.Domain
{
    public class TeeTime
    {
        public DateTime Date { get; set; }
        public DateTime Time { get; set; }

        public ClubBaistUser Golfer1 { get; set; }
        public ClubBaistUser Golfer2 { get; set; }
        public ClubBaistUser Golfer3 { get; set; }
        public ClubBaistUser Golfer4 { get; set; }
        public string BookerId { get; set; }

        public bool Golfer1CheckedIn { get; set; }
        public bool Golfer2CheckedIn { get; set; }
        public bool Golfer3CheckedIn { get; set; }
        public bool Golfer4CheckedIn { get; set; }
    }
}