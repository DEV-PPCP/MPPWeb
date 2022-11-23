using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Net;
using System.Xml.Serialization;
using Iph.Utilities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Data;
using System.Reflection;
using PPCP07302018.Controllers.Session;
using Telerik.Reporting.Processing;
using PPCP07302018.Models.Admin;
using PPCP07302018.Models;

namespace PPCP07302018.Controllers
{
   [OrganizationSessionController]
    public class OrganizationController : Controller
    {
        private string DownloadDocumentPath = System.Configuration.ConfigurationSettings.AppSettings["DownloadDocumentsPath"].ToString();
        public ActionResult OrganizationLogin()
        {
            Login model = new Login();
            model.LoginType = "Organization";
            return RedirectToAction("MPPLogin", "Account", model);
        }

        // GET: Organization
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OrgnizationRegistration()
        {
            
            MasterController mas = new MasterController();
            Session["SystemIPAddress"] = mas.GetIPAddress();
            return View();
        }

        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string OrganizationRegistrationxml(Models.Organization.OrganizationDetails modelParameter)
        {
            string xml = GetXMLFromObject(modelParameter);
            string returnData = xml.Replace("\"", "\'");
            return returnData;
        }
         public string GetOrganizationProviderXml(Models.Organization.ProviderDetails modelParameter)
        {
            string xml = GetXMLFromObject(modelParameter);
            //string returnData = xml.Replace("\"", "\'");
            return xml;
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string SaveDoctorDetailsxml(Models.Organization.AddDoctor modelParameter)
        {
            string xml = GetXMLFromObjects(modelParameter);
            //string returnData = xml.Replace("\"", "\'");
            return xml;
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


        public ActionResult PlanDetails()
        {
            return View();
        }
        public ActionResult PlanEnroll()
        {
            return View();
        }
        public ActionResult PaymentDetails()
        {
            return View();
        }

        
        public ActionResult PlansCreation()
        {
            return View();
        }
        public ActionResult AddUser()
        {
            
            return View();
        }

        public ActionResult ChangePassword()
        {
           
            return View();
        }

        public JsonResult VerifyOrgOTP(string otp)
        {
            string returnString = "0";

            if (!string.IsNullOrEmpty(otp))
            {
                int parsedValue;
                if (!int.TryParse(otp, out parsedValue))
                {
                    returnString = "0";
                }
                else if (Convert.ToInt32(otp) == Convert.ToInt32(Session["OrgloginOTP"]))
                {
                    if (Session["OrganizationTandCFlag"] == "1" || Session["OrganizationUserTandCFlag"] == "1")
                        
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
        /// <summary>
        /// Validate Organization User Credential at the time of login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>        
        public JsonResult VerifyOrganization(PPCP07302018.Models.Organization.MemberLoginDetails model)
        {
            string returnString = "0";
            string password = EncryptAndDecrypt.EncrypString(model.Password);
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.OrganizationDetails> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.OrganizationDetails>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "Username", "PassWord", "IPAddress" };
            string[] ParameterValue = new string[] { model.UserName, password, Convert.ToString(Session["SystemIPAddress"]) };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "ValidateOrganization";

            List<PPCP07302018.Models.Organization.OrganizationDetails> List = objcall.CallServices(Convert.ToInt32(0), "ValidateOrganization", ServiceData);

            if (List.Count >= 1)
            {
                if (List[0].OrganizationID != null && List[0].OrganizationID != 0)
                {
                    returnString = "1";
                    Session["UserID"] = List[0].UserID;
                    Session["FirstName"] = List[0].FirstName;
                    Session["LastName"] = List[0].LastName;
                    Session["Email"] = List[0].Email;
                    Session["DOB"] = List[0].DOB;
                    Session["CityID"] = List[0].CityID;
                    Session["CityName"] = List[0].CityName;
                    Session["StateID"] = List[0].StateID;
                    Session["StateName"] = List[0].StateName;
                    Session["OrganizationID"] = List[0].OrganizationID;
                    Session["OrganizationName"] = List[0].OrganizationName;
                    Session["UserMobileNumber"] = List[0].MobileNumber;
                    Session["UserGender"] = List[0].Gender;
                    Session["OrgPassword"] = model.Password;
                    Session["OrgUserName"] = model.UserName;
                    Session["OrganizationTandCFlag"] = List[0].OrganizationTandCFlag;
                    Session["OrganizationUserTandCFlag"] = List[0].OrganizationUserTandCFlag;
                    Session["HeaderDisplayName"] = List[0].OrganizationName;

                    
                    if (string.IsNullOrEmpty(List[0].Address))
                    {
                        string Address = List[0].CityName + "," + List[0].StateName + " , " + List[0].Zip;
                        Session["UserAddress"] = Address;
                    }
                    else
                    {
                        string Address = List[0].Address + "," + List[0].CityName + "," + List[0].StateName + " , " + List[0].Zip;
                        Session["MemberAddress"] = Address;
                    }


                    if (List[0].IsTwofactorAuthentication == true && List[0].TwoFactorType == 1)
                    {
                        //OTP form
                        if (List[0].Otp != null)
                        {
                            returnString = "0";
                            Session["OrgloginOTP"] = Convert.ToInt32(List[0].Otp);
                            // redirect to OTP form
                        }

                    }
                    else if (List[0].IsTwofactorAuthentication == true && List[0].TwoFactorType == 2)
                    {
                        if (List[0].PreferredIP != Convert.ToString(Session["SystemIPAddress"]))
                        {
                            //OTP form
                            if (List[0].Otp != null)
                            {
                                Session["OrgloginOTP"] = Convert.ToInt32(List[0].Otp);
                                returnString = "0";

                                // redirect to OTP form
                            }

                            else
                            {
                                if (List[0].OrganizationTandCFlag == 1 || List[0].OrganizationUserTandCFlag==1)
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
                            if (List[0].OrganizationTandCFlag == 1 || List[0].OrganizationUserTandCFlag == 1)
                            {
                                returnString = "4";
                            }
                            else
                            {
                                returnString = "1";
                                //redirect to form
                            }
                        }
                    }
                }
                if (List[0].OrganizationTandCFlag == 1 || List[0].OrganizationUserTandCFlag == 1)
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
        public ActionResult OrganizationProfileDetails(PPCP07302018.Models.Organization.OrganizationDetails model)
        {
            ServiceCall<PPCP07302018.Models.Organization.OrganizationDetails> objcall = new ServiceCall<PPCP07302018.Models.Organization.OrganizationDetails>();
            ServiceData ServiceData = new ServiceData();
            string[] ParameterName = new string[] { "OrganizationID" };
            string[] ParameterValue = new string[] { Convert.ToString(Session["OrganizationID"]) };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetOrganizationUsersProfile";
            List<PPCP07302018.Models.Organization.OrganizationDetails> List = objcall.CallServices(Convert.ToInt32(0), "GetOrganizationUsersProfile", ServiceData);
            if (List.Count > 0)
            {
                model.OrganizationID = Convert.ToInt32(Session["OrganizationID"]);
                model.UserID = List[0].UserID;
                model.OrganizationName = List[0].OrganizationName;
                model.TaxID = List[0].TaxID;
                model.OrgEmail = List[0].OrgEmail;
                model.OrgCountryCode = List[0].OrgCountryCode;
                model.OrgMobileNumber = List[0].OrgMobileNumber;
                model.OrgCountryID = List[0].OrgCountryID;
                model.OrgCountryName = List[0].OrgCountryName;
                model.OrgStateID = List[0].OrgStateID;
                model.OrgStateName = List[0].OrgStateName;
                model.OrgCityID = List[0].OrgCityID;
                model.OrgCityName = List[0].OrgCityName;
                model.OrgZip = List[0].OrgZip;
                model.Address = List[0].OrgAddress;
                model.FirstName = List[0].FirstName;
                model.LastName = List[0].LastName;
                model.DOB = List[0].DOB;
                model.Age = List[0].Age;
                model.Gender = List[0].Gender;
                model.Salutation = List[0].Salutation;
                model.SalutationID = List[0].SalutationID;
                model.CountryCode = List[0].UserCountryCode;
                model.MobileNumber = List[0].UserMobileNumber;
                model.CountryID = List[0].UserCountryID;
                model.CountryName = List[0].UserCountryName;
                model.StateID = List[0].UserStateID;
                model.StateName = List[0].UserStateName;
                model.CityID = List[0].UserCityID;
                model.CityName = List[0].UserCityName;
                model.Zip = List[0].UserZip;
                model.Email = List[0].UserEmail;
                model.TwoFactorType = List[0].TwoFactorType;
                model.IsTwofactorAuthentication = List[0].IsTwofactorAuthentication;
                model.UserName = Convert.ToString(Session["OrgUserName"]);
                model.Password = Convert.ToString(Session["OrgPassword"]);
                model.ConfirmPassword = Convert.ToString(Session["OrgPassword"]);
                model.SSN= Convert.ToInt32(List[0].UserSSN);
            }
            return View(model);
        }

        public class ServiceData
        {
            public string WebMethodName;
            public string[] ParameterName;
            public string[] ParameterValue;

        }
        public ActionResult AddDoctor()
        {

            return View();
        }
        public ActionResult AddMember(Models.Organization.AddMemberDetails model)
        {
            string CountryCode = System.Configuration.ConfigurationSettings.AppSettings["CountryCode"].ToString();
            model.CountryCode = CountryCode;
            model.Type = Convert.ToInt32(Utils.GlobalFunctions.Member.Organization);
            return View(model);
        }
        public string GetProviderPlanList()
        {
            List<PPCP07302018.Models.Organization.OrganizationProviderPlans> obj = new List<PPCP07302018.Models.Organization.OrganizationProviderPlans>();
            obj = TempData["ProviderPlanIdValues"] as List<PPCP07302018.Models.Organization.OrganizationProviderPlans>;
            
            string xml = GetXMLFromObjects(obj);
            return xml;

        }
        public string AddMemberxml(Models.Organization.AddMemberDetails modelParameter)
        {
            string xml = GetXMLFromObject(modelParameter);
            string returnData = xml.Replace("\"", "\'");
            return returnData;
        }
        /// <summary>
        /// Display the  Member Details Based on OrganizationID
        /// </summary>
        /// <returns></returns>
        public ActionResult ViewMemberDetails()
        {
            return View();
        }

        public ActionResult ViewPaymentDetails()
        {
            return View();
        }
        public ActionResult ViewOrganization()
        {
            return View();
        }
        public ActionResult ViewProviders()
        {
            return View();
        }
        public ActionResult SubscribePlans()
        {
            TempData["ProviderPlanIdValues"] = null;
            return View();
        }

        public ActionResult ViewUsers(Models.Organization.OrganizationDetails model)
        {
            return View(model);
        }
        public ActionResult GenerateOrganizationTermsAndConditions()
        {
            
            return View();
        }
       
        public JsonResult SubscribeProviderPlans(string PlanId ,string PlanName,string ProviderId, string providerName)
        {
           
            List<PPCP07302018.Models.Organization.OrganizationProviderPlans> objList = new List<Models.Organization.OrganizationProviderPlans>();
            Models.Organization.OrganizationProviderPlans obj = new Models.Organization.OrganizationProviderPlans();

           
            if (TempData["ProviderPlanIdValues"] == null)
            {

                obj.PlanID = Convert.ToInt32(PlanId);
                obj.PlanName = PlanName;
                obj.OrganizationID = Convert.ToInt32(Session["OrganizationID"]);
                obj.OrganizationName = Convert.ToString(Session["OrganizationName"]);
                obj.ProviderID = Convert.ToInt32(ProviderId);
                obj.ProviderName = providerName;
                objList.Add(obj);
            }

            else
            {
                objList = TempData["ProviderPlanIdValues"] as List<PPCP07302018.Models.Organization.OrganizationProviderPlans>;

                if (objList.Any(m => m.PlanID == Convert.ToInt32(PlanId)))
                {
                    var PlanID = objList.Find(m => m.PlanID == Convert.ToInt32(PlanId));
                    objList.Remove(PlanID);
                }
                else
                {
                    obj.PlanID = Convert.ToInt32(PlanId);
                    obj.PlanName = PlanName;
                    obj.OrganizationID = Convert.ToInt32(Session["OrganizationID"]);
                    obj.OrganizationName = Convert.ToString(Session["OrganizationName"]);
                    obj.ProviderID = Convert.ToInt32(ProviderId);
                    obj.ProviderName = providerName;
                    objList.Add(obj);
                }

            }
            TempData["ProviderPlanIdValues"] = objList;
            return Json(objList, JsonRequestBehavior.AllowGet);

        }
        class ServiceCall<T>
        {
            public List<T> CallServices(int TestID, string Methodname, ServiceData data)
            {
                string ServiceUrl = System.Configuration.ConfigurationSettings.AppSettings["ServiceUrl"].ToString();
                List<T> List = new List<T>();
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri(ServiceUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "text");
                    var urls = "OrganizationServices/" + Methodname;
                    var o = data;
                    string T = JsonConvert.SerializeObject(o);
                    var content = new StringContent(T, System.Text.Encoding.UTF8, "application/json");
                    var responseTask = client.PostAsJsonAsync(urls, o);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        var objects = Newtonsoft.Json.Linq.JArray.Parse(jsonString.Result);

                        var studentsa = JsonConvert.DeserializeObject<List<T>>(objects[0].ToString());
                        List<T> student111 = new List<T>();

                        student111.AddRange(studentsa);

                        List = student111;
                    }
                    else
                    {
                        List = new List<T>();
                    }
                }
                return List;
            }

        }
        /// <summary>
        /// GenerateOrganizationCard
        /// Veena
        /// </summary>
        /// <param name="OrganizationID"></param>
        /// <param name="PlanID"></param>
        /// <returns></returns>
        public ActionResult GenerateOrganizationCard(string OrganizationID, string PlanID)
        {

            try
            {
                DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.OrganizationDetails> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.OrganizationDetails>();
                // ServiceData ServiceData = new ServiceData();
                PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
                string[] ParameterName = new string[] { "strOrganizationID" };
                string[] ParameterValue = new string[] { OrganizationID };
                ServiceData.ParameterName = ParameterName;
                ServiceData.ParameterValue = ParameterValue;
                ServiceData.WebMethodName = "GetOrganizationMemberDetails";

                List<PPCP07302018.Models.Organization.OrganizationDetails> objOrganizationDetails = objcall.CallService(Convert.ToInt32(0), "GetOrganizationMemberDetails", ServiceData);

                Telerik.Reporting.ReportBook rptbook = new Telerik.Reporting.ReportBook();
                Telerik.Reporting.ReportBook rptbook1 = new Telerik.Reporting.ReportBook();
                Telerik.Reporting.PageHeaderSection HeaderSection1 = new Telerik.Reporting.PageHeaderSection();
                Telerik.Reporting.PageFooterSection FooterSection1 = new Telerik.Reporting.PageFooterSection();
                HeaderSection1 = new Telerik.Reporting.PageHeaderSection();
                FooterSection1 = new Telerik.Reporting.PageFooterSection();
                Telerik.Reporting.Report rptirDCS = new Telerik.Reporting.Report();
                Telerik.Reporting.DetailSection detail = new Telerik.Reporting.DetailSection();
                detail = new Telerik.Reporting.DetailSection();

                #region Header
                Telerik.Reporting.Shape shapeHeader = new Telerik.Reporting.Shape();
                shapeHeader = new Telerik.Reporting.Shape();
                Telerik.Reporting.HtmlTextBox txtHeading = new Telerik.Reporting.HtmlTextBox();
                txtHeading = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox FeatureDate = new Telerik.Reporting.HtmlTextBox();
                FeatureDate = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox OrganizationName = new Telerik.Reporting.HtmlTextBox();
                OrganizationName = new Telerik.Reporting.HtmlTextBox();

                OrganizationName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                OrganizationName.Name = "OrganizationName";
                OrganizationName.Style.Font.Bold = true;
                OrganizationName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Pixel(14));
                string OrganizationNameDisplay = "My Physician Plan";
                OrganizationName.Value = OrganizationNameDisplay;

                txtHeading.Name = "Heading";
                txtHeading.Value = "Plan Payment Summary";
                txtHeading.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Pixel(14));
                txtHeading.Style.Font.Bold = true;
                txtHeading.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(12D);
                txtHeading.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(1D));

                FeatureDate.Name = "FeatureDate";
                //  FeatureDate.Value = "( " + FromDate + " to " + ToDate + " )";
                FeatureDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Pixel(14));
                FeatureDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(txtHeading.Bottom.Value));
                FeatureDate.Visible = false;

                shapeHeader.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4D), Telerik.Reporting.Drawing.Unit.Inch(FeatureDate.Bottom.Value));
                shapeHeader.Name = "shapeHeader";
                shapeHeader.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                shapeHeader.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.6082919692993164D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                shapeHeader.Stretch = true;
                shapeHeader.Style.Font.Bold = true;
                shapeHeader.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(1D);
                shapeHeader.Visible = true;

                HeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.4);
                HeaderSection1.PrintOnFirstPage = true;
                HeaderSection1.PrintOnLastPage = false;
                HeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                  txtHeading,shapeHeader,FeatureDate,OrganizationName,
                  });
                HeaderSection1.Name = "pageHeaderSection1";
                #endregion

                #region Patient_Details
                Telerik.Reporting.HtmlTextBox htmlTextBoxSubHeading1 = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxSubHeading1 = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.Shape htmlTextBoxSubHeadingShape1 = new Telerik.Reporting.Shape();
                htmlTextBoxSubHeadingShape1 = new Telerik.Reporting.Shape();
                Telerik.Reporting.Shape htmlTextBoxSubHeadingShape3 = new Telerik.Reporting.Shape();
                htmlTextBoxSubHeadingShape3 = new Telerik.Reporting.Shape();

                Telerik.Reporting.HtmlTextBox htmlTextBoxPayer = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPayer = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxPayerColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPayerColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxName = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxName = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxNameColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxNameColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxDOB = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxDOB = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxDOBColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxDOBColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxGender = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxGender = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxPatientGenderColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPatientGenderColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxEmail = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxEmail = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxEmailColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxEmailColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxAddress = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxAddress = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxAddressColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxAddressColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxCity = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxCity = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxCityColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxCityColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextState = new Telerik.Reporting.HtmlTextBox();
                htmlTextState = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxStateColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxStateColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxZip = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxZip = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxZipColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxZipColon = new Telerik.Reporting.HtmlTextBox();

                htmlTextBoxSubHeadingShape3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3D), Telerik.Reporting.Drawing.Unit.Inch(0.1D));
                htmlTextBoxSubHeadingShape3.Name = "htmlTextBoxSubHeadingShape3";
                htmlTextBoxSubHeadingShape3.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                htmlTextBoxSubHeadingShape3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                htmlTextBoxSubHeadingShape3.Stretch = true;
                htmlTextBoxSubHeadingShape3.Style.Font.Bold = true;
                htmlTextBoxSubHeadingShape3.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.8D);
                htmlTextBoxSubHeadingShape3.Visible = true;

                htmlTextBoxSubHeading1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.7D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape3.Bottom.Value));
                htmlTextBoxSubHeading1.Name = "htmlTextBoxSubHeading1" + 1;
                htmlTextBoxSubHeading1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxSubHeading1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
                htmlTextBoxSubHeading1.Style.Font.Bold = false;
                htmlTextBoxSubHeading1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                htmlTextBoxSubHeading1.Value = "<strong>Member Information</strong>";

                htmlTextBoxSubHeadingShape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeading1.Bottom.Value));
                htmlTextBoxSubHeadingShape1.Name = "htmlTextBoxSubHeadingShape1";
                htmlTextBoxSubHeadingShape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                htmlTextBoxSubHeadingShape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                htmlTextBoxSubHeadingShape1.Stretch = true;
                htmlTextBoxSubHeadingShape1.Style.Font.Bold = false;
                htmlTextBoxSubHeadingShape1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.4D);
                htmlTextBoxSubHeadingShape1.Visible = true;

                htmlTextBoxPayer.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape1.Bottom.Value + 0.1D));
                htmlTextBoxPayer.Name = "htmlTextBoxPayer" + 1;
                htmlTextBoxPayer.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPayer.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPayer.Style.Font.Bold = false;
                htmlTextBoxPayer.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPayer.Value = "<strong>Payer</strong>";

                htmlTextBoxPayerColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.200000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape1.Bottom.Value + 0.1D));
                htmlTextBoxPayerColon.Name = "htmlTextBoxPayerColon" + 1;
                htmlTextBoxPayerColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPayerColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPayerColon.Style.Font.Bold = false;
                htmlTextBoxPayerColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPayerColon.Value = "<strong>:</strong>" + "  " + objOrganizationDetails[0].LastName + " " + objOrganizationDetails[0].FirstName;//PayerName

                htmlTextBoxName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPayerColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxName.Name = "htmlTextBoxName" + 1;
                htmlTextBoxName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxName.Style.Font.Bold = false;
                htmlTextBoxName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxName.Value = "<strong>Name</strong>";

                htmlTextBoxNameColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.200000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPayerColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxNameColon.Name = "htmlTextBoxNameColon" + 1;
                htmlTextBoxNameColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxNameColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxNameColon.Style.Font.Bold = false;
                htmlTextBoxNameColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxNameColon.Value = "<strong>:</strong>" + "  " + objOrganizationDetails[0].LastName + " " + objOrganizationDetails[0].FirstName;// MemberName

                htmlTextBoxDOB.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.700000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPayerColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxDOB.Name = "htmlTextBoxDOB" + 1;
                htmlTextBoxDOB.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxDOB.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxDOB.Style.Font.Bold = false;
                htmlTextBoxDOB.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxDOB.Value = "<strong>DOB</strong>";

                htmlTextBoxDOBColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPayerColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxDOBColon.Name = "htmlTextBoxDOBColon" + 1;
                htmlTextBoxDOBColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxDOBColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxDOBColon.Style.Font.Bold = false;
                htmlTextBoxDOBColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxDOBColon.Value = "<strong>:</strong>" + "  " + objOrganizationDetails[0].DOB;//DateOfBirth

                htmlTextBoxGender.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxGender.Name = "htmlTextBoxGender" + 1;
                htmlTextBoxGender.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxGender.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxGender.Style.Font.Bold = false;
                htmlTextBoxGender.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxGender.Value = "<strong>Gender</strong>";

                htmlTextBoxPatientGenderColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.200000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPatientGenderColon.Name = "htmlTextBoxPatientGenderColon" + 1;
                htmlTextBoxPatientGenderColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPatientGenderColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPatientGenderColon.Style.Font.Bold = false;
                htmlTextBoxPatientGenderColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPatientGenderColon.Value = "<strong>:</strong>" + "  " + objOrganizationDetails[0].Gender;//Gender

                htmlTextBoxEmail.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.700000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxEmail.Name = "htmlTextBoxEmail" + 1;
                htmlTextBoxEmail.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxEmail.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxEmail.Style.Font.Bold = false;
                htmlTextBoxEmail.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxEmail.Value = "<strong>Email</strong>";

                htmlTextBoxEmailColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxEmailColon.Name = "htmlTextBoxEmailColon" + 1;
                htmlTextBoxEmailColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxEmailColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxEmailColon.Style.Font.Bold = false;
                htmlTextBoxEmailColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxEmailColon.Value = "<strong>:</strong>" + "  " + objOrganizationDetails[0].Email;//Email

                htmlTextBoxAddress.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPatientGenderColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxAddress.Name = "htmlTextBoxAddress" + 1;
                htmlTextBoxAddress.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxAddress.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxAddress.Style.Font.Bold = false;
                htmlTextBoxAddress.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxAddress.Value = "<strong>Address</strong>";

                htmlTextBoxAddressColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.200000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPatientGenderColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxAddressColon.Name = "htmlTextBoxAddressColon" + 1;
                htmlTextBoxAddressColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxAddressColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxAddressColon.Style.Font.Bold = false;
                htmlTextBoxAddressColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxAddressColon.Value = "<strong>:</strong>" + "  " + objOrganizationDetails[0].Address;//Address

                htmlTextBoxCity.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.700000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPatientGenderColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxCity.Name = "htmlTextBoxCity" + 1;
                htmlTextBoxCity.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxCity.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxCity.Style.Font.Bold = false;
                htmlTextBoxCity.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxCity.Value = "<strong>City</strong>";

                htmlTextBoxCityColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPatientGenderColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxCityColon.Name = "htmlTextBoxCityColon" + 1;
                htmlTextBoxCityColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxCityColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxCityColon.Style.Font.Bold = false;
                htmlTextBoxCityColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxCityColon.Value = "<strong>:</strong>" + "  " + objOrganizationDetails[0].CityName;//City

                htmlTextState.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxAddressColon.Bottom.Value + 0.0120028698444367D));
                htmlTextState.Name = "htmlTextState" + 1;
                htmlTextState.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextState.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextState.Style.Font.Bold = false;
                htmlTextState.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextState.Value = "<strong>State</strong>";

                htmlTextBoxStateColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.200000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxAddressColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxStateColon.Name = "htmlTextBoxStateColon" + 1;
                htmlTextBoxStateColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxStateColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxStateColon.Style.Font.Bold = false;
                htmlTextBoxStateColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxStateColon.Value = "<strong>:</strong>" + "  " + objOrganizationDetails[0].StateName;//State

                htmlTextBoxZip.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.700000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxAddressColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxZip.Name = "htmlTextBoxZip" + 1;
                htmlTextBoxZip.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxZip.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxZip.Style.Font.Bold = false;
                htmlTextBoxZip.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxZip.Value = "<strong>Zip</strong>";

                htmlTextBoxZipColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxAddressColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxZipColon.Name = "htmlTextBoxZipColon" + 1;
                htmlTextBoxZipColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxZipColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxZipColon.Style.Font.Bold = false;
                htmlTextBoxZipColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxZipColon.Value = "<strong>:</strong>" + "  " + objOrganizationDetails[0].Zip;//Zip

                detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                      htmlTextBoxPayer,
                      htmlTextBoxPayerColon,
                      htmlTextBoxSubHeading1,
                      htmlTextBoxSubHeadingShape1,
                      htmlTextBoxSubHeadingShape3,
                      htmlTextBoxName,
                      htmlTextBoxNameColon,
                      htmlTextBoxDOB,
                      htmlTextBoxDOBColon,
                      htmlTextBoxGender,
                      htmlTextBoxPatientGenderColon,
                      htmlTextBoxEmail,
                      htmlTextBoxEmailColon,
                      htmlTextBoxAddress,
                      htmlTextBoxAddressColon,
                      htmlTextBoxCity,
                      htmlTextBoxCityColon,
                      htmlTextState,
                      htmlTextBoxStateColon,
                      htmlTextBoxZip,
                      htmlTextBoxZipColon,
                  });
                #endregion

                #region Member_Plan_Details
                Telerik.Reporting.HtmlTextBox htmlTextBoxSubHeading2 = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxSubHeading2 = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.Shape htmlTextBoxSubHeadingShape2 = new Telerik.Reporting.Shape();
                htmlTextBoxSubHeadingShape2 = new Telerik.Reporting.Shape();
                Telerik.Reporting.Shape htmlTextBoxSubHeadingShape4 = new Telerik.Reporting.Shape();
                htmlTextBoxSubHeadingShape4 = new Telerik.Reporting.Shape();

                Telerik.Reporting.HtmlTextBox htmlTextBoxProviderName = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxProviderName = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxProviderNameColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxProviderNameColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxPlanName = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPlanName = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxPlanNameColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPlanNameColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxPlanAmount = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPlanAmount = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxPlanAmountColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPlanAmountColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxEnrollFee = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxEnrollFee = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxEnrollFeeColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxEnrollFeeColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxPaymentSchdule = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPaymentSchdule = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxPaymentSchduleColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPaymentSchduleColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxPlanDuration = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPlanDuration = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxPlanDurationColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPlanDurationColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextPlanPeriod = new Telerik.Reporting.HtmlTextBox();
                htmlTextPlanPeriod = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxPlanPeriodColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPlanPeriodColon = new Telerik.Reporting.HtmlTextBox();

                Telerik.Reporting.HtmlTextBox htmlTextBoxPlanStatus = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPlanStatus = new Telerik.Reporting.HtmlTextBox();
                Telerik.Reporting.HtmlTextBox htmlTextBoxPlanStatusColon = new Telerik.Reporting.HtmlTextBox();
                htmlTextBoxPlanStatusColon = new Telerik.Reporting.HtmlTextBox();

                DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.OrganizationPlansDetails> objcallOrganizationPlanDetails = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.OrganizationPlansDetails>();
                ServiceData ServiceData1 = new ServiceData();
                string[] ParameterName1 = new string[] { "strOrganizationPlanCode" };
                string[] ParameterValue1 = new string[] { PlanID };
                ServiceData.ParameterName = ParameterName1;
                ServiceData.ParameterValue = ParameterValue1;
                ServiceData.WebMethodName = "GetOrganizationPlanDetails";

                List<PPCP07302018.Models.Organization.OrganizationPlansDetails> objOrganizationPlanDetails = objcallOrganizationPlanDetails.CallService(Convert.ToInt32(0), "GetOrganizationMemberDetails", ServiceData);

                htmlTextBoxSubHeadingShape4.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxZipColon.Bottom.Value + 0.2D));
                htmlTextBoxSubHeadingShape4.Name = "htmlTextBoxSubHeadingShape4";
                htmlTextBoxSubHeadingShape4.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                htmlTextBoxSubHeadingShape4.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                htmlTextBoxSubHeadingShape4.Stretch = true;
                htmlTextBoxSubHeadingShape4.Style.Font.Bold = true;
                htmlTextBoxSubHeadingShape4.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.8D);
                htmlTextBoxSubHeadingShape4.Visible = true;

                htmlTextBoxSubHeading2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.7D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape4.Bottom.Value));
                htmlTextBoxSubHeading2.Name = "htmlTextBoxSubHeading2" + 1;
                htmlTextBoxSubHeading2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxSubHeading2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
                htmlTextBoxSubHeading2.Style.Font.Bold = false;
                htmlTextBoxSubHeading2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                htmlTextBoxSubHeading2.Value = "<strong>Member Plan Details</strong>";

                htmlTextBoxSubHeadingShape2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeading2.Bottom.Value));
                htmlTextBoxSubHeadingShape2.Name = "htmlTextBoxSubHeadingShape2";
                htmlTextBoxSubHeadingShape2.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                htmlTextBoxSubHeadingShape2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                htmlTextBoxSubHeadingShape2.Stretch = true;
                htmlTextBoxSubHeadingShape2.Style.Font.Bold = false;
                htmlTextBoxSubHeadingShape2.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.4D);
                htmlTextBoxSubHeadingShape2.Visible = true;

                htmlTextBoxProviderName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape2.Bottom.Value + 0.1D));
                htmlTextBoxProviderName.Name = "htmlTextBoxProviderName" + 1;
                htmlTextBoxProviderName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxProviderName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                htmlTextBoxProviderName.Style.Font.Bold = false;
                htmlTextBoxProviderName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxProviderName.Value = "<strong>Provider Name</strong>";

                htmlTextBoxProviderNameColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.3D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape2.Bottom.Value + 0.1D));
                htmlTextBoxProviderNameColon.Name = "htmlTextBoxProviderNameColon" + 1;
                htmlTextBoxProviderNameColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxProviderNameColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxProviderNameColon.Style.Font.Bold = false;
                htmlTextBoxProviderNameColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxProviderNameColon.Value = "<strong>:</strong>" + "  " + objOrganizationPlanDetails[0].ProviderName;//ProviderName

                htmlTextBoxPlanName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.700000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape2.Bottom.Value + 0.1D));
                htmlTextBoxPlanName.Name = "htmlTextBoxPlanName" + 1;
                htmlTextBoxPlanName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanName.Style.Font.Bold = false;
                htmlTextBoxPlanName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanName.Value = "<strong>Plan Name</strong>";

                htmlTextBoxPlanNameColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape2.Bottom.Value + 0.1D));
                htmlTextBoxPlanNameColon.Name = "htmlTextBoxPlanNameColon" + 1;
                htmlTextBoxPlanNameColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanNameColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanNameColon.Style.Font.Bold = false;
                htmlTextBoxPlanNameColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanNameColon.Value = "<strong>:</strong>" + "  " + objOrganizationPlanDetails[0].PlanName;//PlanName

                htmlTextBoxPlanAmount.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanAmount.Name = "htmlTextBoxPlanAmount" + 1;
                htmlTextBoxPlanAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanAmount.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanAmount.Style.Font.Bold = false;
                htmlTextBoxPlanAmount.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanAmount.Value = "<strong>Plan Amount</strong>";

                htmlTextBoxPlanAmountColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.3D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanAmountColon.Name = "htmlTextBoxPlanAmountColon" + 1;
                htmlTextBoxPlanAmountColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanAmountColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanAmountColon.Style.Font.Bold = false;
                htmlTextBoxPlanAmountColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanAmountColon.Value = "<strong>:</strong>" + "  " + "$ " + objOrganizationPlanDetails[0].TotalAmount;//PlanAmount

                htmlTextBoxEnrollFee.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.700000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxEnrollFee.Name = "htmlTextBoxEnrollFee" + 1;
                htmlTextBoxEnrollFee.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxEnrollFee.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxEnrollFee.Style.Font.Bold = false;
                htmlTextBoxEnrollFee.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxEnrollFee.Value = "<strong>Enrollment Fee</strong>";

                htmlTextBoxEnrollFeeColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxEnrollFeeColon.Name = "htmlTextBoxEnrollFeeColon" + 1;
                htmlTextBoxEnrollFeeColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxEnrollFeeColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxEnrollFeeColon.Style.Font.Bold = false;
                htmlTextBoxEnrollFeeColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxEnrollFeeColon.Value = "<strong>:</strong>" + "  " + "$ " + objOrganizationPlanDetails[0].EnrollFee;//EnrollFee

                htmlTextBoxPaymentSchdule.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxEnrollFeeColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPaymentSchdule.Name = "htmlTextBoxPaymentSchdule" + 1;
                htmlTextBoxPaymentSchdule.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPaymentSchdule.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPaymentSchdule.Style.Font.Bold = false;
                htmlTextBoxPaymentSchdule.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPaymentSchdule.Value = "<strong>Payment Schedule</strong>";

                htmlTextBoxPaymentSchduleColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.3D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxEnrollFeeColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPaymentSchduleColon.Name = "htmlTextBoxPaymentSchduleColon" + 1;
                htmlTextBoxPaymentSchduleColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPaymentSchduleColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPaymentSchduleColon.Style.Font.Bold = false;
                htmlTextBoxPaymentSchduleColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPaymentSchduleColon.Value = "<strong>:</strong>" + "  " + objOrganizationPlanDetails[0].Paymentschedule;//PaymentSchedule

                htmlTextBoxPlanDuration.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.700000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxEnrollFeeColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanDuration.Name = "htmlTextBoxPlanDuration" + 1;
                htmlTextBoxPlanDuration.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanDuration.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanDuration.Style.Font.Bold = false;
                htmlTextBoxPlanDuration.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanDuration.Value = "<strong>Plan Duration</strong>";

                htmlTextBoxPlanDurationColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxEnrollFeeColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanDurationColon.Name = "htmlTextBoxPlanDurationColon" + 1;
                htmlTextBoxPlanDurationColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanDurationColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanDurationColon.Style.Font.Bold = false;
                htmlTextBoxPlanDurationColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanDurationColon.Value = "<strong>:</strong>" + "  " + objOrganizationPlanDetails[0].Duration;//PlanDuration

                htmlTextPlanPeriod.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanDurationColon.Bottom.Value + 0.0120028698444367D));
                htmlTextPlanPeriod.Name = "htmlTextPlanPeriod" + 1;
                htmlTextPlanPeriod.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextPlanPeriod.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextPlanPeriod.Style.Font.Bold = false;
                htmlTextPlanPeriod.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextPlanPeriod.Value = "<strong>Plan Period</strong>";

                htmlTextBoxPlanPeriodColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.3D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanDurationColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanPeriodColon.Name = "htmlTextBoxPlanPeriodColon" + 1;
                htmlTextBoxPlanPeriodColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanPeriodColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanPeriodColon.Style.Font.Bold = false;
                htmlTextBoxPlanPeriodColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanPeriodColon.Value = "<strong>:</strong>" + "  " + objOrganizationPlanDetails[0].PlanTermName;//PlanPeriod

                htmlTextBoxPlanStatus.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.700000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanDurationColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanStatus.Name = "htmlTextBoxPlanStatus" + 1;
                htmlTextBoxPlanStatus.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanStatus.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanStatus.Style.Font.Bold = false;
                htmlTextBoxPlanStatus.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanStatus.Value = "<strong>Plan Status</strong>";

                htmlTextBoxPlanStatusColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanDurationColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanStatusColon.Name = "htmlTextBoxPlanStatusColon" + 1;
                htmlTextBoxPlanStatusColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanStatusColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanStatusColon.Style.Font.Bold = false;
                htmlTextBoxPlanStatusColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanStatusColon.Value = "<strong>:</strong>" + "  " + objOrganizationPlanDetails[0].PlanStatus;//PlanStatus

                detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                      htmlTextBoxSubHeadingShape4,
                      htmlTextBoxSubHeading2,
                      htmlTextBoxSubHeadingShape2,
                      htmlTextBoxProviderName,
                      htmlTextBoxProviderNameColon,
                      htmlTextBoxPlanName,
                      htmlTextBoxPlanNameColon,
                      htmlTextBoxPlanAmount,
                      htmlTextBoxPlanAmountColon,
                      htmlTextBoxEnrollFee,
                      htmlTextBoxEnrollFeeColon,
                      htmlTextBoxPaymentSchdule,
                      htmlTextBoxPaymentSchduleColon,
                      htmlTextBoxPlanDuration,
                      htmlTextBoxPlanDurationColon,
                      htmlTextPlanPeriod,
                      htmlTextBoxPlanPeriodColon,
                      htmlTextBoxPlanStatus,
                      htmlTextBoxPlanStatusColon,
                  });
                #endregion

                #region Report_Body
                double bottom = 0.0;


                DataTable dt = ToDataTable(objOrganizationPlanDetails);
                // Get Report Details
                // DataTable dt = blobj.GetMemberPlanPaymentDetails(Member_Id, Member_Plan_Id, FromDate, ToDate);
                // DataTable dt = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    Telerik.Reporting.Table tablej = new Telerik.Reporting.Table();
                    tablej = new Telerik.Reporting.Table();
                    Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();

                    Telerik.Reporting.HtmlTextBox htmlTextBoxSubHeading3 = new Telerik.Reporting.HtmlTextBox();
                    htmlTextBoxSubHeading3 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.Shape htmlTextBoxSubHeadingShape5 = new Telerik.Reporting.Shape();
                    htmlTextBoxSubHeadingShape5 = new Telerik.Reporting.Shape();
                    Telerik.Reporting.Shape htmlTextBoxSubHeadingShape6 = new Telerik.Reporting.Shape();
                    htmlTextBoxSubHeadingShape6 = new Telerik.Reporting.Shape();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxStar = new Telerik.Reporting.HtmlTextBox();
                    htmlTextBoxStar = new Telerik.Reporting.HtmlTextBox();

                    Telerik.Reporting.HtmlTextBox htmlTextBox11j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox12j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox13j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox14j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox15j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox16j = new Telerik.Reporting.HtmlTextBox();

                    Telerik.Reporting.HtmlTextBox htmlTextBoxTotPaidAmt = new Telerik.Reporting.HtmlTextBox();

                    Telerik.Reporting.HtmlTextBox htmlTextBox11 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox12 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox13 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox14 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox15 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox16 = new Telerik.Reporting.HtmlTextBox();

                    Telerik.Reporting.TableGroup tableGroup1jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup2jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup3jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup4jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup5jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup6jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup7jj = new Telerik.Reporting.TableGroup();

                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.8000005960464478D)));
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.85000065565109253D)));
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.85000065565109253D)));
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.85000065565109253D)));
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.85000065565109253D)));
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.85000065565109253D)));

                    tablej.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.5D)));

                    tablej.Body.SetCellContent(0, 0, htmlTextBox11);
                    tablej.Body.SetCellContent(0, 1, htmlTextBox12);
                    tablej.Body.SetCellContent(0, 2, htmlTextBox13);
                    tablej.Body.SetCellContent(0, 3, htmlTextBox14);
                    tablej.Body.SetCellContent(0, 4, htmlTextBox15);
                    tablej.Body.SetCellContent(0, 5, htmlTextBox16);

                    tableGroup1jj.Name = "tableGroup1";
                    tableGroup1jj.ReportItem = htmlTextBox11j;
                    tableGroup2jj.Name = "tableGroup2";
                    tableGroup2jj.ReportItem = htmlTextBox12j;
                    tableGroup3jj.Name = "tableGroup3";
                    tableGroup3jj.ReportItem = htmlTextBox13j;
                    tableGroup4jj.Name = "tableGroup4";
                    tableGroup4jj.ReportItem = htmlTextBox14j;
                    tableGroup5jj.Name = "tableGroup5";
                    tableGroup5jj.ReportItem = htmlTextBox15j;
                    tableGroup6jj.Name = "tableGroup6";
                    tableGroup6jj.ReportItem = htmlTextBox16j;

                    tablej.ColumnGroups.Add(tableGroup1jj);
                    tablej.ColumnGroups.Add(tableGroup2jj);
                    tablej.ColumnGroups.Add(tableGroup3jj);
                    tablej.ColumnGroups.Add(tableGroup4jj);
                    tablej.ColumnGroups.Add(tableGroup5jj);
                    tablej.ColumnGroups.Add(tableGroup6jj);

                    tablej.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                              htmlTextBox11,
                              htmlTextBox12,
                              htmlTextBox13,
                              htmlTextBox14,
                              htmlTextBox15,
                              htmlTextBox16,

                              htmlTextBox11j,
                              htmlTextBox12j,
                              htmlTextBox13j,
                              htmlTextBox14j,
                              htmlTextBox15j,
                              htmlTextBox16j,
                      });

                    htmlTextBoxSubHeadingShape5.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanStatusColon.Bottom.Value + 0.2D));
                    htmlTextBoxSubHeadingShape5.Name = "htmlTextBoxSubHeadingShape5";
                    htmlTextBoxSubHeadingShape5.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                    htmlTextBoxSubHeadingShape5.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                    htmlTextBoxSubHeadingShape5.Stretch = true;
                    htmlTextBoxSubHeadingShape5.Style.Font.Bold = true;
                    htmlTextBoxSubHeadingShape5.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.8D);
                    htmlTextBoxSubHeadingShape5.Visible = true;

                    htmlTextBoxSubHeading3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.7D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape5.Bottom.Value));
                    htmlTextBoxSubHeading3.Name = "htmlTextBoxSubHeading3" + 1;
                    htmlTextBoxSubHeading3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                    htmlTextBoxSubHeading3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(11D);
                    htmlTextBoxSubHeading3.Style.Font.Bold = false;
                    htmlTextBoxSubHeading3.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                    htmlTextBoxSubHeading3.Value = "<strong>Plan Payments</strong>";

                    htmlTextBoxSubHeadingShape6.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3.3D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeading3.Bottom.Value));
                    htmlTextBoxSubHeadingShape6.Name = "htmlTextBoxSubHeadingShape6";
                    htmlTextBoxSubHeadingShape6.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                    htmlTextBoxSubHeadingShape6.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.6D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                    htmlTextBoxSubHeadingShape6.Stretch = true;
                    htmlTextBoxSubHeadingShape6.Style.Font.Bold = false;
                    htmlTextBoxSubHeadingShape6.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.4D);
                    htmlTextBoxSubHeadingShape6.Visible = true;

                    tablej.Name = "tablej";
                    tableGroup7jj.Groupings.Add(new Telerik.Reporting.Grouping(null));
                    tableGroup7jj.Name = "detailTableGroup";
                    tablej.RowGroups.Add(tableGroup7jj);
                    tablej.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(6.7500009536743164D), Telerik.Reporting.Drawing.Unit.Inch(1D));
                    tablej.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    tablej.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    objectDataSource.DataMember = "GetPaymentsView";
                    objectDataSource.DataSource = dt;
                    objectDataSource.Name = "objectDataSource";
                    tablej.DataSource = objectDataSource;

                    tablej.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape6.Bottom.Value + 0.1D));
                    bottom = tablej.Bottom.Value;

                    htmlTextBox11.Name = "htmlTextBox11";
                    htmlTextBox11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox11.StyleName = "";
                    htmlTextBox11.Value = "$ " + "{Fields.TotalAmount}";
                    htmlTextBox11.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox11j.Name = "htmlTextBox11j";
                    htmlTextBox11j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox11j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox11j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox11j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox11j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox11j.StyleName = "";
                    htmlTextBox11j.Value = "<strong>Plan Amount</strong>";
                    htmlTextBox11j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox12.Name = "htmlTextBox12";
                    htmlTextBox12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox12.StyleName = "";
                    htmlTextBox12.Value = "$ " + "{Fields.AmountPaid}";
                    htmlTextBox12.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox12j.Name = "htmlTextBox12j";
                    htmlTextBox12j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox12j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox12j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox12j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox12j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox12j.StyleName = "";
                    htmlTextBox12j.Value = "<strong>Paid Amount</strong>";
                    htmlTextBox12j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox13.Name = "htmlTextBox13";
                    htmlTextBox13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox13.StyleName = "";
                    htmlTextBox13.Value = "$ " + "{Fields.DueAmount}";
                    htmlTextBox13.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox13j.Name = "htmlTextBox13j";
                    htmlTextBox13j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox13j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox13j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox13j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox13j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox13j.StyleName = "";
                    htmlTextBox13j.Value = "<strong>Due Amount</strong>";
                    htmlTextBox13j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox14.Name = "htmlTextBox14";
                    htmlTextBox14.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox14.StyleName = "";
                    htmlTextBox14.Value = "";//"{Fields.INSTALLMENT_DATE}&nbsp;&nbsp;"
                    htmlTextBox14.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox14j.Name = "htmlTextBox14j";
                    htmlTextBox14j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox14j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox14j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox14j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox14j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox14j.StyleName = "";
                    htmlTextBox14j.Value = "<strong>Month-Year&nbsp;&nbsp;</strong>";
                    htmlTextBox14j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox15.Name = "htmlTextBox15";
                    htmlTextBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox15.StyleName = "";
                    htmlTextBox15.Value = "";//"&nbsp;&nbsp;{Fields.PAYMENT_DATE}";
                    htmlTextBox15.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox15j.Name = "htmlTextBox15j";
                    htmlTextBox15j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox15j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox15j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox15j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox15j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox15j.StyleName = "";
                    htmlTextBox15j.Value = "<strong>Date of Payment (MM/dd/yyyy)</strong>";
                    htmlTextBox15j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox16.Name = "htmlTextBox16";
                    htmlTextBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox16.StyleName = "";
                    htmlTextBox16.Value = "";//"&nbsp;&nbsp;{Fields.MODE_OF_PAY}"
                    htmlTextBox16.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox16j.Name = "htmlTextBox16j";
                    htmlTextBox16j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox16j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox16j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox16j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox16j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox16j.StyleName = "";
                    htmlTextBox16j.Value = "<strong>&nbsp;&nbsp;Payment Mode</strong>";
                    htmlTextBox16j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBoxTotPaidAmt.Name = "htmlTextBoxTotPaidAmt";
                    htmlTextBoxTotPaidAmt.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.0874997615814209D), Telerik.Reporting.Drawing.Unit.Inch(0.18749986588954926D));
                    htmlTextBoxTotPaidAmt.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBoxTotPaidAmt.StyleName = "";
                    object sumObject;
                    //     var result1 = dt.AsEnumerable().Sum(x => Convert.ToInt32(x["AmountPaid"]));
                    //  sumObject=dt.Compute("Sum(Convert([AmountPaid], 'System.Int32'))", "[AmountPaid] IS NOT NULL");
                    //sumObject = dt.Compute("Sum(Convert(" + AmountPaid + ", 'System.Int32')", "");
                    //  sumObject = dt.Compute("Sum(AmountPaid)", "");
                    htmlTextBoxTotPaidAmt.Value = "<strong>Total Paid Amount:- </strong>" + " $ " + "";
                    htmlTextBoxTotPaidAmt.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(bottom + 0.05));
                    htmlTextBoxTotPaidAmt.Visible = false;

                    htmlTextBoxStar.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(bottom + 1D));
                    htmlTextBoxStar.Name = "htmlTextBoxStar";
                    htmlTextBoxStar.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.6D), Telerik.Reporting.Drawing.Unit.Inch(2.4D));
                    htmlTextBoxStar.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8D);
                    htmlTextBoxStar.StyleName = "";
                    htmlTextBoxStar.Value = "<strong>* * * * END OF REPORT * * * *</strong>";
                    htmlTextBoxStar.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;

                    detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                          htmlTextBoxSubHeadingShape5,
                          htmlTextBoxSubHeading3,
                          htmlTextBoxSubHeadingShape6,
                          tablej,
                          htmlTextBoxTotPaidAmt,
                          htmlTextBoxStar,
                      });
                    detail.Height = Telerik.Reporting.Drawing.Unit.Inch(6.88199608039855957D);
                    detail.PageBreak = Telerik.Reporting.PageBreak.None;
                    detail.Name = "Plan Payment Summary";
                }
                else
                {
                    Telerik.Reporting.HtmlTextBox htmlTextErrorMessageDisplay = new Telerik.Reporting.HtmlTextBox();
                    htmlTextErrorMessageDisplay = new Telerik.Reporting.HtmlTextBox();
                    htmlTextErrorMessageDisplay.Style.Color = System.Drawing.Color.Black;
                    htmlTextErrorMessageDisplay.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.6D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanStatusColon.Bottom.Value + 1.2D));
                    htmlTextErrorMessageDisplay.Name = "htmlTextBoxErrorMessage";
                    htmlTextErrorMessageDisplay.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18749986588954926D));
                    htmlTextErrorMessageDisplay.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.5D);
                    htmlTextErrorMessageDisplay.StyleName = "";
                    htmlTextErrorMessageDisplay.Value = "<strong>No payment has been done yet for this plan.</strong>";
                    detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                      htmlTextErrorMessageDisplay,
                      });
                }
                #endregion

                #region Footer
                Telerik.Reporting.TextBox txtGeneratedby = new Telerik.Reporting.TextBox();
                Telerik.Reporting.TextBox txtGeneratedColon = new Telerik.Reporting.TextBox();
                Telerik.Reporting.TextBox txtGeneratedByValue = new Telerik.Reporting.TextBox();
                Telerik.Reporting.TextBox txtGeneratedDatefooter = new Telerik.Reporting.TextBox();
                Telerik.Reporting.TextBox txtGeneratedDateColon = new Telerik.Reporting.TextBox();
                Telerik.Reporting.TextBox txtGenerateddateValue = new Telerik.Reporting.TextBox();
                Telerik.Reporting.TextBox PageNumbers = new Telerik.Reporting.TextBox();
                Telerik.Reporting.Shape shapeFooter = new Telerik.Reporting.Shape();
                shapeFooter = new Telerik.Reporting.Shape();
                txtGeneratedby = new Telerik.Reporting.TextBox();
                txtGeneratedColon = new Telerik.Reporting.TextBox();
                txtGeneratedByValue = new Telerik.Reporting.TextBox();
                txtGeneratedDatefooter = new Telerik.Reporting.TextBox();
                txtGeneratedDateColon = new Telerik.Reporting.TextBox();
                txtGenerateddateValue = new Telerik.Reporting.TextBox();
                txtGeneratedby.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.8400000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
                txtGeneratedby.Name = "Generated By";
                txtGeneratedby.Value = "Generated By";
                txtGeneratedby.Visible = false;
                txtGeneratedby.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtGeneratedColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.90761510848999D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
                txtGeneratedColon.Name = ":";
                txtGeneratedColon.Value = ":";
                txtGeneratedColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtGeneratedColon.Visible = false;
                txtGeneratedByValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.9600000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
                txtGeneratedByValue.Name = "Generated By";
                txtGeneratedByValue.Value = Convert.ToString(Session["ProviderName"]);
                txtGeneratedByValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtGeneratedByValue.Visible = false;
                txtGeneratedDatefooter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.8400000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
                txtGeneratedDatefooter.Name = "Generated Date";
                txtGeneratedDatefooter.Value = "Generated Date";
                txtGeneratedDatefooter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtGeneratedDateColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.90761510848999D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
                txtGeneratedDateColon.Name = ":";
                txtGeneratedDateColon.Value = ":";
                txtGeneratedDateColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtGenerateddateValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.9600000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
                txtGenerateddateValue.Name = "Generated Date";
                txtGenerateddateValue.Value = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
                txtGenerateddateValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));

                PageNumbers.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.5D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
                PageNumbers.Name = "PageNumbers";
                PageNumbers.Value = "Page { PageNumber} of {PageCount}";
                PageNumbers.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));

                shapeFooter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(txtGeneratedby.Top.Value - 0.15000000596046448D));
                shapeFooter.Name = "shapeFooter";
                shapeFooter.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                shapeFooter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.6082919692993164D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                shapeFooter.Stretch = true;
                shapeFooter.Style.Font.Bold = true;
                shapeFooter.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(1D);
                FooterSection1.PrintOnFirstPage = true;
                FooterSection1.PrintOnLastPage = true;
                FooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                                           txtGeneratedby ,txtGeneratedColon,txtGeneratedByValue,txtGeneratedDatefooter,txtGeneratedDateColon,txtGenerateddateValue,shapeFooter,PageNumbers,
                                          });
                FooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.02);
                FooterSection1.Name = "pageFooterSection1";
                #endregion

                rptirDCS.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { HeaderSection1, detail, FooterSection1 });
                rptirDCS.Name = "Plan Payment Summary";
                rptirDCS.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));//new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
                rptirDCS.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
                rptirDCS.Width = Telerik.Reporting.Drawing.Unit.Inch(6.887535572052002D);
                rptbook1.Reports.Add(rptirDCS);

                ReportProcessor reportProcessor = new ReportProcessor();
                RenderingResult result = reportProcessor.RenderReport("PDF", rptbook1, null);
                rptbook1.DocumentName = "Plan Payment Summary";
                string fileName = rptbook1.DocumentName + "." + result.Extension;
                Response.Clear();
                Response.ContentType = result.MimeType;
                Response.Cache.SetCacheability(HttpCacheability.Private);
                Response.Expires = -1;
                Response.Buffer = true;
                Response.AddHeader("Content-Disposition",
                                   string.Format("{0};FileName=\"{1}\"",
                                                 "attachment",

                            fileName));
                Response.BinaryWrite(result.DocumentBytes);
                if (Response.IsClientConnected)
                {
                    Response.End();

                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }


            //  ViewBag.report = rptbook1;

            return View();
        }
        /// <summary>
        /// Veena
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        ///// <summary>
        /////     To Get new Terms And conditions
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult OrganizationTermsAndConditions(Models.Organization.TermsAndConditions model)
        //{
        //    model.Organization=Convert.ToString(Session["OrganizationTandCFlag"]);
        //    model.OrganizationUsers= Convert.ToString(Session["OrganizationUserTandCFlag"] );         
        //    return View(model);
        //}

        public JsonResult MemberSearchAutoComplete(string Text)
        {
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.MemberAutoComplete> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.MemberAutoComplete>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "OrganizationID", "strMemberID", "searchtext" };

            string[] ParameterValue = new string[] { Convert.ToString(Session["OrganizationID"]), "0", Text };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetMembersList";
            List<PPCP07302018.Models.Admin.MemberAutoComplete> List = objcall.CallServices(Convert.ToInt32(0), "GetMembersList", ServiceData);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserSearchAutoComplete(string Text)
        {
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.OrganizationUserAutoComplete> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.OrganizationUserAutoComplete>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "OrganizationID", "Text" };

            string[] ParameterValue = new string[] { Convert.ToString(Session["OrganizationID"]), Text };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetUserDetailsAutoComplete";
            List<PPCP07302018.Models.Admin.OrganizationUserAutoComplete> List = objcall.CallServicesAdmin(Convert.ToInt32(0), "GetUserDetailsAutoComplete", ServiceData);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SessionOut()
        {
            return View();
        }

        public ActionResult PartialPayments(string MemberPlanID)
        {
            PPCP07302018.Models.Member.MakePayment memberplan = new PPCP07302018.Models.Member.MakePayment();
            memberplan.Plan_Code = Convert.ToInt32(MemberPlanID);
            TempData["TemPaymentDetails"] = null;
            return View(memberplan);

        }

        public ActionResult PendingPlanEnrollment(Models.Member.MemberDetails model)
        {
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
            return View(model);
        }

        public JsonResult SavePaymentDeatilstemp(PPCP07302018.Models.Member.MemberPlanInstallmentMapping model)
        {
            List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping> list = new List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping>();
            List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping> list1 = new List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping>();
            //if (checkedvalue == "1")
            //{
            if (TempData["TemPaymentDetails"] != null)
            {
                list = TempData["TemPaymentDetails"] as List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping>;
                var Getmember = list.Find(m => m.MemberPlanInstallmentID == model.MemberPlanInstallmentID);
                if (list.Any(m => m.MemberPlanInstallmentID == model.MemberPlanInstallmentID))
                {
                    list.Remove(Getmember);

                }
                else
                {
                    list.Add(model);
                }

                // var itemToRemove = list.Single(r => r.MemberPlanInstallmentID == model.MemberPlanInstallmentID);
                // list.Remove(itemToRemove);
                TempData["TemPaymentDetails"] = list as List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping>;
            }
            else
            {
                list.Add(model);
                TempData["TemPaymentDetails"] = list as List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping>;
            }
            // }

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string OrganizationMakePaymentxml(Models.Member.MakePayment modelParameter)
        {
            List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping> list = new List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping>();
            list = TempData["TemPaymentDetails"] as List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping>;
            // string xmldata = GetXMLFromObject(list);
            string objList = JsonConvert.SerializeObject(list);
            modelParameter.MemberPlanInstallmentMapping = objList;
            string xml = GetXMLFromObject(modelParameter);
            string returnData = xml.Replace("\"", "\'");
            return returnData;
        }
        public ActionResult PaymentsReport()
        {
            return View();
        }

       
        public JsonResult PaymentsReportSearch(string ToDate,string FromDate,string ProviderID,string ProviderName,string PlanType,
            string PaymentsStatus)
        {
            Telerik.Reporting.ReportBook rptbook1 = new Telerik.Reporting.ReportBook();
            Telerik.Reporting.PageHeaderSection HeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            Telerik.Reporting.PageFooterSection FooterSection1 = new Telerik.Reporting.PageFooterSection();
            Telerik.Reporting.Report rptirDCS = new Telerik.Reporting.Report();
            Telerik.Reporting.DetailSection detail = new Telerik.Reporting.DetailSection();
            #region Header
            Telerik.Reporting.Shape shape1 = new Telerik.Reporting.Shape();
            shape1 = new Telerik.Reporting.Shape();
            Telerik.Reporting.Shape shapeFooter = new Telerik.Reporting.Shape();
            shapeFooter = new Telerik.Reporting.Shape();
            Telerik.Reporting.HtmlTextBox txtHeading = new Telerik.Reporting.HtmlTextBox();
            txtHeading = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.HtmlTextBox FeatureDate = new Telerik.Reporting.HtmlTextBox();
            FeatureDate = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.HtmlTextBox OrganizationName = new Telerik.Reporting.HtmlTextBox();
            OrganizationName = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.HtmlTextBox DoctorName = new Telerik.Reporting.HtmlTextBox();
            DoctorName = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.PictureBox picLogo = new Telerik.Reporting.PictureBox();
            picLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.607537258118391037), Telerik.Reporting.Drawing.Unit.Inch(0.5));
            picLogo.MimeType = "";
            picLogo.Name = "pictureBox2";
            picLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.016246293783187866D), Telerik.Reporting.Drawing.Unit.Inch(0.69996066689491272D));
            picLogo.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            // picLogo.Value = LogoPath; //img;
            picLogo.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;

            txtHeading.Name = "Heading";
            txtHeading.Value = "<strong>Payments Detail Report</strong>";
            txtHeading.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.6D), Telerik.Reporting.Drawing.Unit.Pixel(20));
            txtHeading.Style.Font.Bold = true;
            txtHeading.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(14);
            txtHeading.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            txtHeading.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4D), Telerik.Reporting.Drawing.Unit.Inch(0.4D));
            FeatureDate.Name = "FeatureDate";
            FeatureDate.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            FeatureDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.6D), Telerik.Reporting.Drawing.Unit.Pixel(14));
            FeatureDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4D), Telerik.Reporting.Drawing.Unit.Inch(txtHeading.Bottom.Value));
            FeatureDate.Visible = false;
            shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.2D), Telerik.Reporting.Drawing.Unit.Inch(txtHeading.Bottom.Value));
            shape1.Name = "shape1";
            shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.35D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
            shape1.Stretch = true;
            shape1.Style.Font.Bold = true;
            shape1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(1D);
            shape1.Visible = true;
            HeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.7);
           // HeaderSection1.PrintOnFirstPage = true;
           // HeaderSection1.PrintOnLastPage = false;
            HeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
