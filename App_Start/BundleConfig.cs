using System.Web;
using System.Web.Optimization;


namespace PPCP07302018
{
    public class BundleConfig
    {
        
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/bundles/Kendocss").Include(
                "~/Content/kendo/2014.3.1316/kendo.common.min.css", "~/Content/kendo/2014.3.1316/kendo.mobile.all.min.css",
                "~/Content/kendo/2014.3.1316/kendo.dataviz.min.css", "~/Content/kendo/2014.3.1316/kendo.default.min.css",
                "~/Content/kendo/2014.3.1316/kendo.dataviz.default.min.css", "~/Content/master.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/masterCss").Include("~/Content/master.css"
                ));

            bundles.Add(new ScriptBundle("~/bundles/KendoScripts").Include("~/Scripts/kendo/2014.3.1316/jquery.min.js",
                "~/Scripts/kendo/2014.3.1316/jszip.min.js", "~/Scripts/kendo/2014.3.1316/kendo.all.min.js",
                "~/Scripts/kendo/2014.3.1316/kendo.aspnetmvc.min.js", "~/Scripts/kendo/2014.3.1316/kendo.modernizr.custom.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/custom-validator").Include(
                                  "~/Scripts/script-custom-validator.js"));
            bundles.Add(new ScriptBundle("~/bundles/MemberScriptFiles").Include(
                                 "~/Scripts/Member.js"));
          
            bundles.Add(new StyleBundle("~/bundles/Membercssfiles").Include(
                                 "~/Content/Member.css", "~/Content/bootstrap.min.css"));


            bundles.Add(new ScriptBundle("~/bundles/OrganizationScriptFiles").Include(
                                 "~/Scripts/Organization.js"));
            bundles.Add(new StyleBundle("~/bundles/Organizationcssfiles").Include(
                                 "~/Content/Member.css", "~/Content/bootstrap.min.css"));
            bundles.Add(new ScriptBundle("~/bundles/FamilyMeberScriptFiles").Include(
                             "~/Scripts/FamilyMember.js"));
            bundles.Add(new ScriptBundle("~/bundles/FamilyRegistrationScriptFiles").Include(
                               "~/Scripts/FamilyRegistration.js"));
            bundles.Add(new ScriptBundle("~/bundles/PlanEnrollScriptFiles").Include(
                             "~/Scripts/PlanEnroll.js"));
            bundles.Add(new ScriptBundle("~/bundles/MemberCredentialsFiles").Include(
                           "~/Scripts/MemberCredentials.js"));
            bundles.Add(new ScriptBundle("~/bundles/ChangePasswordFiles").Include(
                        "~/Scripts/ChangePassword.js"));
            bundles.Add(new ScriptBundle("~/bundles/PaymentDetailsFiles").Include(
                      "~/Scripts/PaymentDetails.js"));
            bundles.Add(new ScriptBundle("~/bundles/ViewMemberFiles").Include(
                      "~/Scripts/ViewMember.js"));
            bundles.Add(new ScriptBundle("~/bundles/PlanDetailsScriptFiles").Include(
                    "~/Scripts/PlanDetails.js"));
            bundles.Add(new ScriptBundle("~/bundles/OrganizationCredentialsScriptFiles").Include(
                   "~/Scripts/OrganizationCredentials.js"));
            bundles.Add(new ScriptBundle("~/bundles/PartialPaymentsScriptFiles").Include(
                "~/Scripts/PartialPayments.js"));

            bundles.Add(new ScriptBundle("~/bundles/OrganizationProfileDetailsScriptFiles").Include(
               "~/Scripts/OrganizationProfileDetails.js"));
            //Addmember : Added by Veena
            bundles.Add(new ScriptBundle("~/bundles/AddmemberScriptFiles").Include(
              "~/Scripts/Addmember.js"));
            //ViewMemberDetails : Added by Veena
            bundles.Add(new ScriptBundle("~/bundles/ViewMemberDetailsScriptFiles").Include(
             "~/Scripts/ViewMemberDetails.js"));
            //ViewPaymentDetails :  Added by Veena
            bundles.Add(new ScriptBundle("~/bundles/ViewPaymentDetailsScriptFiles").Include(
           "~/Scripts/ViewPaymentDetails.js"));
            //ViewUsers :  Added by Veena
            bundles.Add(new ScriptBundle("~/bundles/ViewUsersScriptFiles").Include(
           "~/Scripts/ViewUsers.js"));
            //ViewProviders :  Added by Veena
            bundles.Add(new ScriptBundle("~/bundles/ViewProvidersScriptFiles").Include(
           "~/Scripts/ViewProviders.js"));
            //ViewProvider login details :  Added by Veena
            bundles.Add(new ScriptBundle("~/bundles/ViewProviderScriptFiles").Include(
           "~/Scripts/ViewProvider.js"));
            //WithdrawPlans :  Added by Veena
            bundles.Add(new ScriptBundle("~/bundles/WithdrawPlansScriptFiles").Include(
           "~/Scripts/WithdrawPlans.js"));
            //AddUsers :  Added by Veena
            bundles.Add(new ScriptBundle("~/bundles/AddUsersScriptFiles").Include(
           "~/Scripts/AddUsers.js"));
            //ProviderCredentials :  Added by Veena
            bundles.Add(new ScriptBundle("~/bundles/ProviderCredentialsScriptFiles").Include(
           "~/Scripts/ProviderCredentials.js"));
            //AdminSubscribePlans :  Added by Veena
            bundles.Add(new ScriptBundle("~/bundles/AdminSubscribePlansScriptFiles").Include(
           "~/Scripts/AdminSubscribePlans.js"));
            //DiabledOrganizations :  Added by Veena Created on :05-11-2019
            bundles.Add(new ScriptBundle("~/bundles/DiabledOrganizationsScriptFiles").Include(
           "~/Scripts/DiabledOrganizations.js"));

            //EmployersRegistration :  Added by Veena Created on :06-11-2019
            bundles.Add(new ScriptBundle("~/bundles/EmployersRegistrationScriptFiles").Include(
           "~/Scripts/EmployersRegistration.js"));
            //EmployerForgotPassword :  Added by Veena Created on :06-11-2019
            bundles.Add(new ScriptBundle("~/bundles/EmployerForgotPasswordScriptFiles").Include(
           "~/Scripts/EmployerForgotPassword.js"));

            //ViewPlan js file  :Added By: Ragini Created on :13-09-2019
            bundles.Add(new ScriptBundle("~/bundles/ViewPlansScriptFiles").Include(
           "~/Scripts/ViewPlans.js"));
            //ViewPlan js file  :Added By: Ragini Created on :16-09-2019
            bundles.Add(new ScriptBundle("~/bundles/SubscribePlanScriptFiles").Include(
           "~/Scripts/SubscribePlans.js"));
            //AddPlans js file  :Added By: Ragini Created on :18-09-2019
            bundles.Add(new ScriptBundle("~/bundles/AddPlansScriptFiles").Include(
           "~/Scripts/AddPlans.js"));
            //ViewOrganization js file  :Added By: vinod Created on :21-09-2019
            bundles.Add(new ScriptBundle("~/bundles/ViewOrganizationScriptFiles").Include(
           "~/Scripts/ViewOrganization.js"));
            bundles.Add(new ScriptBundle("~/bundles/PlanMappingScriptFiles").Include(
           "~/Scripts/PlanMapping.js"));
            bundles.Add(new ScriptBundle("~/bundles/AddDoctorsScriptFiles").Include(
                              "~/Scripts/AddDoctors.js"));
            //ProviderRegistration js file  :Added By: Ragini Created on :26-09-2019
            bundles.Add(new ScriptBundle("~/bundles/ProviderRegistrationScriptFiles").Include(
                            "~/Scripts/ProviderRegistration.js"));
            //TermsAndCondition js file  :Added By: Ragini Created on :01-11-2019
            bundles.Add(new ScriptBundle("~/bundles/TermsAndConditionScriptFiles").Include(
                            "~/Scripts/TermsAndConditions.js"));
            bundles.Add(new ScriptBundle("~/bundles/PendingPlanEnrollmentScriptFiles").Include(
                           "~/Scripts/PendingPlanEnrollment.js"));
            //Added by akhil
            bundles.Add(new ScriptBundle("~/bundles/AdminMemberListScriptFiles").Include(
                           "~/Scripts/AdminMemberList.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdminAddMemberScriptFiles").Include(
                          "~/Scripts/AdminAddMember.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminViewPaymentDetailsScriptFiles").Include(
                          "~/Scripts/AdminViewPaymentDetails.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminAddMemebrScriptFiles").Include(
                          "~/Scripts/AdminAddMember.js"));
            //BundleTable.EnableOptimizations = true;
            //BundleTable.EnableOptimizations = true;


        }
    }
}