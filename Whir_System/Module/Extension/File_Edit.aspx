<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="file_edit.aspx.cs" Inherits="whir_system_module_extension_file_edit"
    ValidateRequest="false" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%=SysPath %>res/js/templeteEditor/swfobject.js"></script>
    <script type="text/javascript" language="javascript">
        //装载编码的控件客户端Id
        var editorContent = "<%= EditorContent.ClientID %>";
        function encodeURL(str) {
            return encodeURI(str).replace(/=/g, "%3D").replace(/\+/g, "%2B").replace(/\?/g, "%3F").replace(/\&/g, "%26");
        }

        function htmlEncode(str) {
            return str.replace(/&/g, "&amp;").replace(/\"/g, "&quot;").replace(/</g, "&lt;").replace(/>/g, "&gt;").replace(/ /g, "&nbsp;");
        }

        function htmlDecode(str) {
            return str.replace(/\&quot;/g, "\"").replace(/\&lt;/g, "<").replace(/\&gt;/g, ">").replace(/\&nbsp;/g, " ").replace(/\&amp;/g, "&");
        }
        //获取Flash编辑器的代码
        function getText() {
            var val = document.getElementById('ctlFlash').getText(); //.replace("\n", "\r\n")); //.replace(' ', ' ');
            document.getElementById(editorContent).value = val;

        }

        //设置代码到Flash编辑器
        function setText() {
            var val = document.getElementById(editorContent).value.replace(/\r\n/gi, "\r\n");
            document.getElementById('ctlFlash').setText(val);
        }

        function changeMode() {
            var SoucrePanel = document.getElementById("SoucrePanel");
            var DesignPanel = document.getElementById("DesignPanel");

            if (SoucrePanel.style.display == "") {
                showItem(true);
                getText();
                formSubmit();

            } else {

                showItem(false);
            }
        }

        function showItem(falg) {
            SoucrePanel.style.display = "none";
            DesignPanel.style.display = "";
            $("#btnedit").text("代码");
            $("#SoucreTools").hide();
            $("#ControlTools").hide();
            //            DesignPanel.style.display = "none";
            //            SoucrePanel.style.display = "";
            //            if (falg)
            //            {
            //                SoucrePanel.style.display = "none";
            //                DesignPanel.style.display = "";
            //                $("#btnedit").text("代码");
            //                $("#SoucreTools").hide();
            //                $("#ControlTools").hide();
            //            } else
            //            {
            //                DesignPanel.style.display = "none";
            //                SoucrePanel.style.display = "";
            //                $("#btnedit").text("设计");
            //                $("#designform").remove();
            //                $("#SoucreTools").show();
            //                $("#ControlTools").show();
            //                $("#designframe").attr("src", "about:blank"); //清空iframe的内容
            //            }
        }

        function formSubmit() {
            $("#designform").remove();
            var str = "<form id='designform' name='designform' action='file_design.aspx' method='post' target='designframe'>\r\n";
            str += "<input id='FilePath' name='FilePath' type='hidden' value='<%= FilePath%>'/>\r\n";
            str += "</form>\r\n";
            $("body").append(str);
            document.designform.submit();
        }

        //查找Id跳转到对应的编辑页-------------
        function editTemp(id, windowWidth, windowHeight) {
            alert("查找Id跳转到对应的编辑页");
            var str = editTempVal(id);
            if (str == null) {
                return false;
            }
            var lablename = str[1];

            var url = "set" + lablename + ".aspx?tempid=" + id + "&columnid=<@@columnId %>&columntype=<@@columnType %>";

            var width = 500, height = 350;

            if (lablename == "include") {
                width = 350;
                height = 150;
            }
            else if (lablename == "type" || lablename == "productlist" || lablename == "videolist" || lablename == "video" || lablename == "flash" || lablename == "photo") {
                width = 715;
                height = 490;
            }
            else if (lablename == "newslist" || lablename == "menu" || lablename == "techpatentlist" || lablename == "tenderslist" || lablename == "awardlist") {
                if (windowHeight < 300) {
                    windowHeight = 300;
                }
                width = windowWidth + 345;
                height = windowHeight + 185;
                if (width > 1004) {
                    width = 1004;
                }

                if (height > 600) {
                    height = "555";
                }

                url += "&width=" + windowWidth + "&height=" + windowHeight;
            }
            else if (lablename == "marquee") {
                width = 450;
                height = 250;
            }
            else if (lablename == "service" || lablename == "statis") {
                width = 300;
                height = 200;
            }
            whir.dialog.open("标签设置", url, width, height, "editTemp", true);
        }

        //添加置标
        function addTemp(lablename) {
            alert("添加置标");
            var url = "set" + lablename + ".aspx?columnid=<@@columnId %>&columntype=<@@columnType %>";

            var winth = 500, height = 350;

            if (lablename == "include") {
                winth = 350;
                height = 150;
            }
            else if (lablename == "newslist" || lablename == "type" || lablename == "productlist" || lablename == "videolist" || lablename == "video" || lablename == "flash" || lablename == "photo" || lablename == "menu" || lablename == "techpatentlist" || lablename == "tenderslist" || lablename == "awardlist") {
                winth = 715;
                height = 490;
            }
            else if (lablename == "marquee") {
                winth = 450;
                height = 250;
            }
            else if (lablename == "service" || lablename == "statis") {
                width = 300;
                height = 200;
            }

            whir.dialog.open("添加标签", url, winth, height, "addTemp", true);
        }

        function editSave() {
            if (SoucrePanel.style.display != "none") {
                getText();
            }
            if (confirm('<%="确定保存吗？".ToLang() %>')) {
                return true;
            }
            else {
                return false;
            }
        }

        //在编辑器插入固定置标
        function insertSystem(text) {
            alert("在编辑器插入固定置标");
            var str = '<wtl:system type="' + text + '"></wtl:system>';
            document.getElementById('ctlFlash').insertText(str);
            getText();
        }

        //在编辑器插入可视化置标
        function insertLabel(text) {
            alert("在编辑器插入可视化置标");
            document.getElementById('ctlFlash').insertText(text);
            getText();
        }

        //替换修改后的标签
        function editLabel(id, temp) {
            alert("替换修改后的标签");
            var str = editTempVal(id);
            var myAreaobj = document.getElementById(editorContent);
            var myAreanew = myAreaobj.value.replace(str[0], temp); //替换后的字符
            document.getElementById(editorContent).value = myAreanew;
            //setText();  //更新代码编辑器的字符
            formSubmit();
        }

        //查找Id对应的标签内容
        function editTempVal(id) {
            alert("查找Id对应的标签内容");
            var myArea = document.getElementById(editorContent).value;
            //var regexpStr = "<wtl:([a-zA-Z]{1,})([^>])*?id[ ]{0,}=[ ]{0,}[\"']" + id + "[\"']([^<])*?>(.|\\n)*?</wtl:\\1>";
            var regexpStr = "<wtl:([a-zA-Z]{1,})(.*?)id[ ]{0,}=[ ]{0,}[\"']" + id + "[\"'](.*?)>(.|\\n)*?</wtl:\\1>";
            return myArea.match(new RegExp(regexpStr, "i"));
        }
        //调整Ifram大小
        function reSetIframe() {
            var iframe = document.getElementById("designframe");
            iframe.height = getClientHeight() - 32;
        }

        //取窗口可视范围的高度
        function getClientHeight() {
            var clientHeight = 0;
            if (document.body.clientHeight && document.documentElement.clientHeight) {
                var clientHeight = (document.body.clientHeight < document.documentElement.clientHeight) ? document.body.clientHeight : document.documentElement.clientHeight;
            }
            else {
                var clientHeight = (document.body.clientHeight > document.documentElement.clientHeight) ? document.body.clientHeight : document.documentElement.clientHeight;
            }
            return clientHeight;
        }

        
 
        

        //加载Flash编辑器
        function initFlash() {
            // initialize with parameters
            var flashvars = {
                // indicate the parser, aspx / csharp / javascript / css / vbscript / html / xml / php / phpcode
                parser: "wtl",

                // set the editor to read-only mode
                readOnly: false,

                // the editor detects client installed fonts and use preferred fonts if installed.
                // NOTE: the charactor '|' is required at the begin and end of the list
                preferredFonts: "|Fixedsys|Fixedsys Excelsior 3.01|Fixedsys Excelsior 3.00|Courier New|Courier|",

                // indicate the callback function so that we can load the content into editor once it is initialized.
                onload: "setText"
            };

            // flash player parameters, you can find more information at: http://code.google.com/p/swfobject/wiki/documentation
            var params = { menu: "false", wmode: "transparent", allowscriptaccess: "always" };

            // define the id of the flash control, we need the id in javascript interaction
            var attributes = { id: "ctlFlash", name: "ctlFlash" };

            // embed the flash with size, more information can be found at: http://code.google.com/p/swfobject/wiki/documentation
            swfobject.embedSWF("../../res/js/templeteEditor/CodeHighlightEditor.swf?now=" + (new Date()).getTime(), "flashContent", "100%", "" + getClientHeight() - 30 + "", "9", "expressInstall.swf", flashvars, params, attributes);
        }

        //是否可编辑
        var isHide = false;

         

        //判断模板置标是否正确
        function CheckRight() {
            //var ErrorItem = [];
        }

        //页面加载
        $(function () {
            initFlash(); //加载Flash编辑器
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div>
            <div class="bButton">
                <asp:LinkButton ID="btnSave" runat="server" OnClick="btnSave_Click" CssClass="aAll_button"
                    OnClientClick="return editSave();">
                    <em><img src="../../res/images/icon_baocun.gif" align="absmiddle" /></em><b><%="保存".ToLang() %></b>
                </asp:LinkButton>
                <a href="javascript:void(0);" class="aAll_button"
                    onclick="window.close();"><em>
                        <img src="<%=SysPath %>res/images/icon_shanchu.gif" align="absmiddle" /></em><b><%="关闭".ToLang() %></b>
                    <%="文件：".ToLang() %><asp:Label ID="lblPath" runat="server" Text=""></asp:Label>
                </a>
            </div>
        </div>
        <div>

            <div class="PageBody" style="height: 650px;">
                <div id="SoucrePanel">
                    <textarea id="EditorContent" name="EditorContent" runat="server" style="display: none"></textarea>
                    <div id="flashContent" style="width: 100%;">
                    </div>
                </div>
                <div id="DesignPanel" style="display: none">
                    <iframe id="designframe" name="designframe" width="100%" height="500" frameborder="0"
                        marginheight="0" marginwidth="0" src="about:blank" onload="reSetIframe()"></iframe>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
