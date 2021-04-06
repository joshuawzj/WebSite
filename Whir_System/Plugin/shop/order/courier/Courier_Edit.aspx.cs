/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：courier_edit.aspx.cs
 * 文件描述：添加、编辑配送方式
 * 
 * 创建标识: liuyong 2013-02-05
 * 
 * 修改标识：lurong 2013-02-21  用快递公司代码字段替掉接口字段
 */
using System;

using Whir.Framework;
using Shop.Domain;
using Shop.Service;
using Whir.Service;
using Whir.Domain;

public partial class whir_system_Plugin_shop_order_courier_courier_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 配送方式ID
    /// </summary>
    public int CourierID { get; set; }

    protected ShopCourier CurrentShopCourier { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("419"));
        CourierID = RequestUtil.Instance.GetQueryInt("courierid", 0);
        if (!IsPostBack)
        {
            CurrentShopCourier = ShopCourierService.Instance.SingleOrDefault<ShopCourier>(CourierID);
            if (CurrentShopCourier == null)
            {
                CurrentShopCourier = new ShopCourier();
            }
        }
    }
}