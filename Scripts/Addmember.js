function setParameter(parameterName, methodName) {
    var obj = new Object();
    obj.WebMethodName = methodName;
    obj.XMLdata = parameterName;
    var resultData = JSON.stringify(obj);
    return resultData;
}
function setJsonParameter(parameterName, parameterValue, methodName) {
    var obj = new Object();
    obj.ParameterName = parameterName;
    obj.ParameterValue = parameterValue;
    obj.WebMethodName = methodName;
    var resultData = JSON.stringify(obj);
    resultData = getformattedJsonFromArray(resultData);
    return resultData;
}
function getformattedJsonFromArray(arrayObj) {
    arrayObj = arrayObj.replace(/"/g, "'");
    return arrayObj + "";
}
//To bind Countries dropdownlist by Veena
function BindCountries(Url) {
    var webMethodName = "GetCountries";
    var ParameterNames = "";
    var Url = Url + "DefaultService";
    var jsonPostString = setParameter(ParameterNames, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var countriesList = obj[0];
            $('#CountryNameList').data('kendoDropDownList').dataSource.data();
            for (var r in countriesList) {
                $('#CountryNameList').data('kendoDropDownList').dataSource.insert(r, { Text: countriesList[r].CountryName, Value: countriesList[r].CountryID });
            }
            var countryID = 1;

            if (countryID != 0) {

                var dropdownlist = $("#CountryNameList").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {
                    $('#CountryNameList').data('kendoDropDownList').value("1");
                    //$('#CountryID').val("1");
                    //$("#CountryName").val("United States of America");
                    return dataItem.Value === countryID;
                });
                BindStates(countryID, Url)
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//Bind States to AddMember by Veena
function BindStates(CountryID, Url) {
    var webMethodName = "GetStates";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "CountryID";
    ParameterValues[0] = CountryID;
    var Url = Url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var StatesListList = obj[0];
           
            $('#StateNameList').data('kendoDropDownList').dataSource.data(StatesListList);
            var stateID = 1;

            if (stateID != 0) {

                var dropdownlist = $("#StateNameList").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {
                    $('#StateNameList').data('kendoDropDownList').value("0");
                    return dataItem.Value === stateID;
                });
                BindCities(stateID, Url)
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//To bind Cities dropdownlist based on StateID by Veena
function BindCities(StateID, Url) {
    var webMethodName = "GetCities";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "StateID";
    ParameterValues[0] = StateID;
    var Url = Url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var CitiesListList = obj[0];
            $('#CityNameList').data('kendoDropDownList').dataSource.data(CitiesListList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
// To Get IPAddress in AddMember by Veena
function GetIpAddress() {
    $.ajax({
        url: "/Master/GetIPAddress/",
        data: {},
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//Add Member Details by veena
function AddMembeDetails(MemberRegistrationDetails, session, Url) {
    MemberRegistrationDetails.MemberID = $("#hdnMemberID").val();
    MemberRegistrationDetails.FirstName = $("#FirstName").val();
    MemberRegistrationDetails.LastName = $("#LastName").val();
    MemberRegistrationDetails.DOB = $("#DOB").val();
    MemberRegistrationDetails.Age = $("#Age").val();
    var rbtnMale = $('input[id=rbtnGenderMale]:checked').val();
    if (rbtnMale == 1)
        MemberRegistrationDetails.Gender = rbtnMale;
    var rbtnFEM = $('input[id=rbtnGenderFemale]:checked').val();
    if (rbtnFEM == 2)
        MemberRegistrationDetails.Gender = rbtnFEM;
    MemberRegistrationDetails.SalutationID = $("#SalutationID").val();
    MemberRegistrationDetails.Salutation = $("#Salutation").val();
    MemberRegistrationDetails.Email = $("#Email").val();
    MemberRegistrationDetails.CountryCode = $("#CountryCode").val();
    MemberRegistrationDetails.MobileNumber = $("#MobileNumber").val();
    MemberRegistrationDetails.CountryID = $("#CountryID").val();
    MemberRegistrationDetails.CountryName = $("#CountryName").val();
    MemberRegistrationDetails.StateID = $("#StateID").val();
    MemberRegistrationDetails.StateName = $("#StateName").val();
    MemberRegistrationDetails.CityID = $("#CityID").val();
    MemberRegistrationDetails.CityName = $("#CityName").val();
    if ($("#ZipCode").val() == "") {
        MemberRegistrationDetails.Zip = $("#Zip").val();
    }
    else {
        MemberRegistrationDetails.Zip = $("#Zip").val() + "-" + $("#ZipCode").val();
    }
    MemberRegistrationDetails.UserName = $("#UserName").val();
    MemberRegistrationDetails.Password = $("#Password").val();
    MemberRegistrationDetails.ConfirmPassword = $("#ConfirmPassword").val();

    if ($('#chk2factor').is(":checked") == true) {
        MemberRegistrationDetails.IsTwofactorAuthentication = true;
        MemberRegistrationDetails.PreferredIP = session;
        var rbtnEvery = $('input[id=rbtnEverytime]:checked').val();
        if (rbtnEvery == 1)
            MemberRegistrationDetails.TwoFactorType = rbtnEvery;
        var rbtnSystem = $('input[id=rbtnOnlySysChange]:checked').val();
        if (rbtnSystem == 2)
            MemberRegistrationDetails.TwoFactorType = rbtnSystem;
    }
    else {
        MemberRegistrationDetails.IsTwofactorAuthentication = false;
        MemberRegistrationDetails.TwoFactorType = "0";
        MemberRegistrationDetails.PreferredIP = "";
    }
    debugger;
    if ($('#chkEnroll').is(":checked") == true) {
        //Added by akhil
        MemberRegistrationDetails.MemberRegistrationType = "ByOrg";
        MemberRegistrationDetails.StripeCustomerID = $("#StripeCustomerID").val();
        MemberRegistrationDetails.OrganizationName = document.getElementById("spnSelectedOrganization").innerText;
        MemberRegistrationDetails.OrganizationID = document.getElementById("SpOrganizationID").innerText;
        MemberRegistrationDetails.BillingTypeID = document.getElementById("SpBillingTypeID").innerText;
        MemberRegistrationDetails.ProviderName = document.getElementById("SpProviderName").innerText;
        MemberRegistrationDetails.ProviderID = document.getElementById("SpProviderID").innerText;
        MemberRegistrationDetails.PlanName = document.getElementById("spnSelectedPlan").innerText;
        MemberRegistrationDetails.PlanID = document.getElementById("PlanID").innerText;
        MemberRegistrationDetails.PlanStartDate = $("#PlanStartDate").val();
        MemberRegistrationDetails.PlanEndDate = $("#spnPlanEndDate").val();
        MemberRegistrationDetails.Paymentschedule = document.getElementById("Paymentschedule").innerText;
        MemberRegistrationDetails.NoofInstallments = document.getElementById("spnNoofInstalments").innerText;
        MemberRegistrationDetails.InstallmentAmount = document.getElementById("spnInstallmentAmount").innerText;
        MemberRegistrationDetails.InstallmentFee = document.getElementById("InstallmentFee").innerText;
        MemberRegistrationDetails.StripeAccountID = document.getElementById("spnStripeAccountID").innerText;
        MemberRegistrationDetails.Savings = document.getElementById("Savings").innerText;
        MemberRegistrationDetails.CardID = $("#CardID").val();
        MemberRegistrationDetails.CardNumber = $("#CardNumber").val();
        MemberRegistrationDetails.NameOnCard = $("#NameOnCard").val();
        MemberRegistrationDetails.Amount = document.getElementById("spnInstallmentAmount").innerText;
        MemberRegistrationDetails.MM = $("#MM").val();
        MemberRegistrationDetails.YY = $("#YY").val();
        MemberRegistrationDetails.CVV = $("#CVV").val();
        MemberRegistrationDetails.CommPPCP = document.getElementById("spnCommPPCP").innerText;
        MemberRegistrationDetails.CommPrimaryMember = document.getElementById("spnCommPrimaryMember").innerText;
        MemberRegistrationDetails.EnrollFee = document.getElementById("spnEnrollFee").innerText;
        MemberRegistrationDetails.Duration = document.getElementById("spnDuration").innerText;
        MemberRegistrationDetails.MemberType = "1";
        // var tAmount = document.getElementById("spnPlanAmount").innerText;
        var tAmount = parseInt(document.getElementById("spnPlanAmount").innerText) + parseInt(MemberRegistrationDetails.InstallmentFee) + parseInt(MemberRegistrationDetails.EnrollFee);
      
        MemberRegistrationDetails.TotalAmount = tAmount;
        var amountpaid = $("#AmountPaid").val();
        MemberRegistrationDetails.AmountPaid = $("#AmountPaid").val();
        if (amountpaid == "") {
            MemberRegistrationDetails.AmountPaid = 0;
            MemberRegistrationDetails.DueAmount = tAmount;
            MemberRegistrationDetails.Status = "Pending";
        }
        else {
            MemberRegistrationDetails.AmountPaid = $("#AmountPaid").val();
            var dueamount = tAmount - amountpaid;
        
            if (dueamount == 0.0) {
                MemberRegistrationDetails.DueAmount = dueamount;
                MemberRegistrationDetails.Status = "Paid";
            }
            else {
                MemberRegistrationDetails.DueAmount = dueamount;
                MemberRegistrationDetails.Status = "Partially Paid";
            }
        }
        //End
        if (MemberRegistrationDetails != null) {
            AddMemberRegistrationwithPlandetails(MemberRegistrationDetails, Url);
        }
        else {
            return false;
        }
    }
    if ($('#chkEnroll').is(":checked") == false) {
        if (MemberRegistrationDetails != null) {
            AddMemberRegistration(MemberRegistrationDetails, Url);
        }
        else {
            return false;
        }
    }
}

//To generate Add Member Registration xml by veena
function AddMemberRegistration(MemberRegistrationDetails, Url) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Organization/AddMemberxml',
        data: MemberRegistrationDetails,
        success: function (data, textStatus, jqXHR) {
            debugger;
            AddMemberRegistrationWebApiService(data, Url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function AddMemberRegistrationwithPlandetails(MemberRegistrationDetails, Url) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Master/MemberRegistrationxml',
        data: MemberRegistrationDetails,
        success: function (data, textStatus, jqXHR) {

            AddMemberRegistrationWebApiServicewithPlandetails(data, Url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(MemberRegistration, textStatus);
        },
    });
}

function AddMemberRegistrationWebApiServicewithPlandetails(data, url) {
    debugger;
    var webMethodName = "SaveMemberSignUP";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = url + "MemberXml";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMain"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var obj = result[0];
            $("#divMain").find(".loadingSpinner:first").remove();
            if (obj[0].result == "MemberExists") {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = "Member already exists. You can try searching the member.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            } else {
                if (obj[0].result == null && obj[0].MemberID != null && obj[0].TransactionID != null) {
                    document.getElementById("divSignupPopup").style.display = "block";
                    document.getElementById("spnPopupMessage").innerHTML = "You are registered successfully. Your Transaction ID: " + obj[0].TransactionID;
                    document.getElementById("divSignupPopup").scrollIntoView();
                }
                else if (obj[0].result == null && obj[0].MemberID != null) {
                    document.getElementById("divSignupPopup").style.display = "block";
                    document.getElementById("spnPopupMessage").innerHTML = "You are registered successfully.";
                    document.getElementById("divSignupPopup").scrollIntoView();
                }
                else {
                    document.getElementById("divErrMessagePopup").style.display = "block";
                    document.getElementById("spnPopupErrMessage").innerHTML = obj[0].result + ". Please try again.";
                    document.getElementById("divErrMessagePopup").scrollIntoView();
                }
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            debugger;
            // ErrorMessage(webMethodName, textStatus);
        },
    });
}
//Call the AddMemberDetails web service by veena
function AddMemberRegistrationWebApiService(data, Url) {
    debugger;
    var webMethodName = "AddMemberDetails";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = Url + "Organization";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMain"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var obj = result[0];
            $("#divMain").find(".loadingSpinner:first").remove();
            if (obj[0].result == "MemberExists") {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = "Member already exists. You can try searching the member.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
            else { 
                if (obj[0].result == null && obj[0].UserID != null && obj[0].TransactionID != null) {
                    document.getElementById("divSignupPopup").style.display = "block";
                    document.getElementById("spnPopupMessage").innerHTML = "You are registered successfully. Your Transaction ID: " + obj[0].TransactionID;
                    document.getElementById("divSignupPopup").scrollIntoView();
                }
                else if (obj[0].result == null && obj[0].UserID != null) {
                    document.getElementById("divSignupPopup").style.display = "block";
                    document.getElementById("spnPopupMessage").innerHTML = "You are registered successfully.";
                    document.getElementById("divSignupPopup").scrollIntoView();
                }
                else {
                    document.getElementById("divErrMessagePopup").style.display = "block";
                    document.getElementById("spnPopupErrMessage").innerHTML = obj[0].result + ". Please try again.";
                    document.getElementById("divErrMessagePopup").scrollIntoView();
                }
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//To validate UserName already Exits or Not by Gayathri
function ValidateUserNames(UserName, Url) {
    var webMethodName = "ValidateUserName";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "UserName";
    ParameterValues[0] = UserName;
    var Url = Url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            var obj = result[0];
            if (obj[0].ResultID == 0) {
                document.getElementById("spnUserName").innerHTML = "";
            }
            else {
                document.getElementById("spnUserName").innerHTML = "Username already exists"; return false;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}
//Added by akhil
function CheckMemberExists(FirstName, LastName, Gender, DOB, MobileNumber, Age, url) {
    debugger;
    var tempResult = 0;
    var webMethodName = "CheckMemberExists";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "FirstName";
    ParameterValues[0] = FirstName;
    ParameterNames[1] = "LastName";
    ParameterValues[1] = LastName;
    ParameterNames[2] = "Gender";
    ParameterValues[2] = Gender;
    ParameterNames[3] = "DOB";
    ParameterValues[3] = DOB;
    ParameterNames[4] = "MobileNumber";
    ParameterValues[4] = MobileNumber;
    var Url = url + "Member";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var obj = jQuery.parseJSON(result);
            var val = obj[0];
            if (val >0 ) {//val == 1
                //$("#hdnMemberID").val(val);
                //$("#SearchPlan").show();
                //$("#divSelectPlan").show();
                //$("#EnrollPlan").hide();
                //var OrganizationID = $("#OrganizationID").val();
              //  BindingPlansGrids(OrganizationID, "0", "0", Gender, Age, url);
                document.getElementById("spnExistingMemberValidator").innerHTML = "Member already exists.";
               
            }
            else {

                $("#SearchPlan").show();
                $("#divSelectPlan").show();
                $("#EnrollPlan").hide();
                var OrganizationID = $("#OrganizationID").val();
                BindingPlansGrids(OrganizationID, "0", "0", Gender, Age, url);
                ///BindPlansGrids("0", "0", OrganizationID, Gender, url);
                // BindOrganizations(url);
                //  BindProviderNames(0, url);
                //  BindPlanNames(0, 0, 0, Age, Gender, url);
                document.getElementById("spnExistingMemberValidator").innerHTML = "";
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}

function GetMemberIfExisting(FirstName, LastName, Gender, DOB, MobileNumber, Age, url) {
    debugger;
    var tempResult = 0;
    var webMethodName = "GetMemberIfExisting";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "FirstName";
    ParameterValues[0] = FirstName;
    ParameterNames[1] = "LastName";
    ParameterValues[1] = LastName;
    ParameterNames[2] = "Gender";
    ParameterValues[2] = Gender;
    ParameterNames[3] = "DOB";
    ParameterValues[3] = DOB;
    ParameterNames[4] = "MobileNumber";
    ParameterValues[4] = MobileNumber;
    var Url = url + "Member";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var objlist = jQuery.parseJSON(result);
            var objlist = objlist[0];
            var jsonResult = "";
            var jsonResults = ""
            var OrgId = $("#OrganizationID").val();
            $("#msgMemberExistingInMPP").html("");
            $("#MemberExistsInAnotherOrg").val(false);
            $("#divMemberCredentials").hide();
            $("#divPlanEnrollment").hide();
            $("#MemberCredentials").hide();
            $("#EnrollPlan").hide();
            $("#btnSubmit").hide();
            $("#PlansGrid").data("kendoGrid").dataSource.data("");
            $("#PlansPayment").data("kendoGrid").dataSource.data("");
            if (objlist.length == 0) { //new MPP member
                $("#divMemberCredentials").show();
                $("#MemberCredentials").show();
                $("#divPlanEnrollment").show();
                BindingPlansGridBasedOnFilters($("#OrganizationID").val(), "0", "0", "0", "0", "0", url);
            } else {
                var sameOrgList = objlist.filter(v => v.OrganizationID == OrgId);
                //var sameOrgList = $.grep(objlist, function (v) {
                //    return v.OrganizationID === OrgId;
                //});
                if (sameOrgList.length > 0) {                    
                    document.getElementById("divErrMessagePopup").style.display = "block";
                    document.getElementById("spnPopupErrMessage").innerHTML = "Member already exists in your facility. You can try searching the member.";
                    //document.getElementById("divErrMessagePopup").scrollIntoView();
                } else {
                    //Member already exists in MyPhysicianPlan. Can add but no need for credentials
                    $("#divMemberCredentials").show();
                    $("#MemberExistsInAnotherOrg").val(true);
                    $("#msgMemberExistingInMPP").html("Member exists in MyPhysicianPlan. ");
                    $("#StripeCustomerID").val(objlist[0].StripeCustomerID);
                    $("#divPlanEnrollment").show();
                    BindingPlansGridBasedOnFilters($("#OrganizationID").val(), "0", "0", "0", "0", "0", url);                    
                }
            }
            //for (var r in objlist) {
            //    jsonResult += "<input type='radio' name='" + "AccountType" + "' value='" + objlist[r].MemberID + "' onclick='" + "GetMemberID(this.value)" + "' />" +
            //        "<label for='" + objlist[r].MemberName + "'>" + objlist[r].MemberName + "</label>" + "<br />"
            //    $("#divMemberDetails").html(jsonResult);
            //}
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}

function GetStripeCardDetails(StripeCustomerID, url) {
    debugger;
    //var ConsultFee = $("#eConsultfee").val();
    var webMethodName = "GetStripCardDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "CustomerID";
    ParameterValues[0] = StripeCustomerID;
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    var Url = url + "Member";
    $.ajax({
        type: 'POST',
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (data, textStatus, jqXHR) {
            debugger;
            var obj = jQuery.parseJSON(data);
            var TestsList = obj[0];
            var jsonResult = "";
            if (TestsList.length != 0) {
               // $("#TempValue").val(2);                
                for (var r in TestsList) {
                    jsonResult += "<input type='radio' name='" + "CardsList" + "' value='" + TestsList[r].Id + "," + TestsList[r].Last4 + "," + TestsList[r].Name + "' onclick='" + "check(this.value)" + "'  />" + "&nbsp;" + TestsList[r].Brand + " **** **** **** " + TestsList[r].Last4 + "&nbsp;&nbsp;&nbsp;&nbsp;" + "<br />" +
                        "<label for='" + TestsList[r].Name + "'>" + TestsList[r].Name + "</label>" + "<br />" +
                        "<label for='" + TestsList[r].Name + "'>" + "Expires " + +TestsList[r].ExpirationMonth + "/" + TestsList[r].ExpirationYear + "</label>" + "<br />" +
                        "<br />"
                }

                jsonResult += "<input type='radio' name='" + "CardsList" + "' value='" + 0 + "' onclick='" + "check(this.value)" + "'  />" + " Add New Card " + " <br /><br/>";
               // $("#TextBoxTempValue").val(TestsList[r].Id);
                document.getElementById("divCardSelect").style.display = "block";
            }
            else {
               // $("#TempValue").val(1);
                jsonResult += "<input type='radio' name='" + "CardsList" + "' value='" + 0 + "' checked='checked' onclick='" + "check(this.value)" + "'  />" + " Add New Card " + " <br /><br/>";
                document.getElementById("divCardSelect").style.display = "block";
                document.getElementById("divbankdetails").style.display = "block";
            }
            $("#divCardList").html(jsonResult);
            if (TestsList.length != 0) {
                check(TestsList[0].Id + "," + TestsList[0].Last4 + "," + TestsList[0].Name);
                $("input[name=CardsList][value='" + TestsList[0].Id + "," + TestsList[0].Last4 + "," + TestsList[0].Name + "']").attr('checked', true);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });
}

function check(CardsList) {
    debugger;
    if (CardsList == 0) {
        document.getElementById("divbankdetails").style.display = "block";
        //$("#TempSelectRadio").val("1");
    }
    else {
        //$("#TempSelectRadio").val("2");
        var arr = CardsList;
        var arr_val = arr.split(',');
        $("#CardID").val(arr_val[0]);

        var CardValue = $("#CardID").val();
        //$("#PaymentDetails_CardID").val(CardsList);
        //$("#PaymentDetails_CustomerID").val($("#StripeCustomerID").val());
        //$("#TextBoxTempValue").val(CardsList);

        document.getElementById("divbankdetails").style.display = "none";
    }
}

var FilteredPlans;
function BindingPlansGridBasedOnFilters(OrganizationID, ProviderID, PlanID, StateId, CityId, zip, Url) {
    debugger;

    var webMethodName = "GetPPCPOrganizationProviderPlansBasedonFilters";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "ProviderID";
    ParameterValues[1] = ProviderID;
    ParameterNames[2] = "PlanID";
    ParameterValues[2] = PlanID;
    ParameterNames[3] = "StateID";
    ParameterValues[3] = StateId;
    ParameterNames[4] = "CityID";
    ParameterValues[4] = CityId;
    ParameterNames[5] = "ZIP";
    ParameterValues[5] = zip;
    var Url = Url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            $("#PlansGrid").data("kendoGrid").dataSource.data("");
            $("#divSelectPlan").show();
            var obj = jQuery.parseJSON(result);
            var PlansList = obj[0];
            //select distinct providers
            var providerList = [];
            $.each(PlansList, function (index, obj) {
                var a = $.grep(providerList, function (e) {
                    return obj.ProviderID === e.ProviderID && obj.OrganizationID === e.OrganizationID;
                });
                if (a.length === 0) {
                    providerList.push(obj);
                }
            });
            $("#gridProviders").data("kendoGrid").dataSource.data(providerList);

            FilteredPlans = PlansList;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            res = '';
            //callback(res);
            ErrorMessage(webMethodName, textStatus);
        },
    });
}

function BindPlansGrid(OrganizationID, ProviderID) {
    debugger;
    var plansList = FilteredPlans.filter(element => {
        return element.OrganizationID == OrganizationID && element.ProviderID == ProviderID;
    });
    $("#PlansGrid").data("kendoGrid").dataSource.data(plansList);
    $("#PlansPayment").data("kendoGrid").dataSource.data("");
    $("#btnstepNext").hide();
}

function BindingPlansGrids(OrganizationID, ProviderID, PlanID, MemberGender, MemberAge, url) {
    debugger;
    var webMethodName = "GetPPCPOrganizationProviderPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "ProviderID";
    ParameterValues[1] = ProviderID;
    ParameterNames[2] = "PlanID";
    ParameterValues[2] = PlanID;
    ParameterNames[3] = "MemberAge";
    ParameterValues[3] = MemberAge;
    ParameterNames[4] = "MemberGender";
    ParameterValues[4] = MemberGender;
    ParameterNames[5] = "PlanType";
    ParameterValues[5] = "1";
    var Url = url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var obj = jQuery.parseJSON(result);
            var PlansList = obj[0];

            var PlansList1 = [];
            PlansList1.push(PlansList[0]);
            $("#PlansGrid").data("kendoGrid").dataSource.data("");
            $("#divSelectPlan").show();

            var OrganizationID = $("#OrganizationID").val();

            $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList);

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}

function BindPlans(OrganizationID, ProviderID, PlanID, MemberGender, MemberAge, url) {
    debugger;
    if (OrganizationID != 0 && ProviderID != 0 && PlanID == 0) {
        $("#ProviderNames").data("kendoAutoComplete").destroy();
    }
    var webMethodName = "GetPPCPOrganizationProviderPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "ProviderID";
    ParameterValues[1] = ProviderID;
    ParameterNames[2] = "PlanID";
    ParameterValues[2] = PlanID;
    ParameterNames[3] = "MemberAge";
    ParameterValues[3] = MemberAge;
    ParameterNames[4] = "MemberGender";
    ParameterValues[4] = MemberGender;
    ParameterNames[5] = "PlanType";
    ParameterValues[5] = "1";
    var Url = url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var obj = jQuery.parseJSON(result);
            var PlansList = obj[0];
            if (PlanID == 0) {
                $('#PlanNames').data('kendoAutoComplete').dataSource.data(PlansList);
                $("#divSelectPlan").show();
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList);
            }
            else {
                $("#divPlanPayment").show();
                $("#PlansPayment").data("kendoGrid").dataSource.data(PlansList);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            debugger;
            ErrorMessage(webMethodName, textStatus);
        },
    });
}