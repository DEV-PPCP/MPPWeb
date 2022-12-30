using Iph.Utilities;
using Newtonsoft.Json;
using PPCP07302018.Controllers.Session;
using PPCP07302018.DataAccessLayer;
using PPCP07302018.Models.Admin;
using PPCP07302018.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;
using Telerik.Reporting.Processing;
using WebGrease.Configuration;

namespace PPCP07302018.Controllers
{
    
    public class AccountController : Controller
    {
        public ActionResult MPPLogin(Login model)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            Session.Clear();
            MasterController mas = new MasterController();
            Session["SystemIPAddress"] = mas.GetIPAddress();
            return View(model);
        }

        public ActionResult ForgotCredentials()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
            Session.Clear();
            return RedirectToAction("MPPLogin", "Account", new Login { LoginType = "Member" });
        }

        public JsonResult ValidateCredentials(PPCP07302018.Models.Organization.MemberLoginDetails model)
        {
            string returnString = "0";
            string password = EncryptAndDecrypt.EncrypString(model.Password);
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.User> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.User>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "Username", "PassWord", "IPAddress" };
            string[] ParameterValue = new string[] { model.UserName, password, Convert.ToString(Session["SystemIPAddress"]) };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "ValidateCredentials";

            List<PPCP07302018.Models.Admin.User> List = objcall.CallServicesAdmin(Convert.ToInt32(0), "ValidateCredentials", ServiceData);

            if (List.Count >= 1)
            {
                if (List[0].IsAccountLocked)
                {
                    returnString = "999";
                    return Json(returnString, JsonRequestBehavior.AllowGet);
                }
                returnString = "1";
                Session["UserID"] = List[0].UserID;
                Session["UserName"] = model.UserName;
                Session["Password"] = List[0].Password;
                Session["FirstName"] = List[0].FirstName;
                Session["LastName"] = List[0].LastName;
                Session["Email"] = List[0].Email;
                Session["RoleName"] = List[0].RoleName;
                Session["RoleType"] = List[0].RoleType;
                //Session["DOB"] = List[0].DOB;
                //Session["CityID"] = List[0].CityID;
                //Session["CityName"] = List[0].CityName;
                //Session["StateID"] = List[0].StateID;
                //Session["StateName"] = List[0].StateName;
                Session["OrganizationID"] = List[0].OrganizationID;
                Session["OrganizationName"] = List[0].OrganizationName;
                Session["BillingTypeID"] = List[0].BillingTypeID;
                Session["HasPPVMembers"] = List[0].HasPPVMembers;
                Session["UserCountryCode"] = List[0].CountryCode;
                Session["UserMobileNumber"] = List[0].MobileNumber;
                //Session["UserGender"] = List[0].Gender;
                //Session["OrgPassword"] = model.Password;
                Session["OrgUserName"] = model.UserName;
                Session["IsTermsNeeded"] = List[0].IsTermsNeeded;
                Session["IsTermsAccepted"] = List[0].IsTermsAccepted;
                Session["TermsAndConditionsFile"] = List[0].TermsAndConditionsFile;
                //Session["OrganizationTandCFlag"] = List[0].OrganizationTandCFlag;
                //Session["OrganizationUserTandCFlag"] = List[0].OrganizationUserTandCFlag;
                switch(List[0].RoleType)
                {
                    case GlobalFunctions.RoleType.Organization:
                        Session["HeaderDisplayName"] = List[0].OrganizationName;
                        break;
                    case GlobalFunctions.RoleType.Provider:
                        Session["HeaderDisplayName"] = List[0].OrganizationName + " - " + List[0].FirstName + " " + List[0].LastName;
                        break;
                    case GlobalFunctions.RoleType.Member:
                        Session["HeaderDisplayName"] = List[0].FirstName + " " + List[0].LastName;
                        break;
                    case GlobalFunctions.RoleType.MPP:
                        Session["HeaderDisplayName"] = "Administrator - " + List[0].FirstName + " " + List[0].LastName;
                        break;
                }

                //member related session data
                Session["MemberPrimaryPhone"] = List[0].MobileNumber;
                Session["MemberID"] = List[0].MemberID;
                Session["MemberParentID"] = List[0].MemberParentID;
                Session["MemberName"] = List[0].FirstName + " " + List[0].LastName;
                Session["MemberCountryCode"] = List[0].CountryCode;
                Session["MemberPrimaryPhone"] = List[0].MobileNumber;
                Session["MemberEmail"] = List[0].Email;
                Session["StripeCustomerID"] = List[0].StripeCustomerID;

                //provider related session data
                Session["ProviderID"] = List[0].ProviderID;
                Session["ProviderName"] = List[0].LastName + " " + List[0].FirstName;

                //if (List[0].RoleType == null) 
                //{
                //    if(List[0].RoleName == "Admin") //indicates MPP admin role
                //        Session["TermsAndConditionsFlag"] = "0";
                //    if (List[0].RoleName == "Member") //indicates member role
                //    {
                //        if (List[0].MemberTermsAndConditionsFlag == 1)
                //            Session["TermsAndConditionsFlag"] = "1";
                //        else
                //            Session["TermsAndConditionsFlag"] = "0";
                //    }
                //}
                //else
                //{
                //    //Organization or provider
                //    if (List[0].OrganizationTandCFlag == 1 || List[0].OrganizationUserTandCFlag == 1)
                //        Session["TermsAndConditionsFlag"] = "1";
                //    else
                //        Session["TermsAndConditionsFlag"] = "0";
                //}

                //if (string.IsNullOrEmpty(List[0].Address))
                //{
                //    string Address = List[0].CityName + "," + List[0].StateName + " , " + List[0].Zip;
                //    Session["UserAddress"] = Address;
                //}
                //else
                //{
                //    string Address = List[0].Address + "," + List[0].CityName + "," + List[0].StateName + " , " + List[0].Zip;
                //    Session["MemberAddress"] = Address;
                //}

                if (List[0].IsTwoFactorEnabled)
                {
                    //OTP form
                    if (List[0].TwoFactorType == 1)
                    {
                        if (List[0].OTP != null)
                        {
                            returnString = "0";
                            Session["LoginOTP"] = Convert.ToInt32(List[0].OTP);// redirect to OTP form
                        }
                    }
                    if (List[0].TwoFactorType == 2)
                    {
                        if (List[0].LastIPAddress != Convert.ToString(Session["SystemIPAddress"]))
                        {
                            //OTP form
                            if (List[0].OTP != null)
                            {
                                returnString = "0";
                                Session["LoginOTP"] = Convert.ToInt32(List[0].OTP); // redirect to OTP form
                            }
                        }
                    }
                }
                else
                {
                    //if (List[0].OrganizationTandCFlag == 1 || List[0].OrganizationUserTandCFlag == 1 || List[0].MemberTermsAndConditionsFlag == 1)
                    if (List[0].IsTermsAccepted)
                    {
                        returnString = "1";
                    }
                    else
                    {
                        returnString = "4";
                        // redirect to form
                    }
                }
            }
            else
            {
                returnString = "2";
            }

            var result = new { returnString = returnString, roleType = List.Count >= 1 ? List[0].RoleType : string.Empty };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult VerifyUserOTP(string otp)
        {
            string returnString = "0";

            if (!string.IsNullOrEmpty(otp))
            {
                int parsedValue;
                if (!int.TryParse(otp, out parsedValue))
                {
                    returnString = "0";
                }
                else if (Convert.ToInt32(otp) == Convert.ToInt32(Session["LoginOTP"]))
                {
                    if (Session["IsTermsAccepted"].ToString() == "True")
                    {
                        returnString = "1";
                    }
                    else
                    {
                        returnString = "4";
                    }
                }
                else
                {
                    returnString = "0";
                }
            }

            var result = new { returnString = returnString, roleName = Session["RoleName"].ToString(), roleType = Session["RoleType"] != null ? Session["RoleType"].ToString() : string.Empty };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TermsAndConditions()
        {
            TermsConditions model = new TermsConditions();
            switch(Session["RoleType"].ToString())
            {
                case "Organization":
                    model.Type = Convert.ToInt32(PPCP07302018.Utils.GlobalFunctions.TermsAndConditons.Organization);
                    break;
                case "Member":
                    model.Type = Convert.ToInt32(PPCP07302018.Utils.GlobalFunctions.TermsAndConditons.Member);
                    break;
                case "Provider":
                    model.Type = Convert.ToInt32(PPCP07302018.Utils.GlobalFunctions.TermsAndConditons.Provider);
                    break;
            }
            return View(model);
        }

        public JsonResult EncryptPassword(string clearTextPass)
        {
            string returnString = "0";

            if (!string.IsNullOrEmpty(clearTextPass))
            {
                returnString = EncryptAndDecrypt.EncrypString(clearTextPass);
            }

            var result = new { returnString = returnString };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClaimConfirm(int id)
        {
            DataAccessLayer.ServiceCall<PPCP07302018.Models.MemberVisit> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.MemberVisit>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "VisitId" };
            string[] ParameterValue = new string[] { id.ToString() };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetVisitById";

            List<PPCP07302018.Models.MemberVisit> List = objcall.CallServices(Convert.ToInt32(0), "GetVisitById", ServiceData);

            PPCP07302018.Models.MemberVisit model = List.FirstOrDefault();


            //DataAccessLayer.ServiceCall<PPCP07302018.Models.Result> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Result>();
            //PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            //string[] ParameterName = new string[] { "VisitId" };
            //string[] ParameterValue = new string[] { id.ToString() };
            //ServiceData.ParameterName = ParameterName;
            //ServiceData.ParameterValue = ParameterValue;
            //ServiceData.WebMethodName = "ClaimConfirm";

            //List<PPCP07302018.Models.Result> List = objcall.CallServices(Convert.ToInt32(0), "ClaimConfirm", ServiceData);

            //PPCP07302018.Models.Result model = List.FirstOrDefault();
            //if (model != null && model.ResultID == -1) //Handling Android self clicking
            //{
            //    return null;
            //}
            //if (model != null && model.ResultID == 1)
            //{
            //    model.ResultName = "Thank you for confirming you visited this provider.";
            //}
            //else
            //{
            //    model.ResultName = "Thank you! You have already replied to this message.";
            //}

            return View(model);
        }
        //public ActionResult ClaimDeny(int id)
        //{
        //    DataAccessLayer.ServiceCall<PPCP07302018.Models.Result> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Result>();
        //    PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
        //    string[] ParameterName = new string[] { "VisitId" };
        //    string[] ParameterValue = new string[] { id.ToString() };
        //    ServiceData.ParameterName = ParameterName;
        //    ServiceData.ParameterValue = ParameterValue;
        //    ServiceData.WebMethodName = "ClaimDeny";

        //    List<PPCP07302018.Models.Result> List = objcall.CallServices(Convert.ToInt32(0), "ClaimDeny", ServiceData);

        //    PPCP07302018.Models.Result model = List.FirstOrDefault();
        //    if (model != null && model.ResultID == -1) //Handling Android self clicking
        //    {
        //        return null;
        //    }
        //    if (model != null && model.ResultID == 1)
        //    {
        //        model.ResultName = "Thank you for confirming you did not visit this provider.";
        //    }
        //    else
        //    {
        //        model.ResultName = "Thank you! You have already replied to this message.";
        //    }

        //    return View(model);
        //}

        public ActionResult MemberReferralRegistration(int id)
        {
            Models.Member.MemberDetails model = new Models.Member.MemberDetails();
            model.ReferringMemberId = id;
            return RedirectToAction("MemberRegistration", "Member", model);
        }

        public ActionResult MemberRxRegistration()
        {
            Models.Member.MemberDetails model = new Models.Member.MemberDetails();
            model.IsRxRegistration = true;
            return RedirectToAction("MemberRegistration", "Member", model);
        }
    }
}