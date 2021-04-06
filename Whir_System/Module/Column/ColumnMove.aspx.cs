/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：columnmove.aspx.cs
 * 文件描述：文件库管理页面, 上传文件选择页面
 *
 *          liuyong 2012/12/25 复制、修改此页面
 */
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Whir.Framework;
using Whir.ezEIP.Web;
using Whir.Service;
using Whir.Domain;
using Whir.Language;
using Whir.Repository;

public partial class whir_system_module_column_columnmove : SysManagePageBase
{
    

    StringBuilder _nodeChildBuilder = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }

    /// <summary>
    /// 树绑定
    /// </summary>
    /// <returns></returns>
    protected string BindTree()
    {
        StringBuilder nodeBuilderAll = new StringBuilder();
        nodeBuilderAll.Append("[");
        IList<Column> list = ServiceFactory.ColumnService.GetList(0, CurrentSiteId)
           .OrderBy(p => p.Sort)
           .ThenBy(p=>p.CreateDate)
           .ToList();
     
        List<Column> parentlist = list.Where(p => p.ParentId == 0).ToList();

        foreach (Column cl in parentlist)
        {
            _nodeChildBuilder = new StringBuilder();
            StringBuilder nodeBuilder = new StringBuilder();

            nodeBuilder.Append("{");
            nodeBuilder.Append(("text: '{0}',").FormatWith(Regex.Replace(cl.ColumnName, "[\\W]", "")));
            nodeBuilder.Append("href: '#',");
            nodeBuilder.Append(("columnid: '{0}',").FormatWith(cl.ColumnId));

            List<Column> childColumnList = GetChildList(list, cl.ColumnId);
            nodeBuilder.Append(("tags: [{0}],").FormatWith(childColumnList.Count));

            GetChildNode(list, cl.ColumnId);
            nodeBuilder.Append(_nodeChildBuilder);

            nodeBuilder.Append("},");
            nodeBuilderAll.Append(nodeBuilder.ToStr());
        }
        nodeBuilderAll.Append("]");
        return nodeBuilderAll.ToStr();
    }

    private void GetChildNode(IList<Column> list, int parentId)
    {
        List<Column> childColumnList = GetChildList(list,parentId);

        if (childColumnList.Count > 0)
        {
            _nodeChildBuilder.Append("nodes: [");
            //子节点
            foreach (Column columnItem in childColumnList)
            {
                _nodeChildBuilder.Append("{");
                _nodeChildBuilder.Append(("text: '{0}',").FormatWith(Regex.Replace(columnItem.ColumnName, "[\\W]", "")));
                _nodeChildBuilder.Append("href: '#',");
                _nodeChildBuilder.Append(("columnid: '{0}',").FormatWith(columnItem.ColumnId));
                _nodeChildBuilder.Append(("tags: [{0}],").FormatWith(GetChildList(list, columnItem.ColumnId).Count));

                GetChildNode(list, columnItem.ColumnId);
                _nodeChildBuilder.Append("},");
            }
            _nodeChildBuilder.Append("]");
        }

    }

    private List<Column> GetChildList(IList<Column> list,int parentId)
    {
        return list.Where(p => p.ParentId == parentId).ToList();
    }
 
    /// <summary>
    /// 根据栏目ID， 获取此栏目的父ID， 以及所有的上级父ID
    /// </summary>
    /// <param name="columnID"></param>
    /// <returns></returns>
    private string getAllParents(int columnID)
    {
        string parentIDs = "";
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnID);
        if (column != null)
        {
            if (column.ParentId == 0)
                return parentIDs + ",0";
            else
                parentIDs += column.ParentId + "," + getAllParents(column.ParentId);
        }
        return parentIDs;
    }
    /// <summary>
    /// 更改排序
    /// </summary>
    /// <param name="origin_sort"></param>
    /// <param name="target_sort"></param>
    /// <param name="column_id"></param>
    /// <returns></returns>
    private void ModifySort(int parent_id, long origin_sort, long target_sort, int column_id)
    {
        if (origin_sort > target_sort)
        {
            ServiceFactory.ColumnService.UpdateColumnSort(parent_id, target_sort, column_id);
        }
        else
        {
            ServiceFactory.ColumnService.UpdateColumnSort(parent_id, origin_sort, column_id);
        }
    }
    /// <summary>
    /// 当一个节点存在父节点则调用该方法可以修改它们的sort
    /// </summary>
    /// <param name="origin_id"></param>
    /// <param name="target_id"></param>
    /// <param name="column_id"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    private int ModifyNoParentSort(long origin_id, long target_id, int column_id, ref int count)
    {
        if (origin_id > target_id)
        {
            count += ServiceFactory.ColumnService.UpdateColumnSort(target_id, origin_id, column_id);
        }
        else
        {
            count += ServiceFactory.ColumnService.UpdateColumnSort(origin_id, target_id, column_id);
        }
        return count;
    }
}
