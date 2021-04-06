<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="ClearReleaseFiles.aspx.cs" Inherits="whir_system_module_release_ClearReleaseFiles" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tbody>
                <tr>
                    <td class="box_common" valign="top" width="50%">
                        <h3 class="f_title">
                            系统文件夹</h3>
                        <div style="overflow: auto; padding: 10px;">
                            <asp:ListBox ID="listFolders" Width="300" Height="300" runat="server"></asp:ListBox>
                        </div>
                    </td>
                    <td width="5">
                    </td>
                    <td class="box_common" valign="top">
                        <h3 class="f_title">
                            系统文件</h3>
                        <div style="overflow: auto; padding: 10px;">
                            <asp:ListBox ID="listFiles" Width="300" Height="300" runat="server"></asp:ListBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <p style="color: red;padding-left: 10px;font-size:14px;font-weight: bold">
                            注意：以上为系统默认初始目录，除了上面列出的文件，文件夹，其他的文件，文件夹都将被删除！有进行二次开发的项目，请慎用！</p>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="center">
                        <div class="button_submit_div">
                            <asp:LinkButton ID="btnClearFiles" runat="server" OnClientClick="return confirm('确定要根据当前站点栏目清理已生成的文件吗？')" CssClass="aLink" OnClick="btnClearFiles_Click">
                            <em><img src='<%=SysPath%>res/images/button_submit_icon_1.gif' /></em><b><%="清理生成文件".ToLang()%></b>
                            </asp:LinkButton></div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
