/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：multisite_edit.aspx.cs
 * 文件描述：后台多站点编辑页
 */


using System;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_Module_Developer_SiteEdit : SysManagePageBase
{
    /// <summary>
    /// 当前编辑的站点ID
    /// </summary>
    protected int SiteId { get { return RequestUtil.Instance.GetQueryInt("SiteId",0); } }

    protected SiteInfo CurrentSite { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        CurrentSite = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(SiteId)
                      ?? ModelFactory<SiteInfo>.Insten();
    }
    
}