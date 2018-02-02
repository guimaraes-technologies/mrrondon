$(function () {

    "use strict";

    window.Carrossel = window.Carrossel || {};

    Carrossel.Element = $(".gt-carrossel");

    Carrossel.Values = Carrossel.Element.data();

    setInterval(function () {
        var random = Math.floor((Math.random() * Carrossel.Values.carrosselQtd) + 1);
        var imgFadeIn = $("<div>").css("background-image", `url(${Carrossel.Values.carrosselFile}${random}.png)`);
        var imgFadeOut = Carrossel.Element.find("div");
        Carrossel.Element.prepend(imgFadeIn);
        imgFadeOut.fadeOut(2500, function () { imgFadeOut.remove() });
    }, 5000);
});