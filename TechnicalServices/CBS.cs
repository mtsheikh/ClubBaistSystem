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
        private readonly PlayerScores _playerScoreManager = new PlayerScores();

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
        public List<StandingTeeTimeRequest> FindStandingTeeTimeRequests(string dayOfWeek)
        {
            return _standingTeeTimeRequestsManager.GetStandingTeeTimeRequests(dayOfWeek);
        }
        public StandingTeeTimeRequest FindStandingTeeTimeRequest(string dayOfWeek, DateTime startDate, 
                                                                 DateTime endDate, DateTime time)
        {
            return _standingTeeTimeRequestsManager.GetStandingTeeTimeRequest(dayOfWeek, startDate, endDate, time);
        }
        public bool SubmitStandingTeeTimeRequest(StandingTeeTimeRequest selectedStandingTeeTimeRequest)
        {
            return _standingTeeTimeRequestsManager.AddStandingTeeTimeRequest(selectedStandingTeeTimeRequest);
        }
        public bool CancelStandingTeeTimeRequest(StandingTeeTimeRequest selectedStandingTeeTimeRequest)
        {
            return _standingTeeTimeRequestsManager.RemoveStandingTeeTimeRequest(selectedStandingTeeTimeRequest);
        }

        // Manager functions for Membership Applications
        public bool RecordMembershipApplication(MembershipApplication newMembershipApplication)
        {
            return _membershipApplicationManager.AddMembershipApplication(newMembershipApplication);
        }
        public List<MembershipApplication> ReviewOnHoldMembershipApplications()
        {
            return _membershipApplicationManager.GetOnHoldMembershipApplications();
        }
        public List<MembershipApplication> FindWaitlistedMembershipApplications()
        {
            return _membershipApplicationManager.GetWaitlistedMembershipApplications();
        }
        public MembershipApplication ViewMembershipApplication(int membershipApplicationId)
        {
            return _membershipApplicationManager.FindMembershipApplication(membershipApplicationId);
        }
        public bool RejectMembershipApplication(int membershipApplicationId)
        {
            return _membershipApplicationManager.CancelMembershipApplication(membershipApplicationId);
        }
        public bool WaitlistMembershipApplication(int membershipApplicationId)
        {
            return _membershipApplicationManager.UpdateMembershipApplication(membershipApplicationId);
        }
        public bool ApproveMembershipApplication(MembershipApplication approvedMembershipApplication)
        {
            return _membershipApplicationManager.AddApprovedMemberAccount(approvedMembershipApplication);
        }

        // Manager functions for Member Accounts 
        public MemberAccount ViewMemberAccount(string memberId)
        {
            return _memberAccountManager.FindMemberAccount(memberId);
        }
        
        // Manager functions for Player Scores
        public bool RecordPlayerScores(Scorecard scorecard)
        {
            return _playerScoreManager.AddPlayerScores(scorecard);
        }
        public decimal ViewPlayerHandicap(ClubBaistUser authenticatedPlayer)
        {
            return _playerScoreManager.CalculatePlayerHandicap(authenticatedPlayer);
        }
    }
}