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

function GetOrganizationDetails(OrganizationID, Url) {
    var webMethodName = "GetOrganizationDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strOrganizationID";
    ParameterValues[0] = OrganizationID;
    var Url = Url + "OrganizationServices";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var OrganizationList = obj[0];
            $("#gvOrganizationGrid").data("kendoGrid").dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//To bind Organizations dropdownlist by Ragini
function BindOrganizations(Url, orgId) {
    var webMethodName = "GetPPCPOrganizations";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strOrganizationID";
    ParameterValues[0] = orgId;
    var Url = Url + "DefaultService";
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

            $('#OrganizationName').data('kendoAutoComplete').dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//To Bind States dropdownlist based on CountryID by Ragini
function BindStates(CountryID, ContolID, Url, tempValue) {
    if (tempValue == "2") {
        var Controlname = '#' + ContolID;
        $(Controlname).data('kendoDropDownList').value("");
    }
    var webMethodName = "GetStates";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "CountryID";
    ParameterValues[0] = CountryID;

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
            var StatesListList = obj[0];
            if (tempValue == "1") {
                $('#OrgStateName').data('kendoDropDownList').dataSource.data(StatesListList);
                $('#StateName').data('kendoDropDownList').dataSource.data(StatesListList);
            } else {
                $(Controlname).data('kendoDropDownList').dataSource.data(StatesListList);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//To bind Cities dropdownlist based on StateID by Gayathri
function BindCities(StateID, ContolID, Url, tempValue) {
    if (tempValue == "2") {
        var Controlname = '#' + ContolID;
        $(Controlname).data('kendoDropDownList').value("");
    }
    
    var webMethodName = "GetCities";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "StateID";
    ParameterValues[0] = StateID;

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
            var CitiesListList = obj[0];
            if (tempValue == "1") {

                $('#CityName').data('kendoDropDownList').dataSource.data(CitiesListList);
                $('#OrgCityName').data('kendoDropDownList').dataSource.data(CitiesListList);
            } else {
                $(Controlname).data('kendoDropDownList').dataSource.data(CitiesListList);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function BindProviderNames(OrganizationID, Url) {
    var webMethodName = "GetProviderDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strOrganizationID";
    ParameterValues[0] = OrganizationID;
    var Url = Url + "OrganizationServices";
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
            $('#ProviderNames').data('kendoAutoComplete').dataSource.data(ProvidersList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}