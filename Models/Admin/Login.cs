using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PPCP07302018.Models.Admin
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public string OTP { get; set; }
        public string LastIPAddress { get; set; }
        public string UserStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsAccountLocked { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public DateTime? LastPasswordChangeDate { get; set; }
        public bool IsTwoFactorEnabled { get; set; }
        public int? TwoFactorType { get; set; }
        public bool IsTermsNeeded { get; set; }
        public bool IsTermsAccepted { get; set; }
        public DateTime? TermsAcceptDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AssociatedEntityId { get; set; }

        //additional properties
        public string RoleName { get; set; }
        public string RoleType { get; set; }
        public string TermsAndConditionsFile { get; set; }
        //public int TermsAndConditionsNeeded { get; set; }

        //additional org details
        public int? OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        //public DateTime? OrganizationUserTandC { get; set; }
        //public DateTime? OrganizationTandC { get; set; }
        //public int OrganizationUserTandCFlag { get; set; }
        //public int OrganizationTandCFlag { get; set; }

        //additional member details
        public int? MemberID { get; set; }
        public int? MemberParentID { get; set; }
        //public DateTime? MemberTandCDate { get; set; }
        //public int MemberTermsAndConditionsFlag { get; set; }
        public string StripeCustomerID { get; set; }

        //provider details
        public int? ProviderID { get; set; }

    }

    public class Login
    {
        public string LoginType { get; set; }
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
    }

    public class TermsAndCondition
    {
        public int TermsAndConditionsID { get; set; }
        public string TermsAndConditionsName { get; set; }
        public string TempletPath { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> Type { get; set; }
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
        public string ProviderName { get; set; }
        public string OrganizationName { get; set; }
    }
}