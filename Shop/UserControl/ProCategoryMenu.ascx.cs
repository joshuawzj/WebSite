/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：Shop_UserControl_ProCategoryMenu.cs
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

public partial class Shop_UserControl_ProCategoryMenu : System.Web.UI.UserControl
{
    /// <summary>
    /// 当前分类ParentPath
    /// </summary>
    private string path
    {
        get
        {
            return ViewState["path"] == null ? "" : ViewState["path"].ToString();
        }
        set
        {
            ViewState["path"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (RequestUtil.Instance.GetQueryInt("categoryid", 0) > 0)
            {
                ShopCategory sc = ShopCategoryService.Instance.GetCategoryByID(RequestUtil.Instance.GetQueryInt("categoryid", 0));
                if (sc != null)
                {
                    path = sc.ParentPath;
                }
            }
            string html = "";
            int level = 3;//根据静态页面的html这里只显示三级分类
            BindCategory(0, ref html, level);
            ltCategory.Text = html;
        }
    }
    /// <summary>
    /// 绑定分类
    /// </summary>
    /// <param name="CategoryParentID">分类父ID</param>
    /// <param name="Html"></param>
    private void BindCategory(int CategoryParentID, ref string Html, int level)
    {
        IList<ShopCategory> categorylist = ShopCategoryService.Instance.GetCategoryListByParentID(CategoryParentID);
        if (categorylist.Count > 0)
        {
            Html += "<ul class='ul'>";
            foreach (ShopCategory sc in categorylist)
            {
                Html += " <li><a cid=\"" + sc.CategoryID + "\" " + ((("," + path + ",").IndexOf("," + sc.CategoryID.ToString() + ",") > 0 || sc.CategoryID == RequestUtil.Instance.GetQueryInt("categoryid", 0)) ? "class=\"aon\"" : "") + " path=\"" + sc.ParentPath + "\" href=\"" + WebUtil.Instance.AppPath() + "Shop/productlist.aspx?categoryid=" + sc.CategoryID + "\">" + sc.CategoryName + "</a>";
                if (sc.ParentPath.Split(',').Length < level)
                {
                    BindCategory(sc.CategoryID, ref Html, level);
                }

                Html += "</li>";
            }
            Html += "</ul>";
        }
    }
}