

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

///Binding Values in View Plan grid in Admin module : Ragini on 13-09-2019
function BindPlanDetails(url,Type) { 
        var webMethodName = "GetPlans";
        var ParameterNames = new Array();
        var ParameterValues = new Array();
        ParameterNames[0] = "Type";
        ParameterValues[0] = Type;
        var Url = url + "DefaultService";
        var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
        $.ajax({
            type: "POST",
            url: Url,
            data: jsonPostString,
            dataType: "text",
            contentType: "application/json",
            success: function (result) {
                var obj = jQuery.parseJSON(result);
                var PlansList = obj[0];
               
                $("#ViewPlanDetailsGrid").data("kendoGrid").dataSource.data(PlansList);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });

    }

  

