<%@ Page Language="C#" AutoEventWireup="true" CodeFile="password.aspx.cs" Inherits="Shop_member_password" %>
<%@ Register src="../UserControl/menu.ascx" tagname="menu" tagprefix="uc1" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader"
    TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员中心-修改密码</title>
    <link href="../css/shop_whir.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
     <!--共用头部-->
     <center>
        <img src="../images/header.jpg" /></center>
    <!--共用头部-->
        <uc2:CategoryHeader ID="CategoryHeader1" runat="server" />
    <div class="ShopContain">
        <div class="Sidebar">
            <uc1:menu ID="menu1" runat="server" />
        </div>
        <div class="Main">
            <div class="Current">
                <span>当前位置：<a href="personal.aspx">会员中心</a> > <i>修改密码</i></span><b>修改密码</b></div>
            <!--Start-->
            <div class="TableAll">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="name">
                            旧密码：
                        </td>
                        <td>
                            <whir:TextBox ID="txtOldPassword" runat="server" CssClass="inputAll" ErrorCss="form_error"
                                TipCss="form_tip" Required="True" MinLength="6" MaxLength="20" message="请输入旧密码"
                                RequiredErrorMessage="6-20字符,任意字符组合" TextMode="Password"></whir:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            新密码：
                        </td>
                        <td>
                            <whir:TextBox ID="txtNewPassWord" runat="server" CssClass="inputAll" ErrorCss="form_error"
                                TipCss="form_tip" Required="True" MinLength="6" MaxLength="20" message="请输入新密码"
                                ErrorMessage="两次密码输入不一致" RequiredErrorMessage="6-20字符,任意字符组合" TextMode="Password"></whir:TextBox>
                            <span class="f_note">6-20个字符组成，建议使用英文字母加数字或符号的组合密码</span>
                        </td>
                    </tr>
                    <tr class="trbg">
                        <td class="name">
                            重复新密码：
                        </td>
                        <td>
                            <whir:TextBox ID="txtNewPassWord2" runat="server" CssClass="inputAll" ErrorCss="form_error"
                                TipCss="form_tip" Required="True" MinLength="6" MaxLength="20" message="请输入确认密码"
                                RequiredErrorMessage="6-20字符,任意字符组合" TextMode="Password" ErrorMessage="两次密码输入不一致"
                                CompareTo="txtNewPassWord"></whir:TextBox>
                            <span class="f_note">输入重复密码</span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="保存" CssClass="btn" 
                                onclick="btnSubmit_Click" /> &nbsp;&nbsp;&nbsp;&nbsp;<asp:Literal ID="ltMsg" runat="server"></asp:Literal>
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
