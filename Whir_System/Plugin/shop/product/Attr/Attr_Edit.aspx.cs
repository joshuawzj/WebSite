/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：whir_system_Plugin_shop_product_attr_attr_edit.cs
 * 文件描述：商品规格组编辑、添加操作类
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

public partial class whir_system_Plugin_shop_product_attr_attr_edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 规格组ID
    /// </summary>
    protected int AttrID { get; set; }

    /// <summary>
    /// 规格实体类
    /// </summary>
    protected ShopAttr ShopAttr { get; set; }
    /// <summary>
    /// 操作名称
    /// </summary>
    public string ProcessStr { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("412"));
        AttrID = RequestUtil.Instance.GetQueryInt("attrid", 0);
        BandInfo();
        if (AttrID > 0)//编辑
        {
            if (!IsPostBack)
            {
                ProcessStr = "编辑规格组".ToLang();
            }
        }
        else
        {
            ProcessStr = "添加规格组".ToLang();
        }
    }

    /// <summary>
    /// 绑定编辑信息
    /// </summary>
    private void BandInfo()
    {
        ShopAttr Model = ShopAttrService.Instance.SingleOrDefault<ShopAttr>(AttrID);
        if (Model != null)
        {
            ShopAttr = Model;
        }
        else
        {
            ShopAttr = new ShopAttr();
        }
    }
}