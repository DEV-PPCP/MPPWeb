using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPCP07302018.Models
{
    public class ReferralSummary
    {
        public int TotalPoints { get; set; }
        public int PointsEarned { get; set; }
        public int PointsPending { get; set; }
        public int AvailableToUse { get; set; }
        public int PointsUsed { get; set; }
        public int PointsCashed { get; set; }
        public string MemberReferralLink { get; set; }
    }

    public class Referral
    {
        public int Id { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public bool isReminderSet { get; set; }
        public DateTime? ReferralDate { get; set; }

        //set if member joins
        public DateTime? ReferralJoinedDate { get; set; }
        public int ReferralMemberID { get; set; }
        public int ReferralPlanID { get; set; }
        public string PlanName { get; set; }
        public int Points { get; set; }
        public DateTime? PointsEarnedDate { get; set; }
    }

    public class ReferralCheck
    {
        public int Id { get; set; }
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string MemberAddress { get; set; }
        public string MemberCardID { get; set; }
        public DateTime? RequestDate { get; set; }
        public int Points { get; set; }
        public string Status { get; set; }

    }
}