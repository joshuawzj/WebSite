/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：membergrouplist.aspx.cs
 * 文件描述：会员组列表
 */
using System;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Service;
using Whir.Domain;
using Whir.Language;

public partial class Whir_System_ModuleMark_Member_MemberGroupList : Whir.ezEIP.Web.SysManagePageBase
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("11"));
    }

    
 
}