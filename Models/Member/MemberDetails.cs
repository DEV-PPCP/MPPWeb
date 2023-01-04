using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PPCP07302018.Models.Member
{
    public class MemberDetails
    {
        public bool IsRxRegistration { get; set; }
        public int ReferringMemberId { get; set; }
        public int PointsEarned { get; set; }
        public int PointsUsed { get; set; }
        public int MemberPlanID { get; set; }
        public int MemberID { get; set; }

        public int LoginMemberID { get; set; }
        public string MemberCode { get; set; }
        public string MemberCardID { get; set; }
        public int MemberParentID { get; set; }
        public string MemberName { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<System.DateTime> DOB { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public int Age { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public int Gender { get; set; }
        public int SalutationID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string Salutation { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Incorrect Format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Incorrect Format")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string CountryName { get; set; }
        public Nullable<int> CountryID { get; set; }

        public Nullable<int> StateID { get; set; }
        public Nullable<int> PlanStateID { get; set; }

        public Nullable<int> CityID { get; set; }
        public Nullable<int> PlanCityID { get; set; }

        [Required(ErrorMessage = " This information is required.")]
        public string StateName { get; set; }

        public string PlanStateName { get; set; }

        [Required(ErrorMessage = "This information is required .")]
        public string CityName { get; set; }

        public string PlanCityName { get; set; }

        public string PlanZip { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Incorrect Format")]
        public string Zip { get; set; }
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Incorrect Format")]
        public string ZipCode { get; set; }

        public bool IsCredentialCreated { get; set; }

        [RegularExpression(@"^[A-Za-z0-9_@./#&+-]*$", ErrorMessage = "Invalid  Username")]
        public string UserName { get; set; }
        public string ConfirmUserName { get; set; }
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string OrganizationName { get; set; }
        public int OrganizationID { get; set; }
        public int BillingTypeID { get; set; }
        public int MaxAllowedClaims { get; set; }
        public string OrganizationAddress { get; set; }
        public string ProviderName { get; set; }
        public int ProviderID { get; set; }
        public string PlanName { get; set; }
        public string ProviderAddress { get; set; }
        public int PlanID { get; set; }
        public int ReferringMemberPoints { get; set; }
        public int NewMemberPoints { get; set; }
        public string Specialization { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public DateTime PlanStartDate { get; set; }
        public string Paymentschedule { get; set; }
        public int NoofInstallments { get; set; }
        public decimal InstallmentAmount { get; set; }
        public decimal InstallmentFee { get; set; }
        public int Savings { get; set; }
        [Required(ErrorMessage = "This Information is required")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "This Information is required")]
        public string NameOnCard { get; set; }

        [Required(ErrorMessage = "This Information is required")]
        public string CVV { get; set; }

        [Required(ErrorMessage = "This Information is required")]
        public int MM { get; set; }

        [Required(ErrorMessage = "This Information is required")]
        public int YY { get; set; }

        public int TwoFactorType { get; set; }

        public decimal TotalAmount { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public decimal? Amount { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal DueAmount { get; set; }
        public decimal Discount { get; set; }
        public string Status { get; set; }
        public string Duration { get; set; }
        public int RelationshipID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string Relationship { get; set; }
        public string Address { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsTwofactorAuthentication { get; set; }
        public string PreferredIP { get; set; }
        public string RelationshipName { get; set; }

        public string PlanTermName { get; set; }
        public int? PlanTermMonths { get; set; }
        public decimal? VisitFee { get; set; }
        public decimal? TeleVisitFee { get; set; }
        public decimal? EnrollFee { get; set; }

        public int? FromAge { get; set; }
        public int? ToAge { get; set; }

        public int? GenderID { get; set; }

        public int? PlanType { get; set; }
        public bool? PlanStatus { get; set; }
        public DateTime? PlanEndDate { get; set; }

        public string PlanDescription { get; set; }
        public int? Plan_Code { get; set; }

        public string CardID { get; set; }
        public string StripeCustomerID { get; set; }

        public string StripeAccountID { get; set; }

        public string MemberType { get; set; }

        public decimal? CommPPCP { get; set; }
        public decimal? CommPrimaryMember { get; set; }
        public string PatientTAndCPath { get; set; }
        public string OrganizationTAndCPath { get; set; }
        public string ProviderTAndCPath { get; set; }

    }

    public class LoginModel
    {
        [Required(ErrorMessage = "Please Enter Username.")]
        [RegularExpression(@"^[A-Za-z0-9_@./#&+-]*$", ErrorMessage = "Invalid  Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password.")]
        public string Password { get; set; }
        public string Otp { get; set; }
        public string captcha_response { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ServiceData
    {
        public string WebMethodName;
        public string[] ParameterName;
        public string[] ParameterValue;

    }
    public class ForgotCredentialsModel
    {
        [Required(ErrorMessage = "Please enter username.")]
        public string txtUserName { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "mobile number must be numeric")]
        public string txtMobileNumber { get; set; }


        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string txtEmail { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Invalid  Name")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Invalid  Name")]
        public string Last_Name { get; set; }
       // [Required(ErrorMessage = "This information is required.")]

        public string CountryCode { get; set; }

        public string OTP { get; set; }
    }
    public class ChangePassword
    {
        [Required(ErrorMessage = "This information is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "This information is required")]
        [RegularExpression(@"(?=^.{8,16}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[a-z]).*$", ErrorMessage = "Minimum 8 characters, allows Characters,Special Characters and Numbers")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "This information is required")]
        public string ConfirmPassword { get; set; }
        public int UserID { get; set; }
    }
    public class PaymentDetails
    {
        public int PaymentDetailsID { get; set; }
        public Nullable<int> MemberPlanID { get; set; }
        public Nullable<int> MemberID { get; set; }
        public Nullable<int> MemberParentID { get; set; }
        public string Membername { get; set; }
        public Nullable<int> PlanID { get; set; }
        public string PlanName { get; set; }
        public string TransactionID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<decimal> TransactionFee { get; set; }
    }
    public class LoginHistory
    {
        public string IpAddress { get; set; }
        public DateTime LoginDate { get; set; }
        public DateTime LogoutDate { get; set; }
    }


    public partial class MemberPlan
    {
        public int MemberPlanID { get; set; }
        public int MemberID { get; set; }
        public Nullable<int> MemberParentID { get; set; }
        public string MemberName { get; set; }
        public bool doesPlanExist { get; set; }
        public bool IsActive { get; set; }
        public int PlanID { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public string ProviderName { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> PlanStartDate { get; set; }
        public Nullable<System.DateTime> PlanEndDate { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public int BillingTypeID { get; set; }
        public string BillingType { get; set; }
        public Nullable<int> PlanTermMonths { get; set; }
        public Nullable<decimal> VisitFee { get; set; }
        public Nullable<decimal> TeleVisitFee { get; set; }
        public string PaymentInterval { get; set; }
        // public Nullable<int> Duration { get; set; }
        public string Duration { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string PlanName { get; set; }
        public Nullable<int> Plan_Code { get; set; }
        public int? PlanType { get; set; }
        public string DOB { get; set; }
        public string PatientTAndCPath { get; set; }
    }


    public class MakePayment
    {
        public int MemberID { get; set; }
        public int MemberParentID { get; set; }
        public int CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public int MemberPlanCode { get; set; }
        public string MemberName { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountPaid { get; set; }
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public string CardNumber { get; set; }
        public int Plan_Code { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public string NameOnCard { get; set; }
        public int MM { get; set; }
        public int YY { get; set; }
        public int CVV { get; set; }
        public string CardID { get; set; }
        public string StripeCustomerID { get; set; }
        public string StripeAccountID { get; set; }
        public string OrganizationID { get; set; }
        public string MemberPlanInstallmentMapping { get; set; }
        public Nullable<decimal> CommPPCP { get; set; }
        public Nullable<decimal> CommPrimaryMember { get; set; }
        public string ProviderName { get; set; }
        public string OrganizationName { get; set; }
        public decimal InstallmentAmount { get; set; }
        public int PointsUsed { get; set; }
    }

    public class MemberPlansDetails
    {
        public int MemberPlanID { get; set; }
        public int MemberID { get; set; }
        public Nullable<int> MemberParentID { get; set; }
        public string MemberName { get; set; }
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string Status { get; set; }

        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
        public Nullable<decimal> Discount { get; set; }
        public string PaymentInterval { get; set; }
        public string Duration { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> Plan_Code { get; set; }

        public virtual Member Member { get; set; }

        public Nullable<decimal> Amount { get; set; }
        public string PaymentIntervals { get; set; }
        public Nullable<bool> IsActive { get; set; }

        public string PlanTermName { get; set; }
        public Nullable<int> PlanTermMonths { get; set; }
        public Nullable<decimal> VisitFee { get; set; }
        public Nullable<decimal> EnrollFee { get; set; }
        public Nullable<int> FromAge { get; set; }
        public Nullable<int> ToAge { get; set; }
        public Nullable<int> GenderID { get; set; }
        public Nullable<int> PlanType { get; set; }
        public Nullable<bool> PlanStatus { get; set; }
        public Nullable<System.DateTime> PlanStartDate { get; set; }
        public Nullable<System.DateTime> PlanEndDate { get; set; }
        public string PlanDescription { get; set; }

        public string PStartDate { get; set; }
        public string PEndDate { get; set; }

        public string Paymentschedule { get; set; }

        public int? NoofInstallments { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public decimal? InstallmentFee { get; set; }
        public decimal? Savings { get; set; }
    }

    public class Member
    {
        public int MemberID { get; set; }
        public Nullable<int> MemberParentID { get; set; }
        public string MemberCode { get; set; }
        public Nullable<int> RelationshipID { get; set; }
        public string RelationshipName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<int> CountryID { get; set; }
        public string CountryName { get; set; }
        public Nullable<int> StateID { get; set; }
        public string StateName { get; set; }
        public Nullable<int> CityID { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string Zip { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsTwofactorAuthentication { get; set; }
        public string PreferredIP { get; set; }
        public Nullable<int> TwoFactorType { get; set; }
    }
    public class PlanEnrollMembers
    {
        public int MemberId { get; set; }
        public string Membername { get; set; }
        public int Age { get; set; }
        public int GenderID { get; set; }
    }

    public class MemberPaymentsDetails
    {
        public string PlanStartDate { get; set; }
        public string PlanEndDate { get; set; }
        public string ProviderID { get; set; }
        public string ProviderName { get; set; }
        public string PlanName { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? DueAmount { get; set; }
        public decimal? EnrollFee { get; set; }
        public string PaymentSchedule { get; set; }

        public string Duration { get; set; }
     //   public bool PlanStatus { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string TransactionID { get; set; }
        public string PlanCode { get; set; }
        public int OrganizationID { get; set; }
    }

    public class EmployerLoginModel
    {
        [Required(ErrorMessage = "Please Enter Username.")]
        [RegularExpression(@"^[A-Za-z0-9_@./#&+-]*$", ErrorMessage = "Invalid  Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password.")]
        public string Password { get; set; }
        public string Otp { get; set; }
        public string captcha_response { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class EmployerForgotCredentialsModel
    {
        [RegularExpression(@"(?=^.{8,16}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[a-z]).*$", ErrorMessage = "Minimum 8 characters, allows Characters,Special Characters and Numbers")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "This information is required")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please enter username.")]
        public string txtUserName { get; set; }

        [Required(ErrorMessage = "This information is required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "mobile number must be numeric")]
        public string txtMobileNumber { get; set; }


        [Required(ErrorMessage = "This information is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string txtEmail { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Invalid  Name")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Invalid  Name")]
        public string Last_Name { get; set; }
        //[Required(ErrorMessage = "This information is required.")]

        public string CountryCode { get; set; }

        public string OTP { get; set; }
    }
    public class EmployerRegistrationDetails
    {
        [Required(ErrorMessage = "This information is required.")]
        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public int? OrganizationID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string TaxID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string OrgEmail { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string EmpCountryCode { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Incorrect Format")]
        public string EmpMobileNumber { get; set; }

       

        public int? UserID { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public DateTime? DOB { get; set; }

        //[Required(ErrorMessage = "This information is required.")]
        public int? Age { get; set; }

        //[Required(ErrorMessage = "This information is required.")]
        public string Gender { get; set; }

        public int? SalutationID { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string Salutation { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Incorrect Format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Incorrect Format")]
        public string MobileNumber { get; set; }

        public int? CountryID { get; set; }

        //[Required(ErrorMessage = "This information is required.")]
        public string CountryName { get; set; }

        public string Address { get; set; }

        public int? Type { get; set; }

        public int? StateID { get; set; }

        //[Required(ErrorMessage = "This information is required.")]
        public string StateName { get; set; }

        public int? CityID { get; set; }

        //[Required(ErrorMessage = "This information is required.")]
        public string CityName { get; set; }

        //[Required(ErrorMessage = "This information is required.")]
        //[RegularExpression(@"^\d{5}$", ErrorMessage = "Incorrect Format")]
        public string Zip { get; set; }

        //[RegularExpression(@"^\d{4}$", ErrorMessage = "Incorrect Format")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[A-Za-z0-9_@./#&+-]*$", ErrorMessage = "Invalid  Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string ConfirmPassword { get; set; }

        public string PreferredIP { get; set; }

        public int? TwoFactorType { get; set; }

        public bool? IsTwofactorAuthentication { get; set; }

        public string Otp { get; set; }

        public string UserEmail { get; set; }

        public string UserCountryCode { get; set; }

        public string UserMobileNumber { get; set; }

        public int? UserCountryID { get; set; }


        public string UserCountryName { get; set; }

        public int? UserStateID { get; set; }

        public string UserStateName { get; set; }

        public int? UserCityID { get; set; }

        public string UserCityName { get; set; }

        public string UserZip { get; set; }

        public string UserZipCode { get; set; }

        public int OrganizationUserTandCFlag { get; set; }
        public int OrganizationTandCFlag { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string EmployerName { get; set; }

        public string EmployerType { get; set; }
        public int? EmployerTypeID { get; set; }
    }

    public  class MemberPlanInstallmentMapping
    {
        public int MemberPlanInstallmentID { get; set; }
        public Nullable<int> MemberPlanID { get; set; }
        public Nullable<int> PaymentDetailsID { get; set; }
        public Nullable<System.DateTime> PaymentDueDate { get; set; }
        public Nullable<decimal> PaymentAmount { get; set; }
        public Nullable<decimal> InstallmentAmount { get; set; }
        public Nullable<decimal> PointsAmount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        
        public Nullable<decimal> PaidAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentDate1 { get; set; }
    }
}