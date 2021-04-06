/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：surveylist.aspx.cs
 * 文件描述：问卷调查列表
 */

using System;
using Whir.Framework;
using Whir.Service;
public partial class Whir_System_ModuleMark_Survey_SurveyList : Whir.ezEIP.Web.SysManagePageBase
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
        contentManager1.IsShowDelete = IsRoleHaveColumnRes("删除");
        contentManager1.IsShowEdit = IsRoleHaveColumnRes("修改");
        contentManager1.EditPageUrl = "Content_edit.aspx?columnid=" + ColumnId + "&subjectid=" + SubjectId + "&BackPageUrl=" + CurrentPageUrl;
        contentManager1.IsShowOpenSort = IsRoleHaveColumnRes("排序");

        contentManager1.IsShowQuestion = IsRoleHaveColumnRes("问题管理");
        contentManager1.IsShowSurveyPreview = IsRoleHaveColumnRes("预览");
        contentManager1.IsShowSurveyStatistics = IsRoleHaveColumnRes("统计");
        contentManager1.IsShowSurveyDetail = IsRoleHaveColumnRes("调查明细");
        contentManager1.IsShowEnable = IsRoleHaveColumnRes("启用");
        contentManager1.IsCurrentPageSetting = true;
        contentManager1.Where = workFlowBar1.GetWhereSql();
    }
  
}