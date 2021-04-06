<%@ Page Language="C#"  StylesheetTheme="" EnableTheming="false"%>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Import Namespace="Shop.Service" %>
<%@ Import Namespace="Com.AlipayWap" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
    
        SortedDictionary<string, string> sPara = GetRequestPost();

        if (sPara.Count > 0)//判断是否有带返回参数
        {
            //Com.AlipayInstant.Notify aliNotify = new Com.AlipayInstant.Notify();
            AlipayNotify alipayNotify = new AlipayNotify();
            bool verifyResult = alipayNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

            if (verifyResult)//验证成功
            {
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //请在这里加上商户的业务逻辑程序代码

                //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表
                string trade_no = Request.Form["trade_no"];         //支付宝交易号
                string order_no = Request.Form["out_trade_no"];     //获取订单号
                string total_fee = Request.Form["total_fee"];       //获取总金额
                string subject = Request.Form["subject"];           //商品名称、订单名称
                string body = Request.Form["body"];                 //商品描述、订单备注、描述
                string buyer_email = Request.Form["buyer_email"];   //买家支付宝账号
                string trade_status = Request.Form["trade_status"]; //交易状态

                if (Request.Form["trade_status"] == "TRADE_FINISHED" || Request.Form["trade_status"] == "TRADE_SUCCESS")
                {
                    ShopOrderInfo order = ShopOrderInfoService.Instance.GetShopOrderInfo(order_no);
                    if (order != null && order.Status != 0 && !order.IsPaid)
                    {
                        order.Status = 0;
                        ShopOrderInfoService.Instance.Update(order);
                    }
                    else
                    {
                        //重复交易, 本次未执行... 
                    }

                    Response.Write("success");//写出"success"成功提示, 提醒支付宝停止请求
                }
            }
            else //交易失败
            {
                Response.Write("fail");//写出"fail"失败提示, 提醒支付宝停止请求
            }
        }
    }

    /// <summary>
    /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
    /// </summary>
    /// <returns>request回来的信息组成的数组</returns>
    public SortedDictionary<string, string> GetRequestPost()
    {
        int i = 0;
        SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
        NameValueCollection coll;
        //Load Form variables into NameValueCollection variable.
        coll = Request.Form;

        // Get names of all forms into a string array.
        String[] requestItem = coll.AllKeys;

        for (i = 0; i < requestItem.Length; i++)
        {
            sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
        }

        return sArray;
        
    }
</script>