txtHeading,shape1,FeatureDate
});
            HeaderSection1.Name = "pageHeaderSection1";
            #endregion
            #region Report
            try
            {
                //string Fromdate = model.FromDate.ToString();
                //DateTime FromDate = Convert.ToDateTime(Fromdate);
                //string FromDate1 = FromDate.ToString("MM/dd/yyyy");
                //string ToDate = model.ToDate.ToString();
                //DateTime todate = Convert.ToDateTime(ToDate);
                //string ToDate1 = todate.ToString("MM/dd/yyyy");
                string OrganizationID = Session["OrganizationID"].ToString();
                if (PaymentsStatus == null || PaymentsStatus == ""|| PaymentsStatus == "All")
                {
                    PaymentsStatus = "";
                }
                if (PlanType == null || PlanType == "")
                {
                    PlanType = "";
                }
                if (ProviderID == null || ProviderID == "")
                {
                    ProviderID = "0";
                }
                DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.PPCPReports> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.PPCPReports>();
                // ServiceData ServiceData = new ServiceData();
                PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
                string[] ParameterName = new string[] { "FromDate", "ToDate", "ProviderID", "PaymentStatus", "PlanType", "OrganziationID", "Type" };//, "ProviderName"
                string[] ParameterValue = new string[] { FromDate, ToDate, Convert.ToString(ProviderID), PaymentsStatus, PlanType, OrganizationID, "2" };//,"" 
                ServiceData.ParameterName = ParameterName;
                ServiceData.ParameterValue = ParameterValue;
                ServiceData.WebMethodName = "GetPPCPReports";

                List<PPCP07302018.Models.Organization.PPCPReports> List = objcall.CallServices(Convert.ToInt32(0), "GetPPCPReports", ServiceData);



                double bottom = 0.0;
                if (List.Count > 0)
                {
                    #region Detailed
                    DataTable dt = ToDataTable(List);
                    DataTable dtNew = new DataTable();

                    Telerik.Reporting.Table tablej = new Telerik.Reporting.Table();
                    tablej = new Telerik.Reporting.Table();
                    Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();
                    Telerik.Reporting.HtmlTextBox htmlTextBox11j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox12j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox13j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox14j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox15j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox16j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox17j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox18j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox19j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox20j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox21j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox22j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox23j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxPaymentdateV = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxPaymentdateH = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxStatusV = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxStatusH = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxSummary = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxTotalAmount = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxNetAmount = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxCommissionFee = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox11 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox12 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox13 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox14 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox15 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox16 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.TextBox htmlTextBox17 = new Telerik.Reporting.TextBox();
                    Telerik.Reporting.TextBox htmlTextBox18 = new Telerik.Reporting.TextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox19 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox20 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox21 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox22 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox23 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.TableGroup tableGroup1jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup2jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup3jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup4jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup5jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup6jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup7jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup8jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup9jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup10jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup11jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup12jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup13jj = new Telerik.Reporting.TableGroup();
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.0000005960464478D))); //FirstName
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.0000005960464478D)));//Last Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.9000005960464478D)));//DOB
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.2000065565109253D))); //Plan name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//Start date
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//End date
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//Plan Fee
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//Amount paid
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//Payment date
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//Status

                    tablej.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.3D)));
                    tablej.Body.SetCellContent(0, 0, htmlTextBox11);
                    tablej.Body.SetCellContent(0, 1, htmlTextBox12);
                    tablej.Body.SetCellContent(0, 2, htmlTextBox13);
                    tablej.Body.SetCellContent(0, 3, htmlTextBox14);
                    tablej.Body.SetCellContent(0, 4, htmlTextBox15);
                    tablej.Body.SetCellContent(0, 5, htmlTextBox16);
                    tablej.Body.SetCellContent(0, 6, htmlTextBox17);
                    tablej.Body.SetCellContent(0, 7, htmlTextBox18);
                    tablej.Body.SetCellContent(0, 8, htmlTextBoxPaymentdateV);
                    tablej.Body.SetCellContent(0, 9, htmlTextBoxStatusV);

                    tableGroup1jj.Name = "tableGroup1";
                    tableGroup1jj.ReportItem = htmlTextBox11j;
                    tableGroup2jj.Name = "tableGroup2";
                    tableGroup2jj.ReportItem = htmlTextBox12j;
                    tableGroup3jj.Name = "tableGroup3";
                    tableGroup3jj.ReportItem = htmlTextBox13j;
                    tableGroup4jj.Name = "tableGroup4";
                    tableGroup4jj.ReportItem = htmlTextBox14j;
                    tableGroup5jj.Name = "tableGroup13";
                    tableGroup5jj.ReportItem = htmlTextBox15j;
                    tableGroup6jj.Name = "tableGroup5";
                    tableGroup6jj.ReportItem = htmlTextBox16j;
                    tableGroup7jj.Name = "tableGroup6";
                    tableGroup7jj.ReportItem = htmlTextBox17j;
                    tableGroup8jj.Name = "tableGroup7";
                    tableGroup8jj.ReportItem = htmlTextBox18j;
                    tableGroup9jj.Name = "tableGroup8";
                    tableGroup9jj.ReportItem = htmlTextBoxPaymentdateH;
                    tableGroup10jj.Name = "tableGroup9";
                    tableGroup10jj.ReportItem = htmlTextBoxStatusH;

                    tablej.ColumnGroups.Add(tableGroup1jj);
                    tablej.ColumnGroups.Add(tableGroup2jj);
                    tablej.ColumnGroups.Add(tableGroup3jj);
                    tablej.ColumnGroups.Add(tableGroup4jj);
                    tablej.ColumnGroups.Add(tableGroup5jj);
                    tablej.ColumnGroups.Add(tableGroup6jj);
                    tablej.ColumnGroups.Add(tableGroup7jj);
                    tablej.ColumnGroups.Add(tableGroup8jj);
                    tablej.ColumnGroups.Add(tableGroup9jj);
                    tablej.ColumnGroups.Add(tableGroup10jj);
                    tablej.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
