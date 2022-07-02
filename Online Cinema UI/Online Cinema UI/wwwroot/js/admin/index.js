function SFilms(e) {
    $.ajax({
        type: 'GET',
        url: "/Admin/_MovieSettings",
        success: function (result) {
            $("#mainDiv").html(result);
        }
    });

    $(e).addClass("clickOnButton");
    $(e).parent().children().eq(1).removeClass("clickOnButton");
    $(e).parent().children().eq(2).removeClass("clickOnButton");
}

function SCinemaRoom(e) {
    $.ajax({
        type: 'GET',
        url: "/Admin/_CinemaRoomSettings",
        success: function (result) {
            $("#mainDiv").html(result);
        }
    });

    $(e).addClass("clickOnButton");
    $(e).parent().children().eq(0).removeClass("clickOnButton");
    $(e).parent().children().eq(2).removeClass("clickOnButton");
}

function SSession(e) {
    $.ajax({
        type: 'GET',
        url: "/Admin/_SessionSettings",
        success: function (result) {
            $("#mainDiv").html(result);
        }
    });

    $(e).addClass("clickOnButton");
    $(e).parent().children().eq(0).removeClass("clickOnButton");
    $(e).parent().children().eq(1).removeClass("clickOnButton");
}



$(function () {
    $.ajax({
        type: 'GET',
        url: "/Admin/_MovieSettings",
        success: function (result) {
            $("#mainDiv").html(result);
        }
    });
});
