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

//To Bind PaymentsDetails grid based on OrganizationID in PaymentDetails by veena
function BindViewPaymentDetails(OrganizationID, ToDate, FromDate, Url) {
    debugger;
    var MemberIDValue = 0;
    var MemberID = $("#MemberId").val()
    if (MemberID == "" || MemberID == null) {
        MemberIDValue = 0;
    } else MemberIDValue = $("#MemberId").val();
    //Webservice using Entity Framework
    var webMethodName = "GetOrganizationPaymentDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationId";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "ToDate";
    ParameterValues[1] = ToDate;
    ParameterNames[2] = "FromDate";
    ParameterValues[2] = FromDate;
    ParameterNames[3] = "MemberID";
    ParameterValues[3] = MemberIDValue;//$("#MemberId").val();
    ParameterNames[4] = "PaymentStatus";
    ParameterValues[4] = $("#ddlpaymentstatus").val();
    var rbtnMale = $('input[id=rbtnIndividual]:checked').val();
    var rbtnFEM = $('input[id=rbtnFamily]:checked').val();
    if (rbtnMale == 1) {
        ParameterNames[5] = "PlanType";
        ParameterValues[5] = "1";
    }
    else if (rbtnFEM == 2) {

        ParameterNames[5] = "PlanType";
        ParameterValues[5] = "2";
    }
    else {
        ParameterNames[5] = "PlanType";
        ParameterValues[5] = "0";
    }
    var Url = Url + "OrganizationServices";
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
            debugger;
            var obj = jQuery.parseJSON(result);
            var PaymentsList = obj[0];
            $("#divAmountDetails").show();
            $("#DivPaymentsGrid").show();
            if (PaymentsList.length != 0) {
                document.getElementById("divAmountDetails").style.display = "block";
                document.getElementById("spnSelectedTotalAmount").innerHTML = PaymentsList[0].GrandTotalAmount;
                document.getElementById("spnSelectedPaidAmount").innerHTML = PaymentsList[0].GrandAmountPaid;
                document.getElementById("spnSelectedDueAmount").innerHTML = PaymentsList[0].GrandTotalAmount - PaymentsList[0].GrandAmountPaid;
                $("#PaymentsGrid").data("kendoGrid").dataSource.data(PaymentsList);
            }
            else {
                document.getElementById("spnSelectedTotalAmount").innerHTML = 0;
                document.getElementById("spnSelectedPaidAmount").innerHTML = 0;
                document.getElementById("spnSelectedDueAmount").innerHTML = 0;
                $("#PaymentsGrid").data("kendoGrid").dataSource.data(PaymentsList);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
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
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainRegistration"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
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
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
        },
    });
}

//To Bind PaymentsDetails grid based on OrganizationID in PaymentDetails based on search criteria by veena
function BindAdminViewPaymentDetails(ToDate, FromDate, OrganizationID, ProviderID, PlanID, Url) {

    //Webservice using Entity Framework
    var webMethodName = "GetAdminPaymentDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strToDate";
    ParameterValues[0] = ToDate + " " + "11:59:00 PM";
    ParameterNames[1] = "strFromDate";
    ParameterValues[1] = FromDate + " " + "12:00:00 AM";
    ParameterNames[2] = "strOrganizationID";
    ParameterValues[2] = OrganizationID;
    ParameterNames[3] = "strProviderID";
    ParameterValues[3] = ProviderID;
    ParameterNames[4] = "strPlanID";
    ParameterValues[4] = PlanID;
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
            var PaymentsList = obj[0];
            if (PaymentsList.length != 0) {
                document.getElementById("divAmountDetails").style.display = "block";
                document.getElementById("spnSelectedTotalAmount").innerHTML = PaymentsList[0].GrandTotalAmount;
                document.getElementById("spnSelectedPaidAmount").innerHTML = PaymentsList[0].GrandAmountPaid;
                document.getElementById("spnSelectedDueAmount").innerHTML = PaymentsList[0].GrandTotalAmount - PaymentsList[0].GrandAmountPaid;
                $("#PaymentsGrid").data("kendoGrid").dataSource.data(PaymentsList);
            }
            else {
                document.getElementById("spnSelectedTotalAmount").innerHTML = 0;
                document.getElementById("spnSelectedPaidAmount").innerHTML = 0;
                document.getElementById("spnSelectedDueAmount").innerHTML = 0;
                $("#PaymentsGrid").data("kendoGrid").dataSource.data(PaymentsList);
                document.getElementById("spnSearch").innerHTML = "No Records Found ";

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
        },
    });
}
//To Bind PaymentsDetails grid based on PlanCode in PaymentDetails by veena
function BindAdminViewPaymentsGridDetails(intPlanCode, Url) {
    //Webservice using Entity Framework
    var webMethodName = "GetMemberPlanAndPaymentsDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strPlanCode";
    ParameterValues[0] = intPlanCode;

    var Url = Url + "Member";
    $("<div class='loadingSpinner'></div>").appendTo($("#divViewPaymentDetails"));
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            $("#divViewPaymentDetails").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var PaymentsList = obj[0];
            $("#ViewPaymentsGrid").data("kendoGrid").dataSource.data(PaymentsList);

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divViewPaymentDetails").find(".loadingSpinner:first").remove();
        },
    });
}
//Bind Organizations dropdownlist
function BindOrganizations(Url) {
    var webMethodName = "GetPPCPOrganizations";
    var ParameterNames = "";
    var Url = Url + "DefaultService";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainRegistration"));
    var jsonPostString = setParameter(ParameterNames, webMethodName);
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
            $('#OrganizationNames').data('kendoAutoComplete').dataSource.data(OrganizationList);
            //for (var r in OrganizationList) {
            //    $('#OrganizationNames').data('kendoDropDownList').dataSource.insert(r, { Text: OrganizationList[r].OrganizationName, Value: OrganizationList[r].OrganizationID });
            //}
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
        },
    });
}

//Bind providers based on OrganizationId
function BindProviderNames(OrganizationID, Url) {
    var webMethodName = "GetProviderDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strOrganizationID";
    ParameterValues[0] = OrganizationID;
    var Url = Url + "OrganizationServices";
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
            var ProvidersList = obj[0];
            if (ProvidersList.length != 0) {
                ProvidersList[0].FirstName = ProvidersList[0].FirstName + " " + ProvidersList[0].LastName;
                $('#ProviderNames').data('kendoAutoComplete').dataSource.data(ProvidersList);
                document.getElementById("spnSearch").innerHTML = " ";
            }
            else {
                ProvidersList[0].ProviderName = "" + " " + "";
                $('#ProviderNames').data('kendoAutoComplete').dataSource.data([]);
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
        },
    });
}
//To Bind plans dropdownlist based on OrganizationID,PrividerID and PlanID by Veena

function BindPlanNames(OrganizationID, ProviderID, PlanID, url) {
    var webMethodName = "GetPPCPOrganizationProviderPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "ProviderID";
    ParameterValues[1] = ProviderID;
    ParameterNames[2] = "PlanID";
    ParameterValues[2] = PlanID;
    var Url = url + "DefaultService";
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
            var PlansList = obj[0];
            if (PlansList.length != 0) {
                $('#PlanNameList').data('kendoAutoComplete').dataSource.data(PlansList);
            }
            else {

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
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

            $('#OrganizationNames').data('kendoAutoComplete').dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}