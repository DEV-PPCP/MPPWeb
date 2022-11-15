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

//To get Age on DOB selection by Gayathri
function DateSelectionChanged(e) {
    var birthDate = $("#DOB").val();
    $.ajax({
        url: "/Master/GetAge/",
        data: { birthDate: birthDate },
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {
            var rs = result.split(';');
            var year = rs[0];
            $("#Age").val(year);
        }

    });
}
//Getting IP Address in organization Registration
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
//To bind Countries dropdownlist by Gayathri
function BindCountries(FormID, Url) { //FormID if 1 bind the OrgCountryName 2 for bind CountryName   
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
            for (var r in countriesList) {
                if (FormID == 1) {
                    $('#CountryName').data('kendoDropDownList').dataSource.insert(r, { CountryName: countriesList[r].CountryName, CountryId: countriesList[r].CountryID });
                    var x = $('#OrgCountryName').data('kendoDropDownList').dataSource.insert(r, { CountryName: countriesList[r].CountryName, CountryId: countriesList[r].CountryID });
                    var y = $('#BankCountryName').data('kendoDropDownList').dataSource.insert(r, { CountryName: countriesList[r].CountryName, CountryId: countriesList[r].CountryID });

                }

                else {
                    $('#CountryName').data('kendoDropDownList').dataSource.insert(r, { CountryName: countriesList[r].CountryName, CountryId: countriesList[r].CountryID });
                }
                var dropdownlist = $("#CountryName").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.CountryID === parseInt("1");
                });
                var dropdownlist = $("#OrgCountryName").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.CountryID === parseInt("1");
                });
                var dropdownlist = $("#BankCountryName").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.CountryID === parseInt("1");
                });
                $("#OrgCountryName").val("1");
                $("#OrgCountryId").val("United States of America");
                $("#CountryName").val("1");
                $("#CountryId").val("United States of America");
                $("#BankCountryName").val("1");
                $("#CountryIds").val("United States of America");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//To Bind States dropdownlist based on CountryID by Gayathri
