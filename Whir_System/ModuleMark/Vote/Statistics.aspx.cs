/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：statistics.aspx.cs
 * 文件描述：网上投票统计
 */
using System;
using System.Collections.Generic;
using System.Data;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Repository;



public partial class Whir_System_ModuleMark_Vote_Statistics : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 主题ID
    /// </summary>
    protected int VoteID { get; set; }

    /// <summary>
    /// 答案栏目ID
    /// </summary>
    protected int AnswerColumnID { get; set; }

    /// <summary>
    /// 答案数据表名
    /// </summary>
    protected string AnswerTableName { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 总数
    /// </summary>
    protected int TotalCount { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        VoteID = RequestUtil.Instance.GetQueryInt("voteid", 0);

        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        Init();
        if (AnswerTableName != "")
        {
            LoadAnswers();
        }
    }

    #region 初始化，获取基本信息，为加载问题列表、答案列表作准备

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        IList<Column> list = ServiceFactory.ColumnService.GetListByMainColumnId(ColumnId);
        if (list == null)
        {
            return;
        }
        int VoteModelID = 0, AnswerModelID = 0;
        string VoteTableName = string.Empty;
        foreach (Column column in list)
        {
            switch (column.MarkType)
            {
                case null:
                    VoteModelID = column.ModelId;
                    break;
                case "Answer":
                    AnswerColumnID = column.ColumnId;
                    AnswerModelID = column.ModelId;
                    break;
            }
        }
        //主题表名
        Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(VoteModelID);
        if (model != null)
        {
            VoteTableName = model.TableName;
        }
        else
        {
            return;
        }
        //答案表名
        Model model2 = ServiceFactory.ModelService.SingleOrDefault<Model>(AnswerModelID);
        if (model2 != null)
        {
            AnswerTableName = model2.TableName;
        }
        else
        {
            return;
        }
        //获取调查主题
        string SQL = "SELECT Title FROM {0} WHERE {0}_PID={1} AND IsDel=0".FormatWith(VoteTableName, VoteID);
        string Title = DbHelper.CurrentDb.ExecuteScalar<String>(SQL);
        ltlSurveyTitle.Text = Title;
    }

    #endregion

    #region 获取问题列表、答案列表

    /// <summary>
    /// 加载问题
    /// </summary>
    private void LoadAnswers()
    {
        TotalCount = GetTotalCount();
        String SQL = "SELECT * FROM {0} WHERE TypeID={1} AND QuestionID={2} AND IsDel=0 Order By Sort ASC".FormatWith(AnswerTableName, AnswerColumnID, VoteID);
        DataSet set = DbHelper.CurrentDb.Query(SQL);
        rptAnswers.DataSource = set.Tables[0];
        rptAnswers.DataBind();
    }

    /// <summary>
    /// 获取调查的总数
    /// </summary>
    /// <param name="questionId">问题ID</param>
    /// <returns></returns>
    protected int GetTotalCount()
    {
        string SQL = "SELECT SUM(AnswerCount) FROM {0} WHERE QuestionID = {1} AND TypeID={2}".FormatWith(AnswerTableName, VoteID, AnswerColumnID);
        object obj = DbHelper.CurrentDb.ExecuteScalar<object>(SQL);
        int count = obj.ToInt();
        return count;
    }

    #endregion
}