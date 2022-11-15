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
function BindOrganizations(Url) {
    debugger;

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
            debugger;
            var obj = jQuery.parseJSON(result);
            var OrganizationList = obj[0];
            $('#OrganizationNames').data('kendoAutoComplete').dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function BindProviderNames(OrganizationID, url) {
    debugger;
  
    //if (OrganizationID != 0) {
    //    $("#ProviderNames").data("kendoAutoComplete").destroy();
   // }
    var webMethodName = "GetPPCPOrganizationProviders";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
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
            var ProvidersList = obj[0];
            $('#ProviderNames').data('kendoAutoComplete').dataSource.data(ProvidersList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            debugger;
        },
    });
}

function BindPlanNames(OrganizationID, ProviderID, PlanID,MemberAge,MemberGender,PlanType, Url) {
    debugger;
   
    //if (OrganizationID != 0 && ProviderID != 0 && PlanID == 0) {
    //    $("#ProviderNames").data("kendoAutoComplete").destroy();
    //}
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
    ParameterValues[5] = PlanType;
    var Url = Url+"DefaultService";
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
            $('#PlanNames').data('kendoAutoComplete').dataSource.data(PlansList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function BindPlans(OrganizationID, ProviderID, PlanID,MemberCount,MemberAge,MemberGernder,PlanType, Url) {
    debugger;
    if (OrganizationID != 0 && ProviderID != 0 && PlanID == 0) {
        $("#ProviderNames").data("kendoAutoComplete").destroy();
    }
    var webMethodName = "GetPlanPaymentDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "ProviderID";
    ParameterValues[1] = ProviderID;
    ParameterNames[2] = "PlanID";
    ParameterValues[2] = PlanID;
    ParameterNames[3] = "MemberCount";
    ParameterValues[3] = MemberCount;
    ParameterNames[4] = "MemberAge";
    ParameterValues[4] = MemberAge;
    ParameterNames[5] = "MemberGernder";
    ParameterValues[5] = MemberGernder;
    ParameterNames[6] = "PlanType";
    ParameterValues[6] = PlanType;
    var Url = Url + "DefaultService";
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
        },
    });
}


