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

$(function () {
    $("#addCinemaRoomForm").submit(function (e) {
        e.preventDefault();
        var formAction = $(this).attr("action");

        var formData = new FormData();

        var fileInput = $("#ImageFile")[0];
        var file = fileInput.files[0];
        formData.append("ImageFile", file);

        formData.append("CinemaRoomName", $("#CinemaRoomName").val());
        formData.append("Description", $("#Description").val());

        $.ajax({
            type: "POST",
            url: formAction,
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                createToast("Добавление", "Добавление зала «" + $("#CinemaRoomName").val() + "» прошло успешно", "info");
            },
            error: function (e) {
                createToast("Ошибка", e.statusText, "danger");
            }
        })

    })
});