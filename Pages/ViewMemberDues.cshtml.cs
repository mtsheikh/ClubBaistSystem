using ClubBaistSystem.Domain;
using ClubBaistSystem.TechnicalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubBaistSystem.Pages
{
    [Authorize]
    [BindProperties]
    public class ViewMemberDues : PageModel
    {
        public MemberAccount AuthenticatedMemberAccount { get; set; }
        public ClubBaistUser AuthenticatedUser;
        private readonly CBS _requestDirector = new CBS();

        public void OnGet()
        {
            
            AuthenticatedUser = ClubBaistUsers.GetUserFromUserName(User.Identity.Name);
            AuthenticatedMemberAccount = _requestDirector.ViewMemberAccount(AuthenticatedUser.Id);
        }
    }
}