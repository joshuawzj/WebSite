/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：feedbacklist.aspx.cs
* 文件描述：网上留言页面
*/

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;
using Wuqi.Webdiyer;
using Whir.Language;

public partial class Whir_System_ModuleMark_FeedBack_FeedBackList : Whir.ezEIP.Web.SysManagePageBase
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

        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.SubjectId = SubjectId;
        IsShowBatchDelete = contentManager1.IsShowDelete = IsRoleHaveColumnRes("删除");
        contentManager1.IsShowEdit = IsRoleHaveColumnRes("修改");
        contentManager1.IsShowOpenSort = IsRoleHaveColumnRes("排序");
        contentManager1.IsShowDetail = IsRoleHaveColumnRes("查看");
        contentManager1.Where = workFlowBar1.GetWhereSql();
        contentManager1.EditPageUrl = "FeedBack_Edit.aspx?columnid=" + ColumnId + "&subjectid=" + SubjectId + "&BackPageUrl=" + CurrentPageUrl;

    }




}