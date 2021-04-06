var editdiv = "<div class=\"temp_edit_div\"></div>" //遮层
var edithtml = "<div class=\"temp_edit_btn\"><a href=\"javascript:void(0);\" onclick=\"editArea(this)\" id=\"edit_a_{id}\">编辑</a></div>" //编辑文字

//页面加载
$(function() {
    screenALink(); //屏蔽编辑页面上的链接

    // --------------- 模板上的编辑按钮管理
    $("div[class='temp_edit']").mouseover(function() { overStyle($(this).get(0)) }).mouseleave(function() { outStyle() });
});


//鼠标经过的区域显示
var overStyle = function(obj) {
    var id = $(obj).attr("targetid"), html = edithtml.replace(/{id}/g, id);
    html += editdiv;
    $(html).prependTo(obj);
}

//鼠标离开的区域显示
var outStyle = function() {
    $(".temp_edit_div").remove();
    $(".temp_edit_btn").remove();
}


//屏蔽编辑页面上的链接
var screenALink = function() {
    //屏蔽所有链接
    $("a").attr("href", "javascript:void(0);").attr("target", "");
    //屏蔽所有按钮
    $("input").attr("disabled", "true");
};

var editArea = function(obj) {
    var id = obj.id;
    id = id.replace("edit_a_", "");
    var windowWidth = $("div[targetid='" + id + "']").width();
    var windowHeight = $("div[targetid='" + id + "']").height();

    parent.editTemp(id, windowWidth, windowHeight);
};