htmlTextBox11,
htmlTextBox12,
htmlTextBox13,
htmlTextBox14,
htmlTextBox15,
htmlTextBox16,
htmlTextBox17,
htmlTextBox18,
htmlTextBox11j,
htmlTextBox12j,
htmlTextBox13j,
htmlTextBox14j,
htmlTextBox15j,
htmlTextBox16j,
htmlTextBox17j,
htmlTextBox18j,
htmlTextBoxPaymentdateV,
htmlTextBoxPaymentdateH,
htmlTextBoxStatusV,
htmlTextBoxStatusH
});
                    tablej.Name = "tablej";
                    tableGroup12jj.Groupings.Add(new Telerik.Reporting.Grouping(null));
                    tableGroup12jj.Name = "detailTableGroup";
                    tablej.RowGroups.Add(tableGroup12jj);
                    tablej.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.5500009536743164D), Telerik.Reporting.Drawing.Unit.Inch(1D));
                    tablej.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    tablej.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    objectDataSource.DataMember = "GetPaymentsView";
                    objectDataSource.DataSource = dt;
                    objectDataSource.Name = "objectDataSource";
                    tablej.DataSource = objectDataSource;
                    tablej.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.2D), Telerik.Reporting.Drawing.Unit.Inch(bottom + 0.1));
                    tablej.KeepTogether = false;
                    bottom = tablej.Bottom.Value;
                    htmlTextBox11.Name = "htmlTextBox11";
                    htmlTextBox11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox11.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox11.StyleName = "";
                    htmlTextBox11.Value = "&nbsp;{Fields.LastName}";
                    htmlTextBox11.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox11.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox11j.Name = "htmlTextBoxValue1";
                    htmlTextBox11j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox11j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox11j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox11j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox11j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox11j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox11j.StyleName = "";
                    htmlTextBox11j.Value = "<strong>Last Name</strong>";
                    htmlTextBox11j.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox11j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox12.Name = "htmlTextBox12";
                    htmlTextBox12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox12.StyleName = "";
                    htmlTextBox12.Value = "{Fields.FirstName}";
                    htmlTextBox12.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox12.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox12j.Name = "htmlTextBoxValue2";
                    htmlTextBox12j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox12j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox12j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox12j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox12j.StyleName = "";
                    htmlTextBox12j.Value = "<strong>First Name</strong>";
                    htmlTextBox12j.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox12j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox13.Name = "htmlTextBox13";
                    htmlTextBox13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.9D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox13.StyleName = "";
                    htmlTextBox13.Value = "&nbsp;{Fields.DOB}";
                    htmlTextBox13.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox13.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox13j.Name = "htmlTextBoxValue3";
                    htmlTextBox13j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox13j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox13j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox13j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.9D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox13j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox13j.StyleName = "";
                    htmlTextBox13j.Value = "<strong>&nbsp;DOB</strong>";
                    htmlTextBox13j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox14.Name = "htmlTextBox14";
                    htmlTextBox14.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox14.StyleName = "";
                    htmlTextBox14.Value = "{Fields.PlanName}";
                    htmlTextBox14.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox14.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox14j.Name = "htmlTextBoxValue4";
                    htmlTextBox14j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox14j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox14j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox14j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox14j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox14j.StyleName = "";
                    htmlTextBox14j.Value = "<strong>&nbsp;Plan Name</strong>";
                    htmlTextBox14j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox15.Name = "htmlTextBox22";
                    htmlTextBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox15.StyleName = "";
                    htmlTextBox15.Value = "{Fields.PlanStartDate}";
                    htmlTextBox15.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox15.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox15j.Name = "htmlTextBox15j";
                    htmlTextBox15j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox15j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox15j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox15j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox15j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox15j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox15j.StyleName = "";
                    htmlTextBox15j.Value = "<strong>&nbsp;Start Date</strong>";
                    htmlTextBox15j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox16.Name = "htmlTextBox16";
                    htmlTextBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox16.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox16.StyleName = "";
                    htmlTextBox16.Value = "{Fields.PlanEnddate}";
                    htmlTextBox16.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox19.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox16j.Name = "htmlTextBoxValue9";
                    htmlTextBox16j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox16j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox16j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox16j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox16j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox16j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox16j.StyleName = "";
                    htmlTextBox16j.Value = "<strong>&nbsp;End Date</strong>";
                    htmlTextBox16j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox17.Format = "{0:0.00}";
                    htmlTextBox17.Name = "htmlTextBox17";
                    htmlTextBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox17.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox17.StyleName = "";
                    htmlTextBox17.Value = "{Fields.TotalAmount}";
                    htmlTextBox17.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox17.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox17j.Name = "htmlTextBox17j";
                    htmlTextBox17j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox17j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox17j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox17j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox17j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox17j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox17j.StyleName = "";
                    htmlTextBox17j.Value = "<strong>&nbsp;Plan Fee</strong>";
                    htmlTextBox17j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox18.Format = "{0:0.00}";
                    htmlTextBox18.Name = "htmlTextBox18";
                    htmlTextBox18.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox18.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox18.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox18.StyleName = "";
                    htmlTextBox18.Value = "{Fields.AmountPaid}";
                    htmlTextBox18.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox18.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox18j.Name = "htmlTextBoxValue23";
                    htmlTextBox18j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox18j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox18j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox18j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox18j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox18j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox18j.StyleName = "";
                    htmlTextBox18j.Value = "<strong>&nbsp;Amount Paid</strong>";
                    htmlTextBox18j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);


                    htmlTextBoxPaymentdateV.Name = "htmlTextBoxPaymentdateV";
                    htmlTextBoxPaymentdateV.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBoxPaymentdateV.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBoxPaymentdateV.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBoxPaymentdateV.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBoxPaymentdateV.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBoxPaymentdateV.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBoxPaymentdateV.StyleName = "";
                    htmlTextBoxPaymentdateV.Value = "{Fields.PaymentDate}";
                    htmlTextBoxPaymentdateV.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBoxPaymentdateV.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBoxPaymentdateH.Name = "htmlTextBoxPaymentdateH";
                    htmlTextBoxPaymentdateH.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBoxPaymentdateH.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBoxPaymentdateH.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBoxPaymentdateH.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBoxPaymentdateH.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBoxPaymentdateH.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBoxPaymentdateH.StyleName = "";
                    htmlTextBoxPaymentdateH.Value = "<strong>&nbsp;Payment Date</strong>";
                    htmlTextBoxPaymentdateH.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBoxStatusV.Name = "htmlTextBoxStatusV";
                    htmlTextBoxStatusV.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBoxStatusV.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBoxStatusV.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBoxStatusV.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBoxStatusV.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBoxStatusV.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBoxStatusV.StyleName = "";
                    htmlTextBoxStatusV.Value = "{Fields.PaymentStatus}";
                    htmlTextBoxStatusV.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBoxStatusV.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBoxStatusH.Name = "htmlTextBoxStatusH";
                    htmlTextBoxStatusH.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBoxStatusH.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBoxStatusH.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBoxStatusH.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBoxStatusH.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBoxStatusH.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBoxStatusH.StyleName = "";
                    htmlTextBoxStatusH.Value = "<strong>&nbsp;Status</strong>";
                    htmlTextBoxStatusH.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);


                    detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
