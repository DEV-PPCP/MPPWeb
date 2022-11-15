using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPCP07302018.DataAccessLayer
{
    public class MVCUtilityMethod
    {
        public static DateTime? ConvertDateTime(string TextDate)
        {
            //CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            //newCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            //newCulture.DateTimeFormat.DateSeparator = "/";
            //Thread.CurrentThread.CurrentCulture = newCulture;
            DateTime date = new DateTime();
            if (TextDate != null && TextDate.Split('/').Length > 2)
            {
                int day = Convert.ToInt32(TextDate.Split('/')[0]);
                int month = Convert.ToInt32(TextDate.Split('/')[1]);
                int year = Convert.ToInt32(TextDate.Split('/')[2]);
                date = new DateTime(year, month, day);
                return date;
            }
            return null;
        }
    }
}