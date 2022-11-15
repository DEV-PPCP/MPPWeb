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
//code for subscribe plans for admin module : By Veena
function GetProviderDetails(FromDate, ToDate, ProviderID, Url) {
    var webMethodName = "GetProviderPlanDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strFromDate";
    ParameterValues[0] = FromDate + " " + "12:00:00 AM";
    ParameterNames[1] = "strToDate";
    ParameterValues[1] = ToDate + " " + "11:59:00 PM";
    ParameterNames[2] = "ProviderID";
    ParameterValues[2] = ProviderID;
    var Url = Url + "DefaultService";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainPlan"));
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({

        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divMainPlan").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var ProviderList = obj[0];
            if (ProviderList.length != 0) {
                $("#ViewProSubscribePlanDetailsGrid").data("kendoGrid").dataSource.data(ProviderList);
                document.getElementById("lblErrorMsgSearch").innerHTML = " ";
            }
            else {
                $("#ViewProSubscribePlanDetailsGrid").data("kendoGrid").dataSource.data(ProviderList);
                document.getElementById("lblErrorMsgSearch").innerHTML = "No Records Found ";
            }
            

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#ViewProSubscribePlanDetailsGrid").data("kendoGrid").dataSource.data(ProviderList);
        },
    });
}
//To bind Provider NamesAutocomplete based on OrganizationID in Admin Module By Ragini
function BindProviderNames(FromDate, ToDate, ProviderID, Url) {   
    var webMethodName = "GetProviderPlanDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strFromDate";
    ParameterValues[0] = FromDate;
    ParameterNames[1] = "strToDate";
    ParameterValues[1] = ToDate;
    ParameterNames[2] = "ProviderID";
    ParameterValues[2] = ProviderID;
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
            var ProvidersList = obj[0];
           
            if (ProvidersList.length != 0) {
                $('#ProviderNames').data('kendoAutoComplete').dataSource.data(ProvidersList);
                document.getElementById("lblErrorMsgSearch").innerHTML = " ";
            }
            else {
                $('#ProviderNames').data('kendoAutoComplete').dataSource.data(ProvidersList);
                document.getElementById("lblErrorMsgSearch").innerHTML = "No Records Found ";
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//code for subscribe plans organization dropdownlist for admin module : By Veena
function GetOrganizationDetails(FromDate,ToDate,OrganizationID,Url) {
    var webMethodName = "GetOrganizationPlanDetails";
  var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strFromDate";
    ParameterValues[0] = FromDate +" "+ "12:00:00 AM";
    ParameterNames[1] = "strToDate";
    ParameterValues[1] = ToDate +" "+ "11:59:00 PM";
    ParameterNames[2] = "OrganizationID";
    ParameterValues[2] = OrganizationID;
    var Url = Url + "DefaultService";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainPlan"));
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divMainPlan").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var OrganizationList = obj[0];
            if (OrganizationList.length != 0) {
                $("#ViewOrgSubscribePlanDetailsGrid").data("kendoGrid").dataSource.data(OrganizationList);
                document.getElementById("lblErrorMsgSearch").innerHTML = " ";
            }
            else {
                $("#ViewOrgSubscribePlanDetailsGrid").data("kendoGrid").dataSource.data(OrganizationList);
                document.getElementById("lblErrorMsgSearch").innerHTML = "No Records Found ";
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
// To Get Member Details of Organization using  OrganizationID by Veena
function GetMemberDetails(FromDate,ToDate,MemberID, Url) {
    var webMethodName = "GetMemberPlansDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strFromDate";
    ParameterValues[0] = FromDate + " " + "12:00:00 AM";
    ParameterNames[1] = "strToDate";
    ParameterValues[1] = ToDate + " " + "11:59:00 PM";
    ParameterNames[2] = "MemberID";
    ParameterValues[2] = MemberID;
    var Url = Url + "DefaultService";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainPlan"));
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divMainPlan").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var PlansList = obj[0];
            if (PlansList.length != 0) {
                $("#ViewMemSubscribePlanDetailsGrid").data("kendoGrid").dataSource.data(PlansList);
                document.getElementById("lblErrorMsgSearch").innerHTML = " ";
            }
            else
            {
                $("#ViewMemSubscribePlanDetailsGrid").data("kendoGrid").dataSource.data(PlansList);
                document.getElementById("lblErrorMsgSearch").innerHTML = "No Records Found ";
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}