tablej,
});
                    detail.Height = Telerik.Reporting.Drawing.Unit.Inch(6.88199608039855957D);
                    detail.PageBreak = Telerik.Reporting.PageBreak.None;
                    detail.Name = "Doctor List";
                    #endregion
                }
                else
                {
                    Telerik.Reporting.HtmlTextBox htmlTextErrorMessageDisplay = new Telerik.Reporting.HtmlTextBox();
                    htmlTextErrorMessageDisplay = new Telerik.Reporting.HtmlTextBox();
                    htmlTextErrorMessageDisplay.Style.Color = System.Drawing.Color.Black;
                    htmlTextErrorMessageDisplay.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.4220028698444367D));
                    htmlTextErrorMessageDisplay.Name = "htmlTextBoxErrorMessage";
                    htmlTextErrorMessageDisplay.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18749986588954926D));
                    htmlTextErrorMessageDisplay.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.5D);
                    htmlTextErrorMessageDisplay.StyleName = "";
                    htmlTextErrorMessageDisplay.Value = "<strong>No matching records found.</strong>";
                    detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
htmlTextErrorMessageDisplay,
});
                }
            }
            catch (Exception ex)
            {
                #region Exception
                // ErrorLog Err = new ErrorLog();
                // //Trace the error in the method
                // System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                // string str = trace.GetFrame(0).GetMethod().ReflectedType.FullName;
                // Err.ErrorsEntry(ex.ToString(), ex.Message, ex.GetHashCode(), str, trace.GetFrame(0).GetMethod().Name);
                #endregion
            }
            #endregion
            #region Footer
            Telerik.Reporting.TextBox txtGeneratedby = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedColon = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedByValue = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedDatefooter = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedDateColon = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGenerateddateValue = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox PageNumbers = new Telerik.Reporting.TextBox();
            txtGeneratedby = new Telerik.Reporting.TextBox();
            txtGeneratedColon = new Telerik.Reporting.TextBox();
            txtGeneratedByValue = new Telerik.Reporting.TextBox();
            txtGeneratedDatefooter = new Telerik.Reporting.TextBox();
            txtGeneratedDateColon = new Telerik.Reporting.TextBox();
            txtGenerateddateValue = new Telerik.Reporting.TextBox();
            txtGeneratedby.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.8400000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
            txtGeneratedby.Name = "Generated By";
            txtGeneratedby.Value = "Generated By";
            txtGeneratedby.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.90761510848999D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
            txtGeneratedColon.Name = ":";
            txtGeneratedColon.Value = ":";
            txtGeneratedColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedByValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.9600000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
            txtGeneratedByValue.Name = "Generated By";
            txtGeneratedByValue.Value = Convert.ToString(Session["LastName"]) + " " + Convert.ToString(Session["FirstName"]);
            txtGeneratedByValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedDatefooter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.8400000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            txtGeneratedDatefooter.Name = "Generated Date";
            txtGeneratedDatefooter.Value = "Generated Date";
            txtGeneratedDatefooter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedDateColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.90761510848999D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            txtGeneratedDateColon.Name = ":";
            txtGeneratedDateColon.Value = ":";
            txtGeneratedDateColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGenerateddateValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.9600000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            txtGenerateddateValue.Name = "Generated Date";
            txtGenerateddateValue.Value = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            txtGenerateddateValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            PageNumbers.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.5D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            PageNumbers.Name = "PageNumbers";
            PageNumbers.Value = "Page { PageNumber} of {PageCount}";
            PageNumbers.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            shapeFooter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(txtGeneratedby.Top.Value - 0.15000000596046448D));
            shapeFooter.Name = "shapeFooter";
            shapeFooter.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            shapeFooter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.35), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
            shapeFooter.Stretch = true;
            shapeFooter.Style.Font.Bold = true;
            shapeFooter.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(1D);
            FooterSection1.PrintOnFirstPage = true;
            FooterSection1.PrintOnLastPage = true;
            FooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
