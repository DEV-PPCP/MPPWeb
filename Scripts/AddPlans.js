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

//AddPlans binding TenureList in Admin Module :Ragini
function BindTenure() {
   
    debugger;
    try {
       
        $.ajax({
            type: "POST",
            cache: false,
            url: "/Master/BindTenures",
            success: function (TenureList, textStatus, jqXHR) {
                for (var r in TenureList)
                $('#TenureList').data('kendoDropDownList').dataSource.insert(r, { Text: TenureList[r].Text, Value: TenureList[r].Value });
            },

            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest, textStatus, errorThrown)
            },

        });
    }
    catch(e)
    {
        debugger;
        alert(e.stack);
    }
}
//AddPlans binding PlanType in AdminModule :Ragini
function BindPlanType() {
  $.ajax({
            type: "POST",
            cache: false,
            url: "/Master/BindPlanTypes",
            success: function (PlanTypeList, textStatus, jqXHR) {
                for (var r in PlanTypeList)
                $('#PlanTypeList').data('kendoDropDownList').dataSource.insert(r, { Text: PlanTypeList[r].Text, Value: PlanTypeList[r].Value });
           },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest, textStatus, errorThrown)
            },
  });
}
function BindMemberPlanType() {
    debugger;
    try{
        $.ajax({
            type: "POST",
            cache: false,
            url: "/Master/BindMemberPlanTypes",
            success: function (MemberPlanTypeList, textStatus, jqXHR) {
                for (var r in MemberPlanTypeList)
                    $('#MemberPlanTypeList').data('kendoDropDownList').dataSource.insert(r, { Text: MemberPlanTypeList[r].Text, Value: MemberPlanTypeList[r].Value });
           
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest, textStatus, errorThrown)
            },
        });}
    catch(e){

    }
}
//AddPlans PaymentSchedule details  :Ragini  11/09/2019
function GetPaymentSchedule(Tenure_Id) {
  
    var objText=[];
    var objValue=[];
    var jsonResult = "";
    
    switch(Tenure_Id)
    {
        case "1":
            objText[0]="One Time";
            objValue[0]="1";
            break;
        case "3":
            objText[0]="One Time";
            objValue[0]="1";
           // objText[1]="Monthly";
          //  objValue[1]="3";
            break;
        case "6":
            objText[0]="One Time";
            objValue[0]="1";
            objText[1]="Monthly";
            objValue[1]="6";
            objText[2]="Quarterly";
            objValue[2]="2";
            break;
      
        case "12":
            objText[0]="One Time";
            objValue[0]="1";
            objText[1]="Monthly";
            objValue[1]="12";
            objText[2]="Quarterly";
            objValue[2]="4";
            objText[3]="Half Yearly";
            objValue[3]="2";
            break;

    }
    
    for (var r in objText && objValue) {
        jsonResult += "<input type='checkbox' onclick = '" + "TenureValidation();" + "' name='" + "TypeList" + "' value='" + objText[r] + ";" + objValue[r] + "' onclick='" + "check(this.text)" + "' />" +
                        "<label for='" + objText[r] + "'>" + objText[r] + "</label>" + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
        $("#divPaymentSchedule").html(jsonResult);
    }
}


//AddPlans saving details in Admin Module  :Ragini  11/09/2019
function SaveAddPlans(jsModel, url) {
   
    $.ajax({
        type: 'POST',
        cache: false,
        url: '/Admin/SavePlanDetailsxml',
        data: jsModel,
        success: function (data, textStatus, jqXHR) {
            
            CallSaveAddPlans(data, url);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(XMLHttpRequest, textStatus, errorThrown);
        },
    });
}
function CallSaveAddPlans(data, url) {
   
        var webMethodName = "AddPlans";
        var ParameterNames = data;
        var Url = url + "DefaultXml";
        var jsonPostString = setParameter(ParameterNames, webMethodName);
        $("<div class='loadingSpinner'></div>").appendTo($("#divMainAddPlans"));
        $.ajax({

            type: "POST",
            url: Url,
            data: jsonPostString,
            dataType: "text",
            contentType: "application/json",
            success: function (result) {
          
                $("#divMainAddPlans").find(".loadingSpinner:first").remove();
                debugger;
                var obj = jQuery.parseJSON(result);
                var plans = obj[0];
                if (plans[0].ResultID >=1) {
                    $("#divSuccess").show();
                    $("#lblUserMsg").show();
                    document.getElementById("lblUserMsg").innerHTML = "Plan Details Saved Successfully";
                }
                else if(plans[0].ResultName!=null)
                {
                    $("#divError").show();
                    $("#lblErrorUserMsg").show();
                    document.getElementById("lblErrorUserMsg").innerHTML = plans[0].ResultName;
                }

                else {
                    $("#divInvalid").show();
                    $("#lblInvalidUserMsg").show();
                    document.getElementById("lblInvalidUserMsg").innerHTML = "Please Enter Valid Details ";
                }
            
            },

            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert(XMLHttpRequest, textStatus, errorThrown);
            },

        });
  }

//Binding Organization Details in Add Plans form :Ragini on 18-08-2019
function BindPPCPOrganizations(Url) {
  
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
            $('#OrganizationName').data('kendoDropDownList').dataSource.data(OrganizationList);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}
function ValidateplanName(PlanName, Url) {
    debugger;
    if (PlanName == "") {
        $("#spnPlanNamevalidator").hide();
    }
    else {
        var webMethodName = "ValidatePlanName";
        var ParameterNames = new Array();
        var ParameterValues = new Array();
        ParameterNames[0] = "strPlanName";
        ParameterValues[0] = PlanName;
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
                var PlanTest = obj[0];
                if (PlanTest == 0) {
                    document.getElementById("spnPlanNamevalidator").style.display = "none";
                }
                else {
                    document.getElementById("spnPlanNamevalidator").style.display = "block";
                }

            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            },
        });

    }

}
