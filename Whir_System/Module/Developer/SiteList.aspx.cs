/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：multisite.aspx.cs
 * 文件描述：后台多站点列表页面
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.Service;

public partial class Whir_System_Module_Developer_SiteList : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            BindMultiSiteList();
        }
    }

    //绑定列表
    private void BindMultiSiteList()
    {
        IList<SiteInfo> list = ServiceFactory.SiteInfoService.Query<SiteInfo>("WHERE IsDel=@0", 0).ToList<SiteInfo>();
        rptMultiSite.DataSource = list;
        rptMultiSite.DataBind();

        ltNoRecord.Text = list.Count > 0 ? "" : "找不到记录";
    }


}