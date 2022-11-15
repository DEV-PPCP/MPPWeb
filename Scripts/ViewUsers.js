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

//To get Age on DOB selection by Veena
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
        }

    });
}
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
            for (var r in OrganizationList) {
                $('#OrganizationName').data('kendoAutoComplete').dataSource.insert(r, { OrganizationName: OrganizationList[r].OrganizationName, OrganizationId: OrganizationList[r].OrganizationID });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//Bind the users grid to call the GetOrganizationUsers webservice-Veena
function BindOrganizationsUsersGrid(UserID, OrganizationID, Url) {
    var webMethodName = "GetOrganizationUsers";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "UserID";
    ParameterValues[0] = UserID;
    ParameterNames[1] = "OrganizationID";
    ParameterValues[1] = OrganizationID;
    //var Url = "http://192.168.1.20/ppcpwebservice/OrganizationServices";
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
        
            $("#UsersGrid").data("kendoGrid").dataSource.data(PlansList);
           
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
                $('#Salutation').data('kendoDropDownList').dataSource.insert(r, { Text: SalutationList[r].Text, Value: SalutationList[r].ID });
            }
            var SalutationName = SalutationID;
            if (SalutationName != "" && SalutationName != null) {
                var dropdownlist = $("#Salutation").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.Text === SalutationName;
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function AddUserDetails(jsModel, Url) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Organization/OrganizationRegistrationxml',
        data: jsModel,
        success: function (data, textStatus, jqXHR) {

            CallWebApiServiceAddUser(data, Url)
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

//Call webservice for Add Users-vinod
function CallWebApiServiceAddUser(data, Url) {
    var webMethodName = "UpdateUserDetails";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = Url + "Organization";
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
            if (obj[0].ResultID >= 0 && obj[0].ResultName == null) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Updated successfully.";
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
