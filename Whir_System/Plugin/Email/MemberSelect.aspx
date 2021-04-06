<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="MemberSelect.aspx.cs" Inherits="Whir_system_Plugin_Email_MemberSelect" %>

<%@ Import Namespace="Whir.Language" %>
<%@ MasterType VirtualPath="~/whir_system/master/DialogMasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        //添加按钮
        $(function () {
            $("[name=_dialog] .btn-primary", parent.document)
                .click(function () {
                    if ($table.bootstrapTable('getSelections').length == 0) {
                        window.parent.TipMessage('<%="请选择".ToLang() %>');
                        return false;
                    } else {
                        window.parent.fill_member($table.bootstrapTable('getSelections'));
                        window.parent.whir.dialog.remove();
                        return false;
                    }
                })
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" runat="server">
        <div class="panel-body">
        <whir:ContentManager ID="contentManager1" runat="server"  >
        </whir:ContentManager>
        <div class="operate_foot">
            <asp:HiddenField ID="hidSelected" runat="server" />
        </div>
        </div>
    </form>
</asp:Content>
