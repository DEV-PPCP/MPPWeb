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
function BindCountries(Url, OrgCountryTempID, CountryTempID) {
   
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
                $('#OrgCountryName').data('kendoDropDownList').dataSource.insert(r, { CountryName: countriesList[r].CountryName, CountryId: countriesList[r].CountryID });
            }
            var OrgCountryID = OrgCountryTempID;
            if (OrgCountryID != 0) {

                var dropdownlist = $("#OrgCountryName").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.CountryId === parseInt(OrgCountryID);
                });
                
            }
            var CountryTemp = CountryTempID;
            if (CountryTemp != 0) {

                var dropdownlist = $("#CountryName").data("kendoDropDownList");

                dropdownlist.select(function (dataItem) {

                    return dataItem.CountryId === parseInt(CountryTemp);
                });

            }

        },
    });
}
//Bind the State details and bind the exsisting selected state-vinod
function OrgBindStates(Url, countryID, StateTempID) {

    var webMethodName = "GetStates";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "CountryID";
    ParameterValues[0] = countryID;
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
            $('#OrgStateName').data('kendoDropDownList').dataSource.data(StatesListList);
            var stateID = StateTempID;
            if (stateID != 0) {
                var dropdownlist = $("#OrgStateName").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.StateID === parseInt(stateID);
                });            
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}

function BindStates(Url, countryID, StateTempID) {

    var webMethodName = "GetStates";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "CountryID";
    ParameterValues[0] = countryID;
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
            var stateID = StateTempID;
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

//Bind the City details and bind the exsisting selected City-vinod
function OrgBindCites(Url, StateId, CityTempID) {


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
            $('#OrgCityName').data('kendoDropDownList').dataSource.data(CitiesListList);
            var CityID = CityTempID;
            if (CityID != 0) {
                var dropdownlist = $("#OrgCityName").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.CityID === parseInt(CityID);
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}
function BindCites(Url, StateId, CityTempID) {
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
            var CityID = CityTempID;
            if (CityID != 0) {
                var dropdownlist = $("#CityName").data("kendoDropDownList");
                dropdownlist.select(function (dataItem) {
                    return dataItem.CityID === parseInt(CityID);
                });
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        },
    });
}
function BindSalutation(Salutation) {
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Master/BindSalutation',
        //data: ,
        success: function (SalutationList, textStatus, jqXHR) {
            for (var r in SalutationList) {
                $('#SalutationList').data('kendoDropDownList').dataSource.insert(r, { Text: SalutationList[r].Text, Value: SalutationList[r].ID });
            }
            var SalutationName = Salutation;
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

function UpdateOrganizationProfile(jsModel, Url) {
    $.ajax({

        type: 'POST',
        cache: false,
        url: '/Organization/OrganizationRegistrationxml',
        data: jsModel,
        success: function (data, textStatus, jqXHR) {

            CallWebApiService(data, Url)
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function CallWebApiService(data, Url) {
    var webMethodName = "UpdateOrganizationDetails";
    var ParameterName = data;
    var jsonPostString = setParameter(ParameterName, webMethodName);
    var Url = Url + "Organization";
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

            if (obj[0].UserID != null) {
                document.getElementById("divPaymentPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Your Profile Updated Successfully";
                document.getElementById("divPaymentPopup").scrollIntoView();
            }
            else {
                document.getElementById("divPaymentPopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = obj[0].result + ". Please try again.";
               
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
