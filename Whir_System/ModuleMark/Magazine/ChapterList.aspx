<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="ChapterList.aspx.cs" Inherits="Whir_System_ModuleMark_Magazine_ChapterList"%>
<%@ Import Namespace="Whir.Language"%>
<%@ Import Namespace="Whir.Framework"%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                    <% if (item.MarkType == "Chapter")
                        {%>
                    <li><a href="<%="ChapterList.aspx?columnid={0}&contentid={1}&subjectid={2}&BackPageUrl={3}".FormatWith(item.ColumnId,ItemID,SubjectId,CurrentPageUrl)%>">
                        <%=item.ColumnName.ToLang()%></a></li>
                    <% }%>
                    <%  else
                        {%>
                    <li><a href="<%="ContentList.aspx?columnid={0}&contentid={1}&subjectid={2}&BackPageUrl={3}".FormatWith(item.ColumnId,ItemID, SubjectId,CurrentPageUrl)%>">
                        <%=item.ColumnName.ToLang()%></a></li>
                    <% }%>
                    <% }%>
                    <% }%>
                    <li class="active"><a data-toggle="tab" aria-expanded="true"><%=ColumnName.ToLang()%></a></li>
                    <%if (IsRoleHaveColumnRes("回收站"))
                        { %>
                    <li><a href="Recycle.aspx?columnid=<%=ColumnId %>&subjectid=<%=SubjectId%>&isdel=true&BackPageUrl=<%=CurrentPageUrl%>">
                       <span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%>
                    </a></li>
                    <%} %>
                </ul>
                <br />

                <div class="actions btn-group pull-left">
                   
                     <%if (IsRoleHaveColumnRes("添加"))
                          { %>
                    <a class="btn btn-white" href="Content_Edit.aspx?&columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&magazineid=<%=ItemID%>&BackPageUrl=<%=CurrentPageUrl%>">
                        <i class="glyphicon glyphicon-plus"></i>&nbsp;<%="添加内容".ToLang()%>
                    </a> 
                      <% }%>
                    
                </div>
                <whir:ContentManager ID="contentManager1" runat="server"  >
                </whir:ContentManager>
            </div>
            </form>
        </div>
    </div>
</asp:Content>


  