
/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：sitehome.aspx.cs
 * 文件描述：站点首页设置页面
 *          
 *
 *          1. 根据基类获取到的当前站点ID, 获取站点的相关信息, 显示在页面, 包含添加和编辑功能
 *          2. 首页模板选择器, 打开template_select.aspx页面, 可返回选中的模板文件相对与该站点的路径
 */

using System;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_Module_Column_SiteHome : Whir.ezEIP.Web.SysManagePageBase
{
    protected SiteInfo SiteInfo { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        BindSiteInfo();
    }

    //绑定站点信息
    private void BindSiteInfo()
    {
        SiteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteId) ??
                   ModelFactory<SiteInfo>.Insten();
    }

    /// <summary>
    /// 点击保存事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Save_Command(object sender, CommandEventArgs e)
    {
        //SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteID);
        //if (siteInfo != null)
        //{
        //    siteInfo.CreateMode = rblCreateMode.SelectedValue.ToInt();
        //    siteInfo.DefaultTemp = txtDefaultTemp.Text.Trim();
        //    ServiceFactory.SiteInfoService.Update(siteInfo);

        //    ServiceFactory.SiteInfoService.SaveLog(siteInfo, "update");
        //    Alert("操作成功".ToLang(), true);
        //}
    }
}