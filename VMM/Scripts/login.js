$(document).ready(function () {
    $("#register-form").submit(function (event) {
        return validateRegister();
    });

    $("#Password").on("focusout", function () {
        validatePassword();
        $("input[data-toggle='valid-password']").popover("toggle");
    });
});

function validatePassword() {
    var strPattern = "^(?=.*[A-Z])(?=.*[!@#$&*_ ])(?=.*[a-z]).{8}$";

    if ($("#Password").val().search(strPattern) != -1) {
        // success
        return true;
    } else {
        return false;
    }
}

function validateRegister() {
    if ($("#Email").val() == "") {
        snack("Please enter a valid e-mail address");
        return false;
    }

    if ($("#Username").val() == "") {
        snack("Please choose your username");
        return false;
    }

    if ($("#Password").val() == "" || !validatePassword()) {
        snack("Please choose a valid password");
        return false;
    }

    return true;
}