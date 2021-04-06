<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="subinfo.aspx.cs" Inherits="whir_system_ModuleMark_common_subinfo" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <div class="All_table" style="height: 400px; vertical-align: middle;">
            <table style="margin-top: 100px; width: 350px; height: 125px; border: #d6dfe5 1px solid;"
                valign="middle" align="center" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="border-bottom: 0px;" align="center">
                        <asp:Literal ID="ltInfo" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
