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
//To get Doctor details by providerId=0 Added by ragini
function GetProviderDetails(OrganizationID, ProviderId, Url) {
    debugger;
    var webMethodName = "GetProviderDetails";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "strOrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "strProviderID";
    ParameterValues[1] = ProviderId;
    var Url = Url + "OrganizationServices";
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainRUpdate"));
    var jsonPostString = setJsonParameter(ParameterNames, ParameterValues, webMethodName);
    $.ajax({
      
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            debugger;
            $("#divMainRUpdate").find(".loadingSpinner:first").remove();
            var obj = jQuery.parseJSON(result);
            var PlansList = obj[0];
            if (PlansList.length != 0) {
                $("#ViewProviderGrid").data("kendoGrid").dataSource.data(PlansList);
                //document.getElementById("lblErrorMsgSearch").innerHTML = " ";
            }
            else {
                $("#ViewProviderGrid").data("kendoGrid").dataSource.data(PlansList);
               // document.getElementById("lblErrorMsgSearch").innerHTML = "No Records Found ";
            }
            },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function BindCountries(Url, CountryID) {
    var webMethodName = "GetCountries";
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
            var countriesList = obj[0];
            for (var r in countriesList) {
                $('#CountryName').data('kendoDropDownList').dataSource.insert(r, { CountryName: countriesList[r].CountryName, CountryId: countriesList[r].CountryID });
              
            }
            var OrgCountryID = CountryID;
            if (OrgCountryID != 0) {

                var dropdownlist = $("#CountryName").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.CountryId === parseInt(OrgCountryID);
                });
             }          
          },
    });
}
function BindStates(Url, CountryID, StateID) {
    debugger;
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
            debugger;
            var obj = jQuery.parseJSON(result);
            var StatesListList = obj[0];
            $('#StateName').data('kendoDropDownList').dataSource.data(StatesListList);
            var stateID = StateID;
            if (stateID != 0) {
                var dropdownlist = $("#StateName").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.StateID === parseInt(stateID);
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}
function BindCites(Url, StateId, CityID) {
    var webMethodName = "GetCities";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "StateID";
    ParameterValues[0] = StateId;
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
            $('#CityName').data('kendoDropDownList').dataSource.data(CitiesListList);
            var CityId = CityID;
            if (CityId != 0) {
                var dropdownlist = $("#CityName").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.CityID === parseInt(CityId);
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}
function BindSalutation(SalutationID) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Master/BindSalutation',
        //data: ,
        success: function (SalutationList, textStatus, jqXHR) {
            for (var r in SalutationList) {
                $('#SalutationList').data('kendoDropDownList').dataSource.insert(r, { Text: SalutationList[r].Text, Value: SalutationList[r].Value });
            }
            var SalutationName = SalutationID;
            if (SalutationName != "" && SalutationName != null) {
                var dropdownlist = $("#SalutationList").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.Text === SalutationName;
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//Getting and binding Specilizations in both Admin and Organization Module by Ragini on 31/10/2019
function GetSpecilization(Url, SpecializationTempID) {
    var webMethodName = "GetSpecializationLKP";
    var ParameterNames = "";
    var Url = Url + "OrganizationServices";
    var jsonPostString = setParameter(ParameterNames, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "text",
        contentType: "application/json",
        success: function (result) {
            var obj = jQuery.parseJSON(result);
            var objlist = obj[0];
            var jsonResult = "";
            var jsonResults = "";
            var Id = "";
           
            for (var r in objlist) {
                if (r % 2 == 0 || r == 0) {


                    if (SpecializationTempID.includes(objlist[r].SpecializationID)) {
                        jsonResult += "<input type='checkbox' checked='true'  id='" + objlist[r].SpecializationID + "' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ";" + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
                        "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
                      Id = objlist[r].SpecializationID;
                    }
                    else {
                        jsonResult += "<input type='checkbox'  id='" + objlist[r].SpecializationID + "' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ";" + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
                        "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
                        Id = objlist[r].SpecializationID;
                    }
                    $("#divSpecializationList").html(jsonResult);


                }
                else {
                    if (SpecializationTempID.includes(objlist[r].SpecializationID)) {
                        jsonResults += "<input type='checkbox' checked='true'  id='" + objlist[r].SpecializationID + "' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ";" + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
                        "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
                        Id = objlist[r].SpecializationID;
                    }
                    else {
                        jsonResults += "<input type='checkbox'  id='" + objlist[r].SpecializationID + "' onclick = '" + "SpecializationValidation();" + "' name='" + "SPList" + "' value='" + objlist[r].SpecializationID + ";" + objlist[r].SpecializationName + "' onclick='" + "check(this.value)" + "'  />" +
                        "<label for='" + objlist[r].SpecializationName + "'>" + objlist[r].SpecializationName + "</label>" + "<br />"
                       Id = objlist[r].SpecializationID;
                    }
                    $("#divSpecializationListRight").html(jsonResults);
                }

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function UpdateProviders(jsModel, Url) {
    debugger;
    jsModel.FirstName = $("#FirstName").val();
    jsModel.LastName = $("#LastName").val();
    jsModel.CountryCode = $("#CountryCode").val();
    jsModel.MobileNumber = $("#MobileNumber").val();
    jsModel.DOB = $("#DOB").val();
    jsModel.Gender = $("#hdnGender").val();
    jsModel.Email = $("#Email").val();
            
    jsModel.Zip = $("#Zip").val();
    jsModel.Salutation = $("#SalutationList").val();
    jsModel.CountryCode = $("#CountryCode").val();
            
    jsModel.CountryID = $("#CountryID").val();
    jsModel.CountryName = $("#CountryName").val();
    jsModel.StateID = $("#StateID").val();
    jsModel.StateName = $("#StateName").val();
    jsModel.CityID = $("#CityID").val();
    jsModel.CityName = $("#CityName").val();
    jsModel.NPI = $("#NPI").val();
  
    
    jsModel.Faz = $("#Fax").val();
    jsModel.Address = $("#Address").val();
    jsModel.Degree = $("#Degree").val();

    SaveOrganizationProviders(jsModel,Url);
}

function SaveOrganizationProviders(data, Url) {
    $.ajax({
        type:'POST',
        cache:false,
        url: "/Organization/GetOrganizationProviderXml",
        data: data,
        success:function(data,textStatus, jqXHR)
        {
            CallSaveOrganizationDetails(data,Url);
       },
        error:function(XMLHttpRequest, textStatus, errorThrown){
            alert(XMLHttpRequest, textStatus, errorThrown);
        },
    })
}
function CallSaveOrganizationDetails(data, Url) {
    debugger;
    var webMethodName = "UpdateOrganizationProviderDetails";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    $("<div class='loadingSpinner'></div>").appendTo($("#divMainRUpdate"));
    var Url = Url + "Organization";
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",

        success: function (result) {
            var obj = result[0];
            $("#divMainRUpdate").find(".loadingSpinner:first").remove();
            if (obj[0].ResultID == 1) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Details are Updated Successfully.";
                document.getElementById("divSignupPopup").scrollIntoView();
            }
            else if (obj[0].ResultID == 0) {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = "No changes in Existing Profile. ";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].ResultName + ". Please try again.";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//To bind Organizations AutoComplete in Admin Module By Ragini
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
            OrganizationList[0].FirstName = OrganizationList[0].FirstName + OrganizationList[0].LastName;
            $('#OrganizationName').data('kendoAutoComplete').dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
//To bind Provider NamesAutocomplete based on OrganizationID in Admin Module By Ragini
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
            if (ProvidersList.length != 0) {
                $('#ProviderName').data('kendoAutoComplete').dataSource.data(ProvidersList);
                
            }
            else {
                $('#ProviderName').data('kendoAutoComplete').dataSource.data();
               
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

