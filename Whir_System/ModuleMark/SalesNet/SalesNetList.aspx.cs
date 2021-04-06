/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：salesnetlist.aspx.cs
* 文件描述：销售网络页面。 
*/

using System;
using Whir.Framework;
using Whir.Service;



public partial class Whir_System_ModuleMark_SalesNet_SalesNetList : Whir.ezEIP.Web.SysManagePageBase
{

    #region 属性
    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }
    /// <summary>
    /// 前当流程节点ID
    /// </summary>
    protected int CurrentActivityId { get; set; }

    /// <summary>
    /// 是否显示批量删除按钮
    /// </summary>
    protected bool IsShowBatchDelete { get; set; }

    /// <summary>
    /// 是否显示批量推送
    /// </summary>
    protected bool IsShowBatchCopy { get; set; }

    /// <summary>
    /// 是否显示批量转移
    /// </summary>
    protected bool IsShowBatchCut { get; set; }

    /// <summary>
    /// 是否显示排序
    /// </summary>
    protected bool IsShowSort { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        CurrentActivityId = RequestUtil.Instance.GetQueryInt("flowid", 0);

        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        workFlowBar1.ColumnId = ColumnId;
        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.SubjectId = SubjectId;
        IsShowBatchDelete = contentManager1.IsShowDelete = IsRoleHaveColumnRes("删除");
        contentManager1.IsShowEdit = IsRoleHaveColumnRes("修改");
        contentManager1.EditPageUrl = "SalesNet_edit.aspx?columnid=" + ColumnId + "&subjectid=" + SubjectId + "&BackPageUrl=" + CurrentPageUrl;
        contentManager1.IsShowOpenSort = IsRoleHaveColumnRes("排序");
        contentManager1.IsShowPush = IsRoleHaveColumnRes("批量推送");
        contentManager1.IsShowTransfer = IsRoleHaveColumnRes("批量转移");
        contentManager1.IsShowHistory = IsRoleHaveColumnRes("历史记录");
        contentManager1.IsShowEdit = IsRoleHaveColumnRes("修改");
        contentManager1.IsShowEdit = IsRoleHaveColumnRes("修改");

        contentManager1.Where = workFlowBar1.GetWhereSql();

    }


}