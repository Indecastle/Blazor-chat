var shiftDown = false;



function initChatEvent() {
    var messageBox = $("#chatSend");
    var button = $("#mybutton");
    $(document).keypress(function (e) {
        if (e.keyCode == 13) {
            if (messageBox.is(":focus") && !shiftDown) {
                e.preventDefault(); // prevent another \n from being entered
                button.click();
            }
        }
    });

    $(document).keydown(function (e) {
        if (e.keyCode == 16) shiftDown = true;
    });

    $(document).keyup(function (e) {
        if (e.keyCode == 16) shiftDown = false;
    });
}