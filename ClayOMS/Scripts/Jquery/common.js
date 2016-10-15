$(document).ready(function () {
    $("ul.nav.navbar-nav li a").each(function () {
        $(this).parent().removeClass("active");
        //var currentPage = document.location.pathname.match(/[^\/]+$/)[0];
        var currentPage;
        if (document.location.pathname.match(/[^\/]+$/) != null)
            currentPage = document.location.pathname.match(/[^\/]+$/)[0]

        var linkURL = $(this).attr("href").match(/[^\/]+$/)[0];
        if (currentPage == linkURL) {
            //alert("Current Page: " + currentPage);  alert("Matching Anchor: " + linkURL);
            $(this).parent("li").addClass("active");
        }
    });


    $('[data-toggle=offcanvas]').click(function () {
        $('.row-offcanvas').toggleClass('active')
    });

    if ($("#navbar").hasClass("in")) {
    }
    else {
        $("section").css("margin-top", $("#navbar").outerHeight());
    }
});