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
using System.Web.UI.WebControls;
//非系统的引用
using Shop.Domain;
using Whir.Framework;

using Whir.Service;

public partial class whir_system_Plugin_shop_field_profieldlist : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("415"));
    }
}