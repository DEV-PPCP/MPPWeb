using PPCP07302018.Models.Member;
using PPCP07302018.Models.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Reporting.Processing;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Text;
using Telerik.Web.UI;
using System.Threading.Tasks;
using Iph.Utilities;
using System.Net;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Telerik.Reporting;

namespace PPCP07302018.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// To get IP address by Gayathri
        /// </summary>
        /// <returns></returns>
        public string GetIPAddress()
        {
           
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            //foreach (IPAddress IP in Host.AddressList)
            //{
            string IPAddress = Common.GetUserIp();
           
            //}

            return IPAddress;
        }
        /// <summary>
        /// To get Age based on DateOfBirth by Gayathri
        /// </summary>
        /// <param name="birthDate"></param>
        /// <returns></returns>
        public JsonResult GetAge(string birthDate)
        {
            int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            int day1 = Convert.ToInt32(birthDate.Split('/')[1]);
            int month1 = Convert.ToInt32(birthDate.Split('/')[0]);
            int year1 = Convert.ToInt32(birthDate.Split('/')[2]);
            string sss = day1 + "/" + month1 + "/" + year1;
            string date = PPCP07302018.DataAccessLayer.MVCUtilityMethod.ConvertDateTime(sss).ToString();
            DateTime fromDate = Convert.ToDateTime(date);
            DateTime toDate = Convert.ToDateTime(DateTime.Now);
            int year;
            int month;
            int day;
            int increment = 0;
            if (fromDate.Day > toDate.Day)
            {
                increment = monthDay[fromDate.Month - 1];
            }

            if (increment == -1)
            {
                if (DateTime.IsLeapYear(fromDate.Year))
                {
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }
            }
            if (increment != 0)
            {
                day = (toDate.Day + increment) - fromDate.Day;
                increment = 1;
            }
            else
            {
                day = toDate.Day - fromDate.Day;
            }
            if ((fromDate.Month + increment) > toDate.Month)
            {
                month = (toDate.Month + 12) - (fromDate.Month + increment);
                increment = 1;
            }
            else
            {
                month = (toDate.Month) - (fromDate.Month + increment);
                increment = 0;
            }
            year = toDate.Year - (fromDate.Year + increment);

            string result = year + ";" + month + ";" + day;

            var JsonData = Json(result, JsonRequestBehavior.AllowGet);
            return JsonData;
        }
        /// <summary>
        /// To Bind Monthly dropdownlist for Carddetails by Gayathri
        /// </summary>
        /// <returns></returns>
        public JsonResult BindMonths()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Value = "01", Text = "01" });
            items.Add(new SelectListItem { Value = "02", Text = "02" });
            items.Add(new SelectListItem { Value = "03", Text = "03" });
            items.Add(new SelectListItem { Value = "04", Text = "04" });
            items.Add(new SelectListItem { Value = "05", Text = "05" });
            items.Add(new SelectListItem { Value = "06", Text = "06" });
            items.Add(new SelectListItem { Value = "07", Text = "07" });
            items.Add(new SelectListItem { Value = "08", Text = "08" });
            items.Add(new SelectListItem { Value = "09", Text = "09" });
            items.Add(new SelectListItem { Value = "10", Text = "10" });
            items.Add(new SelectListItem { Value = "11", Text = "11" });
            items.Add(new SelectListItem { Value = "12", Text = "12" });
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// To Bind Year dropdownlist for Carddetails by Gayathri
        /// </summary>
        /// <returns></returns>
        public JsonResult BindYears()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            int Year = DateTime.Now.Year;
            string s = Year.ToString();
            s = s.Substring(2, s.Length - 2);
            Year = Convert.ToInt32(s);
            for (int i = 0; i < 15; i++)
            {
                items.Add(new SelectListItem { Value = (Year + i).ToString(), Text = (Year + i).ToString() });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        //To Convert the modelparameters into XML format for Member Registration by Gayathri
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string MemberRegistrationxml(Models.Member.MemberDetails modelParameter)
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
        public ActionResult SetVariable(string key, string value)
        {
            Session[key] = value;
            return this.Json(new { success = true });
        }
        
        
        public JsonResult VerifyProOTP(string otp)
        {
            string returnString = "0";

            if (!string.IsNullOrEmpty(otp))
            {
                int parsedValue;
                if (!int.TryParse(otp, out parsedValue))
                {
                    returnString = "0";
                }
                else if (Convert.ToInt32(otp) == Convert.ToInt32(Session["ProloginOTP"]))
                {
                    returnString = "1";
                }
                else
                {
                    returnString = "0";
                }
            }
            return Json(returnString, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetPaymentValues(PPCP07302018.Models.Organization.MemberDetails obj)
        {
            Session["ReportParameters"] = obj;
            return Json("", JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult PaymentReceipt(string MemberID, string PlanID, string OrgnizationID)
        {

            DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.MemberLoginDetails> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.MemberLoginDetails>();
            ServiceData ServiceData = new ServiceData();
            string[] ParameterName = new string[] { "MemberID", "PlanID", "OrganizationID" };
            string[] ParameterValue = new string[] { MemberID, PlanID, OrgnizationID };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetPlanDetails";

            List<PPCP07302018.Models.Organization.MemberLoginDetails> List = objcall.CallServiceTestLKP(Convert.ToInt32(0), "GetPlanDetails", ServiceData);

            PPCP07302018.Models.Organization.MemberDetails obj = new Models.Organization.MemberDetails();
            obj = (PPCP07302018.Models.Organization.MemberDetails)Session["ReportParameters"];

            string PayerName = string.Empty;
            string MemberName = string.Empty;
            string DateOfBirth = string.Empty;
            string Gender = string.Empty;
            string Email = string.Empty;
            string Address = string.Empty;
            string City = string.Empty;
            string State = string.Empty;
            string Zip = string.Empty;

            PayerName = "";
            MemberName = obj.MemberName;

            //DateTime dob = Convert.ToDateTime(dtMembers.Rows[0]["MEMBER_DOB"]);
            //DateOfBirth = obj.DOB;
            //int gender = Convert.ToInt32(dtMembers.Rows[0]["GD_GENDER_ID"]);
            //if (gender == 90)
            //    Gender = "Male";
            //else
            //    Gender = "Female";

            Email = obj.Email;
            Address = obj.Address;
            City = obj.City;
            State = obj.State;
            //Zip = Convert.ToString(dtMembers.Rows[0]["MEMBER_ZIP"]);




            string ProviderName = string.Empty;
            string PlanName = string.Empty;
            string PlanAmount = string.Empty;
            string EnrollFee = string.Empty;
            string PaymentSchedule = string.Empty;
            string PlanDuration = string.Empty;
            string PlanPeriod = string.Empty;
            string PlanStatus = string.Empty;


            //ProviderName = Convert.ToString(dtMemberPlans.Rows[0]["PROVIDER_NAME"]);
            //PlanName = Convert.ToString(dtMemberPlans.Rows[0]["PLAN_NAME"]);
            //PlanAmount = Convert.ToString(dtMemberPlans.Rows[0]["INSTALLEMENT_FEE"]);
            //EnrollFee = Convert.ToString(dtMemberPlans.Rows[0]["ENROLLMENT_FEE"]);
            //PaymentSchedule = Convert.ToString(dtMemberPlans.Rows[0]["PAYMENT_SHEDULE"]);
            //PlanDuration = Convert.ToString(dtMemberPlans.Rows[0]["DURATION"]);
            //PlanPeriod = Convert.ToString(dtMemberPlans.Rows[0]["PLAN_START_DATE"]) + " - " + Convert.ToString(dtMemberPlans.Rows[0]["PLAN_END_DATE"]);
            //PlanStatus = Convert.ToString(dtMemberPlans.Rows[0]["PLAN_STATUS"]);



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
            htmlTextBoxPayerColon.Value = "<strong>:</strong>" + "  " + PayerName;

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
            htmlTextBoxNameColon.Value = "<strong>:</strong>" + "  " + MemberName;

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
            htmlTextBoxDOBColon.Value = "<strong>:</strong>" + "  " + DateOfBirth;

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
            htmlTextBoxPatientGenderColon.Value = "<strong>:</strong>" + "  " + Gender;

            htmlTextBoxEmail.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(4.700000333786011D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxNameColon.Bottom.Value + 0.0120028698444367D));
            htmlTextBoxEmail.Name = "htmlTextBoxEmail" + 1;
            htmlTextBoxEmail.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            htmlTextBoxEmail.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            htmlTextBoxEmail.Style.Font.Bold = false;
            htmlTextBoxEmail.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            htmlTextBoxEmail.Value = "<strong>Email</strong>";

            htmlTextBoxEmailColon.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(5.7D), Telerik.Reporting.Drawing.Unit.Inch(htmlTextBoxNameColon.Bottom.Value + 0.0120028698444367D));
            htmlTextBoxEmailColon.Name = "htmlTextBoxEmailColon" + 1;
            htmlTextBoxEmailColon.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.3D), Telerik.Reporting.Drawing.Unit.Inch(0.18000000715255737D));
            htmlTextBoxEmailColon.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            htmlTextBoxEmailColon.Style.Font.Bold = false;
            htmlTextBoxEmailColon.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            htmlTextBoxEmailColon.Value = "<strong>:</strong>" + "  " + Email;

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
            htmlTextBoxAddressColon.Value = "<strong>:</strong>" + "  " + Address;

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
            htmlTextBoxCityColon.Value = "<strong>:</strong>" + "  " + City;

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
            htmlTextBoxStateColon.Value = "<strong>:</strong>" + "  " + State;

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
            htmlTextBoxZipColon.Value = "<strong>:</strong>" + "  " + Zip;

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
            htmlTextBoxProviderNameColon.Value = "<strong>:</strong>" + "  " + ProviderName;

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
            htmlTextBoxPlanNameColon.Value = "<strong>:</strong>" + "  " + PlanName;

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
            htmlTextBoxPlanAmountColon.Value = "<strong>:</strong>" + "  " + "$ " + PlanAmount;

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
            htmlTextBoxEnrollFeeColon.Value = "<strong>:</strong>" + "  " + "$ " + EnrollFee;

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
            htmlTextBoxPaymentSchduleColon.Value = "<strong>:</strong>" + "  " + PaymentSchedule;

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
            htmlTextBoxPlanDurationColon.Value = "<strong>:</strong>" + "  " + PlanDuration;

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
            htmlTextBoxPlanPeriodColon.Value = "<strong>:</strong>" + "  " + PlanPeriod;

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
            htmlTextBoxPlanStatusColon.Value = "<strong>:</strong>" + "  " + PlanStatus;

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
            // Get Report Details

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
            //objectDataSource.DataSource = dt;
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
            htmlTextBox11.Value = "$ " + "{Fields.PLAN_AMOUNT}";
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
            htmlTextBox12.Value = "$ " + "{Fields.PAID_AMOUNT}";
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
            htmlTextBox13.Value = "$ " + "{Fields.DUE_AMOUNT}";
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
            htmlTextBox14.Value = "{Fields.INSTALLMENT_DATE}&nbsp;&nbsp;";
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
            htmlTextBox15.Value = "&nbsp;&nbsp;{Fields.PAYMENT_DATE}";
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
            htmlTextBox16.Value = "&nbsp;&nbsp;{Fields.MODE_OF_PAY}";
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
            //sumObject = dt.Compute("Sum(PAID_AMOUNT)", "");
            //htmlTextBoxTotPaidAmt.Value = "<strong>Total Paid Amount:- </strong>" + " $ " + Convert.ToString(sumObject);
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
            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("PDF", rptbook1, null);
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
            Response.End();
            // Code For Telerik Report Ends

            return View();
        }

       
        //public JsonResult VerifyUser(PPCP07302018.Models.Organization.MemberLoginDetails model)
        //{
        //    string returnString = "2";
        //    string password = EncryptAndDecrypt.EncrypString(model.Password);
        //    DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.MemberLoginDetails> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Organization.MemberLoginDetails>();
        //    ServiceData ServiceData = new ServiceData();
        //    string[] ParameterName = new string[] { "Username", "PassWord", "IPAddress" };
        //    string[] ParameterValue = new string[] { model.UserName, password, Convert.ToString(Session["SystemIPAddress"]) };
        //    ServiceData.ParameterName = ParameterName;
        //    ServiceData.ParameterValue = ParameterValue;
        //    ServiceData.WebMethodName = "ValidateUser";

        //    List<PPCP07302018.Models.Organization.MemberLoginDetails> List = objcall.CallService(Convert.ToInt32(0), "ValidateUser", ServiceData);

        //    if (List.Count >= 1)
        //    {
        //        Session["MemberID"] = List[0].MemberID;
        //        Session["MemberParentID"] = List[0].MemberParentID;
        //        Session["FirstName"] = List[0].CredentialID;
        //        Session["UserStatus"] = List[0].UserStatus;
        //        Session["UserName"] = List[0].UserName;
        //        Session["MemberCountryCode"] = List[0].CountryCode;
        //        Session["MemberPrimaryPhone"] = List[0].MobileNumber;
        //        Session["MemberName"] = List[0].FirstName + " " + List[0].LastName;
        //        Session["MemberDateOfBirth"] = List[0].DateOfBirth;
        //        Session["MemberFirstName"] = List[0].FirstName;
        //        Session["MI"] = List[0].MI;
        //        Session["MemberLastName"] = List[0].LastName;
        //        Session["MemberGender"] = List[0].Gender;
        //        Session["MemberEmail"] = List[0].Email;
        //        Session["MemberSubscriptionFlag"] = List[0].SubscriptionFlag;
        //        Session["MemberRaceEthnicity"] = List[0].RaceEthnicity;
        //        Session["Password"] = model.Password;
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
        //                Session["loginOTP"] = Convert.ToInt32(List[0].OTP);
        //                // redirect to OTP form
        //            }
        //            else
        //            {
        //                returnString = "2";
        //                // Out of scope
        //            }
        //        }
        //        else if (List[0].IsTwofactorAuthentication == true && List[0].TwoFactorType == 2)
        //        {
        //            if (List[0].PreferredIP != Convert.ToString(Session["SystemIPAddress"]))
        //            {
        //                //OTP form
        //                if (List[0].OTP != null)
        //                {
        //                    returnString = "0";
        //                    Session["loginOTP"] = Convert.ToInt32(List[0].OTP);
        //                    // redirect to OTP form
        //                }
        //                else
        //                {
        //                    returnString = "2";
        //                    // Out of scope
        //                }
        //            }
        //            else
        //            {
        //                returnString = "1";
        //                // redirect to form
        //            }
        //        }
        //        else if (List[0].IsTwofactorAuthentication == false)
        //        {
        //            returnString = "1";
        //            // redirect to form
        //        }
        //        else
        //        {
        //            returnString = "1";
        //            // redirect to form
        //        }
        //    }
        //    else
        //    {
        //        returnString = "3";
        //        // Error
        //    }
        //    return Json(returnString, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult BindSalutation()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Dr.", Value = "1" });
            items.Add(new SelectListItem { Text = "Mr.", Value = "2" });
            items.Add(new SelectListItem { Text = "Mrs.", Value = "3" });
            items.Add(new SelectListItem { Text = "Ms.", Value = "4" });


            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindDegree()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "M.D.", Value = "1" });
            items.Add(new SelectListItem { Text = "D.O.", Value = "2" });
     

            return Json(items, JsonRequestBehavior.AllowGet);
        }


        public JsonResult BindTenures()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "1 Month", Value = "1" });
            items.Add(new SelectListItem { Text = "3 Months", Value = "3" });
            items.Add(new SelectListItem { Text = "6 Months", Value = "6" });
            items.Add(new SelectListItem { Text = "12 Months", Value = "12" });
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindMemberPlanTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "Individual Plan", Value = "1" });
            items.Add(new SelectListItem { Text = "Family Plan", Value = "2" });
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BindPlanTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            // items.Add(new SelectListItem { Text = "Domestic", Value = "1" });
            // items.Add(new SelectListItem { Text = "International", Value = "1" });
            items.Add(new SelectListItem { Text = "Dental", Value = "1" });
            items.Add(new SelectListItem { Text = "Eye", Value = "2" });
            items.Add(new SelectListItem { Text = " International Visitor", Value = "3" });
            items.Add(new SelectListItem { Text = "Prescription", Value = "4" });
            items.Add(new SelectListItem { Text = "Primary Care", Value = "5" });
            return Json(items, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPaymentSchedule()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Add(new SelectListItem { Text = "Monthly", Value = "12" });
            items.Add(new SelectListItem { Text = "Quarterly", Value = "3" });
            items.Add(new SelectListItem { Text = "Half Yearly", Value = "6" });
            items.Add(new SelectListItem { Text = "One Time", Value = "1" });


            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ErrorMessage(string webMethodName,string textStatus)
        {

            return Json("",JsonRequestBehavior.AllowGet);
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

        //Added by akhil to download report
        public ActionResult MemberPlanCardReport(string MemberID, string PlanID)
        {
            DataAccessLayer.ServiceCall<PPCP07302018.Models.Member.MemberDetails> objcall = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Member.MemberDetails>();
            PPCP07302018.Models.Member.ServiceData ServiceData = new PPCP07302018.Models.Member.ServiceData();
            string[] ParameterName = new string[] { "strMemberID" };
            string[] ParameterValue = new string[] { MemberID };
            ServiceData.ParameterName = ParameterName;
            ServiceData.ParameterValue = ParameterValue;
            ServiceData.WebMethodName = "GetMemberDetails";
            List<PPCP07302018.Models.Member.MemberDetails> objMemberDetails = objcall.CallService(Convert.ToInt32(0), "GetMemberDetails", ServiceData);

            List<PPCP07302018.Models.Member.MemberPaymentsDetails> objMemberPlanDetails = new List<MemberPaymentsDetails>();
            List<PPCP07302018.Models.Provider.Provider> objProvider = new List<Models.Provider.Provider>();

            if (!string.IsNullOrEmpty(PlanID) && PlanID != "null") //Member in Plan
            {
                DataAccessLayer.ServiceCall<PPCP07302018.Models.Member.MemberPaymentsDetails> objcallMemberPlanDetails = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Member.MemberPaymentsDetails>();
                PPCP07302018.Models.Member.ServiceData ServiceData1 = new PPCP07302018.Models.Member.ServiceData();
                string[] ParameterName1 = new string[] { "strPlanCode" };
                string[] ParameterValue1 = new string[] { PlanID };
                ServiceData.ParameterName = ParameterName1;
                ServiceData.ParameterValue = ParameterValue1;
                ServiceData.WebMethodName = "GetMemberPlanAndPaymentsDetails";
                objMemberPlanDetails = objcallMemberPlanDetails.CallService(Convert.ToInt32(0), "GetMemberPlanAndPaymentsDetails", ServiceData);

                DataAccessLayer.ServiceCall<PPCP07302018.Models.Provider.Provider> objcallProvierDetails = new DataAccessLayer.ServiceCall<PPCP07302018.Models.Provider.Provider>();
                PPCP07302018.Models.Member.ServiceData ServiceData2 = new PPCP07302018.Models.Member.ServiceData();

                string[] ParameterName2 = new string[] { "strOrganizationID", "strProviderID" };
                string[] ParameterValue2 = new string[] { Convert.ToString(objMemberPlanDetails[0].OrganizationID), objMemberPlanDetails[0].ProviderID };

                ServiceData.ParameterName = ParameterName2;
                ServiceData.ParameterValue = ParameterValue2;
                ServiceData.WebMethodName = "GetProviderDetails";

                objProvider = objcallProvierDetails.CallServices(Convert.ToInt32(0), "GetProviderDetails", ServiceData);

                string specializationname = "";
                if (!string.IsNullOrEmpty(objProvider[0].Specialization))
                {
                    string json = "{\"Specializations\":" + objProvider[0].Specialization + "}";
                    var result1 = JsonConvert.DeserializeObject<SpecializationList>(json);
                    for (int i = 0; i < result1.Specializations.Count; i++)
                    {
                        PPCP07302018.Models.Provider.Provider obj = new PPCP07302018.Models.Provider.Provider();
                        obj.SpecializationName = result1.Specializations[i].SpecializationName;
                        specializationname = specializationname + result1.Specializations[i].SpecializationName + ",";
                    }

                    specializationname = specializationname.Substring(specializationname.Length - 1).Equals(",") ? specializationname.TrimEnd(',') : specializationname;
                }
            }            

            string msg = TrelerikReportDownload(objMemberDetails, objMemberPlanDetails, objProvider);
            return View();
        }

        private string TrelerikReportDownload(List<PPCP07302018.Models.Member.MemberDetails> objMemberDetails, List<PPCP07302018.Models.Member.MemberPaymentsDetails> objMemberPlanDetails, List<PPCP07302018.Models.Provider.Provider> objProvider)
        {
            Telerik.Reporting.ReportBook rptbook1 = new Telerik.Reporting.ReportBook();
            int i = 1;
            string EffectiveperiodValue = string.Empty;
            string PlanName = string.Empty;
            string MemberId = string.Empty;
            if (objMemberPlanDetails.Count > 0)
            {
                i = 0;

                PlanName = objMemberPlanDetails[0].PlanName;
                if (objMemberPlanDetails[0].PlanStartDate != null && objMemberPlanDetails[0].PlanStartDate != "" &&
                objMemberPlanDetails[0].PlanEndDate != null && objMemberPlanDetails[0].PlanEndDate != "")
                {
                    DateTime plandatestart = Convert.ToDateTime(objMemberPlanDetails[0].PlanStartDate);
                    string startpalndate = plandatestart.ToString("MM/dd/yyyy");
                    DateTime planenddate = Convert.ToDateTime(objMemberPlanDetails[0].PlanEndDate);
                    string endplandate = planenddate.ToString("MM/dd/yyyy");
                    EffectiveperiodValue = startpalndate + " " + "to" + " " + endplandate;//"11/27/2018 to 05/31/2019";

                }
            }

            string PCPname = string.Empty;
            string OrganizationName = string.Empty;
            string OrgPhone = string.Empty;
            string Address = string.Empty;

            string MemberName = (objMemberDetails[0].LastName + " " + objMemberDetails[0].FirstName).ToUpper();
            //MemberId = "MPP0" + objMemberDetails[0].MobileNumber;
            MemberId = objMemberDetails[0].MemberCardID;
            if (objProvider.Count > 0)
            {
                Address = objProvider[0].Address + ", " + objProvider[0].CityName + ", " + objProvider[0].StateName + ", " + objProvider[0].Zip;
                OrgPhone = objProvider[0].OrgPhone;
                OrganizationName = objProvider[0].OrganizationName;
                PCPname = "Dr. " + (objProvider[0].LastName + " " + objProvider[0].FirstName).ToUpper(); //+ ", " + objProvider[0].Degree).ToUpper();
            }

            string fontname = "Glacial Indifference";

            string imgName = "IDCardLogo.png";
            var dir = Server.MapPath("/images");
            string LogoImage = Path.Combine(dir, imgName);

            string capitalRxLogoFile = "CapitalRxLogo.png";
            string capitalRxImage = Path.Combine(dir, capitalRxLogoFile);
            //string LogoImage = System.Configuration.ConfigurationManager.AppSettings["MemberCardImage"].ToString();

            Telerik.Reporting.ReportBook rptbook = new Telerik.Reporting.ReportBook();
            Telerik.Reporting.Report rptirDCS = new Telerik.Reporting.Report();
            Telerik.Reporting.DetailSection detail = new Telerik.Reporting.DetailSection();
            detail = new Telerik.Reporting.DetailSection();
            rptbook1.DocumentName = "MemberCard"; //Filename
            rptirDCS.Name = "MemberCard";

            if (i == 0)
            {
                i = 1;
                # region Combined card                

                Telerik.Reporting.PictureBox pictureBox = new Telerik.Reporting.PictureBox();
                pictureBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(0D));
                pictureBox.MimeType = "";
                pictureBox.Name = "pictureBox3";
                pictureBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
                pictureBox.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
                pictureBox.Value = LogoImage;//"= Parameters.Logo.Value";

                Telerik.Reporting.HtmlTextBox txtorgName = new Telerik.Reporting.HtmlTextBox();
                txtorgName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
                txtorgName.Name = "txtorgName";
                txtorgName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.0D), Telerik.Reporting.Drawing.Unit.Inch(0.5D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtorgName.Value = "<span style='color: white'><b>Primary Care Plan </b> </span>";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                txtorgName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtorgName.Style.Font.Name = fontname;
                txtorgName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(8.5D);

                Telerik.Reporting.Shape horizantalShape = new Telerik.Reporting.Shape();
                horizantalShape = new Telerik.Reporting.Shape();
                horizantalShape.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0D), Telerik.Reporting.Drawing.Unit.Inch(pictureBox.Bottom.Value + 0.01D));
                horizantalShape.Name = "horizantalShape";
                horizantalShape.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                horizantalShape.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.80082919692993164D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                horizantalShape.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.5D);
                horizantalShape.Style.Color = System.Drawing.Color.White;

                Telerik.Reporting.HtmlTextBox txtMemberId = new Telerik.Reporting.HtmlTextBox();
                txtMemberId.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(horizantalShape.Bottom.Value + 0.1));
                txtMemberId.Name = "txtMemberId";
                txtMemberId.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtMemberId.Value = "<span style='color: white;'>Member ID: <b>" + MemberId + "</b>" 
                                    + "<br/>Member Name: <b>" + MemberName + "</b></span> ";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                txtMemberId.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtMemberId.Style.Font.Name = fontname;

                //Telerik.Reporting.HtmlTextBox txtMemberName = new Telerik.Reporting.HtmlTextBox();
                //txtMemberName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(txtMemberId.Bottom.Value));
                //txtMemberName.Name = "txtMemberName";
                //txtMemberName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                //txtMemberName.Value = "<span style='color: white;'>Member Name :</span> <span style='color: white; font-weight:bold'>" + MemberName + "</span>";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                //txtMemberName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                //txtMemberName.Style.Font.Name = "Arial";

                Telerik.Reporting.HtmlTextBox txtPlanName = new Telerik.Reporting.HtmlTextBox();
                txtPlanName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(txtMemberId.Bottom.Value + 0.1D));
                txtPlanName.Name = "txtPlanName";
                txtPlanName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.4D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtPlanName.Value = "<span style='color: white;'>Plan: " + PlanName + "<br/>Effective: " + EffectiveperiodValue + "</span>";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                txtPlanName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtPlanName.Style.Font.Name = fontname;

                Telerik.Reporting.HtmlTextBox txtPlanAmount = new Telerik.Reporting.HtmlTextBox();
                txtPlanAmount.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.4D), Telerik.Reporting.Drawing.Unit.Inch(txtMemberId.Bottom.Value + 0.1D));
                txtPlanAmount.Name = "txtPlanAmount";
                txtPlanAmount.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtPlanAmount.Value = "<span style='color: white;'><b>Copay</b><br/> In-Person: $35<br/>Tele-Visit: $25</span>";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                txtPlanAmount.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtPlanAmount.Style.Font.Name = fontname;

                //Telerik.Reporting.HtmlTextBox txtEffectiveDate = new Telerik.Reporting.HtmlTextBox();
                //txtEffectiveDate.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(txtPlanAmount.Bottom.Value));
                //txtEffectiveDate.Name = "txtEffectiveDate";
                //txtEffectiveDate.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                //txtEffectiveDate.Value = "<span style='color: white;'>Effective : </span><span style='color: white; font-weight:bold'>" + EffectiveperiodValue + "</span>";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                //txtEffectiveDate.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                //txtEffectiveDate.Style.Font.Name = "Arial";

                Telerik.Reporting.HtmlTextBox txtPCPName = new Telerik.Reporting.HtmlTextBox();
                txtPCPName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(txtPlanAmount.Bottom.Value + 0.1D));
                txtPCPName.Name = "txtPCPName";
                txtPCPName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.4D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtPCPName.Value = "<span style='color: white;'>PCP Name: " + PCPname + "<br/>Facility Name: " + OrganizationName + "</span>";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                txtPCPName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtPCPName.Style.Font.Name = fontname;

                Telerik.Reporting.HtmlTextBox txtPhone = new Telerik.Reporting.HtmlTextBox();
                txtPhone.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.4D), Telerik.Reporting.Drawing.Unit.Inch(txtPlanAmount.Bottom.Value + 0.1D));
                txtPhone.Name = "txtPhone";
                txtPhone.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtPhone.Value = "<span style='color: white;'>Phone: " + OrgPhone + "</span>";
                txtPhone.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtPhone.Style.Font.Name = fontname;

                //Telerik.Reporting.HtmlTextBox txtFacilityName = new Telerik.Reporting.HtmlTextBox();
                //txtFacilityName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(txtPhone.Bottom.Value));
                //txtFacilityName.Name = "txtFacilityName";
                //txtFacilityName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.5D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                //txtFacilityName.Value = "<span style='color: white;'>Facility Name : " + OrganizationName + "</span>";
                //txtFacilityName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                //txtFacilityName.Style.Font.Name = "Arial";

                Telerik.Reporting.HtmlTextBox txtFacilityAddress = new Telerik.Reporting.HtmlTextBox();
                txtFacilityAddress.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(txtPhone.Bottom.Value));
                txtFacilityAddress.Name = "txtFacilityAddress";
                txtFacilityAddress.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.7D), Telerik.Reporting.Drawing.Unit.Inch(0.19996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtFacilityAddress.Value = "<span style='color: white;'>Address: " + Address + "<br/><br/>MyPhysicianPlan is a primary care plan and not insurance.</span>";
                txtFacilityAddress.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtFacilityAddress.Style.Font.Name = fontname;

                detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                                         pictureBox,
                                         txtorgName,
                                         horizantalShape,
                                         txtMemberId,
                                         //txtMemberName,
                                         txtPlanName,
                                         txtPlanAmount,
                                         //txtEffectiveDate,
                                         txtPCPName,
                                         txtPhone,
                                         //txtFacilityName,
                                         txtFacilityAddress
                                    });

                #endregion
            }
            if (i == 1)
            {
                #region RX card

                double startX = 2.3D;

                if (objMemberPlanDetails.Count == 0)
                {
                    startX = 0D;
                    rptirDCS.Name = "RxCard";
                    rptbook1.DocumentName = "RxCard"; //Filename
                }
                if (objMemberPlanDetails.Count > 0)
                { 
                    Telerik.Reporting.Shape dividerLine = new Telerik.Reporting.Shape();
                    dividerLine = new Telerik.Reporting.Shape();
                    dividerLine.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0D), Telerik.Reporting.Drawing.Unit.Inch(startX));
                    dividerLine.Name = "dividerLine";
                    dividerLine.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                    dividerLine.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.80082919692993164D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                    dividerLine.Style.LineStyle = Telerik.Reporting.Drawing.LineStyle.Dashed;
                    dividerLine.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.5D);
                    dividerLine.Style.Color = System.Drawing.Color.White;

                    detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { dividerLine });
                }

                Telerik.Reporting.PictureBox pictureBoxrx = new Telerik.Reporting.PictureBox();
                pictureBoxrx.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(startX + 0.1D));
                pictureBoxrx.MimeType = "";
                pictureBoxrx.Name = "pictureBox3";
                pictureBoxrx.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.2D), Telerik.Reporting.Drawing.Unit.Inch(0.5D));
                pictureBoxrx.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
                pictureBoxrx.Value = LogoImage;

                Telerik.Reporting.HtmlTextBox txtRxorgName = new Telerik.Reporting.HtmlTextBox();
                txtRxorgName.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2D), Telerik.Reporting.Drawing.Unit.Inch(startX + 0.2D));
                txtRxorgName.Name = "txtRxorgName";
                txtRxorgName.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.7D), Telerik.Reporting.Drawing.Unit.Inch(0.5D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtRxorgName.Value = "<span style='color: white;'><b>Prescription Savings Card</b><br/> FREE CARD - NO EXPIRY</span>";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                txtRxorgName.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtRxorgName.Style.Font.Name = fontname;

                Telerik.Reporting.Shape horizantalShaperx = new Telerik.Reporting.Shape();
                horizantalShaperx = new Telerik.Reporting.Shape();
                horizantalShaperx.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.0D), Telerik.Reporting.Drawing.Unit.Inch(pictureBoxrx.Bottom.Value + 0.01D));
                horizantalShaperx.Name = "horizantalShaperx";
                horizantalShaperx.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
                horizantalShaperx.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.80082919692993164D), Telerik.Reporting.Drawing.Unit.Inch(0.0520833320915699D));
                horizantalShaperx.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(0.5D);
                horizantalShaperx.Style.Color = System.Drawing.Color.White;

                Telerik.Reporting.HtmlTextBox txtPharmasist = new Telerik.Reporting.HtmlTextBox();
                txtPharmasist.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(horizantalShaperx.Bottom.Value + 0.1));
                txtPharmasist.Name = "txtPharmasist";
                txtPharmasist.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.8D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtPharmasist.Value = "<span style='color: white;'>Show to your pharmacist</span>";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                txtPharmasist.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtPharmasist.Style.Font.Name = fontname;

                Telerik.Reporting.HtmlTextBox txtPharmaPlan = new Telerik.Reporting.HtmlTextBox();
                txtPharmaPlan.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(1.8D), Telerik.Reporting.Drawing.Unit.Inch(horizantalShaperx.Bottom.Value + 0.1));
                txtPharmaPlan.Name = "txtPharmaPlan";
                txtPharmaPlan.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.9D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtPharmaPlan.Value = "<span style='color: white;'>Pharmacies Call: 844-722-7794</span>";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                txtPharmaPlan.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtPharmaPlan.Style.Font.Name = fontname;

                Telerik.Reporting.HtmlTextBox txtRxDetails = new Telerik.Reporting.HtmlTextBox();
                txtRxDetails.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(txtPharmaPlan.Bottom.Value));
                txtRxDetails.Name = "txtRxDetails";
                txtRxDetails.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(1.5D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtRxDetails.Value = "<span style='color: white; font-weight:bold'>RxBin: 610852<br/> RxPCN: CAPLRX <br/>RxGRP: CRA23</span> ";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                txtRxDetails.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtRxDetails.Style.Font.Name = fontname;

                Telerik.Reporting.HtmlTextBox txtRxMemberId = new Telerik.Reporting.HtmlTextBox();
                txtRxMemberId.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(txtRxDetails.Bottom.Value + 0.1));
                txtRxMemberId.Name = "txtRxMemberId";
                txtRxMemberId.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtRxMemberId.Value = "<span style='color: white'>Member ID: </span> <span style='color: white; font-weight:bold'>" + MemberId + "</span>";//                      : " + Convert.ToString(dtPatientDetails.Rows[0]["EntityName"]);
                txtRxMemberId.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtRxMemberId.Style.Font.Name = fontname;

                Telerik.Reporting.HtmlTextBox txtRxNote = new Telerik.Reporting.HtmlTextBox();
                txtRxNote.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(txtRxMemberId.Bottom.Value + 0.1));
                txtRxNote.Name = "txtRxNote";
                txtRxNote.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtRxNote.Value = "<span style='color: white; font-weight:bold'>This Saving card is not an insurance program<br/> and canot be combined with insurance</span>";
                txtRxNote.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtRxNote.Style.Font.Name = fontname;

                Telerik.Reporting.HtmlTextBox txtRxTermNote = new Telerik.Reporting.HtmlTextBox();
                txtRxTermNote.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(0.1D), Telerik.Reporting.Drawing.Unit.Inch(txtRxNote.Bottom.Value + 0.1));
                txtRxTermNote.Name = "txtRxTermNote";
                txtRxTermNote.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(3.5D), Telerik.Reporting.Drawing.Unit.Inch(0.29996060132980347D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtRxTermNote.Value = "<span style='color: white; font-weight:bold; font-size: 8px;'>This is a free card and may not be sold. <br/> Void where prohibited by law.</span>";
                txtRxTermNote.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtRxTermNote.Style.Font.Name = fontname;

                Telerik.Reporting.HtmlTextBox txtRximgtoptext = new Telerik.Reporting.HtmlTextBox();
                txtRximgtoptext.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(3D), Telerik.Reporting.Drawing.Unit.Inch(txtRxNote.Bottom.Value));
                txtRximgtoptext.Name = "txtRximgtoptext";
                txtRximgtoptext.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.5D), Telerik.Reporting.Drawing.Unit.Inch(0.1D)); //new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(2.1800000667572021D), Telerik.Reporting.Drawing.Unit.Inch(0.15000000596046448D));
                txtRximgtoptext.Value = "<span style='color: white; font-weight:bold; font-size: 6px;'>Powered By</span>";
                //txtRximgtoptext.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
                txtRximgtoptext.Style.Font.Name = fontname;

                Telerik.Reporting.PictureBox rximgBox = new Telerik.Reporting.PictureBox();
                rximgBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Inch(2.8D), Telerik.Reporting.Drawing.Unit.Inch(txtRxNote.Bottom.Value + 0.006));
                rximgBox.MimeType = "";
                rximgBox.Name = "rximgBox";
                rximgBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(0.8D), Telerik.Reporting.Drawing.Unit.Inch(0.2D));
                rximgBox.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
                rximgBox.Value = capitalRxImage;

                detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
                                         pictureBoxrx,
                                         txtRxorgName,
                                         horizantalShaperx,
                                         txtRxDetails,
                                         txtRxMemberId,
                                         txtPharmasist,
                                         txtPharmaPlan,
                                         txtRxNote,
                                         txtRxTermNote,
                                         txtRximgtoptext,
                                         rximgBox
                                    });

                i = 1;
                #endregion
            }
            detail.Style.BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#8a56a1"); //#9c55c4

            rptirDCS.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { detail });            

            rptirDCS.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D), Telerik.Reporting.Drawing.Unit.Inch(0D));//new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D), Telerik.Reporting.Drawing.Unit.Inch(0.20000000298023224D));
            rptirDCS.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Prc32K;
            rptirDCS.Width = Telerik.Reporting.Drawing.Unit.Inch(3.587535572052002D);
            rptbook1.Reports.Add(rptirDCS);

            ReportProcessor reportProcessor = new ReportProcessor();
            RenderingResult result = reportProcessor.RenderReport("PDF", rptbook1, null);
            
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
            return "";
        }

        public class SpecializationList
        {
            public List<Specialization> Specializations { get; set; }
        }

        public class Specialization
        {
            public int SpecializationId { get; set; }
            public string SpecializationName { get; set; }

        }
        [HttpPost]
        [AcceptVerbs(HttpVerbs.Post)]
        public string PendingPlanEnroll(Models.Member.MemberDetails modelParameter)
        {
            List<Models.Member.MemberDetails> list = new List<Models.Member.MemberDetails>();
            list.Add(modelParameter);
            string xml = GetXMLFromObject(list);
            string returnData = xml.Replace("\"", "\'");
            return returnData;
        }
    }
}
