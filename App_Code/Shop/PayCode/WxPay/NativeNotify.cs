using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Repository;
using Shop.Domain;
using Shop.Service;
using System.Data;


/// <summary>
/// 扫码支付模式一回调处理类
/// 接收微信支付后台发送的扫码结果，调用统一下单接口并将下单结果返回给微信支付后台
/// </summary>
public class NativeNotify : Notify
{
    public NativeNotify(Page page)
        : base(page)
    {

    }

    public override void ProcessNotify()
    {
        WxPayData notifyData = GetNotifyData();

        LogHelper.Log("修改订单状态 Start0001...", "");

        //检查openid和product_id是否返回
        if (!notifyData.IsSet("openid"))
        {
            WxPayData res = new WxPayData();
            res.SetValue("return_code", "FAIL");
            res.SetValue("return_msg", "回调数据异常");
            LogHelper.Log("The data WeChat post is error : " + res.ToXml());
            page.Response.Write(res.ToXml());
            page.Response.End();
        }

        //调统一下单接口，获得下单结果
        string openid = notifyData.GetValue("openid").ToString();
        string product_id = "";

        //-----------------------（扫码支付,公众号支付）获取订单-------------------------------
        string out_trade_no = notifyData.GetValue("out_trade_no").ToString();

        LogHelper.Log("out_trade_no: " + out_trade_no);
        ShopOrderInfo order = ShopOrderInfoService.Instance.GetShopOrderInfo(out_trade_no);
        //-----------------------（扫码支付,公众号支付）获取订单END-------------------------------

        WxPayData unifiedOrderResult = new WxPayData();
        try
        {
            unifiedOrderResult = UnifiedOrder(openid, product_id);
        }
        catch (Exception ex)//若在调统一下单接口时抛异常，立即返回结果给微信支付后台
        {
            WxPayData res = new WxPayData();
            res.SetValue("return_code", "FAIL");
            res.SetValue("return_msg", "统一下单失败");
            LogHelper.Log("UnifiedOrder failure : " + res.ToXml());
            page.Response.Write(res.ToXml());
            page.Response.End();
        }

        //若下单失败，则立即返回结果给微信支付后台
        if (!unifiedOrderResult.IsSet("appid") || !unifiedOrderResult.IsSet("mch_id") || !unifiedOrderResult.IsSet("prepay_id"))
        {
            WxPayData res = new WxPayData();
            res.SetValue("return_code", "FAIL");
            res.SetValue("return_msg", "统一下单失败");
            LogHelper.Log("UnifiedOrder failure : " + res.ToXml());
            page.Response.Write(res.ToXml());
            page.Response.End();
        }

        LogHelper.Log("统一下单成功,则返回成功结果给微信支付后台 : ", " ");


        //统一下单成功,则返回成功结果给微信支付后台
        WxPayData data = new WxPayData();
        data.SetValue("return_code", "SUCCESS");
        data.SetValue("return_msg", "OK");
        data.SetValue("appid", WxPayConfig.APPID);
        data.SetValue("mch_id", WxPayConfig.MCHID);
        data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());
        data.SetValue("prepay_id", unifiedOrderResult.GetValue("prepay_id"));
        data.SetValue("result_code", "SUCCESS");
        data.SetValue("err_code_des", "OK");
        data.SetValue("sign", data.MakeSign());

        LogHelper.Log("UnifiedOrder success , send data to WeChat : " + data.ToXml());

        //----------------------（扫码支付,公众号支付都会运行到这里进行）更改支付状态-----------------------------------
        if (order != null && order.Status != 0)
        {
            order.IsPaid = true;
            order.Status = 0;
            ShopOrderInfoService.Instance.Update(order);
        }
        else
        {
            //重复交易, 本次未执行... 
        }
        //---------------------（扫码支付,公众号支付都会运行到这里进行）更改支付状态END---------------------------------

        page.Response.Write(data.ToXml());
        page.Response.End();
    }

    private WxPayData UnifiedOrder(string openId, string productId)
    {
        //统一下单
        WxPayData req = new WxPayData();
        req.SetValue("body", "test");
        req.SetValue("attach", "test");
        req.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());
        req.SetValue("total_fee", 1);
        req.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));
        req.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));
        req.SetValue("goods_tag", "test");
        req.SetValue("trade_type", "NATIVE");
        req.SetValue("openid", openId);
        req.SetValue("product_id", productId);
        WxPayData result = WxPayApi.UnifiedOrder(req, 6);
        return result;
    }
}
