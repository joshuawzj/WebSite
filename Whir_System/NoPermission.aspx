<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="NoPermission.aspx.cs" Inherits="Whir_System_NoPermission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="NoPermission-main">

        <div class="table-cell">
            <div class="main-height">
                <header>
			<h1><span>!</span>无权限</h1>
		</header>
                <figure>
        <figcaption>
			<h2>您无权限访问本页面！</h2>
            
            <ul class="list">
                <li>1.请确认您访问的网址是否正确</li>
                <li>2.请稍后5分钟或者重新登录再刷新本页面</li>
                 <li>3.如果执行以上操作后依然提示无权限，请联系管理员开放访问权限给您</li>
            </ul>
			<p>　</p>
		</figcaption>
		</figure>
            </div>
        </div>
        

    </div>
    <script type="text/javascript">
        $(function ($) {
            $(window).on("resize", function (e) {
                var wh = $(window).height()
                var mh = $("#NoPermission-main .main-height").height()
                //var navh=$("#breadcrumb").outerHeight()
                if (wh > mh) {
                    $("#NoPermission-main").height(wh - 116);
                } else {
                    $("#NoPermission-main").height("auto");
                }
            }).trigger("resize");
        });

    </script>
</asp:Content>

