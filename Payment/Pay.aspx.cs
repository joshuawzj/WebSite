using System;
using System.Data;
using Shop.Domain;
using Shop.Service;
using Whir.Framework;
public partial class Payment_Pay : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        int ID = RequestUtil.Instance.GetQueryInt("id", 0);//订单ID

        ShopOrderInfo orderInfo = ShopOrderInfoService.Instance.GetShopOrderInfo(ID);
        string paytype = "";
        if (orderInfo != null)
        {
            try
            {
                DataTable dt = PayInterface.Common.Tools.GetPayTypeList();
                DataRow dr = dt.Select("id='" + orderInfo.PaymentID.ToStr()+"'")[0];
                paytype = dr["paytype"].ToStr().ToLower();
            }
            catch
            {

            }

            string url = "";
            switch (paytype)
            {
                case "alipayinstant":
                    url = "AlipayInstant.aspx";//支付宝即时到账接口
                    break;
                case "99bill":
                    url = "99Bill.aspx";//快钱支付网关
                    break;
                case "chinabank":
                    url = "ChinaBank.aspx";//ChinaBank网银在线支付网关
                    break;
                case "chinapay":
                    url = "ChinaPay.aspx";//ChinaPay银联在线支付网关
                    break;
                case "tenpay":
                    url = "Tenpay.aspx";//财付通
                    break;
                case "alipay":
                    url = "Alipay.aspx";//支付宝担保交易接口
                    break;
                case "alipaywap": //支付宝手机H5支付
                    url = "";
                    break;
            }
            if (url != "")
            {
                Response.Redirect(url + "?id=" + ID.ToStr());
            }
            else
            {
                Response.Write("该支付方式暂不支持！");
            }
        }
        else
        {
            Response.Write("该订单已不存在！");
        }

    }
}
