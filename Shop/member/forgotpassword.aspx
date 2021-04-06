<%@ Page Language="C#" AutoEventWireup="true" CodeFile="forgotpassword.aspx.cs" Inherits="Shop_member_forgotpassword" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader"
    TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员中心-找回密码</title>
    <link href="../css/shop_whir.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript">
        function changeCode() {
            $('#imgcode').attr('src', $('#imgcode').attr('src') + '?');
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
     <!--共用头部-->
     <center>
        <img src="../images/header.jpg" /></center>
    <!--共用头部-->
        <uc2:CategoryHeader ID="CategoryHeader1" runat="server" />
    <div class="ShopContain">
        <div class="MainBox">
            <div class="Current">
                <span></span><b>找回密码</b></div>
            <asp:PlaceHolder ID="phStart" runat="server">
                <h6 class="step">
                    <span class="a_on"><b>填写账号名</b></span> <span><b>验证身份</b></span> <span class="end"><b>
                        完成</b></span>
                </h6>
                <div class="TableAll" style="width: 690px; margin: auto;">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td class="name">
                                找回方式：
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rbWay" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                    RepeatLayout="Flow" OnSelectedIndexChanged="rbWay_SelectedIndexChanged">
                                    <asp:ListItem Value="1" Selected="Selected">通过用户名找回</asp:ListItem>
                                    <asp:ListItem Value="2">通过安全邮箱找回</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="name">
                                <asp:Literal ID="ltNameEmail" runat="server" Text="用户名："></asp:Literal>
                            </td>
                            <td>
                                <whir:textbox id="txtNameEmail" width="280px" cssclass="inputAll" runat="server"
                                    required="True" message="*" maxlength="60"></whir:textbox>
                            </td>
                        </tr>
                        <tr>
                            <td class="name">
                                验证码：
                            </td>
                            <td>
                                <whir:textbox id="txtCode" runat="server" cssclass="inputAll" maxlength="4" 
                                    required="True" regular="Custom" validationexpression="\d{4}" width="100"></whir:textbox>
                                <img id="imgcode" src="checkcode.ashx" onclick="this.src=this.src+'?'" height="25px" style=" cursor:pointer"/>
                                <a href="javascript:changeCode()" class="a_fontblue"><u>刷新验证码</u></a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:Button ID="btnNext" runat="server" Text="下一步" CssClass="btn" OnClick="btnNext_Click" />&nbsp;&nbsp;&nbsp;<asp:Literal
                                    ID="ltMsgStart" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phMiddle" runat="server" Visible="false">
                <h6 class="step">
                    <span class="a_mid"><b>填写账号名</b></span> <span class="a_on"><b>验证身份</b></span> <span
                        class="end"><b>完成</b></span>
                </h6>
                <div class="TableAll" style="width: 690px; margin: auto;">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td class="name">
                                用户名：
                            </td>
                            <td>
                                <asp:Literal ID="ltUserName" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td class="name">
                                邮箱地址：
                            </td>
                            <td>
                                <asp:Literal ID="ltEmail" runat="server"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                            <asp:Button ID="btnSendEmail" runat="server" Text="确认账户信息,发送邮件" CssClass="btn" OnClick="btnSendEmail_Click" /> &nbsp;&nbsp;&nbsp;<asp:Literal
                                    ID="ltMiddleMsg" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:PlaceHolder>
            <asp:PlaceHolder ID="phEnd" runat="server" Visible="false">
                <h6 class="step">
                    <span><b>填写账号名</b></span> <span class="a_mid"><b>验证身份</b></span> <span class="a_on end">
                        <b>完成</b></span>
                </h6>
                <div class="TableAll" style="width: 690px; margin: auto;">
                    <div class="txt_success" runat="server" id="divSuccess">
                        <h5>
                            您的帐户信息已经发送至指定邮箱，请查收！</h5>
                        为确保您的帐户安全，请在查收邮箱登录后及时修改密码！<br />
                        <br />
                        <input type="button" name="button" id="button" value="完成" onclick="window.navigate('login.aspx');" class="btn" />
                    </div>
                    <div class="txt_failure"  runat="server" id="divfailure">
                        <h5>
                            您的帐户信息已经发送至指定邮箱，请查收！</h5>
                        为确保您的帐户安全，请在查收邮箱登录后及时修改密码！<br />
                        <br />
                        <input type="button" name="button" id="button" value="完成" onclick="window.navigate('login.aspx');" class="btn" />
                    </div>
                </div>
            </asp:PlaceHolder>
            <asp:HiddenField ID="hdUserID" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
