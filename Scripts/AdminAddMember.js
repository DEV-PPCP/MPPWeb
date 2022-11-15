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
//To get Age on DOB selection by Gayathri
function DateSelectionChanged(e) {
    var birthDate = $("#DOB").val();
    $.ajax({
        url: "/Master/GetAge/",
        data: { birthDate: birthDate },
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {
            var rs = result.split(';');
            var year = rs[0];
            $("#Age").val(year);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function GetIpAddress() {
    $.ajax({
        url: "/Master/GetIPAddress/",
        data: {},
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {


        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//To bind Countries dropdownlist by Gayathri
function BindCountries(ServiceUrl) {
    var webMethodName = "GetCountries";
    var ParameterNames = "";

    var Url = ServiceUrl + "DefaultService";
    var jsonPostString = setParameter(ParameterNames, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var countriesList = obj[0];
            for (var r in countriesList) {
                $('#CountryNameList').data('kendoDropDownList').dataSource.insert(r, { Text: countriesList[r].CountryName, Value: countriesList[r].CountryID });
            }
            var countryID = 1;

            if (countryID != 0) {

                var dropdownlist = $("#CountryNameList").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {
                    $('#CountryNameList').data('kendoDropDownList').value("1");
                    return dataItem.Value === countryID;
                });
                //  BindStates(countryID, ServiceUrl)
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
        },
    });
}
//To Bind States dropdownlist based on CountryID by Gayathri
function BindStates(CountryID, ServiceUrl) {
    // $('#StateNameList').data('kendoDropDownList').value("");
    var webMethodName = "GetStates";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "CountryID";
    ParameterValues[0] = CountryID;
    var Url = ServiceUrl + "DefaultService";
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
            $('#StateNameList').data('kendoDropDownList').dataSource.data(StatesListList);
            var stateID = 1;

            if (stateID != 0) {

                var dropdownlist = $("#StateNameList").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {
                    $('#StateNameList').data('kendoDropDownList').value("0");
                    return dataItem.Value === stateID;
                });
                // BindCities(stateID, ServiceUrl)
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
//To bind Cities dropdownlist based on StateID by Gayathri
function BindCities(StateID, ServiceUrl, tempvalue) {
    if (tempvalue == "2") {
        $('#CityNameList').data('kendoDropDownList').value("");
    }
    var webMethodName = "GetCities";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "StateID";
    ParameterValues[0] = StateID;
    var Url = ServiceUrl + "DefaultService";
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
            $('#CityNameList').data('kendoDropDownList').dataSource.data(CitiesListList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
//To validate UserName already Exits or Not by Gayathri
function ValidateUserNames(UserName, url) {
    var webMethodName = "ValidateUserName";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "UserName";
    ParameterValues[0] = UserName;
    var Url = url + "DefaultService";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            var obj = result[0];
            if (obj[0].ResultID == 0) {
                document.getElementById("spnUserName").innerHTML = "";
            }
            else {
                document.getElementById("spnUserName").innerHTML = "Username already exists"; return false;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
//To bind Organizations dropdownlist by Gayathri
function BindOrganizations(url) {
    var webMethodName = "GetPPCPOrganizations";
    var ParameterNames = "";
    var Url = url + "DefaultService";
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
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
//To bind Provider Names dropdown based on OrganizationID by Gayathri
function BindProviderNames(OrganizationID, url) {
    //if (OrganizationID != 0) {
    //    $("#ProviderNames").data("kendoAutoComplete").destroy();
    //}
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
            var obj = jQuery.parseJSON(result);
            var ProvidersList = obj[0];
            $('#ProviderNames').data('kendoAutoComplete').dataSource.data(ProvidersList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
//To Bind plans dropdownlist based on OrganizationID,PrividerID and PlanID by Gayathri
function BindPlans(OrganizationID, ProviderID, PlanID, MemberGender, MemberAge, url) {
    debugger;
    if (OrganizationID != 0 && ProviderID != 0 && PlanID == 0) {
        $("#ProviderNames").data("kendoAutoComplete").destroy();
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
            ErrorMessage(webMethodName, textStatus);
        },
    });
}



//To Bind plans grid for Plan Details by Gayathri
function BindPlansGrid(PlanID, MemberID, OrganizationID, ProviderID, url) {
    var webMethodName = "GetPlanDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "PlanID";
    ParameterValues[0] = PlanID;
    ParameterNames[1] = "MemberID";
    ParameterValues[1] = MemberID;
    ParameterNames[2] = "OrganizationID";
    ParameterValues[2] = OrganizationID;
    ParameterNames[3] = "ProviderID";
    ParameterValues[3] = ProviderID;
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
            $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
//To Bind plans auntocomplete based on OrganizationID,PrividerID and PlanID in PlanDetails by Gayathri
function BindPlanNames(OrganizationID, ProviderID, PlanID, MemberAge, MemberGender, url) {
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
            $('#PlanNames').data('kendoAutoComplete').dataSource.data(PlansList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}

function BindRelationShip(url) {
    var webMethodName = "GetRelationShipDetails";
    var ParameterNames = "";
    var Url = url + "DefaultService";
    var jsonPostString = setParameter(ParameterNames, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var RelationshipList = obj[0];
            for (var r in RelationshipList) {
                $('#RelationshipList').data('kendoDropDownList').dataSource.insert(r, { Text: RelationshipList[r].RelationshipName, Value: RelationshipList[r].RelationshipID });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            res = '';
            ErrorMessage(webMethodName, textStatus);
            //callback(res);

        },
    });
}
//To generate Payment xml in PlanDetails by Gayathri
function MakePlanPayment(MemberRegistrationDetails, url) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Member/MakePaymentxml',
        data: MemberRegistrationDetails,
        success: function (data, textStatus, jqXHR) {

            CallWebApiServiceforPayment(data, url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
//To make Payment in PlanDetails by Gayathri
function CallWebApiServiceforPayment(data, url) {
    var webMethodName = "UpdateMemberPlanPayments";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = url + "MemberXml";
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            var obj = result[0];
            if (obj[0].TransactionID != null) {
                $("#divMakePayment").hide();
                document.getElementById("divPaymentPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Payment is successful. Your Transaction ID: " + obj[0].TransactionID;
                document.getElementById("divPaymentPopup").scrollIntoView();
            }
            else {
                $("#divMakePayment").hide();
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].result + ". Please try again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
///Binding values of Organization, Provider, Plan through autocomplete in MemberRegistration, AvailablePlane, PlanDetails  : Ragini on 14-08-2019
function BindingPlansGrids(OrganizationID, ProviderID, PlanID, MemberGender, MemberAge, url) {
    if (MemberGender == undefined) {
        MemberGender = "";
    }
    if (MemberAge == undefined) {
        MemberAge = "";
    }

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
                        BindingPlansGrid(OrganizationID, ProviderID, PlanID);
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
                        BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                    }
                });
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList1);
            }
            else if (OrganizationID == 0 && ProviderID != 0 && PlanID == 0) {
                $("#PlansGrid").data("kendoGrid").dataSource.data("");
                $("#divSelectPlan").show();
                //  $("#OrganizationNames").data("kendoAutoComplete").destroy();
                $("#PlanNames").data("kendoAutoComplete").destroy();
                //$("#OrganizationNames").kendoAutoComplete({
                //    dataSource: PlansList, dataTextField: "OrganizationName", dataValueField: "OrganizationID", select: function (e) {
                //        var dataItem = this.dataItem(e.item.index());
                //        $("#OrganizationName").val(dataItem.OrganizationName);
                //        $("#OrganizationID").val(dataItem.OrganizationID);
                //        var OrganizationID = $("#OrganizationID").val();
                //        var ProviderID = $("#ProviderID").val();
                //        var PlanID = $("#PlanID").val();
                //        BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                //    }
                //});
                $("#PlanNames").kendoAutoComplete({
                    dataSource: PlansList, dataTextField: "PlanName", dataValueField: "PlanID", select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        $("#PlanName").val(dataItem.PlanName);
                        $("#PlanID").val(dataItem.PlanID);
                        $("#OrganizationID").val(dataItem.OrganizationID);
                        $("#OrganizationName").val(dataItem.OrganizationName);
                        var OrganizationID = $("#OrganizationID").val();
                        var ProviderID = $("#ProviderID").val();
                        var PlanID = $("#PlanID").val();
                        BindingPlansGrid(OrganizationID, ProviderID, PlanID);


                    }
                });

                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList);
                //alert(PlansList[0].OrganizationID +" Anusha");
                //  $("#OrganizationNames").data("kendoAutoComplete").destroy();
                BindSpecificOrganizations(PlansList[0].OrganizationID, url);
            }
            else if (OrganizationID == 0 && ProviderID != 0 && PlanID != 0) {
                var PlansList1 = [];
                PlansList1.push(PlansList[0]);
                $("#PlansGrid").data("kendoGrid").dataSource.data();
                $("#divSelectPlan").show();


                $("#OrganizationNames").kendoAutoComplete({
                    dataSource: PlansList1, dataTextField: "OrganizationName", dataValueField: "OrganizationID", select: function (e) {
                        var dataItem = this.dataItem(e.item.index());
                        $("#OrganizationName").val(dataItem.OrganizationName);
                        $("#OrganizationID").val(dataItem.OrganizationID);
                        var OrganizationID = $("#OrganizationID").val();
                        var ProviderID = $("#ProviderID").val();
                        var PlanID = $("#PlanID").val();
                        BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                    }
                });
                //$("#OrganizationNames").data("kendoAutoComplete").destroy();
                //alert("BindSpecificOrganizations");
                //BindSpecificOrganizations($("#OrganizationID").val(),url);
                $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList1);
            }
            else if (OrganizationID != 0 && ProviderID == 0 && PlanID == 0) {
                $("#PlansGrid").data("kendoGrid").dataSource.data("");
                $("#divSelectPlan").show();
                //$("#ProviderNames").data("kendoAutoComplete").destroy();
                $("#PlanNames").data("kendoAutoComplete").destroy();

                var OrganizationIDs = $("#OrganizationID").val();
                var ProviderIDs = $("#ProviderID").val();
                var PlanIDs = $("#PlanID").val();

                BindProviderNames(OrganizationIDs, url);
                BindPlanNames(OrganizationIDs, ProviderIDs, PlanIDs, "", "", url);
                //  BindingPlansGrid(OrganizationIDs, ProviderIDs, PlanIDs);//, url

                //$("#ProviderNames").kendoAutoComplete({
                //    dataSource: PlansList, dataTextField: "ProviderName", dataValueField: "ProviderID", select: function (e) {
                //        var dataItem = this.dataItem(e.item.index());
                //        $("#ProviderName").val(dataItem.ProviderName);
                //        $("#ProviderID").val(dataItem.ProviderID);
                //        var OrganizationID = $("#OrganizationID").val();
                //        var ProviderID = $("#ProviderID").val();
                //        var PlanID = $("#PlanID").val();
                //        BindingPlansGrid(OrganizationID, ProviderID, PlanID);
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
                        BindingPlansGrid(OrganizationID, ProviderID, PlanID);
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
                        BindingPlansGrid(OrganizationID, ProviderID, PlanID);
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
                        BindingPlansGrid(OrganizationID, ProviderID, PlanID);
                    }
                });
                //Added by akhil to show only specific org doctors
                var OrganizationIDs = $("#OrganizationID").val();
                var ProviderIDs = $("#ProviderID").val();
                var PlanIDs = $("#PlanID").val();

                BindProviderNames(OrganizationIDs, url);
                BindPlanNames(OrganizationIDs, ProviderIDs, PlanIDs, "", "", url);
                // BindingPlansGrid(OrganizationIDs, ProviderIDs, PlanIDs);//, url
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
            ErrorMessage(webMethodName, textStatus);
        },
    });
}

//Binding values of  grid PlanName, MemberName, OrganizationName, ProviderName in AvailablePlans : Ragini on 14-08-2019
function BindPlansGrids(PlanID, MemberID, OrganizationID, ProviderID, url) {
    var webMethodName = "GetPlanDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "PlanID";
    ParameterValues[0] = PlanID;
    ParameterNames[1] = "MemberID";
    ParameterValues[1] = MemberID;
    ParameterNames[2] = "OrganizationID";
    ParameterValues[2] = OrganizationID;
    ParameterNames[3] = "ProviderID";
    ParameterValues[3] = ProviderID;
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
            $("#PlansGrid").data("kendoGrid").dataSource.data(PlansList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
function GetRelationShip(url) {
    var webMethodName = "GetRelationShipDetails";
    var ParameterNames = "";
    var Url = url + "DefaultService";
    var jsonPostString = setParameter(ParameterNames, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var RelationshipList = obj[0];
            for (var r in RelationshipList) {
                $('#RelationshipList').data('kendoDropDownList').dataSource.insert(r, { Text: RelationshipList[r].RelationshipName, Value: RelationshipList[r].RelationshipID });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            res = '';
            //callback(res);
            ErrorMessage(webMethodName, textStatus);
        },
    });
}


function MembeDetails(MemberRegistrationDetails, session, Url, CurrentMonth) {
    debugger;
    MemberRegistrationDetails.FirstName = $("#FirstName").val();
    MemberRegistrationDetails.LastName = $("#LastName").val();
    MemberRegistrationDetails.DOB = $("#DOB").val();
    MemberRegistrationDetails.Age = $("#Age").val();
    var rbtnMale = $('input[id=rbtnGenderMale]:checked').val();
    if (rbtnMale == 1)
        MemberRegistrationDetails.Gender = rbtnMale;
    var rbtnFEM = $('input[id=rbtnGenderFemale]:checked').val();
    if (rbtnFEM == 2)
        MemberRegistrationDetails.Gender = rbtnFEM;
    MemberRegistrationDetails.SalutationID = $("#SalutationID").val();
    MemberRegistrationDetails.Salutation = $("#Salutation").val();
    MemberRegistrationDetails.Email = $("#Email").val();
    MemberRegistrationDetails.CountryCode = $("#CountryCode").val();
    MemberRegistrationDetails.MobileNumber = $("#MobileNumber").val();
    MemberRegistrationDetails.CountryID = $("#CountryID").val();
    MemberRegistrationDetails.CountryName = $("#CountryName").val();
    MemberRegistrationDetails.StateID = $("#StateID").val();
    MemberRegistrationDetails.StateName = $("#StateName").val();
    MemberRegistrationDetails.CityID = $("#CityID").val();
    MemberRegistrationDetails.CityName = $("#CityName").val();
    MemberRegistrationDetails.RelationshipID = "0";
    MemberRegistrationDetails.RelationshipName = "Self";
    if ($("#ZipCode").val() == "") {
        MemberRegistrationDetails.Zip = $("#Zip").val();
    }
    else {
        MemberRegistrationDetails.Zip = $("#Zip").val() + "-" + $("#ZipCode").val();
    }
    MemberRegistrationDetails.UserName = $("#UserName").val();
    MemberRegistrationDetails.Password = $("#Password").val();
    MemberRegistrationDetails.ConfirmPassword = $("#ConfirmPassword").val();
    MemberRegistrationDetails.OrganizationName = document.getElementById("spnSelectedOrganization").innerText;
    MemberRegistrationDetails.OrganizationID = document.getElementById("SpOrganizationID").innerText;
    MemberRegistrationDetails.ProviderName = document.getElementById("SpProviderName").innerText;
    MemberRegistrationDetails.ProviderID = document.getElementById("SpProviderID").innerText;
    MemberRegistrationDetails.PlanName = document.getElementById("spnSelectedPlan").innerText;
    MemberRegistrationDetails.PlanID = document.getElementById("PlanID").innerText;
    MemberRegistrationDetails.PlanStartDate = $("#PlanStartDate").val();
    MemberRegistrationDetails.PlanEndDate = $("#spnPlanEndDate").val();
    MemberRegistrationDetails.Paymentschedule = document.getElementById("Paymentschedule").innerText;
    MemberRegistrationDetails.NoofInstallments = document.getElementById("spnNoofInstalments").innerText;
    MemberRegistrationDetails.InstallmentAmount = document.getElementById("spnInstallmentAmount").innerText;
    MemberRegistrationDetails.InstallmentFee = document.getElementById("InstallmentFee").innerText;
    MemberRegistrationDetails.StripeAccountID = document.getElementById("spnStripeAccountID").innerText;
    MemberRegistrationDetails.Savings = document.getElementById("Savings").innerText;
    MemberRegistrationDetails.CardNumber = $("#CardNumber").val();
    MemberRegistrationDetails.NameOnCard = $("#NameOnCard").val();
    MemberRegistrationDetails.Amount = document.getElementById("spnInstallmentAmount").innerText;
    MemberRegistrationDetails.MM = $("#MM").val();
    MemberRegistrationDetails.YY = $("#YY").val();
    MemberRegistrationDetails.CVV = $("#CVV").val();
    MemberRegistrationDetails.CommPPCP = document.getElementById("spnCommPPCP").innerText;
    MemberRegistrationDetails.CommPrimaryMember = document.getElementById("spnCommPrimaryMember").innerText;
    MemberRegistrationDetails.Duration = document.getElementById("spnDuration").innerText;
    MemberRegistrationDetails.EnrollFee = document.getElementById("spnEnrollFee").innerText;
    //  var tAmount = document.getElementById("spnPlanAmount").innerText;
    var tAmount = parseInt(document.getElementById("spnPlanAmount").innerText) + parseInt(MemberRegistrationDetails.InstallmentFee) + parseInt(MemberRegistrationDetails.EnrollFee);
    MemberRegistrationDetails.TotalAmount = tAmount;
    var amountpaid = $("#AmountPaid").val();
    MemberRegistrationDetails.AmountPaid = $("#AmountPaid").val();
    if (amountpaid == "") {
        MemberRegistrationDetails.AmountPaid = 0;
        MemberRegistrationDetails.DueAmount = tAmount;
        MemberRegistrationDetails.Status = "Pending";
    }
    else {
        MemberRegistrationDetails.AmountPaid = $("#AmountPaid").val();
        var dueamount = tAmount - amountpaid;
        alert(dueamount);
        if (dueamount == 0.0) {
            MemberRegistrationDetails.DueAmount = dueamount;
            MemberRegistrationDetails.Status = "Paid";
        }
        else {
            MemberRegistrationDetails.DueAmount = dueamount;
            MemberRegistrationDetails.Status = "Partially Paid";
        }
    }
    if ($('#chk2factor').is(":checked") == true) {
        MemberRegistrationDetails.IsTwofactorAuthentication = true;
        MemberRegistrationDetails.PreferredIP = session;
        var rbtnEvery = $('input[id=rbtnEverytime]:checked').val();
        if (rbtnEvery == 1)
            MemberRegistrationDetails.TwoFactorType = rbtnEvery;
        var rbtnSystem = $('input[id=rbtnOnlySysChange]:checked').val();
        if (rbtnSystem == 2)
            MemberRegistrationDetails.TwoFactorType = rbtnSystem;
    }
    else {
        MemberRegistrationDetails.IsTwofactorAuthentication = false;
        MemberRegistrationDetails.TwoFactorType = "0";
        MemberRegistrationDetails.PreferredIP = "";
    }
    if (MemberRegistrationDetails != null) {
        MemberRegistration(MemberRegistrationDetails, Url);
    }
    else {
        return false;
    }
}

//To generate Member Registration xml by Gayathri
function MemberRegistration(MemberRegistrationDetails, Url) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Master/MemberRegistrationxml',
        data: MemberRegistrationDetails,
        success: function (data, textStatus, jqXHR) {

            CallWebApiService(data, Url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(MemberRegistration, textStatus);
        },
    });
}


function CallWebApiService(data, url) {
    debugger;
    var webMethodName = "SaveMemberSignUP";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = url + "MemberXml";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainRegistration"));
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
        success: function (result) {
            var obj = result[0];
            $("#divMainRegistration").find(".loadingSpinner:first").remove();
            if (obj[0].result == null && obj[0].MemberID != null && obj[0].TransactionID != null && obj[0].TransactionID != "") {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "You are registered successfully. Your Transaction ID: " + obj[0].TransactionID;
                document.getElementById("divSignupPopup").scrollIntoView();
            }
            else if (obj[0].result == null && obj[0].MemberID != null) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "You are registered successfully.";
                document.getElementById("divSignupPopup").scrollIntoView();
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].result + ". Please try again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}

/// ChnageSearchPlans in MemberRegistration : Ragini on 14-08-2019 ///
function ChangeSearchPlans() {
    $("#PlansGrid").data("kendoGrid").dataSource.data("");
    $("#divSelectPlan").hide();
    $("#OrganizationNames").data("kendoAutoComplete").destroy();
    $("#PlanNames").data("kendoAutoComplete").destroy();
    $("#ProviderNames").data("kendoAutoComplete").destroy();
    BindOrganizations();
    BindProviderNames(0);
    BindPlanNames(0, 0, 0);
}

/// PasswordValidation in MemberRegistration : Ragini on 14-08-2019 ///
function validatePasswords() {
    //To Compare Password and ConfirmPassword (in Step1)
    if (($("#Password").val()) == ($("#ConfirmPassword").val())) {
        document.getElementById("PasswordErrorMsg").innerHTML = "";
    }
    else {
        document.getElementById("PasswordErrorMsg").innerHTML = "Password and Confirm Password should be Match.";
    }
}

///To check Password strength in MemberRegistration  : Ragini on 14-08-2019 
function chkPasswordStrengths(txtPassword, strenghtMsg, errorMsg) {
    var desc = new Array();
    desc[0] = "Very Weak";
    desc[1] = "Weak";
    desc[2] = "Average";
    desc[3] = "Strong";
    desc[4] = "Strong";
    desc[5] = "Strong";

    var score = 0;

    //if txtPassword bigger than 6 give 1 point
    if (txtPassword.length > 6) score++;

    //if txtPassword has both lower and uppercase characters give 1 point
    if ((txtPassword.match(/[a-z]/)) && (txtPassword.match(/[A-Z]/))) score++;

    //if txtPassword has at least one number give 1 point
    if (txtPassword.match(/\d+/)) score++;

    //if txtPassword has at least one special caracther give 1 point
    if (txtPassword.match(/.[!,#,$,%,^,&,*,?,_,~,-,(,)]/)) score++;

    //if txtPassword bigger than 12 give another 1 point
    if (txtPassword.length > 12) score++;

    strenghtMsg.innerHTML = desc[score];

    strenghtMsg.className = "strength" + score;
}

///AmountPaid Validations in MemberRegistration  : Ragini on 14-08-2019 
function ValidateAmountPaids(AmountPaid) {
    if (AmountPaid == "") {
        document.getElementById("spnAmountPaid").innerHTML = "Please enter amount"; return false;
    }
    else {
        var totalamount = document.getElementById("TotalAmount").innerText;
        if (totalamount == AmountPaid || totalamount > AmountPaid) {
            document.getElementById("spnAmountPaid").innerHTML = "";
        }
        else {
            document.getElementById("spnAmountPaid").innerHTML = "Please check your amount"; return false;
        }
    }
}
//PlanDetails   :Ragini
function ViewPaymentReports(MemberID, PlanID, OrganizationID, url) {

    alert("1");
    var ProviderID = 0;
    var webMethodName = "GetPlanDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "PlanID";
    ParameterValues[0] = PlanID;
    ParameterNames[1] = "MemberID";
    ParameterValues[1] = MemberID;
    ParameterNames[2] = "OrganizationID";
    ParameterValues[2] = OrganizationID;
    ParameterNames[3] = "ProviderID";
    ParameterValues[3] = ProviderID;
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
            bindReport(obj[0]);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}

//CheckMemberExists by vinod
function CheckMemberExists(FirstName, LastName, Gender, DOB, MobileNumber, Age, url) {
    debugger;
    var tempResult = 0;
    var webMethodName = "CheckMemberExists";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "FirstName";
    ParameterValues[0] = FirstName;
    ParameterNames[1] = "LastName";
    ParameterValues[1] = LastName;
    ParameterNames[2] = "Gender";
    ParameterValues[2] = Gender;
    ParameterNames[3] = "DOB";
    ParameterValues[3] = DOB;
    ParameterNames[4] = "MobileNumber";
    ParameterValues[4] = MobileNumber;
    var Url = url + "Member";
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var val = obj[0];
            if (val > 0) {//val == 1
                document.getElementById("spnExistingMemberValidator").innerHTML = "Member Already Exists";
            }
            else {
                $("#MemberDetails").hide();
                $("#divStep1").hide();
                $("#SearchPlan").show();
                $("#EnrollPlan").hide();
                $("#Step1").removeClass("CurrentPage");
                $("#Step1").addClass("Clicked");
                $("#Step2").addClass("CurrentPage");
                $("#Step3").addClass("Initial");
                BindOrganizations(url);
                BindProviderNames(0, url);
                BindPlanNames(0, 0, 0, Age, Gender, url);
                document.getElementById("spnExistingMemberValidator").innerHTML = "";
            }

        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ErrorMessage(webMethodName, textStatus);
        },
    });
}
function ErrorMessage(webMethodName, textStatus) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Master/ErrorMessage',
        data: { webMethodName: webMethodName, textStatus: textStatus },
        success: function (data, textStatus, jqXHR) {

            // CallWebApiService(data, Url);
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
