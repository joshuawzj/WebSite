<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="categorylist.aspx.cs" Inherits="whir_system_ModuleMark_category_categorylist" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript">
        $(function () {

            var isAdd = '<%=IsAdd %>'.toLowerCase() == 'true' ? true : false;
            var isOutput = '<%=IsOutput %>'.toLowerCase() == 'true' ? true : false;
            var isDelete = '<%=IsDelete %>'.toLocaleLowerCase() == 'true' ? true : false;
            var isRecycle = '<%=IsRecycle %>'.toLocaleLowerCase() == 'true' ? true : false;
            var isSort = '<%=IsSort %>'.toLocaleLowerCase() == 'true' ? true : false;

            //是否具有添加功能
            if (isAdd)
                $("#aAdd").css("display", "");
            else
                $("#aAdd").css("display", "none");

            //是否具有导出功能
            if (isOutput)
                $("#aOutput").css("display", "");
            else
                $("#aOutput").css("display", "none");


            //是否具有回收站功能
            if (isRecycle)
                $("#aRecycle").css("display", "");
            else
                $("#aRecycle").css("display", "none");

        });
    </script>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <form enctype="multipart/form-data" class="form-horizontal bv-form">
                <div class="panel-body">
                    <ul class="nav nav-tabs">
                        <% if (AttchlistColumn != null)
                            {%>
                        <% foreach (var item in AttchlistColumn)
                            {%>
                        <li><a href="<%="{0}Modulemark/Common/Contentlist.aspx?columnid={1}&subjectid={2}".FormatWith(SysPath, item.ColumnId, SubjectId) %>">
                            <%=item.ColumnName.ToLang()%></a></li>
                        <%  
                                }
                            }%>
                        <li class="active"><a data-toggle="tab" aria-expanded="true"><span class="entypo-flow-cascade"></span>&nbsp;<%=ColumnName%></a></li>
                    </ul>
                    <div class="space15"></div>

                    <div class="actions btn-group pull-left">

                        <%if (IsRoleHaveColumnRes("添加"))
                            { %>
                        <a class="btn btn-white" href="Content_Edit.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&BackPageUrl=<%=CurrentPageUrl%>">
                            <i class="glyphicon glyphicon-plus"></i>&nbsp;<%="添加内容".ToLang() %>
                        </a><% }%>
                    </div>
                    <whir:WorkFlowBar ID="workFlowBar1" runat="server"></whir:WorkFlowBar>
                    <whir:ContentManager ID="contentManager1" runat="server" IsMarkType="true"></whir:ContentManager>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
