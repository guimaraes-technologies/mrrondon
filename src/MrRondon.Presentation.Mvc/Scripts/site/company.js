$(function () {

    "use strict";

    window.Company = window.Company || {}

    Company.DropsUrl = $("select[data-drop-url]");

    Company.SetDropdown = function ($this) {
        $this.closest(".ui.dropdown").dropdown({
            onChange: function (value, text, $selectedItem) {
                var $select = $selectedItem.closest(".ui.dropdown").find("select");
                $($select.data("dropSetresult")).closest(".ui.dropdown").dropdown("clear");
                $($select.data("dropSetresult")).closest(".ui.dropdown").dropdown({
                    apiSettings: {
                        cache: true,
                        url: $select.data("dropUrl").replace("value", value)
                    }
                });
            }
        });
    };

    Company.DropsUrl.each(function () {
        Company.SetDropdown($(this));
    });
});


