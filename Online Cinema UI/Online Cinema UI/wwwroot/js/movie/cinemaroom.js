
function GetAllSessions(e) {
    $.ajax({
        type: 'GET',
        data: { "cinemaRoomId": $("#Id").val() },
        url: "/CinemaRoom/_ListSessions",
        success: function (result) {
            $("#listSessionsDiv").html(result);
        }
    });
    $("#ajax").val("");
}

function SearchByDate(e) {
    $.ajax({
        type: 'GET',
        data: {
            "cinemaRoomId": $("#Id").val(),
            "dateSession": $("#dateSession").val()
        },
        url: "/CinemaRoom/_ListSessions",
        success: function (result) {
            if (result != "") $("#listSessionsDiv").html(result);
            else $("#listSessionsDiv").html(result);
        }
    });
}

function ScrollChat(e) {
    shouldScroll = messages.scrollTop + messages.clientHeight === messages.scrollHeight;
    if (!shouldScroll) {
        $(e).attr("data-scroll", "false");
    }
    else {
        $(e).attr("data-scroll", "true");
    }
}

const messages = document.getElementById('ChatRoom');

function getMessages() {
    shouldScroll = messages.scrollTop + messages.clientHeight === messages.scrollHeight;
    if (!shouldScroll && $(messages).attr("data-scroll") == "true") {
        scrollToBottom();
    }
}

function scrollToBottom() {
    messages.scrollTop = messages.scrollHeight;
}

function checkChatMessage() {
    let res = $('#MessageTextarea').val();
    if (res != 0) $('#sendMessageButton').removeAttr('disabled');
    else $('#sendMessageButton').attr('disabled', 'disabled');
}

$("textarea").each(function () {
    this.setAttribute("style", "height:" + (this.scrollHeight) + "px;overflow-y:hidden;");
}).on("input", function () {

    this.style.height = "auto";
    this.style.height = (this.scrollHeight) + "px";
    if (parseInt(this.style.height) > 150) {
        this.style.height = "150px"
    }
});
function showChat(e) {
    if ($(e).attr("data-open") == "true") {
        $("#chatDiv").attr("hidden", "hidden");
        $(e).attr("data-open", "false");
        $("#videoDiv").css("width", "100vw");
    }
    else if ($(e).attr("data-open") == "false") {
        $("#chatDiv").removeAttr("hidden");
        $(e).attr("data-open", "true");
        $("#videoDiv").css("width", "75vw");
    }
}

function addNewButton(data) {

    var myPlayer = data.player,
        controlBar,
        newElement = document.createElement('button'),
        newLink = document.createElement('span');

    newElement.id = data.id;
    newElement.className = 'vjs-control vjs-button chat-button';
    newElement.type = "button"

    newLink.innerHTML = "<i class='fa " + data.icon + " line-height' aria-hidden='true' style='color:#3eaaaf'></i>";
    newElement.appendChild(newLink);
    rightPanel = $(".amp-controlbaricons-right");
    controlBar = document.getElementsByClassName('vjs-control-bar')[0];

    insertBeforeNode = document.getElementsByClassName('vjs-fullscreen-control')[0];
    $('.vjs-control-bar').append(newElement);

    return newElement;
}

function addSourceToVideo(element, src, type) {
    element.innerHTML = "";
    var source = document.createElement('source');

    source.src = src;
    source.type = type;

    element.appendChild(source);
}

window.onload = function () {
    addNewButton({ player: "video", icon: "fas fa-comment-alt", id: "buttonChat" });
    $("#buttonChat").attr("data-open", "true");
    $("#buttonChat").on("click", function () { showChat(this); })
};

$(function () {
    IsEnded = true;

    var myplayer = videojs('video');
    myplayer.controlBar.progressControl.hide();

    var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();

    connection.on("Send", function (user, message) {
        var msg = "<p class='text-white m-0' style='overflow-wrap: break-word;' ><span style='color:#3eaaaf'>" + user + "</span>: " + message + "</p >";
        $("#ChatRoom").append(msg)
        getMessages();

    });

    var currentTime;
    var startTime;
    connection.on("GetSession", function (message) {
        if ($("#sessionId").val() == message) {
            return;
        }

        IsEnded = false;
        $.ajax({
            type: 'GET',
            data: { "sessionId": message },
            url: '/CinemaRoom/GetSession',
            success: function (result) {
                var res = new Date();
                startTime = convertUTCDateToLocalDate(new Date(result.start));
                currentTime = (new Date() - Date.parse(startTime)) / 1000;

                var res = $("#sessionId").val()
                if ($("#sessionId").val() == "") {
                    self.player.src({ type: 'video/mp4', src: result.movie.moviePath });
                    self.player.currentTime(currentTime);
                    $("#sessionId").val(result.id);
                    $("#movieTitle").text(result.movie.movieTitle);

                }
                var vid = self.player.currentTime();
                if (!(vid <= (currentTime + 1) && vid >= (currentTime - 1))) {
                    self.player.currentTime(currentTime);
                }
            }
        });
    });

    function convertUTCDateToLocalDate(date) {
        var newDate = new Date(date.getTime() + date.getTimezoneOffset() * 60 * 1000);

        var offset = date.getTimezoneOffset() / 60;
        var hours = date.getHours();

        newDate.setHours(hours - offset);

        return newDate;
    }


    connection.start().then(function () {
        var room = $("#ChatId").val();
        var user = '@user.UserName';
        connection.invoke("SendMessage", user, "", room, true).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
    }).catch(function (err) {
        return console.error(err.toString());
    });

    $("#sendMessageButton").on("click", function () {
        var room = $("#ChatId").val();
        var user = '@user.UserName';
        var message = $("#MessageTextarea").val();
        connection.invoke("SendMessage", user, message, room, false).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();
        $("#MessageTextarea").val("");
        $('#sendMessageButton').attr('disabled', 'disabled');
    });

    var self = this;
    this.player = videojs('video');

    this.player.on("ended", function () {
        $("#sessionId").val("");
        $("#movieTitle").text("");
        if (self.player.src() != "/Movies/Background Video.mp4")
            self.player.src({ type: 'video/mp4', src: "/Movies/Background Video.mp4" });

        IsEnded = true;
        self.player.play();

    });

    this.player.on("play", function () {
        if (IsEnded) { return; }
        currentTime = (new Date() - Date.parse(startTime)) / 1000;

        var vid = self.player.currentTime();
        if (!(vid <= (currentTime + 1) && vid >= (currentTime - 1))) {
            self.player.currentTime(currentTime);
        }
    });

})