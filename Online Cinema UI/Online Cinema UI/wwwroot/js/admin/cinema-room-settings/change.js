$("#ImageFile").change(function (event) {
    let files = event.target.files;
    $("#viewImage").attr("src", window.URL.createObjectURL(files[0]));
});

$("textarea").each(function () {
    this.setAttribute("style", "height:" + (this.scrollHeight) + "px;overflow-y:hidden;");
}).on("input", function () {
    this.style.height = "auto";
    this.style.height = (this.scrollHeight) + "px";
});

function Clear() {
    $("#viewImage").attr("src", "../Images/background-fon.jpg");
    $("#fluxGenre").html("");
}

function GetAllSessions(e) {
    $.ajax({
        type: 'GET',
        data: { "cinemaRoomId": $("#Id").val() },
        url: "/Admin/_ListSessions",
        success: function (result) {
            $("#listSessionsDiv").html(result);
        }
    });
    $("#ajax").val("");
}

function SearchByDate(e) {
    $.ajax({
        type: 'GET',
        data: {
            "cinemaRoomId": $("#Id").val(),
            "dateSession": $("#dateSession").val()
        },
        url: "/Admin/_ListSessions",
        success: function (result) {
            if (result != "") $("#listSessionsDiv").html(result);
            else $("#listSessionsDiv").html(result);
        }
    });
}

$(function () {
    $("#changeCinemaRoomForm").submit(function (e) {
        e.preventDefault();
        var formAction = $(this).attr("action");

        var formData = new FormData();

        var fileInput = $("#ImageFile")[0];
        var file = fileInput.files[0];
        formData.append("ImageFile", file);

        formData.append("Id", $("#Id").val());
        formData.append("CinemaRoomName", $("#CinemaRoomName").val());
        formData.append("Description", $("#Description").val());
        formData.append("Remote", $('#Remote').is(":checked"));

        $.ajax({
            type: "POST",
            url: formAction,
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                createToast("Изменение", "Изменение зала «" + $("#CinemaRoomName").val() + "» прошло успешно", "info");
            },
            error: function (e) {
                createToast("Ошибка", e.statusText, "danger");
            }
        })

    })
});