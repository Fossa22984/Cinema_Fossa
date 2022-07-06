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

function detachGenre(elem) {
    var text = ($(elem).text()).replace(/\s/g, '');
    collectionGenres.splice(collectionGenres.indexOf(text), 1);
    $(elem).detach();
}


var collectionGenres = new Array();
GetGenres();
function GetGenres() {
    var Genres = $("#fluxGenre").children();

    for (var i = 0; i < Genres.length; i++) {
        collectionGenres.push(($(Genres[i]).text()).replace(/\s/g, ''));
    }
}

$(function () {
    $.ajax({
        type: 'GET',
        url: "/Admin/GetListGenre",
        success: function (result) {
            debugger
            for (var i = 0; i < result.length; i++)
                $("#menuGenre").append("<li><label class='text-white'>" + result[i] + "</label></li>");

            var res = $("#menuGenre li lable").val();
            $("#menuGenre li").children().on("click", function (e) {
                var text = $(this).text();
                if (!collectionGenres.some(x => x == text)) {
                    collectionGenres.push(text);

                    $("#fluxGenre").append("<label class='btn clickOnButton text-white mr-3' onclick='{detachGenre(this)}'>" + text + " &nbsp; <i class='fas fa-times' style='font-size:12px'></i></label >");
                }
            });
        },
        error: function (e) {
            alert(e.statusText);
        }
    });

    $("#changeFilmForm").submit(function (e) {
        e.preventDefault();
        var formAction = $(this).attr("action");

        var formData = new FormData();

        var fileInput = $("#ImageFile")[0];
        var file = fileInput.files[0];
        formData.append("ImageFile", file);


        var listLable = $("#fluxGenre").children();
        var genres = [];
        for (var i = 0; i < listLable.length; i++)
            genres.push($(listLable[i]).text());
        formData.append("genre", genres);

        formData.append("Id", $("#Id").val());
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
        formData.append("Remote", $('#Remote').is(":checked"));

        $.ajax({
            type: "POST",
            url: formAction,
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                createToast("Изменение", "Изменение «" + $("#MovieTitle").val() + "» прошло успешно", "info");
            },
            error: function (e) {
                createToast("Ошибка", e.statusText, "danger");
            }
        })

    })
});