/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：roleslist.aspx.cs
* 文件描述：管理员角色列表页。 

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Security.Domain;
using Whir.Language;

public partial class Whir_System_Module_Setting_RoleList : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性

    /// <summary>
    /// 是否具有添加权限
    /// </summary>
    protected bool IsAdd { get; set; }

    /// <summary>
    /// 是否具有成员管理权限
    /// </summary>
    protected bool IsMemberManagement { get; set; }

    /// <summary>
    /// 是否具有编辑权限
    /// </summary>
    protected bool IsEdit { get; set; }

    /// <summary>
    /// 是否具有删除权限
    /// </summary>
    protected bool IsDelete { get; set; }

    /// <summary>
    /// 是否具有菜单权限  菜单权限、栏目权限、工作流权限）
    /// </summary>
    protected bool IsMenuAccess { get; set; }

    /// <summary>
    /// 是否具有栏目权限
    /// </summary>
    protected bool IsColumnAccess { get; set; }

    /// <summary>
    /// 是否具有工作流权限
    /// </summary>
    protected bool IsWorkflowAccess { get; set; }

    protected List<Roles> AllRoles { get; set; }
    protected List<Roles> RolesTreeList { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("30"));
            BindList();
        }
    }

    #region 绑定
    //绑定列表
    private void BindList()
    {        
        var currentRoles = Whir.Security.ServiceFactory.RolesService.SingleOrDefault<Roles>(CurrentUserRolesId);
        var currentUserRolesId = IsDevUser || IsSuperUser ? 0 : currentRoles.ParentId; //非root和admin角色，获取当前角色的上级角色
        var list = Whir.Security.ServiceFactory.RolesService.GetList(currentUserRolesId).ToList();
        AllRoles = list;
        RolesTreeList = GetNodeRoles(currentUserRolesId, 1);

        rptList.DataSource =  RolesTreeList;
        rptList.DataBind();
    }

    /// <summary>
    /// 获取下级菜单
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="depth"></param>
    /// <returns></returns>
    private List<Roles> GetNodeRoles(int parentId, int depth)
    {
        var menus = new List<Roles>();
        foreach (var menu in AllRoles.Where(p => p.ParentId == parentId))
        {
            menu.Depth = depth;
            menus.Add(menu);
            menus.AddRange(GetNodeRoles(menu.RoleId, depth + 1));
        }
        return menus;
    }
    #endregion

    #region 事件

    //行绑定时
    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Roles model = e.Item.DataItem as Roles;
            PlaceHolder lbtnDel = e.Item.FindControl("lbtnDel") as PlaceHolder;

            if (lbtnDel != null)
            {
                if (model.RoleId == 1 || model.RoleId == 2)//主键值为1的无法删除
                {
                    lbtnDel.Visible = false;
                }
            }

            PlaceHolder phCx = e.Item.FindControl("phCX") as PlaceHolder;
            PlaceHolder phSq = e.Item.FindControl("phSQ") as PlaceHolder;
            PlaceHolder phAddChildren = e.Item.FindControl("phAddChildren") as PlaceHolder;

            if (phCx != null && phSq != null)
            {
                //超级管理员设置菜单、栏目、权限设置
                if (model.RoleId == 1 || model.RoleId == 2)
                {
                    phCx.Visible = false;
                    phSq.Visible = false;
                }
            }

            if (model.RoleId == 1)
            {
                if (phAddChildren != null) phAddChildren.Visible = false;
                if (IsDevUser)//开发者才能看到开发角色
                {
                    e.Item.Visible = true;
                }
                else
                {
                    e.Item.Visible = false;
                }
            }
        }
    }
    #endregion
}