/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：sort_category.aspx.cs
 * 文件描述：类别选择排序页面
 */

using System;
using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Whir.Repository;
using System.Data;

public partial class whir_system_ModuleMark_common_sort_category : Whir.ezEIP.Web.SysManagePageBase
{    
    
    /// <summary>
    /// 子站ID
    /// </summary>
    protected int SubjectId { get; set; }
    /// <summary>
    /// 要排序的栏目
    /// </summary>
    protected int ColumnId { get; set; }

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


        contentManager1.ColumnId = ColumnId;
        contentManager1.SelectedType = SelectedType.RadioBox;
        contentManager1.IsDel = false;
        contentManager1.IsShowDelete = false;
        contentManager1.IsShowEdit = false;
        contentManager1.Where = exceptData();

    }

    //获取要排除的主键ID值
    private string exceptData()
    {
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
        Model model = ServiceFactory.ModelService.GetModelByColumnId(ColumnId);
        if (model == null) return string.Empty;
        if (ExceptPrimaryIDs.IsEmpty()) return string.Empty;
        GetWorkflowWhere();//工作流过滤条件
        string primaryKeyName = model.TableName + "_PID";
        string exceptWhere = " IsDel=0 ";

        string sql = "select ParentId from {0} where typeid=@0 and IsDel={1} AND {2} in (@1)".FormatWith(model.TableName, 0, primaryKeyName);
        int parentID = DbHelper.CurrentDb.FirstOrDefault<string>(sql, ColumnId, ExceptPrimaryIDs).ToInt();
        //子站排序
        if (SubjectId > 0)
        {
            exceptWhere += " AND SubjectId='" + SubjectId + "'";
        }
        if (!WhereSQL.IsEmpty())
        {
            exceptWhere += WhereSQL;
        }
        exceptWhere += "AND {0} not in ({1}) and ParentId = {2}".FormatWith(primaryKeyName, ExceptPrimaryIDs, parentID);

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
            int WorkFlowId = model != null ? model.WorkFlow : 0;

            if (WorkFlowId <= 0)
            { //没有使用流程节点的，忽略下面操作
                return;
            }

            //用于记录该工作流所有流程节点，格式"1,2,3"
            string ids = "";
            int FirstActivityId = 0;//流程的第一个节点

            //其它节点
            List<AuditActivity> list = ServiceFactory.AuditActivityService.GetListBySort(WorkFlowId);
            for (int i = 0; i < list.Count; i++)
            {
                AuditActivity a = list[i];
                if (i == 0) { FirstActivityId = a.ActivityId; }
                ids += a.ActivityId + ",";
            }
            ids = ids.Trim(',');

            int CurrentActivityID = FlowID == 0 ? -1 : FlowID;
            if (CurrentActivityID == FirstActivityId)
            {
                //把所有不是该流程节点的值都归于第一个节点，因为之前可能是A工作流，加记录后又改成B工作流后造成的
                ids = "," + ids + ",";
                ids = ids.Replace(string.Format(",{0},", FirstActivityId), "").Trim(',');//不包括本身
                string NoIds = (ids + ",-1,-2").Trim(',');
                WhereSQL += string.Format(" AND (state NOT IN({0}) OR State IS NULL)", NoIds);
            }
            else
            {
                WhereSQL += string.Format(" AND state={0}", CurrentActivityID);
            }
        }
    }

}