function BindingPlansGrid(OrganizationID, ProviderID, PlanID,MemberAge,MemberGender,PlanType, url) {
    debugger;
    if (MemberGender == undefined) {
        MemberGender = "";
    }
    if (MemberAge == undefined) {
        MemberAge = "";
    }
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
    ParameterValues[5] = PlanType;
    var Url = url + "DefaultService";
    //var Url = "http://192.168.1.20/ppcpwebservice/DefaultService";
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
            if (OrganizationID == 0 && ProviderID == 0 && PlanID != 0) {
                var PlansList1 = [];
                PlansList1.push(PlansList[0]);
                $("#PlansGrid").data("kendoGrid").dataSource.data("");
                $("#divSelectPlan").show();
                $("#OrganizationNames").data("kendoAutoComplete").destroy();
                $("#ProviderNames").data("kendoAutoComplete").destroy();
                $("#ProviderNames").kendoAutoComplete({
                    dataSource: PlansList1, dataTextField: "ProviderName", dataValueField: "ProviderID", select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        $("#ProviderName").val(dataItem.ProviderName);
                        $("#ProviderID").val(dataItem.ProviderID);
                        var OrganizationID = $("#OrganizationID").val();
                        var ProviderID = $("#ProviderID").val();
                        var PlanID = $("#PlanID").val();
                      //  BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                    }
                });
                $("#OrganizationNames").kendoAutoComplete({
                    dataSource: PlansList1, dataTextField: "OrganizationName", dataValueField: "OrganizationID", select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        $("#OrganizationName").val(dataItem.OrganizationName);
                        $("#OrganizationID").val(dataItem.OrganizationID);
                        var OrganizationID = $("#OrganizationID").val();
                        var ProviderID = $("#ProviderID").val();
                        var PlanID = $("#PlanID").val();
                      //  BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                    }
                });
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList1);
            }
            else if (OrganizationID == 0 && ProviderID != 0 && PlanID == 0) {
                $("#PlansGrid").data("kendoGrid").dataSource.data("");
                $("#divSelectPlan").show();
              // $("#OrganizationNames").data("kendoAutoComplete").destroy();
                $("#PlanNames").data("kendoAutoComplete").destroy();
                //$("#OrganizationNames").kendoAutoComplete({
                //    dataSource: PlansList, dataTextField: "OrganizationName", dataValueField: "OrganizationID", select: function (e) {
                //        var dataItem = this.dataItem(e.item.index());
                //        $("#OrganizationName").val(dataItem.OrganizationName);
                //        $("#OrganizationID").val(dataItem.OrganizationID);
                //        var OrganizationID = $("#OrganizationID").val();
                //        var ProviderID = $("#ProviderID").val();
                //        var PlanID = $("#PlanID").val();
                //       // BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                //    }
                //});
                $("#PlanNames").kendoAutoComplete({
                    dataSource: PlansList, dataTextField: "PlanName", dataValueField: "PlanID", select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        $("#PlanName").val(dataItem.PlanName);
                        $("#PlanID").val(dataItem.PlanID);
                        var OrganizationID = $("#OrganizationID").val();
                        var ProviderID = $("#ProviderID").val();
                        var PlanID = $("#PlanID").val();
                      //  BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                    }
                });
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList);
                BindSpecificOrganizations(PlansList[0].OrganizationID, url);
            }
            else if (OrganizationID == 0 && ProviderID != 0 && PlanID != 0) {
                var PlansList1 = [];
                PlansList1.push(PlansList[0]);
               $("#PlansGrid").data("kendoGrid").dataSource.data();
                $("#divSelectPlan").show();
               $("#OrganizationNames").data("kendoAutoComplete").destroy();
                $("#OrganizationNames").kendoAutoComplete({
                    dataSource: PlansList1, dataTextField: "OrganizationName", dataValueField: "OrganizationID", select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        $("#OrganizationName").val(dataItem.OrganizationName);
                        $("#OrganizationID").val(dataItem.OrganizationID);
                        var OrganizationID = $("#OrganizationID").val();
                        var ProviderID = $("#ProviderID").val();
                        var PlanID = $("#PlanID").val();
                       // BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                    }
                });
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList1);
            }
            else if (OrganizationID != 0 && ProviderID == 0 && PlanID == 0) {
                $("#PlansGrid").data("kendoGrid").dataSource.data("");
                $("#divSelectPlan").show();
               // $("#ProviderNames").data("kendoAutoComplete").destroy();
                $("#PlanNames").data("kendoAutoComplete").destroy();

                var OrganizationIDs = $("#OrganizationID").val();
                var ProviderIDs = $("#ProviderID").val();
                var PlanIDs = $("#PlanID").val();

                BindProviderNames(OrganizationIDs, url);
                BindPlanNames(OrganizationIDs, ProviderIDs, PlanIDs, "", "", url);
               // BindingPlansGrid(OrganizationIDs, ProviderIDs, PlanIDs, url);
                //$("#ProviderNames").kendoAutoComplete({
                //    dataSource: PlansList, dataTextField: "ProviderName", dataValueField: "ProviderID", select: function (e) {
                //        var dataItem = this.dataItem(e.item.index());
                //        $("#ProviderName").val(dataItem.ProviderName);
                //        $("#ProviderID").val(dataItem.ProviderID);
                //        var OrganizationID = $("#OrganizationID").val();
                //        var ProviderID = $("#ProviderID").val();
                //        var PlanID = $("#PlanID").val();
                //      //  BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                //    }
                //});
                $("#PlanNames").kendoAutoComplete({
                    dataSource: PlansList, dataTextField: "PlanName", dataValueField: "PlanID", select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        $("#PlanName").val(dataItem.PlanName);
                        $("#PlanID").val(dataItem.PlanID);
                        var OrganizationID = $("#OrganizationID").val();
                        var ProviderID = $("#ProviderID").val();
                        var PlanID = $("#PlanID").val();
                       // BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                    }
                });
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList);
            }
            else if (OrganizationID != 0 && ProviderID == 0 && PlanID != 0) {
                var PlansList1 = [];
                PlansList1.push(PlansList[0]);
                $("#PlansGrid").data("kendoGrid").dataSource.data("");
                $("#divSelectPlan").show();
               $("#ProviderNames").data("kendoAutoComplete").destroy();
                $("#ProviderNames").kendoAutoComplete({
                    dataSource: PlansList1, dataTextField: "ProviderName", dataValueField: "ProviderID", select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        $("#ProviderName").val(dataItem.ProviderName);
                        $("#ProviderID").val(dataItem.ProviderID);
                        var OrganizationID = $("#OrganizationID").val();
                        var ProviderID = $("#ProviderID").val();
                        var PlanID = $("#PlanID").val();
                       // BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                    }
                });
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList1);
            }
            else if (OrganizationID != 0 && ProviderID != 0 && PlanID == 0) {
                $("#PlansGrid").data("kendoGrid").dataSource.data("");
                $("#divSelectPlan").show();
                $("#PlanNames").data("kendoAutoComplete").destroy();
                $("#PlanNames").kendoAutoComplete({
                    dataSource: PlansList, dataTextField: "PlanName", dataValueField: "PlanID", select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        $("#PlanName").val(dataItem.PlanName);
                        $("#PlanID").val(dataItem.PlanID);
                        var OrganizationID = $("#OrganizationID").val();
                        var ProviderID = $("#ProviderID").val();
                        var PlanID = $("#PlanID").val();
                  //      BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                    }
                });
                //Added by akhil to show only specific org doctors
                var OrganizationIDs = $("#OrganizationID").val();
                var ProviderIDs = $("#ProviderID").val();
                var PlanIDs = $("#PlanID").val();

                BindProviderNames(OrganizationIDs, url);
                BindPlanNames(OrganizationIDs, ProviderIDs, PlanIDs, "", "", url);
                //BindingPlansGrid(OrganizationIDs, ProviderIDs, PlanIDs, url);
                //End
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList);
            }
            else if (OrganizationID != 0 && ProviderID != 0 && PlanID != 0) {
                var PlansList1 = [];
                PlansList1.push(PlansList[0]);
                $("#PlansGrid").data("kendoGrid").dataSource.data("");
                $("#divSelectPlan").show();
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList1);
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}