function BindStates(CountryID, ContolID, Url, tempValue) {
    if (tempValue == "2") {
        var Controlname = '#' + ContolID;
        $(Controlname).data('kendoDropDownList').value("");
    }

    //var raw = $(Controlname).data('kendoDropDownList').dataSource.data({});
    //var length = raw.length;
    //// iterate and remove "done" items
    //var item, i;
    //for (i = length - 1; i >= 0; i--) {
    //    item = raw[i];
    //    $(Controlname).data('kendoDropDownList').dataSource.remove(item);
    //}
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
            if (tempValue == "1") {
                $('#OrgStateName').data('kendoDropDownList').dataSource.data(StatesListList);
                $('#StateName').data('kendoDropDownList').dataSource.data(StatesListList);
            } else {
                $(Controlname).data('kendoDropDownList').dataSource.data(StatesListList);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//To bind Cities dropdownlist based on StateID by Gayathri
function BindCities(StateID, ContolID, Url, tempValue) {
    if (tempValue == "2") {
        var Controlname = '#' + ContolID;
        $(Controlname).data('kendoDropDownList').value("");
    }
    
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
            if (tempValue == "1") {

                $('#CityName').data('kendoDropDownList').dataSource.data(CitiesListList);
                $('#OrgCityName').data('kendoDropDownList').dataSource.data(CitiesListList);
            } else {
                $(Controlname).data('kendoDropDownList').dataSource.data(CitiesListList);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//Family Registration UserName Validation
function ValidateUserNames(UserName, Url) {
    var webMethodName = "ValidateUserName";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "Username";
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

//AddDoctors :Ragini 11-09-2019
//function ValidateuserName(UserName, Url) {
//    if ($("#UserName").val() == "") {
//        $("#spnUserName").hide();
//    }
//    var webMethodName = "ValidateProviderUserName";
//    var ParameterNames = new Array();
//    var ParameterValues = new Array();
//    ParameterNames[0] = "Username";
//    ParameterValues[0] = UserName;
//    var Url = Url + "OrganizationServices";
//    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
//    $.ajax({
//        type: "POST",
//        url: Url,
//        data: jsonPostString,
//        dataType: "json",
//        contentType: "application/json",
//        success: function (result) {

//            if (result == 0) {

//                document.getElementById("spnUserName").innerHTML = " ";
//            }

//            else {
//                document.getElementById("spnUserName").innerHTML = "Username already exists"; return false;

//            }
//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {

//        },
//    });
//}

function SavePlanDetails(jsModel) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Organization/SavePlanDetailsXML',
        data: jsModel,
        success: function (data, textStatus, jqXHR) {

            CallWebApiServicePlanDetails(data, Url)
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function CallWebApiServicePlanDetails(data, Url) {
    var webMethodName = "SavePlanDetailsXML";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    //var Url = "http://192.168.1.20/ppcpwebservice/Organization";
    var Url = Url + "Organization";
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function organizationRegistration(jsModel, Url) {
    $.ajax({

        type: 'POST',
        cache: false,
        url: '/Organization/OrganizationRegistrationxml',
        data: jsModel,
        success: function (data, textStatus, jqXHR) {

            CallWebApiService(data, Url)
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function CallWebApiService(data, Url) {
    debugger;
    var webMethodName = "OrganizationSignUp";
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
            var obj = result[0];
            $("#divMain").find(".loadingSpinner:first").remove();
            if (obj[0].UserID > 0 && obj[0].Result == null) {
                $("#divOTPAfterLogin").show();
                $("#lblUserMessagee").show();
                $("#btnSuccessClose").show();
                $("#lblUserMessag").hide();
                $("#btnErrorClose").hide();
                document.getElementById("lblUserMessagee").innerHTML = "Organization Details Saved Successfully";
            }
            else if (obj[0].Result != "") {
                $("#divOTPAfterLogin").show();
                $("#lblUserMessag").show();
                $("#btnErrorClose").show();
                document.getElementById("lblUserMessag").innerHTML = obj[0].Result +".Please Enter Valid Details! ";
            }
            else {
                $("#divOTPAfterLogin").show();
                $("#lblUserMessag").show();
                $("#btnErrorClose").show();
                document.getElementById("lblUserMessag").innerHTML = "Please Enter Valid Details. ";
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function BindPPCPOrganizations(Url) {
    var webMethodName = "GetPPCPOrganizations";
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
            var OrganizationList = obj[0];
            $('#OrganizationName').data('kendoDropDownList').dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function BindOrganizationsCheckBoxes() {
    var webMethodName = "GetPPCPOrganizations";
    var ParameterNames = "";
    var Url = "http://192.168.1.20/ppcpwebservice/DefaultService";
    var jsonPostString = setParameter(ParameterNames, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var OrganizationList = obj[0];
            $('#OrganizationName').data('kendoDropDownList').dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function BindOrganizations(Url) {
    var webMethodName = "GetPPCPOrganizations";
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
            var OrganizationList = obj[0];
            for (var r in OrganizationList) {
                $('#OrganizationName').data('kendoAutoComplete').dataSource.insert(r, { OrganizationName: OrganizationList[r].OrganizationName, OrganizationId: OrganizationList[r].OrganizationID });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//Display the Salutation details vinod
function BindSalutation() {

    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Organization/BindSalutation',
        //data: ,
        success: function (SalutationList, textStatus, jqXHR) {

            for (var i = 0; i <= 5; i++) {

                $('#SalutationList').data('kendoDropDownList').dataSource.insert(i, { Text: SalutationList[i].Text, Value: SalutationList[i].Value });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//Add users-vinod
function AddUserDetails(jsModel, Url) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Organization/OrganizationRegistrationxml',
        data: jsModel,
        success: function (data, textStatus, jqXHR) {

            CallWebApiServiceAddUser(data, Url)
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//Call webservice for Add Users-vinod
function CallWebApiServiceAddUser(data, Url) {
    var webMethodName = "AddUserDetails";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = Url + "Organization";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainRegistration"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            var obj = result[0];
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
            if (obj[0].result == null && obj[0].UserID != null) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("AddUserSuccess").innerHTML = "You are registered successfully.";
                document.getElementById("divSignupPopup").scrollIntoView();
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].result + ". Please try again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//Bind the users grid to call the GetOrganizationUsers webservice-Vinod
function BindOrganizationsUsersGrid(UserID, OrganizationID, Url) {
    var webMethodName = "GetOrganizationUsers";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "UserID";
    ParameterValues[0] = UserID;
    ParameterNames[1] = "OrganizationID";
    ParameterValues[1] = OrganizationID;
    //var Url = "http://192.168.1.20/ppcpwebservice/OrganizationServices";
    var Url = Url + "OrganizationServices";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var PlansList = obj[0];
            $("#UsersGrid").data("kendoGrid").dataSource.data(PlansList);

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function BindDegree() {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Master/BindDegree',
        //data: ,
        success: function (SalutationList, textStatus, jqXHR) {
            for (var r in SalutationList) {
                $('#Degree').data('kendoDropDownList').dataSource.insert(r, { Text: SalutationList[r].Text, Value: SalutationList[r].ID });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}


//Add Doctors getting specilizations :Ragini
function GetSpecilization(Url) {
    var webMethodName = "GetSpecializationLKP";
    var ParameterNames = "";
    var Url = Url + "OrganizationServices";
    var jsonPostString = setParameter(ParameterNames, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var objlist = obj[0];
            var jsonResult = "";
            var jsonResults = ""

            for (var r in objlist) {
                if (r % 2 == 0 || r == 0) {
                    jsonResult += "<input type='checkbox' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ";" + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
                    "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
                    $("#divSpecializationList").html(jsonResult);
                }
                else {
                    jsonResults += "<input type='checkbox' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ";" + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
                   "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
                    $("#divSpecializationListRight").html(jsonResults);
                }
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//Add Doctors getting specilizations :Ragini
//function GetSpecilization(Url) {
//    var webMethodName = "GetSpecializationLKP";
//    var ParameterNames = "";
//    var Url = Url + "OrganizationServices";
//    var jsonPostString = setParameter(ParameterNames, webMethodName);
//    $.ajax({
//        type: "POST",
//        url: Url,
//        data: jsonPostString,
//        dataType: "text",
//        contentType: "application/json",
//        success: function (result) {
//            var obj = jQuery.parseJSON(result);
//            var objlist = obj[0];
//            var jsonResult = "";
//            var jsonResults = ""

//            for (var r in objlist) {
//                if (r % 2 == 0 || r == 0) {
//                    jsonResult += "<input type='checkbox' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ';' + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
//                    "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
//                    $("#divSpecializationList").html(jsonResult);
//                }
//                else {
//                    jsonResults += "<input type='checkbox' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ';' + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
//                   "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
//                    $("#divSpecializationListRight").html(jsonResults);
//                }
//            }
//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//        },
//    });
//}

//Add Doctors Saving Details
//function SaveDoctorDetails(jsModel, url) {
//    $.ajax({
//        type: 'POST',
//        cache: false,
//        url: '/Organization/SaveDoctorDetailsxml',
//        data: jsModel,
//        success: function (data, textStatus, jqXHR) {
//            CallSaveAddDotors(data, url);
//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            alert(XMLHttpRequest, textStatus, errorThrown);
//        },
//    });


//}

//function CallSaveAddDotors(data, url) {
//    var webMethodName = "AddDoctorDetails";
//    var ParameterName = data;
//    var jsonPostString = setParameter(ParameterName, webMethodName);
//    var Url = url + "Organization";

//    $.ajax({
//        type: "POST",
//        url: Url,
//        data: jsonPostString,
//        dataType: "json",
//        contentType: "application/json",
//        success: function (result) {
//            var obj = result[0];

//            if (obj[0].result == null && obj[0].UserID != null) {
//                document.getElementById("divSignupPopup").style.display = "block";
//                document.getElementById("spnPopupMessage").innerHTML = "You are registered successfully.";
//                document.getElementById("divSignupPopup").scrollIntoView();
//            }
//            else {
//                document.getElementById("divErrMessagePopup").style.display = "block";
//                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].result + ". Please try again.";
//                document.getElementById("divErrMessagePopup").scrollIntoView();
//            }
//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//        },
//    });
//}

////AddPlans binding TenureList in Admin Module :Ragini
//function BindTenure() {
//    $.ajax({
//        type: "POST",
//        cache: false,
//        url: "/Admin/BindTenures",
//        success: function (TenureList, textStatus, jqXHR) {
//            for (var r in TenureList) {
//                $('#TenureList').data('kendoDropDownList').dataSource.insert(r, { Text: TenureList[r].Text, Value: TenureList[r].Value });
//            }
//        },

//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            alert(XMLHttpRequest, textStatus, errorThrown)
//        },

//    });

//}

////AddPlans binding PlanType in AdminModule :Ragini
//function BindPlanType() {
//    $.ajax({
//        type: "POST",
//        cache: false,
//        url: "/Admin/BindPlanTypes",
//        success: function (PlanTypeList, textStatus, jqXHR) {

//            for (var r in PlanTypeList) {
//                $('#PlanTypeList').data('kendoDropDownList').dataSource.insert(r, { Text: PlanTypeList[r].Text, Value: PlanTypeList[r].Value });
//            }
//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            alert(XMLHttpRequest, textStatus, errorThrown)
//        },
//    });
//}

//function BindMemberPlanType() {

//    $.ajax({
//        type: "POST",
//        cache: false,
//        url: "/Admin/BindMemberPlanTypes",
//        success: function (MemberPlanTypeList, textStatus, jqXHR) {

//            for (var r in MemberPlanTypeList) {
//                $('#MemberPlanTypeList').data('kendoDropDownList').dataSource.insert(r, { Text: MemberPlanTypeList[r].Text, Value: MemberPlanTypeList[r].Value });
//            }
//        },
//        error: function (XMLHttpRequest, textStatus, errorThrown) {
//            alert(XMLHttpRequest, textStatus, errorThrown)
//        },
//    });
//}

/////To check Password strength in AddDoctors (Organization Module)  : Ragini on 13-09-2019 
//function chkPasswordStrength(txtPassword, strenghtMsg, errorMsg) {

//    var desc = new Array();
//    desc[0] = "Very Weak";
//    desc[1] = "Weak";
//    desc[2] = "Average";
//    desc[3] = "Strong";
//    desc[4] = "Strong";
//    desc[5] = "Strong";

//    var score = 0;

//    //if txtPassword bigger than 6 give 1 point
//    if (txtPassword.length > 6) score++;

//    //if txtPassword has both lower and uppercase characters give 1 point
//    if ((txtPassword.match(/[a-z]/)) && (txtPassword.match(/[A-Z]/))) score++;

//    //if txtPassword has at least one number give 1 point
//    if (txtPassword.match(/\d+/)) score++;

//    //if txtPassword has at least one special caracther give 1 point
//    if (txtPassword.match(/.[!,#,$,%,^,&,*,?,_,~,-,(,)]/)) score++;

//    //if txtPassword bigger than 12 give another 1 point
//    if (txtPassword.length > 12) score++;

//    strenghtMsg.innerHTML = desc[score];

//    strenghtMsg.className = "strength" + score;
//}


///// PasswordValidation in AddDoctors : Ragini on 13-09-2019 ///
//function validatePassword() {
//    //To Compare Password and ConfirmPassword (in Step1)
//    if (($("#Password").val()) == ($("#ConfirmPassword").val())) {
//        document.getElementById("PasswordErrorMsg").innerHTML = "";
//    }
//    else {
//        document.getElementById("PasswordErrorMsg").innerHTML = "Password and Confirm Password should be Match.";
//    }
//}

