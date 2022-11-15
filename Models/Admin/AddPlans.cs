using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPCP07302018.Models.Admin
{
    public class AddPlans
    {


        public int PlanID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string PlanCode { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string PlanName { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<decimal> MonthlyFee { get; set; }

        public string PaymentIntervals { get; set; }

        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }

        public Nullable<int> CreatedBy { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }

        public Nullable<int> ModifiedBy { get; set; }

        public Nullable<int> ModifiedDate { get; set; }

        public string PlanTermName { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<int> PlanTermMonths { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<decimal> VisitFee { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<decimal> EnrollFee { get; set; }
        [Required(ErrorMessage = "This information is required.")]

        public Nullable<decimal> InstalmentFee { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<int> FromAge { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<int> ToAge { get; set; }
        public Nullable<int> GenderID { get; set; }

        public Nullable<int> PlanType { get; set; }

        public Nullable<bool> PlanStatus { get; set; }

        public Nullable<System.DateTime> PlanStartDate { get; set; }

        public Nullable<System.DateTime> PlanEndDate { get; set; }
        public Nullable<System.DateTime> UnsubscribeDate { get; set; }
        //[Required(ErrorMessage = "This information is required.")]
        public string PlanDescription { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<System.DateTime> EffectiveDate { get; set; }

        public Nullable<int> PlanMemberType { get; set; }
        //[Required(ErrorMessage = "This information is required.")]
        public Nullable<decimal> PlanFeeAddMember { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<decimal> CommPrimaryMember { get; set; }
        //[Required(ErrorMessage = "This information is required.")]
        public Nullable<decimal> CommAdditionalMember { get; set; }

        public Nullable<bool> IsThirdParty { get; set; }

        public Nullable<int> OrganizationID { get; set; }
        //[Required(ErrorMessage = "This information is required.")]
        public string OrganizationName { get; set; }
        //[Required(ErrorMessage = "This information is required.")]
        public string Features { get; set; }
        //[Required(ErrorMessage = "This information is required.")]
        public string Patient_Features { get; set; }

        public string PrimaryUrl { get; set; }
        public string PatientTermsandCondition { get; set; }
        public string OrganizationTermsandCondition { get; set; }
        public string ProviderTermsandCondition { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<decimal> Amountforpractice { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public Nullable<decimal> CommPPCP { get; set; }
    }
    public class PlanMapping
    {
        public Nullable<int> OrganizationID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string OrganizationName { get; set; }
        public Nullable<int> ProviderID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string ProviderNames { get; set; }
        public Nullable<int> PlanID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string PlanName { get; set; }


    }

    public class ProviderDetails
    {
        public int ProviderID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
        public string ProviderName { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        [Required(ErrorMessage = "This information is required.")]
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
    }
    public class WidthdrawPlans
    {
        public int PlanID { get; set; }
    }
    public class TermsConditions
    {
        public int TermsAndConditionsID { get; set; }
        public string PatientTermsAndConditionsName { get; set; }
        public string OrganizationTermsAndConditionsName { get; set; }
        public string UsersTermsAndConditionsName { get; set; }
        public string TempletPath { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> Type { get; set; }


    }
    public class ProviderAutoComplete
    {
        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
    }
    public class MemberAutoComplete
    {
        public int MemberID { get; set; }
        public string MemberName { get; set; }
    }
    public class OrganizationAutoComplete
    {
        public int OrganizationID { get; set; }
        public string OrganizationName { get; set; }
    }
    public class OrganizationUserAutoComplete
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
    }
    public partial class PlansMapping
    {
        public int PlanMapID { get; set; }
        public int PlanID { get; set; }
        public string PlanName { get; set; }
        public Nullable<int> ProviderID { get; set; }
        public string ProviderName { get; set; }
        public int OrganizationID { get; set; }
        public string Organizationname { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> PlanStartDate { get; set; }
    }
    public class OrganizationPlan
    {   
        public string PlanName { get; set; }        
        public string CreatedDate { get; set; }
        public string PlanstartDate { get; set; }
        public string OrganizationName { get; set; }       
    }
}