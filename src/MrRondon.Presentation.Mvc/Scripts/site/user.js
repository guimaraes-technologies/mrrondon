$(function () {

    window.Usuario = window.Usuario || {}

    Usuario.DropRole = $(".gt-drop-role").dropdown({
        onChange: function (value, text, $selectedItem) {

            Validation.OnSubmit($selectedItem.closest("form"));
        }
    });

});