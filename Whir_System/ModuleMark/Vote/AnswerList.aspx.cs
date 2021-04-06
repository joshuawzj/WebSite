/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：answerlist.aspx.cs
 * 文件描述：答案列表
 */
using System;

using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_Vote_AnswerList : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 问题ID
    /// </summary>
    protected int QuestionID { get; set; }

    /// <summary>
    /// 投票栏目ID
    /// </summary>
    protected int VoteColumnID { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        QuestionID = RequestUtil.Instance.GetQueryInt("questionid", 0);
        VoteColumnID = RequestUtil.Instance.GetQueryInt("votecolumnid", 0);

        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.SubjectId = SubjectId;
        contentManager1.IsShowDelete = IsRoleHaveColumnRes("删除");
        contentManager1.IsShowEdit = IsRoleHaveColumnRes("编辑");
        contentManager1.IsShowOpenSort = IsRoleHaveColumnRes("排序");
        contentManager1.EditPageUrl = "Content_edit.aspx?columnid=" + ColumnId + "&QuestionID=" + QuestionID + "&subjectid=" + SubjectId + "&history=false" + "&BackPageUrl=" + CurrentPageUrl;

    }
}