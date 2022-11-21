function divPopUpSuccessClose() {
    debugger;
    $.ajax({
        url: '@Url.Action("VerifyOrgOTP", "Account")',
        data: { otp: $("#OTP").val() },
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {

            if (result == "1") {
                var getUrl = '@Url.Action("ViewPlans", "Admin")';
                window.location = getUrl;
            }
            else {
                $("#lblUserMessag").html("Invalid OTP");
                return false;
            }
        },
        error: function (ex) {
            alert(ex.error());
            location.reload();
        }
    });
}

function ValidateCredentials() {

    var username = $("#UserName").val();
    var password = $("#Password").val();

    var captcha_response = grecaptcha.getResponse();
    if (username == "" && password == "" && captcha_response.length == "0") {
        document.getElementById("spnInvalidUser").innerHTML = "";
        $("#spnInvalidUser").html("Please enter Username.");
        return false;
    }
    else if (password == "") {
        document.getElementById("spnInvalidUser").innerHTML = "";
        $("#spnInvalidUser").html("Please enter Password.");
        return false;
    }
    else if (captcha_response.length == "0") {
        document.getElementById("spnInvalidUser").innerHTML = "";
        $("#spnInvalidUser").html("Please check the checkbox");
        return false;
    }
    else {
        document.getElementById("spnInvalidUser").innerHTML = "";
    }

    jsModel = {};
    jsModel.UserName = username
    jsModel.Password = password;
    //var getUrl = '@Url.Action("ViewPlans", "Admin")';
    //window.location = getUrl;
    $.ajax({
        url: "@Url.Action('ValidateCredentials', 'Account')",
        data: jsModel,
        dataType: 'json',
        contentType: 'application/json',
        success: function (result) {


            if (result == "1") {
                var getUrl = '@Url.Action("ViewPlans", "Admin")';
                window.location = getUrl;
            }
            else if (result == "0") {
                $("#divOTPAfterLogin").show();
            }
            else if (result == "2") {
                $("#spnInvalidUser").html("Your username or password is incorrect.");
            }
            else if (result == "999") {
                $("#spnInvalidUser").html("Your account is locked. Please contact our support team.");
            }
            else {
                document.getElementById("spnInvalidUser").innerHTML = "";
            }
        },
        error: function (ex) {
            alert(ex.error());
            //location.reload();
        }
    });
}