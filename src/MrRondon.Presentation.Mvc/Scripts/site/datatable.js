$(function () {

    "use strict";

    window.Table = window.Table || {};

    var $table = $(document.body).find(".gt-datatable");

    $table.DataTable({
        processing: true,
        serverSide: true,
        sort: false,
        oClasses: {
            //sWrapper: "gt-mt-n-80px",
            sProcessing: "ui active inverted dimmer",
            sPaging: "ui pagination menu ",
            sPageButton: "item",
            sPageButtonActive: "active",
            sPageButtonDisabled: "disabled"
        },
        ajax: {
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: $table.data("action"),
            data: function (d) {
                return JSON.stringify({ parameters: d });
            }
        },
        language: {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "<div class='ui medium text loader'>Carregando...</div>",
            "sZeroRecords": "Nenhum registro encontrado",
            "sSearch": "",
            "oPaginate": {
                "sNext": "<i class='angle right icon icon'>",
                "sPrevious": "<i class='angle left icon icon'>",
                "sFirst": "",
                "sLast": ""
            },
            "oAria": {
                "sSortAscending": "",
                "sSortDescending": ""
            }
        },
        createdRow: function (row, data, index) {
            $(row).find("td:first").attr("hidden", true);
            if (data[0] === "False") { $(row).addClass('negative'); }
        },
        dom:
             "<'ui basic segment gt-datatable-filter' f>" +
             "<'ui basic segment gt-datatable-table' tr>" +
             "<'ui basic segment gt-datatable-pagination' p>"
    });

    $.extend(true, $table.DataTable.ext.renderer, {
        pageButton: {
            _: function (settings, host, idx, buttons, page, pages) {
                var classes = settings.oClasses;
                var lang = settings.oLanguage.oPaginate;
                var aria = settings.oLanguage.oAria.paginate || {};
                var btnDisplay, btnClass, counter = 0;

                var attach = function (container, buttons) {
                    var i, ien, node, button;
                    var clickHandler = function (e) {
                        settings.oApi._fnPageChange(settings, e.data.action, true);
                    };

                    for (i = 0, ien = buttons.length ; i < ien ; i++) {
                        button = buttons[i];

                        if ($.isArray(button)) {
                            attach(container, button);
                        }
                        else {
                            btnDisplay = '';
                            btnClass = '';

                            switch (button) {
                                case 'ellipsis':
                                    btnDisplay = '&#x2026;';
                                    btnClass = 'disabled';
                                    break;

                                case 'first':
                                    btnDisplay = lang.sFirst;
                                    btnClass = button + (page > 0 ?
                                        '' : ' disabled');
                                    break;

                                case 'previous':
                                    btnDisplay = lang.sPrevious;
                                    btnClass = button + (page > 0 ?
                                        '' : ' disabled');
                                    break;

                                case 'next':
                                    btnDisplay = lang.sNext;
                                    btnClass = button + (page < pages - 1 ?
                                        '' : ' disabled');
                                    break;

                                case 'last':
                                    btnDisplay = lang.sLast;
                                    btnClass = button + (page < pages - 1 ?
                                        '' : ' disabled');
                                    break;

                                default:
                                    btnDisplay = button + 1;
                                    btnClass = page === button ?
                                        'active' : '';
                                    break;
                            }

                            var tag = btnClass.indexOf('disabled') === -1 ?
                                'a' :
                                'div';

                            if (btnDisplay) {
                                node = $('<' + tag + '>', {
                                    'class': classes.sPageButton + ' ' + btnClass,
                                    'id': idx === 0 && typeof button === 'string' ?
                                        settings.sTableId + '_' + button :
                                        null,
                                    'href': '#',
                                    'aria-controls': settings.sTableId,
                                    'aria-label': aria[button],
                                    'data-dt-idx': counter,
                                    'tabindex': settings.iTabIndex
                                })
                                    .html(btnDisplay)
                                    .appendTo(container);

                                settings.oApi._fnBindAction(
                                    node, { action: button }, clickHandler
                                );

                                counter++;
                            }
                        }
                    }
                };

                var activeEl;

                try { activeEl = $(host).find(document.activeElement).data('dt-idx'); }
                catch (e) { }

                attach($(host).empty(), buttons);

                if (activeEl !== undefined) {
                    $(host).find('[data-dt-idx=' + activeEl + ']').focus();
                }
            }
        }
    });

    var $filter = $table.closest("#DataTables_Table_0_wrapper").find(".gt-datatable-filter > div > label");
    $filter.addClass("fluid ui left icon input").append("<i class='search link icon'></i>");
    $filter.find("input").attr("placeholder", "Pesquisar...");

    Table.Reload = function (action) {
        $table.DataTable().ajax.url(action).load();
    };
});