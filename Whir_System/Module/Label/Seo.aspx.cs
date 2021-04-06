/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：seo.aspx.cs
 * 文件描述： 获取当前栏目的标题、关键字和站点描述置标页面
 */

using System;

public partial class Whir_System_Module_Label_Seo : Whir.ezEIP.Web.SysManagePageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
}