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




function GetMemberFamilyPlanDetails(Url, MemberParentID, Type) {
    debugger;
    var webMethodName = "GetMemberFamilyPlanDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strMemberParentID";
    ParameterValues[0] = MemberParentID;
    ParameterNames[1] = "PlanType";
    ParameterValues[1] = Type;
    var Url = Url + "Member";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $("<div class='loadingSpinner'></div>").appendTo($("#divOrganizationPayments"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divOrganizationPayments").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var OrganizationList = obj[0];
            $('#PlansGrid').data('kendoGrid').dataSource.data(OrganizationList);
            //if (Type==2) {

            //    if (OrganizationList[0].MemberParentID == OrganizationList[0].MemberID) {
            //        var individualList = OrganizationList[0];

            //        $('#PlansGrid').data('kendoGrid').dataSource.data(individualList);
            //    }

            //}
            //else {
            //    $('#PlansGrid').data('kendoGrid').dataSource.data(OrganizationList);
            //}
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divOrganizationPayments").find(".loadingSpinner:first").remove();
        },
    });
}

function GetFamilyPlanMemberDetails(Url, MemberParentID) {
    debugger;
    var webMethodName = "GetFamilyPlanMemberDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "MemberPlanID";
    ParameterValues[0] = MemberParentID;
    var Url = Url + "Member";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $("<div class='loadingSpinner'></div>").appendTo($("#divOrganizationPayments"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            $("#divOrganizationPayments").find(".loadingSpinner:first").remove();
            var objlist = jQuery.parseJSON(result);
            var objlist = objlist[0];
            var jsonResult = "";
            var jsonResults = ""
            for (var r in objlist) {
                jsonResult += "<input type='radio' name='" + "AccountType" + "' value='" + objlist[r].MemberID + "' onclick='" + "GetMemberID(this.value)" + "' />" +
                "<label for='" + objlist[r].MemberName + "'>" + objlist[r].MemberName + "</label>" + "<br />"
                $("#divMemberDetails").html(jsonResult);
            }
            document.getElementById("divPaymentPopup").style.display = "block";

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divOrganizationPayments").find(".loadingSpinner:first").remove();
        },
    });
}