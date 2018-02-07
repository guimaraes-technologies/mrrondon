$(function () {

    "use strict";

    window.Correios = window.Correios || {};

    Correios.CleanFields = function () {
        $(".additional-information").val("");
        $(".neighborhood").val("");
        $(".city").val("");
        $(".street").val("");
    };

    Correios.SearchZipCode = function (element) {
        element.on("change", function () {
            var container = $(this).closest("#container-zipcode").addClass("loading");
            var zipcode = element.val();
            var url = "http://api.postmon.com.br/v1/cep/" + zipcode;
            $.getJSON(url, function (data) {
                $(".additional-information").val(data.complemento);
                $(".neighborhood").val(data.bairro);
                $(".city").val(data.cidade);
                $(".street").val(data.logradouro);
            })
            .fail(function () {
                Correios.CleanFields();
            })
            .always(function () {
                container.removeClass("loading");
            });
        });
    };
});