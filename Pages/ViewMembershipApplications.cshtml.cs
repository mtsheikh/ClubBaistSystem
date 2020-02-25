using System.Collections.Generic;
using ClubBaistSystem.Domain;
using ClubBaistSystem.TechnicalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubBaistSystem.Pages
{
    [BindProperties]
    public class ViewMembershipApplications : PageModel
    {
        [TempData]
        public string Alert { get; set; }
        private readonly CBS _requestDirector = new CBS();
        public List<MembershipApplication> OnHoldMembershipApplications { get; private set; }
        public List<MembershipApplication> WaitlistedMembershipApplications { get; private set; }
        
        public void OnGet()
        {
            OnHoldMembershipApplications = _requestDirector.FindOnHoldMembershipApplications();
            WaitlistedMembershipApplications = _requestDirector.FindWaitlistedMembershipApplications();
        }
        
        
    }
}