<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="menu_resetResources.aspx.cs" Inherits="whir_system_module_extension_menu_resetResources" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="mainbox">
        <dl class="title_column">
            <a href="menulist.aspx"><b><%="菜单管理".ToLang()%></b></a> <em class="line"></em>
            <asp:HyperLink ID="hlkEdit" runat="server" NavigateUrl="menu_edit.aspx"><b><%="添加菜单".ToLang()%></b></asp:HyperLink>
            <em class="line"></em><a href="menu_resetResources.aspx" class="aSelect"><b><%="重置权限资源".ToLang()%></b></a>
        </dl>
        <div class="line_border">
        </div>
        <br />
        <div class="note_yellow">
            <%="由于在数据库直接手工添加的菜单无法在权限管理进行授权，需要重置资源 。".ToLang()%></div>
        <div class="operate_foot">
            <asp:UpdatePanel ID="upReset" runat="server">
            <ContentTemplate>            
            <asp:LinkButton ID="btnReset" runat="server" CssClass="aLink" OnClick="btnReset_Click"><b><%="开始重置".ToLang()%></b></asp:LinkButton>
            </ContentTemplate>
            </asp:UpdatePanel>
            
        </div>
    </div>
</asp:Content>
