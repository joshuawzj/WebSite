<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="generationhomepage.aspx.cs" Inherits="whir_system_module_release_generationhomepage" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath %>res/js/progressbar/jquery.progressbar.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <h4 class="font_title">
            <%="生成网站首页".ToLang()%></h4>
        <div class="All_table">
            <table width="100%">
                <tr>
                    <td class="item" width="<%=LanguageHelper.GetSplitValue("100px|100px|190px") %>">
                        <%="网站首页模板：".ToLang()%>
                    </td>
                    <td>
                        <asp:Literal ID="litDefaultTemp" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td class="item">
                        <%="生成存放路径：".ToLang()%>
                    </td>
                    <td class="button_submit_div">
                        <asp:Literal ID="litPublishPath" runat="server"></asp:Literal>
                        <a class="aLink" onclick="publish();"><b>
                            <%="生成网站首页".ToLang()%></b></a>
                    </td>
                </tr>
                <tr>
                    <td class="item" valign="top">
                        <%="生成结果：".ToLang()%>
                    </td>
                    <td>
                        <div id="publishProgress" style="float:left;">
                        </div>
                        <img id="progressImg" src="<%=SysPath%>res/images/loading_r.gif" width="25" style="display:none; float:left;" />
                        <div id="publishStatus" class="publish_status">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script type="text/javascript">
        $(function () {
            autoHeight();
            window.onresize = function () { autoHeight(); };

            initPublish();
            initProgress(0);
        });

        //自动高度
        function autoHeight() {
            var doc_height = $(document).height();

            var status_height = doc_height - 250;
            $("#publishStatus").height(status_height);
        }

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
            var initText = getNowDateString() + " -" + '<%="暂未发布".ToLang()%>';
            $("#publishStatus").html(initText);
        }

        //初始化进度条
        function initProgress(progress, callback) {
            $("#publishProgress").progressBar(progress, {
                width: "500",
                height: "20",
                boxImage: "<%=SysPath%>res/js/progressbar/images/progressbar.gif",
                barImage: {
                    0: "<%=SysPath%>res/js/progressbar/images/progressbg_red.gif",
                    30: "<%=SysPath%>res/js/progressbar/images/progressbg_orange.gif",
                    70: "<%=SysPath%>res/js/progressbar/images/progressbg_green.gif"
                },
                callback: callback
            });
        }
    </script>
    <script type="text/javascript">

        //发布
        function publish() {
            $(".aLink").attr({ "disabled": "disabled" });
            $("#progressImg").show();

            var requestUrl = "<%=SysPath%>ajax/publish/publish.aspx";
            var requestData = {
                siteID: '<%= CurrentSiteId %>',
                time: new Date().getMilliseconds()
            };
            $.ajax({
                url: requestUrl,
                data: requestData,
                cache: false,
                success: function (result) {
                    getPublishStatus(result);
                },
                error: function () {
                    TipError('<%="请求发生错误".ToLang()%>');
                }
            });
        }

        var timeout;
        //获取发布状态
        function getPublishStatus(xmlPath) {

            initProgress(0);
            $("#publishStatus").html("");

            setTimeout(function () {

                //开始异步请求
                $.ajax({
                    url: xmlPath,
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
                            initProgress(progress, function () {
                                $("#publishStatus").prepend(inner);
                            });

                        });

                        if (lastIndex < lastCount) {
                            timeout = setTimeout(function () {
                                getPublishStatus(xmlPath);
                            }, 100);
                        } else {
                            clearTimeout(timeout);
                            var finish = getNowDateString() + ' - <em><%="发布完成".ToLang()%></em><br />';
                            $("#publishStatus").prepend(finish);
                            $(".aLink").removeAttr("disabled");
                            $("#progressImg").hide();
                        }
                    },
                    error: function () { }
                });
                //异步请求结束
            }, 500);
        }
    </script>
</asp:Content>
