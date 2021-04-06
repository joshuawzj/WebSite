<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="FeedBackList.aspx.cs" Inherits="Whir_System_ModuleMark_FeedBack_FeedBackList" %>
      <%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading"><%=ColumnName%></div>
            <form enctype="multipart/form-data" class="form-horizontal bv-form">
            <div class="panel-body">
                  <div class="space15"></div>
                <div class="actions btn-group pull-left">
                     <%if (IsRoleHaveColumnRes("添加"))
                          { %>
                        <a class="btn btn-white" href="FeedBack_Edit.aspx?&columnid=<%=ColumnId%>&subjectid=<%=SubjectId%>&BackPageUrl=<%=CurrentPageUrl%>">
                            <i class="glyphicon glyphicon-plus"></i>&nbsp;<%="添加内容".ToLang()%>
                        </a> 
                         <% }%>
                </div>
                <whir:WorkFlowBar ID="workFlowBar1" runat="server"></whir:WorkFlowBar>
                <whir:ContentManager ID="contentManager1" runat="server" ></whir:ContentManager>
            </div>
            </form>
        </div>
    </div>
</asp:Content>
