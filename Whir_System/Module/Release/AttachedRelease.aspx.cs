/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：attachedrelease.aspx.cs
 * 文件描述：附带发布
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml.Linq;

using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Label;
using Whir.Language;
using Whir.Service;

public partial class whir_system_module_release_attachedrelease : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
}