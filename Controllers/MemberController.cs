using PPCP07302018.Models.Member;
using PPCP07302018.Models.Organization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using Iph.Utilities;
using PPCP07302018.Controllers.Session;
using Telerik.Reporting.Processing;


using System.Data;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PPCP07302018.Controllers
{

     [MemberSessionController]
    //  [MyErrorHandlerController]
    public class MemberController : Controller
    {
        //Comments
        private object objmember;

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
        // GET: Member
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MemberRegistration(Models.Member.MemberDetails model)
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


        public ActionResult MemberLogin()
        {
            MasterController mas = new MasterController();
            Session["SystemIPAddress"] = mas.GetIPAddress();
            return View();
        }
        public ActionResult MemberCredentials()
        {
            return View();
        }

        public ActionResult FamilyRegistration(Models.Member.MemberDetails model)
        {
            string CountryCode = System.Configuration.ConfigurationSettings.AppSettings["CountryCode"].ToString();
            model.CountryCode = CountryCode;
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
            return View(model);
        }
        public ActionResult PlanEnroll(Models.Member.MemberDetails model)
        {
            TempData["SaveEnrollSelectedMember"] = null;
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
            return View(model);
        }
        //To Convert the modelparameters into XML format for Member Registration by Gayathri
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string PlanEnrollxml(Models.Member.MemberDetails modelParameter)
        {
            string returnData = "";
            try
            {
                List<Models.Member.MemberDetails> MemberDetails = new List<Models.Member.MemberDetails>();
                List<Models.Member.MemberDetails> MemberDetails1 = new List<Models.Member.MemberDetails>();

                if (TempData["SaveEnrollSelectedMember"] != null)
                {
                    List<PPCP07302018.Models.Member.PlanEnrollMembers> objList1 = TempData["SaveEnrollSelectedMember"] as List<PPCP07302018.Models.Member.PlanEnrollMembers>;
                    TempData.Keep("SaveEnrollSelectedMember");
                    for (int i = 0; i < objList1.Count; i++)
                    {
                        Models.Member.MemberDetails objmember = new Models.Member.MemberDetails();//new Models.Member.MemberDetails(); 
                        objmember.MemberID = objList1[i].MemberId;
                        objmember.MemberName = objList1[i].Membername;
                        objmember.LoginMemberID = Convert.ToInt32(Session["MemberID"]);
                        objmember.Email = Convert.ToString(Session["MemberEmail"]);
                        objmember.MobileNumber = Convert.ToString(Session["MemberPrimaryPhone"]);
                        objmember.MemberParentID = modelParameter.MemberParentID;
                        objmember.PlanID = modelParameter.PlanID;
                        objmember.PlanName = modelParameter.PlanName;
                        objmember.OrganizationID = modelParameter.OrganizationID;
                        objmember.OrganizationName = modelParameter.OrganizationName;
                        objmember.ProviderID = modelParameter.ProviderID;
                        objmember.ProviderName = modelParameter.ProviderName;
                        objmember.Status = modelParameter.Status;
                        objmember.PlanStartDate = modelParameter.PlanStartDate;
                        objmember.PlanEndDate = modelParameter.PlanEndDate;
                        objmember.TotalAmount = modelParameter.TotalAmount;
                        objmember.AmountPaid = modelParameter.AmountPaid;
                        objmember.DueAmount = modelParameter.DueAmount;
                        objmember.InstallmentAmount = modelParameter.InstallmentAmount;
                        objmember.NoofInstallments = modelParameter.NoofInstallments;
                        objmember.InstallmentFee = modelParameter.InstallmentFee;
                        objmember.Savings = modelParameter.Savings;
                        objmember.Paymentschedule = modelParameter.Paymentschedule;
                        objmember.Duration = modelParameter.Duration;
                        objmember.PlanType= modelParameter.PlanType;
                        if (modelParameter.CVV != null)
                        {
                            objmember.CVV = modelParameter.CVV;
                            objmember.CardNumber = modelParameter.CardNumber;
                            objmember.NameOnCard = modelParameter.NameOnCard;
                            objmember.MM = modelParameter.MM;
                            objmember.YY = modelParameter.YY;
                        }
                        else
                        {
                            objmember.CardID = modelParameter.CardID;
                        }
                        objmember.StripeCustomerID = Convert.ToString(Session["StripeCustomerID"]);
                        objmember.StripeAccountID = modelParameter.StripeAccountID;
                        objmember.CommPPCP = modelParameter.CommPPCP;
                        objmember.CommPrimaryMember = modelParameter.CommPrimaryMember;
                        objmember.EnrollFee = modelParameter.EnrollFee;
                        MemberDetails.Add(objmember);
                    }
                }

                string xml = GetXMLFromObject(MemberDetails);
                returnData = xml.Replace("\"", "\'");
            }
            catch (Exception Ex)
            {

            }
            return returnData;
        }

        public ActionResult PaymentDetails()
        {
            return View();
        }
        public ActionResult LoginHistory()
        {
            return View();
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
        public ActionResult PlanDetails(Models.Member.MemberDetails model)
        {
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
            return View(model);
        }
        public ActionResult AvailablePlans(Models.Member.MemberDetails model)
        {
            foreach (var modelValue in ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
            return View(model);
        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string MakePaymentxml(Models.Member.MakePayment modelParameter)
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
        public ActionResult ChangePassword()
        {
            return View();
        }
        public ActionResult DefaultPage()
        {
            return View();
        }

        public ActionResult TestPage()
        {
            return View();
        }

        public ActionResult SendCustomerMessage()
        {
            return View();
        }

        /// <summary>
        /// Authenticate User at the time of login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //public JsonResult VerifyUser(PPCP07302018.Models.Organization.MemberLoginDetails model)
        //{
        //    string returnString = "0";  // "2"
        //    DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.MemberLoginDetails> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.MemberLoginDetails>();
        //    ServiceData ServiceData = new ServiceData();
        //    string[] ParameterName = new string[] { "Username", "PassWord", "IPAddress" };
        //    string[] ParameterValue = new string[] { model.UserName, model.Password, Convert.ToString(Session["SystemIPAddress"]) };
        //    ServiceData.ParameterName = ParameterName;
        //    ServiceData.ParameterValue = ParameterValue;
        //    ServiceData.WebMethodName = "ValidateUser";

        //    List<PPCP07302018.Models.Organization.MemberLoginDetails> List = objcall.CallService(Convert.ToInt32(0), "ValidateUser", ServiceData);

        //    if (List.Count >= 1)
        //    {
        //        Session["MemberID"] = List[0].MemberID;
        //        Session["FirstName"] = List[0].CredentialID;
        //        Session["UserStatus"] = List[0].UserStatus;
        //        Session["UserName"] = List[0].UserName;
        //        Session["MemberCountryCode"] = List[0].CountryCode;
        //        Session["MemberPrimaryPhone"] = List[0].Primary_Phone;
        //        Session["MemberName"] = List[0].First_Name + " " + List[0].Last_Name;
        //        Session["MemberDateOfBirth"] = List[0].DateOfBirth;
        //        Session["MemberFirstName"] = List[0].First_Name;
        //        Session["MI"] = List[0].MI;
        //        Session["MemberLastName"] = List[0].Last_Name;
        //        Session["MemberGender"] = List[0].Gender;
        //        Session["MemberEmail"] = List[0].Email;
        //        Session["MemberSubscriptionFlag"] = List[0].SubscriptionFlag;
        //        Session["MemberRaceEthnicity"] = List[0].RaceEthnicity;
        //        Session["StripeCustomerID"] = List[0].StripeCustomerID;
        //        if (List[0].Street1 == null && List[0].Street2 == null)
        //        {
        //            string Address = List[0].State_Name + "," + List[0].City + " , " + List[0].Zip;
        //            Session["MemberAddress"] = Address;
        //        }
        //        else if (List[0].Street1 != null && List[0].Street2 == null)
        //        {
        //            string Address = List[0].State_Name + "," + List[0].City + " ," + List[0].Street1 + " ," + List[0].Zip;
        //            Session["MemberAddress"] = Address;
        //        }
        //        else if (List[0].Street1 != null && List[0].Street2 != null)
        //        {
        //            string Address = List[0].State_Name + "," + List[0].City + " ," + List[0].Street1 + " ," + List[0].Street2 + " ," + List[0].Zip;
        //            Session["MemberAddress"] = Address;
        //        }
        //        Session["2F_Primary_Phone"] = List[0].Primary_Phone;
        //        Session["2F_OTP"] = List[0].OTP;

        //        if (List[0].IsTwofactorAuthentication == true && List[0].TwoFactorType == 1)
        //        {
        //            //OTP form
        //            if (List[0].OTP != null)
        //            {
        //                returnString = "0";
        //                //model.ErrorMessage = "0";   // redirect to OTP form
        //            }
        //            else
        //            {
        //                returnString = "2";
        //                // model.ErrorMessage = "2";  // Out of scenario
        //            }
        //        }
        //        else if (List[0].IsTwofactorAuthentication == true && List[0].TwoFactorType == 2)
        //        {
        //            if (List[0].OTP != null)
        //            {
        //                returnString = "0";
        //                //model.ErrorMessage = "0";   // redirect to OTP form
        //            }
        //            else
        //            {
        //                returnString = "1";
        //                //  model.ErrorMessage = "1";   // redirect to Profile form
        //            }
        //        }
        //        else if (List[0].IsTwofactorAuthentication == false)
        //        {
        //            returnString = "1";
        //            //model.ErrorMessage = "1";   // redirect to Profile form
        //        }
        //        else
        //        {
        //            returnString = "2";
        //            // model.ErrorMessage = "2";   // Out of scenario
        //        }
        //    }
        //    else
        //    {
        //        returnString = "3";
        //        //  model.ErrorMessage = "3";
        //    }
        //    return Json(returnString, JsonRequestBehavior.AllowGet);
        //}
        public string MemberRegistrationxml(Models.Member.MemberDetails modelParameter)
        {

            string xml = GetXMLFromObject(modelParameter);
            string returnData = xml.Replace("\"", "\'");
            return returnData;
        }
        public JsonResult VerifyUser(PPCP07302018.Models.Organization.MemberLoginDetails model)
        {
            string returnString = "2";
            string password = EncryptAndDecrypt.EncrypString(model.Password);
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.MemberLoginDetails> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.MemberLoginDetails>();
            ServiceData ServiceData = new ServiceData();
            string[] ParameterName = new string[] { "Username", "PassWord", "IPAddress" };
            string[] ParameterValue = new string[] { model.UserName, password, Convert.ToString(Session["SystemIPAddress"]) };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "ValidateUser";

            List<PPCP07302018.Models.Organization.MemberLoginDetails> List = objcall.CallService(Convert.ToInt32(0), "ValidateUser", ServiceData);

            if (List.Count >= 1)
            {
                Session["MemberID"] = List[0].MemberID;
                Session["MemberParentID"] = List[0].MemberParentID;
                Session["FirstName"] = List[0].CredentialID;
                Session["UserStatus"] = List[0].UserStatus;
                Session["UserName"] = model.UserName;// List[0].UserName;
                Session["MemberCountryCode"] = List[0].CountryCode;
                Session["MemberPrimaryPhone"] = List[0].MobileNumber;
                Session["MemberName"] = List[0].FirstName + " " + List[0].LastName;
                Session["MemberDateOfBirth"] = List[0].DateOfBirth;
                Session["MemberFirstName"] = List[0].FirstName;
                Session["MI"] = List[0].MI;
                Session["MemberLastName"] = List[0].LastName;
                Session["MemberGender"] = List[0].Gender;
                Session["MemberEmail"] = List[0].Email;
                Session["MemberSubscriptionFlag"] = List[0].SubscriptionFlag;
                Session["MemberRaceEthnicity"] = List[0].RaceEthnicity;
                Session["Password"] = model.Password;
                Session["StripeCustomerID"] = List[0].StripeCustomerID;
                Session["OrganizationUserTandCFlag"] = List[0].OrganizationUserTandCFlag;
                Session["OrganizationUserTandCFlag"] = List[0].OrganizationUserTandCFlag;
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

        public ActionResult SessionOut()
        {
            return View();
        }
        public ActionResult GenerateMemberCard(string MemberID, string PlanID)
        {

            try
            {
                DataAccessLayer.ServiceCall<PPCP07302018.Models.Member.MemberDetails> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Member.MemberDetails>();
                ServiceData ServiceData = new ServiceData();
                string[] ParameterName = new string[] { "strMemberID" };
                string[] ParameterValue = new string[] { MemberID };
                ServiceData.ParameterName = ParameterName;
                ServiceData.ParameterValue = ParameterValue;
                ServiceData.WebMethodName = "GetMemberDetails";

                List<PPCP07302018.Models.Member.MemberDetails> objMemberDetails = objcall.CallService(Convert.ToInt32(0), "GetMemberDetails", ServiceData);
                DateTime objMemberDetailsDate = Convert.ToDateTime(objMemberDetails[0].DOB);
                DateTime obj = new DateTime();
                string MemberGender = "";
                if (objMemberDetails[0].Gender == 1)
                {

                    MemberGender = "Male";
                }
                else
                {
                    MemberGender = "Female";
                }

                string objMemberDetailsDate1 = objMemberDetailsDate.ToString("MM/dd/yyyy");
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
                string OrganizationNameDisplay = "Physician Primary Care Plan";
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
                htmlTextBoxPayer.Visible = false;

                htmlTextBoxPayerColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.200000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape1.Bottom.Value + 0.1D));
                htmlTextBoxPayerColon.Name = "htmlTextBoxPayerColon" + 1;
                htmlTextBoxPayerColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPayerColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPayerColon.Style.Font.Bold = false;
                htmlTextBoxPayerColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPayerColon.Value = "<strong>:</strong>" + "  " + objMemberDetails[0].LastName + " " + objMemberDetails[0].FirstName;//PayerName
                htmlTextBoxPayerColon.Visible = false;

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
                htmlTextBoxNameColon.Value = "<strong>:</strong>" + "  " + objMemberDetails[0].LastName + " " + objMemberDetails[0].FirstName;// MemberName

                htmlTextBoxDOB.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.300000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPayerColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxDOB.Name = "htmlTextBoxDOB" + 1;
                htmlTextBoxDOB.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxDOB.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxDOB.Style.Font.Bold = false;
                htmlTextBoxDOB.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxDOB.Value = "<strong>DOB</strong>";

                htmlTextBoxDOBColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.0D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPayerColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxDOBColon.Name = "htmlTextBoxDOBColon" + 1;
                htmlTextBoxDOBColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxDOBColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxDOBColon.Style.Font.Bold = false;
                htmlTextBoxDOBColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxDOBColon.Value = "<strong>:</strong>" + "  " + objMemberDetailsDate1;//DateOfBirth

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
                htmlTextBoxPatientGenderColon.Value = "<strong>:</strong>" + "  " + MemberGender;//Gender

                htmlTextBoxEmail.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.300000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxEmail.Name = "htmlTextBoxEmail" + 1;
                htmlTextBoxEmail.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxEmail.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxEmail.Style.Font.Bold = false;
                htmlTextBoxEmail.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxEmail.Value = "<strong>Email</strong>";

                htmlTextBoxEmailColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.0D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxEmailColon.Name = "htmlTextBoxEmailColon" + 1;
                htmlTextBoxEmailColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.7D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxEmailColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxEmailColon.Style.Font.Bold = false;
                htmlTextBoxEmailColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxEmailColon.Value = "<strong>:</strong>" + "  " + objMemberDetails[0].Email; ///Email

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
                htmlTextBoxAddressColon.Value = "<strong>:</strong>" + "  " + objMemberDetails[0].Address;//Address

                htmlTextBoxCity.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.300000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPatientGenderColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxCity.Name = "htmlTextBoxCity" + 1;
                htmlTextBoxCity.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxCity.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxCity.Style.Font.Bold = false;
                htmlTextBoxCity.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxCity.Value = "<strong>City</strong>";

                htmlTextBoxCityColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.0D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPatientGenderColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxCityColon.Name = "htmlTextBoxCityColon" + 1;
                htmlTextBoxCityColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxCityColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxCityColon.Style.Font.Bold = false;
                htmlTextBoxCityColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxCityColon.Value = "<strong>:</strong>" + "  " + objMemberDetails[0].CityName;//City

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
                htmlTextBoxStateColon.Value = "<strong>:</strong>" + "  " + objMemberDetails[0].StateName;//State

                htmlTextBoxZip.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.300000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxAddressColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxZip.Name = "htmlTextBoxZip" + 1;
                htmlTextBoxZip.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxZip.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxZip.Style.Font.Bold = false;
                htmlTextBoxZip.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxZip.Value = "<strong>Zip</strong>";

                htmlTextBoxZipColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.0D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxAddressColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxZipColon.Name = "htmlTextBoxZipColon" + 1;
                htmlTextBoxZipColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxZipColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxZipColon.Style.Font.Bold = false;
                htmlTextBoxZipColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxZipColon.Value = "<strong>:</strong>" + "  " + objMemberDetails[0].Zip;//Zip

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

                DataAccessLayer.ServiceCall<PPCP07302018.Models.Member.MemberPaymentsDetails> objcallMemberPlanDetails = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Member.MemberPaymentsDetails>();
                ServiceData ServiceData1 = new ServiceData();
                //string[] ParameterName1 = new string[] { "strMemberPlanCode" };
                //string[] ParameterValue1 = new string[] { PlanID };
                //ServiceData.ParameterName = ParameterName1;
                //ServiceData.ParameterValue = ParameterValue1;
                //ServiceData.WebMethodName = "GetMemberPlanDetails";
                string[] ParameterName1 = new string[] { "strPlanCode" };
                string[] ParameterValue1 = new string[] { PlanID };
                ServiceData.ParameterName = ParameterName1;
                ServiceData.ParameterValue = ParameterValue1;
                ServiceData.WebMethodName = "GetMemberPlanAndPaymentsDetails";

                List<PPCP07302018.Models.Member.MemberPaymentsDetails> objMemberPlanDetails = objcallMemberPlanDetails.CallService(Convert.ToInt32(0), "GetMemberPlanAndPaymentsDetails", ServiceData);

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
                htmlTextBoxProviderNameColon.Value = "<strong>:</strong>" + "  " + objMemberPlanDetails[0].ProviderName;//ProviderName

                htmlTextBoxPlanName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.300000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape2.Bottom.Value + 0.1D));
                htmlTextBoxPlanName.Name = "htmlTextBoxPlanName" + 1;
                htmlTextBoxPlanName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanName.Style.Font.Bold = false;
                htmlTextBoxPlanName.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanName.Value = "<strong>Plan Name</strong>";

                htmlTextBoxPlanNameColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.900000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxSubHeadingShape2.Bottom.Value + 0.1D));
                htmlTextBoxPlanNameColon.Name = "htmlTextBoxPlanNameColon" + 1;
                htmlTextBoxPlanNameColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanNameColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanNameColon.Style.Font.Bold = false;
                htmlTextBoxPlanNameColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanNameColon.Value = "<strong>:</strong>" + "  " + objMemberPlanDetails[0].PlanName;//PlanName

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
                htmlTextBoxPlanAmountColon.Value = "<strong>:</strong>" + "  " + "$ " + objMemberPlanDetails[0].TotalAmount;//PlanAmount

                htmlTextBoxEnrollFee.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.300000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxEnrollFee.Name = "htmlTextBoxEnrollFee" + 1;
                htmlTextBoxEnrollFee.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxEnrollFee.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxEnrollFee.Style.Font.Bold = false;
                htmlTextBoxEnrollFee.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxEnrollFee.Value = "<strong>Enrollment Fee</strong>";

                htmlTextBoxEnrollFeeColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.900000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanNameColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxEnrollFeeColon.Name = "htmlTextBoxEnrollFeeColon" + 1;
                htmlTextBoxEnrollFeeColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxEnrollFeeColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxEnrollFeeColon.Style.Font.Bold = false;
                htmlTextBoxEnrollFeeColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxEnrollFeeColon.Value = "<strong>:</strong>" + "  " + "$ " + objMemberPlanDetails[0].EnrollFee;//EnrollFee

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
                dynamic data = JObject.Parse(objMemberPlanDetails[0].PaymentSchedule);
                htmlTextBoxPaymentSchduleColon.Value = "<strong>:</strong>" + "  " + data.Paymentschedule;// objMemberPlanDetails[0].Paymentschedule//PaymentSchedule
                htmlTextBoxPlanDuration.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.300000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxEnrollFeeColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanDuration.Name = "htmlTextBoxPlanDuration" + 1;
                htmlTextBoxPlanDuration.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanDuration.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanDuration.Style.Font.Bold = false;
                htmlTextBoxPlanDuration.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanDuration.Value = "<strong>Plan Duration</strong>";

                htmlTextBoxPlanDurationColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.900000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxEnrollFeeColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanDurationColon.Name = "htmlTextBoxPlanDurationColon" + 1;
                htmlTextBoxPlanDurationColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanDurationColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanDurationColon.Style.Font.Bold = false;
                htmlTextBoxPlanDurationColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanDurationColon.Value = "<strong>:</strong>" + "  " + objMemberPlanDetails[0].Duration;//PlanDuration

                htmlTextPlanPeriod.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.000000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanDurationColon.Bottom.Value + 0.0120028698444367D));
                htmlTextPlanPeriod.Name = "htmlTextPlanPeriod" + 1;
                htmlTextPlanPeriod.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextPlanPeriod.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextPlanPeriod.Style.Font.Bold = false;
                htmlTextPlanPeriod.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextPlanPeriod.Value = "<strong>Plan Period</strong>";
                htmlTextPlanPeriod.Visible = false;

                htmlTextBoxPlanPeriodColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.3D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanDurationColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanPeriodColon.Name = "htmlTextBoxPlanPeriodColon" + 1;
                htmlTextBoxPlanPeriodColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanPeriodColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanPeriodColon.Style.Font.Bold = false;
                htmlTextBoxPlanPeriodColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanPeriodColon.Value = "<strong>:</strong>" + "  " + data.PlanTermName;//PlanPeriod
                htmlTextBoxPlanPeriodColon.Visible = false;

                htmlTextBoxPlanStatus.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.300000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanDurationColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanStatus.Name = "htmlTextBoxPlanStatus" + 1;
                htmlTextBoxPlanStatus.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanStatus.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanStatus.Style.Font.Bold = false;
                htmlTextBoxPlanStatus.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanStatus.Value = "<strong>Plan Status</strong>";
                htmlTextBoxPlanStatus.Visible = false;

                htmlTextBoxPlanStatusColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.900000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxPlanDurationColon.Bottom.Value + 0.0120028698444367D));
                htmlTextBoxPlanStatusColon.Name = "htmlTextBoxPlanStatusColon" + 1;
                htmlTextBoxPlanStatusColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8708339691162109D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
                htmlTextBoxPlanStatusColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                htmlTextBoxPlanStatusColon.Style.Font.Bold = false;
                htmlTextBoxPlanStatusColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                htmlTextBoxPlanStatusColon.Value = "<strong>:</strong>" + "  ";//+objMemberPlanDetails[0].PlanStatus//PlanStatus
                htmlTextBoxPlanStatusColon.Visible = false;

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


                DataTable dt = ToDataTable(objMemberPlanDetails);
                // Get Report Details
                // DataTable dt = blobj.GetMemberPlanPaymentDetails(Member_Id, Member_Plan_Id, FromDate, ToDate);
                // DataTable dt = new DataTable();
                if (dt.Rows.Count > 0)
                {
                    decimal PaidAmount = 0;
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
                    // string strAmountPaid = Fields.AmountPaid;
                    //PaidAmount = PaidAmount +Convert.ToDecimal(strAmountPaid);
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
                    htmlTextBox14.Visible = false;

                    htmlTextBox14j.Name = "htmlTextBox14j";
                    htmlTextBox14j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox14j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox14j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox14j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox14j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox14j.StyleName = "";
                    htmlTextBox14j.Value = "<strong>Month-Year&nbsp;&nbsp;</strong>";
                    htmlTextBox14j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox14j.Visible = false;

                    htmlTextBox15.Name = "htmlTextBox15";
                    htmlTextBox15.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox15.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox15.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox15.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox15.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox15.StyleName = "";
                    htmlTextBox15.Value = "&nbsp;&nbsp;{Fields.PaymentDate}";
                    htmlTextBox15.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox15.Visible = true;

                    htmlTextBox15j.Name = "htmlTextBox15j";
                    htmlTextBox15j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox15j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox15j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox15j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox15j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox15j.StyleName = "";
                    htmlTextBox15j.Value = "<strong>Date of Payment (MM/dd/yyyy)</strong>";
                    htmlTextBox15j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox15j.Visible = true;

                    htmlTextBox16.Name = "htmlTextBox16";
                    htmlTextBox16.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox16.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox16.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox16.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox16.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox16.StyleName = "";
                    htmlTextBox16.Value = "Card Payment";//"&nbsp;&nbsp;{Fields.MODE_OF_PAY}"
                    htmlTextBox16.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox16.Visible = true;

                    htmlTextBox16j.Name = "htmlTextBox16j";
                    htmlTextBox16j.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
                    htmlTextBox16j.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
                    htmlTextBox16j.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
                    htmlTextBox16j.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.3D));
                    htmlTextBox16j.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBox16j.StyleName = "";
                    htmlTextBox16j.Value = "<strong>&nbsp;&nbsp;Payment Mode</strong>";
                    htmlTextBox16j.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Inch(0.00900000D);
                    htmlTextBox16j.Visible = true;

                    htmlTextBoxTotPaidAmt.Name = "htmlTextBoxTotPaidAmt";
                    htmlTextBoxTotPaidAmt.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.0874997615814209D), Telerik.Reporting.Drawing.Unit.Inch(0.18749986588954926D));
                    htmlTextBoxTotPaidAmt.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
                    htmlTextBoxTotPaidAmt.StyleName = "";
                    object sumObject;
                    sumObject = dt.AsEnumerable().Sum(x => Convert.ToDecimal(x["AmountPaid"]));
                    //  sumObject=dt.Compute("Sum(Convert([AmountPaid], 'System.'))", "[AmountPaid] IS NOT NULL");
                    //sumObject = dt.Compute("Sum(Convert(" + AmountPaid + ", 'System.Int32')", "");
                    // sumObject = dt.Compute("Sum(AmountPaid)", "");
                    //  int sum = Convert.ToInt32(dt.Compute("Sum(AmountPaid)", "[AmountPaid] > 0"));
                    htmlTextBoxTotPaidAmt.Value = "<strong>Total Paid Amount:- </strong>" + " $ " + sumObject;
                    htmlTextBoxTotPaidAmt.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1D), Telerik.Reporting.Drawing.Unit.Inch(bottom + 0.05));
                    htmlTextBoxTotPaidAmt.Visible = true;

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

        public ActionResult FamilyMember()
        {
            return View();
        }
        public JsonResult RemoveEnrollSelectedMembers()
        {
            TempData["SaveEnrollSelectedMember"] = null;
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveEnrollSelectedMembers(int Members, string MemberName, string GenderID, string MemberAge)
        {
            DateTime dt = Convert.ToDateTime(MemberAge);
            var today = DateTime.Today;
            var a = (today.Year * 100 + today.Month) * 100 + today.Day;
            var b = (dt.Year * 100 + dt.Month) * 100 + dt.Day;
            int age = (a - b) / 10000;
            List<PPCP07302018.Models.Member.PlanEnrollMembers> objList1 = new List<PPCP07302018.Models.Member.PlanEnrollMembers>();
            Models.Member.PlanEnrollMembers obj = new Models.Member.PlanEnrollMembers();

            if (TempData["SaveEnrollSelectedMember"] == null)
            {
                obj.MemberId = Members;
                obj.Membername = MemberName;
                obj.Age = age;
                obj.GenderID = Convert.ToInt32(GenderID);
                objList1.Add(obj);
            }

            else
            {
                objList1 = TempData["SaveEnrollSelectedMember"] as List<PPCP07302018.Models.Member.PlanEnrollMembers>;
                var Getmember = objList1.Find(m => m.MemberId == Members);
                if (objList1.Any(m => m.MemberId == Members))
                {
                    objList1.Remove(Getmember);

                }
                else
                {
                    obj.MemberId = Members;
                    obj.Membername = MemberName;
                    obj.Age = age;
                    obj.GenderID = Convert.ToInt32(GenderID);
                    objList1.Add(obj);
                }
            }

            TempData["SaveEnrollSelectedMember"] = objList1;
            return Json(objList1, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RemoveUnchecked()
        {
            TempData["SaveEnrollSelectedMember"] = null;
            return Json(0, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ViewMember(PPCP07302018.Models.Member.MemberDetails model, string MemberID)
        {
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Member.MemberDetails> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Member.MemberDetails>();
            ServiceData ServiceData = new ServiceData();
            string[] ParameterName = new string[] { "strMemberID" };
            string[] ParameterValue = new string[] { MemberID };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetMemberDetails";
            List<PPCP07302018.Models.Member.MemberDetails> List = objcall.CallService(Convert.ToInt32(0), "GetMemberDetails", ServiceData);
            if (List.Count >= 1)
            {
                model.FirstName = List[0].FirstName;
                model.LastName = List[0].LastName;
                Session["ViewMemberID"] = List[0].MemberID;
                model.MobileNumber = List[0].MobileNumber;
                model.CountryCode = List[0].CountryCode;
                model.CountryID = List[0].CountryID;
                model.CountryName = List[0].CountryName;
                model.SalutationID = List[0].SalutationID;
                model.Salutation = List[0].Salutation;
                model.StateID = List[0].StateID;
                model.StateName = List[0].StateName;
                model.CityName = List[0].CityName;
                model.DOB = List[0].DOB;
                model.CityID = List[0].CityID;
                model.RelationshipID = List[0].RelationshipID;
                model.RelationshipName = List[0].RelationshipName;
                model.UserName = List[0].UserName;
                model.Password = List[0].Password;
                model.Gender = List[0].Gender;
                model.Email = List[0].Email;
                model.Zip = List[0].Zip;
            }

            return View(model);
        }

        public ActionResult PartialPayments(string MemberPlanID,string MemberPlanIDs)
        {
            PPCP07302018.Models.Member.MakePayment memberplan = new PPCP07302018.Models.Member.MakePayment();
            memberplan.Plan_Code = Convert.ToInt32(MemberPlanID);
            memberplan.MemberPlanCode= Convert.ToInt32(MemberPlanIDs);
            TempData["TemPaymentDetails"] = null;
            return View(memberplan);
        }

        public JsonResult BindSalutation()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Value = "01", Text = "Dr." });
            items.Add(new SelectListItem { Value = "02", Text = "Mr." });
            items.Add(new SelectListItem { Value = "03", Text = "Mrs." });
            items.Add(new SelectListItem { Value = "04", Text = "Ms." });

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GenerateMemberTermsAndConditions()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveStripeCustomerID(string StripeCustomerID)
        {
            Session["StripeCustomerID"] = StripeCustomerID;
            return Json("", JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///     To Get new Terms And conditions
        /// </summary>
        /// <returns></returns>
        public ActionResult PatientsTermsAndConditions()
        {
            return View();
        }

        public ActionResult CancelPlans(string MemberPlanID)
        {
            PPCP07302018.Models.Member.MakePayment memberplan = new PPCP07302018.Models.Member.MakePayment();
            memberplan.Plan_Code = Convert.ToInt32(MemberPlanID);
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

       // [HttpPost]
       // [AcceptVerbs(HttpVerbs.Post)]
        public string GetXMLPaymentDetails( string list)
        {
           // List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping> list = new List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping>();
           // list = TempData["TemPaymentDetails"] as List<PPCP07302018.Models.Member.MemberPlanInstallmentMapping>;
            string xml = GetXMLFromObject(list);
            string returnData = xml.Replace("\"", "\'");
            return returnData;
        }
    }
}

