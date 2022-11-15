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
//To get organization details by vinod

function GetOrganizationDetails(OrganizationID,Url) {
    var webMethodName = "GetDisabledOrganizationDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strOrganizationID";
    ParameterValues[0] = OrganizationID;
    var Url = Url + "DefaultService";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainRegistration"));
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var OrganizationList = obj[0];
            $("#gvOrganizationGrid").data("kendoGrid").dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}