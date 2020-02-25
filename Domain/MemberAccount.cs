using System.Collections.Generic;
namespace ClubBaistSystem.Domain
{
    public class MemberAccount
    {
        public string MemberId { get; set; }
        public List<MemberAccountEntry> AccountEntries { get; set; }
        public decimal TotalBalance { get; set; }   
    }
}