using Iph.Utilities;
using Newtonsoft.Json;
using PPCP07302018.Controllers.Session;
using PPCP07302018.DataAccessLayer;
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
using System.Xml;
using System.Xml.Serialization;
using Telerik.Reporting.Processing;

namespace PPCP07302018.Controllers
{
    //[AdminSessionController]
    public class AdminController : Controller
    {
        private string DownloadDocumentPath = System.Configuration.ConfigurationSettings.AppSettings["DownloadDocumentsPath"].ToString();
        static string UploadDocumentPath = System.Configuration.ConfigurationSettings.AppSettings["UploadDocumentSignaturePath"].ToString();
        public ActionResult AddPlans()
        {
            //modelParameter.OrganizationTermsandCondition = Convert.ToString(Session["OrganiationTandCFilePath"]);
            //modelParameter.PatientTermsandCondition = Convert.ToString(Session["PatientTandCFilePath"]);
            //modelParameter.ProviderTermsandCondition = Convert.ToString(Session["ProviderTandCFilePath"]);
            return View();
        }
        [System.Web.Mvc.HttpPost]
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        public string SavePlanDetailsxml(Models.Admin.AddPlans modelParameter)
        {
            modelParameter.PatientTermsandCondition = Convert.ToString(Session["PatientTandCFilePath"]);
            modelParameter.OrganizationTermsandCondition = Convert.ToString(Session["OrganiationTandCFilePath"]);
            modelParameter.ProviderTermsandCondition = Convert.ToString(Session["ProviderTandCFilePath"]);
            string xml = GetXMLFromObjects(modelParameter);
            //string returnData = xml.Replace("\"", "\'");
            return xml;
        }
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.AcceptVerbs(HttpVerbs.Post)]
        //public string GetWithdrawPlanList()
        //{
        //    List<PPCP07302018.Models.Admin.WidthdrawPlans> obj = new List<PPCP07302018.Models.Admin.WidthdrawPlans>();
        //    obj = TempData["PlanIdValues"] as List<PPCP07302018.Models.Admin.WidthdrawPlans>;

        //    string xml = GetXMLFromObjects(obj);
        //    return xml;

        //}

