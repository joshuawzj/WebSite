/*
 * Copyright © 2009-2018 万户网络技术有限公司
 * 文 件 名：TestData.aspx.cs
 * 文件描述： 一键生成测试数据
 */
using System;

public partial class Whir_System_Module_Developer_TestData : Whir.ezEIP.Web.SysManagePageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("416"));
    }

}