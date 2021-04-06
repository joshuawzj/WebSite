/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：infor.aspx.cs
 * 文件描述：字段值置标。主要用于显示某条记录的某个字段值,限用于详细页和单篇页使用
 * (使用于非详细页与单篇则需要指定columnid,itemid这两个属性)
 */
using System;
using Whir.ezEIP.Web;

public partial class Whir_System_Module_Label_Infor : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
}