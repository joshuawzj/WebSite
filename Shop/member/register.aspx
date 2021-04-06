<%@ Page Language="C#" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="Shop_member_register" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader"
    TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户注册</title>
    <link href="../css/shop_whir.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript">
        function changeCode() {
            $('#imgcode').attr('src', $('#imgcode').attr('src') + '?');
        }

        $(function () {
            $("#txtUserName").blur(function () {
                var loginName = $.trim($("#txtUserName").val());
                var email = $.trim($("#txtEmail").val());
                if (loginName.length >= 5) {
                    $.get("ajaxonly.aspx", { onlyvalue: loginName, flag: 0 }, function (data) {
                        if (data == 1) {
                            $("#txtUserName").next().show();
                            $("#txtUserName").next().text("用户名已经存在");
                            $("#btnSubmit").attr("disabled", "disabled");
                        } else {
                            $("#btnSubmit").attr("disabled", "");
                        }
                    });
                }
            });
            $("#txtEmail").blur(function () {
                var email = $.trim($("#txtEmail").val());
                if (email.length >= 1) {
                    $.get("ajaxonly.aspx", { onlyvalue: email, flag: 1 }, function (data) {
                        if (data == 1) {
                            $("#txtEmail").next().show();
                            $("#txtEmail").next().text("此邮箱已经注册");
                            $("#btnSubmit").attr("disabled", "disabled");
                        } else {
                            $("#btnSubmit").attr("disabled", "");
                        }
                    });
                }
            });
        });
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
                <span>已有账号，现在就去 <a class="a_fontblue" href="login.aspx"><u>登录</u></a></span><b>用户注册</b></div>
            <!--Start-->
            <div class="TableAll">
                <table border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="name">
                            用户名：
                        </td>
                        <td>
                            <whir:textbox id="txtUserName" width="280px" cssclass="inputAll" runat="server" required="True"
                                maxlength="20" minlength="5" regular="UserName" errormessage="5-20字符,以字母开头的字母、数字、下划线组合"></whir:textbox>
                            <span class="f_note">用户名为5-20个字符(包括小写字母、数字、下划线)</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            密码：
                        </td>
                        <td>
                            <whir:textbox id="txtNewPassWord" runat="server" cssclass="inputAll" width="150px"
                                errorcss="form_error" tipcss="form_tip" required="True" minlength="6" maxlength="20"
                                message="请输入新密码" errormessage="两次密码输入不一致" requirederrormessage="6-20字符,任意字符组合"
                                textmode="Password"></whir:textbox>
                            <span class="f_note">6-20个字符组成，建议使用英文字母加数字或符号的组合密码</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            确认密码：
                        </td>
                        <td>
                            <whir:textbox id="txtNewPassWord2" runat="server" cssclass="inputAll" width="150px"
                                errorcss="form_error" tipcss="form_tip" required="True" minlength="6" maxlength="20"
                                message="请输入确认密码" requirederrormessage="6-20字符,任意字符组合" textmode="Password" errormessage="两次密码输入不一致"
                                compareto="txtNewPassWord"></whir:textbox>
                            <span class="f_note">输入重复密码</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            邮箱地址：
                        </td>
                        <td>
                            <whir:textbox id="txtEmail" width="280px" cssclass="inputAll" runat="server" required="True"
                                regular="Email" message="*" maxlength="60"></whir:textbox>
                            <span class="f_note">请输入您常用的邮箱，方便日后找回密码。</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            验证码：
                        </td>
                        <td>
                            <whir:textbox id="txtCode" runat="server" cssclass="inputAll" maxlength="4"
                                required="True" regular="Custom" validationexpression="\d{4}" width="100"></whir:textbox>
                            <img id="imgcode" src="checkcode.ashx" onclick="this.src=this.src+'?'" height="25px" style=" cursor:pointer" />
                            <a href="javascript:changeCode()" class="a_fontblue"><u>刷新验证码</u></a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="同意以下协议,提交" CssClass="btn" OnClick="btnSubmit_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="txt_agreement">
                <asp:Literal ID="ltAgreement" runat="server"></asp:Literal>
            </div>
            <!--End-->
        </div>
    </div>
    </form>
</body>
</html>
