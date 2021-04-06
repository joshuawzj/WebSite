<%@ Page Language="C#" AutoEventWireup="true" CodeFile="generationwebsites.aspx.cs"
    Inherits="Whir_System_Module_Release_generationwebsites" %>

<%@ Import Namespace="Whir.Language" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script type="text/javascript" src="<%=SysPath %>Res/assets/js/jquery.js"></script>
    <link rel="stylesheet" href="<%=SysPath %>Res/assets/css/style.css"/>
    <link rel="stylesheet" type="text/css" href="<%=SysPath %>Res/css/css_whir_v450.css" />
    <link rel="stylesheet" href="<%=SysPath %>Res/assets/css/loader-style.css"/>
    <link rel="stylesheet" href="<%=SysPath %>Res/assets/css/bootstrap.css"/>
    <link rel="stylesheet" href="<%=SysPath %>Res/assets/js/toastr-master/toastr.css" />
    <link rel="stylesheet" href="<%=SysPath %>Res/assets/js/iCheck/flat/all.css" />
    <link href="<%=SysPath %>Res/assets/js/tree/jquery.treeview.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="<%=SysPath%>Res/assets/js/progress-bar/number-pb.css" />
    <script src="<%=SysPath %>Res/assets/js/tree/bootstrap-treeview.js" type="text/javascript"></script>
    <script type="text/javascript" src="<%=SysPath %>Res/assets/js/iCheck/jquery.icheck.js"></script>
    <script type="text/javascript" src="<%=SysPath %>res/js/whir/whir.ui.js" lang="<%=GetLoginUserLanguageType() %>"></script>
    <script type="text/javascript" src="<%=SysPath %>res/js/commons.js"></script>
    <script type="text/javascript" src="<%=SysPath %>Res/assets/js/toastr-master/toastr.js"></script>
    <script type="text/javascript" src="<%=SysPath%>Res/assets/js/progress-bar/number-pb.js"></script>
    <script type="text/javascript" src="<%=SysPath%>Res/assets/js/progress-bar/src/jquery.velocity.min.js"></script>

    <%-- ReSharper disable once DuplicatingLocalDeclaration --%>
    <script type="text/javascript">

        //选择的栏目Id
        var columnids = "";

        $(document).ready(function () {

            //美化checkbox
            whir.checkbox.destroy();
            whir.skin.checkbox();

            var defaultData = [];
            var currentsiteid = "<%=CurrentSiteId %>";

            whir.checkbox.destroy();
            whir.skin.checkbox();

            $.get("<%=SysPath %>ajax/content/column_generats.aspx?siteid=" + currentsiteid + "&time=" + new Date().getMilliseconds(), "", function (data) {

                defaultData = eval(data);

                //初始化控件
                var $checkableTree = $('#treeview-checkable').treeview1({
                    data: defaultData,
                    showIcon: false,
                    showCheckbox: true,
                    onNodeChecked: function (event, node) {
                        columnids = "";
                        $checkableTree.treeview1('checkChildNode', [node.nodeId, { silent: true }]);
                        $($('#treeview-checkable').treeview1('getChecked')).each(function (id, checkeNode) {
                            columnids += checkeNode.columnid + ',';
                        });
                    },
                    onNodeUnchecked: function (event, node) {
                        columnids = "";
                        $checkableTree.treeview1('unCheckChildNode', [node.nodeId, { silent: true }]);
                        $($('#treeview-checkable').treeview1('getChecked')).each(function (id, checkeNode) {
                            columnids += checkeNode.columnid + ',';
                        });
                    }
                });

                //全选
                $('#btn-check-all').on('click', function (e) {
                    $checkableTree.treeview1('checkAll', { silent: $('#chk-check-silent').is(':checked') });
                });

                //反选
                $('#btn-uncheck-all').on('click', function (e) {
                    $checkableTree.treeview1('uncheckAll', { silent: $('#chk-check-silent').is(':checked') });
                });

                //全部展开
                $('#btn-expand-all').on('click', function (e) {
                    var levels = 99;//展开99层菜单，已经非常大了
                    $checkableTree.treeview1('expandAll', { levels: levels, silent: $('#chk-expand-silent').is(':checked') });
                });

                //全部折叠
                $('#btn-collapse-all').on('click', function (e) {
                    $checkableTree.treeview1('collapseAll', { silent: $('#chk-expand-silent').is(':checked') });
                });

                //默认全部折叠
                $('#btn-collapse-all').click();
            });


        });

    </script>
