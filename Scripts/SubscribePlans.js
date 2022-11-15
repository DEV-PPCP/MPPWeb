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

///Binding Values in Subscribe Plan grid in Organization Module  : Ragini on 16-09-2019
function BindSubscribeDetails(url, OrganizationID, Type) {
    var webMethodName = "GetOrganizationPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strOrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "strType";
    ParameterValues[1] = Type;
    var Url = url + "OrganizationServices";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainPlan"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            $("#divMainPlan").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var PlansList = obj[0];
            if (PlansList[0].GenderID == 1) {
                document.getElementById("spnAvailable").innerHTML = "Male";

            }
            else if (PlansList[0].GenderID == 2) {
                document.getElementById("spnAvailable").innerHTML = "Female";
            }
            else {
                document.getElementById("spnAvailable").innerHTML = "Male,Female";
            }

            $("#ViewSubscribePlanDetailsGrid").data("kendoGrid").dataSource.data(PlansList);
           
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMainPlan").find(".loadingSpinner:first").remove();
        },
    });

}

/**/
function SaveEnrollPlans(OrgId, PlanId, PlanStartDate, Url) {
    var webMethodName = "InsertOrganizationPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrgId;
    ParameterNames[1] = "PlanID";
    ParameterValues[1] = PlanId;
    ParameterNames[2] = "StartDate";
    ParameterValues[2] = PlanStartDate;
    var Url = Url + "OrganizationServices";
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
            $("#divSuccess").show();
            var obj = jQuery.parseJSON(result);
           
            if (obj[0] == 1) {
                $("#lblMsg").show();
                document.getElementById("lblMsg").innerHTML = "Plan Details Submitted Successfully";
            }
            else if ( obj[0] == null) {
                $("#lblMsg").show();
                document.getElementById("lblMsg").innerHTML = "Please Enter Valid Details ";
            }
          },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMainPlan").find(".loadingSpinner:first").remove();
        },
    });

}



//Bind Provider Details :Ragini  On 26/09/2019
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
            $('#ProviderNames').data('kendoDropDownList').dataSource.data(ProvidersList);
          
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}


//binding provider plans  :Ragini on 26-09-2019
function OrganizationplanProviders(ProviderId, OrgId, Type, Url) {
    debugger;
    var webMethodName = "GetOrganizationProviderPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strOrganizationID";
    ParameterValues[0] = OrgId;
    ParameterNames[1] = "strProviderID";
    ParameterValues[1] = ProviderId;
    ParameterNames[2] = "strType";
    ParameterValues[2] = Type;
    var Url = Url + "OrganizationServices";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainPlan"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            $("#divMainPlan").find(".loadingSpinner:first").remove();
                var obj = jQuery.parseJSON(result);
                var ProvidersList = obj[0];
                if (result = ""){
                if (ProvidersList[0].GenderID == 1) {
                    document.getElementById("spnAvailable").innerHTML = "Male";

                }
                else if (ProvidersList[0].GenderID == 2) {
                    document.getElementById("spnAvailable").innerHTML = "Female";
                }
                else if (ProvidersList[0].GenderID == 3) {
                    document.getElementById("spnAvailable").innerHTML = "Male,Female";
                }
            }

            $("#ViewProviderPlanGrid").data("kendoGrid").dataSource.data(ProvidersList);
             },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function GetSubscribePlans(OrgID,OrgName,PlanID,PlanName,StartDate,ProviderID,ProviderName,Url) {
    var webMethodName = "InsertOrganizationProviderPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrgID;
    ParameterNames[1] = "PlanID";
    ParameterValues[1] = PlanID;
    ParameterNames[2] = "StartDate";
    ParameterValues[2] = StartDate;
    ParameterNames[3] = "ProviderID";
    ParameterValues[3] = ProviderID;
    ParameterNames[4] = "OrganizationName";
    ParameterValues[4] = OrgName;
    ParameterNames[5] = "PlanName";
    ParameterValues[5] = PlanName;
    ParameterNames[6] = "ProviderName";
    ParameterValues[6] = ProviderName;
    var Url = Url + "OrganizationServices";
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
            $("#divSuccess").show();
            var obj = jQuery.parseJSON(result);
            var plans = obj[0];
            if (plans[0].ResultID == 1) {
                $("#lblMsg").show();
                document.getElementById("lblMsg").innerHTML = "Plan Details Submitted Successfully";
            }
            else if (plans[0].ResultName != null) {
                $("#lblErrorMsg").show();
                document.getElementById("lblErrorMsg").innerHTML = plans[0].ResultName + " .Please Enter Valid Details ";
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMainPlan").find(".loadingSpinner:first").remove();
        },
    });

   
}

function DateSelectionChanged(e) {
    //var birthDate = $("#OrgPlanStartDate").val();
    var birthDate = $("#OrgPlanStartDate").val();
    $.ajax({
        url: "/Master/GetAge/",
        data: { birthDate: birthDate },
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {
            var rs = result.split(';');
            var year = rs[0];
            //$("#Age").val(year);
        }

    });
}
//code for subscribe plans for admin module
function GetProviderDetails(OrganizationID, ProviderId, Url) {
    var webMethodName = "GetProviderDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strOrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "strProviderID";
    ParameterValues[1] = ProviderId;
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
            var PlansList = obj[0];

            $("#ViewProSubscribePlanDetailsGrid").data("kendoGrid").dataSource.data(PlansList);

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function UnSubscribePlan(PlanID, OrgId, MapID, Url) {
    debugger;
    var webMethodName = "UnSubscribePlanDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrgId;
    ParameterNames[1] = "PlanID";
    ParameterValues[1] = PlanID;
    ParameterNames[2] = "MapID";
    ParameterValues[2] = MapID;
    var Url = Url + "OrganizationServices";
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
            $("#divSuccess").show();
            var obj = jQuery.parseJSON(result);
            var plans = obj[0];
            debugger;
            if (plans[0].ResultID == 1) {
                $("#lblMsg").show();
                document.getElementById("lblMsg").innerHTML = "Plan UnSubscribed Successfully";
            }
            else if (obj[0].ResultID == null) {
                $("#lblMsg").show();
                document.getElementById("lblMsg").innerHTML = "UnSubscrib failure ";
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMainPlan").find(".loadingSpinner:first").remove();
        },
    });
}
function UnSubscribedProviderPlanDetails(OrganizationID, ProviderID, MapID, URL) {
    debugger;
    var webMethodName = "UnSubscribedProviderPlan";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
   // ParameterNames[1] = "PlanID";
   // ParameterValues[1] = PlanID;
    ParameterNames[1] = "ProviderID";
    ParameterValues[1] = ProviderID;
    ParameterNames[2] = "MapID";
    ParameterValues[2] = MapID;
    var Url = URL + "OrganizationServices";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainPlan"));
    $.ajax({

        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;   
            $("#divMainPlan").find(".loadingSpinner:first").remove();
            $("#divSuccess").show();
            var obj = jQuery.parseJSON(result);
            var plans = obj[0];
            if (plans[0].ResultID == 1) {
                $("#lblMsg").show();
                document.getElementById("lblMsg").innerHTML = "Plan UnSubscrib Successfully";
            }
            else if (plans[0].ResultName != null) {
                $("#lblErrorMsg").show();
                document.getElementById("lblErrorMsg").innerHTML =" UnSubscrib failure ";
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMainPlan").find(".loadingSpinner:first").remove();
        },
    });
}