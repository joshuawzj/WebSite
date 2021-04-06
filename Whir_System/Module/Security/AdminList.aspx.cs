/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：adminlist.aspx.cs
* 文件描述：管理员列表页。 
*/
using System;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Security.Domain;
using Whir.Language;
using Whir.Security;
using Whir.Domain;
 

public partial class Whir_System_Module_Setting_AdminList : Whir.ezEIP.Web.SysManagePageBase
{
    public Whir.Security.Domain.Roles CurrentRole = new Whir.Security.Domain.Roles();
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("29"));
        CurrentRole = ServiceFactory.RolesService.SingleOrDefault<Roles>(RequestUtil.Instance.GetQueryInt("rolesid", 0)) ?? ModelFactory<Roles>.Insten(); ;
    }
}