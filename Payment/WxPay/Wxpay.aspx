<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Wxpay.aspx.cs" Inherits="Payment_Wxpay" %>

<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <wtl:include ID="Include1" runat="server" FileName="head.html">
    </wtl:include>

    <style type="text/css">
        
        /*微信支付*/
        .wechat{ min-height:600px; width:840px; margin:30px auto;}
        .wechat .title{ font-size:16px; color:#333; font-weight:bold; padding:30px 0;}
        .wechat .left,
        .wechat .right{ width:420px; float:left; text-align:center;}
        .wechat .left .pic{ width:240px; height:240px; margin:0 auto; border:2px solid #ddd; text-align:center; line-height:240px; background:#fff; padding:20px;}
        .wechat .left .pic img{ vertical-align: middle; max-width:100%; max-height:100%; vertical-align: middle;}
        .wechat .left .info{ width:182px; background:url(../images/saoyisao.jpg); height:65px; padding-left:100px; text-align:left; color:#fff; font-size:16px; margin:35px auto 0; line-height:32px;}
        
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <wtl:include ID="Include2" runat="server" FileName="top.html">
    </wtl:include>
    <!--top End-->
     <section class="Contain">
           <div class="Contain">
             <!--网页主体部分 开始-->
            <!--页面内容-->
            <div class="wechat">
    	        <div class="title">微信支付</div>
                <div class="left">
        	        <div class="pic" id="QRCode1">
                        <%if (!IsPaid)
                         {%>
                             <asp:Image ID="Image1" runat="server" Style="width: 240px; height: 240px;" />
                        <%}%>
                        <%else
                         { %>
                            <script type="text/javascript">
                                alert("该订单已经支付无需支付");
                                location.href = location.href = "<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>index.aspx";
                            </script>
                        <%}%>
        	        </div>
                    <div class="info">请使用微信扫一扫<br>扫描二维码支付</div>
                </div>
                <div class="right"><img src="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>cn/images/shouji.jpg" /></div>
                <div class="clear"></div>
            </div>
        <!--网页主体部分 结束-->
    </div>
     </section>

    <wtl:include ID="Include4" runat="server" FileName="bottom.html">
    </wtl:include>
    </form>
</body>
</html>
<%if (!IsPaid)
  {%>
<script type="text/javascript">
        var orderid = <%=Server.UrlDecode(RequestUtil.Instance.GetQueryString("id")) %>;
        setInterval(function () {
            $.ajax({
                url: "<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>Ajax/Order.aspx",
                type: "post",
                data:{orderid:orderid},
                success: function (data) {
                    if(data!="0"){
                        alert("支付成功");
                         location.href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>index.aspx";
                       // location.href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>member/order.aspx?id="+orderid+"";
                        //location.href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>shop/member/orders.aspx";
                    }
                }
            })
        }, 3000)
        
</script>
<%}%>
