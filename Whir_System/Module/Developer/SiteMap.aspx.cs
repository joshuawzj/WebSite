/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：sitemap.aspx.cs
 * 文件描述： 网站地图生成
 */
using System;

public partial class Whir_System_Module_Developer_SiteMap : Whir.ezEIP.Web.SysManagePageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }

}