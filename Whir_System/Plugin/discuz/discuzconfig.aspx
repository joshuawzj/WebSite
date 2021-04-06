<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="discuzconfig.aspx.cs" Inherits="whir_system_Plugin_discuz_discuzconfig" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <div class="box_common">
            <h3 class="f_title">
                第三方用户集成</h3>
            <div class="All_table">
                <table width="100%" cellspacing="0" cellpadding="0" class="table2">
                    <tr>
                        <td width="150" class="item">
                            论坛访问URL：
                        </td>
                        <td>
                            <whir:textbox id="txtDiscuzURL" runat="server" cssclass="text_common" width="300px"
                          ErrorCss="form_error" TipCss="form_tip" required="True" regular="Url" />
                        </td>
                    </tr>
                    <tr>
                        <td class="item">
                            API Key：
                        </td>
                        <td>
                            <whir:textbox id="txtAPIKey" runat="server" cssclass="text_common" width="300px"
                          ErrorCss="form_error" TipCss="form_tip" required="True" minlength="6" />在论坛中进入“通行证设置”界面，添加“整合程序”,获取生成的API和密钥
                        </td>
                    </tr>
                    <tr>
                        <td class="item">
                            密钥：
                        </td>
                        <td>
                            <whir:textbox id="txtDiscuzKey" runat="server" cssclass="text_common" width="300px"
                          ErrorCss="form_error" TipCss="form_tip" required="True" minlength="6" />
                        </td>
                    </tr>
                    <tr>
                        <td class="item">
                            数据库IP地址或服务器名：
                        </td>
                        <td>
                            <whir:textbox id="txtServerName" runat="server" cssclass="text_common" width="300px"
                          ErrorCss="form_error" TipCss="form_tip" required="True" />论坛数据库连接字符串
                        </td>
                    </tr>
                    <tr>
                        <td class="item">
                            数据库登录帐号：
                        </td>
                        <td>
                            <whir:textbox id="txtLoginName" runat="server" cssclass="text_common" width="300px"
                          ErrorCss="form_error" TipCss="form_tip" required="True" />
                        </td>
                    </tr>
                    <tr>
                        <td class="item">
                            数据库登录密码：
                        </td>
                        <td>
                            <whir:textbox id="txtLoginPassword" runat="server" cssclass="text_common" width="300px"
                                textmode="Password" />  当文本框为空时，密码保持不变
                        </td>
                    </tr>
                    <tr>
                        <td class="item">
                            数据库名称：
                        </td>
                        <td>
                            <whir:textbox id="txtDatabaseName" runat="server" cssclass="text_common" width="300px"
                          ErrorCss="form_error" TipCss="form_tip" required="True" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="button_submit_div">
            <asp:LinkButton ID="linkBtnSave" CssClass="aLink" runat="server" 
                onclick="linkBtnSave_Click"><em><img src='<%=SysPath %>res/images/button_submit_icon_6.gif' /></em><b>保存</b></asp:LinkButton>
        </div>
    </div>
</asp:Content>
