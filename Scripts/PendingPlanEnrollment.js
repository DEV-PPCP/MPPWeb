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
function BindPendingPaymentsDetails(Url, OrganizationID) {
    debugger;
    var webMethodName = "GetMemberPendingEnrollment";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    var Url = Url + "OrganizationServices";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMain"));
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            $("#divMain").find(".loadingSpinner:first").remove();
            debugger;
            var obj = jQuery.parseJSON(result);
            var PaymentsList = obj[0];
            if (PaymentsList.length != 0) {
                
                $("#PaymentsPendinggrid").data("kendoGrid").dataSource.data(PaymentsList);
            }
            else {
                document.getElementById("spnSelectedTotalAmount").innerHTML = 0;
                document.getElementById("spnSelectedPaidAmount").innerHTML = 0;
                document.getElementById("spnSelectedDueAmount").innerHTML = 0;
                $("#PaymentsPendinggrid").data("kendoGrid").dataSource.data(PaymentsList);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMain").find(".loadingSpinner:first").remove();
        },
    });
}
function BindingPlansGrids(OrganizationID, ProviderID, PlanID, MemberGender, MemberAge, url) {
    debugger;
    var webMethodName = "GetPPCPOrganizationProviderPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "ProviderID";
    ParameterValues[1] = ProviderID;
    ParameterNames[2] = "PlanID";
    ParameterValues[2] = PlanID;
    ParameterNames[3] = "MemberAge";
    ParameterValues[3] = MemberAge;
    ParameterNames[4] = "MemberGender";
    ParameterValues[4] = MemberGender;
    ParameterNames[5] = "PlanType";
    ParameterValues[5] = "1";
    var Url = url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var obj = jQuery.parseJSON(result);
            var PlansList = obj[0];

            var PlansList1 = [];
            PlansList1.push(PlansList[0]);
            $("#PlansGrid").data("kendoGrid").dataSource.data("");
            $("#SearchPlan").show();
            $("#divSelectPlan").show();

            var OrganizationID = $("#OrganizationID").val();

            $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList);



        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
           // ErrorMessage(webMethodName, textStatus);
        },
    });
}


function BindPlans(OrganizationID, ProviderID, PlanID, MemberGender, MemberAge, url) {
    debugger;
    
    var webMethodName = "GetPPCPOrganizationProviderPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "ProviderID";
    ParameterValues[1] = ProviderID;
    ParameterNames[2] = "PlanID";
    ParameterValues[2] = PlanID;
    ParameterNames[3] = "MemberAge";
    ParameterValues[3] = MemberAge;
    ParameterNames[4] = "MemberGender";
    ParameterValues[4] = MemberGender;
    ParameterNames[5] = "PlanType";
    ParameterValues[5] = "1";
    var Url = url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var obj = jQuery.parseJSON(result);
            var PlansList = obj[0];
            if (PlanID == 0) {
                $('#PlanNames').data('kendoAutoComplete').dataSource.data(PlansList);
                $("#divSelectPlan").show();
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList);
            }
            else {
                $("#divPlanPayment").show();
                $("#PlansPayment").data("kendoGrid").dataSource.data(PlansList);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            debugger;
           // ErrorMessage(webMethodName, textStatus);
        },
    });
}
function EnrollPlan(MemberRegistrationDetails, url) {
    debugger;
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Master/PendingPlanEnroll',
        data: MemberRegistrationDetails,
        success: function (data, textStatus, jqXHR) {
            debugger;
            CallWebApiServices(data, url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function CallWebApiServices(data, url) {
    debugger;
    var webMethodName = "EnrollPlanDetails";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = url + "MemberXml";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMain"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var obj = result[0];
            $("#divMain").find(".loadingSpinner:first").remove();
            if (obj[0].result == null && obj[0].MemberID != null && obj[0].TransactionID != null) {
                if (obj[0].StripeCustomerID != null && obj[0].StripeCustomerID != "") {
                    SaveStripeCustomerID(obj[0].StripeCustomerID, obj[0].TransactionID);
                }
                else {
                    document.getElementById("divSignupPopup").style.display = "block";
                    document.getElementById("spnPopupMessage").innerHTML = "Plan enrolled successfully. Your Transaction ID: " + obj[0].TransactionID;
                    document.getElementById("divSignupPopup").scrollIntoView();
                }

            }
            else if (obj[0].result == null && obj[0].MemberID != null) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Plan enrolled successfully.";
                document.getElementById("divSignupPopup").scrollIntoView();
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].result + ". Please try again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function SaveStripeCustomerID(StripeCustomerID, TransactionID) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Member/SaveStripeCustomerID ',
        data: { StripeCustomerID: StripeCustomerID },
        success: function (data, textStatus, jqXHR) {
            debugger;
            document.getElementById("divSignupPopup").style.display = "block";
            document.getElementById("spnPopupMessage").innerHTML = "Plan enrolled successfully. Your Transaction ID: " + TransactionID;
            document.getElementById("divSignupPopup").scrollIntoView();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}