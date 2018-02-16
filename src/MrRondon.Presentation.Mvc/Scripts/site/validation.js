$(function () {

    window.Validation = window.Validation || {};

    Validation.validateEmail = function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

    Validation.validate = function () {
        var $this = $(this);
        var divAllItems = $(this).closest(".all-items");
        var result = divAllItems.find(".result");
        result.text("");
        var email = $this.val();
        if (Validation.validateEmail(email)) {
            result.text("");
        } else {
            result.text(email + " não é um email válido.");
            result.css("color", "red");
        }
        return false;
    }

    Validation.mask = function (element, mask) {
        element.attr("maxlength", 100);
        var divAllItems = element.closest(".all-items");
        var result = divAllItems.find(".result");
        result.text("");
        element.unbind();
        element.unmask();
        switch (mask) {
            case "cpf": element.mask("999.999.999-99"); break;
            case "data": element.mask("99/99/9999"); break;
            case "hora": element.mask("99:99"); break;
            case "cep": element.mask("99999-999"); break;
            case "cnpj": element.mask("99.999.999/9999-99"); break;
            case "telephone": element.mask("(99) 9999-9999"); break;
            case "cellphone": element.mask("(99) 99999-9999"); break;
            case "email": element.bind("blur", Validation.validate); break;
        }
    }

    $.fn.form.settings.rules.remote = function (value, input, test) {
        $.ajax({
            type: "GET",
            url: $(this).data("val-remote-url"),
            data: { value },
            async: false,
            success: res => input = res
        });

        return input;
    };

    Validation.Form = $("form.ui.form");

    Validation.SetDropdown = function (element) {
        if (element.hasClass("dropdown")) {
            element.dropdown({
                forceSelection: false
            });
        }
    };

    Validation.SetMask = function (element) {
        if (element.data("gtmask")) {
            switch (element.data("gtmask")) {
                case "cpf": element.mask("999.999.999-99"); break;
                case "data": element.mask("99/99/9999"); break;
                case "hora": element.mask("99:99"); break;
                case "cep": element.mask("99999-999"); break;
                case "cnpj": element.mask("99.999.999/9999-99"); break;
                case "telephone": element.mask("(99) 99999-9999"); break;
                case "cellphone": element.mask("(99) 99999-9999"); break;
            }
        }
    };

    Validation.SetFunctions = function (element) {
        if (element.data("function")) {
            eval(element.data("function"))(element);
        }
    };

    Validation.OnSubmit = function (form) {
        var $form = $(form);
        var fields = {};

        $form.find("input, textarea, select").each(function (i, element) {

            var $element = $(element);

            if ($element.data("rules")) { fields[$element.attr("name")] = { identifier: $element.attr("name"), rules: $element.data("rules") }; }

            Validation.SetDropdown($element);
            Validation.SetMask($element);
            Validation.SetFunctions($element);

        });

        $form.form({
            fields,
            inline: true,
            on: "blur",
            onSuccess: () => $form.closest("article.ui.segment").addClass("loading")
        });
    };

    Validation.OnSubmitAjax = function (form) {

        var $form = $(form);
        var fields = {};

        $form.find("input, textarea, select").each(function (i, element) {

            var $element = $(element);

            if ($element.data("rules")) { fields[$element.attr("name")] = { identifier: $element.attr("name"), rules: $element.data("rules") }; }

            Validation.SetDropdown($element);
            Validation.SetMask($element);
            Validation.SetFunctions($element);

        });

        $form.form({
            fields,
            inline: true,
            on: 'blur',
            onSuccess: function (e) {
                e.preventDefault();
                return $.ajax({
                    type: $form.attr('method'),
                    url: $form.attr('action'),
                    data: $form.serialize(),
                    success: function () {
                        $($form.data("closeModal")).modal("hide");
                    }
                });
            }
        });
    };

    Validation.Form.each(function (key, form) {
        Validation.OnSubmit(form);
    });
});