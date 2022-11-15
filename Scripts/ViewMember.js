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
            //var FirstName = PlansList.FirstName
            //var LastName = PlansList.LastName;
            //var MemberName = FirstName + LastName;
            $("#FamilyMemberGrid").data("kendoGrid").dataSource.data(PlansList);

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
////
function GetFamilyDetailss(MemberID, Url) {
   
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
            //var FirstName = PlansList.FirstName
            //var LastName = PlansList.LastName;
            //var MemberName = FirstName + LastName;

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

///To get RelationshipDetails in FamilyRegistration by Ragini
function GetRelationShip(Url,RelationshipID) {
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
            var RelationID = RelationshipID;
            if (RelationID != 0) {

                var dropdownlist = $("#RelationshipList").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.Value === parseInt(RelationID);
                });

            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            res = '';
            //callback(res);

        },
    });
}

//Family Member Registarion by Ragini
//function UpdateFamilyMembeDetails(MemberRegistrationDetails, session, Url, MemberPatientId) {
//    debugger;
//    MemberRegistrationDetails.MemberParentID = MemberPatientId;
//    MemberRegistrationDetails.RelationshipID = $("#RelationshipID").val();
//    MemberRegistrationDetails.RelationshipName = $("#Relationship").val();
//    MemberRegistrationDetails.FirstName = $("#FirstName").val();
//    MemberRegistrationDetails.LastName = $("#LastName").val();
//    MemberRegistrationDetails.DOB = $("#DOB").val();
//    MemberRegistrationDetails.Age = $("#Age").val();
//    var rbtnMale = $('input[id=rbtnGenderMale]:checked').val();
//    if (rbtnMale == 1)
//        MemberRegistrationDetails.Gender = rbtnMale;
//    var rbtnFEM = $('input[id=rbtnGenderFemale]:checked').val();
//    if (rbtnFEM == 2)
//        MemberRegistrationDetails.Gender = rbtnFEM;
//    MemberRegistrationDetails.SalutationID = $("#SalutationID").val();
//    MemberRegistrationDetails.Salutation = $("#Salutation").val();
//    MemberRegistrationDetails.Email = $("#Email").val();
//    MemberRegistrationDetails.CountryCode = $("#CountryCode").val();
//    MemberRegistrationDetails.MobileNumber = $("#MobileNumber").val();
//    MemberRegistrationDetails.CountryID = $("#CountryID").val();
//    MemberRegistrationDetails.CountryName = $("#CountryName").val();
//    MemberRegistrationDetails.StateID = $("#StateID").val();
//    MemberRegistrationDetails.StateName = $("#StateName").val();
//    MemberRegistrationDetails.CityID = $("#CityID").val();
//    MemberRegistrationDetails.CityName = $("#CityName").val();
//    if ($("#ZipCode").val() == "") {
//        MemberRegistrationDetails.Zip = $("#Zip").val();
//    }
//    else {
//        MemberRegistrationDetails.Zip = $("#Zip").val() + "-" + $("#ZipCode").val();
//    }
//    MemberRegistrationDetails.UserName = $("#UserName").val();
//    MemberRegistrationDetails.Password = $("#Password").val();
//    MemberRegistrationDetails.ConfirmPassword = $("#ConfirmPassword").val();
//    if ($('#chk2factor').is(":checked") == true) {
//        MemberRegistrationDetails.IsTwofactorAuthentication = true;
//        MemberRegistrationDetails.PreferredIP = session;
//        var rbtnEvery = $('input[id=rbtnEverytime]:checked').val();
//        if (rbtnEvery == 1)
//            MemberRegistrationDetails.TwoFactorType = rbtnEvery;
//        var rbtnSystem = $('input[id=rbtnOnlySysChange]:checked').val();
//        if (rbtnSystem == 2)
//            MemberRegistrationDetails.TwoFactorType = rbtnSystem;
//    }
//    else {
//        MemberRegistrationDetails.IsTwofactorAuthentication = false;
//        MemberRegistrationDetails.TwoFactorType = "0";
//        MemberRegistrationDetails.PreferredIP = "";
//    }
//    if (MemberRegistrationDetails != null) {
//        UpdateFamilyMembeDetails(MemberRegistrationDetails, Url);
//    }
//    else {
//        return false;
//    }
//}
//To generate Family Member Registration xml by vinod
function UpdateFamilyMembeDetails(MemberRegistrationDetails, Url) {
    debugger;
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Member/MemberRegistrationxml',
        data: MemberRegistrationDetails,
        success: function (data, textStatus, jqXHR) {

            UpdateFamilyMember(data, Url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//Display the Salutation details vinod
function BindSalutation(Salutation) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Member/BindSalutation',
        //data: ,
        success: function (SalutationList, textStatus, jqXHR) {

            //var SalutationList = jQuery.parseJSON(data);
            // var SalutationList = obj[0];
            for (var r in SalutationList) {
                $('#SalutationList').data('kendoDropDownList').dataSource.insert(r, { Text: SalutationList[r].Text, Value: SalutationList[r].Value });
            }
            var SalutationName = Salutation;
            if (SalutationName != "" && SalutationName != null) {

                var dropdownlist = $("#SalutationList").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.Text === SalutationName;
                });
                // BindStates(countryID, StateTempID, CityTempID);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//Call the AddFamilyMemberDetails web service by vinod
function UpdateFamilyMember(data, Url) {
    debugger;
    var webMethodName = "UpdateFamilyMemberDetails";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = Url + "MemberXml";
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
            if (obj[0].Exception == null && obj[0].ResultID != null) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Member details Updated successfully.";
                document.getElementById("divSignupPopup").scrollIntoView();
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].Exception + ". Please try again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//Bind the Country details and bind the exsisting selected county-vinod
function BindCountries(Url, CountryTempID) {
   
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
            var countryID = CountryTempID;
            if (countryID != 0) {

                var dropdownlist = $("#CountryNameList").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.Value === parseInt(countryID);
                });
               // BindStates(countryID, StateTempID, CityTempID);
            }

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



//Bind the State details and bind the exsisting selected state-vinod
function BindStates(Url, countryID, StateTempID) {
   
    var webMethodName = "GetStates";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "CountryID";
    ParameterValues[0] = countryID;
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
            var stateID = StateTempID;
            if (stateID != 0) {
                var dropdownlist = $("#StateNameList").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.StateID ===parseInt(stateID);
                });
               // BindCites(stateID, CityTempID);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}

//Bind the City details and bind the exsisting selected City-vinod
function BindCites(Url, StateId, CityTempID) {
   

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
            var CityID = CityTempID;
            if (CityID != 0) {
                var dropdownlist = $("#CityNameList").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.CityID === parseInt(CityID);
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}

//////To bind Cities dropdownlist based on StateID by Ragini
function BindCities(StateID, Url) {
   
    $('#CityNameList').data('kendoDropDownList').value("");
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


//Updating family Members;
function ViewFamilyDetails(UpdateDetails, MemberID,countryName,DOB,stateName,CityName, Url) {
   
    if (UpdateDetails.Gender == 1) {
        $("#rbtnGenderMale").prop("checked", true);
    }
    else {
        $("#rbtnGenderFemale").prop("checked", true);
    }
   UpdateDetails.DOB = DOB;

}









