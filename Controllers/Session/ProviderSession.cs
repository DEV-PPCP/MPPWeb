using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPCP07302018.Controllers.Session
{
    public class ProviderSession: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            // string controllername = this.ValueProvider.GetValue("controller").RawValue.ToString();
            HttpContext ctx = HttpContext.Current;
            // check  sessions here
            string actionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            if ((actionName.Equals("ProviderLogin")) || (actionName.Equals("VerifyProvider")) ||
                (actionName.Equals("ProviderRegistration")) || (actionName.Equals("ProviderCredentials")) ||
                (actionName.Equals("SessionOut")))
            {
                base.OnActionExecuting(filterContext);

            }
            else
            {
                if (HttpContext.Current.Session["HeaderDisplayName"] == null)
                {
                    filterContext.Result = new RedirectResult("~/Member/SessionOut?key=4");
                    return;
                }
            }

        }
    }
}