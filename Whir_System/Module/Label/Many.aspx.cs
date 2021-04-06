/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：many.aspx.cs
 * 文件描述：  多图片显示，多文件下载置标页面
 */

using System;

public partial class Whir_System_Module_Label_Many : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
}