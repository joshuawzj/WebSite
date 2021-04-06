/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：detaillist.aspx.cs
 * 文件描述：网上投票详细列表
 */
using System;

using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_Vote_DetailList : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 调查主题ID
    /// </summary>
    protected int TopicID { get; set; }

    /// <summary>
    /// 网上投票栏目ID
    /// </summary>
    protected int VoteColumnID { get; set; }


    /// <summary>
    /// 是否显示批量删除按钮
    /// </summary>
    protected bool IsShowBatchDelete { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        TopicID = RequestUtil.Instance.GetQueryInt("topicid", 0);
        VoteColumnID = RequestUtil.Instance.GetQueryInt("votecolumnid", 0);

        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);


        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.IsShowEdit = false;
        contentManager1.SubjectId = SubjectId;
        contentManager1.IsShowDelete = IsRoleHaveColumnRes("删除");
        contentManager1.IsShowDetail = IsRoleHaveColumnRes("查看");
        contentManager1.DetailUrl = "View.aspx?columnid=" + VoteColumnID + "&voteid=" + TopicID + "&itemid={itemid}&subjectid=" + SubjectId;
        contentManager1.Where = "TopicID=" + TopicID;

    }
}