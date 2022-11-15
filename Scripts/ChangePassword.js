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

///Updating Password in MemberChangePassword : Ragini on 14-08-2019 ///
function btnUpdateScript(MemberID,Url) {
    var txtpassword = $("#NewPassword").val();
    var txtconfirmpassword = $("#ConfirmPassword").val();

    if (txtpassword == txtconfirmpassword) {
        document.getElementById("spnErrorMessage").innerHTML = "";

        var webMethodName = "validateChangePassword";
        var ParameterNames = new Array();
        var ParameterValues = new Array();
        ParameterNames[0] = "UserID";
        ParameterValues[0] = MemberID;
        ParameterNames[1] = "Password";
        ParameterValues[1] = txtconfirmpassword;
        ParameterNames[2] = "TypeID";
        ParameterValues[2] = "1";
        var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
        var Url = Url + "DefaultService";
        $.ajax({
            type: "POST",
            url: Url,
            data: jsonPostString,
            dataType: "json",
            contentType: "application/json",
            success: function (result) {
                var obj = result[0];
                if (obj[0].ResultID > 0) {

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
    else {
        document.getElementById("spnErrorMessage").innerHTML = "Password doesnot match.";

    }
}
///Updating Password in OrganizationChangePassword : Ragini on 14-08-2019 ///
function btnOrgUpdateScript(UserID, Url) {
    var txtpassword = $("#NewPassword").val();
    var txtconfirmpassword = $("#ConfirmPassword").val();

    if (txtpassword == txtconfirmpassword) {
        document.getElementById("spnErrorMessage").innerHTML = "";

        var webMethodName = "UpdateOrgPassword";
        var ParameterNames = new Array();
        var ParameterValues = new Array();
        ParameterNames[0] = "UserID";
        ParameterValues[0] = UserID;
        ParameterNames[1] = "Password";
        ParameterValues[1] = txtconfirmpassword;
      
        var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
        var Url = Url + "OrganizationServices";
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
    else {
        document.getElementById("spnErrorMessage").innerHTML = "Password doesnot match.";

    }
}
