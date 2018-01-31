$(function() {

    "use strict";

    window.GT = window.GT || {};

    if ($(".gt-info-menu").length) {
        $(".gt-info-menu > a.item[href$='" + window.location.pathname + "']").addClass("active");
    }

    if ($(".gt-info-menu-drop").length) {
        var itemActive = $(".gt-info-menu-drop > .menu > a.item[href$='" + window.location.pathname + "']")
            .addClass("active");
        $(".gt-info").find(".gt-info-menu-drop").dropdown().find(".text").text(itemActive.text());
    }

    GT.dropdown = function(element) {
        $(element).dropdown({
            forceSelection: false
        });
    };

    GT.inputdatepicker = function(element) {
        $(element).datepicker({
            format: "dd/mm/yyyy",
            days: ["D", "S", "T", "Q", "Q", "S", "S"],
            daysShort: ["D", "S", "T", "Q", "Q", "S", "S"],
            daysMin: ["D", "S", "T", "Q", "Q", "S", "S"],
            months: [
                "JANEIRO", "FEVEREIRO", "MARÇO", "ABRIL", "MAIO", "JUNHO", "JULHO", "AGOSTO", "SETEMBRO", "OUTUBRO",
                "NOVEMBRO", "DEZEMBRO"
            ],
            monthsShort: ["JAN", "FEV", "MAR", "ABR", "MAI", "JUN", "JUL", "AGO", "SET", "OUT", "NOV", "DEZ"],
            autoHide: true
        });
    };

    //GT.inputclockpicker = function (element) {
    //    $(element).clockpicker({
    //        placement: 'top',
    //        align: 'left',
    //        autoclose: true
    //    });
    //};

    GT.OnpenMenu = function(e) {
        $(e).closest(".gt-navigation-left").toggleClass("ativo");
    };

    GT.OnpenSubMenu = function(e) {
        $(e).closest(".gt-container").find("article.gt-article").toggleClass("ativo");
        $(e).closest(".item").toggleClass("ativo");
    };

    GT.SearchRemoto = function() {
        $(".gt-dropdown-remote").dropdown({
            minCharacters: 2,
            saveRemoteData: false,
            debug: true,
            allowAdditions: false,
            useLabels: true,
            dataType: "json",
            apiSettings: {
                cache: false,
                url: $(".gt-dropdown-remote").data("url"),
                onResponse: function(itens) {
                    var response = { results: [] };
                    $.each(itens,
                        function(index, item) {
                            response.results.push({
                                name: item.name,
                                value: item.value,
                                text: item.text
                            });
                        });
                    return response;
                }
            },
            message: {
                source: " Nenhuma fonte utilizada e o módulo Semantic API não foi incluído.",
                noResults: "Sua pesquisa não retornou resultados.",
                logging: "Erro no log de depuração, saindo.",
                noTemplate: "Um nome de modelo válido não foi especificado.",
                serverError: "Houve um problema com a consulta do servidor.",
                maxResults: "Os resultados devem ser uma matriz para usar a configuração maxResults.",
                method: "O método que você chamou não está definido."
            }
        });
    }

    GT.notification = function(type, message) {
        switch (type) {
        case "success":
            Lobibox.notify("success",
                {
                    position: "top right",
                    sound: "../../sounds/sound2",
                    delay: 3000,
                    msg: message,
                    title: "Sucesso",
                    icon: "smile icon"
                });
            break;
        case "error":
            Lobibox.notify("error",
                {
                    position: "top right",
                    sound: "../../sounds/sound4",
                    delay: 3000,
                    msg: message,
                    title: "Erro",
                    icon: "frown icon"
                });
            break;
        case "warning":
            Lobibox.notify("warning",
                {
                    position: "top right",
                    sound: "../../sounds/sound5",
                    delay: 3000,
                    msg: message,
                    title: "Alerta",
                    icon: "warning icon"
                });
            break;
        case "info":
            Lobibox.notify("info",
                {
                    position: "top right",
                    sound: "../../sounds/sound6",
                    delay: 3000,
                    msg: message,
                    title: "Informação",
                    icon: "info circle icon"
                });
            break;
        default:
        }
    }

    GT.SearchRemoto();
});