//Sliding Effect Control
head.js(_sysPath+"res/assets/js/skin-select/jquery.cookie.js");
head.js(_sysPath + "res/assets/js/skin-select/skin-select.js");

//Bootstrap
//head.js(_sysPath + "res/assets/js/bootstrap.js");

////Accordion and Sliding menu
function accordionMenu() {
    head.js(_sysPath + "res/assets/js/custom/scriptbreaker-multiple-accordion-1.js", function () {
        $(".topnav").accordionze({
            accordionze: true,
            speed: 500,
            closedSign: '<span class="icon-plus"></span>&nbsp;&nbsp;&nbsp;&nbsp;',
            openedSign: '<span class="icon-minus"></span>&nbsp;&nbsp;&nbsp;&nbsp;'
        });
    });

}

////右滑菜单  v520新版后没有右滑菜单了，和jq3.3.1产生兼容性问题，先注释
//head.js(_sysPath + "res/assets/js/slidebars/slidebars.min.js", function () {
//    $(document).ready(function() {
//        var mySlidebars = new $.slidebars();
//        $('.toggle-left').on('click', function() {
//            mySlidebars.toggle('right');

//            //每次打开和关闭右滑菜单之后，左侧菜单会往下掉10个像素
//            var leftMenuTop =  $("#skin-select").offset().top;
//            if (leftMenuTop != 0) {
//                $("#skin-select").css("margin-top", -leftMenuTop + "px");
//            }
//        });
//    });
//}); 

//TOOL TIP
head.js(_sysPath + "res/assets/js/tip/jquery.tooltipster.js", function () {
    $('.tooltip-tip-x').tooltipster({
        position: 'right'

    });
    $('.tooltip-tip').tooltipster({
        position: 'right',
        animation: 'slide',
        theme: '.tooltipster-shadow',
        delay: 1,
        offsetX: '-12px',
        onlyOne: true

    });
    $('.tooltip-tip4').tooltipster({
        position: 'right',
        animation: 'slide',
        offsetX: '-36px',
        theme: '.tooltipster-shadow',
        onlyOne: true
    });
    $('.tooltip-tip3').tooltipster({
        position: 'right',
        animation: 'slide',
        offsetX: '-24px',
        theme: '.tooltipster-shadow',
        onlyOne: true
    });

    $('.tooltip-tip2').tooltipster({
        position: 'right',
        animation: 'slide',
        offsetX: '-12px',
        theme: '.tooltipster-shadow',
        onlyOne: true
    });
    $('.tooltip-top').tooltipster({
        position: 'top'
    });
    $('.tooltip-right').tooltipster({
        position: 'right'
    });
    $('.tooltip-left').tooltipster({
        position: 'left'
    });
    $('.tooltip-bottom').tooltipster({
        position: 'bottom'
    });
    $('.tooltip-reload').tooltipster({
        position: 'right',
        theme: '.tooltipster-white',
        animation: 'fade'
    });
    $('.tooltip-fullscreen').tooltipster({
        position: 'left',
        theme: '.tooltipster-white',
        animation: 'fade'
    });
});

//NICE SCROLL
head.js(_sysPath + "res/assets/js/nano/jquery.nanoscroller.js", function () {
    $(".nano").nanoScroller({
        //stop: true 
        scroll: 'top',
        scrollTop: 0,
        sliderMinHeight: 40,
        preventPageScrolling: true
        //alwaysVisible: false
    });
});

//PAGE LOADER
head.js(_sysPath + "res/assets/js/pace/pace.js", function () {
    paceOptions = {
        ajax: false, // disabled
        document: false, // disabled
        eventLag: false, // disabled
        elements: {
            selectors: ['.my-page']
        }
    };
});

head.js(_sysPath + "res/assets/js/gage/raphael.2.1.0.min.js", _sysPath + "res/assets/js/gage/justgage.js", function () {
    var g1;
    window.onload = function() {
        var g1 = new JustGage({
            id: "g1",
            value: getRandomInt(0, 1000),
            min: 0,
            max: 1000,
            relativeGaugeSize: true,
            gaugeColor: "rgba(0,0,0,0.4)",
            levelColors: "#0DB8DF",
            labelFontColor : "#ffffff",
            titleFontColor: "#ffffff",
            valueFontColor :"#ffffff",
            label: "VISITORS",
            gaugeWidthScale: 0.2,
            donut: true
        });
    };
});
