


function GetClaims(Url, OrganizationID, ClaimStartDate, ClaimEndDate, ClaimStatus) {
    debugger;
    var webMethodName = "GetClaims";
    var ParameterNames = new Array();
    var ParameterValues = new Array();
    ParameterNames[0] = "OrganizationID";
    ParameterValues[0] = OrganizationID;
    ParameterNames[1] = "ClaimStartDate";
    ParameterValues[1] = ClaimStartDate;
    ParameterNames[2] = "ClaimEndDate";
    ParameterValues[2] = ClaimEndDate;
    ParameterNames[3] = "ClaimStatus";
    ParameterValues[3] = ClaimStatus;
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
            var list = obj[0];
            //$("#DivMemberGrid").show();
            $("#gridClaims").data("kendoGrid").dataSource.data(list);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        },
    });
}

function EditOptions(mode, VisitId) {
    if (mode == 'view') {
        alert('view' + VisitId);
    } 
    if (mode == 'edit') {
        alert('edit' + VisitId);
    }
}