</head>
<body class="wap-nobody-css">
    <input type="hidden" id="select-columnid" />
    <div class="panel">
        <div class="panel-body">
            <div class="row">
                <div class="col-md-3">
                    <div class="panel panel-default " style="height: 100%">
                        <div class="panel-heading">
                            <a id="btn-expand-all" class="aLink" href="javascript:;">
                                <b><%="全部展开".ToLang()%></b> </a>
                            <a id="btn-collapse-all" class="aLink" href="javascript:;">
                                <b><%="全部折叠".ToLang()%></b> </a>
                            <a id="btn-check-all" class="aLink" href="javascript:;">
                                <b><%="全选".ToLang()%></b> </a>
                            <a id="btn-uncheck-all" class="aLink" href="javascript:;">
                                <b><%="取消".ToLang()%></b></a>
                        </div>
                        <div id="treeview-checkable" class="">
                        </div>
                    </div>
                </div>
                <div class="col-md-9">
                    <div class="panel panel-default " style="height: 100%">
                        <div class="panel-heading">
                            <%="生成内容".ToLang()%>
                        </div>
                        <div class="panel-body">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <a class="btn btn-info" href="javascript:;" onclick="publish();">
                                            <b><%="生成所选栏目".ToLang()%></b></a>
                                        <a class="btn btn-info" href="javascript:;" onclick="publish(true);">
                                            <b><%="生成整站".ToLang()%></b></a>
                                        <a class="btn btn-info" href="javascript:;" onclick="publishIndex();">
                                            <b><%="生成网站首页".ToLang()%></b></a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                        <ul class="list">
                                            <li>
                                                <input type="checkbox" checked="checked" id="cbxHome" />
                                                <label for="cbxDefault">
                                                    <%="网站首页".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="checkbox" checked="checked" id="cbxDefault" />
                                                <label for="cbxList">
                                                    <%="栏目首页".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="checkbox" checked="checked" id="cbxList" />
                                                <label for="cbxContent">
                                                    <%="栏目列表页".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="checkbox" checked="checked" id="cbxContent" />
                                                <label for="cbxAttach">
                                                    <%="栏目内容页".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="checkbox" checked="checked" id="cbxInclude" />
                                                <label for="cbxInclude">
                                                    <%="公共文件".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="checkbox" id="cbxAttach" />
                                                <label for="cbxAttach">
                                                    <%="附带发布".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="checkbox" id="cbxSubColumns" />
                                                <label for="cbxSubColumns">
                                                    <%="子站".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="checkbox" id="cbxHtml" />
                                                <label for="cbxHtml">
                                                    <%="静态页".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div class="number-pb">
                                            <div class="number-pb-shown"></div>
                                            <div class="number-pb-num">0%</div>
                                        </div>
                                        <div id="publishProgress" style="float: left;">
                                        </div>
                                        <img id="progressImg" src="<%=SysPath%>res/images/loading_r.gif" width="25" style="display: none; float: left;" />
                                    </td>
                                </tr>
                            </table>
                            <div class="publish_status" id="publishStatus" style="overflow: auto; height: 100%;">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        //栏目发布文件存放路径
        var publicFilePath = "<%= UploadFilePath %>Release/publicColumns.xml";
        //初始化进度条
        var IsProgress = false;
        var IsJurisdiction = false;
        var bars = $('.number-pb').NumberProgressBar({
            style: 'percentage'
        });

        function initProgress(progress, callback) {
            var num;
            if (progress < 0) {
                num = 0;
            } else if (progress > 100) {
                num = 100;
            } else {
                num = progress;
            }
            bars.reach(num, callback);
        }

        $(function () {
            initPublish();
            //initProgress(0);
        });

        //获取当前时间字符串
        function getNowDateString() {
            var now = new Date();
            var year = now.getFullYear();
            var month = now.getMonth() + 1;
            var date = now.getDate();
            var hour = now.getHours();
            var minutes = now.getMinutes();
            var seconds = now.getSeconds();
            var milliseconds = now.getMilliseconds();
            var initDate = year
                + "-"
                + (month < 10 ? ("0" + month) : month)
                + "-"
                + (date < 10 ? ("0" + date) : date)
                + "&nbsp;"
                + (hour < 10 ? ("0" + hour) : hour)
                + ":"
                + (minutes < 10 ? ("0" + minutes) : minutes)
                + ":"
                + (seconds < 10 ? ("0" + seconds) : seconds)
                + "."
                + milliseconds;
            return "<span>[" + initDate + "]</span>";
        }

        //初始化发布状态框的内容
        function initPublish() {
            var initText = getNowDateString() + '-<%="暂未发布".ToLang() %>';
            $("#publishStatus").html(initText);
        }

        //发布
        function publish(isAllSite) {

            $(".aLink").attr({ "disabled": "disabled" });
            var selectedColumns = columnids;
            if ($.trim(selectedColumns).length > 0) {
                selectedColumns = selectedColumns.substring(0, selectedColumns.length - 1);
            }
            else if (!isAllSite) {
                window.parent.whir.toastr.error('<%="请选择栏目".ToLang() %>');
                return;
            }

            var siteId = '<%= CurrentSiteId %>';
            var isHome = $("#cbxHome:checked").length;
            var isDefault = $("#cbxDefault:checked").length;
            var isList = $("#cbxList:checked").length;
            var isContent = $("#cbxContent:checked").length;
            var isAttach = $("#cbxAttach:checked").length;
            var isInclude = $("#cbxInclude:checked").length;
            var isSubColumns = $("#cbxSubColumns:checked").length;
            var isHtml = $("#cbxHtml:checked").length;

            if (!isAllSite) {
                if ((isDefault || isList || isContent) && selectedColumns.length <= 0) {
                    window.parent.whir.toastr.error('<%="请选择栏目".ToLang() %>');
                    return;
                }
                if ($(":checked").length == 0) {
                    window.parent.whir.toastr.error('<%="请选择生成包含项".ToLang() %>');
                    return;
                }
            } else {
                //整站生成
                selectedColumns = 0;
            }
            setTimeout(
                initProgress(0), 100);

            currentIndex = 0;
            $("#publishStatus").html("");
            $(".aLink").attr({ "disabled": "disabled" });
            $("#progressImg").show();
            var requestUrl = "<%=SysPath%>ajax/publish/publish.aspx";
            var requestData = {
                columnID: selectedColumns,
                siteID: siteId,
                isHome: isHome,
                isDefault: isDefault,
                isList: isList,
                isContent: isContent,
                isAttach: isAttach,
                isInclude: isInclude,
                isSubColumns: isSubColumns,
                isHtml: isHtml,
                time: new Date().getMilliseconds()
            };

            $.ajax({
                url: requestUrl,
                type: "post",
                data: requestData,
                cache: false,
                success: function (result) {
                    //不用等待处理结果，处理结果用ajax直接访问发布文件
                    if (result.indexOf("没有权限") != -1) {
                        window.parent.whir.toastr.warning(result);
                        return;
                    } else {
                        IsJurisdiction = true;
                    }
                },
                error: function () {
                    window.parent.whir.toastr.error('<%="请求发生错误".ToLang() %>');
                }
            });

            IsJurisdiction = true;
            //异步读取状态
            var t = setTimeout('getPublishStatus()', 3000);

        }

        var timeout;
        //获取发布状态
        var i = 0;
        var index = 0;
        var timeIndex = 0;
        var currentIndex = 0;
        var tryTimes = 0;
        function getPublishStatus() {

            if (IsJurisdiction) {

                //开始异步请求
                $.ajax({
                    url: publicFilePath + "?t=" + new Date().getMilliseconds(),
                    dataType: 'xml',
                    cache: false,
                    success: function (result) {
                        if (result == null) {
                            setTimeout('getPublishStatus()', 200);
                            return true;
                        }
                        var items = $(result).find("item");
                        var lastItem = items.eq(items.length - 1);
                        var lastCount = lastItem.find("count").text();
                        var lastIndex = lastItem.find("index").text();
                        var length = items.length;
                        if (length <= index) {
                            setTimeout('getPublishStatus()', 200);
                            return true;
                        }

                        $(items).each(function (idx, item) {
                            if (index > 0 && idx < index) {
                                return true;
                            }
                            var count = $(item).find("count").text();
                            var message = $(item).find("message").text();
                            var inner = getNowDateString() + " - " + message;
                            currentIndex = parseInt($(item).find("index").text());

                            var previewUrl = $(item).find("previewUrl");
                            if (previewUrl.length > 0) {
                                previewUrl = ' <a href="' + previewUrl.text() + '" target="_blank">[<%="预览".ToLang() %>]</a>';
                                inner += previewUrl;
                            }
                            inner += "<br />";
                            index = index + 1;
                            //进度条
                            var progress = parseInt(currentIndex / lastCount * 100);
                            var time = parseInt(timeIndex) * 200;
                            timeIndex = timeIndex + 1;

                            setTimeout(function () {
                                i = i + 1;
                                initProgress(progress);
                                $("#publishStatus").prepend(inner);
                                if (parseInt(index) == parseInt(i)) {
                                    if (parseInt(lastIndex) < parseInt(lastCount)) {
                                        timeIndex = 0;
                                        setTimeout('getPublishStatus()', 500);
                                    }
                                    if (parseInt(lastIndex) == parseInt(lastCount)) {
                                        clearTimeout(timeout);
                                        var finish = getNowDateString() + ' - <em><%="发布完成".ToLang() %></em><br />';
                                    $("#publishStatus").prepend(finish);
                                    $(".aLink").removeAttr("disabled");
                                    $("#progressImg").hide();
                                    window.parent.whir.toastr.success('<%="发布成功".ToLang()%>');
                                    i = 0;
                                    index = 0;
                                    timeIndex = 0;
                                }
                            }
                        }, time);

                        });

                    },
                    error: function () {
                        tryTimes++;
                        if (tryTimes < 10) {
                            setTimeout('getPublishStatus()', 3000);
                        }
                        else
                            window.parent.whir.toastr.error('<%="发布失败".ToLang()%>');
                    }
                });
                //异步请求结束
            }
        }

        //发布
        function publishIndex() {
            $(".aLink").attr({ "disabled": "disabled" });
            $("#progressImg").show();
            $("#publishStatus").text("");
            initProgress(0);

            var requestUrl = "<%=SysPath%>ajax/publish/publish.aspx";
            var requestData = {
                siteID: '<%= CurrentSiteId %>',
                isHtml: $("#cbxHtml:checked").length,
                time: new Date().getMilliseconds()
            };
            $.ajax({
                url: requestUrl,
                data: requestData,
                cache: false,
                success: function (result) {
                    if (result.indexOf("没有权限") != -1) {
                        window.parent.whir.toastr.warning(result);
                        return;
                    } else {
                        IsJurisdiction = true;
                        $("#progressImg").hide();
                        getIndexPublishStatus();
                    }
                },
                error: function () {
                    window.parent.whir.toastr.error('<%="请求发生错误".ToLang()%>');
                }
            });
        }

        function getIndexPublishStatus() {
            if (IsJurisdiction) {
                //开始异步请求
                $.ajax({
                    url: publicFilePath + "?t=" + new Date().getMilliseconds(),
                    dataType: 'xml',
                    cache: false,
                    success: function (result) {
                        var items = $(result).find("item");
                        var lastItem = items.eq(items.length - 1);
                        var lastCount = lastItem.find("count").text();
                        var lastIndex = lastItem.find("index").text();
                        $(items).each(function (idx, item) {
                            var count = $(item).find("count").text();
                            var index = $(item).find("index").text();
                            var message = $(item).find("message").text();
                            var inner = getNowDateString() + " - " + message;
                            var previewUrl = $(item).find("previewUrl");
                            if (previewUrl.length > 0) {
                                previewUrl = ' <a href="' + previewUrl.text() + '" target="_blank">[<%="预览".ToLang() %>]</a>';
                                inner += previewUrl;
                            }

                            inner += "<br />";

                            //进度条
                            var progress = parseInt(index / count * 100);
                            var time = parseInt(index) * 500;
                            setTimeout(function () {
                                $("#publishStatus").prepend(inner);
                                initProgress(progress);
                                if (progress == 100) {
                                    $("#publishStatus").prepend(inner);
                                    clearTimeout(timeout);
                                    var finish = getNowDateString() + ' - <em><%="发布完成".ToLang()%></em><br />';
                                $("#publishStatus").prepend(finish);
                                $(".aLink").removeAttr("disabled");
                                $("#progressImg").hide();
                                window.parent.whir.toastr.success('<%="发布成功".ToLang()%>');
                            }
                            else {
                                if (parseInt(lastIndex) < parseInt(lastCount)) {
                                    getPublishStatus();
                                }
                            }

                        }, time);

                        });

                    }
                });

            }
        }
        //移动端
        $(function ($) {
            $(window).on("resize", function (e) {
                if ($(window).width() <= 640) {
                    //alert(1)
                    setTimeout(function () {
                        $("#btn-collapse-all").click();
                    }, 500);
                }
            }).trigger("resize");
        });
    </script>
</body>
</html>
