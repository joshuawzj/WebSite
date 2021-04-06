/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：copyright.aspx.cs
 * 文件描述： 获取网站版权信息置标页面
 */

using System;
using System.Collections.Generic;
using System.Linq;

using Whir.Domain;
using Whir.Service;

public partial class Whir_System_Module_Label_CopyRight : Whir.ezEIP.Web.SysManagePageBase
{
    public IList<SiteInfo> Sites { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            BindSite();
        }
    }

    /// <summary>
    /// 绑定站点
    /// </summary>
    private void BindSite()
    {
        Sites = ServiceFactory.SiteInfoService.GetList().ToList<SiteInfo>();
    }
}