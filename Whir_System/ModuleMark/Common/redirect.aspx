<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="redirect.aspx.cs" Inherits="whir_system_ModuleMark_common_redirect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<table width="100%" height="100%">
    <tr>
        <td align="center" valign="middle">
            <div id="no_data" style="width:300px; height:150px; line-height:150px;">
                <asp:Literal ID="litRedirect" runat="server"></asp:Literal>
            </div>
        </td>
    </tr>
</table>

</asp:Content>

