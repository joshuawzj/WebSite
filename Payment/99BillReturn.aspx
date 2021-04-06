<%@ Page Language="C#"  StylesheetTheme="" EnableTheming="false"%>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Import Namespace="Shop.Service" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {

        string OrderId = HttpContext.Current.Request["orderId"].ToStr();
        string OrderAmount = HttpContext.Current.Request["orderAmount"].ToDecimal().ToString("f2");

        // Msg:为success时支付成功; 为false时支付失败; 为error时支付错误
        string Msg = HttpContext.Current.Request["msg"].ToStr();

        if (Msg == "success")
        {
            ShopOrderInfo order = ShopOrderInfoService.Instance.GetShopOrderInfo(OrderId.ToInt(0));
            if (order != null && order.Status != 0 && !order.IsPaid)
            {
                //更改订单状态

                order.Status = 0;
                
                ShopOrderInfoService.Instance.Update(order);
                
            }
            else
            {
                //重复交易, 本次未执行... 
            }

            Response.Redirect(PayInterface.Common.Tools.GetWebUrl()+"/Shop/member/orderinfo.aspx?id=" + order.OrderID.ToString());
        }
        else //交易失败
        {
            Response.Write("交易失败...");
            // Response.Redirect(PageUrl.Instance.Build("member/index"));
        }
        
    }
</script>

