using System;

namespace ClubBaistSystem.Domain
{
    public class MemberAccountEntry
    {
        public decimal Amount { get; set; }
        public DateTime WhenCharged { get; set; }
        public DateTime WhenMade { get; set; }
        public string EntryDecription { get; set; }
    }
}