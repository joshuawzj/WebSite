/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：Jurisdiction_menu.aspx.cs
 * 文件描述：管理员分配菜单权限
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Framework;

public partial class Whir_System_Module_Security_Jurisdiction_Menu : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性


    /// <summary>
    /// URL传参，角色ID
    /// </summary>
    public int RoleID
    {
        get;
        set;
    }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        RoleID = RequestUtil.Instance.GetQueryInt("roleid", 0);
        if (RoleID == 0) { return; }

        if (!IsPostBack)
        {
            //获取所有允许客户使用的菜单资源
            JudgeOpenPagePermission(IsCurrentRoleMenuRes("328"));
            BindMenuList();
        }
    }

    #region 绑定
    //绑定菜单列表
    private void BindMenuList()
    {
        var list = Whir.Security.ServiceFactory.RolesService.GetMenuListByRoleId(true, RoleID, 6);
        var memberWorkFlowMenuList = GetMemberWorkFlowMenu();

	 if (list.Exists(p => p.MenuId == 297))
           list.InsertRange(list.FindIndex(p => p.MenuId == 297), memberWorkFlowMenuList);

        rptMenuList.ItemDataBound += MenuList_ItemDataBound;
        rptMenuList.DataSource = list;
        rptMenuList.DataBind();
    }

    protected List<Whir.Domain.Menu> GetMemberWorkFlowMenu()
    {
        List<Whir.Domain.Menu> list = new List<Whir.Domain.Menu>();
        Column column = Whir.Service.ServiceFactory.ColumnService.SingleOrDefault<Column>(1);
        if (column.WorkFlow > 0)
        {
            var wfList = Whir.Service.ServiceFactory.AuditActivityService.GetListBySort(column.WorkFlow);

            foreach (var auditActivity in wfList)
            {
                var menuId = 9999 + auditActivity.ActivityId;

                var name = "&nbsp;&nbsp;&nbsp;&nbsp;│ &nbsp;&nbsp;&nbsp;&nbsp;├─ <input type = \"checkbox\" value = \"{0}\" {3} name = \"menucheckbox\" level = \"{1}\" onclick = \"javascript: checknode(this);\" > {2}"
                          .FormatWith(menuId, 2, auditActivity.ActivityName, Whir.Security.ServiceFactory.RolesService.IsRoleHaveMenuRes(menuId.ToStr(), RoleID) ? "checked" : "");

                Whir.Domain.Menu menu = new Whir.Domain.Menu()
                {
                    MenuId = menuId,
                    Level = 2,
                    ParentId = 10,
                    MenuName = name
                };
                list.Add(menu);
            }
        }
        return list;
    }

    /// <summary>
    /// 列表行绑定行为事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void MenuList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
        {
            var menu = e.Item.DataItem as Whir.Domain.Menu;
            if (menu == null) return;

            var litMenuName = e.Item.FindControl("litMenuName") as Literal;
            string menuName = menu.MenuName;

            if (litMenuName != null)
            {
                litMenuName.Text = menuName;
            }

        }
    }
    #endregion

    //保存
    protected void btnSave_Click(object sender, EventArgs e)
    {
        ClearMenuCookies();
        string ids = Request["menucheckbox"].ToStr().Trim(',');
        Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(0, CurrentSiteId, RoleID, ids, 0);
        SimpleAlert(upSave, "保存成功", false);
    }
    /// <summary>
    /// 清除菜单缓存
    /// </summary>
    private void ClearMenuCookies()
    {
        string refreshName = "menu_refresh_flag{0}".FormatWith(0);
        CookieUtil.Instance.SetCookie(refreshName, "1", "1");
        refreshName = "menu_refresh_flag{0}".FormatWith(1);
        CookieUtil.Instance.SetCookie(refreshName, "1", "1");
        refreshName = "menu_refresh_flag{0}".FormatWith(406);
        CookieUtil.Instance.SetCookie(refreshName, "1", "1");
        refreshName = "menu_refresh_flag{0}".FormatWith(390);
        CookieUtil.Instance.SetCookie(refreshName, "1", "1");
        refreshName = "menu_refresh_flag{0}".FormatWith(6);
        CookieUtil.Instance.SetCookie(refreshName, "1", "1");
    }
}