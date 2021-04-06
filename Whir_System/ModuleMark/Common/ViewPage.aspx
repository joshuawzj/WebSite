<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="ViewPage.aspx.cs" Inherits="Whir_System_ModuleMark_Common_ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
     
            <div class="panel">
                 <div class="panel-body">
            <table width="100%" height="100%">
                <tr>
                    <td align="center" valign="middle">
                        <div id="no_data" style="width:400px; height:150px; line-height:150px;">
                            <asp:Literal ID="litRedirect" runat="server"></asp:Literal>
                        </div>
                    </td>
                </tr>
            </table>
            </div>
          </div>
      
</asp:content>
