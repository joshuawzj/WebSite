using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Whir.Framework;
using Shop.Domain;
using Shop.Service;
using System.Data;
using Whir.Repository;

public partial class Payment_JsApiWxPay : System.Web.UI.Page
{

    /// <summary>
    /// 调用js获取收货地址时需要传入的参数
    /// 格式：json串
    /// 包含以下字段：
    ///     appid：公众号id
    ///     scope: 填写“jsapi_address”，获得编辑地址权限
    ///     signType:签名方式，目前仅支持SHA1
    ///     addrSign: 签名，由appid、url、timestamp、noncestr、accesstoken参与签名
    ///     timeStamp：时间戳
    ///     nonceStr: 随机字符串
    /// </summary>
    public static string wxEditAddrParam { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        int OrderId = Request.QueryString["id"].ToInt();
        if (OrderId == 0)
        {
            OrderId = Request.QueryString["state"].ToInt();
        }

        LogHelper.Log("page load");
        if (!IsPostBack)
        {
            JsApiPay jsApiPay = new JsApiPay(this);
            try
            {
                //调用【网页授权获取用户信息】接口获取用户的openid和access_token
                jsApiPay.GetOpenidAndAccessToken();

                //获取收货地址js函数入口参数
                wxEditAddrParam = jsApiPay.GetEditAddressParameters();
                string openid = jsApiPay.openid;

                //获取订单
                ShopOrderInfo model = ShopOrderInfoService.Instance.GetShopOrderInfo(OrderId);
                if (model != null)
                {
                    jsApiPay.total_fee = Convert.ToInt32(model.PayAmount * 100);
                }
                else
                {
                    Response.Write("不存在该订单,或者已经支付");
                    Response.End();
                }

                string url = "JsApiPayPage.aspx?id=" + OrderId + "&openid=" + openid + "&total_fee=" + jsApiPay.total_fee;
                Response.Redirect(url);
            }
            catch (Exception ex)
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "页面加载出错，请重试" + "</span>");
            }
        }

    }
}