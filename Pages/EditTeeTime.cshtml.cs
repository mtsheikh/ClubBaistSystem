using System;
using System.Collections.Generic;
using ClubBaistSystem.Domain;
using ClubBaistSystem.TechnicalServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static System.String;

namespace ClubBaistSystem.Pages
{
    [BindProperties]
    public class EditTeeTime : PageModel
    {
        public string ReservedGolfer1Name { get; set; }
        public string ReservedGolfer2Name { get; set; }
        public string ReservedGolfer3Name { get; set; }
        public string ReservedGolfer4Name { get; set; }

        public bool ReservedGolfer1CheckedIn { get; set; }
        public bool ReservedGolfer2CheckedIn { get; set; }
        public bool ReservedGolfer3CheckedIn { get; set; }
        public bool ReservedGolfer4CheckedIn { get; set; }

        public ClubBaistUser AuthenticatedUser;
        
        [TempData]
        public string Alert { get; set; }

        public TeeTime SelectedTeeTime;
        private DateTime SelectedDate { get; set; }
        private DateTime SelectedTime { get; set; }

        private readonly CBS _requestDirector = new CBS();

        public ActionResult OnPost(string submit)
        {
            AuthenticatedUser = ClubBaistUsers.GetUserFromUserName(User.Identity.Name);
            SelectedDate = DateTime.Parse(Request.Query["date"]);
            SelectedTime = DateTime.Parse(Request.Query["time"]);
            
            if (!ModelState.IsValid) return Page();
            var result = false;
            
            SelectedTeeTime = _requestDirector.FindTeeTime(SelectedDate, SelectedTime);

            SelectedTeeTime.Golfer1.FullName = ReservedGolfer1Name;
            SelectedTeeTime.Golfer2.FullName = ReservedGolfer2Name;
            SelectedTeeTime.Golfer3.FullName = ReservedGolfer3Name;
            SelectedTeeTime.Golfer4.FullName = ReservedGolfer4Name;
            if (IsNullOrEmpty(SelectedTeeTime.BookerId) || SelectedTeeTime.BookerId == " "
                                                        || SelectedTeeTime.BookerId == "") SelectedTeeTime.BookerId = AuthenticatedUser.Id;
          
            var cancelledGolfers = new List<string>();

            switch (submit)
            {
                case "cancelForGolferOne":
                    cancelledGolfers.Add(ReservedGolfer1Name);
                    break;
                
                case "cancelForGolferTwo":
                    cancelledGolfers.Add(ReservedGolfer2Name);
                    break;
                
                case "cancelForGolferThree":
                    cancelledGolfers.Add(ReservedGolfer3Name);
                    break;
                
                case "cancelForGolferFour":
                    cancelledGolfers.Add(ReservedGolfer4Name);
                    break;
                case "cancelTeeTime":
                    if(!IsNullOrEmpty(ReservedGolfer1Name)) cancelledGolfers.Add(ReservedGolfer1Name);
                    if(!IsNullOrEmpty(ReservedGolfer2Name)) cancelledGolfers.Add(ReservedGolfer2Name);
                    if(!IsNullOrEmpty(ReservedGolfer3Name)) cancelledGolfers.Add(ReservedGolfer3Name);
                    if(!IsNullOrEmpty(ReservedGolfer4Name)) cancelledGolfers.Add(ReservedGolfer4Name);
                    SelectedTeeTime.BookerId = " ";
                    break;
                case "CheckInGolfers":
                    SelectedTeeTime.Golfer1CheckedIn = ReservedGolfer1CheckedIn;
                    SelectedTeeTime.Golfer2CheckedIn = ReservedGolfer2CheckedIn;
                    SelectedTeeTime.Golfer3CheckedIn = ReservedGolfer3CheckedIn;
                    SelectedTeeTime.Golfer4CheckedIn = ReservedGolfer4CheckedIn;
                    break;
            }
            
            result = submit switch
            {
                "cancelForGolferOne" => _requestDirector.CancelTeeTime(SelectedTeeTime, cancelledGolfers),
                "cancelForGolferTwo" => _requestDirector.CancelTeeTime(SelectedTeeTime, cancelledGolfers),
                "cancelForGolferThree" => _requestDirector.CancelTeeTime(SelectedTeeTime, cancelledGolfers),
                "cancelForGolferFour" => _requestDirector.CancelTeeTime(SelectedTeeTime, cancelledGolfers),
                "cancelTeeTime" => _requestDirector.CancelTeeTime(SelectedTeeTime, cancelledGolfers),
                "Add" => _requestDirector.ReserveTeeTime(SelectedTeeTime),
                "CheckInGolfers" => _requestDirector.CheckInGolfers(SelectedTeeTime),
                _ => result
            };
            
            if (!result) return Page();

            Alert = $"Modified TeeTime for {SelectedDate:dddd, dd MMMM yyyy} at {SelectedTime:hh:mm tt}";
            return RedirectToPage("ReserveTeeTime");
        }

        public void OnGet()
        {
            AuthenticatedUser = ClubBaistUsers.GetUserFromUserName(User.Identity.Name);
            SelectedDate = DateTime.Parse(Request.Query["date"]);
            SelectedTime = DateTime.Parse(Request.Query["time"]);

            SelectedTeeTime = _requestDirector.FindTeeTime(SelectedDate,SelectedTime);
            ReservedGolfer1Name = SelectedTeeTime.Golfer1.FullName;
            ReservedGolfer2Name = SelectedTeeTime.Golfer2.FullName;
            ReservedGolfer3Name = SelectedTeeTime.Golfer3.FullName;
            ReservedGolfer4Name = SelectedTeeTime.Golfer4.FullName;
            ReservedGolfer1CheckedIn = SelectedTeeTime.Golfer1CheckedIn;
            ReservedGolfer2CheckedIn = SelectedTeeTime.Golfer2CheckedIn;
            ReservedGolfer3CheckedIn = SelectedTeeTime.Golfer3CheckedIn;
            ReservedGolfer4CheckedIn = SelectedTeeTime.Golfer4CheckedIn;
        }
    }
}