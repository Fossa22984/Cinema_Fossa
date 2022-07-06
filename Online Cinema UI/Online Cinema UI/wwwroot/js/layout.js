$(function () {
    var connection = new signalR.HubConnectionBuilder().withUrl("/notification").build();
    connection.start().then(function () {
        room = $("#userIdHidden").text();
        connection.invoke("Subscribe", room).catch(function (err) {
            return console.error(err.toString());
        });
        connection.invoke("GetNotifications", $("#userIdHidden").text()).catch(function (err) {
            return console.error(err.toString());
        });
    }).catch(function (err) {
        return console.error(err.toString());
    });
    connection.on("SendProgress", function (nameFilm, progress, id, notificationType) {
        debugger;

        var str = 'nameFilm -> ' + nameFilm + ' progress -> ' + progress + ' id -> ' + id;
        console.log(str);

        showProgress(nameFilm, progress, id);

        if (notificationType == 1)
            createToast("Начало загрузки", "Загрузка " + nameFilm + " началась", "info");

        if (notificationType == 2)
            createToast("Загрузка окончена", "Загрузка " + nameFilm + " закончилась", "success");
    });

    //addDownload("Name Film", 58, 1);
    //addDownload("Name Film1", 49, 2);
    //addDownload("Name Film2", 84, 3);
    //test("Name Film", 58, 1);
    numberOfDownloads();
})
function showProgress(nameFilm, progress, id) {
    var count = $("#download #" + id).children();
    if (count.length == 0) {
        addDownload(nameFilm, progress, id);
    }
    else {
        var progressBar = $(count[0]).children().last().children().first();
        $(progressBar).css("width", progress + "%");
        $(progressBar).attr("aria-valuenow", progress);
        $(progressBar).text(progress + "%");
    }
    numberOfDownloads();
}


function addDownload(nameFilm, progress, id) {
    debugger;
    $("#download").append("<li  id='" + id + "'><a class='dropdown-item text-white' href='#'>" +
        "<p class='control-label text-white mb-2 pl-2 pr-2 text-overflow'>" +
        "<i class='fas fa-download text-white mr-2'></i>" + nameFilm + "</p><div class='progress mb-2'>" +
        "<div class='progress-bar' role='progressbar' style='width: " + progress + "%; background: #3eaaaf;' aria-valuenow='" + progress + "' aria-valuemin='0' aria-valuemax='100'>" + progress + "%</div>" +
        "</div></a><hr class='dropdown-divider' style='border-top: 1px solid #3eaaaf63'></li>");
}

function numberOfDownloads() {
    debugger;
    var count = $("#download").children().length
    if (count > 0) {
        $(".num").text(count);
        $("#navbarDropdownBell").attr("data-toggle", "dropdown");
        $("#numberOfDownloads").css("visibility", "visible");
    }
    else {
        $("#navbarDropdownBell").removeAttr("data-toggle");
        $("#numberOfDownloads").css("visibility", "hidden");
    }
}

function checkSearch() {
    let res = $('#searchInput').val();
    if (res != 0) {
        $("#searchHidden").val(res);
        $('#searchButton').removeAttr('disabled');
    }

    else $('#searchButton').attr('disabled', 'disabled');
}

function createToast(title, text, theme) {
    debugger;
    new Toast({
        title: title,
        text: text,
        theme: theme,
        autohide: true,
        interval: 5000
    });
}