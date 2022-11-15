using Iph.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using PPCP07302018.Controllers.Session;
using Telerik.Reporting.Processing;
using PPCP07302018.DataAccessLayer;

namespace PPCP07302018.Controllers
{
    
    public class ProviderController : Controller
    {
        public ActionResult ProviderLogin()
        {
            MasterController mas = new MasterController();
            Session["SystemIPAddress"] = mas.GetIPAddress();
            return View();
        }
       
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        public string ProviderDetailsxml(Models.Provider.ProviderDetails modelParameter)
        {
            string xml = GetXMLFromObjects(modelParameter);
            // string returnData = xml.Replace("\"", "\'");
            return xml;
        }
        public JsonResult VerifyProvider(PPCP07302018.Models.Provider.ProviderLogin model)
        {
            string returnString = "0";
            string password = EncryptAndDecrypt.EncrypString(model.Password);
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Provider.ProviderLogin> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Provider.ProviderLogin>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "Username", "PassWord", "IPAddress" };
            string[] ParameterValue = new string[] { model.UserName, password, Convert.ToString(Session["SystemIPAddress"]) };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "ValidateProviderLogin";

            List<PPCP07302018.Models.Provider.ProviderLogin> List = objcall.CallServicePro(Convert.ToInt32(0), "ValidateProviderLogin", ServiceData);
            
            if (List.Count >= 1)
            {
                if (List[0].OrganizationID != null && List[0].OrganizationID != 0)
                {
                    returnString = "1";
                    Session["ProviderID"] = List[0].ProviderID;
                    Session["FirstName"] = List[0].FirstName;
                    Session["LastName"] = List[0].LastName;
                    Session["ProviderName"] = List[0].LastName+" "+ List[0].FirstName;
                    Session["Email"] = List[0].Email;
                    Session["DOB"] = List[0].DOB;
                    Session["CityID"] = List[0].CityID;
                    Session["CityName"] = List[0].CityName;
                    Session["StateID"] = List[0].StateID;
                    Session["StateName"] = List[0].StateName;
                    Session["OrganizationID"] = List[0].OrganizationID;
                    Session["OrganizationName"] = List[0].Organizationname;
                    Session["UserMobileNumber"] = List[0].MobileNumber;
                    Session["UserGender"] = List[0].Gender;
                    Session["OrgPassword"] = model.Password;
                    Session["OrgUserName"] = model.UserName;
                    Session["Salutation"] = List[0].Salutation;
                    Session["SalutationID"] = List[0].SalutationID;
                    Session["CountryID"] = List[0].CountryID;
                    Session["CountryName"] = List[0].CountryName;
                    Session["NPI"] = List[0].NPI;
                    Session["Zip"] = List[0].Zip;
                    Session["Specialization"] = List[0].Specialization;
                    Session["SpecializationID"] = List[0].SpecializationID;
                    Session["SpecializationName"] = List[0].SpecializationName;
                    Session["IsTwofactorAuthentication"] = List[0].IsTwofactorAuthentication;
                    Session["TwoFactorType"] = List[0].TwoFactorType;
                    Session["CountryCode"] = List[0].CountryCode;
                    Session["HeaderDisplayName"] = List[0].LastName + " " + List[0].FirstName;
                    Session["Fax"] = List[0].Fax;
                    Session["Address"] = List[0].Address;
                    Session["Degree"] = List[0].Degree;

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


                    // Session["2F_Primary_Phone"] = List[0].Primary_Phone;
                    //  Session["2F_OTP"] = List[0].OTP;

                    if (List[0].IsTwofactorAuthentication == true && List[0].TwoFactorType == 1)
                    {
                        //OTP form
                        if (List[0].OTP != null)
                        {
                            returnString = "0";
                            Session["ProloginOTP"] = Convert.ToInt32(List[0].OTP);
                            // redirect to OTP form
                        }
                        else
                        {
                            returnString = "2";
                            // Out of scope
                        }
                    }
                    else if (List[0].IsTwofactorAuthentication == true && List[0].TwoFactorType == 2)
                    {
                        if (List[0].PreferredIP != Convert.ToString(Session["SystemIPAddress"]))
                        {
                            //OTP form
                            if (List[0].OTP != null)
                            {
                                returnString = "0";
                                Session["ProloginOTP"] = Convert.ToInt32(List[0].OTP);
                                // redirect to OTP form
                            }
                            else
                            {
                                returnString = "2";
                                // Out of scope
                            }
                        }
                        else
                        {
                            returnString = "1";
                            // redirect to form
                        }
                    }
                    else if (List[0].IsTwofactorAuthentication == false)
                    {
                        returnString = "1";
                        // redirect to form
                    }
                    else
                    {
                        returnString = "1";
                        // redirect to form
                    }
                }
                else
                {
                    returnString = "2";
                }
            }

