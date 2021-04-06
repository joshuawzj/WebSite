/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：SurveyInfo.ascx.cs
 * 文件描述：问卷调查用户控件
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Repository;
using System.Data;

#region 说明：用到此用户控件的页面必须包含以下脚本
/* 
 <script src="res/js/jquery.js" type="text/javascript"></script>
    <script src="res/js/progressbar/jquery.progressbar.min.js" type="text/javascript"></script>
    <script src="res/js/common.js" type="text/javascript"></script>
    <script>
        $(function () {
            $(".progressbar").each(function () {
                var jq = $(this);
                whir.progressbar.setProgressbarByFraction("", jq, jq.next().text());//appPath根据实际页面定
            })
        });
    </script>
 */
#endregion

public partial class label_survey_SurveyInfo : System.Web.UI.UserControl
{
    #region 对外属性
    /// <summary>
    /// 问卷调查栏目ID
    /// </summary>
    public int ColumnId { get; set; }
    #endregion

    #region 内部属性
    /// <summary>
    /// 问题主题ID(Whir_U_Question_PID)
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
    /// 总数
    /// </summary>
    protected int TotalCount { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // 获取调查主题信息.
            Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
            if (column == null)
                return;
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
            LoadSubjectInfo();
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
    /// 获取调查主题名称、主题主键
    /// </summary>
    private void LoadSubjectInfo()
    {
        string SQL = "SELECT {0}_PID,Title FROM {0} WHERE IsDel=0 AND Enable=1".FormatWith(SurveyTableName);
        DataTable table = DbHelper.CurrentDb.Query(SQL).Tables[0];
        if (table != null && table.Rows.Count > 0)
        {
            TopicID = table.Rows[0][0].ToStr().ToInt();
            ltlSurveyTitle.Text = table.Rows[0][1].ToStr();
        }
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
        String SQL = "SELECT * FROM {0} WHERE TypeID={1} AND TopicID={2} AND IsDel=0 ORDER BY Sort desc,CreateDate desc".FormatWith(model.TableName, QuestionColumnID, TopicID);
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
                TotalCount = GetTotalCount(drv[QuestionTableName + "_PID"].ToInt());
                String SQL = "SELECT * FROM {0} WHERE TypeID={1} AND QuestionID={2} AND IsDel=0 ORDER BY Sort desc,CreateDate desc".FormatWith(AnswerTableName, AnswerColumnID, drv[QuestionTableName + "_PID"]);
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