<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    CodeFile="ContentList.aspx.cs" Inherits="Whir_System_ModuleMark_Jobs_ContentList" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
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
                        <li class="active"><a data-toggle="tab" aria-expanded="true"><%=ColumnName%></a></li>
                        <% if (IsShowAttchlist && AttchlistColumn != null)
                            {%>
                        <% foreach (var item in AttchlistColumn)
                            {%>
                        <% if (item.MarkType == "Category")
                            {%>
                        <li><a href="<%="{0}Modulemark/Jobs/Categorylist.aspx?columnid={1}&subjectid={2}".FormatWith(SysPath, item.ColumnId, SubjectId)%>">
                            <span class="entypo-flow-cascade"></span>&nbsp;<%=item.ColumnName.ToLang()%></a></li>
                        <% }
                                }
                            }%>
                        <%if (IsRoleHaveColumnRes("回收站"))
                            { %>
                        <li><a href="<%=SysPath %>Modulemark/Common/Recycle.aspx?columnid=<%=ColumnId %>&SubjectId=<%=SubjectId %>&isdel=true&BackPageUrl=<%=CurrentPageUrl%>">
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
                    <whir:ContentManager ID="contentManager1" runat="server"></whir:ContentManager>
                </div>
            </form>
        </div>
    </div>
</asp:Content>
