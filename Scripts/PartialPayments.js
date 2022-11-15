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


function GetPlanDetails(MemberPlanCode, URl) {
    debugger;
    var webMethodName = "GetMemberPlanDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strMemberPlanCode";
    ParameterValues[0] = MemberPlanCode;
    var Url = URl + "Member";
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
            document.getElementById("spnOrganizationID").innerHTML = PlansList[0].OrganizationID;
            document.getElementById("spnProviderName").innerHTML = PlansList[0].ProviderName;
            document.getElementById("spnOrganizationName").innerHTML = PlansList[0].OrganizationName;
            document.getElementById("lblPlanName").innerHTML = PlansList[0].PlanName;
            if (PlansList.length > 1) {
                document.getElementById("lbl_PlanType").innerHTML = "Family Plan";
            }
            else {
                document.getElementById("lbl_PlanType").innerHTML = "Individual Plan";

            }

            document.getElementById("lblPlanTenure").innerHTML = PlansList[0].Duration;
            document.getElementById("lblPlanDescription").innerHTML = PlansList[0].PlanDescription;
            document.getElementById("lblFrequency").innerHTML = PlansList[0].Paymentschedule;
            document.getElementById("lblTerm").innerHTML = PlansList[0].NoofInstallments;
            document.getElementById("lblPlanStartDate").innerHTML = PlansList[0].PStartDate;
            document.getElementById("lbl_PlanEndDateValue").innerHTML = PlansList[0].PEndDate;
            document.getElementById("lblVisitFee").innerHTML = PlansList[0].VisitFee;
            document.getElementById("lblEnrollFee").innerHTML = PlansList[0].EnrollFee;
            document.getElementById("lblMonthlyFee").innerHTML = PlansList[0].InstallmentAmount;
            document.getElementById("lbl_NoOfAddMem").innerHTML = PlansList.length;
            document.getElementById("lblPlanAmount").innerHTML = PlansList[0].TotalAmount;
            document.getElementById("lblTransactionFee").innerHTML = PlansList[0].InstallmentFee;
            
            var Amount = document.getElementById("lblMonthlyFee").innerText;

            $("#Amount").val(PlansList[0].AmountPaid);
            debugger;

            //if (PlansList[0].AmountPaid == 0) {
            //    var TotalAmount = 0;
            //    try {
            //        var Amount = PlansList[0].InstallmentAmount;
            //        var InstalmentAmount = PlansList[0].InstallmentFee;
            //        TotalAmount = parseFloat(Amount) + parseFloat(InstalmentAmount);
            //    } catch (err) {

            //    }
            //    $("#AmountPaid").val(TotalAmount);
            //}
            //else {
            //    $("#AmountPaid").val(PlansList[0].InstallmentAmount);
            //}
            
            document.getElementById("lbl_PaidAmount").innerHTML = PlansList[0].AmountPaid;
            document.getElementById("lblPendingAmount").innerHTML = PlansList[0].DueAmount;
            document.getElementById("spnPlan_code").innerHTML = PlansList[0].Plan_Code;
            document.getElementById("spnPlanID").innerHTML = PlansList[0].PlanID;
            document.getElementById("spnStripeAccountID").innerHTML = PlansList[0].AccountID;
            $("#MemberPlans").data("kendoGrid").dataSource.data(PlansList);
            document.getElementById("spnCommPPCP").innerHTML = PlansList[0].CommPPCP;
            document.getElementById("spnCommPrimaryMember").innerHTML = PlansList[0].CommPrimaryMember;
            GetPaymentInstallmentDetails(PlansList[0].MemberPlanID, URl);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            console.log(XMLHttpRequest, textStatus, errorThrown);
        },
    });
}

function GetPaymentInstallmentDetails(MemberPlanID,ServiceUrl){
        debugger;
        var webMethodName = "GetMemberPlanInstallments";
        var ParameterNames = new Array();
        var ParameterValues = new Array();
        ParameterNames[0] = "MemberPlanID";
        ParameterValues[0] = MemberPlanID;
        var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
        var Url = ServiceUrl + "Member";
        $.ajax({
            type: 'POST',
            url: Url,
            data: jsonPostString,
            dataType: "text",
            contentType: "application/json",
            success: function (data, textStatus, jqXHR) {
                debugger;
                var obj = jQuery.parseJSON(data);
                var TestsList = obj[0];
                $("#PaymentsGrid").data("kendoGrid").dataSource.data(TestsList);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                debugger;
            }
        });
    }

//To generate Payment xml in PlanDetails by Gayathri
function MakePlanPayment(MemberRegistrationDetails, Url) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Member/MakePaymentxml',
        data: MemberRegistrationDetails,
        success: function (data, textStatus, jqXHR) {
            CallWebApiServiceforPayment(data, Url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//To make Payment in PlanDetails by Gayathri
function CallWebApiServiceforPayment(data, Url) {
    debugger;
    var webMethodName = "UpdateMemberPlanPayments";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = Url + "MemberXml";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMain"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            var obj = result[0];
            $("#divMain").find(".loadingSpinner:first").remove();
            if (obj[0].ResultID != null && obj[0].TransactionID != null) {
                if (obj[0].StripeCustomerID != null && obj[0].StripeCustomerID != "") {
                    SaveStripeCustomerID(obj[0].StripeCustomerID, obj[0].TransactionID);
                }
                else {

                    $("#divMakePayment").hide();
                    document.getElementById("divPaymentPopup").style.display = "block";
                    document.getElementById("spnPopupMessage").innerHTML = "Payment is successful. Your Transaction ID: " + obj[0].TransactionID;
                    document.getElementById("divPaymentPopup").scrollIntoView();

                }
            }

            else {
                // $("#divMakePayment").hide();
                $("#divErrMessagePopup").show();
                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].result + ". Please try again.";
                //document.getElementById("divErrMessagePopup").scrollIntoView();
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
            $("#divMakePayment").hide();
            document.getElementById("divPaymentPopup").style.display = "block";
            document.getElementById("spnPopupMessage").innerHTML = "Payment is successful. Your Transaction ID: " + TransactionID;
            document.getElementById("divPaymentPopup").scrollIntoView();
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}