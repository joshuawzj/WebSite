/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：generationhomepage.aspx.cs
 * 文件描述：生成网站首页 
 */
using System;

using Whir.ezEIP.Web;
using Whir.Domain;
using Whir.Service;
using Whir.Language;

public partial class whir_system_module_release_generationhomepage : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            bindSiteInfo();
        }
    }
    //绑定站点信息
    private void bindSiteInfo()
    {
        SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteId);
        if (siteInfo != null)
        {
            litDefaultTemp.Text = AppName + siteInfo.Path + "/template/" + siteInfo.DefaultTemp;

            string publishPath = AppName;
 
            //不是默认站点, 必然生成在站点目录
            if (!siteInfo.IsDefault)
                publishPath += siteInfo.Path + "/index";
            else
                publishPath += "index";

            //生成模式
            if (siteInfo.CreateMode == 1)
                publishPath += ".html";
            else if (siteInfo.CreateMode == 2)
                publishPath += ".aspx";
            else
                publishPath = "不生成首页".ToLang();

            litPublishPath.Text = publishPath;

        }
    }

}