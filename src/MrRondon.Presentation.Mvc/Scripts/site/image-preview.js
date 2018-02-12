$(function () {

    "use strict";

    window.ImagePreview = window.ImagePreview || {}

    ImagePreview.Size = 10; //MB
    ImagePreview.FileExtentions = [".jfif", ".jpg", ".jpeg", ".png"];

    ImagePreview.FileUpload = $(document.body).find("[data-fileupload]");

    ImagePreview.Valid = function (fileName, fileSize) {
        var extension = fileName.substr(fileName.lastIndexOf("."));
        var mbSize = (1024 * 1024) * ImagePreview.Size;
        
        if (ImagePreview.FileExtentions.indexOf(extension.toLowerCase()) > -1) {

            if (fileSize > mbSize) {
                GT.notification("error", `Tamanho máximo permitido: ${ImagePreview.Size} MB`);
            }

            return true;
        }
        else {
            var str = "";
            $.each(ImagePreview.FileExtentions, (x, y) => str += y + ", ");
            GT.notification("error", `É permitido apenas arquivos com as seguintes extensões: ${str}`);
        }

        return false;
    };

    ImagePreview.Show = function (element) {
            var $this = $(element);
        $this.on("change", function () {
            var fileName = $this.val().replace(/\\/g, "/").replace(/.*\//, "");
            var fileSize = $this.get(0).files[0].size;

            if (ImagePreview.Valid(fileName, fileSize)) {
                var fReader = new FileReader();
                fReader.readAsDataURL($(this).prop("files")[0]);
                fReader.onloadend = function (event) {
                    $(element.$input).attr("src", event.target.result);
                    var showIn = $this.data("show");
                    $(showIn).attr("src", event.target.result);
                };
            }
        });
    };

    ImagePreview.FileUpload.each(function () { ImagePreview.Show($(this)); });
});