function snack(strMessage) {
    $("#snacks").after("<div id='snackbar'></div>");
    var $bar = $("#snackbar").last();
    $bar.html(strMessage);
    $bar.addClass("show");
    addSnackListener();

    setTimeout(function () {
        $bar.removeClass("show");
        setTimeout(function () {
            $bar.remove();
        }, 500);
    }, 3000);
}

function addSnackListener() {
    $("#snackbar").click(function () {
        $(this).remove();
    });
}