function MembeDetails(MemberRegistrationDetails,url)
{
   
   
    MemberRegistrationDetails.OrganizationName=document.getElementById("SelectedOrganization").innerText;
    MemberRegistrationDetails.OrganizationID=document.getElementById("SpOrganizationID").innerText;
    MemberRegistrationDetails.ProviderName=document.getElementById("SpProviderName").innerText;
    MemberRegistrationDetails.ProviderID=document.getElementById("SpProviderID").innerText;
    MemberRegistrationDetails.PlanName=document.getElementById("SelectedPlan").innerText;
    MemberRegistrationDetails.PlanID=document.getElementById("PlanID").innerText;
    MemberRegistrationDetails.PlanStartDate=$("#PlanStartDate").val();
    MemberRegistrationDetails.Paymentschedule=document.getElementById("Paymentschedule").innerText;
    MemberRegistrationDetails.NoofInstallments=document.getElementById("Installments").innerText;
    MemberRegistrationDetails.InstallmentAmount=document.getElementById("InstallmentAmount").innerText;
    MemberRegistrationDetails.InstallmentFee=document.getElementById("InstallmentFee").innerText;
    MemberRegistrationDetails.Savings=document.getElementById("Savings").innerText;
    MemberRegistrationDetails.CardNumber=$("#CardNumber").val();
    MemberRegistrationDetails.NameOnCard=$("#NameOnCard").val();
    MemberRegistrationDetails.MM=$("#MM").val();
    MemberRegistrationDetails.YY=$("#YY").val();
    MemberRegistrationDetails.CVV=$("#CVV").val();
    var tAmount=document.getElementById("TotalAmount").innerText;
    MemberRegistrationDetails.TotalAmount=tAmount;
    var amountpaid=$("#AmountPaid").val();
    MemberRegistrationDetails.AmountPaid=$("#AmountPaid").val();
    if(amountpaid=="")
    {
        MemberRegistrationDetails.AmountPaid=0;
        MemberRegistrationDetails.DueAmount=tAmount;
        MemberRegistrationDetails.Status="Pending";
    }
    else{
        MemberRegistrationDetails.AmountPaid=$("#AmountPaid").val();
        var dueamount=tAmount-amountpaid;
        if(dueamount==0.0)
        {
            MemberRegistrationDetails.DueAmount=dueamount;
            MemberRegistrationDetails.Status="Paid";
        }
        else{
            MemberRegistrationDetails.DueAmount=dueamount;
            MemberRegistrationDetails.Status="Partially Paid";
        }
    }
    if (MemberRegistrationDetails != null) {
        EnrollPlan(MemberRegistrationDetails,url);
    }
    else {
        return false;
    }
}

