<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    CodeFile="categorylist.aspx.cs" Inherits="whir_system_ModuleMark_jobs_categorylist" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
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
                         <% if (item.MarkParentId ==0)
                            {%>
                           <li > <a   href="<%="{0}Modulemark/Jobs/Contentlist.aspx?columnid={1}&subjectid={2}".FormatWith(SysPath, item.ColumnId, SubjectId)%>"><%=item.ColumnName%></a>
                          </li> 
                         <% } %>
                        <% }%>
                        <% }%>
                       <li class="active"><a data-toggle="tab" aria-expanded="true"><span class="entypo-flow-cascade"></span>&nbsp;<%=ColumnName.ToLang()%></a></li>
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
                    <whir:ContentManager ID="contentManager1" runat="server" IsMarkType="true" ></whir:ContentManager>
                </div>
            </form>
        </div>
    </div>
</asp:content>

