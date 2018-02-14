$(document).ready(function () {

    $(".ui.dropdown").dropdown();

    $(".ui.dropdown .remove.icon").on("click", function (e) {
        $(this).parent(".dropdown").dropdown("clear");
        e.stopPropagation();
    });

});