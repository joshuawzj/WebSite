<%@ Page Language="C#"  StylesheetTheme="" EnableTheming="false"%>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Shop.Domain" %>
<%@ Import Namespace="Shop.Service" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
   
        SortedDictionary<string, string> sPara = GetRequestPost();

        if (sPara.Count > 0)//判断是否有带返回参数
        {
            Com.Alipay.Notify aliNotify = new Com.Alipay.Notify();
            bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

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

                if (Request.Form["trade_status"] == "WAIT_BUYER_PAY")
                {//该判断表示买家已在支付宝交易管理中产生了交易记录，但没有付款

                    //判断该笔订单是否在商户网站中已经做过处理（可参考“集成教程”中“3.4返回数据处理”）
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //如果有做过处理，不执行商户的业务程序
                    Response.Write("success");  //请不要修改或删除
                }
                else if (Request.Form["trade_status"] == "WAIT_SELLER_SEND_GOODS")
                {//该判断示买家已在支付宝交易管理中产生了交易记录且付款成功，但卖家没有发货

                    //判断该笔订单是否在商户网站中已经做过处理（可参考“集成教程”中“3.4返回数据处理”）
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //如果有做过处理，不执行商户的业务程序
                    ModifyOrderStatus(order_no, total_fee, trade_no);
                    Response.Write("success");  //请不要修改或删除
                } else if (Request.Form["trade_status"] == "WAIT_BUYER_CONFIRM_GOODS")
                {//该判断表示卖家已经发了货，但买家还没有做确认收货的操作

                    //判断该笔订单是否在商户网站中已经做过处理（可参考“集成教程”中“3.4返回数据处理”）
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //如果有做过处理，不执行商户的业务程序
                    ModifyOrderStatus(order_no, total_fee, trade_no);
                    Response.Write("success");  //请不要修改或删除
                }
                else if (Request.Form["trade_status"] == "TRADE_FINISHED")
                {//该判断表示买家已经确认收货，这笔交易完成

                    //判断该笔订单是否在商户网站中已经做过处理（可参考“集成教程”中“3.4返回数据处理”）
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //如果有做过处理，不执行商户的业务程序
                    ModifyOrderStatus(order_no, total_fee, trade_no);
                    Response.Write("success");  //请不要修改或删除
                }
                else
                {
                    ModifyOrderStatus(order_no, total_fee, trade_no);
                    Response.Write("success");  //其他状态判断。
                }
            }
            else //交易失败
            {
                Response.Write("fail");//写出"fail"失败提示, 提醒支付宝停止请求
            }
        }
        else
        {
            Response.Write("无通知参数");
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

