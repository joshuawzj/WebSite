<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SubmitForm_PreView.aspx.cs"
    Inherits="Whir_System_Module_Developer_SubmitForm_Preiew" %>

<%@ Import Namespace="Whir.Language" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <link href="<%=AppName %>res/css/global.css" rel="stylesheet" type="text/css" />

    <link rel="stylesheet" href="<%=AppName%>Editor/KindEditor/themes/default/default.css" />
    <link rel="stylesheet" href="<%=AppName%>Editor/KindEditor/plugins/code/prettify.css" />
    <script type="text/javascript" src="<%=AppName%>Editor/KindEditor/kindeditor.js"></script>
    <script type="text/javascript" src="<%=AppName%>Editor/KindEditor/lang/zh_CN.js"></script>
    <script type="text/javascript" src="<%=AppName%>Editor/KindEditor/plugins/code/prettify.js"></script>
    <script src="<%=AppName %>res/js/area/Area.js" type="text/javascript"></script>
    <script src="<%=AppName %>res/js/area/<%=JsName %>" type="text/javascript"></script>


    <link href="<%=AppName%>res/css/global.css" rel="stylesheet" type="text/css" />
    <link href="<%=AppName%>res/js/webuploader/webuploader.css " rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="<%=AppName%>res/js/jquery-3.3.1.min.js"></script>
    <script type="text/javascript" src="<%=AppName%>res/js/common.js"></script>
    <script type="text/javascript" src="<%=AppName%>res/js/jquery.form.js"></script>
    <script type="text/javascript" src="<%=AppName%>res/js/Jquery.Query.js"></script>
    <script type="text/javascript" src="<%=AppName%>res/js/submitform_validator.js" websiteurl="<%=AppName %>"></script>
    <script type="text/javascript" src="<%=AppName%>res/js/DatePicker/WdatePicker.js"></script>
    <script type="text/javascript" src="<%=AppName%>res/js/webuploader/webuploader.js"></script>
    <script type="text/javascript" src="<%=AppName%>res/js/webuploader/uploadRun.js"></script>

    <script type="text/javascript">
        <%if (isTemplatePreview)
        { %>
        $(function () {
            var content = window.parent.getContent();
            $("#divPreview").html(content);
        });
        <%} %>
    </script>
</head>
<body>
    <div class="mainbox" id="divbody">
        <h3>
            <%="提交表单预览".ToLang() %></h3>
        <div class="line_border">
        </div>
        <div>
            <%if (isTemplatePreview)
                { %>
            <div id="divPreview"></div>
            <%}
                else
                {%>
            <asp:Literal ID="litData" runat="server"></asp:Literal>
            <%} %>
        </div>
    </div>
</body>
</html>
