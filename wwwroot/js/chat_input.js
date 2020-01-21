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




var droppedFiles = null;

function fileContainerChangeFile(e) {
    document.getElementById('fileSelectBox').classList.remove('fileContainerDragOver');
    try {
        droppedFiles = document.getElementById('fs').files;
        document.getElementById('fileName').textContent = droppedFiles[0].name;
    } catch (error) { ; }
    // you can also use the property from the fs field, but this won't work
    // with good old IE.
    try {
        aName = document.getElementById('fs').value;
        if (aName !== '') {
            document.getElementById('fileName').textContent = aName;
        }
    } catch (error) {
        ;
    }

}

function dragOver(e) {
    document.getElementById('fileSelectBox').classList.add('fileContainerDragOver');
    e.preventDefault();
    e.stopPropagation();
}

function dragOver2(e) {
    document.getElementById('fileSelectBox').classList.remove('mydisable');
    document.getElementById('chatSend').classList.add('mydisable');
    e.preventDefault();
    e.stopPropagation();
}

function leaveDrop2(e) {
    document.getElementById('fileSelectBox').classList.remove('fileContainerDragOver');
    document.getElementById('fileSelectBox').classList.add('mydisable');
    document.getElementById('chatSend').classList.remove('mydisable');
}

function onDrop(e) {
    document.getElementById('fileSelectBox').classList.remove('fileContainerDragOver');
    try {
        droppedFiles = e.dataTransfer.files;
        var filesCount = droppedFiles.length;

        if (filesCount === 1) {
            // if single file is selected, show file name
            document.getElementById('fileName').textContent = droppedFiles[0].name;
        } else {
            // otherwise show number of files
            document.getElementById('fileName').textContent = filesCount + ' files selected';
        }
    } catch (error) { ; }

    leaveDrop2(e);
    //e.preventDefault();
    e.stopPropagation();
}




function setFocusToTextBox() {
    document.getElementById("mybutton").focus();
    document.getElementById("chatSend").focus();
}