        public string GetWithdrawPlanList(Models.Admin.WidthdrawPlans modelParameter)
        {
            string xml = GetXMLFromObjects(modelParameter);
            // string returnData = xml.Replace("\"", "\'");
            return xml;
        }
        /// <summary>
        /// Generate xml for save PlanMapping details-vinod
        /// </summary>
        /// <param name="modelParameter"></param>
        /// <returns></returns>
        public string GenaratePlanMappingxml(Models.Admin.PlanMapping modelParameter)
        {
            string xml = GetXMLFromObjects(modelParameter);
            string returnData = xml.Replace("\"", "\'");
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

        public ActionResult ViewPlans()
        {

            Session["HeaderDisplayName"] = "Admin";
            return View();

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
                                if (List[0].OrganizationTandCFlag == 1 || List[0].OrganizationUserTandCFlag == 1)
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
            //string AdminUsername = System.Configuration.ConfigurationSettings.AppSettings["AdminUsername"].ToString();
            //string AdminPassword = System.Configuration.ConfigurationSettings.AppSettings["AdminPassword"].ToString();

            //if (AdminUsername == model.UserName && AdminPassword == model.Password)
            //{
            //    return Json("1", JsonRequestBehavior.AllowGet);
            //}
            //else {
            //    return Json("2", JsonRequestBehavior.AllowGet);
            //}
        }

        public ActionResult ViewOrganizations()
        {
            return View();

        }
        public ActionResult WithdrawPlans()
        {
            TempData["PlanIdValues"] = null;
            return View();
        }
        public JsonResult RemoveWithdrawPlans(string PlanId)
        {
            int TempValue = Convert.ToInt32(PlanId);
            List<PPCP07302018.Models.Admin.WidthdrawPlans> objList = new List<Models.Admin.WidthdrawPlans>();
            Models.Admin.WidthdrawPlans obj = new Models.Admin.WidthdrawPlans();


            if (TempData["PlanIdValues"] == null)
            {

                obj.PlanID = TempValue;

                objList.Add(obj);
            }

            else
            {
                objList = TempData["PlanIdValues"] as List<PPCP07302018.Models.Admin.WidthdrawPlans>;

                if (objList.Any(m => m.PlanID == TempValue))
                {
                    var PlanID = objList.Find(m => m.PlanID == TempValue);
                    objList.Remove(PlanID);
                }
                else
                {
                    obj.PlanID = TempValue;
                    objList.Add(obj);
                }

            }
            TempData["PlanIdValues"] = objList;
            return Json(objList, JsonRequestBehavior.AllowGet);

        }


        public ActionResult PlanMapping()
        {
            return View();
        }

        public ActionResult AdminLogin()
        {
            return View();
        }
        public ActionResult ViewPayments()
        {
            return View();
        }


        public ActionResult PatientTandCUploadedFiles(IEnumerable<HttpPostedFileBase> PateintTermsAndConditions)
        {

            string DocumentPath = UploadDocumentPath + "\\" + "T&C" + "\\" + Session["PageName"] + "\\" + "Patient";
            if (!Directory.Exists(DocumentPath))
            {
                if (!Directory.Exists(DocumentPath)) Directory.CreateDirectory(DocumentPath);
            }
            foreach (var file in PateintTermsAndConditions)
            {
                string filename = DateTime.Now.ToString("ddMMyyyyhhmmss") + Path.GetFileName(file.FileName);
                string DownloadPath = Path.Combine(DocumentPath, filename);
                Session["PatientTandCFilePath"] = filename;

                file.SaveAs(DownloadPath);

            }


            return Content("");
        }
        public ActionResult OrganizationTandCUploadedFiles(IEnumerable<HttpPostedFileBase> OrganizationTermsaAndConditions)
        {


            string DocumentPath = UploadDocumentPath + "\\" + "T&C" + "\\" + Session["PageName"] + "\\" + "Organization";
            if (!Directory.Exists(DocumentPath))
            {
                if (!Directory.Exists(DocumentPath))
                    Directory.CreateDirectory(DocumentPath);
            }
            foreach (var file in OrganizationTermsaAndConditions)
            {
                string filename = DateTime.Now.ToString("ddMMyyyyhhmmss") + Path.GetFileName(file.FileName);
                string DownloadPath = Path.Combine(DocumentPath, filename);
                Session["OrganiationTandCFilePath"] = filename;
                file.SaveAs(DownloadPath);

            }


            return Content("");
        }
        public ActionResult ProviderTandCUploadedFiles(IEnumerable<HttpPostedFileBase> PoviderTermsAndConditions)
        {
            string DocumentPath = UploadDocumentPath + "\\" + "T&C" + "\\" + Session["PageName"] + "\\" + "Provider";
            if (!Directory.Exists(DocumentPath))
            {
                if (!Directory.Exists(DocumentPath)) Directory.CreateDirectory(DocumentPath);
            }
            foreach (var file in PoviderTermsAndConditions)
            {
                string filename = DateTime.Now.ToString("ddMMyyyyhhmmss") + Path.GetFileName(file.FileName);
                string DownloadPath = Path.Combine(DocumentPath, filename);
                Session["ProviderTandCFilePath"] = filename;
                file.SaveAs(DownloadPath);

            }


            return Content("");
        }


        public ActionResult RemoveUploadedPatientTandC(string[] fileNames)
        {
            try
            {
                if (fileNames != null)
                {
                    Session["PatientTandCFilePath"] = null;
                    foreach (var fullName in fileNames)
                    {
                        var fileName = Path.GetFileName(fullName);
                        var physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                        // RemoveFileNameResults(fileName);
                        // TODO: Verify user permissions
                        if (System.IO.File.Exists(physicalPath))
                        {
                            // The files are not actually removed in this demo
                            // System.IO.File.Delete(physicalPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }
        public ActionResult RemoveUploadedOrganizationTandC(string[] fileNames)
        {
            try
            {
                if (fileNames != null)
                {
                    Session["OrganiationTandCFilePath"] = null;
                    foreach (var fullName in fileNames)
                    {
                        var fileName = Path.GetFileName(fullName);
                        var physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                        // RemoveFileNameResults(fileName);
                        // TODO: Verify user permissions
                        if (System.IO.File.Exists(physicalPath))
                        {
                            // The files are not actually removed in this demo
                            // System.IO.File.Delete(physicalPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }
        public ActionResult RemoveUploadedProviderTandC(string[] fileNames)
        {
            try
            {
                if (fileNames != null)
                {
                    Session["ProviderTandCFilePath"] = null;
                    foreach (var fullName in fileNames)
                    {
                        var fileName = Path.GetFileName(fullName);
                        var physicalPath = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                        // RemoveFileNameResults(fileName);
                        // TODO: Verify user permissions
                        if (System.IO.File.Exists(physicalPath))
                        {
                            // The files are not actually removed in this demo
                            // System.IO.File.Delete(physicalPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }


        public ActionResult ViewProviders()
        {
            return View();
        }
        public ActionResult ViewMembers()
        {
            return View();
        }
        public ActionResult SubscribePlans()
        {

            return View();
        }
        public ActionResult ViewUser()
        {
            return View();
        }


        public ActionResult TermsAndConditions(Models.Admin.TermsConditions model)
        {
            Session["PageName"] = "TermsAndConditions";
            return View();
        }


        //json to Get Path for Terms and Condition 
        public JsonResult GetTCPath(string Type)
        {
            string Path = "";
            if (Type == "1")
            {
                Path = Convert.ToString(Session["PatientTandCFilePath"]);
            }
            else if (Type == "2")
            {
                Path = Convert.ToString(Session["OrganiationTandCFilePath"]);
            }
            else
            {
                Path = Convert.ToString(Session["ProviderTandCFilePath"]);
            }
            return Json(Path, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProviderSearchAutoComplete(string Text)
        {
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.ProviderAutoComplete> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.ProviderAutoComplete>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "OrganizationID", "Text" };

            string[] ParameterValue = new string[] { "0", Text };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetProviderDetailsAutoComplete";
            List<PPCP07302018.Models.Admin.ProviderAutoComplete> List = objcall.CallServicesAdmin(Convert.ToInt32(0), "GetProviderDetailsAutoComplete", ServiceData);
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MemberSearchAutoComplete(string Text)
        {
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.MemberAutoComplete> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.MemberAutoComplete>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "OrganizationID", "Text" };

            string[] ParameterValue = new string[] { "0", Text };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetMembersAutoComplete";
            List<PPCP07302018.Models.Admin.MemberAutoComplete> List = objcall.CallServicesAdmin(Convert.ToInt32(0), "GetMembersAutoComplete", ServiceData);
            return Json(List, JsonRequestBehavior.AllowGet);
        }
        public JsonResult OrganizationSearchAutoComplete(string Text)
        {
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.OrganizationAutoComplete> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.OrganizationAutoComplete>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "Text" };

            string[] ParameterValue = new string[] { Text };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetOrganizationAutoComplete";
            List<PPCP07302018.Models.Admin.OrganizationAutoComplete> List = objcall.CallServicesAdmin(Convert.ToInt32(0), "GetOrganizationAutoComplete", ServiceData);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DiabledOrganizations()
        {
            return View();
        }



        public JsonResult UserSearchAutoComplete(string Text)
        {
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.OrganizationUserAutoComplete> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Admin.OrganizationUserAutoComplete>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "OrganizationID", "Text" };

            string[] ParameterValue = new string[] { "0", Text };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetUserDetailsAutoComplete";
            List<PPCP07302018.Models.Admin.OrganizationUserAutoComplete> List = objcall.CallServicesAdmin(Convert.ToInt32(0), "GetUserDetailsAutoComplete", ServiceData);
            return Json(List, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdminMemberList()
        {
            return View();
        }
        public string AddMemberxml(Models.Organization.AddMemberDetails modelParameter)
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
        public ActionResult AdminAddMember(Models.Member.MemberDetails model)
        {
            MasterController mas = new MasterController();
            Session["SystemIPAddress"] = mas.GetIPAddress();
            string CountryCode = System.Configuration.ConfigurationSettings.AppSettings["CountryCode"].ToString();
            model.CountryCode = CountryCode;
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
            // model.Type = Convert.ToInt32(Utils.GlobalFunctions.Member.Organization);
            return View(model);
        }
        public ActionResult AdminViewPaymentDetails()
        {
            return View();
        }
        public ActionResult AdminPartialPayments(string MemberPlanID)
        {
            PPCP07302018.Models.Member.MakePayment memberplan = new PPCP07302018.Models.Member.MakePayment();
            memberplan.Plan_Code = Convert.ToInt32(MemberPlanID);
            TempData["TemPaymentDetails"] = null;
            return View(memberplan);
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
        ///  [HttpPost]
        // [AcceptVerbs(HttpVerbs.Post)]
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
        public ActionResult AdminPaymentsReport()
        {
            return View();
        }
        public JsonResult PaymentsReportSearch(string ToDate, string FromDate, string ProviderID, string ProviderName, string PlanType,
    string PaymentsStatus, string OrganizationID)
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
                // string OrganizationID = Session["OrganizationID"].ToString();
                if (PaymentsStatus == null || PaymentsStatus == "" || PaymentsStatus == "All")
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
        public ActionResult AdminMemberReport()
        {
            return View();
        }
        public JsonResult MemberReportSearch(string ToDate, string FromDate, string ProviderID, string ProviderName, string PlanType,
           string PaymentsStatus, string OrganizationID)
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
                //  string OrganizationID = Session["OrganizationID"].ToString();
                if (PaymentsStatus == null || PaymentsStatus == "" || PaymentsStatus == "All")
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

        public ActionResult AdminPaymentsTransactionReport()
        {
            return View();
        }
        public JsonResult PaymentsTransactionReportSearch(string ToDate, string FromDate, string OrganizationID)
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
            shape1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(8.55D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
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
                // string OrganizationID = Session["OrganizationID"].ToString();
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
                string[] ParameterName = new string[] { "OrganizationID", "FromDate", "ToDate" };//, "ProviderName"
                string[] ParameterValue = new string[] { OrganizationID, FromDate, ToDate };//,"" 
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
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.5000005960464478D))); //Organization Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.0000005960464478D)));//Patient  Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.9000005960464478D)));//DOB
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.5000065565109253D))); //Plan Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(1.5000005960464478D)));//Doctor Name
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.8000005960464478D)));//Amount Paid
                    tablej.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Inch(0.8000005960464478D)));//Paymentd date

                    tablej.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Inch(0.3D)));
                    tablej.Body.SetCellContent(0, 0, htmlTextBox16);
                    tablej.Body.SetCellContent(0, 1, htmlTextBox11);
                    tablej.Body.SetCellContent(0, 2, htmlTextBox12);
                    tablej.Body.SetCellContent(0, 3, htmlTextBox13);
                    tablej.Body.SetCellContent(0, 4, htmlTextBox17);
                    tablej.Body.SetCellContent(0, 5, htmlTextBox14);
                    tablej.Body.SetCellContent(0, 6, htmlTextBox15);
                    // tablej.Body.SetCellContent(0, 6, htmlTextBox16);

                    //  tablej.Body.SetCellContent(0, 7, htmlTextBox18);
                    //  tablej.Body.SetCellContent(0, 8, htmlTextBoxPaymentdateV);
                    //  tablej.Body.SetCellContent(0, 9, htmlTextBoxStatusV);

                    tableGroup6jj.Name = "tableGroup5";
                    tableGroup6jj.ReportItem = htmlTextBox16j;
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

                    tablej.ColumnGroups.Add(tableGroup6jj);
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

                    htmlTextBox16.Name = "htmlTextBox16";
                    htmlTextBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox16.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox16.StyleName = "";
                    htmlTextBox16.Value = "{Fields.OrganizationName}";
                    htmlTextBox16.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox19.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox16j.Name = "htmlTextBoxValue9";
                    htmlTextBox16j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox16j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox16j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox16j.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox16j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox16j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox16j.StyleName = "";
                    htmlTextBox16j.Value = "<strong>&nbsp;Organization Name</strong>";
                    htmlTextBox16j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);

                    htmlTextBox11.Name = "htmlTextBox11";
                    htmlTextBox11.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox11.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox11.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox11.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox11.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
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
                    htmlTextBox11j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
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
                    htmlTextBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox13.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox13.StyleName = "";
                    htmlTextBox13.Value = "{Fields.PlanName}";
                    htmlTextBox13.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Inch(0.05D);
                    htmlTextBox13.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox13j.Name = "htmlTextBoxValue3";
                    htmlTextBox13j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox13j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox13j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox13j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox13j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                    htmlTextBox13j.StyleName = "";
                    htmlTextBox13j.Value = "<strong>&nbsp;Plan Name</strong>";
                    htmlTextBox13j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);


                    htmlTextBox17.Name = "htmlTextBox17";
                    htmlTextBox17.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
                    htmlTextBox17.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox17.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox17.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox17.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
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
                    htmlTextBox17j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
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

        public ActionResult AddDoctor()
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
    }
}
