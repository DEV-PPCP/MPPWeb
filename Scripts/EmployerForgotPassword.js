//
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
    var Organizationvalidators = $("#usernamediv").kendoValidator().data("kendoValidator");
    var usernamevalidate = $("#divUserName").kendoValidator().data("kendoValidator");
    if ($("#rbtnUserName").is(":checked") == true) {
        if (Organizationvalidators.validate()) {
            var webMethodName = "ValidateEmpForgotCredentials";
            var username = $("#txtUserName").val();
            var ParameterNames = new Array();
            var ParameterValues = new Array();
            ParameterNames[0] = "UserName";
            ParameterValues[0] = username;
            ParameterNames[1] = "FirstName";
            ParameterValues[1] = $("#First_Name").val();;
            ParameterNames[2] = "LastName";
            ParameterValues[2] = $("#Last_Name").val();
            ParameterNames[4] = "CountryCode";
            ParameterValues[4] = $("#CountryCode").val();
            ParameterNames[3] = "MobileNumber";
            ParameterValues[3] = $("#txtMobileNumber").val();
            ParameterNames[5] = "Email";
            ParameterValues[5] = $("#txtEmail").val();
            ParameterNames[6] = "Type";
            ParameterValues[6] = "2";
            var Url = Url + "Employer";
            var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
            $.ajax({
                type: "POST",
                url: Url,
                data: jsonPostString,
                dataType: "json",
                contentType: "application/json",
                success: function (result) {
                    var obj = result[0];
                    if (obj.length > 0) {
                        $('#FirstName').val(obj[0].FirstName);
                        $('#LastName').val(obj[0].LastName);
                        $('#UserID').val(obj[0].UserID);
                        $('#UserName').val(obj[0].UserName);
                        $('#CountryID').val(obj[0].CountryCode);
                        $('#MobileNo').val(obj[0].MobileNumber);
                        $('#Email').val(obj[0].Email);
                        $('#UserPassword').val(obj[0].UserPassword);
                        $("#OtpNumber").val(obj[0].Otp);
                        document.getElementById("divfsLogin").style.display = "none";
                        document.getElementById("divvalidotp").style.display = "block";
                        document.getElementById("spnMessage").innerHTML = "An Onetime Password is sent to your Mobile Number: " + $('#MobileNo').val();

                      
                         } else {
                        document.getElementById("spnForgotUsername").innerHTML = "Please enter valid data.";
                        return;
                    }

                }
            });

        }
    } else if ($("#rbtnPassword").is(":checked") == true) {
        if (usernamevalidate.validate()) {
            var username = $("#txtUserName").val();
            var webMethodName = "ValidateEmpForgotCredentials";
            var ParameterNames = new Array();
            var ParameterValues = new Array();
            ParameterNames[0] = "UserName";
            ParameterValues[0] = username;
            ParameterNames[1] = "FirstName";
            ParameterValues[1] = $("#First_Name").val();;
            ParameterNames[2] = "LastName";
            ParameterValues[2] = $("#Last_Name").val();
            ParameterNames[3] = "CountryCode";
            ParameterValues[3] = $("#CountryCode").val();
            ParameterNames[4] = "MobileNumber";
            ParameterValues[4] = $("#txtMobileNumber").val();
            ParameterNames[5] = "Email";
            ParameterValues[5] = $("#txtEmail").val();
            ParameterNames[6] = "Type";
            ParameterValues[6] = "1";
            var Url = Url + "Employer";

            var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
            $.ajax({
                type: "POST",
                url: Url,
                data: jsonPostString,
                dataType: "json",
                contentType: "application/json",
                success: function (result) {
                    var obj = result[0];
                    if (obj == "") {
                        document.getElementById("spnUserName").innerHTML = "Username not valid";
                        return;
                    }
                    else {
                        document.getElementById("spnUserName").innerHTML = "";
                        document.getElementById("divfsLogin").style.display = "none";
                        $('#FirstName').val(obj[0].FirstName);
                        $('#LastName').val(obj[0].LastName);
                        $('#UserID').val(obj[0].UserID);
                        $('#UserName').val(obj[0].UserName);
                        $('#CountryID').val(obj[0].CountryCode);
                        $('#MobileNo').val(obj[0].MobileNumber);
                        $('#Email').val(obj[0].Email);
                        $('#UserPassword').val(obj[0].UserPassword);
                        $("#OtpNumber").val(obj[0].Otp);
                        $("#fsSecurityQuestions").show();
                        document.getElementById("divvalidotp").style.display = "block";
                        document.getElementById("spnMessage").innerHTML = "An Onetime Password is sent to your Mobile Number " + $('#MobileNo').val();
                       
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {

                },
            });
        }
    }
}
//Resend OTP FOR Organization Credentials  : Ragini on 27-08-2019
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

function UpdateEmpPassword(Url) {
    var txtpassword = $("#NewPassword").val();
    var txtconfirmpassword = $("#ConfirmPassword").val();
    var ID = $("#UserID").val();
    var webMethodName = "UpdateEmpPassword";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "UserID";
    ParameterValues[0] = ID;
    ParameterNames[1] = "Password";
    ParameterValues[1] = txtconfirmpassword;
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    var Url = Url + "Employer";
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            if (result > 0) {

                document.getElementById("divChangePwdPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Password has been changed successfully.";
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = "Please enter valid Data";
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
        }
    });


}



