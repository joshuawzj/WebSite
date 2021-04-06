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

public partial class whir_system_module_subject_columnmove : SysManagePageBase
{
    /// <summary>
    /// 固定值 - 1:子站, 2:专题
    /// </summary>
    protected int SubjectTypeId { get; set; }

    /// <summary>
    /// 当前的子站/专题类型ID
    /// </summary>
    protected int SubjectClassId { get; set; }

    StringBuilder _nodeChildBuilder = new StringBuilder();

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        SubjectTypeId = RequestUtil.Instance.GetQueryInt("subjecttypeid", 1);
        SubjectClassId = RequestUtil.Instance.GetQueryInt("subjectclassid", 0);

    }

    /// <summary>
    /// 树绑定
    /// </summary>
    /// <returns></returns>
    protected string BindTree()
    { 
        StringBuilder nodeBuilderAll = new StringBuilder();
        nodeBuilderAll.Append("[");
        IList<Column> list = ServiceFactory.ColumnService.GetSubjectColumnList(0, SubjectClassId,0);
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
    /// 触发提交事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbSubmit_Click(object sender, EventArgs e)
    {
        //if (hfOrigin.Value == "")
        //{
        //    Alert("请选择源栏目".ToLang());
        //}
        //else if (hfTarget.Value == "")
        //{
        //    Alert("请选择目标栏目".ToLang());
        //}
        //else if (rblColumnMove.SelectedValue == "")
        //{
        //    Alert("请选择移动类型".ToLang());
        //}
        //else
        //{
        //    string origin = hfOrigin.Value;
        //    string target = hfTarget.Value;
        //    string selValue = rblColumnMove.SelectedValue;
        //    ColumnMove(origin, target, selValue);
        //}
    }

    /// <summary>
    /// 栏目移动
    /// </summary>
    /// <param name="origin">栏目源</param>
    /// <param name="target">目标源</param>
    /// <param name="selValue">移动方式</param>
    private void ColumnMove(string origin, string target, string selValue)
    {
        string childNode = string.Empty;//获取子节点,这是需要移动的栏目ID字符串
        string[] origins = origin.Substring(0, origin.Length - 1).Split(',');//去掉origin字符串最后一个,字符
        string[] targets = target.Substring(0, target.Length - 1).Split(',');//去掉target字符串最后一个,字符
        List<string> alOrigin = new List<string>();
        List<string> alTarget = new List<string>();

        //左侧的List集合
        foreach (string str in origins)//12,114
        {
            IList<Column> listOrigin = ServiceFactory.ColumnService.GetColumnByParentId(str.ToInt(), CurrentSiteId);

            if (!alOrigin.Contains(str))
            {
                foreach (Column cl in listOrigin)
                {
                    alOrigin.Add(cl.ColumnId.ToStr());
                }
            }
        }
        //右侧的List集合
        foreach (string str in targets)
        {
            IList<Column> listTarget = ServiceFactory.ColumnService.GetColumnByParentId(str.ToInt(), CurrentSiteId);

            if (!alTarget.Contains(str))
            {
                foreach (Column cl in listTarget)
                {
                    alTarget.Add(cl.ColumnId.ToStr());
                }
            }
        }
        //左右两侧比较，左侧多选，右侧单选，则取右侧与左侧进行对比
        //如果存在有相同的，则提示栏目源与目标栏目移动出错
        int targetId = targets[0].ToInt();
        //foreach (string strT in alTarget)
        //{
        if (alOrigin.Count == 1 && alOrigin[0].ToInt() == targetId)
        {
            Alert("目标栏目和源栏目相同，请正确勾选源栏目和目标栏目".ToLang());
            return;
        }
        if (alOrigin.Contains(targetId.ToStr()))
        {
            Alert("目标栏目是源栏目的子栏目，请正确勾选源栏目和目标栏目".ToLang());
            return;
        }

        //根据目标栏目的id查询出最大的SortID
        string SQL="select max(Sort) Sort,max(ParentId) ParentId from Whir_Dev_Column WHERE IsDel=0 AND ColumnId=@0 AND SiteId=@1 AND SiteType=@2 AND (MarkType='' OR MarkType IS NULL)";
        Column targetColumn = DbHelper.CurrentDb.SingleOrDefault<Column>(SQL,targetId,CurrentSiteId,SubjectClassId);
        //栏目
        long sort = targetColumn.Sort;
        int parentid = targetColumn.ParentId;
        int count = 0;
        switch (selValue)
        {
            case "Child":

                //遍历所有选中的栏目源
                //这样只能移动第一级
                foreach (string strO in alOrigin)
                {
                    string parentIDs = getAllParents(strO.ToInt());
                    Column thisColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(strO.ToInt());
                    if (thisColumn == null) continue;

                    bool isMoved = false;

                    if (isMoved) continue;

                    for (int i = 0; i < parentIDs.Trim(',').Split(',').Length; i++)
                    {
                        string pids = parentIDs.Trim(',').Split(',')[i];

                        if (pids.IsEmpty()) continue;

                        if (alOrigin.Contains(pids))//已添加过
                        {
                            isMoved = true;
                            continue;
                        }

                        if (i == 0)
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(targetId, ++sort, strO.ToInt());
                        }
                        else
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(++sort, strO.ToInt());
                        }
                    }
                }
                if (count > 0)
                {
                    Alert("操作成功".ToLang(), true);
                    return;
                }
                break;
            case "Before":
                //遍历所有选中的栏目源
                //这样只能移动第一级
                foreach (string strO in alOrigin)
                {
                    string parentIDs = getAllParents(strO.ToInt());
                    Column thisColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(strO.ToInt());
                    long beforeSortID = thisColumn.Sort;
                    if (thisColumn == null) continue;

                    bool isMoved = false;

                    if (isMoved) continue;

                    for (int i = 0; i < parentIDs.Trim(',').Split(',').Length; i++)
                    {
                        string pids = parentIDs.Trim(',').Split(',')[i];

                        if (pids.IsEmpty()) continue;

                        if (alOrigin.Contains(pids))//已添加过
                        {
                            isMoved = true;
                            continue;
                        }
                        //栏目源换成目标栏目的sort
                        //查询栏目源和目标栏目之前的所有sort，并所有都改变
                        //父节点减一
                        if (i == 0)
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(parentid, --sort, strO.ToInt());
                        }
                        else
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(--sort, strO.ToInt());
                        }
                    }
                }
                if (count > 0)
                {
                    Alert("操作成功".ToLang(), true);
                    return;
                }

                break;
            case "After":
                //遍历所有选中的栏目源
                //这样只能移动第一级
                foreach (string strO in alOrigin)
                {
                    string parentIDs = getAllParents(strO.ToInt());
                    Column thisColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(strO.ToInt());
                    long afterSortID = thisColumn.Sort;
                    if (thisColumn == null) continue;

                    bool isMoved = false;

                    if (isMoved) continue;

                    for (int i = 0; i < parentIDs.Trim(',').Split(',').Length; i++)
                    {
                        string pids = parentIDs.Trim(',').Split(',')[i];

                        if (pids.IsEmpty()) continue;

                        if (alOrigin.Contains(pids))//已添加过
                        {
                            isMoved = true;
                            continue;
                        }
                        if (i == 0)
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(parentid, ++sort, strO.ToInt());
                        }
                        else
                        {
                            count += ServiceFactory.ColumnService.UpdateColumnMove(++sort, strO.ToInt());
                        }
                    }
                }
                if (count > 0)
                {
                    Alert("操作成功".ToLang(), true);
                    return;
                }
                break;
        }
        //}

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
