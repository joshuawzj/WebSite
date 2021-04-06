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

public partial class whir_system_Plugin_shop_product_attr_attrvalue_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 规格组ID
    /// </summary>
    protected int AttrID { get; set; }

    /// <summary>
    /// 规格值ID
    /// </summary>
    protected int AttrValueID { get; set; }

    /// <summary>
    /// 规格组
    /// </summary>
    public ShopAttr ShopAttr { get; set; }
    /// <summary>
    /// 规格值
    /// </summary>
    public ShopAttrValue ShopAttrVal { get; set; }
    /// <summary>
    /// 操作字符串
    /// </summary>
    public string ProcessStr { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("412"));
        AttrID = RequestUtil.Instance.GetQueryInt("attrid", 0);
        AttrValueID = RequestUtil.Instance.GetQueryInt("attrvalueid", 0);
        if (!IsPostBack)
        {
            var model = ShopAttrService.Instance.SingleOrDefault<ShopAttr>(AttrID);
            if (model != null)
            {
                ShopAttr = model;
            }
            else
            {
                ShopAttr = new ShopAttr();
            }
            if (AttrValueID > 0)//编辑模式
            {
                ShopAttrValue Model = ShopAttrValueService.Instance.SingleOrDefault<ShopAttrValue>(AttrValueID);
                ShopAttrVal = Model;
                ProcessStr = "编辑规格值".ToLang();
            }
            else
            {
                ShopAttrVal = new ShopAttrValue();
                ProcessStr = "添加规格值".ToLang();
            }
        }
    }
}