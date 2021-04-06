/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：submitformlist.aspx.cs
 * 文件描述：提交表单页面
 */
using System;


public partial class Whir_System_Module_Developer_SubmitFormList : Whir.ezEIP.Web.SysManagePageBase
{


    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser || IsSuperUser);
    }
}