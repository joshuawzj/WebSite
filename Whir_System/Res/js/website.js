// JavaScript Document

//Scrollbar
SetHeight();

window.onresize = function() {
	//SetHeight()；
};

function SetHeight() {
	var height = jQuery(window).height() - 95;
	jQuery("#MainArea").attr("height", height);
	jQuery(".menu").css("height", height);
	jQuery(".div_height_filetree2").css("height", height);
	jQuery("#column_tree").css("height", height); //栏目树
	jQuery("#subsite_tree").css("height", height); //子站树
	jQuery("#subject_tree").css("height", height); //专题树
}

//text
jQuery(function() {
	jQuery(":text:not([readonly])").focus(function() {
		jQuery(":text").removeClass("text_common_hover");
		jQuery(this).addClass("text_common_hover");
	});

});
//textarea
jQuery(function() {
    jQuery("textarea:not([readonly])").click(function () {/*表格异行加底色*/
		jQuery("textarea").removeClass("textarea_hover");
		jQuery(this).addClass("textarea_hover");
	});
});

//list table
function SetTrClass() {
	jQuery(".All_list .trClass th:last").attr("class", "thEnd");

	jQuery(".All_list tr:odd").addClass("tdColor");

	jQuery(".All_list tr:even").addClass("tdBgColor");
}

SetTrClass(); /*初始加载*/

/*清除表格异行加底色*/
function ClearTrClass() {
	jQuery(".All_list .trClass th:last").attr("class", "thEnd");
	jQuery(".All_list tr:visible").removeClass("tdBgColor").removeClass("tdColor");
}
/*重新为可见行加底色*/
function RestTrClass() {
	ClearTrClass();
	jQuery(".All_list tr:visible:odd").addClass("tdColor");
	jQuery(".All_list tr:visible:even").addClass("tdBgColor");
}


jQuery(".All_list tr").mouseover(function() {
	$(this).addClass("tdMoveColor");
});

jQuery(".All_list tr").mouseout(function() {
	$(this).removeClass("tdMoveColor");
	$(this).addClass("tdColor");
});
//pages disabled
jQuery(function () {
    $(".pages a").each(function (i, item) {
        var disabled = $(this).attr("disabled");
        if (disabled == 'disabled') {
            jQuery(this).css("color", "#808080");
            jQuery(this).css("cursor", "default");

        }
    });
    //此代码有问题，checkbox初始化时有disabled时，jquery改变disabled后，鼠标无法选中
    //$("*[disabled]").unbind("click").css("color", "#808080").css("cursor", "default").bind("click", function () {
    //    return false;
    //}); //.removeAttr("disabled");
});
	
	

