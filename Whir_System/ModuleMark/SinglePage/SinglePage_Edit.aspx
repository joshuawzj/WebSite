<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="SinglePage_Edit.aspx.cs" Inherits="Whir_System_ModuleMark_SinglePage_SinglePage_Edit" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        //打开历史页面
        function openBak() {
            whir.dialog.frame('<%="历史".ToLang()%>', "<%= SysPath%>ModuleMark/Common/HistoryBak.aspx?columnid=<%= ColumnId %>&itemid=<%= ItemID %>&time=" + new Date().getMilliseconds(), null, 1100, 600, false);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <whir:DynamicForm ID="dynamicForm1" runat="server" FormType="LeftAndRight" IsSinglePage="true"></whir:DynamicForm>
</asp:Content>

