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


////////
function GetFamilyDetails(MemberParentID, Url) {
    var webMethodName = "GetFamilyDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strMemberParentID";
    ParameterValues[0] = MemberParentID;
    var Url = Url + "Member";
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
            $("#FamilyMemberGrid").data("kendoGrid").dataSource.data(PlansList);
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
////
function GetFamilyDetailss(MemberParentID, Url) {
    var webMethodName = "GetFamilyDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strMemberParentID";
    ParameterValues[0] = MemberParentID;
    var Url = Url + "Member";
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
            $("#Individuals").data("kendoGrid").dataSource.data(PlansList);
            $("#FamilyMembers").data("kendoGrid").dataSource.data(PlansList);
            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

/// To Get IPAddress in Family Registration by Ragini
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
            for (var r in countriesList) {
                $('#CountryNameList').data('kendoDropDownList').dataSource.insert(r, { Text: countriesList[r].CountryName, Value: countriesList[r].CountryID });
            }
            var countryID = 1;

            if (countryID != 0) {

                var dropdownlist = $("#CountryNameList").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {
                    $('#CountryNameList').data('kendoDropDownList').value("1");
                    return dataItem.Value === countryID;
                });
                BindStates(countryID, Url)
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

///To get RelationshipDetails in FamilyRegistration by Ragini
function GetRelationShip(Url) {
var webMethodName = "GetRelationShipDetails";
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
        var RelationshipList = obj[0];
        for (var r in RelationshipList) {
            $('#RelationshipList').data('kendoDropDownList').dataSource.insert(r, { Text: RelationshipList[r].RelationshipName, Value: RelationshipList[r].RelationshipID });
        }
    },
    error: function (XMLHttpRequest, textStatus, errorThrown) {
        res = '';
        //callback(res);

    },
});
}

//Family Member Registarion by Ragini
function FamilyMembeDetails(MemberRegistrationDetails, session, Url,MemberPatientId) {
    MemberRegistrationDetails.MemberID = MemberPatientId;
    MemberRegistrationDetails.RelationshipID = $("#RelationshipID").val();
    MemberRegistrationDetails.RelationshipName = $("#Relationship").val();
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
    if (MemberRegistrationDetails != null) {
        FamilyMemberRegistration(MemberRegistrationDetails,Url);
    }
    else {
        return false;
    }
}
//To generate Family Member Registration xml by vinod
function FamilyMemberRegistration(MemberRegistrationDetails,Url) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Member/MemberRegistrationxml',
        data: MemberRegistrationDetails,
        success: function (data, textStatus, jqXHR) {

            FamilyFamilyMemberRegistrationWebApiService(data,Url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//Call the AddFamilyMemberDetails web service by vinod
function FamilyFamilyMemberRegistrationWebApiService(data, Url) {
    var webMethodName = "AddFamilyMemberDetails";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = Url+"MemberXml";
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
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//Validate UserNames by Ragini
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
//Bind States in Family Registration by Ragini
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
//To bind Cities dropdownlist based on StateID by Ragini
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


