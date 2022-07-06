function LoadSession() {
    // Get the <datalist> and <input> elements.
    var dataList = document.getElementById('json-datalist');
    var input = document.getElementById('ajax');

    $.ajax({
        type: 'GET',
        url: "/Admin/GetListSessions",
        success: function (result) {
            result.forEach(function (item) {
                var array = item["value"].split("(");
                var nameSession = "";
                for (var i = 0; i < array.length - 1; i++) {
                    nameSession += array[i]
                }


                var localDateStr = array[array.length - 1].substring(0, array[array.length - 1].length - 1);
                var localDate = convertUTCDateToLocalDate(new Date(localDateStr));

                // Create a new <option> element.
                var option = document.createElement('option');

                // Set the value using the item in the JSON array.
                option["data-value"] = item["key"];
                var localDateFormat = `${localDate.toLocaleDateString()} ${localDate.toLocaleTimeString().substring(0, localDate.toLocaleTimeString().length - 3)}`
                option.value = `${nameSession} (${localDateFormat})`;

                // Add the <option> element to the <datalist>.
                dataList.appendChild(option);
            });


            input.placeholder = "Редактировать существующий";
        },
        error: function (e) {
            input.placeholder = "Couldn't load datalist options :(";
        }
    });

    // Update the placeholder text.
    input.placeholder = "Loading options...";
}

function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

    var offset = date.getTimezoneOffset() / 60;
    var hours = date.getHours();

    newDate.setHours(hours - offset);

    return newDate;
}

$(function () {
    LoadSession();
});

function ChangeSession(e) {
    $.ajax({
        type: 'GET',
        data: { "session": $('option[value="' + $("#ajax").val() + '"]').first().prop("data-value") },
        url: "/Admin/_ChangeSession",
        success: function (result) {
            if (result != "") $("#sessionSettingsDiv").html(result);
            else $("#sessionSettingsDiv").html("");
        }
    });
}

function AddSession(e) {
    $.ajax({
        type: 'GET',
        url: "/Admin/_AddSession",
        success: function (result) {
            $("#sessionSettingsDiv").html(result);
        }
    });
    $("#ajax").val("");
}