/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：Jurisdiction_menu.aspx.cs
 * 文件描述：开发者分客户分配权限
 */

using System;
using System.Linq;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Language;

public partial class whir_system_module_developer_Jurisdiction_menu : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性

    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgeOpenPagePermission(IsDevUser);
            BindMenuList();

        }
    }

    #region 绑定
    //绑定菜单列表
    private void BindMenuList()
    {
        rptMenuList.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(MenuList_ItemDataBound);
        rptMenuList.DataSource = Whir.Security.ServiceFactory.RolesService.GetMenuListByRoleId(true, 2,6);//“开发”不能授权给客户
        rptMenuList.DataBind();
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
            Whir.Domain.Menu menu = e.Item.DataItem as Whir.Domain.Menu;
            if (menu == null) return;

            Literal litMenuName = e.Item.FindControl("litMenuName") as Literal;
            if (litMenuName != null)
            {
                string menuName = menu.MenuName;
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

        Whir.Security.ServiceFactory.RolesService.UpdateRoleJurisdiction(0, CurrentSiteId, 2, ids, 0);
 
        SimpleAlert(upSave, "保存成功".ToLang(), false);
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