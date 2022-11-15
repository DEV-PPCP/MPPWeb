using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPCP07302018.Utils
{
    public class GlobalFunctions
    {

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
            Patient = 1,
            Organization = 2,
            User = 3
        }
    }
}