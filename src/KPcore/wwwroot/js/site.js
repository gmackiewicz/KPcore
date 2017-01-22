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
    $(".btn-delete").append(" <i style='color:white' class='fa fa-trash-o'></i>");
    $(".btn-edit").append(" <i style='color:white' class='fa fa-pencil-square-o'></i>");
    $(".user-list-main > h3").click(function () {
        $(".user-list").slideToggle("fast");
        $(".user-list-main > h3 > i").toggleClass("fa-plus-square-o").toggleClass("fa-minus-square-o");
    });

    $(".deadline-list-main > h3").click(function() {
        $(".deadline-list").slideToggle("fast");
        $(".deadline-list-main > h3 > i").toggleClass("fa-plus-square-o").toggleClass("fa-minus-square-o");
    });
});