function EnrollPlan(MemberRegistrationDetails, url) {
    debugger;
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Member/PlanEnrollxml',
        data: MemberRegistrationDetails,
        success: function (data, textStatus, jqXHR) {
            debugger;
            CallWebApiServices(data,url);
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
    var Url = url+"MemberXml";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainPlanEnroll"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            debugger;
            var obj = result[0];
            $("#divMainPlanEnroll").find(".loadingSpinner:first").remove();
            if (obj[0].result == null && obj[0].MemberID != null && obj[0].TransactionID != null) {
                if (obj[0].StripeCustomerID != null && obj[0].StripeCustomerID!="") {
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

function SaveStripeCustomerID(StripeCustomerID,TransactionID) {
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
//
function GetFamilyDetails(MemberParentID, Url) {
    debugger;
    var webMethodName = "GetFamilyDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strMemberParentID";
    ParameterValues[0] = MemberParentID;
    var Url = Url + "Member";
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
            
            $("#IndividualsMemberGrid").data("kendoGrid").dataSource.data(PlansList);
            $("#FamilyMemberGrid").data("kendoGrid").dataSource.data(PlansList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function BindSpecificOrganizations(OrganizationID, url) {
    debugger;
    //  alert("org");
    var webMethodName = "GetPPCPSpecificOrganization";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
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
            var OrganizationList = obj[0];
            $('#OrganizationNames').data('kendoAutoComplete').dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
function buttonchangesrarch(url) {
    debugger;
    $("#PlansGrid").data("kendoGrid").dataSource.data("");
    $("#divSelectPlan").hide();
    //Added by akhil
    $("#OrganizationNames").data("kendoAutoComplete").value("");
    $("#OrganizationName").val("");
    $("#OrganizationID").val("0");
    $("#PlanNames").data("kendoAutoComplete").value("");
    $("#PlanName").val("");
    $("#PlanID").val("0");
    $("#ProviderNames").data("kendoAutoComplete").value("");
    $("#ProviderName").val("");
    $("#ProviderID").val("0");
    //End
    //$("#OrganizationNames").data("kendoAutoComplete").destroy();
    //$("#PlanNames").data("kendoAutoComplete").destroy();
    //$("#ProviderNames").data("kendoAutoComplete").destroy();
    BindOrganizations(url);
    BindProviderNames(0, url);
    BindPlanNames(0, 0, 0, "", "", url);
}