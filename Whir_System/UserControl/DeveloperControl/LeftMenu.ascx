<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LeftMenu.ascx.cs"
    Inherits="Whir_System_UserControl_DeveloperControl_LeftMenu" %>
<%@ Import Namespace="Whir.Language" %>
<script type="text/javascript">
    var subjectTypeId = "0";
    var subjectId = "0";
    var columnId = "0";
    var classId = "0";
    var currentColumnId = <%=Whir.Framework.RequestUtil.Instance.GetQueryInt("columnId",0)%>;
    var currentSubjectId = <%=Whir.Framework.RequestUtil.Instance.GetQueryInt("subjectId",0)%>;
    //右击div显示的位置
    function showRMenu(x, y) {
        $("#rMenu").show();
        $(".menuStyle").show();

        //判断当前点击的位置 菜单采用absolute定位，点击的位置相对菜单（第一个父元素）距离窗口顶部的高度  高度为115
        $("#rMenu").css({ "top": (y - 115) + "px", "left": (x + 30) + "px", "visibility": "visible" });

        $("body").bind("mousedown", onBodyMouseDown);
        try {
            $(document.getElementById("MainArea").contentWindow.document.body).bind("mousedown", onBodyMouseDown);
        }
        catch (e) {
            //跨域时无法访问iframe里的内容，拒绝访问。
        }
        $("#rMenu li").removeClass();
        //右键菜单样式
        $("#rMenu li").mouseover(function () {
            $("#rMenu li").removeClass();
            $(this).addClass("itemHoverStyle");
        });
    }
    function onBodyMouseDown(event) {
        if (!(event.target.id === "rMenu" || $(event.target).parents("#rMenu").length > 0)) {
            $("#rMenu").hide();
        }
    }

    //查看前台
    function visit() {
        window.open("<%= SysPath%>ModuleMark/Common/ViewPage.aspx?columnid=" + columnId + "&subjectid=" + subjectId + "&time=" + new Date().getMilliseconds());
    }

    //发布
    function publise() {
        columnRelease();
    }

    //发布栏目
    function columnRelease() {
        var url = "";
        if (subjectTypeId == "1" || subjectTypeId == "2") { //模版子站、自定义子站
            url = "<%=SysPath %>Module/Release/Release_Select.aspx?columnid=" +
                columnId +
                "&classid=" +
                classId +
                "&menuid=" +
                subjectTypeId +
                "&time=" +
                new Date().getMilliseconds();
        } else { //内容管理
            url = "<%=SysPath %>Module/Release/Release_Select.aspx?columnid=" +
                columnId +
                "&menuid=" +
                subjectTypeId +
                "&time=" +
                new Date().getMilliseconds();
        }

        whir.dialog.frame('<%="发布栏目".ToLang()%>', url, null, 900, 350, false);
    }

    //栏目设置
    function colomnSet() {
        var url = "";
        if (subjectTypeId == "1") {   //子站
            url = "<%=SysPath %>Module/Subject/SubjectColumn_Edit.aspx?subjecttypeid=1&subjectclassid=" +
                classId +
                "&subjectid=" +
                subjectId +
                "&columnid=" +
                columnId +
                "&time=" +
                new Date().getMilliseconds();

        } else if (subjectTypeId == "2") { //专题
            url = "<%=SysPath %>Module/Subject/SubjectColumn_Edit.aspx?subjecttypeid=2&subjectclassid=" +
                classId +
                "&subjectid=" +
                subjectId +
                "&columnid=" +
                columnId +
                "&time=" +
                new Date().getMilliseconds();
        } else {                         //内容
            url = "<%=SysPath %>Module/Column/ColumnEdit.aspx?columnid=" +
                columnId +
                "&time=" +
                new Date().getMilliseconds();
        }
    window.location.href = url;
}

//添加外部链接
function link() {
    window.location.href = "<%=SysPath %>Module/Column/Column_Link.aspx?columnid=" + columnId + "&time=" + new Date().getMilliseconds() + "&flagtype=outlink";
    }
    //表单管理
    function form() {
        var url = "";
        if (subjectTypeId == "1") {
            url = "<%=SysPath %>Module/Subject/SubjectFormlist.aspx?subjectclassid=" +
                classId +
                "&subjecttypeid=1&columnid=" +
                columnId +
                "&time=" +
                new Date().getMilliseconds();
        } else if (subjectTypeId == "2") {
            url = "<%=SysPath %>Module/Subject/SubjectFormlist.aspx?subjectclassid=" +
                classId +
                "&subjecttypeid=2&columnid=" +
                columnId +
                "&time=" +
                new Date().getMilliseconds();
        } else {
            url = "<%=SysPath %>Module/Column/Formlist.aspx?columnid=" + columnId + "&time=" + new Date().getMilliseconds();
        }
    window.location.href = url;
}
//隐藏右击弹出框
function rMenuHide() {
    $("#rMenu").hide();
}

