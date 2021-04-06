<%@ Page Language="C#"  StylesheetTheme="" EnableTheming="false"%>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Import Namespace="Shop.Service" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
    
        SortedDictionary<string, string> sPara = GetRequestGet();

        if (sPara.Count > 0)//判断是否有带返回参数
        {
            Com.Alipay.Notify aliNotify = new Com.Alipay.Notify();
            bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);

            if (verifyResult)//验证成功
            {
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //请在这里加上商户的业务逻辑程序代码

                //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表
                string trade_no = Request.QueryString["trade_no"];              //支付宝交易号
                string order_no = Request.QueryString["out_trade_no"];	        //获取订单号
                string total_fee = Request.QueryString["price"];	            //获取总金额
                string subject = Request.QueryString["subject"];                //商品名称、订单名称
                string body = Request.QueryString["body"];                      //商品描述、订单备注、描述
                string buyer_email = Request.QueryString["buyer_email"];        //买家支付宝账号
                string receive_name = Request.QueryString["receive_name"];      //收货人姓名
                string receive_address = Request.QueryString["receive_address"];//收货人地址
                string receive_zip = Request.QueryString["receive_zip"];        //收货人邮编
                string receive_phone = Request.QueryString["receive_phone"];    //收货人电话
                string receive_mobile = Request.QueryString["receive_mobile"];  //收货人手机
                string trade_status = Request.QueryString["trade_status"];      //交易状态

                if (Request.QueryString["trade_status"] == "WAIT_SELLER_SEND_GOODS")
                {
                    //判断该笔订单是否在商户网站中已经做过处理（可参考“集成教程”中“3.4返回数据处理”）
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //如果有做过处理，不执行商户的业务程序
                    ModifyOrderStatus(order_no, total_fee, trade_no);
                }
                else
                {
                    Response.Write("trade_status=" + Request.QueryString["trade_status"]);
                    ModifyOrderStatus(order_no, total_fee, trade_no);                   
                }
            }
            else //交易失败
            {
                Response.Write("交易失败...");
                // Response.Redirect(PageUrl.Instance.Build("member/index"));
            }
        }
    }

    /// <summary>
    /// 修改订单状态
    /// </summary>
    /// <param name="order_no"></param>
    public void ModifyOrderStatus(string order_no, string total_fee, string trade_no)
    {
        ShopOrderInfo order = ShopOrderInfoService.Instance.GetShopOrderInfo(order_no);
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
        Response.Redirect(PayInterface.Common.Tools.GetWebUrl() + "/shop/member/orderinfo.aspx?id=" + order.OrderID.ToString()); 
    }
    
    /// <summary>
    /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public SortedDictionary<string, string> GetRequestGet()
    {
        int i = 0;
        SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
        NameValueCollection coll;
        //Load Form variables into NameValueCollection variable.
        coll = Request.QueryString;

        // Get names of all forms into a string array.
        String[] requestItem = coll.AllKeys;

        for (i = 0; i < requestItem.Length; i++)
        {
            sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
        }

        return sArray;
        
    }
</script>

