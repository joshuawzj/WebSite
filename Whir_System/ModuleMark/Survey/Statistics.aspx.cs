/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：statistics.aspx.cs
 * 文件描述：调查统计
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Repository;
using System.Data;
using Wuqi.Webdiyer;
using Whir.Language;

public partial class Whir_System_ModuleMark_Survey_Statistics : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 问题主题ID
    /// </summary>
    protected int TopicID { get; set; }

    /// <summary>
    /// 问题栏目ID
    /// </summary>
    protected int QuestionColumnID { get; set; }

    /// <summary>
    /// 答案栏目ID
    /// </summary>
    protected int AnswerColumnID { get; set; }

    /// <summary>
    /// 问题答案ModelID
    /// </summary>
    protected int AnswerModelID { get; set; }

    /// <summary>
    /// 问题ModelID
    /// </summary>
    protected int QuestionModelID { get; set; }

    /// <summary>
    /// 主题表
    /// </summary>
    protected string SurveyTableName { get; set; }

    /// <summary>
    /// 问题表
    /// </summary>
    protected string QuestionTableName { get; set; }

    /// <summary>
    /// 问题答案表
    /// </summary>
    protected string AnswerTableName { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        if (!IsPostBack)
        {
            TopicID = RequestUtil.Instance.GetQueryInt("topicId", 0);
            ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);    // 调查主题对应栏目Id
            // 获取调查主题信息.
            Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
            if (null == column) return;
            Model mainModel = ServiceFactory.ModelService.SingleOrDefault<Model>(column.ModelId);
            if (mainModel == null)
            {
                return;
            }
            else
            {
                SurveyTableName = mainModel.TableName;
            }
            InitIDs();
            LoadTitle();
            LoadTableName();
            LoadQuestions();
        }
    }

    #region 初始化，获取基本信息，为加载问题列表、答案列表作准备

    /// <summary>
    /// 初始化获取栏目ID
    /// </summary>
    private void InitIDs()
    {
        QuestionColumnID = AnswerColumnID = 0;
        IList<Column> list = ServiceFactory.ColumnService.GetListByMainColumnId(ColumnId);
        if (list == null)
            return;
        foreach (Column column in list)
        {
            switch (column.MarkType)
            {
                case "Question":
                    QuestionColumnID = column.ColumnId;
                    QuestionModelID = column.ModelId;
                    break;
                case "Answer":
                    AnswerColumnID = column.ColumnId;
                    AnswerModelID = column.ModelId;
                    break;
            }
        }
    }

    /// <summary>
    /// 获取调查主题
    /// </summary>
    private void LoadTitle()
    {
        //string SQL = "SELECT Title FROM {0} WHERE TypeID={1} AND IsDel=0".FormatWith(SurveyTableName, ColumnId);
        string SQL = "SELECT Title FROM {0} WHERE {0}_PID={1} AND IsDel=0".FormatWith(SurveyTableName, TopicID);
        string Title = DbHelper.CurrentDb.ExecuteScalar<String>(SQL);
        ltlSurveyTitle.Text = Title;
    }

    /// <summary>
    /// 获取问题表名、答案表名
    /// </summary>
    private void LoadTableName()
    {
        Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(AnswerModelID);
        if (model != null)
        {
            AnswerTableName = model.TableName;
        }
        Model model2 = ServiceFactory.ModelService.SingleOrDefault<Model>(QuestionModelID);
        if (model2 != null)
        {
            QuestionTableName = model2.TableName;
        }
    }

    #endregion

    #region 获取问题列表、答案列表

    /// <summary>
    /// 加载问题
    /// </summary>
    private void LoadQuestions()
    {
        Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(QuestionModelID);
        String SQL = "SELECT * FROM {0} WHERE TypeID={1} AND TopicID={2} AND IsDel=0 Order By Sort ASC".FormatWith(model.TableName, QuestionColumnID, TopicID);
        DataSet set = DbHelper.CurrentDb.Query(SQL);
        rptQuestions.DataSource = DbHelper.CurrentDb.Query(SQL).Tables[0];
        rptQuestions.DataBind();
    }

    //加载答案
    protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Repeater rptAnswers = (Repeater)e.Item.FindControl("rptAnswers");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (null != rptAnswers)
            {
                String SQL = "SELECT * FROM {0} WHERE TypeID={1} AND QuestionID={2} AND IsDel=0 Order By Sort ASC".FormatWith(AnswerTableName, AnswerColumnID, drv[QuestionTableName + "_PID"]);
                DataSet set = DbHelper.CurrentDb.Query(SQL);
                rptAnswers.DataSource = set.Tables[0];
                rptAnswers.DataBind();
            }
        }
    }

    /// <summary>
    /// 获取调查的总数
    /// </summary>
    /// <param name="questionId">问题ID</param>
    /// <returns></returns>
    protected int GetTotalCount(int questionId)
    {
        string SQL = "SELECT SUM(AnswerCount) FROM {0} WHERE QuestionID = {1}".FormatWith(AnswerTableName, questionId);
        object obj = DbHelper.CurrentDb.ExecuteScalar<object>(SQL);
        int count = obj.ToInt();
        return count;
    }

    #endregion
}