//初始化 去掉cookies
$(function () {
    var isMenu = whir.cookie.get("isMenu");
    if (isMenu.indexOf("column") >= 0) {
        subjectTypeId = isMenu.substring(6);
        openColumnMenu(subjectTypeId);
    } else {
        var type = isMenu.substring(4);
        openMenu(type);
        $("#columnMenu").hide();
        $("#sysMenu").show();
    }
});
//搜索菜单
function searchMenu() {
    $("#searchColumn ul").empty();
    var key = $("input[name=search]").val();
    isMenu = whir.cookie.get("isMenu");
    var menuType = "sysMenu"; //默认搜索 系统菜单
    if (isMenu.indexOf("column") >= 0)   //搜索 栏目菜单
        menuType = "columnMenu";
    $("#" + menuType).hide();

    if (key != "") {
        key=key.toLowerCase();
        $("#" + menuType + " li").each(function () {
            var html = $(this).find("a.ajax-load").html();
            var title = $(this).find("a.ajax-load").attr("title");
            var pyname = $(this).find("a.ajax-load").attr("pyname");
            var szmname = $(this).find("a.ajax-load").attr("szmname");
            var path = $(this).find("a.ajax-load").attr("path");
            var isShow = false;
            if (html && html.toLowerCase().indexOf('columnid="' + key + '"') >= 0) {
                isShow = true;
            }
            else  if (title && title.toLowerCase().indexOf(key) >= 0) {
                isShow = true;
            }
            else  if ((szmname && szmname.indexOf(key) >= 0) || (pyname && pyname.indexOf(key) >= 0)) {
                isShow = true;
            }
            else if( path && path.indexOf(key) >= 0){   
                isShow = true;
            }

            if (isShow) {
                var cloneObj = $(this).clone(true);
                cloneObj.find("ul").remove();
                cloneObj.find("h4").remove();
                $("#searchColumn ul").append(cloneObj);
            }
        });
        $("#searchColumn").show();
    } else {
        $("#searchColumn").hide();
        $("#" + menuType).show();
    }
}

//菜单点击
function menuClick() {
    $("span.caret").click(function (event) {
        event.stopPropagation(); //防止冒泡

        $("#visit").hide(); //查看前台
        $("#publise").hide(); //发布
        $("#colomnSet").hide(); //栏目设置
        $("#form").hide(); //表单管理
        $("#link").hide(); //栏目管理

        columnId = $(this).attr("columnId");
        subjectId = $(this).attr("subjectId");
        classId = $(this).attr("classId");
            
        var ids = $(this).attr("buttonIds").split(",");
        for (var i = 0; i < ids.length; i++) {
            switch (parseInt(ids[i])) {
                case 1:
                    $("#visit").show();
                    break;
                case 2:
                    $("#publise").show();
                    break;
                case 3:
                    $("#colomnSet").show();
                    break;
                case 4:
                    //$("#colomnSet").show();
                    break;
                case 5:
                    //$("#colomnSet").show();
                    break;
                case 6:
                    $("#link").show();
                    break;
                case 7:
                    $("#form").show();
                    break;

            }
        }
        showRMenu(getX(event), getY(event));
    });
}

function getX(e) {
    e = e || window.event;
    return e.pageX || e.clientX + document.body.scroolLeft;
}

function getY(e) {
    e = e || window.event;
    return e.pageY || e.clientY + document.boyd.scrollTop;
}

