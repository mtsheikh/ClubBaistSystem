using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClubBaistSystem.Domain;
using ClubBaistSystem.TechnicalServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ClubBaistSystem.Pages
{
    [Authorize(Roles = "Shareholder,ShareholderSpouse")]
    [BindProperties]
    public class EditStandingTeeTimeRequest : PageModel
    {
        [Required]
        public string inputtedShareholder1 { get; set; }
        [Required]
        public string inputtedShareholder2 { get; set; }
        [Required]
        public string inputtedShareholder3 { get; set; }
        [Required]
        public string inputtedShareholder4 { get; set; }
        [Required]
        public DateTime inputtedStartDate { get; set; }
        [Required]
        public DateTime inputtedEndDate { get; set; }
        
        [TempData]
        public string Alert { get; set; }
        public List<StandingTeeTimeRequest> StandingTeeTimeRequests { get; set; }

        public StandingTeeTimeRequest SelectedStandingTeeTimeRequest;

        CBS _requestDirector = new CBS();
        
        public void OnGet()
        {
            string dayOfWeek = Request.Query["day"];
            var time = DateTime.Parse(Request.Query["time"]);
         
            StandingTeeTimeRequests = _requestDirector.FindStandingTeeTimeRequests(dayOfWeek);

            foreach (var standingTeeTimeRequest in StandingTeeTimeRequests)
            {
                if (standingTeeTimeRequest.DayOfWeek == dayOfWeek
                    && standingTeeTimeRequest.Time.ToString("hh:mm tt") == time.ToString("hh:mm tt"))
                {
                    inputtedShareholder1 = standingTeeTimeRequest.Shareholder1.FullName;
                    inputtedShareholder2 = standingTeeTimeRequest.Shareholder2.FullName;
                    inputtedShareholder3 = standingTeeTimeRequest.Shareholder3.FullName;
                    inputtedShareholder4 = standingTeeTimeRequest.Shareholder4.FullName;
                    inputtedStartDate = standingTeeTimeRequest.StartDate;
                    inputtedEndDate = standingTeeTimeRequest.EndDate;

                    SelectedStandingTeeTimeRequest = standingTeeTimeRequest;
                }
            }
        }
        public ActionResult OnPost()
        {
            var requestedDayOfWeek = Request.Query["day"];
            var requestedTime = DateTime.Parse(Request.Query["time"]);
            var requestedStartDate = inputtedStartDate;
            var requestedEndDate = inputtedEndDate;

            var clubBaistUserManager = new ClubBaistUsers();

            if (!ModelState.IsValid) return Page();
            var result = false;
            
            SelectedStandingTeeTimeRequest = new StandingTeeTimeRequest();
            SelectedStandingTeeTimeRequest.StartDate = requestedStartDate;
            SelectedStandingTeeTimeRequest.EndDate = requestedEndDate;
            SelectedStandingTeeTimeRequest.Time = requestedTime;
            SelectedStandingTeeTimeRequest.DayOfWeek = requestedDayOfWeek;

            SelectedStandingTeeTimeRequest.Shareholder1.FullName = inputtedShareholder1;
            SelectedStandingTeeTimeRequest.Shareholder2.FullName = inputtedShareholder2;
            SelectedStandingTeeTimeRequest.Shareholder3.FullName = inputtedShareholder3;
            SelectedStandingTeeTimeRequest.Shareholder4.FullName = inputtedShareholder4;

            result = _requestDirector.SubmitStandingTeeTimeRequest(SelectedStandingTeeTimeRequest);

            if (!result) return Page();

            Alert = $"Edited Standing Time for {requestedDayOfWeek} at {requestedTime:hh:mm tt}";
            return RedirectToPage("SubmitStandingTeeTimeRequest");
        }
    }
}