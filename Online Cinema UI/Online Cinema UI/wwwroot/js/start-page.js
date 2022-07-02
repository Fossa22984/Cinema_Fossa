function LoginIn(e) {
    $.ajax({
        type: 'GET',
        url: "/Account/_Login",
        success: function (result) {
            $("#mainDiv").html(result);
        }
    });

    $(e).addClass("clickOnButton");
    $(e).parent().children().eq(1).removeClass("clickOnButton");
}

function SignUp(e) {
    $.ajax({
        type: 'GET',
        url: "/Account/_Register",
        success: function (result) {
            $("#mainDiv").html(result);
        }
    });

    $(e).addClass("clickOnButton");
    $(e).parent().children().eq(0).removeClass("clickOnButton");
}

function ShowPassword(e) {
    var input = $(e).parent().children().first();

    if ($(input).attr("type") === "password") {
        $(e).attr("class", "fas fa-eye-slash field-icon");
        $(input).attr("type", "text");
    } else {
        $(e).attr("class", "fas fa-eye field-icon");
        $(input).attr("type", "password");
    }
}