            return Json(returnString, JsonRequestBehavior.AllowGet);
        }
       public ActionResult GenerateProviderTermsAndConditions()
        {
            return View();
        }
        public JsonResult BindSalutation()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Dr.", Value = "1" });
            items.Add(new SelectListItem { Text = "Mr.", Value = "2" });
            items.Add(new SelectListItem { Text = "Mrs.", Value = "3" });
            items.Add(new SelectListItem { Text = "Ms.", Value = "4" });
            items.Add(new SelectListItem { Text = "Other", Value = "5" });

            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ProviderRegistration()
        {
            return View();
        }

        public ActionResult ProviderCredentials()
        {
            return View();
        }
        [ProviderSession]
        public ActionResult ViewProvider(PPCP07302018.Models.Provider.AddDoctor model)
        {
            model.OrganizationName = Convert.ToString(Session["OrganizationName"]);
            model.OrganizationID = Convert.ToInt32(Session["OrganizationID"]);
            model.ProviderID = Convert.ToString(Session["ProviderID"]);
            model.FirstName = Convert.ToString(Session["FirstName"]);
            model.LastName = Convert.ToString(Session["LastName"]);
            model.SalutationID = Convert.ToInt32(Session["SalutationID"]);
            model.Salutation = Convert.ToString(Session["Salutation"]);
            model.DOB = Convert.ToDateTime(Session["DOB"]);
            model.Gender = Convert.ToString(Session["UserGender"]);
            model.CountryCode = Convert.ToString(Session["CountryCode"]);
            model.MobileNumber = Convert.ToString(Session["UserMobileNumber"]);
            model.Email = Convert.ToString(Session["Email"]);
            model.NPI = Convert.ToString(Session["NPI"]);
            model.CountryID = Convert.ToInt32(Session["CountryID"]);
            model.CountryName = Convert.ToString(Session["CountryName"]);
            model.StateName = Convert.ToString(Session["StateName"]);
            model.CityName = Convert.ToString(Session["CityName"]);
            model.StateID = Convert.ToInt32(Session["StateID"]);
            model.CityID = Convert.ToInt32(Session["CityID"]);
            model.Fax= Convert.ToString(Session["Fax"]);
            model.Address = Convert.ToString(Session["Address"]);
            model.Zip = Convert.ToString(Session["Zip"]);
           model.Specialization=Convert.ToString(Session["Specialization"]);
           model.SpecializationID=Convert.ToString(Session["SpecializationID"]);
           model.SpecializationName=Convert.ToString(Session["SpecializationName"]);
            // model.Email = Convert.ToString(Session["UserEmail"]);
            model.TwoFactorType = Convert.ToInt32(Session["TwoFactorType"]); 
            model.IsTwofactorAuthentication = Convert.ToBoolean(Session["IsTwofactorAuthentication"]);
            model.UserName = Convert.ToString(Session["OrgUserName"]);
            model.Password = Convert.ToString(Session["OrgPassword"]);
            model.ConfirmPassword = Convert.ToString(Session["OrgPassword"]);
            model.Degree= Convert.ToString(Session["Degree"]);
            //model.Salutation = Convert.ToString(Session["Salutation"]);
            //model.Salutation = Convert.ToString(Session["Salutation"]);
            //     Session["ProviderID"] = model.ProviderID;
            //     Session["FirstName"] = model.FirstName;
            //     Session["LastName"] = model.LastName;
            //     Session["Email"] = model.Email;
            //     Session["DOB"] = model.DOB;
            //     Session["CityID"] = model.CityID;
            //     Session["CityName"] = model.CityName;
            //     Session["StateID"] = model.StateID;
            //     Session["StateName"] = model.StateName;
            //     Session["OrganizationID"] = model.OrganizationID;
            //     Session["OrganizationName"] = model.OrganizationName;
            //     Session["UserMobileNumber"] = model.MobileNumber;
            //     Session["UserGender"] = model.Gender;
            //     //Session["OrgPassword"] = model.Password;
            //     //Session["OrgUserName"] = model.UserName;

            return View(model);
        }
        public class ServiceData
        {
            public string WebMethodName;
            public string[] ParameterName;
            public string[] ParameterValue;

        }
        public JsonResult ProviderSearchAutoComplete(string Text)
{
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Provider.ProviderAutoComplete> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Provider.ProviderAutoComplete>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "OrganizationID", "Text" };
           
            string[] ParameterValue = new string[] {Convert.ToString(Session["OrganizationID"]), Text };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetProviderDetailsAutoComplete";
            List<PPCP07302018.Models.Provider.ProviderAutoComplete> List = objcall.CallServicesAdmin(Convert.ToInt32(0), "GetProviderDetailsAutoComplete", ServiceData);
            return Json(List, JsonRequestBehavior.AllowGet); 
        }


        public static string GetXMLFromObjects(object o)
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
        /// <summary>
        ///     To Get new Terms And conditions
        /// </summary>
        /// <returns></returns>
        public ActionResult ProviderTermsAndConditions()
        {
            return View();
        }
        [ProviderSession]
        public ActionResult SubscribedPlans()
        {
            return View();
        }
    }
}
