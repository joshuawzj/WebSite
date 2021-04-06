/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：detaillist.aspx.cs
 * 文件描述：问卷调查详细列表
 */

using System;

using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_Survey_Detaillist : Whir.ezEIP.Web.SysManagePageBase
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
    /// 问卷调查栏目ID
    /// </summary>
    protected int SurveyColumnID { get; set; }


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
        SurveyColumnID = RequestUtil.Instance.GetQueryInt("surveycolumnid", 0);

        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

  
        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.SubjectId = SubjectId;
        IsShowBatchDelete = contentManager1.IsShowDelete = IsRoleHaveColumnRes("删除");
        contentManager1.IsShowEdit = false;
        contentManager1.Where =  "TopicID=" + TopicID;
        contentManager1.IsShowOpenSort=IsRoleHaveColumnRes("排序");
        contentManager1.IsShowDetail = IsRoleHaveColumnRes("查看");
        contentManager1.DetailUrl = "View.aspx?columnid=" + ColumnId + "&topicid=" + TopicID + "&itemid={itemid}&subjectid=" + SubjectId;

    }

   
}