shapeFooter, PageNumbers
});//txtGeneratedby ,txtGeneratedColon,txtGeneratedByValue,txtGeneratedDatefooter,txtGeneratedDateColon,txtGenerateddateValue,
            FooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.02);
            FooterSection1.Name = "pageFooterSection1";
            #endregion
            rptirDCS.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { HeaderSection1, detail, FooterSection1 });
            rptirDCS.Name = "Payments Report";
            rptirDCS.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            rptirDCS.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Standard9x11;
            rptirDCS.Width = Telerik.Reporting.Drawing.Unit.Inch(6.887535572052002D);
            rptirDCS.PageSettings.Landscape = false;
            rptbook1.Reports.Add(rptirDCS);
            ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", rptbook1, null);
            rptbook1.DocumentName = "Payments Report";
            string fileName = rptbook1.DocumentName + "." + result.Extension;

            //save documents in path
            string LabPath = DownloadDocumentPath ;
            if (!Directory.Exists(LabPath))
            {
                if (!Directory.Exists(LabPath)) Directory.CreateDirectory(LabPath);
            }
            List<PPCP07302018.Models.Organization.UploadLabResults> files = new List<PPCP07302018.Models.Organization.UploadLabResults>();
            if (fileName != null)
            {

                // Some browsers send file names with full path.
                // We are only interested in the file name.                  
                PPCP07302018.Models.Organization.UploadLabResults objCommunicationAttachment = new PPCP07302018.Models.Organization.UploadLabResults();
                objCommunicationAttachment.Uploadresults.DocumentName = fileName;
                objCommunicationAttachment.Uploadresults.InsertedDate = System.DateTime.Today.ToString("dd/MM/yyyy");
                string PhyPath = DateTime.Now.ToString("ddMMyyyyhhmmss");
                string DocumentPath = Path.Combine(LabPath, PhyPath);
                //System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(DocumentPath);
                //if (!di.Exists) di.Create();
                System.IO.FileStream fs;
                fs = new FileStream(DocumentPath + fileName, FileMode.Append);
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                objCommunicationAttachment.Uploadresults.DocumentPath = fs.Name;
                TempData["FileName"] = fs.Name;
               // objCommunicationAttachment.Uploadresults.Type = Convert.ToInt32(Utils.GlobalFunctions.AttachmentType.DoctorAttachments);
                //objCommunicationAttachment.Uploadresults.ModuleType = Convert.ToInt32(Utils.GlobalFunctions.ModuleType.eConsultation);
                objCommunicationAttachment.Uploadresults.AttachmentDate = DateTime.Now;//Convert.ToDateTime(testdone);
                fs.Close();
                files.Add(objCommunicationAttachment);
                TempData["UploadedFilesLabResults1"] = files;
            }
            return Json(files, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MembersReport()
        {

            return View();
        }

        public JsonResult MemberReportSearch(string ToDate, string FromDate, string ProviderID, string ProviderName, string PlanType,
            string PaymentsStatus)
        {
            Telerik.Reporting.ReportBook rptbook1 = new Telerik.Reporting.ReportBook();
            Telerik.Reporting.PageHeaderSection HeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            Telerik.Reporting.PageFooterSection FooterSection1 = new Telerik.Reporting.PageFooterSection();
            Telerik.Reporting.Report rptirDCS = new Telerik.Reporting.Report();
            Telerik.Reporting.DetailSection detail = new Telerik.Reporting.DetailSection();
            #region Header
            Telerik.Reporting.Shape shape1 = new Telerik.Reporting.Shape();
            shape1 = new Telerik.Reporting.Shape();
            Telerik.Reporting.Shape shapeFooter = new Telerik.Reporting.Shape();
            shapeFooter = new Telerik.Reporting.Shape();
            Telerik.Reporting.HtmlTextBox txtHeading = new Telerik.Reporting.HtmlTextBox();
            txtHeading = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.HtmlTextBox FeatureDate = new Telerik.Reporting.HtmlTextBox();
            FeatureDate = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.HtmlTextBox OrganizationName = new Telerik.Reporting.HtmlTextBox();
            OrganizationName = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.HtmlTextBox DoctorName = new Telerik.Reporting.HtmlTextBox();
            DoctorName = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.PictureBox picLogo = new Telerik.Reporting.PictureBox();
            picLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.607537258118391037), Telerik.Reporting.Drawing.Unit.Inch(0.5));
            picLogo.MimeType = "";
            picLogo.Name = "pictureBox2";
            picLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.016246293783187866D), Telerik.Reporting.Drawing.Unit.Inch(0.69996066689491272D));
            picLogo.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            // picLogo.Value = LogoPath; //img;
            picLogo.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;

            txtHeading.Name = "Heading";
            txtHeading.Value = "<strong>Member List</strong>";
            txtHeading.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.6D), Telerik.Reporting.Drawing.Unit.Pixel(20));
            txtHeading.Style.Font.Bold = true;
            txtHeading.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(14);
            txtHeading.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            txtHeading.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4D), Telerik.Reporting.Drawing.Unit.Inch(0.4D));
            FeatureDate.Name = "FeatureDate";
            FeatureDate.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            FeatureDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.6D), Telerik.Reporting.Drawing.Unit.Pixel(14));
            FeatureDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4D), Telerik.Reporting.Drawing.Unit.Inch(txtHeading.Bottom.Value));
            FeatureDate.Visible = false;
            shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.2D), Telerik.Reporting.Drawing.Unit.Inch(txtHeading.Bottom.Value));
            shape1.Name = "shape1";
            shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.35D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
            shape1.Stretch = true;
            shape1.Style.Font.Bold = true;
            shape1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(1D);
            shape1.Visible = true;
            HeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.7);
           // HeaderSection1.PrintOnFirstPage = true;
           // HeaderSection1.PrintOnLastPage = false;
            HeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
txtHeading,shape1
});
            HeaderSection1.Name = "pageHeaderSection1";
            #endregion
            #region Report
            try
            {
                //string ToDate = System.DateTime.Now.ToString("MM/dd/yyyy");
                //DateTime Todate = Convert.ToDateTime(ToDate);
                //DateTime todate = Todate.AddMonths(-1);
                //string FromDate = todate.ToString("MM/dd/yyyy");
                string OrganizationID = Session["OrganizationID"].ToString();
                if (PaymentsStatus == null || PaymentsStatus == ""|| PaymentsStatus == "All")
                {
                    PaymentsStatus = "";
                }
                if (PlanType == null || PlanType == "")
                {
                    PlanType = "";
                }
                if (ProviderID == null || ProviderID == "")
                {
                    ProviderID = "0";
                }
                DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.PPCPReports> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.PPCPReports>();
                PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
                string[] ParameterName = new string[] { "FromDate", "ToDate", "ProviderID", "PaymentStatus", "PlanType", "OrganziationID", "Type" };//, "ProviderName"
                string[] ParameterValue = new string[] { FromDate, ToDate, ProviderID, PaymentsStatus, PlanType, OrganizationID, "1" };//,"" 
                ServiceData.ParameterName = ParameterName;
                ServiceData.ParameterValue = ParameterValue;
                ServiceData.WebMethodName = "GetPPCPReports";
                List<PPCP07302018.Models.Organization.PPCPReports> List = objcall.CallServices(Convert.ToInt32(0), "GetPPCPReports", ServiceData);



                double bottom = 0.0;
                if (List.Count > 0)
                {
                    #region Detailed
                    DataTable dt = ToDataTable(List);
                    DataTable dtNew = new DataTable();
                    
                    Telerik.Reporting.Table tablej = new Telerik.Reporting.Table();
                    tablej = new Telerik.Reporting.Table();
                    Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();
                    Telerik.Reporting.HtmlTextBox htmlTextBox11j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox12j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox13j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox14j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox15j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox16j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox17j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox18j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox19j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox20j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox21j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox22j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox23j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxPaymentdateV = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxPaymentdateH = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxStatusV = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxStatusH = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxSummary = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxTotalAmount = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxNetAmount = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxCommissionFee = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox11 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox12 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox13 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox14 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox15 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox16 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox17 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox18 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox19 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox20 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox21 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox22 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox23 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.TableGroup tableGroup1jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup2jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup3jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup4jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup5jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup6jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup7jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup8jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup9jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup10jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup11jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup12jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup13jj = new Telerik.Reporting.TableGroup();
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.5000005960464478D))); //FirstName
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.5000005960464478D)));//Last Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//DOB
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.2000065565109253D))); //Mobile Number
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.7000005960464478D)));//Plan Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//Start date
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//End date
                  //  tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//Amount paid
                  //  tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//Payment date
                  //  tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.7000005960464478D)));//Status

                    tablej.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.3D)));
                    tablej.Body.SetCellContent(0, 0, htmlTextBox11);
                    tablej.Body.SetCellContent(0, 1, htmlTextBox12);
                    tablej.Body.SetCellContent(0, 2, htmlTextBox13);
                    tablej.Body.SetCellContent(0, 3, htmlTextBox17);
                    tablej.Body.SetCellContent(0, 4, htmlTextBox14);
                    tablej.Body.SetCellContent(0, 5, htmlTextBox15);
                    tablej.Body.SetCellContent(0, 6, htmlTextBox16);
                    
                  //  tablej.Body.SetCellContent(0, 7, htmlTextBox18);
                  //  tablej.Body.SetCellContent(0, 8, htmlTextBoxPaymentdateV);
                  //  tablej.Body.SetCellContent(0, 9, htmlTextBoxStatusV);

                    tableGroup1jj.Name = "tableGroup1";
                    tableGroup1jj.ReportItem = htmlTextBox11j;
                    tableGroup2jj.Name = "tableGroup2";
                    tableGroup2jj.ReportItem = htmlTextBox12j;
                    tableGroup3jj.Name = "tableGroup3";
                    tableGroup3jj.ReportItem = htmlTextBox13j;
                    tableGroup7jj.Name = "tableGroup6";
                    tableGroup7jj.ReportItem = htmlTextBox17j;
                    tableGroup4jj.Name = "tableGroup4";
                    tableGroup4jj.ReportItem = htmlTextBox14j;
                    tableGroup5jj.Name = "tableGroup13";
                    tableGroup5jj.ReportItem = htmlTextBox15j;
                    tableGroup6jj.Name = "tableGroup5";
                    tableGroup6jj.ReportItem = htmlTextBox16j;
                    
                   // tableGroup8jj.Name = "tableGroup7";
                   // tableGroup8jj.ReportItem = htmlTextBox18j;
                   // tableGroup9jj.Name = "tableGroup8";
                   // tableGroup9jj.ReportItem = htmlTextBoxPaymentdateH;
                   // tableGroup10jj.Name = "tableGroup9";
                   // tableGroup10jj.ReportItem = htmlTextBoxStatusH;

                    tablej.ColumnGroups.Add(tableGroup1jj);
                    tablej.ColumnGroups.Add(tableGroup2jj);
                    tablej.ColumnGroups.Add(tableGroup3jj);
                    tablej.ColumnGroups.Add(tableGroup7jj);
                    tablej.ColumnGroups.Add(tableGroup4jj);
                    tablej.ColumnGroups.Add(tableGroup5jj);
                    tablej.ColumnGroups.Add(tableGroup6jj);
                    
                  //  tablej.ColumnGroups.Add(tableGroup8jj);
                  //  tablej.ColumnGroups.Add(tableGroup9jj);
                  //  tablej.ColumnGroups.Add(tableGroup10jj);
                    tablej.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
htmlTextBox11,
htmlTextBox12,
htmlTextBox13,
htmlTextBox14,
htmlTextBox15,
htmlTextBox16,
htmlTextBox17,
htmlTextBox18,
htmlTextBox11j,
htmlTextBox12j,
htmlTextBox13j,
htmlTextBox14j,
htmlTextBox15j,
htmlTextBox16j,
htmlTextBox17j,
htmlTextBox18j,
htmlTextBoxPaymentdateV,
htmlTextBoxPaymentdateH,
htmlTextBoxStatusV,
htmlTextBoxStatusH
});
                    tablej.Name = "tablej";
                    tableGroup12jj.Groupings.Add(new Telerik.Reporting.Grouping(null));
                    tableGroup12jj.Name = "detailTableGroup";
                    tablej.RowGroups.Add(tableGroup12jj);
                    tablej.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.5500009536743164D), Telerik.Reporting.Drawing.Unit.Inch(1D));
                    tablej.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    tablej.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    objectDataSource.DataMember = "GetPaymentsView";
                    objectDataSource.DataSource = dt;
                    objectDataSource.Name = "objectDataSource";
                    tablej.DataSource = objectDataSource;
                    tablej.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.2D), Telerik.Reporting.Drawing.Unit.Inch(bottom + 0.1));
                    tablej.KeepTogether = false;
                    bottom = tablej.Bottom.Value;
                    htmlTextBox11.Name = "htmlTextBox11";
                    htmlTextBox11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox11.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox11.StyleName = "";
                    htmlTextBox11.Value = "&nbsp;{Fields.LastName}";
                    htmlTextBox11.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox11.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox11j.Name = "htmlTextBoxValue1";
                    htmlTextBox11j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox11j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox11j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox11j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox11j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox11j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox11j.StyleName = "";
                    htmlTextBox11j.Value = "<strong>Last Name</strong>";
                    htmlTextBox11j.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox11j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox12.Name = "htmlTextBox12";
                    htmlTextBox12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox12.StyleName = "";
                    htmlTextBox12.Value = "{Fields.FirstName}";
                    htmlTextBox12.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox12.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox12j.Name = "htmlTextBoxValue2";
                    htmlTextBox12j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox12j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox12j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox12j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox12j.StyleName = "";
                    htmlTextBox12j.Value = "<strong>First Name</strong>";
                    htmlTextBox12j.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox12j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox13.Name = "htmlTextBox13";
                    htmlTextBox13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox13.StyleName = "";
                    htmlTextBox13.Value = "{Fields.DOB}";
                    htmlTextBox13.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox13.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox13j.Name = "htmlTextBoxValue3";
                    htmlTextBox13j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox13j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox13j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox13j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox13j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox13j.StyleName = "";
                    htmlTextBox13j.Value = "<strong>&nbsp;DOB</strong>";
                    htmlTextBox13j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);


                    htmlTextBox17.Name = "htmlTextBox17";
                    htmlTextBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox17.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox17.StyleName = "";
                    htmlTextBox17.Value = "{Fields.MobileNumber}";
                    htmlTextBox17.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox17.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox17j.Name = "htmlTextBox17j";
                    htmlTextBox17j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox17j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox17j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox17j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox17j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox17j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox17j.StyleName = "";
                    htmlTextBox17j.Value = "<strong>&nbsp;Mobile Number</strong>";
                    htmlTextBox17j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);


                    htmlTextBox14.Name = "htmlTextBox14";
                    htmlTextBox14.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox14.StyleName = "";
                    htmlTextBox14.Value = "{Fields.PlanName}";
                    htmlTextBox14.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox14.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox14j.Name = "htmlTextBoxValue4";
                    htmlTextBox14j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox14j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox14j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox14j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox14j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox14j.StyleName = "";
                    htmlTextBox14j.Value = "<strong>&nbsp;Plan Name</strong>";
                    htmlTextBox14j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox15.Name = "htmlTextBox22";
                    htmlTextBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox15.StyleName = "";
                    htmlTextBox15.Value = "{Fields.PlanStartDate}";
                    htmlTextBox15.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox15.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox15j.Name = "htmlTextBox15j";
                    htmlTextBox15j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox15j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox15j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox15j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox15j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox15j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox15j.StyleName = "";
                    htmlTextBox15j.Value = "<strong>&nbsp;Start Date</strong>";
                    htmlTextBox15j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox16.Name = "htmlTextBox16";
                    htmlTextBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox16.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox16.StyleName = "";
                    htmlTextBox16.Value = "{Fields.PlanEnddate}";
                    htmlTextBox16.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox19.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox16j.Name = "htmlTextBoxValue9";
                    htmlTextBox16j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox16j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox16j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox16j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox16j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox16j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox16j.StyleName = "";
                    htmlTextBox16j.Value = "<strong>&nbsp;End Date</strong>";
                    htmlTextBox16j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    

                    //htmlTextBox18.Name = "htmlTextBox18";
                    //htmlTextBox18.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    //htmlTextBox18.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    //htmlTextBox18.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    //htmlTextBox18.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    //htmlTextBox18.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    //htmlTextBox18.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    //htmlTextBox18.StyleName = "";
                    //htmlTextBox18.Value = "{Fields.AmountPaid}";
                    //htmlTextBox18.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    //htmlTextBox18.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    //htmlTextBox18j.Name = "htmlTextBoxValue23";
                    //htmlTextBox18j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    //htmlTextBox18j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    //htmlTextBox18j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    //htmlTextBox18j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    //htmlTextBox18j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    //htmlTextBox18j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    //htmlTextBox18j.StyleName = "";
                    //htmlTextBox18j.Value = "<strong>&nbsp;Amount Paid</strong>";
                    //htmlTextBox18j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);


                    //htmlTextBoxPaymentdateV.Name = "htmlTextBoxPaymentdateV";
                    //htmlTextBoxPaymentdateV.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    //htmlTextBoxPaymentdateV.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    //htmlTextBoxPaymentdateV.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    //htmlTextBoxPaymentdateV.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    //htmlTextBoxPaymentdateV.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    //htmlTextBoxPaymentdateV.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    //htmlTextBoxPaymentdateV.StyleName = "";
                    //htmlTextBoxPaymentdateV.Value = "{Format('{{0:MM/dd/yyyy}}',Fields.PlanEnddate)}";
                    //htmlTextBoxPaymentdateV.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    //htmlTextBoxPaymentdateV.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    //htmlTextBoxPaymentdateH.Name = "htmlTextBoxPaymentdateH";
                    //htmlTextBoxPaymentdateH.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    //htmlTextBoxPaymentdateH.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    //htmlTextBoxPaymentdateH.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    //htmlTextBoxPaymentdateH.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    //htmlTextBoxPaymentdateH.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    //htmlTextBoxPaymentdateH.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    //htmlTextBoxPaymentdateH.StyleName = "";
                    //htmlTextBoxPaymentdateH.Value = "<strong>&nbsp;Payment Date</strong>";
                    //htmlTextBoxPaymentdateH.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    //htmlTextBoxStatusV.Name = "htmlTextBoxStatusV";
                    //htmlTextBoxStatusV.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    //htmlTextBoxStatusV.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    //htmlTextBoxStatusV.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    //htmlTextBoxStatusV.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    //htmlTextBoxStatusV.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    //htmlTextBoxStatusV.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    //htmlTextBoxStatusV.StyleName = "";
                    //htmlTextBoxStatusV.Value = "{Fields.Status}";
                    //htmlTextBoxStatusV.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    //htmlTextBoxStatusV.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    //htmlTextBoxStatusH.Name = "htmlTextBoxStatusH";
                    //htmlTextBoxStatusH.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    //htmlTextBoxStatusH.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    //htmlTextBoxStatusH.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    //htmlTextBoxStatusH.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    //htmlTextBoxStatusH.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    //htmlTextBoxStatusH.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    //htmlTextBoxStatusH.StyleName = "";
                    //htmlTextBoxStatusH.Value = "<strong>&nbsp;Status</strong>";
                    //htmlTextBoxStatusH.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);


                    detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
