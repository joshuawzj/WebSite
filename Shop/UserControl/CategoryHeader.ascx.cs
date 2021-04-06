/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：Shop_UserControl_CategoryHeader.cs
 * 文件描述：商品分类菜单
 * 
 * 创建标识: yangwb
 * 
 * 修改标识：
 */
using System;
using System.Collections.Generic;

using Shop.Domain;
using Whir.Framework;
using System.Data;
using Shop.Service;
public partial class Shop_UserControl_CategoryHeader : Shop.Common.UserControlBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
           // BindCategory();
            BindCartCount();
        }
    }
    //绑定购物车数量
    private void BindCartCount()
    {
        DataSet ds = ShopCartService.Instance.GetShopCartDetailed();
        if (ds.Tables.Count > 1)
        {
            DataTable dt = ds.Tables[1];
            if (dt.Rows.Count > 0)
            {
                ltCartCount.Text = dt.Rows[0]["Qutity"].ToInt().ToStr();
            }
        }
    }
    /// <summary>
    /// 绑定分类
    /// </summary>   
    //private void BindCategory()
    //{
    //    string Html = "";
    //    IList<ShopCategory> categorylist = ShopCategoryService.Instance.GetCategoryListByParentID(0);
    //    if (categorylist.Count > 0)
    //    {
    //        Html += "<dl class=\"item\">";
    //        foreach (ShopCategory sc in categorylist)
    //        {
    //            Html += " <dt><h4><a cid=\"" + sc.CategoryID + "\"  path=\"" + sc.ParentPath + "\" href=\"" + WebUtil.Instance.AppPath() + "Shop/productlist.aspx?categoryid=" + sc.CategoryID + "\">" + sc.CategoryName + "</a></h4>";
    //            IList<ShopCategory> childrenlist = ShopCategoryService.Instance.GetCategoryListByParentID(sc.CategoryID);
    //            if (childrenlist.Count > 0)
    //            {
    //                Html += "<i>";
    //                foreach (ShopCategory child in childrenlist)
    //                {
    //                    Html += "<a  cid=\"" + child.CategoryID + "\"  path=\"" + child.ParentPath + "\" href=\"" + WebUtil.Instance.AppPath() + "Shop/productlist.aspx?categoryid=" + child.CategoryID + "\">" + child.CategoryName + "</a>&nbsp;&nbsp;&nbsp;";
    //                }
    //                Html += "</i>";
    //            }
    //            Html += "</dt>";
    //            Html += "<dd class=\"sub\">加载中...</dd>";
    //        }
    //        Html += "</dl>";
    //    }
    //    ltCategoryList.Text = Html;
    //}
}