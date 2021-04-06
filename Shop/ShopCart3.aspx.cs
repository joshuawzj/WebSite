using System;
using Shop.Common;
using Shop.Domain;
using Shop.Service;
using Whir.Framework;
public partial class Shop_ShopCart3 :PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WebUser.IsLogin("shop/member/login.aspx");
        if (!IsPostBack)
        {
            int orderID = RequestUtil.Instance.GetQueryInt("orderId",0);
            ShopOrderInfo orderInfo = ShopOrderInfoService.Instance.GetShopOrderInfo(orderID);
           
            if (orderInfo != null)
            {
                litOrderNo.Text = "订单号："+orderInfo.OrderNo;
                if (orderInfo.PaymentID > 0)
                {

                    ClientScript.RegisterStartupScript(Page.GetType(), "pay", string.Format("window.open('{0}Payment/Pay.aspx?id={1}')", AppName, orderID), true);
                }
            }
            else
            {
                litOrderNo.Text = "您的订单号：订单异常，请及时联系我们！";
                phTip.Visible = false;
            }
        }
    }
}