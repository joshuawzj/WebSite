<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    CodeFile="content_edit.aspx.cs" Inherits="whir_system_ModuleMark_category_content_edit" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="mainbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td class="box_common" valign="top">
                    <whir:DynamicForm ID="dynamicForm1" runat="server" FormType="LeftAndRight"></whir:DynamicForm>
                    
                </td>
            </tr>
        </table>
    </div>

</asp:Content>
