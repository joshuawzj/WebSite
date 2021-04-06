<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    CodeFile="JobRequestList.aspx.cs" Inherits="Whir_System_ModuleMark_Jobs_JobRequestList"%>
<%@ Import Namespace="Whir.Language"%>
<%@ Import Namespace="Whir.Framework"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
                    <% if (item.MarkType == "Category")
                        {%>
                    <li><a href="<%="Categorylist.aspx?columnid={0}&subjectid={1}".FormatWith(item.ColumnId, SubjectId)%>">
                        <%=item.ColumnName.ToLang()%></a></li>
                    <% }
                        else
                        { %>
                    <li><a href="<%="Contentlist.aspx?columnid={0}&subjectid={1}".FormatWith(item.ColumnId, SubjectId)%>">
                        <%=item.ColumnName.ToLang()%></a></li>
                    <%}
                            }
                        }%>
                    <li class="active"><a data-toggle="tab" aria-expanded="true"><%=ColumnName%></a></li>
                    <%if (IsRoleHaveColumnRes("回收站"))
                        { %>
                    <li><a href="Recycle.aspx?columnid=<%=ColumnId %>&SubjectId=<%=SubjectId %>&isdel=true&BackPageUrl=<%=CurrentPageUrl%>">
                       <span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%>
                    </a></li>
                    <%} %>
                </ul>
                <div class="space15"></div>
                <div class="actions btn-group pull-left">
                    
                     <%if (IsRoleHaveColumnRes("添加"))
                          { %>
                    <a class="btn btn-white" href="Content_Edit.aspx?columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&BackPageUrl=<%=CurrentPageUrl%>">
                        <i class="glyphicon glyphicon-plus"></i>&nbsp;<%="添加内容".ToLang()%>
                    </a>
                     <% }%>
                     
                </div>
                <whir:WorkFlowBar ID="workFlowBar1" runat="server"></whir:WorkFlowBar>
                <whir:ContentManager ID="contentManager1" runat="server" IsShowExportDoc="true" >
                </whir:ContentManager>
            </div>
            </form>
        </div>
    </div>
        <script type="text/javascript">
            //批量导出
            function openSelectColumn() {
                if ($("#hidChoose").val() == '') {
                    TipMessage('<%="请选择".ToLang()%>');
                    return false;
                }
                var url = "<%=SysPath%>Handler/Common/Common.aspx?_action=InportJobRequestAll";
                $('<form method="post" action="' +
                        url +
                        '"><input type="hidden" name="Selected" value="' +
                        $("#hidChoose").val() +
                        '"><input type="hidden" name="ColumnId" value="<%=ColumnId%>"></form>')
                    .appendTo('body')
                    .submit()
                    .remove();
                return false;
            }
        </script>
</asp:Content>
