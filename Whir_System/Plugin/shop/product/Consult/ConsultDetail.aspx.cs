using System;
using Shop.Domain;
using Shop.Service;
using Whir.ezEIP.Web;
using Whir.Framework;

public partial class Whir_System_Plugin_shop_product_Consult_ConsultDetail : SysManagePageBase
{
    protected ShopProInfo ShopProductInfo { get; set; }
    protected ShopConsult ShopConsultInfo { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("414"));
        int consultId = RequestUtil.Instance.GetQueryInt("ConsultID", 0);
        ShopConsultInfo = ShopConsultService.Instance.GetShopConsultById(consultId);
        if (ShopConsultInfo != null)
            ShopProductInfo = ShopProInfoService.Instance.GetShopProById(ShopConsultInfo.ProID);
        else
            ShopProductInfo = new ShopProInfo();
    }
}