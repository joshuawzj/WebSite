/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：inforarea.aspx.cs
 * 文件描述： 内容置标。主要用于单篇或内容页显示某个字段值
 */
using System;
using Whir.ezEIP.Web;

public partial class Whir_System_Module_Label_InforArea : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
}