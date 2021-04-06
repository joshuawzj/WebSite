/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：columnlist.aspx.cs
 * 文件描述：栏目列表页
 *          */

using System;
using System.Collections.Generic;
using System.Linq;

using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Service;

public partial class Whir_System_Module_Column_ColumnList : SysManagePageBase
{
    protected List<Model> AllModel { get; set; }

    protected List<Column> AllColumn { get; set; }
    protected List<Column> ColumnTreeList { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser || IsSuperUser);
        AllModel = ServiceFactory.ModelService.GetList().ToList();

        AllColumn = ServiceFactory.ColumnService.GetList(0, CurrentSiteId, 0, 0)
            .OrderBy(p => p.Sort)
            .ThenBy(p => p.CreateDate)
            .ToList();
        ColumnTreeList = GetNodeColumns(0, 1);
    }

    /// <summary>
    /// 获取下级栏目
    /// </summary>
    /// <param name="parentId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private List<Column> GetNodeColumns(int parentId, int level)
    {
        var columns = new List<Column>();
        foreach (var column in AllColumn.Where(p => p.ParentId == parentId))
        {
            column.LevelNum = level;
            columns.Add(column);
            columns.AddRange(GetNodeColumns(column.ColumnId, level + 1));
        }
        return columns;
    }
}