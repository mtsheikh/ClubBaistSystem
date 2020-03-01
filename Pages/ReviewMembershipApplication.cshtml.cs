using ClubBaistSystem.Domain;
using ClubBaistSystem.TechnicalServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubBaistSystem.Pages
{
    [BindProperties]
    public class ReviewMembershipApplication : PageModel
    {
        [TempData]
        public string Alert { get; set; }

        public MembershipApplication SelectedMembershipApplication { get; set; }
        
        private readonly CBS _requestDirector = new CBS();
        public void OnGet()
        {
            var membershipApplicationId = int.Parse(Request.Query["id"]);
            SelectedMembershipApplication = _requestDirector.ViewMembershipApplication(membershipApplicationId);
        }

        public ActionResult OnPost(string submit)
        {
            if (!ModelState.IsValid) return Page();
            var result = false;

            var membershipApplicationId = int.Parse(Request.Query["id"]);

            result = submit switch
            {
                "rejectApplication" => _requestDirector.RejectMembershipApplication(membershipApplicationId),
                "waitlistApplication" => _requestDirector.WaitlistMembershipApplication(membershipApplicationId),
                "approveApplication" => _requestDirector.ApproveMembershipApplication(_requestDirector.ViewMembershipApplication(membershipApplicationId)),
                _ => result
            };
            
            if (!result) return Page();

            Alert = submit switch
            {
                "rejectApplication" => $"Rejected Membership Application Successfully",
                "waitlistApplication" => $"Waitlisted Membership Application Successfully",
                "approveApplication" => $"Approved Membership Application Successfully",
                _ => Alert
            };

            return RedirectToPage("ViewMembershipApplications");
        }
    }
}