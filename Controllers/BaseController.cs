using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPCP07302018.Controllers
{
    public class BaseController : Controller
    {
        public bool UserIsInRole(string roletype, string roleName)
        {
            bool isInRole = false;
            if(roletype == Session["RoleType"].ToString()  && roleName == Session["RoleName"].ToString())
            {
                isInRole = true;
            }
            return isInRole;
        }
    }
}