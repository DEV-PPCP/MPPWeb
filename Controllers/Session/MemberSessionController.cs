using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PPCP07302018.Controllers.Session
{
    public class MemberSessionController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            // string controllername = this.ValueProvider.GetValue("controller").RawValue.ToString();
            HttpContext ctx = HttpContext.Current;
            // check  sessions here
            string actionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            if((actionName.Equals("VerifyUser")) || (actionName.Equals("MemberLogin")) || 
                (actionName.Equals("MemberRegistration")) || (actionName.Equals("MemberCredentials")) ||
                (actionName.Equals("SessionOut")))
            {
                base.OnActionExecuting(filterContext);

            }
            else
            {
                if (HttpContext.Current.Session["UserName"] == null)
                {
                   filterContext.Result = new RedirectResult("~/Member/SessionOut?key=1");
                   return;
                }
            }
            
        }
    }
}
