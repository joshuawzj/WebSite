/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：whir_system_Plugin_shop_product_attr_attrvalue_edit.cs
 * 文件描述：商品规格值操作类
 * 
 * 创建标识: liuyong 2013-01-30
 * 
 * 修改标识：
 */
using System;
using System.Web.UI.WebControls;

using Whir.Framework;
using Shop.Domain;
using Shop.Service;
using Whir.Service;
using Whir.Domain;
using Whir.Language;

public partial class whir_system_Plugin_shop_product_search_searchvalue_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 规格组ID
    /// </summary>
    protected int SearchID { get; set; }

    /// <summary>
    /// 规格值ID
    /// </summary>
    protected int SearchValueID { get; set; }
    /// <summary>
    /// 操作字符串
    /// </summary>
    public string ProcessStr { get; set; }

    /// <summary>
    /// 搜选项值
    /// </summary>
    public ShopSearchValue ShopSearchVal { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("413"));
        SearchID = RequestUtil.Instance.GetQueryInt("searchid", 0);
        SearchValueID = RequestUtil.Instance.GetQueryInt("searchvalueid", 0);
        ShopSearchValue Model = ShopSearchValueService.Instance.SingleOrDefault<ShopSearchValue>(SearchValueID);
        if (Model != null)
        {
            ShopSearchVal = Model;
        }
        else
        {
            ShopSearchVal = new ShopSearchValue();
        }
        if (!IsPostBack)
        {
            if (SearchValueID > 0)//编辑模式
            {
                
                ProcessStr = "编辑搜选项值".ToLang();
            }
            else
            {
                ProcessStr="添加搜选项值".ToLang();
            }
        }
    }
}