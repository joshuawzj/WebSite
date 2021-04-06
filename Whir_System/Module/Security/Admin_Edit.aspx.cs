/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：admin_edit.aspx.cs
* 文件描述：管理员编辑页。 
*/
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Security.Domain;
using Whir.Service;

public partial class Whir_System_Module_Setting_Admin_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前编辑的管理员ID
    /// </summary>
    protected int UserId { get; set; }

    /// <summary>
    /// 当前页类型：PageType=2是代表从角色的成员管理里跳过来的
    /// </summary>
    protected int PageType { get; set; }

    protected Users CurrenUsers { get; set; }
    protected IDictionary<int, string> Roles { get; set; }
    protected IDictionary<int, string> FirstSelect { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        UserId = RequestUtil.Instance.GetQueryInt("userid", 0);
        FirstSelect = new Dictionary<int, string>();
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("320") || IsCurrentRoleMenuRes("322"));
            BindRoles();
            if (UserId != 0)
            {
                PageMode = EnumPageMode.Update;
                 
            }
            
        }
        
        BindDate();
    }
    /// <summary>
    /// 绑定管理员信息
    /// </summary>
    private void BindDate()
    {
        CurrenUsers = Whir.Security.ServiceFactory.UsersService.SingleOrDefault<Users>(UserId) ?? ModelFactory<Users>.Insten(); 
    }
    /// <summary>
    /// 绑定角色
    /// </summary>
    private void BindRoles()
    {
        var currentRoleId = IsDevUser ? 0 : CurrentUserRolesId;
        Roles = Whir.Security.ServiceFactory.RolesService.GetRoleTreeList(currentRoleId,0);
        if (!IsDevUser)
        {
            FirstSelect.Add(CurrentUserRolesId, CurrentUserRolesName.ToLang());
        }
       
    }

}