/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：answerlist.aspx.cs
 * 文件描述：问卷调查问题答案列表
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
using Wuqi.Webdiyer;
using Whir.Language;

public partial class Whir_System_ModuleMark_Survey_Answerlist : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 问题ID
    /// </summary>
    protected int QuestionID { get; set; }

    /// <summary>
    /// 问卷调查栏目ID
    /// </summary>
    protected int SurveyColumnID { get; set; }

    /// <summary>
    /// 问题栏目ID
    /// </summary>
    protected int QuestionColumnID { get; set; }

    /// <summary>
    /// 主题ID
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
        QuestionID = RequestUtil.Instance.GetQueryInt("questionid", 0);
        TopicID = RequestUtil.Instance.GetQueryInt("topicid", 0);

        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.SubjectId = SubjectId;
        contentManager1.IsShowDelete = IsRoleHaveColumnRes("删除");
        contentManager1.Where = "QuestionID=" + QuestionID;
        contentManager1.EditPageUrl = "Content_edit.aspx?columnid=" + ColumnId + "&QuestionID=" + QuestionID + "&subjectid=" + SubjectId + "&history=false" + "&BackPageUrl=" + CurrentPageUrl;
        GetColumnIDs();

    }

    /// <summary>
    /// 获取层级栏目ID
    /// </summary>
    protected void GetColumnIDs()
    {
        IList<Column> list = ServiceFactory.ColumnService.GetMarkListByColumnId(ColumnId);
        if (list != null)
        {
            foreach (var column in list)
            {
                switch (column.MarkType)
                {
                    case null://主栏目
                        SurveyColumnID = column.ColumnId;
                        break;
                    case "Question":
                        QuestionColumnID = column.ColumnId;
                        break;
                }
            }
        }
    }
}