<%@ Page Language="C#" StylesheetTheme="" EnableTheming="false" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Import Namespace="Shop.Service" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
    
        Dictionary<string,string> dict = PayInterface.TenPay.GetNotify();
        if (dict != null)//交易成功
        {
            string transaction_id, orderNO, total_fee, pay_result;
            transaction_id = dict["transaction_id"];//财付通交易单号
            orderNO = dict["orderNO"];              //订单号/ID
            total_fee = dict["total_fee"];          //交易金额,以分为单位
            pay_result = dict["pay_result"];        //支付结果

            if (pay_result == "0")
            {
                ShopOrderInfo order = ShopOrderInfoService.Instance.GetShopOrderInfo(orderNO.ToInt(0));
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

                //告诉财付通处理成功,并且用户浏览器显示页面
                PayInterface.Model.TenPay pay = new PayInterface.Model.TenPay().Insten();

                //创建PayResponseHandler实例
                tenpay.PayResponseHandler resHandler = new tenpay.PayResponseHandler(HttpContext.Current);
                resHandler.setKey(pay.PayOnlineKey);
                if (resHandler.isTenpaySign())
                {
                    
                    resHandler.doShow(PayInterface.Common.Tools.GetWebUrl() + "/shop/member/orderinfo.aspx?id=" + order.OrderID.ToStr());
                }
            }
        }
        else //交易失败
        { 
            Response.Write("交易失败..."); 
        }
        
    }
</script>
