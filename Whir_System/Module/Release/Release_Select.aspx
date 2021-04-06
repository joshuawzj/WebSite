<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="Release_Select.aspx.cs" Inherits="Whir_System_Module_Release_Release_Select" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="<%=SysPath%>Res/assets/js/progress-bar/number-pb.css">
    <script type="text/javascript" src="<%=SysPath%>Res/assets/js/progress-bar/number-pb.js"></script>
    <script type="text/javascript" src="<%=SysPath%>Res/assets/js/progress-bar/src/jquery.velocity.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="alert alert-danger no-opacity">
            <button data-dismiss="alert" class="close" type="button">
                ×</button>
            <span class="entypo-attention"></span><span>
                <%="提示： 本次生成的页面将覆盖原有页面，确认生成页面吗？".ToLang()%></span>
        </div>

        <div class="form-group ">
            <ul class="list" style="padding-right: 0px;">
                <li>
                    <input type="checkbox" checked="checked" id="cbxDefault" />
                    <label for="cbxDefault">
                        <%="发布栏目首页".ToLang()%></label>
                </li>
                <li>
                    <input type="checkbox" checked="checked" id="cbxList" />
                    <label for="cbxList">
                        <%="发布列表页".ToLang()%></label>
                </li>
                <li>
                    <input type="checkbox" checked="checked" id="cbxContent" />
                    <label for="cbxContent">
                        <%="发布详细页".ToLang()%></label>
                </li>
                <li>
                    <input type="checkbox" checked="checked" id="cbxAttach" />
                    <label for="cbxAttach">
                        <%="附带发布".ToLang()%></label>
                </li>
                <li>
                    <input type="checkbox" checked="checked" id="cbxInclude" />
                    <label for="cbxInclude">
                        <%="发布公共文件".ToLang()%></label>
                </li>
                <li>
                    <input type="checkbox" id="cbxHtml" />
                    <label for="cbxHtml">
                        <%="静态页".ToLang()%></label>
                </li>
                <li><a class="btn btn-white" onclick="publish();">
                    <%="开始生成".ToLang()%></a> </li>
            </ul>
        </div>
        <div class="form-group  " style="padding-top: 40px;">
            <div class="number-pb">
                <div class="number-pb-shown"></div>
                <div class="number-pb-num">0%</div>
                <em class="img_progress">
                    <img id="progressImg" src="<%=SysPath%>res/images/loading_r.gif" width="25" style="display: none; float: right; margin-right: -50px; margin-top: -10px;" /></em>
            </div>
        </div>
        <div class="form-group">
            <div id="publishStatus" class="publish_status" style="padding-top: 5px;"></div>
        </div>
    </div>
    <script type="text/javascript">

        $(function () {
            initPublish();
            //initProgress(0);
        });
        var IsProgress = false;
        var bars = $('.number-pb').NumberProgressBar({
            style: 'percentage'
        })
        //初始化进度条
        function initProgress(progress, callback) {
            if (progress < 0) {
                num = 0;
            } else if (progress > 100) {
                num = 100;
            } else {
                num = progress
            }
            bars.reach(num, callback);
        }

        //初始化发布状态框的内容
        function initPublish() {
            var initText = getNowDateString() + ' -<%="暂未发布".ToLang() %>';
            $("#publishStatus").html(initText);
        }

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

    </script>
    <script type="text/javascript">
        //发布
        function publish() {
            var selectedColumns = '<%= ColumnId %>';
            var selectedMenu = '<%= MenuId %>';
            var classId = '<%=ClassId %>';
            var siteID = '<%= CurrentSiteId %>';
            var isDefault = $("#cbxDefault:checked").length;
            var isList = $("#cbxList:checked").length;
            var isContent = $("#cbxContent:checked").length;
            var isAttach = $("#cbxAttach:checked").length;
            var isInclude = $("#cbxInclude:checked").length;
            var isHtml = $("#cbxHtml:checked").length;
            if ((isDefault || isList || isContent) && selectedColumns.length <= 0) {
                TipMessage('<%="请选择栏目".ToLang() %>');
                return;
            }
            if ($(":checked").length == 0) {
                TipMessage('<%="请选择生成包含项".ToLang() %>');
                return;
            }
            initProgress(0);

            $("#publishStatus").html("");

            $(".btn_bold").attr({ "disabled": "disabled" });
            $("#progressImg").show();

            var requestUrl = "<%=SysPath%>ajax/publish/publish_singleColumn.aspx"; //发布单个栏目，包括其子栏目
            var requestData = {
                columnID: selectedColumns,
                menuID: selectedMenu,
                classID: classId,
                siteID: siteID,
                isHome: 0,
                isDefault: isDefault,
                isList: isList,
                isContent: isContent,
                isAttach: isAttach,
                isInclude: isInclude,
                isHtml: isHtml,
                time: new Date().getMilliseconds()
            };
            $.ajax({
                url: requestUrl,
                data: requestData,
                cache: false,
                success: function (result) {
                    if (result.indexOf("没有权限") != -1) {
                        window.parent.whir.toastr.warning(result);
                    } else {
                        getPublishStatus(result);
                    }
                },
                error: function () {
                    TipError('<%="请求发生错误".ToLang() %>');
                }
            });
        }
        var publicFilePath = "<%= UploadFilePath %>Release/publicColumns.xml";

        var timeout;
        //获取发布状态
        var i = 0;
        var index = 0;
        var timeIndex = 0;
        var currentIndex = 0;
        var tryTimes = 0;
        function getPublishStatus() {

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
    </script>
</asp:Content>
