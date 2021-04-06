using System;
using System.Collections.Generic;
using System.Data;
using Whir.Framework;
using Whir.Repository;

public class NativePay
{
    /**
    * 生成扫描支付模式一URL
    * @param productId 商品ID
    * @return 模式一URL
    */
    public string GetPrePayUrl(string productId)
    {
        LogHelper.Log("Native pay mode 1 url is producing...");

        WxPayData data = new WxPayData();
        data.SetValue("appid", WxPayConfig.APPID);//公众帐号id
        data.SetValue("mch_id", WxPayConfig.MCHID);//商户号
        data.SetValue("time_stamp", WxPayApi.GenerateTimeStamp());//时间戳
        data.SetValue("nonce_str", WxPayApi.GenerateNonceStr());//随机字符串
        data.SetValue("product_id", productId);//商品ID
        data.SetValue("sign", data.MakeSign());//签名
        string str = ToUrlParams(data.GetValues());//转换为URL串
        string url = "weixin://wxpay/bizpayurl?" + str;

        LogHelper.Log("Get native pay mode 1 url : " + url);
        return url;

    }

    /**
    * 生成直接支付url，支付url有效期为2小时,模式二
    * @param productId 商品ID
    * @return 模式二URL
    */
    public string GetPayUrl(string productId)
    {
        LogHelper.Log("Native pay mode 2 url is producing...");

        //------------------------------------扫码支付获取订单得到应付金额-----------------------------------------
        int total_fee = 0;
        DataTable DtOrder = DbHelper.CurrentDb.Query("select top 1 * from Whir_U_Forms where TypeID=30 and Whir_U_Forms=@0", productId.ToInt()).Tables[0];
        if (DtOrder.Rows.Count > 0)
        {
            total_fee = Convert.ToInt32(DtOrder.Rows[0]["Amount"].ToDecimalFormat() * 100);
        }
        //------------------------------------扫码支付获取订单得到应付金额END-----------------------------------------

        string out_trade_no = WxPayApi.GenerateOutTradeNo();

        WxPayData data = new WxPayData();
        data.SetValue("body", "订单");//商品描述
        data.SetValue("attach", "订单");//附加数据
        data.SetValue("out_trade_no", out_trade_no);//随机字符串
        data.SetValue("total_fee", total_fee);//总金额
        data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
        data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
        data.SetValue("goods_tag", "jjj");//商品标记
        data.SetValue("trade_type", "NATIVE");//交易类型
        data.SetValue("product_id", productId);//商品ID

        //------------------------------------扫码支付更改订单标识号(OutTradeNo字段)-----------------------------------------
        DbHelper.CurrentDb.Execute("UPDATE Whir_U_Forms SET OutTradeNo=@0 WHERE Whir_U_Forms_PID=@1", out_trade_no, DtOrder.Rows[0]["Whir_U_Forms_PID"].ToStr());
        //------------------------------------扫码支付更改订单标识号(OutTradeNo字段)END-----------------------------------------

        WxPayData result = WxPayApi.UnifiedOrder(data, 6);//调用统一下单接口
        string url = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码链接

        LogHelper.Log("Get native pay mode 2 url : " + url);
        return url;
    }

    /**
    * 参数数组转换为url格式
    * @param map 参数名与参数值的映射表
    * @return URL字符串
    */
    private string ToUrlParams(SortedDictionary<string, object> map)
    {
        string buff = "";
        foreach (KeyValuePair<string, object> pair in map)
        {
            buff += pair.Key + "=" + pair.Value + "&";
        }
        buff = buff.Trim('&');
        return buff;
    }
}
