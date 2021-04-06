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

            //必填参数//

            //请与贵网站订单系统中的唯一订单号匹配
            string out_trade_no = model.OrderNo;
            //订单名称，显示在支付宝收银台里的“商品名称”里，显示在支付宝的交易管理的“商品名称”的列表里。
            string subject = "订单:" + out_trade_no;
            //订单描述、订单详细、订单备注，显示在支付宝收银台里的“商品描述”里
            string body = "订单:" + out_trade_no;
            //订单总金额，显示在支付宝收银台里的“应付总额”里
            string price = model.PayAmount.ToString("f2");


            string logistics_fee = "0.00";                  				//物流费用，即运费。
            string logistics_type = "EXPRESS";				                //物流类型，三个值可选：EXPRESS（快递）、POST（平邮）、EMS（EMS）
            string logistics_payment = "SELLER_PAY";            			//物流支付方式，两个值可选：SELLER_PAY（卖家承担运费）、BUYER_PAY（买家承担运费）

            string quantity = "1";              							//商品数量，建议默认为1，不改变值，把一次交易看成是一次下订单而非购买一件商品。

            //选填参数//

            //买家收货信息（推荐作为必填）
            //该功能作用在于买家已经在商户网站的下单流程中填过一次收货信息，而不需要买家在支付宝的付款流程中再次填写收货信息。
            //若要使用该功能，请至少保证receive_name、receive_address有值
            //收货信息格式请严格按照姓名、地址、邮编、电话、手机的格式填写
            string receive_name = "";			                            //收货人姓名，如：张三
            string receive_address = "";			                        //收货人地址，如：XX省XXX市XXX区XXX路XXX小区XXX栋XXX单元XXX号
            string receive_zip = "";                  			            //收货人邮编，如：123456
            string receive_phone = "";                		                //收货人电话号码，如：0571-81234567
            string receive_mobile = "";               		                //收货人手机号码，如：13312341234

            //订单列表地址，不允许加?id=123这类自定义参数
            string show_url = PayInterface.Common.Tools.GetWebUrl() + "/shop/member/orders.aspx"; 
            
            
            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            System.Collections.Generic.SortedDictionary<string, string> sParaTemp = new System.Collections.Generic.SortedDictionary<string, string>();
            sParaTemp.Add("body", body);
            sParaTemp.Add("logistics_fee", logistics_fee);
            sParaTemp.Add("logistics_payment", logistics_payment);
            sParaTemp.Add("logistics_type", logistics_type);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("payment_type", "1");
            sParaTemp.Add("price", price);
            sParaTemp.Add("quantity", quantity);
            sParaTemp.Add("receive_address", receive_address);
            sParaTemp.Add("receive_mobile", receive_mobile);
            sParaTemp.Add("receive_name", receive_name);
            sParaTemp.Add("receive_phone", receive_phone);
            sParaTemp.Add("receive_zip", receive_zip);
            sParaTemp.Add("show_url", show_url);
            sParaTemp.Add("subject", subject);

            //构造即时到帐接口表单提交HTML数据，无需修改
            Com.Alipay.Service ali = new Com.Alipay.Service();
            string sHtmlText = ali.Create_partner_trade_by_buyer(sParaTemp);
            Response.Write(sHtmlText);
            
        }
        else
        {
            Response.Write("不存在此订单！");
        }
        
    }
    
</script>
