<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    CodeFile="contentlist.aspx.cs" Inherits="whir_system_ModuleMark_category_contentlist"%>

<%@ Import Namespace="Whir.Framework"%>
<%@ Import Namespace="Whir.Language"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script type="text/javascript">
        $(function () {

            var isAdd = '<%=IsAdd%>'.toLowerCase() == 'true' ? true : false;
            var isOutput = '<%=IsOutput%>'.toLowerCase() == 'true' ? true : false;
            var isDelete = '<%=IsDelete%>'.toLocaleLowerCase() == 'true' ? true : false;
            var isRecycle = '<%=IsRecycle%>'.toLocaleLowerCase() == 'true' ? true : false;
            var isSort = '<%=IsSort%>'.toLocaleLowerCase() == 'true' ? true : false;

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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%=ColumnName%></div>
            <form enctype="multipart/form-data" class="form-horizontal bv-form">
                <div class="panel-body">
                    <div class="actions btn-group pull-left">
                        <% if (IsShowAttchlist && AttchlistColumn != null)
                            {%>
                        <% foreach (var item in AttchlistColumn)
                            {%>
                        <% if (item.MarkType == "Category")
                            {%>
                        <a class="btn btn-white" href="<%="{0}Modulemark/Jobs/Categorylist.aspx?columnid={1}&subjectid={2}".FormatWith(SysPath, item.ColumnId, SubjectId)%>"><span class="entypo-flow-cascade"></span>&nbsp;<%=item.ColumnName.ToLang()%></a>
                        <% } else {%>
                        <a class="btn btn-white" href="<%="Contentlist.aspx?columnid={0}&subjectid={1}".FormatWith(item.ColumnId, SubjectId)%>"><%=item.ColumnName%></a>
                        <% }%>
                        <% }%>
                        <% }%>
                        <%if (IsRoleHaveColumnRes("添加"))
                          { %>
                        <a class="btn btn-white" href="Content_Edit.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&BackPageUrl=<%=CurrentPageUrl%>">
                            <i class="glyphicon glyphicon-plus"></i>&nbsp;<%="添加内容".ToLang()%>
                        </a>
                          <% }%>
                         <%if (IsRoleHaveColumnRes("回收站"))
                           { %>
                        <a class="btn btn-white" href="<%=SysPath%>Modulemark/Common/Recycle.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&isdel=true&BackPageUrl=<%=CurrentPageUrl%>">
                            <span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%>
                        </a>
                         <% }%>
                    </div>
                    <whir:ContentManager ID="contentManager1" runat="server"  ></whir:ContentManager>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
