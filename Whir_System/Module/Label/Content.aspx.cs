/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：content.aspx.cs
 * 文件描述： 内容置标，获取内容页的上一篇，下一篇
 *          
 */
using System;

using Whir.ezEIP.Web;

public partial class Whir_System_Module_Label_Content : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }

}