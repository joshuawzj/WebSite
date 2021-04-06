<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    CodeFile="Content_Edit.aspx.cs" Inherits="Whir_System_ModuleMark_Jobs_Content_Edit" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <whir:DynamicForm ID="dynamicForm1" runat="server" FormType="LeftAndRight"></whir:DynamicForm>
</asp:Content>
