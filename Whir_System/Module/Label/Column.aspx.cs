/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：column.aspx.cs
 * 文件描述：栏目信息显示置标，用于调用栏目信息，如栏目名称及栏目ID等。
 *          
 */
using System;

using Whir.ezEIP.Web;

public partial class Whir_System_Module_Label_Column : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
}