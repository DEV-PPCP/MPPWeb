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

function credentials(Url) {
    debugger;
    //oValidator().data("kendoValidator");
    var webMethodName = "ValidateForgotCredentials";
    var Url = Url + "DefaultService";

    $("<div class='loadingSpinner'></div>").appendTo($("#fsUserCredential"));
    var usernamevalidate = $("#divUserName").kendoValidator().data("kendoValidator");
    var Organizationvalidators = $("#usernamediv").kendoValidator().data("kendoValidator");
    var ParameterNames = new Array();
    var ParameterValues = new Array();

    if ($("#rbtnUserName").is(":checked") == true) { //Forgot Username
        if (Organizationvalidators.validate()) {
            ParameterNames[0] = "UserName";
            ParameterValues[0] = "";
            ParameterNames[1] = "FirstName";
            ParameterValues[1] = $("#First_Name").val();
            ParameterNames[2] = "Lastname";
            ParameterValues[2] = $("#Last_Name").val();
            ParameterNames[3] = "CountryCode";
            ParameterValues[3] = $("#CountryCode").val();
            ParameterNames[4] = "MobileNumber";
            ParameterValues[4] = $("#txtMobileNumber").val();
            ParameterNames[5] = "Email";
            ParameterValues[5] = $("#txtEmail").val();
        } else {
            document.getElementById("spnForgotUsername").innerHTML = "Please enter valid data.";
            return;
        }
    }
    if ($("#rbtnPassword").is(":checked") == true) { //Forgot Password
        if (usernamevalidate.validate()) {
            ParameterNames[0] = "UserName";
            ParameterValues[0] = $("#txtUserName").val();
            ParameterNames[1] = "FirstName";
            ParameterValues[1] = "";
            ParameterNames[2] = "Lastname";
            ParameterValues[2] = "";
            ParameterNames[3] = "CountryCode";
            ParameterValues[3] = "";
            ParameterNames[4] = "MobileNumber";
            ParameterValues[4] = "";
            ParameterNames[5] = "Email";
            ParameterValues[5] = "";
        }
        else {
            document.getElementById("spnForgotUsername").innerHTML = "Please enter valid data.";
            return;
        }
    }
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var obj = result[0];
            $("#fsUserCredential").find(".loadingSpinner:first").remove();
            if (obj.length > 0) {
                document.getElementById("spnErrorMessage").innerHTML = "";
                document.getElementById("divfsLogin").style.display = "none";
                $('#FirstName').val(obj[0].FirstName);
                $('#LastName').val(obj[0].LastName);
                $('#UserID').val(obj[0].UserID);
                $('#UserName').val(obj[0].UserName);
                $('#CountryID').val(obj[0].CountryCode);
                $('#MobileNo').val(obj[0].MobileNumber);
                $('#Email').val(obj[0].Email);
                $('#UserPassword').val(obj[0].UserPassword);
                $("#OtpNumber").val(obj[0].OTP);
                document.getElementById("divvalidotp").style.display = "block";
                document.getElementById("spnMessage").innerHTML = "An Onetime Password is sent to your Mobile Number: " + $('#MobileNo').val() + " and EmailID: " + $("#txtEmail").val();
            } else {
                document.getElementById("spnErrorMessage").innerHTML = "Please check the data entered.";
                return;
            }
        }
    });
}

/// Resend OTP in Member Credentials : Ragini on 14-08-2019 ///
function resendOTPs(Url) {
    var reqcount = $("#Count").val();
    if (reqcount == 2) {
        document.getElementById("divErrMessagePopup").style.display = "block";
        document.getElementById("spnPopupErrMessage").innerHTML = "Your reach the maximum reasend OTP.Please try again later";
    } else {
        reqcount = parseInt(reqcount) + parseInt(1);
        $("#Count").val(reqcount);
        credentials(Url);
    }
}

/// OTP for Member Credentials : Ragini on 14-08-2019 ///
function OTPSendbuttons(Url) {
    debugger;
    $("<div class='loadingSpinner'></div>").appendTo($("#fsUserCredential"));
    if ($('#OTP').val() != "") {
        document.getElementById("spnValidateOtp").innerHTML = "";
        var otp = $('#OTP').val();
        var opt1 = $('#OtpNumber').val();
        if (otp == $('#OtpNumber').val()) {
            var TempID;
            if ($("#rbtnPassword").is(":checked")) {
                TempID = 1;
            }
            else {
                TempID = 2;
            }
            var webMethodName = "SendCredentials";
            var ParameterNames = new Array();
            var ParameterValues = new Array();
            ParameterNames[0] = "FirstName";
            ParameterValues[0] = $('#FirstName').val();
            ParameterNames[1] = "LastName";
            ParameterValues[1] = $('#LastName').val();
            ParameterNames[2] = "CountryCode";
            ParameterValues[2] = $('#CountryID').val();
            ParameterNames[3] = "MobileNumber";
            ParameterValues[3] = $('#MobileNo').val();
            ParameterNames[4] = "Email";
            ParameterValues[4] = $('#Email').val();
            ParameterNames[5] = "Password";
            ParameterValues[5] = $('#UserPassword').val();
            ParameterNames[6] = "UserName";
            ParameterValues[6] = $('#UserName').val();
            ParameterNames[7] = "TempID";
            ParameterValues[7] = TempID;
            var Url = Url + "DefaultService";
            var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
            $.ajax({
                type: "POST",
                url: Url,
                data: jsonPostString,
                dataType: "json",
                contentType: "application/json",
                success: function (result) {
                    debugger;
                    var obj = result[0];
                    $("#fsUserCredential").find(".loadingSpinner:first").remove();
                    if (obj > 0) {
                        if (TempID == 1) {
                            document.getElementById("divChangePwdPopup").style.display = "block";
                            //$("#divChangePwdPopup").show();
                            document.getElementById("spnPopupMessage").innerHTML = "Your Password is sent to the Mobile Number and Email on record"; // ": " + $('#MobileNo').val() + " and EmailID: " + $('#Email').val();
                            document.getElementById("divUserName").scrollIntoView();
                        } else {
                            document.getElementById("divChangePwdPopup").style.display = "block";
                            document.getElementById("spnPopupMessage").innerHTML = "Your UserName is sent to your Mobile Number and Email on record"; //: " + $('#MobileNo').val() + " and EmailID: " + $('#Email').val();
                            document.getElementById("usernamediv").scrollIntoView();
                        }
                    }
                    else {
                        document.getElementById("divErrMessagePopup").style.display = "block";
                        // $("#divChangePwdPopup").show();
                        document.getElementById("spnPopupErrMessage").innerHTML = "Please enter valid Data";
                    }
                }
            });
        }
        else {
            $("#fsUserCredential").find(".loadingSpinner:first").remove();
            document.getElementById("spnValidateOtp").innerHTML = "Please enter valid OTP";
            return;
        }
    }
    else {
        $("#fsUserCredential").find(".loadingSpinner:first").remove();
        document.getElementById("spnValidateOtp").innerHTML = "Please enter OTP";
        return;
    }
}
