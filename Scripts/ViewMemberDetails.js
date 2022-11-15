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
// To Get IPAddress in ViewMemberDetails Veena
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
// To Get Member Details of Organization using  OrganizationID by Veena
function GetMemberDetails(OrganizationID,strMemberID, Url) {
    var webMethodName = "GetMembersList";//GetOrganizationMemberDetails
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "strMemberID";
    ParameterValues[1] = strMemberID;
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
           
            $("#ViewMemberGrid").data("kendoGrid").dataSource.data(PlansList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function BindCountries(Url, CountryID) {
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
                $('#CountryNameList').data('kendoDropDownList').dataSource.insert(r, { CountryName: countriesList[r].CountryName, CountryId: countriesList[r].CountryID });

            }
            var OrgCountryID = CountryID;
            if (OrgCountryID != 0) {
                
                var dropdownlist = $("#CountryNameList").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.CountryId === parseInt(OrgCountryID);
                });
            
            }

        },
    });
}
function BindStates(Url, CountryID, StateID) {
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
            var stateID = StateID;
            if (stateID != 0) {
                var dropdownlist = $("#StateNameList").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.StateID === parseInt(stateID);
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}
function BindCites(Url, StateId, CityID) {
    var webMethodName = "GetCities";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "StateID";
    ParameterValues[0] = StateId;
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
            var CityId = CityID;
            if (CityId != 0) {
                var dropdownlist = $("#CityNameList").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.CityID === parseInt(CityId);
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}
function BindSalutation(SalutationID) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Master/BindSalutation',
        //data: ,
        success: function (SalutationList, textStatus, jqXHR) {
            for (var r in SalutationList) {
                $('#SalutationList').data('kendoDropDownList').dataSource.insert(r, { Text: SalutationList[r].Text, Value: SalutationList[r].Value });
            }
            var SalutationName = SalutationID;
            if (SalutationName != "" && SalutationName != null) {
                var dropdownlist = $("#SalutationList").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.Text === SalutationName;
                });
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//Add Member Details by veena
function AddMembeDetails(MemberRegistrationDetails, session, Url) {
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
   // MemberRegistrationDetails.SalutationID = $("#SalutationID").val();
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
    if (MemberRegistrationDetails != null) {
        AddMemberRegistration(MemberRegistrationDetails, Url);
    }
    else {
        return false;
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

            AddMemberRegistrationWebApiService(data, Url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//Call the AddMemberDetails web service by veena
function AddMemberRegistrationWebApiService(data, Url) {
    var webMethodName = "UpdateMemberDetails";
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
        
            if (obj[0].ResultID >= 0 && obj[0].ResultName == null) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Updated successfully.";
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
function BindOrganizationMembersAutoComplete(OrganizationID, Text, Url) {
    var webMethodName = "GetOrganizationMembersAutoComplete";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "Text";
    ParameterValues[1] = Text;
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
            var MemberList = obj[0];
            $('#MemberName').data('kendoAutoComplete').dataSource.data(MemberList);
           
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}