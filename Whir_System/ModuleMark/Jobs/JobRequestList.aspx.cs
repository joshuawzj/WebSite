/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：jobrequestlist.aspx.cs
 * 文件描述：应聘信息
 */

using System;
using Whir.Framework;
using Whir.Service;
using System.Collections.Generic;
using Whir.Domain;
using System.Linq;
using Whir.Language;
using Whir.ezEIP.Web;

public partial class Whir_System_ModuleMark_Jobs_JobRequestList : SysManagePageBase
{
    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 当前的应聘ID
    /// </summary>
    protected int JobsId { get; set; }

    /// <summary>
    /// 是否显示批量删除按钮
    /// </summary>
    protected bool IsShowBatchDelete { get; set; }

    /// <summary>
    /// 是否显示排序
    /// </summary>
    protected bool IsShowSort { get; set; }

    /// <summary>
    /// 是否显示导出
    /// </summary>
    protected bool IsShowOutput { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 绑定附属操作
    /// </summary>
    protected IList<Column> AttchlistColumn { get; set; }

    protected bool IsShowAttchlist { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {

        ColumnId = WebUtil.Instance.GetQueryInt("columnid", 0);
        JobsId = WebUtil.Instance.GetQueryInt("jobsid", 0);

        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.SubjectId = SubjectId;
        contentManager1.IsShowDelete = IsRoleHaveColumnRes("删除");
        contentManager1.IsShowEdit = IsRoleHaveColumnRes("修改");
        contentManager1.IsShowOpenSort = IsRoleHaveColumnRes("排序");
        contentManager1.IsShowExport = IsRoleHaveColumnRes("导出");
        contentManager1.IsShowDetail = IsRoleHaveColumnRes("查看");
        contentManager1.EditPageUrl = "Content_Edit.aspx?columnid=" + ColumnId + "&jobid=" + JobsId + "&subjectid=" + SubjectId + "&BackPageUrl=" + CurrentPageUrl;
        contentManager1.DetailUrl = "JobRequestDetails.aspx?columnid=" + ColumnId + "&itemid={itemid}&subjectid=" + SubjectId;
        contentManager1.Where = workFlowBar1.GetWhereSql() + " AND JobID=" + JobsId;
        BindAttchColumnList();
    }

    //绑定附属栏目
    private void BindAttchColumnList()
    {
        var listColumn = ServiceFactory.ColumnService.GetMarkListByColumnId(ColumnId);
        var mainColumn = listColumn.FirstOrDefault(p => p.MarkParentId == 0);

        if (mainColumn != null)
        {
            if (!mainColumn.IsCategory)
                AttchlistColumn = listColumn.Where(p => p.MarkParentId== 0).ToList();
            else
                AttchlistColumn = listColumn.Where(p => p.MarkParentId == 0 || p.MarkType == "Category").ToList();
 
             IsShowAttchlist = IsRoleHaveColumnRes("类别管理");
        }
    }

}