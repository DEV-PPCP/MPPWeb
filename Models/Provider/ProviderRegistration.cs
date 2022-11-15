using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PPCP07302018.Models.Provider
{
    public class ProviderRegistration
    {
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
        public int SalutationID { get; set; }

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
        [Required(ErrorMessage = "This information is required.")]

        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Incorrect Format")]
        public string Fax { get; set; }
        [Required(ErrorMessage = "This information is required.")]

        public string Address { get; set; }

        public string Degree { get; set; }

        //public int? OrgIDP { get; set; }

    }
    public class ProviderDetails
    {
        public int ProviderID { get; set; }
        public Nullable<int> OrganizationID { get; set; }
        public string OrganizationName { get; set; }
        public string NPI { get; set; }
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
}
