using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ClubBaistSystem.Domain;
using ClubBaistSystem.TechnicalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.String;

namespace ClubBaistSystem.Pages
{
    [Authorize(Roles = "Shareholder,ShareholderSpouse")]
    [BindProperties]
    public class EditStandingTeeTimeRequest : PageModel
    {
        private readonly CBS _requestDirector = new CBS();

        private StandingTeeTimeRequest _selectedStandingTeeTimeRequest;

        [Required] public string InputtedShareholder1 { get; set; }

        [Required] public string InputtedShareholder2 { get; set; }

        [Required] public string InputtedShareholder3 { get; set; }

        [Required] public string InputtedShareholder4 { get; set; }

        [Required] public DateTime InputtedStartDate { get; set; }

        [Required] public DateTime InputtedEndDate { get; set; }

        [TempData] public string Alert { get; set; }

        public List<StandingTeeTimeRequest> StandingTeeTimeRequests { get; set; }

        public void OnGet()
        {
            string dayOfWeek = Request.Query["day"];
            var time = DateTime.Parse(Request.Query["time"]);

            StandingTeeTimeRequests = _requestDirector.FindStandingTeeTimeRequests(dayOfWeek);

            foreach (var standingTeeTimeRequest in StandingTeeTimeRequests.Where(standingTeeTimeRequest =>
                standingTeeTimeRequest.DayOfWeek == dayOfWeek
                && standingTeeTimeRequest.Time.ToString("hh:mm tt") == time.ToString("hh:mm tt")))
            {
                InputtedShareholder1 = standingTeeTimeRequest.Shareholder1.FullName;
                InputtedShareholder2 = standingTeeTimeRequest.Shareholder2.FullName;
                InputtedShareholder3 = standingTeeTimeRequest.Shareholder3.FullName;
                InputtedShareholder4 = standingTeeTimeRequest.Shareholder4.FullName;
                InputtedStartDate = standingTeeTimeRequest.StartDate;
                InputtedEndDate = standingTeeTimeRequest.EndDate;

                _selectedStandingTeeTimeRequest = standingTeeTimeRequest;
            }
        }

        public ActionResult OnPost(string submit)
        {
            var authenticatedUser = ClubBaistUsers.GetUserFromUserName(User.Identity.Name);
            var requestedDayOfWeek = Request.Query["day"];
            var requestedTime = DateTime.Parse(Request.Query["time"]);
            var requestedStartDate = InputtedStartDate;
            var requestedEndDate = InputtedEndDate;

            if (!ModelState.IsValid) return Page();
            var result = false;

            _selectedStandingTeeTimeRequest = new StandingTeeTimeRequest();
            _selectedStandingTeeTimeRequest.StartDate = requestedStartDate;
            _selectedStandingTeeTimeRequest.EndDate = requestedEndDate;
            _selectedStandingTeeTimeRequest.Time = requestedTime;
            _selectedStandingTeeTimeRequest.DayOfWeek = requestedDayOfWeek;

            _selectedStandingTeeTimeRequest.Shareholder1.FullName = InputtedShareholder1;
            _selectedStandingTeeTimeRequest.Shareholder2.FullName = InputtedShareholder2;
            _selectedStandingTeeTimeRequest.Shareholder3.FullName = InputtedShareholder3;
            _selectedStandingTeeTimeRequest.Shareholder4.FullName = InputtedShareholder4;
            
            if (IsNullOrEmpty(_selectedStandingTeeTimeRequest.BookerId) || _selectedStandingTeeTimeRequest.BookerId == " "
                                                        || _selectedStandingTeeTimeRequest.BookerId == "") _selectedStandingTeeTimeRequest.BookerId = authenticatedUser.Id;

            if (submit == "cancelStandingTeeTimeRequest") _selectedStandingTeeTimeRequest.BookerId = " ";
            
            result = submit switch
            {
                "submitStandingTeeTimeRequest" => _requestDirector.SubmitStandingTeeTimeRequest(_selectedStandingTeeTimeRequest),
                "cancelStandingTeeTimeRequest" => _requestDirector.CancelStandingTeeTimeRequest(_selectedStandingTeeTimeRequest),
                _ => result
            };

            if (!result) return Page();

            Alert = $"Edited Standing Time for {requestedDayOfWeek} at {requestedTime:hh:mm tt}";
            return RedirectToPage("SubmitStandingTeeTimeRequest");
        }
    }
}