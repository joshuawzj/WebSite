using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;
using Shop.Service;
using Shop.Domain;
using Whir.Repository;
public partial class Ajax_Order : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string orderid = RequestUtil.Instance.GetFormString("orderid");
        ShopOrderInfo model = ShopOrderInfoService.Instance.GetShopOrderInfo(orderid);
        if (model != null)
        {
            if (model.IsPaid)
            {
                Response.Write(0);
                Response.End();
            }
            else
            {
                Response.Write(1);
                Response.End();
            }
        }
    }
}