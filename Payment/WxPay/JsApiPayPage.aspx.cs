using Shop.Domain;
using Shop.Service;
using System;
using Whir.Framework;

public partial class WXPay_JsApiPayPage : System.Web.UI.Page
{
    public static string wxJsApiParam { get; set; } //H5调起JS API参数

    protected void Page_Load(object sender, EventArgs e)
    {
        LogHelper.Log("page load");
        if (!IsPostBack)
        {
            string openid = Request.QueryString["openid"];
            int total_fee = 0;
            int OrderID = RequestUtil.Instance.GetQueryInt("id", 0);

            // Log.Info(" ", "openid: " + openid + "total_fee: " + total_fee + "OrderID: " + OrderID);

            //获取订单
            ShopOrderInfo model = ShopOrderInfoService.Instance.GetShopOrderInfo(OrderID);
            if (model != null)
            {
                total_fee = Convert.ToInt32(model.PayAmount * 100);
            }
            else
            {
                Response.Write("不存在该订单,或者已经支付");
                Response.End();
            }

            //若传递了相关参数，则调统一下单接口，获得后续相关接口的入口参数
            JsApiPay jsApiPay = new JsApiPay(this);
            jsApiPay.openid = openid;
            jsApiPay.total_fee = total_fee;

            //JSAPI支付预处理
            try
            {
                WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult();
                wxJsApiParam = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                LogHelper.Log("wxJsApiParam : " + wxJsApiParam);
                //在页面上显示订单信息
                //  Response.Write("<span style='color:#00CD00;font-size:20px'>订单详情：</span><br/>");
                //   Response.Write("<span style='color:#00CD00;font-size:20px'>" + unifiedOrderResult.ToPrintStr() + "</span>");

                Literal1.Text = "<script>callpay();</script>";
            }
            catch (Exception ex)
            {
                Response.Write("<span style='color:#FF0000;font-size:20px'>" + "下单失败，请返回重试" + "</span>");

            }
        }
    }

}