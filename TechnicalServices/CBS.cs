using System;
using System.Collections.Generic;
using ClubBaistSystem.Domain;

namespace ClubBaistSystem.TechnicalServices
{
    public class CBS
    {
        private readonly DailyTeeSheets _dailyTeeSheetsManager = new DailyTeeSheets();
        private readonly TeeTimes _teeTimesManager = new TeeTimes(); 
        private readonly StandingTeeTimeRequests _standingTeeTimeRequestsManager = new StandingTeeTimeRequests();
        private readonly MembershipApplications _membershipApplicationManager = new MembershipApplications();
        private readonly MemberAccounts _memberAccountManager = new MemberAccounts();

        // Manager functions for TeeTimes
        public bool CheckInGolfers(TeeTime bookedTeeTime)
        {
            return _teeTimesManager.CheckInGolfersForTeeTime(bookedTeeTime);
        }
        public DailyTeeSheet FindDailyTeeSheet(DateTime dailyTeeSheetDate, ClubBaistUser authenticatedUser)
        {
            return _dailyTeeSheetsManager.GetDailyTeeSheet(dailyTeeSheetDate, authenticatedUser);
        }
        public TeeTime FindTeeTime(DateTime selectedDate, DateTime selectedTime)
        {
            return _teeTimesManager.GetTeeTime(selectedTime, selectedDate);
        }
        public bool ReserveTeeTime(TeeTime selectedTeeTime)
        {
            return _teeTimesManager.AddTeeTime(selectedTeeTime);
        }
        public bool CancelTeeTime(TeeTime bookedTeeTime, List<string> cancelledGolfers)
        {
            return _teeTimesManager.RemoveTeeTime(bookedTeeTime, cancelledGolfers);
        }

        // Manager functions for Standing TeeTime Requests
        public List<StandingTeeTimeRequest
        > FindStandingTeeTimeRequests(string dayOfWeek)
        {
            return _standingTeeTimeRequestsManager.GetStandingTeeTimeRequests(dayOfWeek);
        }
        public bool SubmitStandingTeeTimeRequest(StandingTeeTimeRequest selectedStandingTeeTimeRequest)
        {
            return _standingTeeTimeRequestsManager.AddStandingTeeTimeRequest(selectedStandingTeeTimeRequest);
        }
        
        // Manager functions for Membership Applications
        public bool RecordMembershipApplication(MembershipApplication newMembershipApplication)
        {
            return _membershipApplicationManager.AddMembershipApplication(newMembershipApplication);
        }
        public List<MembershipApplication> FindOnHoldMembershipApplications()
        {
            return _membershipApplicationManager.GetOnHoldMembershipApplications();
        }
        public List<MembershipApplication> FindWaitlistedMembershipApplications()
        {
            return _membershipApplicationManager.GetWaitlistedMembershipApplications();
        }
        public MembershipApplication FindMembershipApplication(int membershipApplicationId)
        {
            return _membershipApplicationManager.GetMembershipApplication(membershipApplicationId);
        }
        public bool RejectMembershipApplication(int membershipApplicationId)
        {
            return _membershipApplicationManager.CancelMembershipApplication(membershipApplicationId);
        }
        public bool WaitlistMembershipApplication(int membershipApplicationId)
        {
            return _membershipApplicationManager.WaitlistMembershipApplication(membershipApplicationId);
        }

        // Manager functions for Member Accounts 
        public MemberAccount ViewMemberAccount(string memberId)
        {
            return _memberAccountManager.FindMemberAccount(memberId);
        }
    }
}