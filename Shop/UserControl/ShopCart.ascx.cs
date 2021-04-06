/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：Shop_UserControl_ShopCart.cs
 * 文件描述：购物车
 * 
 * 创建标识: lurong 2013-2-18
 * 
 * 修改标识：
 */
using System;
using System.Data;
using Shop.Common;
using Shop.Service;
using Whir.Framework;
public partial class Shop_UserControl_ShopCart : UserControlBase
{
    public Decimal Total = 0;//总计
    public int Count = 0;//总数

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindCartList();
        }
    }

    /// <summary>
    /// 更新购物车
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdateShopCart_Click(object sender, EventArgs e)
    {
        string proNum = RequestUtil.Instance.GetFormString("pronum");
        foreach (string item in proNum.Split(','))
        {
            ShopCartService.Instance.AddCart(item.Split('_')[1].ToInt(0), item.Split('_')[0].ToInt(0));
        }
        Response.Redirect(AppName + "Shop/ShopCart.aspx");
    }


    /// <summary>
    /// 移除商品
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRemovePro_Click(object sender, EventArgs e)
    {
        int cartid = RequestUtil.Instance.GetFormString("dels").ToInt(0);
        if (cartid>0)
        {
            ShopCartService.Instance.RemovePro(cartid);
        }
        Response.Redirect(AppName + "Shop/ShopCart.aspx");
    }

     /// <summary>
    /// 移除选中的商品
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnRemoveSelPro_Click(object sender, EventArgs e)
    {
        string cartids = RequestUtil.Instance.GetFormString("dels");
        foreach (string itemid in cartids.Split(','))
        {
            if (itemid.ToInt(0) > 0)
            {
                ShopCartService.Instance.RemovePro(itemid.ToInt(0));
            }
        }
        Response.Redirect(AppName + "Shop/ShopCart.aspx");
    }
    


    /// <summary>
    /// 清空购物车
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClearShopCart_Click(object sender, EventArgs e)
    {
        ShopCartService.Instance.ClearCart();
        Response.Redirect(AppName + "Shop/ShopCart.aspx");
    }


    #region 数据绑定
    /// <summary>
    /// 绑定购物车
    /// </summary>
    private void BindCartList()
    {
        try
        {
            DataSet ds = ShopCartService.Instance.GetShopCartDetailed();
            rptShopCart.DataSource = ds.Tables[0];
            rptShopCart.DataBind();

            Count = ds.Tables[1].Rows[0][0].ToInt(0);
            Total = ds.Tables[2].Rows[0][0].ToDecimal(0);
            if (Count > 0)
                CartView.ActiveViewIndex = 1;
        }
        catch
        {
            CartView.ActiveViewIndex = 0;
        }
    }
    #endregion
}