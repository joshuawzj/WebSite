<%@ Page Language="C#" StylesheetTheme="" EnableTheming="false" %>
<%@ Import Namespace="System.Data" %>
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
        ShopOrderInfo order = ShopOrderInfoService.Instance.GetShopOrderInfo(OrdersId);
        DataTable dt = ShopOrderProductService.Instance.GetShopOrderProductsByOrderID(OrdersId);
        if (order != null && dt!=null && MemberID == order.MemberID)
        {
            if (order.IsPaid)
            {
                //已支付处理
                Response.Write("该订单已经支付过！");
                Response.End();
            }

            //转至支付页面
            string NotifyURL = PayInterface.Common.Tools.GetWebUrl() + "/payment/99BillNotify.aspx";
            decimal Money = order.PayAmount;
            if (Money > 0.00m)
            {
                string ProNames = "";
                 int ProCount =0;
                foreach (DataRow item in dt.Rows)
                {
                    ProNames += item["ProName"].ToStr() + "，";
                    ProCount += item["Count"].ToInt(0);
                }
                ProNames = ProNames.TrimEnd('，');

               

                //传入参数: 订单号, 商品名称, 订单描述信息, 支付人名, 商品总数量, 支付金额, 通知地址
                PayInterface._99Bill.Pay(order.OrderNo, ProNames, order.OrderNo, WebUser.GetUserValue("LoginName"), ProCount, Money, NotifyURL);
            }
        }
        else
        {
            Response.Write("不存在此订单！");
        }
        
    }
    
</script>
