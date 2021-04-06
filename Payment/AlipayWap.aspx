<%@ Page Language="C#" StylesheetTheme="" EnableTheming="false" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Import Namespace="Shop.Service" %>
<%@ Import Namespace="Com.AlipayWap" %>
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

            ////////////////////////////////////////////请求参数////////////////////////////////////////////


            //商户订单号，商户网站订单系统中唯一订单号，必填
            string out_trade_no = model.OrderNo;

            //订单名称，必填
            string subject = "订单:" + out_trade_no;

            //付款金额，必填
            string total_fee = model.PayAmount.ToString("f2");

            //收银台页面上，商品展示的超链接，必填
            string show_url = PayInterface.Common.Tools.GetWebUrl() + "/shop/member/orders.aspx";

            //商品描述，可空
            string body = "订单:" + out_trade_no;



            ////////////////////////////////////////////////////////////////////////////////////////////////
            PayInterface.Model.AlipayInstant config = new PayInterface.Model.AlipayInstant().Insten();
           Config.partner = config.V_Mid;
           Config.seller_id = config.V_Mid;
           Config.key = config.PayOnlineKey;
           Config.notify_url = config.Notify_url;
           Config.return_url = config.ReturnURL;
            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", config.V_Mid);
            sParaTemp.Add("seller_id", config.V_Mid);
            sParaTemp.Add("_input_charset", "utf-8");
            sParaTemp.Add("service", Config.service);
            sParaTemp.Add("payment_type", "1");
            sParaTemp.Add("notify_url", config.Notify_url);
            sParaTemp.Add("return_url", config.ReturnURL);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("show_url", show_url);
            sParaTemp.Add("app_pay", "Y");//启用此参数可唤起钱包APP支付。
            sParaTemp.Add("body", body);
            //其他业务参数根据在线开发文档，添加参数.文档地址:https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.2Z6TSk&treeId=60&articleId=103693&docType=1
            //如sParaTemp.Add("参数名","参数值");

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "get", "确认");
            Response.Write(sHtmlText);
            
        }
        else
        {
            Response.Write("不存在此订单！");
        }
        
    }
    
</script>
