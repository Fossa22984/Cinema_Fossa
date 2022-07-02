
$(function () {
    // Get the <datalist> and <input> elements.
    var dataList = document.getElementById('json-datalist');
    var input = document.getElementById('ajax');

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
});

function ChangeFilm(e) {
    $.ajax({
        type: 'GET',
        data: { "movie": $('option[value="' + $("#ajax").val() + '"]').first().prop("data-value") },
        url: "/Admin/_ChangeMovie",
        success: function (result) {
            debugger;
            if (result != "") $("#movieSettingsDiv").html(result);
            else $("#movieSettingsDiv").html("");
        }
    });
}

function AddFilm(e) {
    $.ajax({
        type: 'GET',
        url: "/Admin/_AddMovie",
        success: function (result) {
            $("#movieSettingsDiv").html(result);
        }
    });
    $("#ajax").val("");
}