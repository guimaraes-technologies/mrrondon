$(function () {

    window.User = window.User || {}
    User.DropRole = $(".gt-drop-role").dropdown({
        onChange: function (value, text, $selectedItem) {

            Validation.OnSubmit($selectedItem.closest("form"));
        }
    });
});