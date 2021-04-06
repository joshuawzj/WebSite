/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：subjectcolumnlist.aspx.cs
 * 文件描述：专题/模板子站的栏目列表页面
 */

using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class whir_system_module_subject_subjectcolumnlist : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 固定值 - 1:子站, 2:专题
    /// </summary>
    protected int SubjectTypeId { get; set; }

    /// <summary>
    /// 当前的子站/专题类型ID
    /// </summary>
    protected int SubjectClassId { get; set; }

    /// <summary>
    /// 当前的子站ID
    /// </summary>
    protected int SubjectId { get; set; }

    protected List<string> GuangliLink = new List<string>();
    protected List<string> AddLink = new List<string>();
    protected bool IsMove { get; set; }
    protected List<Column> Columns { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser||IsSuperUser);
        SubjectTypeId = RequestUtil.Instance.GetQueryInt("subjecttypeid", 1);
        SubjectClassId = RequestUtil.Instance.GetQueryInt("subjectclassid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectId", 0);

        if (SubjectTypeId == 1)
        {
            GuangliLink.Add("子站栏目结构".ToLang());
            AddLink.Add("添加子站类型".ToLang());
        }
        else
        {
            GuangliLink.Add("专题栏目结构".ToLang());
            AddLink.Add("添加专题类型".ToLang());
        }
        GuangliLink.Add("SubjectList.aspx?subjecttypeid=" + SubjectTypeId);
        AddLink.Add("SubjectcLass_Edit.aspx?subjecttypeid=" + SubjectTypeId);


        if (!IsPostBack)
        {
            BindColumnList();
            //控制栏目移动权限
            IsMove = SubjectTypeId == 1 ? IsCurrentRoleMenuRes("211") : IsCurrentRoleMenuRes("212");
        }
    }

    //绑定栏目列表
    private void BindColumnList()
    {
        Columns = GetNodeColumns(ServiceFactory.ColumnService.GetSubjectColumnList(0, SubjectClassId,SubjectId), 0, 1);

    }

    /// <summary>
    /// 获取下级栏目
    /// </summary>
    /// <param name="allColumn"></param>
    /// <param name="parentId"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    public List<Column> GetNodeColumns(IList<Column> allColumn, int parentId, int level)
    {
        var columns = new List<Column>();
        foreach (var column in allColumn.Where(p => p.ParentId == parentId))
        {
            column.LevelNum = level;
            columns.Add(column);
            columns.AddRange(GetNodeColumns(allColumn, column.ColumnId, level + 1));
        }
        return columns;
    }




    /// <summary>
    /// 底部批量操作点击事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Batch_Command(object sender, CommandEventArgs e)
    {
        try
        {
            string commandArgs = e.CommandArgument.ToStr();
            string commandName = e.CommandName.ToStr();

            switch (commandName)
            {
                case "Sort":
                    batchSort();
                    //刷新左侧菜单脚本
                    ReflashLeftMenu();
                    Alert("操作成功".ToLang(), true);
                    break;
            }
        }
        catch (Exception ex)
        {
            ErrorAlert("操作失败：".ToLang() + ex.Message);
            return;
        }
    }

    //批量排序
    private void batchSort()
    {
        string strSort = "";//hidSort.Value.Trim(',');
        string[] arrSort = strSort.Split(',');
        foreach (string str in arrSort)
        {
            int columnID = str.Split('|')[0].ToInt();
            long sort = str.Split('|')[1].ToLong(0);
            ServiceFactory.ColumnService.ModifyColumnSort(columnID, sort);
        }
    }


}