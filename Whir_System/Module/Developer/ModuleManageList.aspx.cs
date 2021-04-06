/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：menu_resetResources.aspx.cs
 * 文件描述： 重置菜单资源
 */
using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Whir.Service;
using Whir.Framework;
using Whir.Domain;
using Whir.Language;



public partial class whir_system_module_developer_ModuleManageList : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
}