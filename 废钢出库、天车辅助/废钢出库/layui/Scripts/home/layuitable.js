
define(['text!/home/layuitable'], function (template) {
    function init(element) {
        
        $(element).fadeOut(400, function () {
            $(this).replaceWith(template);

        });

    };
    //init();
    return {
        //color: "black",
        color: "",
        template: template,
        init: init
    };


})