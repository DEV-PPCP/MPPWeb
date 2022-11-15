using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PPCP07302018.Models.Provider
{
    public class ProviderLogin
    {
        public string SpecializationID { get; set; }
        public string SpecializationName { get; set; }
        public string Specialization { get; set; }
        public int ProviderID { get; set; }
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
        public string NPI { get; set; }
        public string SalutationID { get; set; }

        public string Fax { get; set; }

        public string Degree { get; set; }

       

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
    public class ProviderAutoComplete
    {
        public int ProviderID { get; set; }
        public string ProviderName { get; set; }
    }
}