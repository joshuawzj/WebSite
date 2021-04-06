<%@ Page Language="C#" AutoEventWireup="true" CodeFile="JsApiPayPage.aspx.cs" Inherits="WXPay_JsApiPayPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <meta http-equiv="content-type" content="text/html;charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/> 
    <title>微信支付</title>
</head>
    <script type="text/javascript">

               //调用微信JS api 支付
               function jsApiCall()
               {
                   WeixinJSBridge.invoke(
                   'getBrandWCPayRequest',
                   <%=wxJsApiParam%>,//josn串
                    function (res)
                    {
                       if (res.err_msg == "get_brand_wcpay_request:ok")
                       {
                            alert("支付成功...");
                        }else
                        {
                           alert("支付失败...");
                           WeixinJSBridge.call('closeWindow');
                        }
                         window.location.href="<%= Whir.Framework.WebUtil.Instance.AppPath() %>member/OrderList.aspx?state=1";
                     }
                    );
               }

               function callpay()
               {
                   if (typeof WeixinJSBridge == "undefined")
                   {
                       if (document.addEventListener)
                       {
                           document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                       }
                       else if (document.attachEvent)
                       {
                           document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                           document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                       }
                   }
                   else
                   {
                       jsApiCall();
                   }
               }
               
     </script>


<body>
    <form id="form1" runat="server">
        正在跳转支付页面...
    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
    </form>
</body>
</html>
