/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：menuedit.aspx.cs
 * 文件描述：后台菜单添加/编辑页面
 *          
 *
 *          1. 输入的"父菜单"下拉框, 只显示一级和二级, 所有菜单最多只支持三级  guoc 2012-08-18  
 *  1. 输入的"父菜单"下拉框, 只显示一级和二级和三级, 所有菜单最多只支持四级
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class Whir_System_Module_Developer_MenuEdit : SysManagePageBase
{
    /// <summary>
    /// 当前编辑的菜单ID
    /// </summary>
    protected int MenuId { get; set; }
    protected Menu CurrentMenu { get; set; }

    protected List<Menu> AllMenu { get; set; }
    protected List<Menu> MenuTreeList { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        MenuId = RequestUtil.Instance.GetQueryInt("menuid", 0);
        CurrentMenu = ServiceFactory.MenuService.SingleOrDefault<Menu>(MenuId)
                      ?? ModelFactory<Menu>.Insten();


        AllMenu = ServiceFactory.MenuService.GetList()
            .OrderBy(p => p.MenuType)
            .ThenBy(p => p.Sort)
            .ThenBy(p => p.CreateDate)
            .ToList();
        MenuTreeList = GetNodeMenus(0, 1);
    }

    /// <summary>
    /// 获取下级菜单
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private List<Menu> GetNodeMenus(int parentId, int level)
    {
        var menus = new List<Menu>();
        foreach (var menu in AllMenu.Where(p=>p.ParentId == parentId))
        {
            int fix = level;
            while (fix-- > 0)
            {
                //往前追加空格，代表层级
                menu.MenuName = "　" + menu.MenuName.ToLang();
            }
            menu.Level = level;
            menus.Add(menu);
            menus.AddRange(GetNodeMenus(menu.MenuId, level + 1));
        }
        return menus;
    }
    
}