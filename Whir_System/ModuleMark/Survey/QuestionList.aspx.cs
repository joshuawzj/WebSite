﻿/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：questionlist.aspx.cs
 * 文件描述：问卷调查问题列表
 */

using System;
using Whir.Framework;
using Whir.Service;

public partial class Whir_System_ModuleMark_Survey_QuestionList : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 前栏目ID
    /// </summary>
    protected int PreColumnID { get; set; }

    /// <summary>
    /// 调查主题ID
    /// </summary>
    protected int TopicID { get; set; }


    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }


    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        PreColumnID = RequestUtil.Instance.GetQueryInt("PreColumnID", 0);
        TopicID = RequestUtil.Instance.GetQueryInt("topicid", 0);

        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);


        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.SubjectId = SubjectId;
        contentManager1.IsShowDelete = IsRoleHaveColumnRes("删除");
        contentManager1.IsShowEdit = IsRoleHaveColumnRes("修改");
        contentManager1.IsShowOpenSort = IsRoleHaveColumnRes("排序");
        contentManager1.IsShowSurveyAnswer = IsRoleHaveColumnRes("答案管理");
        contentManager1.Where = "TopicID=" + TopicID;
        contentManager1.EditPageUrl = "Content_edit.aspx?columnid=" + ColumnId + "&topicid=" + TopicID + "&subjectid=" + SubjectId + "&history=false" + "&BackPageUrl=" +
                                      CurrentPageUrl;


    }
}



 

  