<%@ Page Language="C#" AutoEventWireup="true" CodeFile="personal.aspx.cs" Inherits="Shop_member_personal" %>
<%@ Register src="../UserControl/menu.ascx" tagname="menu" tagprefix="uc1" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader"
    TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员中心-个人信息</title>
    <link href="../css/shop_whir.css" rel="stylesheet" type="text/css" /> 
    <script src="../../res/js/DatePicker/WdatePicker.js" type="text/javascript"></script>
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
                <span>当前位置：<a href="personal.aspx">会员中心</a> > <i>个人信息</i></span><b>个人信息</b></div>
            <!--Start-->
            <div class="TableAll">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="name">
                            用户名：
                        </td>
                        <td>
                            <asp:Literal ID="ltLoginName" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            安全邮箱：
                        </td>
                        <td>
                            <asp:Literal ID="ltEmail" runat="server"></asp:Literal>
                            <a href="email.aspx" class="a_fontbtn">修改并验证>></a>
                        </td>
                    </tr>
                    <tr class="trbg">
                        <td class="name">
                            昵称：
                        </td>
                        <td>
                            <whir:TextBox ID="txtNickName" CssClass="inputAll" runat="server" message="*" MaxLength="10"></whir:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            性别：
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblSex" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="男" Selected="Selected">男</asp:ListItem>
                                <asp:ListItem Value="女">女</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr class="trbg">
                        <td class="name">
                            出生日期：
                        </td>
                        <td>
                            <whir:DateBox ID="txtBridth" CssClass="inputAll" runat="server" message="*"></whir:DateBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            收 货 人：
                        </td>
                        <td>
                            <whir:TextBox ID="txtTakeName" CssClass="inputAll" runat="server" message="*" MaxLength="10"></whir:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            地 址：
                        </td>
                        <td>
                            <whir:DropDownList ID="ddlProv" runat="server" Required="true" OnSelectedIndexChanged="ddlProv_SelectedIndexChanged"
                                AutoPostBack="true">
                            </whir:DropDownList>
                            <whir:DropDownList ID="ddlCity" runat="server" AutoPostBack="true" Required="true"
                                OnSelectedIndexChanged="ddlCity_SelectedIndexChanged">
                            </whir:DropDownList>
                            <whir:DropDownList ID="ddlArea" runat="server" Required="true">
                            </whir:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            街道信息：
                        </td>
                        <td>
                            <whir:TextBox ID="txtAddress" CssClass="inputAll" runat="server" message="*" MaxLength="128"></whir:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            手机号码：
                        </td>
                        <td>
                            <whir:TextBox ID="txtMobile" CssClass="inputAll" runat="server" message="*" Required="true"
                                Regular="Custom" ValidationExpression="^0?(13[0-9]|14[5-9]|15[012356789]|166|17[0-8]|18[0-9]|19[8-9])[0-9]{8}$" MaxLength="13"></whir:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            固定电话：
                        </td>
                        <td>
                            <whir:TextBox ID="txtTel" CssClass="inputAll" runat="server" message="*" Required="true"
                                Regular="Phone" MaxLength="13"></whir:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            邮箱地址：
                        </td>
                        <td>
                            <whir:TextBox ID="txtTakeEmail" CssClass="inputAll" runat="server" message="*" Regular="Email"
                                MaxLength="40"></whir:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            邮政编码：
                        </td>
                        <td>
                            <whir:TextBox ID="txtPostCode" CssClass="inputAll" runat="server" message="*" Regular="Zipcode"
                                MaxLength="13"></whir:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="提交" CssClass="btn" OnClick="btnSubmit_Click" />
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
