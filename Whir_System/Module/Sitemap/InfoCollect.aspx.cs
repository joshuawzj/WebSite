/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：infocollect.aspx.cs
 * 文件描述：信息统计
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.ezEIP.Web;
using Whir.Security.Domain;
using Whir.Language;

public partial class whir_system_module_sitemap_infocollect : SysManagePageBase
{
    //后台相对路径 
    public string SystemPath = WebUtil.Instance.AppPath() + AppSettingUtil.GetString("SystemPath");
    
    protected void Page_Load(object sender, EventArgs e)
    {
         
    }

     
}