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

//To get Age on DOB selection by Veena
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

//To bind Countries dropdownlist by Veena
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
                    var y = $('#CountryName').data('kendoDropDownList').dataSource.insert(r, { CountryName: countriesList[r].CountryName, CountryId: countriesList[r].CountryID });
                    
                }

                else {
                    $('#CountryName').data('kendoDropDownList').dataSource.insert(r, { CountryName: countriesList[r].CountryName, CountryId: countriesList[r].CountryID });
                }
                var dropdownlist = $("#CountryName").data("kendoDropDownList");
                var countryID = 1;
                dropdownlist.select(function (dataItem) {
                    $('#CountryName').data('kendoDropDownList').value("1");
                    // return dataItem.CountryID === parseInt("1");
                    return dataItem.CountryID === countryID;
                });
                var dropdownlist = $("#CountryName").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {
                    $('#CountryName').data('kendoDropDownList').value("1");
                    // return dataItem.CountryID === parseInt("1");
                    return dataItem.CountryID === countryID;
                });
                $("#CountryName").val("United States of America");
                $("#CountryId").val("1");
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//To Bind States dropdownlist based on CountryID by Veena
function BindStates(CountryID, ContolID, Url, tempValue) {
    if (tempValue == "2") {
        var Controlname = '#' + ContolID;
        $(Controlname).data('kendoDropDownList').value("");
    }
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
                $('#StateName').data('kendoDropDownList').dataSource.data(StatesListList);
            } else {
                $(Controlname).data('kendoDropDownList').dataSource.data(StatesListList);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//To bind Cities dropdownlist based on StateID by Veena
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
            } else {
                $(Controlname).data('kendoDropDownList').dataSource.data(CitiesListList);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//AddUser UserName Validation
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
//Bind Organizations
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