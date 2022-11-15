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
function BindCountries(ServiceUrl) {
    var webMethodName = "GetCountries";
    var ParameterNames = "";

    var Url = ServiceUrl + "DefaultService";
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
                //  BindStates(countryID, ServiceUrl)
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

            $("#divMainRegistration").find(".loadingSpinner:first").remove();
        },
    });
}
//To Bind States dropdownlist based on CountryID by Gayathri
function BindStates(CountryID, ServiceUrl) {
    var webMethodName = "GetStates";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "CountryID";
    ParameterValues[0] = CountryID;
    var Url = ServiceUrl + "DefaultService";
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
                // BindCities(stateID, ServiceUrl)
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//To bind Cities dropdownlist based on StateID by Gayathri
function BindCities(StateID, ServiceUrl, tempvalue) {
    if (tempvalue == "2") {
        $('#CityNameList').data('kendoDropDownList').value("");
    }
    var webMethodName = "GetCities";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "StateID";
    ParameterValues[0] = StateID;
    var Url = ServiceUrl + "DefaultService";
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
}//Employer Registration UserName Validation
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
function EmployerRegistration(jsModel, Url) {
    $.ajax({

        type: 'POST',
        cache: false,
        url: '/Employer/EmployerRegistrationxml',
        data: jsModel,
        success: function (data, textStatus, jqXHR) {

            CallWebApiService(data, Url)
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function CallWebApiService(data, Url) {
    var webMethodName = "EmployerSignUp";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = Url + "EmployerXml";
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
                document.getElementById("lblUserMessagee").innerHTML = "Employer Details Saved Successfully";
            }
            else if (obj[0].Result != "") {
                $("#divOTPAfterLogin").show();
                $("#lblUserMessag").show();
                $("#btnErrorClose").show();
                document.getElementById("lblUserMessag").innerHTML = obj[0].Result + ".Please Enter Valid Details! ";
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