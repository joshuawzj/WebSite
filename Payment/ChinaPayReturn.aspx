<%@ Page Language="C#"  StylesheetTheme="" EnableTheming="false"%>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Import Namespace="Shop.Service" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
    
        Dictionary<string, string> dict = PayInterface.ChinaPay.GetNotify();
        if (dict != null)//交易成功
        {
            ShopOrderInfo order = ShopOrderInfoService.Instance.GetShopOrderInfo(dict["DB_OrdId"].ToInt(-1));
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

            Response.Redirect(PayInterface.Common.Tools.GetWebUrl() + "/shop/member/orderinfo.aspx?id=" + order.OrderID.ToStr());
        }
        else //交易失败
        {
            Response.Write("交易失败...");
            System.Threading.Thread.Sleep(3000);
            Response.Redirect(PayInterface.Common.Tools.GetWebUrl() + "/shop/member/personal.aspx");
        }
        
    }
    
</script>

