/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：location.aspx.cs
 * 文件描述：  当前页位置导航置标页面
 */

using System;

public partial class Whir_System_Module_Label_Location : Whir.ezEIP.Web.SysManagePageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
}
