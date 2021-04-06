/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：sort.aspx.cs
 * 文件描述：选择排序页面
 */

using System;
using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class Whir_System_ModuleMark_Common_Sort : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 要排序的栏目
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 子站ID
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 当前栏目的所有记录总数
    /// </summary>
    protected int Total { get; set; }

    /// <summary>
    /// 查询要排除的条件
    /// </summary>
    protected string ExceptPrimaryIDs { get; set; }

    /// <summary>
    /// 根据传入的columnid，返回用于过滤列表的SQL的条件参数
    /// </summary>
    public string WhereSQL
    {
        get
        {
            return ViewState["WhereSQL"].ToStr();
        }
        set
        {
            ViewState["WhereSQL"] = value;
        }
    }

    /// <summary>
    /// 工作流ID
    /// </summary>
    protected int FlowID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        ExceptPrimaryIDs = RequestUtil.Instance.GetQueryString("except");
        FlowID = RequestUtil.Instance.GetQueryInt("flowid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        Total = RequestUtil.Instance.GetQueryInt("total", 0);

        JudgeOpenPagePermission(ColumnId == 1 ? IsCurrentRoleMenuRes("302") : IsRoleHaveColumnRes("排序", ColumnId, SubjectId == 0 ? -1 : SubjectId));

        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.RadioBox;
        contentManager1.IsDel = false;
        contentManager1.IsShowDelete = false;
        contentManager1.IsShowEdit = false;
        contentManager1.IsOpenFrame = true;
        contentManager1.SubjectId = SubjectId;
        contentManager1.Where = ExceptData();

    }

    //获取要排除的主键ID值
    private string ExceptData()
    {
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        Model model = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
        if (model == null) return string.Empty;
        if (ExceptPrimaryIDs.IsEmpty()) return string.Empty;
        GetWorkflowWhere();//工作流过滤条件
        string primaryKeyName = model.TableName + "_PID";
        string exceptWhere = " IsDel=0 ";
        //子站排序
        if (SubjectId > 0)
        {
            exceptWhere += " AND SubjectId='" + SubjectId + "'";
        }
        if (!WhereSQL.IsEmpty())
        {
            exceptWhere += WhereSQL;
        }
        exceptWhere += "AND {0} not in ({1})".FormatWith(primaryKeyName, ExceptPrimaryIDs);

        //if (column.IsCategory)
        //{
        //    exceptWhere += " AND CategoryID in (SELECT CategoryID FROM {0} WHERE  {1} in ({2}))".FormatWith(model.TableName, primaryKeyName, ExceptPrimaryIDs);
        //}
        //else 

        if (column.MarkType == "JobRequest")
        {
            exceptWhere += " AND JobId in (SELECT JobId FROM {0} WHERE  {1} in ({2}))".FormatWith(model.TableName, primaryKeyName, ExceptPrimaryIDs);
        }
        else if (column.MarkType == "Answer")
        {
            exceptWhere += " AND QuestionID in (SELECT QuestionID FROM {0} WHERE  {1} in ({2}))".FormatWith(model.TableName, primaryKeyName, ExceptPrimaryIDs);
        }


        #region 动态参数
        int topicId = RequestUtil.Instance.GetQueryInt("topicId", 0);//问卷调查、网上投票用到，问题列表
        if (topicId > 0)
        {
            exceptWhere += " AND TopicID=" + topicId;
        }

        int questionId = RequestUtil.Instance.GetQueryInt("questionid", 0);//问卷调查、网上投票用到，答案列表
        if (questionId > 0)
        {
            exceptWhere += " AND QuestionID=" + questionId;
        }

        int magazineid = RequestUtil.Instance.GetQueryInt("magazineid", 0); //电子期刊中的期刊Id，通章节列表或者文章列表传递过来

        if (magazineid > 0)
        {
            exceptWhere += " AND MagazineID=" + magazineid;
        }
        #endregion
        return exceptWhere;
    }

    /// <summary>
    /// 获取工作流过滤条件
    /// </summary>
    /// <returns></returns>
    private void GetWorkflowWhere()
    {
        if (!IsPostBack)
        {
            Column model = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
            int workFlowId = model != null ? model.WorkFlow : 0;

            if (workFlowId <= 0)
            { //没有使用流程节点的，忽略下面操作
                return;
            }

            //用于记录该工作流所有流程节点，格式"1,2,3"
            string ids = "";
            int firstActivityId = 0;//流程的第一个节点

            //其它节点
            List<AuditActivity> list = ServiceFactory.AuditActivityService.GetListBySort(workFlowId);
            for (int i = 0; i < list.Count; i++)
            {
                AuditActivity a = list[i];
                if (i == 0) { firstActivityId = a.ActivityId; }
                ids += a.ActivityId + ",";
            }
            ids = ids.Trim(',');

            int currentActivityId = FlowID == 0 ? -1 : FlowID;
            if (currentActivityId == firstActivityId)
            {
                //把所有不是该流程节点的值都归于第一个节点，因为之前可能是A工作流，加记录后又改成B工作流后造成的
                ids = "," + ids + ",";
                ids = ids.Replace(string.Format(",{0},", firstActivityId), "").Trim(',');//不包括本身
                string NoIds = (ids + ",-1,-2").Trim(',');
                WhereSQL += string.Format(" AND (state NOT IN({0}) OR State IS NULL)", NoIds);
            }
            else
            {
                WhereSQL += string.Format(" AND state={0} ", currentActivityId);
            }
        }
    }

}