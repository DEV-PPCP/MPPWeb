

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

///Binding Values in View Plan grid in Admin module : Veena
function BindPlanDetails(url, type) {
    var webMethodName = "GetPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "Type";
    ParameterValues[0] = type;
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
               $("#ViewPlanDetailsGrid").data("kendoGrid").dataSource.data(PlansList);

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });

    }

  
function GetWithdrawPlans(Url) {
        $.ajax({
            type: 'POST',
            cache: false,
            url: '/Admin/GetWithdrawPlanList',
            success: function (data, textStatus, jqXHR) {
                  $("#spnerrorMessage").hide();
                    CallWithdrawPlans(Url, data);
              },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest, textStatus, errorThrown);
            },
        });
    }

function CallWithdrawPlans(Url,data)
{
    var webMethodName = "WithdrawPlans";
    var ParameterNames = data;
    var Url = Url + "DefaultXml";
    var jsonPostString = setParameter(ParameterNames, webMethodName);
    $.ajax({
        type: "POST",
        url: Url,
        data: jsonPostString,
        dataType: "json",
        contentType: "application/json",
      
        success: function (result) {
            var obj = result[0];
            var objlist = obj[0];
            if (objlist.ResultID >= 1) {
                document.getElementById("divSignupPopup").style.display = "block";
                document.getElementById("spnPopupMessage").innerHTML = "Plan withdrawn successfully.";
                document.getElementById("divSignupPopup").scrollIntoView();

            }
          else if (objlist.ResultName != null) {

                $("#divError").show();
                $("#lblErrorUserMsg").show();
                document.getElementById("lblErrorUserMsg").innerHTML = objlist[0].ResultName;
            }
            else {
                document.getElementById("divErrMessagePopup").style.display = "block";
                document.getElementById("spnPopupErrMessage").innerHTML = " Please Select Valid Checkboxes";
                document.getElementById("divErrMessagePopup").scrollIntoView();
            }
        },

       error: function (XMLHttpRequest, textStatus, errorThrown) {
        alert(XMLHttpRequest, textStatus, errorThrown);
    },
    });
  

}
//UnsubscribeWithdrawPlans by veena
function WithdrawPlans(PlanId, UnsubscribeDate, Url) {
    var webMethodName = "WithdrawPlans";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
   
    ParameterNames[0] = "PlanID";
    ParameterValues[0] = PlanId;
    ParameterNames[1] = "UnsubscribeDate";
    ParameterValues[1] = UnsubscribeDate;
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
            var plans = obj[0];
            if (plans[0].ResultID >= 1)
            {                
                document.getElementById("divSuccess").style.display = "block";
                document.getElementById("lblMsg").innerHTML = "You are registered successfully.";
                document.getElementById("divSuccess").scrollIntoView();
              
            }
            else if (plans[0].ResultName != null) {
                document.getElementById("divSuccess").style.display = "block";
                document.getElementById("lblErrorMsg").innerHTML = plans[0].ResultName;
            }
            else  {
                document.getElementById("divSuccess").style.display = "block";
                document.getElementById("lblErrorMsg").innerHTML = "Please Enter Valid Details.";
                document.getElementById("divSuccess").scrollIntoView();
            }
          },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });

}