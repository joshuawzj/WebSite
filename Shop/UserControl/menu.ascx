<%@ Control Language="C#" AutoEventWireup="true" CodeFile="menu.ascx.cs" Inherits="Shop_UserControl_menu" EnableViewState="false" %>

<div class="Menu">
    <h1><em>会员中心</em><i>Member Center</i></h1>
    <div class="box">
        <h2>个人帐户</h2>
        <ul>
            <li><a href="personal.aspx">个人信息</a></li>
            <li><a href="password.aspx">修改密码</a></li>
        </ul>
        <h2>订单中心</h2>
        <ul>
            <li><a href="orders.aspx">我的订单</a></li>
            <li><a href="advisory.aspx">我的咨询</a></li>
        </ul>
        <h2>退出</h2>
        <ul>
            <li>
                <a href="loginout.aspx">退出</a>
            </li>
        </ul>

    </div>
</div>
<script>
    $(function () {


        if (location.href.toLocaleLowerCase().indexOf("personal.aspx") > 0) {
            $(".box a[href='personal.aspx']").addClass("aSelect");
        }
        else if (location.href.toLocaleLowerCase().indexOf("email.aspx") > 0) {
            $(".box a[href='personal.aspx']").addClass("aSelect");
        }
        else if (location.href.toLocaleLowerCase().indexOf("password.aspx") > 0) {
            $(".box a[href='password.aspx']").addClass("aSelect");
        }
        else if (location.href.toLocaleLowerCase().indexOf("orders.aspx") > 0) {
            $(".box a[href='orders.aspx']").addClass("aSelect");
        }
        else if (location.href.toLocaleLowerCase().indexOf("orderinfo.aspx") > 0) {
            $(".box a[href='orders.aspx']").addClass("aSelect");
        }
        else if (location.href.toLocaleLowerCase().indexOf("advisory.aspx") > 0) {
            $(".box a[href='advisory.aspx']").addClass("aSelect");
        }
    });
</script>
