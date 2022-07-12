
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

    date = yyyy + '-' + mm + '-' + dd + ' ' + hour + ':' + minutes + ':' + seconds;
    return date;
}

function convertUTCDateToLocalDate(date) {
    var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

    var offset = date.getTimezoneOffset() / 60;
    var hours = date.getHours();

    newDate.setHours(hours - offset);

    return newDate;
}

$(function () {

    debugger;


    var inputStart = $(".StartTd");
    for (var i = 0; i < inputStart.length; i++) {
        var val = $(inputStart[i]).text();
        var localDate = convertUTCDateToLocalDate(new Date(val));
        var dateForInput = prepareDateForInput(localDate);

        $(inputStart[i]).text(dateForInput);
    }

    var inputEnd = $(".EndTd");
    for (var i = 0; i < inputEnd.length; i++) {
        var val = $(inputEnd[i]).text();
        var localDate = convertUTCDateToLocalDate(new Date(val));
        var dateForInput = prepareDateForInput(localDate);

        $(inputEnd[i]).text(dateForInput);
    }
})