// 0=功能模块、1=系统设置、id=开发菜单、id=商城菜单、id=公众号菜单 
function openMenu(type) {

    $("#searchColumn").hide();
    whir.cookie.set("isMenu", "menu" + type);
    whir.ajax.post("<%=SysPath %>Handler/Developer/Menu.aspx", {
            data: {
                _action: "GetMenu",
                SiteId: '<%=SiteInfo.SiteId%>',
                type: type
            },
            success: function (response) {
                whir.loading.remove();
                if (response.Status == true) {
                    $("#sysMenu").html(response.Message);
                    $("#sysMenu").show();
                    $("#columnMenu").hide();
                } else {
                    whir.toastr.error(response.Message);
                    whir.loading.remove();
                }
            }
        });
    }

    //打开 内容管理、子站、专题 菜单
    function openColumnMenu(type) {
        subjectTypeId = type;
        $("#searchColumn").hide();
        whir.cookie.set("isMenu", "column" + type);
        whir.ajax.post("<%=SysPath %>Handler/Developer/Menu.aspx", {
            data: {
                _action: "GetColumnMenu",
                SiteId: '<%=SiteInfo.SiteId%>',
                SubjectTypeId: type
            },
            success: function (response) {
                whir.loading.remove();
                if (response.Status == true) {
                    $("#columnMenu").html(response.Message);
                    $("#sysMenu").hide();
                    $("#columnMenu").show();
                    menuClick();
                    accordionMenu(); //加载好数据后，再生成折叠
                    setCurrentColumnId();
                } else {
                    whir.toastr.error(response.Message);
                    whir.loading.remove();
                }
            }
        });
    }

    //设置最后点击的栏目Id
    function setLastColumnId(columnId, subjectId) {
        whir.cookie.set("lastColumnMenuId", columnId + "-" + subjectId, 1);
    }

    //设置当前选中栏目，展开上次状态
    function setCurrentColumnId() {
        $("span[columnid='" + currentColumnId + "'][subjectid='" + currentSubjectId + "']").parent().addClass("activeMenu"); //标志当前栏目对应的菜单，显示不一样的底色
        var columnid = whir.cookie.get("lastColumnMenuId"); //cookies 在scriptbreaker-multiple-accordion-1.js 点击事件 写入
        if (columnid.lastIndexOf("column") < 0) {
            $("span[columnid='" + currentColumnId + "'][subjectid='" + currentSubjectId + "']").parents("ul").show();
            $("span[columnid='" + currentColumnId + "'][subjectid='" + currentSubjectId + "']").show();

            setTimeout(function () {
                $("span[columnid='" + currentColumnId + "'][subjectid='" + currentSubjectId + "']").parentsUntil(".topnav").each(function () {
                    if ($(this)[0].tagName == "LI")
                        $(this).children("a").find(".icon-plus").addClass("icon-minus").removeClass("icon-plus");
                });
            }, 200);
        }
        else {
            $("#" + columnid).parents("ul").show();
            $("#" + columnid).next("ul").show();
            setTimeout(function () {
                $("#" + columnid).parentsUntil(".topnav").each(function () {
                    if ($(this)[0].tagName == "LI")
                        $(this).children("a").find(".icon-plus").addClass("icon-minus").removeClass("icon-plus");
                });
            }, 200);
        }
    }


</script>

<div id="rMenu" class="nav navbar-nav navbar-right" style="display: none; position: absolute; z-index: 999;">
    <ul class="menuStyle dropdown-setting dropdown-menu" role="menu">
        <li id="visit" class="itemStyle">
            <a href="javascript:;" onclick="visit();rMenuHide();">
                <span class="glyphicon glyphicon-eye-open"></span>&nbsp;<%="查看前台".ToLang()%>
            </a>
        </li>
        <li id="publise" class="itemStyle">
            <a href="javascript:;" onclick="publise();rMenuHide();">
                <span class="glyphicon glyphicon-download"></span>&nbsp;<%="发布".ToLang()%>
            </a>
        </li>
        <li id="colomnSet" class="itemStyle">
            <a href="javascript:;" onclick="colomnSet();rMenuHide();">
                <span class="glyphicon glyphicon-cog"></span>&nbsp;<%="栏目设置".ToLang()%>
            </a>
        </li>
        <li id="link" class="itemStyle">
            <a href="javascript:;" onclick="link();rMenuHide();">
                <span class="glyphicon glyphicon-pencil"></span>&nbsp;<%="栏目外链".ToLang()%>
            </a>
        </li>
        <li id="form" class="itemStyle">
            <a href="javascript:;" onclick="form();rMenuHide();">
                <span class="glyphicon glyphicon-edit"></span>&nbsp;<%="表单管理".ToLang()%>
            </a>
        </li>
    </ul>
</div>

<div id="sysMenu"></div>
<div id="columnMenu"></div>


