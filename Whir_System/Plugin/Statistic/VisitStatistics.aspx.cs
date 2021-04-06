/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：onlinecustomer.cs
 * 文件描述：流量统计代码管理
 */
using System;
using Whir.Config;
using Whir.Config.Models;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Service;

public partial class Whir_System_Plugin_Statistic_VisitStatistics : SysManagePageBase
{
    protected SiteInfo SiteInfo { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SiteInfo = SiteInfoHelper.SiteInfo;
        }
    }
}