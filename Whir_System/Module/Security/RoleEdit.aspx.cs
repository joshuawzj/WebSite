/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：RoleEdit.aspx.cs
* 文件描述：角色组编辑页。 

*/

using System;
using System.Collections.Generic;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Security.Domain;
using Whir.Service;
using Whir.ezEIP.Web;
using ServiceFactory = Whir.Security.ServiceFactory;

public partial class Whir_System_Module_Setting_RoleEdit : SysManagePageBase
{
    #region 属性

    /// <summary>
    /// 当前编辑的角色组ID
    /// </summary>
    protected int RoleId { get; set; }
    /// <summary>
    /// 当前编辑的角色组ID
    /// </summary>
    protected int ParentRoleId { get; set; }

    protected Roles CurrentRoles { get; set; }
    protected IDictionary<int, string> Roles { get; set; }
    protected IDictionary<int, string> FirstSelect { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
       
        RoleId = RequestUtil.Instance.GetQueryInt("rolesid", 0);
        ParentRoleId = RequestUtil.Instance.GetQueryInt("ParentRoleId", 0);
        FirstSelect = new Dictionary<int, string>();
        BindDate();
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("324") || IsCurrentRoleMenuRes("326"));
            var currentRoleId = IsDevUser ? 0 : CurrentUserRolesId;
            Roles = ServiceFactory.RolesService.GetRoleTreeList(currentRoleId, CurrentRoles.Depth);
            Roles.Remove(CurrentRoles.RoleId);
            if (!IsDevUser)
            {
                FirstSelect.Add(CurrentUserRolesId, CurrentUserRolesName );
            }

            if (RoleId != 0)
            {

                PageMode = EnumPageMode.Update;
            }

        }
    }

    /// <summary>
    /// 绑定信息
    /// </summary>
    private void BindDate()
    {
        CurrentRoles = ServiceFactory.RolesService.SingleOrDefault<Roles>(RoleId) ?? ModelFactory<Roles>.Insten();
    }
}