
using System;
using System.Web.UI.WebControls;

using Whir.Framework;
using Shop.Service;
using Shop.Domain;

public partial class whir_system_Plugin_shop_product_consult_consultlist : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("414"));
    }
}