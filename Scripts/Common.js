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

function BindDropdown(LookupTypeId, dropdownName, Url) {
    debugger;
    var webMethodName = "GetLookupValues";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "LookupTypeId";
    ParameterValues[0] = LookupTypeId;

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
            var lookupValueList = obj[0];
            $('#' + dropdownName).data('kendoDropDownList').dataSource.data(lookupValueList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function divPopUpSuccessClose() {
    debugger;
    $.ajax({
        url: '@Url.Action("VerifyOrgOTP", "Account")',
        data: { otp: $("#OTP").val() },
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {

            if (result == "1") {
                var getUrl = '@Url.Action("ViewPlans", "Admin")';
                window.location = getUrl;
            }
            else {
                $("#lblUserMessag").html("Invalid OTP");
                return false;
            }
        },
        error: function (ex) {
            alert(ex.error());
            location.reload();
        }
    });
}

function ValidateCredentials() {

    var username = $("#UserName").val();
    var password = $("#Password").val();

    var captcha_response = grecaptcha.getResponse();
    if (username == "" && password == "" && captcha_response.length == "0") {
        document.getElementById("spnInvalidUser").innerHTML = "";
        $("#spnInvalidUser").html("Please enter Username.");
        return false;
    }
    else if (password == "") {
        document.getElementById("spnInvalidUser").innerHTML = "";
        $("#spnInvalidUser").html("Please enter Password.");
        return false;
    }
    else if (captcha_response.length == "0") {
        document.getElementById("spnInvalidUser").innerHTML = "";
        $("#spnInvalidUser").html("Please check the checkbox");
        return false;
    }
    else {
        document.getElementById("spnInvalidUser").innerHTML = "";
    }

    jsModel = {};
    jsModel.UserName = username
    jsModel.Password = password;
    //var getUrl = '@Url.Action("ViewPlans", "Admin")';
    //window.location = getUrl;
    $.ajax({
        url: "@Url.Action('ValidateCredentials', 'Account')",
        data: jsModel,
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {


            if (result == "1") {
                var getUrl = '@Url.Action("ViewPlans", "Admin")';
                window.location = getUrl;
            }
            else if (result == "0") {
                $("#divOTPAfterLogin").show();
            }
            else if (result == "2") {
                $("#spnInvalidUser").html("Your username or password is incorrect.");
            }
            else if (result == "999") {
                $("#spnInvalidUser").html("Your account is locked. Please contact our support team.");
            }
            else {
                document.getElementById("spnInvalidUser").innerHTML = "";
            }
        },
        error: function (ex) {
            alert(ex.error());
            //location.reload();
        }
    });
}

function BindOrganizations(autoCompleteName, url) {
    var webMethodName = "GetPPCPOrganizations";
    var ParameterNames = "";
    var Url = url + "DefaultService";
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
            $('#' + autoCompleteName).data('kendoAutoComplete').dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}

function setParameter(parameterName, methodName) {
    var obj = new Object();
    obj.WebMethodName = methodName;
    obj.XMLdata = parameterName;
    var resultData = JSON.stringify(obj);
    return resultData;
}