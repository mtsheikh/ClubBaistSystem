using System.Collections.Generic;
using ClubBaistSystem.Domain;
using ClubBaistSystem.TechnicalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubBaistSystem.Pages
{
    [Authorize(Roles = "Shareholder,ShareholderSpouse")]
    public class SubmitStandingTeeTimeRequest : PageModel
    {
        readonly List<string> Weekdays = new List<string>()
        {
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday",
            "Sunday"
        };
        [BindProperty]
        public List<string> AvailableWeekDays { get; set; }
        public List<StandingTeeTimeRequest> RequestedStandingTeeTimeRequests;
        [TempData]
        public string Alert { get; set; }

        CBS _requestDirector = new CBS();
        public void OnGet()
        {
            AvailableWeekDays = Weekdays;
        }
        public void OnPost()
        {
            AvailableWeekDays = Weekdays;
            string day = Request.Query["day"];
            RequestedStandingTeeTimeRequests = _requestDirector.FindStandingTeeTimeRequests(day);
        }
        
    }
}