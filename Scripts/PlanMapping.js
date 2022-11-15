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
//To bind Organizations dropdownlist by Gayathri
function BindOrganizations(Url) {
    var webMethodName = "GetPPCPOrganizations";
    var ParameterNames = "";
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
           
            $('#OrganizationNames').data('kendoAutoComplete').dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//To bind Provider Names dropdown based on OrganizationID by Gayathri
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
//To Bind plans dropdownlist based on OrganizationID,PrividerID and PlanID by Gayathri

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
            

            $("#PlanName").data("kendoAutoComplete").dataSource.data(PlansList);

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function SavePlanMapping(OrganizationName, OrganizationID, ProviderName,
                    ProviderID, PlanName, PlanID, url) {
    
    var webMethodName = "SavePlanMapping";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationName";
    ParameterValues[0] = OrganizationName;
    ParameterNames[1] = "OrganizationID";
    ParameterValues[1] = OrganizationID;
    ParameterNames[2] = "ProviderName";
    ParameterValues[2] = ProviderName;
    ParameterNames[3] = "ProviderID";
    ParameterValues[3] = ProviderID;
    ParameterNames[4] = "PlanName";
    ParameterValues[4] = PlanName;
    ParameterNames[5] = "PlanID";
    ParameterValues[5] = PlanID;
    var Url = url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $("<div class='loadingSpinner'></div>").appendTo($("#divAddPlans"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divAddPlans").find(".loadingSpinner:first").remove();

            var res = jQuery.parseJSON(result);
            var obj = res[0];
            if (obj[0].ResultID >= 1 && obj[0].ResultName == null) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").style.color = "green";
                document.getElementById("spnPopupMessage").innerHTML = "Plan Mapping saved Successfully";
                document.getElementById("divSignupPopup").scrollIntoView();
            }
            else if (obj[0].ResultID == -1 && obj[0].ResultName != null) {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").style.color = "Orange";
                document.getElementById("spnPopupErrMessage").innerHTML = "Already Mapped.";
                document.getElementById("divSignupPopup").scrollIntoView();
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").style.color = "red";
                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].ResultName + ". Please try again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest, textStatus, errorThrown);
        },

    });
}

function CallToSavePlanMappingDetails(data, url) {
    var webMethodName = "SavePlanMapping";
    var ParameterNames = data;
    var Url = url + "DefaultXml";
    var jsonPostString = setParameter(ParameterNames, webMethodName);
    $("<div class='loadingSpinner'></div>").appendTo($("#divAddPlans"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divAddPlans").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var plans = obj[0];
            if (plans[0].ResultID >= 1) {
                $("#divSuccess").show();
                $("#lblUserMsg").show();
                document.getElementById("lblUserMsg").innerHTML = "Plan Details Saved Successfully";
            }
            else if (plans[0].ResultName == null) {
                document.getElementById("lblUserMsg").innerHTML = plans[0].ResultName;
            }
            else {
                $("#divSuccess").show();
                $("#lblUserMsg").show();
                document.getElementById("lblUserMsg").innerHTML = "Please Enter Valid Details ";
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest, textStatus, errorThrown);
        },

    });
}