tablej,
});
                    detail.Height = Telerik.Reporting.Drawing.Unit.Inch(6.88199608039855957D);
                    detail.PageBreak = Telerik.Reporting.PageBreak.None;
                    detail.Name = "Members List";
                    #endregion
                }
                else
                {
                    Telerik.Reporting.HtmlTextBox htmlTextErrorMessageDisplay = new Telerik.Reporting.HtmlTextBox();
                    htmlTextErrorMessageDisplay = new Telerik.Reporting.HtmlTextBox();
                    htmlTextErrorMessageDisplay.Style.Color = System.Drawing.Color.Black;
                    htmlTextErrorMessageDisplay.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.4220028698444367D));
                    htmlTextErrorMessageDisplay.Name = "htmlTextBoxErrorMessage";
                    htmlTextErrorMessageDisplay.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18749986588954926D));
                    htmlTextErrorMessageDisplay.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.5D);
                    htmlTextErrorMessageDisplay.StyleName = "";
                    htmlTextErrorMessageDisplay.Value = "<strong>No matching records found.</strong>";
                    detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
htmlTextErrorMessageDisplay,
});
                }
            }
            catch (Exception ex)
            {
                #region Exception
                // ErrorLog Err = new ErrorLog();
                // //Trace the error in the method
                // System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                // string str = trace.GetFrame(0).GetMethod().ReflectedType.FullName;
                // Err.ErrorsEntry(ex.ToString(), ex.Message, ex.GetHashCode(), str, trace.GetFrame(0).GetMethod().Name);
                #endregion
            }
            #endregion
            #region Footer
            Telerik.Reporting.TextBox txtGeneratedby = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedColon = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedByValue = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedDatefooter = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedDateColon = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGenerateddateValue = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox PageNumbers = new Telerik.Reporting.TextBox();
            txtGeneratedby = new Telerik.Reporting.TextBox();
            txtGeneratedColon = new Telerik.Reporting.TextBox();
            txtGeneratedByValue = new Telerik.Reporting.TextBox();
            txtGeneratedDatefooter = new Telerik.Reporting.TextBox();
            txtGeneratedDateColon = new Telerik.Reporting.TextBox();
            txtGenerateddateValue = new Telerik.Reporting.TextBox();
            txtGeneratedby.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.8400000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
            txtGeneratedby.Name = "Generated By";
            txtGeneratedby.Value = "Generated By";
            txtGeneratedby.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.90761510848999D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
            txtGeneratedColon.Name = ":";
            txtGeneratedColon.Value = ":";
            txtGeneratedColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedByValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.9600000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
            txtGeneratedByValue.Name = "Generated By";
            txtGeneratedByValue.Value = Convert.ToString(Session["LastName"]) + " " + Convert.ToString(Session["FirstName"]) ;
            txtGeneratedByValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedDatefooter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.8400000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            txtGeneratedDatefooter.Name = "Generated Date";
            txtGeneratedDatefooter.Value = "Generated Date";
            txtGeneratedDatefooter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedDateColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.90761510848999D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            txtGeneratedDateColon.Name = ":";
            txtGeneratedDateColon.Value = ":";
            txtGeneratedDateColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGenerateddateValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.9600000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            txtGenerateddateValue.Name = "Generated Date";
            txtGenerateddateValue.Value = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            txtGenerateddateValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            PageNumbers.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.5D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            PageNumbers.Name = "PageNumbers";
            PageNumbers.Value = "Page { PageNumber} of {PageCount}";
            PageNumbers.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            shapeFooter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(txtGeneratedby.Top.Value - 0.15000000596046448D));
            shapeFooter.Name = "shapeFooter";
            shapeFooter.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            shapeFooter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.35), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
            shapeFooter.Stretch = true;
            shapeFooter.Style.Font.Bold = true;
            shapeFooter.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(1D);
            FooterSection1.PrintOnFirstPage = true;
            FooterSection1.PrintOnLastPage = true;
            FooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
shapeFooter, PageNumbers
});//txtGeneratedby ,txtGeneratedColon,txtGeneratedByValue,txtGeneratedDatefooter,txtGeneratedDateColon,txtGenerateddateValue,
            FooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.02);
            FooterSection1.Name = "pageFooterSection1";
            #endregion
            rptirDCS.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { HeaderSection1, detail, FooterSection1 });//
            rptirDCS.Name = "Members Report";
            rptirDCS.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            rptirDCS.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Standard9x11;
            rptirDCS.Width = Telerik.Reporting.Drawing.Unit.Inch(6.887535572052002D);
            rptirDCS.PageSettings.Landscape = false;
            rptbook1.Reports.Add(rptirDCS);
            ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", rptbook1, null);
            rptbook1.DocumentName = "Members Report";
            string fileName = rptbook1.DocumentName + "." + result.Extension;
            //save documents in path
            string LabPath = DownloadDocumentPath;
            if (!Directory.Exists(LabPath))
            {
                if (!Directory.Exists(LabPath)) Directory.CreateDirectory(LabPath);
            }
            List<PPCP07302018.Models.Organization.UploadLabResults> files = new List<PPCP07302018.Models.Organization.UploadLabResults>();
            if (fileName != null)
            {

                // Some browsers send file names with full path.
                // We are only interested in the file name.                  
                PPCP07302018.Models.Organization.UploadLabResults objCommunicationAttachment = new PPCP07302018.Models.Organization.UploadLabResults();
                objCommunicationAttachment.Uploadresults.DocumentName = fileName;
                objCommunicationAttachment.Uploadresults.InsertedDate = System.DateTime.Today.ToString("dd/MM/yyyy");
                string PhyPath = DateTime.Now.ToString("ddMMyyyyhhmmss");
                string DocumentPath = Path.Combine(LabPath, PhyPath);
                //System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(DocumentPath);
                //if (!di.Exists) di.Create();
                System.IO.FileStream fs;
                fs = new FileStream(DocumentPath + fileName, FileMode.Append);
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                objCommunicationAttachment.Uploadresults.DocumentPath = fs.Name;
                TempData["FileName"] = fs.Name;
                // objCommunicationAttachment.Uploadresults.Type = Convert.ToInt32(Utils.GlobalFunctions.AttachmentType.DoctorAttachments);
                //objCommunicationAttachment.Uploadresults.ModuleType = Convert.ToInt32(Utils.GlobalFunctions.ModuleType.eConsultation);
                objCommunicationAttachment.Uploadresults.AttachmentDate = DateTime.Now;//Convert.ToDateTime(testdone);
                fs.Close();
                files.Add(objCommunicationAttachment);
                TempData["UploadedFilesLabResults1"] = files;
            }

            return Json(files, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult PaymentTransactionReport()
        {
            return View();
        }

        public JsonResult PaymentsTransactionReportSearch(string ToDate, string FromDate)
        {
            Telerik.Reporting.ReportBook rptbook1 = new Telerik.Reporting.ReportBook();
            Telerik.Reporting.PageHeaderSection HeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            Telerik.Reporting.PageFooterSection FooterSection1 = new Telerik.Reporting.PageFooterSection();
            Telerik.Reporting.Report rptirDCS = new Telerik.Reporting.Report();
            Telerik.Reporting.DetailSection detail = new Telerik.Reporting.DetailSection();
            #region Header
            Telerik.Reporting.Shape shape1 = new Telerik.Reporting.Shape();
            shape1 = new Telerik.Reporting.Shape();
            Telerik.Reporting.Shape shapeFooter = new Telerik.Reporting.Shape();
            shapeFooter = new Telerik.Reporting.Shape();
            Telerik.Reporting.HtmlTextBox txtHeading = new Telerik.Reporting.HtmlTextBox();
            txtHeading = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.HtmlTextBox FeatureDate = new Telerik.Reporting.HtmlTextBox();
            FeatureDate = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.HtmlTextBox OrganizationName = new Telerik.Reporting.HtmlTextBox();
            OrganizationName = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.HtmlTextBox DoctorName = new Telerik.Reporting.HtmlTextBox();
            DoctorName = new Telerik.Reporting.HtmlTextBox();
            Telerik.Reporting.PictureBox picLogo = new Telerik.Reporting.PictureBox();
            picLogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.607537258118391037), Telerik.Reporting.Drawing.Unit.Inch(0.5));
            picLogo.MimeType = "";
            picLogo.Name = "pictureBox2";
            picLogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.016246293783187866D), Telerik.Reporting.Drawing.Unit.Inch(0.69996066689491272D));
            picLogo.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.Stretch;
            // picLogo.Value = LogoPath; //img;
            picLogo.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;

            txtHeading.Name = "Heading";
            txtHeading.Value = "<strong>Payments Transaction Detail Report</strong>";
            txtHeading.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.6D), Telerik.Reporting.Drawing.Unit.Pixel(20));
            txtHeading.Style.Font.Bold = true;
            txtHeading.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(14);
            txtHeading.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            txtHeading.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4D), Telerik.Reporting.Drawing.Unit.Inch(0.4D));
            FeatureDate.Name = "FeatureDate";
            FeatureDate.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            FeatureDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.6D), Telerik.Reporting.Drawing.Unit.Pixel(14));
            FeatureDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4D), Telerik.Reporting.Drawing.Unit.Inch(txtHeading.Bottom.Value));
            FeatureDate.Visible = false;
            shape1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.2D), Telerik.Reporting.Drawing.Unit.Inch(txtHeading.Bottom.Value));
            shape1.Name = "shape1";
            shape1.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.35D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
            shape1.Stretch = true;
            shape1.Style.Font.Bold = true;
            shape1.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(1D);
            shape1.Visible = true;
            HeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(0.7);
            // HeaderSection1.PrintOnFirstPage = true;
            // HeaderSection1.PrintOnLastPage = false;
            HeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
txtHeading,shape1
});
            HeaderSection1.Name = "pageHeaderSection1";
            #endregion
            #region Report
            try
            {
                //string ToDate = System.DateTime.Now.ToString("MM/dd/yyyy");
                //DateTime Todate = Convert.ToDateTime(ToDate);
                //DateTime todate = Todate.AddMonths(-1);
                //string FromDate = todate.ToString("MM/dd/yyyy");
                string OrganizationID = Session["OrganizationID"].ToString();
                //if (PaymentsStatus == null || PaymentsStatus == "" || PaymentsStatus == "All")
                //{
                //    PaymentsStatus = "";
                //}
                //if (PlanType == null || PlanType == "")
                //{
                //    PlanType = "";
                //}
                //if (ProviderID == null || ProviderID == "")
                //{
                //    ProviderID = "0";
                //}
                DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.TransactionstoPractice> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.TransactionstoPractice>();
                PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
                string[] ParameterName = new string[] { "OrganizationID", "FromDate", "ToDate"  };//, "ProviderName"
                string[] ParameterValue = new string[] { OrganizationID,FromDate, ToDate };//,"" 
                ServiceData.ParameterName = ParameterName;
                ServiceData.ParameterValue = ParameterValue;
                ServiceData.WebMethodName = "GetTransactionsToPractice";
                List<PPCP07302018.Models.Organization.TransactionstoPractice> List = objcall.CallServices(Convert.ToInt32(0), "GetTransactionsToPractice", ServiceData);



                double bottom = 0.0;
                if (List.Count > 0)
                {
                    #region Detailed
                    DataTable dt = ToDataTable(List);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (Convert.ToString(dt.Rows[i]["DOB"]) != "" || Convert.ToString(dt.Rows[i]["DOB"]) != null)
                        {
                            DateTime DOB = Convert.ToDateTime(dt.Rows[i]["DOB"]);
                            string DOB1 = DOB.ToString("MM/dd/yyyy");
                            dt.Rows[i]["DOB"] = DOB1;
                        }
                        else { dt.Rows[i]["DOB"] = ""; }
                        if (Convert.ToString(dt.Rows[i]["PaymentDate"]) != "" || Convert.ToString(dt.Rows[i]["PaymentDate"]) != null)
                        {
                            DateTime paymetdte = Convert.ToDateTime(dt.Rows[i]["PaymentDate"]);
                            string paymetdate1 = paymetdte.ToString("MM/dd/yyyy");
                            dt.Rows[i]["PaymentDate"] = paymetdate1;
                        }
                        else dt.Rows[i]["PaymentDate"] = "";
                    }
                    DataTable dtNew = new DataTable();

                    Telerik.Reporting.Table tablej = new Telerik.Reporting.Table();
                    tablej = new Telerik.Reporting.Table();
                    Telerik.Reporting.ObjectDataSource objectDataSource = new Telerik.Reporting.ObjectDataSource();
                    Telerik.Reporting.HtmlTextBox htmlTextBox11j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox12j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox13j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox14j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox15j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox16j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox17j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox18j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox19j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox20j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox21j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox22j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox23j = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxPaymentdateV = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxPaymentdateH = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxStatusV = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxStatusH = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxSummary = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxTotalAmount = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxNetAmount = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBoxCommissionFee = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox11 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox12 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox13 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox14 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox15 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox16 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox17 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox18 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox19 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox20 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox21 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox22 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.HtmlTextBox htmlTextBox23 = new Telerik.Reporting.HtmlTextBox();
                    Telerik.Reporting.TableGroup tableGroup1jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup2jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup3jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup4jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup5jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup6jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup7jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup8jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup9jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup10jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup11jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup12jj = new Telerik.Reporting.TableGroup();
                    Telerik.Reporting.TableGroup tableGroup13jj = new Telerik.Reporting.TableGroup();
                   // tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.5000005960464478D))); //Organization Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.7000005960464478D)));//Patient  Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.9000005960464478D)));//DOB
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.7000065565109253D))); //Plan Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.7000005960464478D)));//Doctor Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.8000005960464478D)));//Amount Paid
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.8000005960464478D)));//Paymentd date

                    tablej.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.3D)));
                   // tablej.Body.SetCellContent(0, 0, htmlTextBox16);
                    tablej.Body.SetCellContent(0, 0, htmlTextBox11);
                    tablej.Body.SetCellContent(0, 1, htmlTextBox12);
                    tablej.Body.SetCellContent(0, 2, htmlTextBox13);
                    tablej.Body.SetCellContent(0, 3, htmlTextBox17);
                    tablej.Body.SetCellContent(0, 4, htmlTextBox14);
                    tablej.Body.SetCellContent(0, 5, htmlTextBox15);
                    // tablej.Body.SetCellContent(0, 6, htmlTextBox16);

                    //  tablej.Body.SetCellContent(0, 7, htmlTextBox18);
                    //  tablej.Body.SetCellContent(0, 8, htmlTextBoxPaymentdateV);
                    //  tablej.Body.SetCellContent(0, 9, htmlTextBoxStatusV);

                   // tableGroup6jj.Name = "tableGroup5";
                   // tableGroup6jj.ReportItem = htmlTextBox16j;
                    tableGroup1jj.Name = "tableGroup1";
                    tableGroup1jj.ReportItem = htmlTextBox11j;
                    tableGroup2jj.Name = "tableGroup2";
                    tableGroup2jj.ReportItem = htmlTextBox12j;
                    tableGroup3jj.Name = "tableGroup3";
                    tableGroup3jj.ReportItem = htmlTextBox13j;
                    tableGroup7jj.Name = "tableGroup6";
                    tableGroup7jj.ReportItem = htmlTextBox17j;
                    tableGroup4jj.Name = "tableGroup4";
                    tableGroup4jj.ReportItem = htmlTextBox14j;
                    tableGroup5jj.Name = "tableGroup13";
                    tableGroup5jj.ReportItem = htmlTextBox15j;


                    // tableGroup8jj.Name = "tableGroup7";
                    // tableGroup8jj.ReportItem = htmlTextBox18j;
                    // tableGroup9jj.Name = "tableGroup8";
                    // tableGroup9jj.ReportItem = htmlTextBoxPaymentdateH;
                    // tableGroup10jj.Name = "tableGroup9";
                    // tableGroup10jj.ReportItem = htmlTextBoxStatusH;

                   // tablej.ColumnGroups.Add(tableGroup6jj);
                    tablej.ColumnGroups.Add(tableGroup1jj);
                    tablej.ColumnGroups.Add(tableGroup2jj);
                    tablej.ColumnGroups.Add(tableGroup3jj);
                    tablej.ColumnGroups.Add(tableGroup7jj);
                    tablej.ColumnGroups.Add(tableGroup4jj);
                    tablej.ColumnGroups.Add(tableGroup5jj);
                    // 

                    //  tablej.ColumnGroups.Add(tableGroup8jj);
                    //  tablej.ColumnGroups.Add(tableGroup9jj);
                    //  tablej.ColumnGroups.Add(tableGroup10jj);
                    tablej.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
