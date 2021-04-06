/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：category.aspx.cs
 * 文件描述：树型结构类别读取标签（只适用于模块版本号为Category的栏目）
 *          
 */

using System;

using Whir.ezEIP.Web;

public partial class Whir_System_Module_Label_Category : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
}