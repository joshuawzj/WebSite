<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="TemplateSelector.aspx.cs" Inherits="Whir_System_Module_Column_TemplateSelector" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>

        <div class="panel">

            <ul class="nav nav-tabs">
                <li class="active"><a data-toggle="tab" href="#template"><%="模板列表".ToLang()%></a></li>
                <li class=""><a data-toggle="tab" href="#include"><%="公用模板".ToLang()%></a></li>
            </ul>

            <div class="panel-body">
                <div class="tab-content">
                    <div id="template" class="tab-pane active">
                        <table width="100%" class="table table-bordered table-noPadding">
                            <thead>
                                <tr class="trClass">
                                    <th>&nbsp;</th>
                                    <th><%="文件名".ToLang()%></th>
                                    <th><%="文件大小".ToLang()%></th>
                                    <th><%="创建日期".ToLang()%></th>
                                    <th><%="最后修改日期".ToLang()%></th>
                                </tr>
                            </thead>
                            <tbody id="tbdTemplate"></tbody>
                        </table>
                    </div>
                    <div id="include" class="tab-pane">

                        <table width="100%" class="table table-bordered table-noPadding">
                            <thead>
                                <tr class="trClass">
                                    <th>&nbsp;</th>
                                    <th><%="文件名".ToLang()%></th>
                                    <th><%="文件大小".ToLang()%></th>
                                    <th><%="创建日期".ToLang()%></th>
                                    <th><%="最后修改日期".ToLang()%></th>
                                </tr>
                            </thead>
                            <tbody id="tbdInclude"></tbody>
                        </table>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <script id="tempRow" type="text/html">
        ${ each templates as template index }$
        <tr>
            <td>
                <div class="table-radio">
                    <input type="radio" name="template" value="${=template.filePath}$" />
                </div>
            </td>
            <td>${=template["fileName"]}$</td>
            <td>${=template["fileSize"]}$</td>
            <td>${=template["createDate"]}$</td>
            <td>${=template["lastWriteDate"]}$</td>
        </tr>
        ${ /each }$
    </script>
    <script type="text/javascript">
        var allTemplateJson = <%=AllTemplateJson%>;
        var allIncludeJson = <%=AllIncludeJson%>;

        head.js(_sysPath + "res/js/ArtTemplate/template-debug.js", function () {
            template.config("openTag", "${"); //此处自定义开启标记
            template.config("closeTag", "}$"); //此处自定义闭合标记

            var templateHtml = template("tempRow", { templates: allTemplateJson });
            var includeHtml = template("tempRow", { templates: allIncludeJson });
            $("#tbdTemplate").html(templateHtml);
            $("#tbdInclude").html(includeHtml);

            //美化单选按钮
            whir.skin.radio();
        });
    </script>
</asp:Content>
