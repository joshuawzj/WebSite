<%@ Page Language="C#"  StylesheetTheme="" EnableTheming="false" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Import Namespace="Shop.Service" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {

        int MemberID = WebUser.GetUserValue("Whir_Mem_Member_PID").ToInt(0);
        if (MemberID == 0)
        {
            //未登录处理
            string returnUrl = Server.UrlEncode(Request.Url.ToStr());
            string url = PayInterface.Common.Tools.GetWebUrl() + "/shop/member/login.aspx?BackPageUrl=" + returnUrl;
            Response.Redirect(url, true);
        }
        
        int OrdersId = RequestUtil.Instance.GetQueryInt("id", -1);
        ShopOrderInfo order = ShopOrderInfoService.Instance.GetShopOrderInfo(OrdersId);
        if (order != null &&MemberID == order.MemberID)
        {
            if (order.IsPaid)
            {
                //已支付处理
                Response.Write("该订单已经支付过！");
                Response.End();
            }
            
            //转至支付页面
            string ReturnURL = PayInterface.Common.Tools.GetWebUrl() + "/payment/ChinaBankReceive.aspx";
            string NotifyURL = PayInterface.Common.Tools.GetWebUrl() + "/payment/ChinaBankAutoReceive.aspx";

            decimal Money = order.PayAmount;
            if (Money > 0.00m)
            {
                PayInterface.ChinaBank.Pay(order.OrderNo, order.OrderNo, Money, ReturnURL);
            }
        }
        else
        {
            Response.Write("不存在此订单！");
        }
        
    }

    
</script>