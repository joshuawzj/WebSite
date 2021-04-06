/*
 * Copyright © 2009-2013 万户网络技术有限公司
 * 文 件 名：profield_edit.aspx.cs
 * 文件描述：商品自定义属性编辑页面
 *          
 * 
 * 创建标识: 
 * 
 * 修改标识：
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
//非系统的引用
using Whir.Domain;
using Whir.Framework;
using Whir.Service;

using Whir.Config.Models;
using Whir.Config;
using Shop.Domain;
using System.Data;

public partial class whir_system_Plugin_shop_product_category_category_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 保存编辑时的主键ID
    /// </summary>
    public int CategoryID
    {
        get
        {
            if (ViewState["CategoryID"] == null)
            {
                ViewState["CategoryID"] = 0;
            }
            return ViewState["CategoryID"].ToInt();
        }
        set
        {
            ViewState["CategoryID"] = value;
        }
    }
    /// <summary>
    /// 商品分类下拉
    /// </summary>
    public DataTable OptionTable { get; set; }
    /// <summary>
    /// 商品分类
    /// </summary>
    public ShopCategory ShopCategory { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("411"));
            CategoryID = RequestUtil.Instance.GetQueryInt("categoryid", 0);
            bindOption();//绑定上级类目
            bindOldData(CategoryID);
        }
    }

    //绑定上级类目   
    private void bindOption()
    {
        List<DataRow> list = ShopCategoryService.Instance.GetAllCategoryList(0);
        if (list.Count == 0)
        {
            OptionTable = new DataTable();
        }
        else
        {
            OptionTable = list.CopyToDataTable();
        }
    }
    /// <summary>
    /// 绑定原有数据
    /// </summary>
    /// <param name="CategoryID">类目ID</param>
    private void bindOldData(int CategoryID)
    {
        ShopCategory sc = ShopCategoryService.Instance.GetCategoryByID(CategoryID);
        if (sc != null)
        {
            ShopCategory = sc;
        }
        else
        {
            ShopCategory = new ShopCategory();
        }
    }
}