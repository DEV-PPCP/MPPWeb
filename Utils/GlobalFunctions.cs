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
    }
}