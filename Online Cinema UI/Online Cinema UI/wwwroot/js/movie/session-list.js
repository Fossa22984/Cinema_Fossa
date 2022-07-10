
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

$(function () {

    debugger;
    var inputStart = document.getElementsByClassName('StartTd');
    for (var i = 0; i < inputStart.length; i++) {
        var val = inputStart[i].getAttribute('value');
        var localDate = convertUTCDateToLocalDate(new Date(val));
        var dateForInput = prepareDateForInput(localDate);

        inputStart[i].setAttribute('text', dateForInput);
    }

    var inputEnd = document.getElementsByClassName('EndTd');
    for (var i = 0; i < inputEnd.length; i++) {
        var val = inputEnd[i].getAttribute('value');
        var localDate = convertUTCDateToLocalDate(new Date(val));
        var dateForInput = prepareDateForInput(localDate);

        inputEnd[i].setAttribute('text', dateForInput);
    }
})