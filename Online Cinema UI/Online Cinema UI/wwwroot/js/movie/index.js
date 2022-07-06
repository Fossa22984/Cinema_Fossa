
$(function () {
    AjaxRequest();
    Change();

    $('#flux').bind('scroll', function () {
        if ($(this).scrollTop() + $(this).innerHeight() >= $(this)[0].scrollHeight) {
            AjaxRequest();
            Change();
        }
    });

    $("#searchButton").on("click", function () {
        $.ajax({
            type: 'POST',
            url: "/CinemaRoom/_CinemaRoomCard",
            success: function (result) {
                $("#flux").html(result);
            }
        });

        parseInt($("#pageNumber").val("1"));
    });

});

function AjaxRequest() {
    let res = parseInt($("#pageNumber").val());
    $.ajax({
        type: 'POST',
        url: "/CinemaRoom/_CinemaRoomCard",
        //$('#posttext-' + publicationId).val()
        success: function (result) {
            $("#flux").append(result);
        }
    });
}

function Change() {
    let count = parseInt($("#pageNumber").val());
    count++;
    $("#pageNumber").val(count);
}