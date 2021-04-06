/*
 * Copyright © 2009-2011 万户网络技术有限公司
 * 文 件 名：whir_system_Plugin_shop_product_attr_attrlist.cs
 * 文件描述：商品规格组、规格值操作类
 * 
 * 创建标识: liuyong 2013-01-30
 * 
 * 修改标识：
 */
using System.Web.UI.WebControls;
using System.Linq;
using System.Collections.Generic;

using System;
using Whir.Framework;
using Shop.Service;
using Shop.Domain;

public partial class whir_system_Plugin_shop_product_attr_attrlist : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 搜索关键字
    /// </summary>
    private string SearchKey { get; set; }

    /// <summary>
    /// 编辑、删除时返回的ID
    /// </summary>
    protected string AttrId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("412"));
    }
     
}