/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：
 * 文件描述：
 */
using System;
using Whir.ezEIP.Web;


public partial class Whir_System_Module_Extension_Collectlist : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("38"));
    }
}