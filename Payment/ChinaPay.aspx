<%@ Page Language="C#"  StylesheetTheme="" EnableTheming="false"%>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Import Namespace="Shop.Service" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {

        int MemberID = WebUser.GetUserValue("Whir_Mem_Member_PID").ToInt(0);
        if (MemberID == 0 || !WebUser.IsLogin())
        {
            //未登录处理
            string returnUrl = Server.UrlEncode(Request.Url.ToStr());
            string url = PayInterface.Common.Tools.GetWebUrl() + "/shop/member/login.aspx?BackPageUrl=" + returnUrl;
            Response.Redirect(url, true);
        }

        int OrdersId = RequestUtil.Instance.GetQueryInt("id", -1);
        ShopOrderInfo model = ShopOrderInfoService.Instance.GetShopOrderInfo(OrdersId);
        if (model != null && MemberID == model.MemberID)
        {
            if (model.IsPaid)
            {
                //已支付处理
                Response.Write("该订单已经支付过！");
                Response.End();
            }

            //转至支付页面
            string ReturnURL = PayInterface.Common.Tools.GetWebUrl() + "/payment/ChinaPayReturn.aspx";
            string NotifyURL = PayInterface.Common.Tools.GetWebUrl() + "/payment/ChinaPayNotify.aspx";
            string ErrorURL = PayInterface.Common.Tools.GetWebUrl() + "/payment/ChinaPayError.aspx";
            decimal Money = model.PayAmount;
            if (Money > 0.00m)
            {
                PayInterface.ChinaPay.Pay(model.OrderID.ToStr(), model.OrderNo, Money, ReturnURL, NotifyURL, ErrorURL);
            }
        }
        else
        {
            Response.Write("不存在此订单！");
        }
        
    }
    
</script>

