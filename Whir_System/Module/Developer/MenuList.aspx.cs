/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：menulist.aspx.cs
 * 文件描述：后台菜单列表页面
 *          
 *
 *          1. 显示三级菜单, 列表的排序输入框异步请求ajax/developer/menu_modifysort.aspx进行排序
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Service;

public partial class Whir_System_Module_Developer_MenuList : SysManagePageBase
{
    protected List<Menu> AllMenu { get; set; }
    protected List<Menu> MenuTreeList { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
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
        foreach (var menu in AllMenu.Where(p => p.ParentId == parentId).OrderBy(p => p.Sort).ThenBy(p => p.CreateDate))
        {
            menu.Level = level;
            menus.Add(menu);
            menus.AddRange(GetNodeMenus(menu.MenuId, level + 1));
        }
        
        return menus;
    }
}