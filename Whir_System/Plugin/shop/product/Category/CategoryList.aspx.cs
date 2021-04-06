/*
 * Copyright © 2009-2013 万户网络技术有限公司
 * 文 件 名：profieldlist.aspx.cs
 * 文件描述：商品自定义属性编辑页面
 *          
 * 
 * 创建标识: 
 * 
 * 修改标识：
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
//非系统的引用
using Shop.Domain;
using Whir.Framework;


public partial class whir_system_Plugin_shop_product_category_categorylist : Whir.ezEIP.Web.SysManagePageBase
{
    public List<ShopCategory> CategoryList { get; set; }

    public List<ShopCategory> CategoryTree { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("411"));
            bindCategorylist();
        }
    }
    /// <summary>
    /// 绑定商品类别列表
    /// </summary>
    private void bindCategorylist()
    {
        CategoryList = ShopCategoryService.Instance.GetList().ToList();
        CategoryTree = GetNodeCategory(0, 1);
    }
    public List<ShopCategory> GetNodeCategory(int parentID, int level)
    {
        var list = new List<ShopCategory>();
        foreach (var category in CategoryList.Where(p => p.ParentID == parentID))
        {
            category.ParentPath = level.ToStr();
            list.Add(category);
            list.AddRange(GetNodeCategory(category.CategoryID, level + 1));
        }

        return list;
    }
}