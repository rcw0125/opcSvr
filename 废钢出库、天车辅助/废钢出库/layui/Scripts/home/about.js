
define(['text!/home/about'], function (template) {
    function init(element) {

        //$(element).html(template);
         $(element).fadeOut(400, function () {
            $(this).replaceWith(template);

        });

    };
    //init();
    return {
        color: "",
        template: template,
        init: init
    };


})