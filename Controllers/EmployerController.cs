using Iph.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace PPCP07302018.Controllers
{
    public class EmployerController : Controller
    {
        // GET: Employeer
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult EmployersRegistration(Models.Member.EmployerRegistrationDetails model)
        {
            MasterController mas = new MasterController();
            Session["SystemIPAddress"] = mas.GetIPAddress();
            string CountryCode = System.Configuration.ConfigurationSettings.AppSettings["CountryCode"].ToString();
            model.CountryCode = CountryCode;
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
            return View(model);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string EmployerRegistrationxml(Models.Organization.OrganizationDetails modelParameter)
        {
            string xml = GetXMLFromObject(modelParameter);
            string returnData = xml.Replace("\"", "\'");
            return returnData;
        }
        public static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }

        public ActionResult EmployersLogin()
        {
            MasterController mas = new MasterController();
            Session["SystemIPAddress"] = mas.GetIPAddress();
            return View();
        }

        public ActionResult EmployerForgotPassword()
        {
            return View();
        }
        public JsonResult VerifyEmployer(PPCP07302018.Models.Organization.MemberLoginDetails model)
        {
            string returnString = "2";
            string password = EncryptAndDecrypt.EncrypString(model.Password);
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.MemberLoginDetails> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.MemberLoginDetails>();
            //ServiceData ServiceData = new ServiceData();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "Username", "PassWord", "IPAddress" };
            string[] ParameterValue = new string[] { model.UserName, password, Convert.ToString(Session["SystemIPAddress"]) };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "ValidateUser";

            List<PPCP07302018.Models.Organization.MemberLoginDetails> List = objcall.CallService(Convert.ToInt32(0), "Validateser", ServiceData);

            if (List.Count >= 1)
            {
                // Session["EmployerID"] = List[0].MemberID;
                Session["MemberID"] = List[0].MemberID;
                Session["FirstName"] = List[0].FirstName;
                Session["LastName"] = List[0].LastName;
                Session["Email"] = List[0].Email;
                Session["DOB"] = List[0].DOB;
                Session["CityID"] = List[0].CityID;
                Session["CityName"] = List[0].CityName;
                Session["StateID"] = List[0].StateID;
                Session["StateName"] = List[0].StateName;
                Session["OrganizationID"] = List[0].OrganizationID;
                Session["OrganizationName"] = List[0].Organizationname;
                Session["UserMobileNumber"] = List[0].MobileNumber;
                Session["CountryCode"] = List[0].CountryCode;
                Session["UserGender"] = List[0].Gender;
                Session["OrgPassword"] = model.Password;
                Session["OrgUserName"] = model.UserName;
                Session["Salutation"] = List[0].Salutation;
                Session["CountryID"] = List[0].CountryID;
                Session["CountryName"] = List[0].CountryName;
                Session["Zip"] = List[0].Zip;
                Session["IsTwofactorAuthentication"] = List[0].IsTwofactorAuthentication;
                Session["TwoFactorType"] = List[0].TwoFactorType;
               

                if (List[0].Street1 == null && List[0].Street2 == null)
                {
                    string Address = List[0].State_Name + "," + List[0].City + " , " + List[0].Zip;
                    Session["MemberAddress"] = Address;
                }
                else if (List[0].Street1 != null && List[0].Street2 == null)
                {
                    string Address = List[0].State_Name + "," + List[0].City + " ," + List[0].Street1 + " ," + List[0].Zip;
                    Session["MemberAddress"] = Address;
                }
                else if (List[0].Street1 != null && List[0].Street2 != null)
                {
                    string Address = List[0].State_Name + "," + List[0].City + " ," + List[0].Street1 + " ," + List[0].Street2 + " ," + List[0].Zip;
                    Session["MemberAddress"] = Address;
                }
                Session["2F_Primary_Phone"] = List[0].Primary_Phone;
                Session["2F_OTP"] = List[0].OTP;

                if (List[0].IsTwofactorAuthentication == true && List[0].TwoFactorType == 1)
                {
                    //OTP form
                    if (List[0].OTP != null)
                    {
                        Session["loginOTP"] = Convert.ToInt32(List[0].OTP);
                        returnString = "0";

                        // redirect to OTP form
                    }

                }
                else if (List[0].IsTwofactorAuthentication == true && List[0].TwoFactorType == 2)
                {
                    if (List[0].PreferredIP != Convert.ToString(Session["SystemIPAddress"]))
                    {
                        //OTP form
                        if (List[0].OTP != null)
                        {

                            Session["loginOTP"] = Convert.ToInt32(List[0].OTP);
                            returnString = "0";
                            // redirect to OTP form
                        }

                    }
                    else
                    {
                        if (List[0].OrganizationUserTandCFlag == 1 || List[0].OrganizationTandCFlag == 1)
                        {
                            returnString = "4";
                        }
                        else
                        {
                            returnString = "1";
                            // redirect to form
                        }
                    }
                }
                else if (List[0].IsTwofactorAuthentication == false)
                {
                    if (List[0].OrganizationUserTandCFlag == 1 || List[0].OrganizationTandCFlag == 1)
                    {
                        returnString = "4";
                    }
                    else
                    {
                        returnString = "1";
                        // redirect to form
                    }
                }
                if (List[0].OrganizationUserTandCFlag == 1 || List[0].OrganizationTandCFlag == 1)
                {
                    returnString = "4";
                }
            }
            else
            {
                returnString = "3";
            }

            return Json(returnString, JsonRequestBehavior.AllowGet);
        }
        public JsonResult VerifyEmployerOTP(string otp)
        {
            string returnString = "0";

            if (!string.IsNullOrEmpty(otp))
            {
                int parsedValue;
                if (!int.TryParse(otp, out parsedValue))
                {
                    returnString = "0";
                }
                else if (Convert.ToInt32(otp) == Convert.ToInt32(Session["loginOTP"]))
                {
                    if (Session["TermsAndConditionsFlag"] == "1")
                    {
                        returnString = "4";
                    }
                    else
                    {
                        returnString = "1";
                    }
                }
                else
                {
                    returnString = "0";
                }
            }
            return Json(returnString, JsonRequestBehavior.AllowGet);
        }
    }
}