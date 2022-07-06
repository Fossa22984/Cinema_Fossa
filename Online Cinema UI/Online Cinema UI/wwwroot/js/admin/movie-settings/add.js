$("#ImageFile").change(function (event) {
    let files = event.target.files;
    $("#viewImage").attr("src", window.URL.createObjectURL(files[0]));
});

$("#VideoFile").change(function (event) {
    let files = event.target.files;
    debugger;
    $("#VideoTitle").text("Video: «" + files[0].name + "»");
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

var collectionGenres = new Array();
$(function () {
    $.ajax({
        type: 'GET',
        url: "/Admin/GetListGenre",
        success: function (result) {

            for (var i = 0; i < result.length; i++)
                $("#menuGenre").append("<li><label class='text-white'>" + result[i] + "</label></li>");

            var res = $("#menuGenre li lable").val();
            $("#menuGenre li").children().on("click", function (e) {
                var text = $(this).text();

                var text = $(this).text();
                if (!collectionGenres.some(x => x == text)) {
                    collectionGenres.push(text);

                    $("#fluxGenre").append("<label class='btn clickOnButton text-white mr-3' onclick='{$(this).detach()}'>" + text + " &nbsp; <i class='fas fa-times' style='font-size:12px'></i></label >");
                }
            });
        },
        error: function (e) {
            alert(e.statusText);
        }
    });


    $("#addFilmForm").submit(function (e) {
        e.preventDefault();
        var formAction = $(this).attr("action");

        var formData = new FormData();

        var fileInput = $("#ImageFile")[0];
        var file = fileInput.files[0];
        formData.append("ImageFile", file);

        var videoInput = $("#VideoFile")[0];
        var video = videoInput.files[0];
        formData.append("VideoFile", video);

        var listLable = $("#fluxGenre").children();
        var genres = [];
        for (var i = 0; i < listLable.length; i++)
            genres.push($(listLable[i]).text());
        formData.append("genre", genres);

        formData.append("MovieTitle", $("#MovieTitle").val());
        formData.append("MoviePath", $("#MoviePath").val());
        formData.append("DateOfRelease", $("#DateOfRelease").val());
        formData.append("Author", $("#Author").val());
        formData.append("Actors", $("#Actors").val());
        formData.append("Country", $("#Country").val());
        formData.append("AgeLimit", $("#AgeLimit").val());
        formData.append("Description", $("#Description").val());
        formData.append("MovieBudget", $("#MovieBudget").val());
        formData.append("IsCartoon", $('#IsCartoon').is(":checked"));

        $.ajax({
            type: "POST",
            url: formAction,
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                //alert("success");
            },
            error: function (e) {
                createToast("Ошибка", e.statusText, "danger");
                //alert(e.statusText)
            }
        })

        //createToast('tit','text');

        createToast("Подготовка к загрузке", "Загрузка " + $("#MovieTitle").val() + " скоро начнется", "warning");

    })
});