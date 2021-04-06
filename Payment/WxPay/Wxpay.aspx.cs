using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Service;
using Whir.Framework;
using Shop.Service;
using Shop.Domain;


public partial class Payment_Wxpay : System.Web.UI.Page
{
    public Whir.Domain.Column PageColumn;
    public Whir.Domain.SiteInfo PageSiteInfo;

    /// <summary>
    /// 是否支付
    /// </summary>
    public bool IsPaid { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        //ezip
        PageColumn = ServiceFactory.ColumnService.SingleOrDefault<Whir.Domain.Column>(1);
        PageSiteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<Whir.Domain.SiteInfo>(1);
        Whir.Label.LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn);
        DataBind();

        LogHelper.Log("page load007");

        string proid = RequestUtil.Instance.GetQueryInt("id", 0).ToStr();
        ShopOrderInfo model = ShopOrderInfoService.Instance.SingleOrDefault<ShopOrderInfo>(proid.ToInt());

        if (model == null)
        {
            Response.Write("参数错误...");
            Response.End();
        }
        else
        {
            IsPaid = model.IsPaid; //已经支付
        }

        NativePay nativePay = new NativePay();

        //生成扫码支付模式一url:用于线下
        // string url1 = nativePay.GetPrePayUrl(proid);

        //生成扫码支付模式二url：用于线上
        string url2 = nativePay.GetPayUrl(proid);

        //将url生成二维码图片
        Image1.ImageUrl = "WxPay/WxMakeQRCode.aspx?data=" + HttpUtility.UrlEncode(url2);
    }
}