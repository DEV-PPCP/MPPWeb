using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPCP07302018.Utils
{
    public class GlobalFunctions
    {
        public static class RoleType
        {
            public const string MPP = "MPP";
            public const string Organization = "Organization";
            public const string Provider = "Provider";
            public const string Member = "Member";
        }
        public static class RoleName
        {
            public const string Admin = "Admin";
            public const string ReadOnly = "ReadOnly";
        }

        public static class BillingType
        {
            public const int Lumpsum = 25;
            public const int PPV = 26;
        }

        public static class VisitType
        {
            public const int InPerson = 15;
            public const int TeleVisit = 16;
        }
        public static class PaymentType
        {
            public const int Cash = 20;
            public const int Credit = 21;
            public const int Check = 22;
        }

        public static class ClaimStatus
        {
            public const int Saved = 1;
            public const int Submitted = 2;
            public const int Verification = 3;
            public const int Denied = 4;
            public const int Approved = 5;
            public const int Paid = 6;
        }
        public static class ClaimSubStatus
        {
            public const int PlanNotActive = 7;
            public const int ExceededClaimCount = 8;
            public const int TextSent = 9;
            public const int EmailSent = 10;
            public const int TextAndEmailSent = 11;
            public const int MemberContactError = 12;
            public const int MemberDenied = 13;
        }

        /// <summary>
        /// In Member Table Type Column is their we have to save the Type
        /// </summary>
        public enum Member
        {
            Member = 1,
            Organization = 2,
            Provider=3
        }
        /// <summary>
        /// In Terms&Conditions Table Type Column is their we have to save the Type
        /// </summary>
        public enum TermsAndConditons
        {
            //Patient = 1,
            //Organization = 2,
            //User = 3

            Organization = 1,
            Member = 2,
            Provider = 3,
            User = 4
        }

        public enum LookupType
        {
            ClaimStatus = 2,
            VisitType = 3,
            PaymentType = 4,
        }
    }
}