htmlTextBox11,
htmlTextBox12,
htmlTextBox13,
htmlTextBox14,
htmlTextBox15,
htmlTextBox16,
htmlTextBox17,
htmlTextBox18,
htmlTextBox11j,
htmlTextBox12j,
htmlTextBox13j,
htmlTextBox14j,
htmlTextBox15j,
htmlTextBox16j,
htmlTextBox17j,
htmlTextBox18j,
htmlTextBoxPaymentdateV,
htmlTextBoxPaymentdateH,
htmlTextBoxStatusV,
htmlTextBoxStatusH
});
                    tablej.Name = "tablej";
                    tableGroup12jj.Groupings.Add(new Telerik.Reporting.Grouping(null));
                    tableGroup12jj.Name = "detailTableGroup";
                    tablej.RowGroups.Add(tableGroup12jj);
                    tablej.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(7.5500009536743164D), Telerik.Reporting.Drawing.Unit.Inch(1D));
                    tablej.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    tablej.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    objectDataSource.DataMember = "GetPaymentsView";
                    objectDataSource.DataSource = dt;
                    objectDataSource.Name = "objectDataSource";
                    tablej.DataSource = objectDataSource;
                    tablej.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.2D), Telerik.Reporting.Drawing.Unit.Inch(bottom + 0.1));
                    tablej.KeepTogether = false;
                    bottom = tablej.Bottom.Value;

                    //htmlTextBox16.Name = "htmlTextBox16";
                    //htmlTextBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    //htmlTextBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    //htmlTextBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    //htmlTextBox16.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    //htmlTextBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    //htmlTextBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    //htmlTextBox16.StyleName = "";
                    //htmlTextBox16.Value = "{Fields.OrganizationName}";
                    //htmlTextBox16.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    //htmlTextBox19.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    //htmlTextBox16j.Name = "htmlTextBoxValue9";
                    //htmlTextBox16j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    //htmlTextBox16j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    //htmlTextBox16j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    //htmlTextBox16j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    //htmlTextBox16j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    //htmlTextBox16j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    //htmlTextBox16j.StyleName = "";
                    //htmlTextBox16j.Value = "<strong>&nbsp;Organization Name</strong>";
                    //htmlTextBox16j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox11.Name = "htmlTextBox11";
                    htmlTextBox11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox11.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox11.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox11.StyleName = "";
                    htmlTextBox11.Value = "&nbsp;{Fields.MemberName}";
                    htmlTextBox11.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox11.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox11j.Name = "htmlTextBoxValue1";
                    htmlTextBox11j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox11j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox11j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox11j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox11j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox11j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox11j.StyleName = "";
                    htmlTextBox11j.Value = "<strong>Patient Name</strong>";
                    htmlTextBox11j.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox11j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox12.Name = "htmlTextBox12";
                    htmlTextBox12.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox12.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox12.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox12.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.9D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox12.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox12.StyleName = "";
                    htmlTextBox12.Value = "{Fields.DOB}";
                    htmlTextBox12.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox12.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox12j.Name = "htmlTextBoxValue2";
                    htmlTextBox12j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox12j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox12j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.9D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox12j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox12j.StyleName = "";
                    htmlTextBox12j.Value = "<strong>DOB</strong>";
                    htmlTextBox12j.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox12j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox13.Name = "htmlTextBox13";
                    htmlTextBox13.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox13.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox13.StyleName = "";
                    htmlTextBox13.Value = "{Fields.PlanName}";
                    htmlTextBox13.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox13.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox13j.Name = "htmlTextBoxValue3";
                    htmlTextBox13j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox13j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox13j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox13j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox13j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox13j.StyleName = "";
                    htmlTextBox13j.Value = "<strong>&nbsp;Plan Name</strong>";
                    htmlTextBox13j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);


                    htmlTextBox17.Name = "htmlTextBox17";
                    htmlTextBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox17.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox17.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox17.StyleName = "";
                    htmlTextBox17.Value = "{Fields.DoctorName}";
                    htmlTextBox17.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox17.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox17j.Name = "htmlTextBox17j";
                    htmlTextBox17j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox17j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox17j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox17j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox17j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox17j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox17j.StyleName = "";
                    htmlTextBox17j.Value = "<strong>&nbsp;Doctor Name</strong>";
                    htmlTextBox17j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);


                    htmlTextBox14.Name = "htmlTextBox14";
                    htmlTextBox14.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox14.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.9D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox14.StyleName = "";
                    htmlTextBox14.Value = "{Fields.AmountPaid}";
                    htmlTextBox14.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox14.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox14j.Name = "htmlTextBoxValue4";
                    htmlTextBox14j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox14j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox14j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox14j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.9D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox14j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox14j.StyleName = "";
                    htmlTextBox14j.Value = "<strong>&nbsp;Amount Paid</strong>";
                    htmlTextBox14j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox15.Name = "htmlTextBox22";
                    htmlTextBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.9D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox15.StyleName = "";
                    htmlTextBox15.Value = "{Fields.PaymentDate}";
                    htmlTextBox15.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox15.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox15j.Name = "htmlTextBox15j";
                    htmlTextBox15j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox15j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox15j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox15j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox15j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.9D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox15j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox15j.StyleName = "";
                    htmlTextBox15j.Value = "<strong>&nbsp;Payment Date</strong>";
                    htmlTextBox15j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);






                    detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
tablej,
});
                    detail.Height = Telerik.Reporting.Drawing.Unit.Inch(6.88199608039855957D);
                    detail.PageBreak = Telerik.Reporting.PageBreak.None;
                    detail.Name = "PaymentsTransaction List";
                    #endregion
                }
                else
                {
                    Telerik.Reporting.HtmlTextBox htmlTextErrorMessageDisplay = new Telerik.Reporting.HtmlTextBox();
                    htmlTextErrorMessageDisplay = new Telerik.Reporting.HtmlTextBox();
                    htmlTextErrorMessageDisplay.Style.Color = System.Drawing.Color.Black;
                    htmlTextErrorMessageDisplay.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.4220028698444367D));
                    htmlTextErrorMessageDisplay.Name = "htmlTextBoxErrorMessage";
                    htmlTextErrorMessageDisplay.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18749986588954926D));
                    htmlTextErrorMessageDisplay.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.5D);
                    htmlTextErrorMessageDisplay.StyleName = "";
                    htmlTextErrorMessageDisplay.Value = "<strong>No matching records found.</strong>";
                    detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
htmlTextErrorMessageDisplay,
});
                }
            }
            catch (Exception ex)
            {
                #region Exception
                // ErrorLog Err = new ErrorLog();
                // //Trace the error in the method
                // System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
                // string str = trace.GetFrame(0).GetMethod().ReflectedType.FullName;
                // Err.ErrorsEntry(ex.ToString(), ex.Message, ex.GetHashCode(), str, trace.GetFrame(0).GetMethod().Name);
                #endregion
            }
            #endregion
            #region Footer
            Telerik.Reporting.TextBox txtGeneratedby = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedColon = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedByValue = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedDatefooter = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGeneratedDateColon = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox txtGenerateddateValue = new Telerik.Reporting.TextBox();
            Telerik.Reporting.TextBox PageNumbers = new Telerik.Reporting.TextBox();
            txtGeneratedby = new Telerik.Reporting.TextBox();
            txtGeneratedColon = new Telerik.Reporting.TextBox();
            txtGeneratedByValue = new Telerik.Reporting.TextBox();
            txtGeneratedDatefooter = new Telerik.Reporting.TextBox();
            txtGeneratedDateColon = new Telerik.Reporting.TextBox();
            txtGenerateddateValue = new Telerik.Reporting.TextBox();
            txtGeneratedby.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.8400000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
            txtGeneratedby.Name = "Generated By";
            txtGeneratedby.Value = "Generated By";
            txtGeneratedby.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.90761510848999D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
            txtGeneratedColon.Name = ":";
            txtGeneratedColon.Value = ":";
            txtGeneratedColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedByValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.9600000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.449842643737793D));
            txtGeneratedByValue.Name = "Generated By";
            txtGeneratedByValue.Value = Convert.ToString(Session["LastName"]) + " " + Convert.ToString(Session["FirstName"]);
            txtGeneratedByValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedDatefooter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.8400000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            txtGeneratedDatefooter.Name = "Generated Date";
            txtGeneratedDatefooter.Value = "Generated Date";
            txtGeneratedDatefooter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGeneratedDateColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.90761510848999D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            txtGeneratedDateColon.Name = ":";
            txtGeneratedDateColon.Value = ":";
            txtGeneratedDateColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            txtGenerateddateValue.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.9600000333786011D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            txtGenerateddateValue.Name = "Generated Date";
            txtGenerateddateValue.Value = System.DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");
            txtGenerateddateValue.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.562502384185791D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            PageNumbers.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(6.5D), Telerik.Reporting.Drawing.Unit.Inch(0.649842643737793D));
            PageNumbers.Name = "PageNumbers";
            PageNumbers.Value = "Page { PageNumber} of {PageCount}";
            PageNumbers.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
            shapeFooter.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.4000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(txtGeneratedby.Top.Value - 0.15000000596046448D));
            shapeFooter.Name = "shapeFooter";
            shapeFooter.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            shapeFooter.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.35), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
            shapeFooter.Stretch = true;
            shapeFooter.Style.Font.Bold = true;
            shapeFooter.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(1D);
            FooterSection1.PrintOnFirstPage = true;
            FooterSection1.PrintOnLastPage = true;
            FooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
shapeFooter, PageNumbers
});//txtGeneratedby ,txtGeneratedColon,txtGeneratedByValue,txtGeneratedDatefooter,txtGeneratedDateColon,txtGenerateddateValue,
            FooterSection1.Height = Telerik.Reporting.Drawing.Unit.Inch(1.02);
            FooterSection1.Name = "pageFooterSection1";
            #endregion
            rptirDCS.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { HeaderSection1, detail, FooterSection1 });
            rptirDCS.Name = "PaymentsTransaction Report";
            rptirDCS.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));
            rptirDCS.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Standard9x11;
            rptirDCS.Width = Telerik.Reporting.Drawing.Unit.Inch(6.887535572052002D);
            rptirDCS.PageSettings.Landscape = false;
            rptbook1.Reports.Add(rptirDCS);
            ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", rptbook1, null);
            rptbook1.DocumentName = "PaymentsTransaction Report";
            string fileName = rptbook1.DocumentName + "." + result.Extension;
            //save documents in path
            string LabPath = DownloadDocumentPath;
            if (!Directory.Exists(LabPath))
            {
                if (!Directory.Exists(LabPath)) Directory.CreateDirectory(LabPath);
            }
            List<PPCP07302018.Models.Organization.UploadLabResults> files = new List<PPCP07302018.Models.Organization.UploadLabResults>();
            if (fileName != null)
            {

                // Some browsers send file names with full path.
                // We are only interested in the file name.                  
                PPCP07302018.Models.Organization.UploadLabResults objCommunicationAttachment = new PPCP07302018.Models.Organization.UploadLabResults();
                objCommunicationAttachment.Uploadresults.DocumentName = fileName;
                objCommunicationAttachment.Uploadresults.InsertedDate = System.DateTime.Today.ToString("dd/MM/yyyy");
                string PhyPath = DateTime.Now.ToString("ddMMyyyyhhmmss");
                string DocumentPath = Path.Combine(LabPath, PhyPath);
                //System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(DocumentPath);
                //if (!di.Exists) di.Create();
                System.IO.FileStream fs;
                fs = new FileStream(DocumentPath + fileName, FileMode.Append);
                fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length);
                objCommunicationAttachment.Uploadresults.DocumentPath = fs.Name;
                TempData["FileName"] = fs.Name;
                // objCommunicationAttachment.Uploadresults.Type = Convert.ToInt32(Utils.GlobalFunctions.AttachmentType.DoctorAttachments);
                //objCommunicationAttachment.Uploadresults.ModuleType = Convert.ToInt32(Utils.GlobalFunctions.ModuleType.eConsultation);
                objCommunicationAttachment.Uploadresults.AttachmentDate = DateTime.Now;//Convert.ToDateTime(testdone);
                fs.Close();
                files.Add(objCommunicationAttachment);
                TempData["UploadedFilesLabResults1"] = files;
            }

            return Json(files, JsonRequestBehavior.AllowGet);
        }

        #region Billing
        public ActionResult BillingDashboard()
        {
            return View();
        }

        public ActionResult ClaimSubmission(string mode, int VisitId)
        {
            DataAccessLayer.ServiceCall<PPCP07302018.Models.MemberVisit> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.MemberVisit>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "VisitId" };

            string[] ParameterValue = new string[] { VisitId.ToString() };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetVisitById";
            List<PPCP07302018.Models.MemberVisit> List = objcall.CallServices(Convert.ToInt32(0), "GetVisitById", ServiceData);

            MemberVisit model = List.FirstOrDefault();

            return View(model);
        }
        #endregion
    }

}