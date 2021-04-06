<%@ Page Language="C#" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="Shop_member_login" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader"
    TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户登录</title>
    <link href="../css/shop_whir.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript">
        function changeCode() {
            $('#imgcode').attr('src', $('#imgcode').attr('src') + '?');
        }

        function befortLogin() {
            var name = $.trim($("#<%=txtUserName.ClientID %>").val());
            var pwd = $.trim($("#<%=txtPassword.ClientID %>").val());
            var imgcode = $.trim($("#<%=txtCode.ClientID %>").val());
            if (name == "") {
                alert("用户名不能为空");
                $("#<%=txtUserName.ClientID %>").focus();
                return false;
            }
            if (pwd == "") {
                $("#<%=txtPassword.ClientID %>").focus();
                alert("密码不能为空");
                return false;
            }
            if (imgcode == "") {
                alert("请输入验证码");
                $("#<%=txtCode.ClientID %>").focus();
                return false;
            }
            var exp = /\d{4}/;
            if (exp.test(imgcode)) {
            } else {
                alert("验证码格式不正确");
                $("#<%=txtCode.ClientID %>").val("");
                $("#<%=txtCode.ClientID %>").focus();
                return false;
            }
            return true;
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
                <span>没有账号，现在就去 <a class="a_fontblue" href="register.aspx"><u>注册</u></a></span><b>用户登录</b></div>
            <!--Start-->
            <div class="TableAll" style="float: left;">
                <table border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="name">
                            用户名：
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="inputAll" TabIndex="20"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            密码：
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" CssClass="inputAll" TextMode="Password"
                                TabIndex="21"></asp:TextBox>
                            <a href="forgotpassword.aspx" class="a_fontblue"><u>忘记密码?</u></a>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            验证码：
                        </td>
                        <td>
                            <asp:TextBox ID="txtCode" runat="server" CssClass="inputAll" MaxLength="4" TabIndex="22"
                                Width="100"></asp:TextBox>
                            <img id="imgcode" src="checkcode.ashx" onclick="this.src=this.src+'?'" height="25px" style=" cursor:pointer" />
                            <a href="javascript:changeCode()" class="a_fontblue"><u>刷新验证码</u></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnLogin" runat="server" Text="登录" CssClass="btn" OnClick="btnLogin_Click"
                                OnClientClick="return befortLogin()" />
                        </td>
                    </tr>
                </table>
            </div>
            <!--End-->
        </div>
    </div>
    </form>
</body>
</html>
