function ChangeStart(e) {
    ChangeMovie();
    ChangeCinemaRoom();
}

function ChangeMovie(e) {
    var res = $("#End").val();
    $.ajax({
        type: 'GET',
        data: {
            "movieId": $('option[value="' + $("#ajaxMovie").val() + '"]').first().prop("data-value"),
            "start": $("#Start").val()
        },
        url: "/Admin/GetMovieDuration",
        success: function (result) {
            if (result != "") $("#End").val(result);
            else $("#End").val("");
        }
    });
}

function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

    var offset = date.getTimezoneOffset() / 60;
    var hours = date.getHours();

    newDate.setHours(hours - offset);

    return newDate;
}

function ChangeCinemaRoom(e) {
    if ($('option[value="' + $("#ajaxCinemaRoom").val() + '"]').first().prop("data-value") != undefined) {
        $.ajax({
            type: 'GET',
            data: {
                "cinemaRoomId": $('option[value="' + $("#ajaxCinemaRoom").val() + '"]').first().prop("data-value"),
                "dateSession": $("#Start").val()
            },
            url: "/Admin/_ListSessions",
            success: function (result) {
                if (result != "") $("#listSessionsDiv").html(result);
                else $("#listSessionsDiv").html("");
            }
        });
    }
    else $("#listSessionsDiv").html("");
}

function prepareDateForInput(date) {
    var dd = date.getDate();
    var mm = date.getMonth() + 1; // Месяца идут с 0, так что добавляем 1.
    var yyyy = date.getFullYear();
    var minutes = date.getMinutes();
    var hour = date.getHours();
    var seconds = date.getSeconds();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }
    if (hour < 10) {
        hour = '0' + hour
    }
    if (minutes < 10) {
        minutes = '0' + minutes
    }
    if (seconds < 10) {
        seconds = '0' + seconds
    }

    date = yyyy + '-' + mm + '-' + dd + 'T' + hour + ':' + minutes + ':' + seconds;
    return date;
}

$(function () {
    // Get the <datalist> and <input> elements.
    var dataListCinemaRoom = document.getElementById('json-datalistCinemaRoom');
    var inputCinemaRoom = document.getElementById('ajaxCinemaRoom');

    var inputStart = document.getElementById('Start');
    var val = inputStart.getAttribute('value');
    var localDate = convertUTCDateToLocalDate(new Date(val));
    var dateForInput = prepareDateForInput(localDate);

    inputStart.setAttribute('value', dateForInput);

    $.ajax({
        type: 'GET',
        url: "/Admin/GetListCinemaRooms",
        success: function (result) {
            result.forEach(function (item) {
                // Create a new <option> element.
                var option = document.createElement('option');


                // Set the value using the item in the JSON array.
                option["data-value"] = item["key"];
                option.value = item["value"];

                if (option["data-value"] == $("#CinemaRoomId").val())
                    $("#ajaxCinemaRoom").val(option.value);

                // Add the <option> element to the <datalist>.
                dataListCinemaRoom.appendChild(option);
            });


            inputCinemaRoom.placeholder = "Cinema Room";
        },
        error: function (e) {
            inputCinemaRoom.placeholder = "Couldn't load datalist options :(";
        }
    });

    // Update the placeholder text.
    inputCinemaRoom.placeholder = "Loading options...";


    // Get the <datalist> and <input> elements.
    var dataListMovie = document.getElementById('json-datalistMovie');
    var inputMovie = document.getElementById('ajaxMovie');

    $.ajax({
        type: 'GET',
        url: "/Admin/GetListMovies",
        success: function (result) {
            result.forEach(function (item) {

                // Create a new <option> element.
                var option = document.createElement('option');

                // Set the value using the item in the JSON array.
                option["data-value"] = item["key"];
                option.value = item["value"];
                if (option["data-value"] == $("#MovieId").val())
                    $("#ajaxMovie").val(option.value);
                // Add the <option> element to the <datalist>.
                dataListMovie.appendChild(option);
            });


            inputMovie.placeholder = "Movie";
        },
        error: function (e) {
            inputMovie.placeholder = "Couldn't load datalist options :(";
        }
    });

    // Update the placeholder text.
    inputMovie.placeholder = "Loading options...";


    $("#changeSessionForm").submit(function (e) {
        e.preventDefault();
        var formAction = $(this).attr("action");

        var formData = new FormData();

        var startDateUtc = new Date($("#Start").val()).toUTCString();
        var endDateUtc = new Date($("#End").val()).toUTCString();
        formData.append("Id", $("#Id").val());
        formData.append("Start", startDateUtc);
        formData.append("End", endDateUtc);
        formData.append("MovieId", $('option[value="' + $("#ajaxMovie").val() + '"]').first().prop("data-value"));
        formData.append("CinemaRoomId", $('option[value="' + $("#ajaxCinemaRoom").val() + '"]').first().prop("data-value"));

        debugger;

        $.ajax({
            type: "POST",
            url: formAction,
            data: formData,
            processData: false,
            contentType: false,
            success: function () {
                createToast("Изменение", "Изменение сессии прошло успешно", "info");
                ChangeCinemaRoom();

            },
            error: function (e) {
                createToast("Ошибка", e.statusText, "danger");
            }
        })
    })
});