  function ChangeStart(e) {
        ChangeMovie();
        ChangeCinemaRoom();
    }

    function ChangeMovie(e) {
        var res = $("#End").val();
        debugger;
        $.ajax({
            type: 'GET',
            data: {
                "movieId": $('option[value="' + $("#ajaxMovie").val() + '"]').first().prop("data-value"),
                "start": $("#Start").val()
            },
            url: "/Admin/GetMovieDuration",
            success: function (result) {
                debugger;
                if (result != "") $("#End").val(result);
                else $("#End").val("");
            }
        });
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
                    debugger;
                    if (result != "") $("#listSessionsDiv").html(result);
                    else $("#listSessionsDiv").html("");
                }
            });
        }
        else $("#listSessionsDiv").html("");
    }

    $(function () {

        debugger;
        // Get the <datalist> and <input> elements.
        var dataListCinemaRoom = document.getElementById('json-datalistCinemaRoom');
        var inputCinemaRoom = document.getElementById('ajaxCinemaRoom');

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
                    
                    debugger;
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
                    debugger;
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


            formData.append("Id", $("#Id").val());
            formData.append("Start", $("#Start").val());
            formData.append("End", $("#End").val());
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
                    alert("success");
                    ChangeCinemaRoom();

                },
                error: function (e) {
                    alert(e.statusText)
                }
            })
        })
    });