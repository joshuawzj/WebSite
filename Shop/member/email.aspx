<%@ Page Language="C#" AutoEventWireup="true" CodeFile="email.aspx.cs" Inherits="Shop_member_email" %>

<%@ Register src="../UserControl/menu.ascx" tagname="menu" tagprefix="uc1" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader"
    TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>会员中心-个人信息-修改安全邮箱</title>
    <link href="../css/shop_whir.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript">
        $(function () {
            $("#txtTakeEmail").blur(function () {
                var email = $.trim($("#txtTakeEmail").val());
                if (email.length >= 1) {
                    $.get("ajaxonly.aspx", { onlyvalue: email, flag: 1 }, function (data) {
                        if (data == 1) {
                            $("#txtTakeEmail").next().show();
                            $("#txtTakeEmail").next().text("此邮箱已经注册");
                            $("#btnSend").attr("disabled", "disabled");
                        } else {
                            $("#btnSend").attr("disabled", "");
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
        <div class="Sidebar">
            <uc1:menu ID="menu1" runat="server" />
        </div>
        <div class="Main">
            <div class="Current">
                <span>当前位置：<a href="personal.aspx">会员中心</a> > <a href="personal.aspx">个人信息</a> > <i>修改安全邮箱</i></span><b>修改安全邮箱</b></div>
            <!--Start-->
            <div class="TableAll">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td class="name">
                            安全邮箱：
                        </td>
                        <td>
                            <asp:Literal ID="ltOldEmail" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td class="name">
                            修改邮箱：
                        </td>
                        <td>
                            <whir:TextBox ID="txtTakeEmail" CssClass="inputAll" runat="server" message="*" Regular="Email"
                                Required="true" MaxLength="40"></whir:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSend" runat="server" Text="发送验证邮件" OnClick="btnSend_Click" />
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
