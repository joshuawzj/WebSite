<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="ContentList.aspx.cs" Inherits="Whir_System_ModuleMark_Magazine_ContentList" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
 <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            
            <form enctype="multipart/form-data" class="form-horizontal bv-form">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li class="active"><a data-toggle="tab"  aria-expanded="true"><%=ColumnName%></a></li>
                          
                       <%if (IsRoleHaveColumnRes("回收站"))
                         { %>
                    <li > <a   href="Recycle.aspx?columnid=<%=ColumnId %>&SubjectId=<%=SubjectId %>&isdel=true&BackPageUrl=<%=CurrentPageUrl%>">
                       <span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%>
                    </a></li>
                      <%} %>
                  </ul>
                  <div class="space15"></div>
                <div class="actions btn-group pull-left">
                    <%if (IsRoleHaveColumnRes("添加"))
                      { %>
                     <a class="btn btn-white" href="Content_Edit.aspx?columnid=<%= ColumnId %>&subjectid=<%= SubjectId %>&BackPageUrl=<%= CurrentPageUrl %>">
                        <i class="glyphicon glyphicon-plus"></i>&nbsp;<%= "添加内容".ToLang() %>
                    </a> 
                     <% }%>
                         
                    <asp:Literal ID="ltContent" runat="server"></asp:Literal>
                </div>
                 <whir:WorkFlowBar ID="workFlowBar1" runat="server"></whir:WorkFlowBar>
                <whir:ContentManager ID="contentManager1" runat="server"  ></whir:ContentManager>
            </div>
            </form>
        </div>
    </div>
</asp:content>


