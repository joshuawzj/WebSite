<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="MemberList.aspx.cs" Inherits="Whir_System_ModuleMark_Member_MemberList" %>

<%@ Import Namespace="Whir.Language" %>
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
                    <li class="active"><a data-toggle="tab"  aria-expanded="true"><%="会员列表".ToLang()%></a></li>
                      <%if (IsCurrentRoleMenuRes("296"))
                          { %>
                    <li > <a   href="Recycle.aspx?columnid=<%=ColumnId %>&isdel=true&BackPageUrl=<%=CurrentPageUrl%>">
                       <span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%>
                    </a></li>
                      <%} %>
                  </ul>
                <div class="space15"></div>
                <div class="actions btn-group pull-left">
                     <%if (IsCurrentRoleMenuRes("295"))
                          { %>
                    <a class="btn btn-white" href="Member_Edit.aspx?columnid=<%=ColumnId %>&BackPageUrl=<%=CurrentPageUrl%>">
                        <i class="glyphicon glyphicon-plus"></i>&nbsp;<%= "添加会员".ToLang() %>
                    </a>
                    <%} %>
             
                </div>  
                <div class="member-list-last">
                 <whir:WorkFlowBar ID="workFlowBar1" runat="server"></whir:WorkFlowBar>
                <whir:ContentManager ID="contentManager1" runat="server"  ></whir:ContentManager>
            </div>
            </div>
            </form>
        </div>
    </div>
</asp:Content>
