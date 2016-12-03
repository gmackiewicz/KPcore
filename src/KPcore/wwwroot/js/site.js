// Write your Javascript code.
$(document).ready(function () {
    $("#btn-edit, #btn-cancel").click(function () {
        $("#group-name-display, #group-name-edit, #btn-edit").toggleClass("hidden");
        $("#group-name-validation-error").addClass("hidden");
    });
    $('#group-name-input').on('input', function () {
        var input = $(this);
        var isNotEmpty = input.val();
        if (isNotEmpty) {
            $("#group-name-validation-error").addClass("hidden");
            $("#grp-name-submit").removeClass("hidden");
        } else {
            $("#grp-name-submit").addClass("hidden");
            $("#group-name-validation-error").removeClass("hidden");
        }
    });
});