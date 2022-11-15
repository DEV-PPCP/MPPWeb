using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
namespace PPCP07302018.Models.Organization
{
    public class OrganizationDetails
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
        public string OrgCountryCode { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Incorrect Format")]
        public string OrgMobileNumber { get; set; }

        public int? OrgCountryID { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string OrgCountryName { get; set; }

        public int? OrgStateID { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string OrgStateName { get; set; }

        public int? OrgCityID { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string OrgCityName { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Incorrect Format")]
        public string OrgZip { get; set; }

        public string OrgZipCode { get; set; }

        public string OrgAddress { get; set; }

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

     //   [Required(ErrorMessage = "This information is required.")]
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
        [RegularExpression(@"^[A-Za-z0-9_@./#&+-]*$", ErrorMessage = "Invalid  Username")]
        public string ConfirmUserName { get; set; }


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
        [Required(ErrorMessage = "This information is required.")]
        public string AccountNumber { get; set; }

        public string AccountCountryName { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string AccountHolderName { get; set; }

        public int SSN { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string RoutingNumber { get; set; }
        public int OrganizationUserTandCFlag { get; set; }
        public int OrganizationTandCFlag { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string UserSSN { get; set; }


        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Incorrect Format")]
        public string NPI { get; set; }
      
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Incorrect Format")]
        public string Fax { get; set; }
        public string Degree { get; set; }
        //[Required(ErrorMessage = "This Information is Requried.")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Incorrect Format")]
        public string ProviderEmail { get; set; }
       // [Required(ErrorMessage = "This Information is Requried")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Incorrect Format")]
        public string ProviderMobileNumber { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string ProviderSalutation { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string ProviderFirstName { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string ProviderLastName { get; set; }

       // [Required(ErrorMessage = "This information is required.")]
        public DateTime? ProviderDOB { get; set; }
        public string SpecializationID { get; set; }
        public string SpecializationName { get; set; }
        public string Specialization { get; set; }
        public int? ProviderSalutationID { get; set; }
    }
    public class ServiceMethodMetaData
    {
        public string WebMethodName;
        public string XMLdata;
    }
    public class MemberLoginDetails
    {
        public int? CredentialID { get; set; }
        public int? MemberID { get; set; }
        public int? MemberParentID { get; set; }
        public string UserName { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Password { get; set; }
        public string UserStatus { get; set; }
        public int? WrongLoginCount { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModfiedDate { get; set; }
        public bool? IsTwofactorAuthentication { get; set; }
        public string PreferredIP { get; set; }
        public int? TwoFactorType { get; set; }
        public string Primary_Phone { get; set; }
        public string OTP { get; set; }
        public string City { get; set; }
        public string State_Name { get; set; }
        public string Zip { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string DateOfBirth { get; set; }
        public int? CountryCode { get; set; }
        public string MI { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public int? SubscriptionFlag { get; set; }
        public string RaceEthnicity { get; set; }
        public int? UserID { get; set; }
        public string MemberCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }
        public DateTime? DOB { get; set; }
        public string MobileNumber { get; set; }
        public int? CountryID { get; set; }
        public string CountryName { get; set; }
        public int? StateID { get; set; }
        public string StateName { get; set; }
        public int? CityID { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public int? OrganizationID { get; set; }
        public string Organizationname { get; set; }
        public int? OrganizationType { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsActive { get; set; }

        public string StripeCustomerID { get; set; }
        public int OrganizationUserTandCFlag { get; set; }
        public int OrganizationTandCFlag { get; set; }
    }

    public class MemberDetails
    {
        public int? MemberPlanID { get; set; }
        public int? MemberID { get; set; }
        public string MemberName { get; set; }
        public string OrganizationName { get; set; }
        public int? OrganizationID { get; set; }
        public string ProviderName { get; set; }
        public int? ProviderID { get; set; }
        public string PlanName { get; set; }
        public int? PlanID { get; set; }
        public string PlanStartDate { get; set; }
        public string PlanEnddate { get; set; }
        public string PaymentInterval { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? AmountPaid { get; set; }
        public decimal? DueAmount { get; set; }
        public decimal? Discount { get; set; }
        public string Status { get; set; }
        public int? Duration { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
    public class PlansCreation
    {
        [Required(ErrorMessage = "This information is required.")]
        public string PlanName { get; set; }
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public string PlanCode { get; set; }  // eg. PLAN101
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public string PlanFee { get; set; }
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public string MonthlyFee { get; set; }
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public string OfficeVisitFee { get; set; }
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public DateTime? EffectiveDate { get; set; }  // eg. Plan Start Date
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public string Gender { get; set; }  // eg. Plan applicable for M, F or Both
        /*********************/
        public int? PlanTenureID { get; set; }
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public string PlanTenure { get; set; }  // eg. Plan Duration
        /*********************/
        public int? PlanTypeID { get; set; }
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public string PlanType { get; set; }   // eg. Family or Individual
        /*********************/
        public int? PlanCategoryID { get; set; }
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public string PlanCategory { get; set; }  // eg. Domestic or International       
        /*********************/
        public int? OrganizationID { get; set; }
        /*********************/
        public string OrganizationName { get; set; }
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public string PlanAmount { get; set; }
        /*********************/
        public int PlanDurationID { get; set; }
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public string PlanDuration { get; set; }  // eg. Plan Duration
        /*********************/
        public int DurationInMonthsID { get; set; }
        /*********************/
        public string DurationInMonths { get; set; }  // eg. If Plan Duration less than 1 yr
        /*********************/
        public string PrimaryUrl { get; set; }
        /*********************/
        public string PlanDescription { get; set; }
        /*********************/
        public string PlanFeatures { get; set; }
        /*********************/
        public string MemberFeatures { get; set; }
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public DateTime? PlanStartDate { get; set; }  // eg. Plan Start Date
        /*********************/
        [Required(ErrorMessage = "This information is required.")]
        public DateTime? PlanEndDate { get; set; }  // eg. Plan End Date
    }
    public class ForgotCredentialsModel
    {
        [RegularExpression(@"(?=^.{8,16}$)((?=.*\d)|(?=.*\W+))(?![.\n])(?=.*[a-z]).*$", ErrorMessage = "Minimum 8 characters, allows Characters,Special Characters and Numbers")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "This information is required")]
        public string ConfirmPassword { get; set; }

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
        //[Required(ErrorMessage = "This information is required.")]

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


    public class OrganizationUserDetails
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public Nullable<int> ParentOrganizationID { get; set; }
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
        public Nullable<int> Type { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salutation { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        public string OrgUserEmail { get; set; }
        public string OrgUserCountryCode { get; set; }
        public string OrgUserMobileNumber { get; set; }
        public Nullable<int> OrgUserCountryID { get; set; }
        public string OrgUserCountryName { get; set; }
        public Nullable<int> OrgUserStateID { get; set; }
        public string OrgUserStateName { get; set; }
        public Nullable<int> OrgUserCityID { get; set; }
        public string OrgUserCityName { get; set; }
        public string OrgUserAddress { get; set; }
        public string OrgUserZip { get; set; }
    }



    public class AddDoctor
    {
        public string ProviderID { get; set; }
        public string SpecializationID { get; set; }
        public string SpecializationName { get; set; }
        public string Specialization { get; set; }

        public int OrganizationID { get; set; }

        public string OrganizationName { get; set; }

        [Required(ErrorMessage = "This Information is Requried")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "This Information is Requried")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "This Information is Requried")]
        public string Salutation { get; set; }
        public int? SalutationID { get; set; }

        [Required(ErrorMessage = "This Information is Requried")]
        public DateTime DOB { get; set; }

        [Required(ErrorMessage = "This Information is requried")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "This Information is Requried.")]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage = "Incorrect Format")]
        public string Email { get; set; }

        public string CountryCode { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Incorrect Format")]
        public string Zip { get; set; }

        [RegularExpression(@"^\d{4}$", ErrorMessage = "Incorrect Format")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Incorrect Format")]
        public string NPI { get; set; }

        public Nullable<int> CountryID { get; set; }

        [Required(ErrorMessage = "This Information is Requried.")]
        public string CountryName { get; set; }

        public Nullable<int> StateID { get; set; }

        [Required(ErrorMessage = "This Information is Requried.")]
        public string StateName { get; set; }

        public Nullable<int> CityID { get; set; }

        [Required(ErrorMessage = "This Information is Requried.")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "This Information is Requried")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Incorrect Format")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "This Information is Requried")]
        [RegularExpression(@"^[A-Za-z0-9_@./#&+-]*$", ErrorMessage = "Invalid  Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This Information is Requried.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This Information is Requried.")]
        public string ConfirmPassword { get; set; }

        public bool IsTwofactorAuthentication { get; set; }

        public string PreferredIP { get; set; }

        public int? TwoFactorType { get; set; }

        public int? OrgIDP { get; set; }
        public string Fax { get; set; }

        public string Address { get; set; }

        public string Degree { get; set; }
    }
    public class AddMemberDetails
    {
        public int? UserID { get; set; }
        public int MemberID { get; set; }

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

        //public int SalutationID { get; set; }
        //[Required(ErrorMessage = "This information is required.")]
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

        public Nullable<int> CityID { get; set; }

        [Required(ErrorMessage = " This information is required.")]
        public string StateName { get; set; }

        [Required(ErrorMessage = "This information is required .")]
        public string CityName { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Incorrect Format")]
        public string Zip { get; set; }

        [RegularExpression(@"^\d{4}$", ErrorMessage = "Incorrect Format")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[A-Za-z0-9_@./#&+-]*$", ErrorMessage = "Invalid  Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        [RegularExpression(@"^[A-Za-z0-9_@./#&+-]*$", ErrorMessage = "Invalid  Username")]
        public string ConfirmUserName { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "This information is required.")]
        public string OrganizationName { get; set; }

        public int OrganizationID { get; set; }

        public int RelationshipID { get; set; }
        public string RelationshipName { get; set; }
        public int Type { get; set; }
        public bool IsTwofactorAuthentication { get; set; }
        public string PreferredIP { get; set; }
        public int? TwoFactorType { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public DateTime PlanStartDate { get; set; }

        public decimal? AmountPaid { get; set; }
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
    }
    public class ViewPaymentDetails
    {
        public int OrganizationID { get; set; }
        public int PaymentDetailsID { get; set; }
        public Nullable<int> MemberPlanID { get; set; }
        public Nullable<int> MemberID { get; set; }
        public Nullable<int> MemberParentID { get; set; }
        public string Membername { get; set; }
        public Nullable<int> PlanID { get; set; }
        public string PlanName { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public string TransactionID { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<decimal> PaidAmount { get; set; }
        public Nullable<decimal> NetAmount { get; set; }
        public Nullable<decimal> TransactionFee { get; set; }
        public string PaymentStatus { get; set; }
        public string OrganizationName { get; set; }
    }
    public class OrganizationPlansDetails
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
        public int? OrgID { get; set; }
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
        public DateTime EffectiveDate { get; set; }

        public virtual MemberDetails Member { get; set; }

        public Nullable<decimal> MonthlyFee{ get; set; }
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
        [Required(ErrorMessage = "This information is required.")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> OrgPlanStartDate { get; set; }
        public Nullable<System.DateTime> OrgPlanEndDate { get; set; }
        public Nullable<System.DateTime> ProviderPlanStartDate { get; set; }
        public Nullable<System.DateTime> ProviderPlanEndDate { get; set; }
        public string PlanDescription { get; set; }

        public string PStartDate { get; set; }
        public string PEndDate { get; set; }

        public string Paymentschedule { get; set; }

        public int? NoofInstallments { get; set; }
        public decimal? InstallmentAmount { get; set; }
        public decimal? InstallmentFee { get; set; }
        public decimal? Savings { get; set; }
        public string PatientTAndCPath { get; set; }
        public string OrganizationTAndCPath { get; set; }
        public string ProviderTAndCPath { get; set; }
        public int MapID { get; set; }
    }

    public class PlansAndPlansMapping
    {

        public int? PlanID { get; set; }
        public string PlanCode { get; set; }
        public string PlanName { get; set; }
        public Nullable<decimal> MonthlyFee { get; set; }
        public string PaymentIntervals { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
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
        public Nullable<System.DateTime> EffectiveDate { get; set; }
        public Nullable<int> PlanMemberType { get; set; }
        public Nullable<decimal> PlanFeeAddMember { get; set; }
        public Nullable<decimal> CommPrimaryMember { get; set; }
        public Nullable<decimal> CommAdditionalMember { get; set; }
        public Nullable<bool> IsThirdParty { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public Nullable<int> ProviderID { get; set; }

        public string ProviderName { get; set; }
        public string OrganizationName { get; set; }
        public string Features { get; set; }
        public string Patient_Features { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public int? OrgID { get; set; }
        public string OrgName { get; set; }

    }
    //model for provider login
   
    public class ProviderDetails
    {
        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string NPI { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string Salutation { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Gender { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string Email { get; set; }
        public string CountryCode { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string MobileNumber { get; set; }
        public Nullable<int> CountryID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string CountryName { get; set; }
        public Nullable<int> StateID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string StateName { get; set; }
        public Nullable<int> CityID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string CityName { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string Zip { get; set; }
        public string ZipCode { get; set; }


        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsTwofactorAuthentication { get; set; }
        public string PreferredIP { get; set; }
        public Nullable<int> TwoFactorType { get; set; }
        public Nullable<bool> IS_TERMS_ACCECPTED { get; set; }
        public string Tax_No { get; set; }
        public Nullable<int> No_of_Patients_Accepting { get; set; }
        public string Specialization { get; set; }
        public string SpecializationName { get; set; }
        public string SpecializationID { get; set; }

        public string OTP { get; set; }
        public string Fax { get; set; }

        public string Degree { get; set; }

      
    }

    public class OrganizationProviderPlans
    {
        public int OrganizationID { get; set; }
        public int PlanID { get; set; }
        public int ProviderID { get; set; }
        public string OrganizationName { get; set; }
        public string ProviderName { get; set; }
        public string PlanName { get; set; }
    }

    public class MemberAutoComplete
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }
    }
    public class TermsAndConditions
    {
        public string  OrganizationUsers { get; set; }
        public string Organization { get; set; }

    }
    public class PPCPReports
    {
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProviderName { get; set; }
        public string ProviderID { get; set; }
        public string DOB { get; set; }
        public string PlanName { get; set; }
        public string PlanStartDate { get; set; }
        public string PlanEndDate { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public Nullable<decimal> DueAmount { get; set; }
        public string PaymentDate { get; set; }
        public string MobileNumber { get; set; }
        public string PaymentStatus { get; set; }
        public string PlanType { get; set; }
        public string OrganizationID { get; set; }
        public string OrganizationName { get; set; }
    }

    public class TransactionstoPractice
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }
        public string PlanName { get; set; }
        public string DOB { get; set; }
        public string DoctorName { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }
        public string PaymentDate { get; set; }
        public int OrganizationID { get; set; }
        public int MemberPlanID { get; set; }
        public int ID { get; set; }
        public string OrganizationName { get; set; }
    }

    //Added by akhil
    public class UploadLabResults
    {
        public UploadLabResults()
        {
            Uploadresults = new UpLoadResults();
            RemoveUploadresults = new RemoveUpLoadResults();
        }
        public UpLoadResults Uploadresults
        {
            get;
            set;
        }
        public RemoveUpLoadResults RemoveUploadresults
        {
            get;
            set;
        }
        public class UpLoadResults
        {
            public int OrganizationID { get; set; }
            public string OrganizationName { get; set; }
            public DateTime TestDoneDate { get; set; }
            public string InsertedDate { get; set; }
            //public string FileName { get; set; }
            public string DocumentPath { get; set; }
            public string DocumentPath1 { get; set; }
            public int Id { get; set; }
            [XmlIgnore]
            public IEnumerable<HttpPostedFileBase> Files { get; set; }
            public int RandomID { get; set; }
            [XmlIgnore]
            public string FullPathNamePdf { get; set; }
            public int? LabOrderID { get; set; }
            public int DocumentRepositoryID { get; set; }
            public string DocumentName { get; set; }
            public string DocumentType { get; set; }
            public int VisibiltyFlag { get; set; }
            public bool IsDeleted { get; set; }
            public int InsertedByID { get; set; }
            public int ModifiedByID { get; set; }
            public float DocumentSize { get; set; }
            public int ResultID { get; set; }
            public int AttachmentID { get; set; }
            public int? Type { get; set; }
            public int? ModuleType { get; set; }
            public int MemberID { get; set; }

            public DateTime? AttachmentDate { get; set; }

            public string InsertDate { get; set; }

            public string Document { get; set; }
        }
        public class RemoveUpLoadResults
        {
            public int? LabOrderID { get; set; }
            public bool IsDeleted { get; set; }
            public int AttachmentID { get; set; }
        }
        public class Attachment
        {
            public int? AttachmentID { get; set; }
            public int? LabOrderID { get; set; }
            public int? DocumentRepositoryID { get; set; }
            public string DocumentName { get; set; }
            public string DocumentType { get; set; }
            public string DocumentPath { get; set; }
            public int? VisibiltyFlag { get; set; }
            public bool? IsDeleted { get; set; }
            public int? InsertedByID { get; set; }
            public DateTime? InsertedDate { get; set; }
            public int? ModifiedByID { get; set; }
            public DateTime? ModifiedDate { get; set; }
            public decimal? DocumentSize { get; set; }
            public int RandomID { get; set; }
            public int? Type { get; set; }
            public int? OrganizationID { get; set; }
            public DateTime? AttachmentDate { get; set; }
            public int? ModuleType { get; set; }
            public string ResultsDate { get; set; }
        }


    }
}
