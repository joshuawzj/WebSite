/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：paymentlist.aspx.cs
 * 文件描述：支付方式管理
 * 
 * 创建标识: liuyong 2012-08-21
 * 
 * 修改标识：
 */
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

using Whir.Framework;
using Whir.ezEIP.Web;
using Whir.Language;
using Whir.Service;

public partial class whir_system_Plugin_shop_order_payment_paymentlist : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("408"));
        }
    }
}