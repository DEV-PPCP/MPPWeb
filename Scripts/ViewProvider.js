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

//To bind Countries dropdownlist by veena
function BindCountries(Url, CountryID) { //FormID if 1 bind the OrgCountryName 2 for bind CountryName
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

                $('#CountryName').data('kendoDropDownList').dataSource.insert(r, { CountryName: countriesList[r].CountryName, CountryId: countriesList[r].CountryID });
            }
            var OrgCountryID = CountryID;
            if (OrgCountryID != 0) {
                var dropdownlist = $("#CountryName").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.CountryId === parseInt(OrgCountryID);
                });
            }
           
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//To Bind States dropdownlist based on CountryID  by Veena
function BindStates(Url, CountryID, StateTempID) {
    var webMethodName = "GetStates";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "CountryID";
    ParameterValues[0] = CountryID;
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainAddDoctors"));
    var Url = Url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divMainAddDoctors").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var StatesListList = obj[0];
            $('#StateName').data('kendoDropDownList').dataSource.data(StatesListList);
            var StateID = StateTempID;
            if (StateID != 0) {
                var dropdownlist = $("#StateName").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.StateID === parseInt(StateID);
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//To bind Cities dropdownlist based on StateID by Veena
function BindCities(Url, StateID, CityTempID) {
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
            $('#CityName').data('kendoDropDownList').dataSource.data(CitiesListList);
            var CityID = CityTempID;
            if (CityID != 0) {
                var dropdownlist = $("#CityName").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.CityID === parseInt(CityID);
                });
            }

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
        url: '/Master/BindSalutation',
        //data: ,
        success: function (SalutationList, textStatus, jqXHR) {
            for (var r in SalutationList) {
                $('#SalutationList').data('kendoDropDownList').dataSource.insert(r, { Text: SalutationList[r].Text, Value: SalutationList[r].Value });
              
            }
            var SalutationName = Salutation;
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
//Add Doctors getting specilizations :Ragini
function GetSpecilization(Url, SpecializationTempID) {
    var Id = [];
    Id = SpecializationTempID.split(",");
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
            var jsonResults = "";
            for (var r in objlist) {
                if (r % 2 == 0 || r == 0) {


                    if (SpecializationTempID.includes(objlist[r].SpecializationID)) {
                        jsonResult += "<input type='checkbox' checked='true'  id='" + objlist[r].SpecializationID + "' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ";" + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
                        "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
                        Id = objlist[r].SpecializationID;
                    }
                    else {
                        jsonResult += "<input type='checkbox'  id='" + objlist[r].SpecializationID + "' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ";" + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
                        "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
                        Id = objlist[r].SpecializationID;
                    }
                    $("#divSpecializationList").html(jsonResult);


                }
                else {
                    if (SpecializationTempID.includes(objlist[r].SpecializationID)) {
                        jsonResults += "<input type='checkbox' checked='true'  id='" + objlist[r].SpecializationID + "' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ";" + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
                        "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
                        Id = objlist[r].SpecializationID;
                    }
                    else {
                        jsonResults += "<input type='checkbox'  id='" + objlist[r].SpecializationID + "' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ";" + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
                        "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
                        Id = objlist[r].SpecializationID;
                    }
                    $("#divSpecializationListRight").html(jsonResults);
                }

            }

           
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//Add Doctors Saving Details
function SaveDoctorDetails(jsModel, url) {
    debugger;
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Provider/ProviderDetailsxml',
        data: jsModel,
        success: function (data, textStatus, jqXHR) {
            CallSaveAddDotors(data, url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest, textStatus, errorThrown);
        },
    });
}

function CallSaveAddDotors(data, url) {
    debugger;
    var webMethodName = "UpdateDoctorDetails";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = url + "ProviderXml";

    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            var obj = result[0];
           
            if (obj[0].ResultID == 1) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Details are Updated Successfully.";
                document.getElementById("divSignupPopup").scrollIntoView();
            }
            else if (obj[0].ResultID == 0) {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = "No changes in Existing Profile.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].ResultName + ". Please try again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//ProviderRegistration :v
function ValidateuserName(UserName, Url) {
    if ($("#UserName").val() == "") {
        $("#spnUserName").hide();
    }
    var webMethodName = "ValidateProviderUserName";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "Username";
    ParameterValues[0] = UserName;
    var Url = Url + "OrganizationServices";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            if (result == 0) {

                document.getElementById("spnUserName").innerHTML = " ";
            }

            else {
                document.getElementById("spnUserName").innerHTML = "Username already exists"; return false;

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}

/// PasswordValidation in AddDoctors : Ragini on 13-09-2019 ///
function validatePassword() {
    //To Compare Password and ConfirmPassword (in Step1)
    if (($("#Password").val()) == ($("#ConfirmPassword").val())) {
        document.getElementById("PasswordErrorMsg").innerHTML = "";
    }
    else {
        document.getElementById("PasswordErrorMsg").innerHTML = "Password and Confirm Password should be Match.";
    }
}

///To check Password strength in AddDoctors (Organization Module)  : Ragini on 13-09-2019 
function chkPasswordStrength(txtPassword, strenghtMsg, errorMsg) {
    var desc = new Array();
    desc[0] = "Very Weak";
    desc[1] = "Weak";
    desc[2] = "Average";
    desc[3] = "Strong";
    desc[4] = "Strong";
    desc[5] = "Strong";

    var score = 0;

    //if txtPassword bigger than 6 give 1 point
    if (txtPassword.length > 6) score++;

    //if txtPassword has both lower and uppercase characters give 1 point
    if ((txtPassword.match(/[a-z]/)) && (txtPassword.match(/[A-Z]/))) score++;

    //if txtPassword has at least one number give 1 point
    if (txtPassword.match(/\d+/)) score++;

    //if txtPassword has at least one special caracther give 1 point
    if (txtPassword.match(/.[!,#,$,%,^,&,*,?,_,~,-,(,)]/)) score++;

    //if txtPassword bigger than 12 give another 1 point
    if (txtPassword.length > 12) score++;

    strenghtMsg.innerHTML = desc[score];

    strenghtMsg.className = "strength" + score;
}

