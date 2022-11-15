function getformattedJsonFromArray(arrayObj) {
    arrayObj = arrayObj.replace(/"/g, "'");
    return arrayObj + "";
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
//Calling Webmethod to save Terms and Conditions from Admin Module
function SaveTermsAndConditions(Path,TermsCondition, Type, Url) {
    var webMethodName = "InsertTermsAndConditions";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "TermsAndConditionName";
    ParameterValues[0] = TermsCondition;
    ParameterNames[1] = "TempletPath";
    ParameterValues[1] = Path;
    ParameterNames[2] = "Type";
    ParameterValues[2] = Type;

    var Url = Url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $("<div class='loadingSpinner'></div>").appendTo($("#divHeader"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divHeader").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var list = obj[0];
            if (list[0].ResultID != 0) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Terms And Conditions are Saved Successfully.";
                document.getElementById("divSignupPopup").scrollIntoView();
            }

            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = list[0].ResultName + " Please try Again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//Getting Updated Organization Terms and Conditions in Organization Module --Ragini on 02//11/2019
function GetUpdatedOrgLink(OrgType, Url) {
        var webMethodName = "GetTermsAndConditions";
        var ParameterNames = new Array();
        var ParameterValues = new Array();
        ParameterNames[0] = "strType";
        ParameterValues[0] = OrgType;
        var Url = Url + "DefaultService";
        $("<div class='loadingSpinner'></div>").appendTo($("#divGrid"));
        var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
        $.ajax({
            type: "POST",
            url: Url,
            data: jsonPostString,
            dataType: "text",
            contentType: "application/json",
            success: function (results) {
                $("#divGrid").find(".loadingSpinner:first").remove();
                var obj = jQuery.parseJSON(results);
                var Paths = obj[0];
                document.getElementById("spnPatientTCName").innerHTML = Paths[0].TermsAndConditionsName;
                $("#hdnOrganizationTAndConditions").val(Paths[0].TempletPath);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },

        });
    }
//Getting Updated Organization User  Terms and Conditions in Organization Module --Ragini on 02//11/2019
function GetUpdatedUserLink(UserType, Url) {
        var webMethodName = "GetTermsAndConditions";
        var ParameterNames = new Array();
        var ParameterValues = new Array();
        ParameterNames[0] = "strType";
        ParameterValues[0] = UserType;
        var Url = Url + "DefaultService";
        $("<div class='loadingSpinner'></div>").appendTo($("#divGrid"));
        var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
        $.ajax({
            type: "POST",
            url: Url,
            data: jsonPostString,
            dataType: "text",
            contentType: "application/json",
            success: function (result) {
                $("#divGrid").find(".loadingSpinner:first").remove();
                var obj = jQuery.parseJSON(result);
                var Path = obj[0];
                document.getElementById("spnUserTCName").innerHTML = Path[0].TermsAndConditionsName;
                $("#hdnUserTAndConditions").val(Path[0].TempletPath);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },

        });
    }
//Update Terms And condtions in Members Moduldes
function UpdateOrgTermsAndCondition(OrgID, UserID, OrgType, UserType, Url) {
    var webMethodName = "UpdateTermsandConditions";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrgID;
    ParameterNames[1] = "UserID";
    ParameterValues[1] = UserID;
    ParameterNames[2] = "OrgType";
    ParameterValues[2] = OrgType;
    ParameterNames[3] = "OrgUserType";
    ParameterValues[3] = UserType;
  
    var Url = Url + "OrganizationServices";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divGrid").hide();
            if (result.ResultID != 0) {
                var url = $("#RedirectTo").val();
                location.href = url;
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = result.ResultName + " Please try Again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
        
         },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },

    });
}

//Update Terms And condtions in Member Moduldes
function UpdateTermsAndCondition(ID, Url) {
    var webMethodName = "UpdateTermsandConditions";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "MemberID";
    ParameterValues[0] = ID;
    var Url = Url + "Member";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divGrid").hide();
            if (result.ResultID != 0) {
                var url = $("#RedirectTo").val();
                location.href = url;
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = result.ResultName + " Please try Again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },

    });
}
//Member registration Terms&Conditions :Ragini(08/11/2019)
function GetUpdatedLink(Type,Url){
    var webMethodName = "GetTermsAndConditions";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strType";
    ParameterValues[0] = Type;
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
            var Path = obj[0];
            document.getElementById("spnUserTCName").innerHTML = Path[0].TermsAndConditionsName; 
       //     document.getElementById("spnTCName").innerHTML = Path[0].TermsAndConditionsName;

            $("#hdnTAndConditions").val(Path[0].TempletPath);
           
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },

    });
}
