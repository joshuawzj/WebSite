<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="iischeck.aspx.cs" Inherits="whir_system_iischeck" %>
    <%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
   <div class="content-wrap"> 
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
            <table id="table_local" width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <th class="th_title_name" colspan="2">
                        <%="读写权限".ToLang() %>
                    </th>
                </tr>
                <tr>
                    <td class="item" width="<%=LanguageHelper.GetSplitValue("150px|150px|200px")%>">
                        <%="空间是否支持写入：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_QX" ForeColor="#999999" Font-Bold="true" Text="未知" runat="server" />
                         <%if (IsCurrentRoleMenuRes("370"))
                           { %>
                        <asp:Button ID="btnCheckWrite" runat="server" OnClick="btnCheckWrite_Click" Text="" CssClass="btn_Check" />
                        <%} %>
                        <%="写入权限说明：有些空间商的空间看起来用一些asp.net探针运行正常，其实只是验证了asp.net对空间的读取权限，asp.net的写入权限可能没有的，要是不支持差不多所有使用的Access数据库的asp.net程序用不了，也生成不了静态页面。如果写入权限为支持的话基本这个空间才可以正常使用。".ToLang() %>
                    </td>
                </tr>
                <tr>
                    <th class="th_title_name" colspan="2">
                    <%="基本信息".ToLang() %>
                    </th>
                </tr>
                <asp:Panel runat="server" ID="ServiceInfo">
                <tr>
                    <td class="item">
                    <%="服务器名称：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_ServerName" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="操作系统：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_OS" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="服务器IP：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_ServerIP" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="服务器域名：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_ServerDomain" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="服务器请求超时：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_ScriptTimeout" runat="server" />s
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="服务器现在时间：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_now" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="Session总数：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_SessionCount" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="Application总数：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_ApplicationCount" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="IIS版本：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_IISVER" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item" nowrap="nowrap">
                    <%=".net Framework版本：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_framework" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="相对路径：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_XDLJ" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="物理路径：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_WLLJ" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="系统运行时间：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_ServerRunTime" runat="server" />h
                    </td>
                </tr>
                </asp:Panel>
                <asp:Panel runat="server" ID="notData" Visible="False">
                    <tr>
                    <td class="item">
                        <%= "服务器设置配置不可访问".ToLang() %>
                    </td>
                    <td>
                    </td>
                </tr>
                    
                </asp:Panel>
                <tr>
                    <th colspan="2" >
                    <%="组件信息".ToLang() %>
                    </th>
                </tr>
                <tr>
                    <td class="item">
                    <%="Access数据库组件：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_AccessObject" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="FSO组件：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_FSO" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="JMAIL邮件发送组件：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_JMAIL" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="CDONTS邮件发送组件：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_CDONTS" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="AspJpeg组件：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_AspJpeg" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="item">
                    <%="ASPUpload上传组件：".ToLang() %>
                    </td>
                    <td>
                        <asp:Label ID="Label_ASPUpload" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
     </div>
    
    </form>
</asp:Content>
