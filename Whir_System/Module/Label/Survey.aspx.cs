/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：survey.aspx.cs
 * 文件描述：survey置标生成器
 */
using System;
using Whir.ezEIP.Web;

public partial class Whir_System_Module_Label_Survey : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
}