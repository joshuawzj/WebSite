/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：preview.aspx.cs
 * 文件描述：问卷调查前台Ajax调用返回调查信息文件
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Repository;

public partial class label_survey_preview : System.Web.UI.Page
{
    #region 属性
    /// <summary>
    /// 问题主题表名称
    /// </summary>
    protected string SurveyTableName { get; set; }

    /// <summary>
    /// 问题调查表表名
    /// </summary>
    protected string QuestionTableName { get; set; }

    /// <summary>
    /// 问题调查答案表表名
    /// </summary>
    protected string AnswerTableName { get; set; }

    /// <summary>
    /// 问题调查明细表表名
    /// </summary>
    protected string DetailTableName { get; set; }

    /// <summary>
    /// 问题栏目ID
    /// </summary>
    protected int AnswerColumnId { get; set; }

    /// <summary>
    /// 网站根目录
    /// </summary>
    protected string AppName { get { return WebUtil.Instance.AppPath(); } }

    /// <summary>
    /// 主题ID
    /// </summary>
    public int TopicId { get; set; }
    /// <summary>
    /// 问题调查栏目ID
    /// </summary>
    public int SurveyColumnId { get; set; }
    #endregion

    #region 多语言属性
    /// <summary>
    /// 提交按钮文字
    /// </summary>
    public string SubmitText { get; set; }

    /// <summary>
    /// 成功提交后挑战的页面
    /// </summary>
    public string SuccessUrl { get; set; }

    /// <summary>
    /// 提交成功提示
    /// </summary>
    public string SuccessfulTips { get; set; }

    /// <summary>
    /// 提交失败提示
    /// </summary>
    public string FailedTips { get; set; }

    /// <summary>
    /// 没有答完题目就提交调查的提示
    /// </summary>
    public string UnSelectAllTips { get; set; }

    /// <summary>
    /// IP重复提示
    /// </summary>
    public string IpRepeatTips { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        SurveyColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        #region 获取多语言提示
        SubmitText = Safe360.CheckData(RequestUtil.Instance.GetQueryString("submittext"))?"提交":RequestUtil.Instance.GetQueryString("submittext");
        SuccessfulTips = Safe360.CheckData(RequestUtil.Instance.GetQueryString("successfultips")) ? "提交成功" : RequestUtil.Instance.GetQueryString("successfultips");
        FailedTips = Safe360.CheckData(RequestUtil.Instance.GetQueryString("failedtips")) ? "提交失败" : RequestUtil.Instance.GetQueryString("failedtips");
        UnSelectAllTips = Safe360.CheckData(RequestUtil.Instance.GetQueryString("unselectalltips")) ? "所有问题必须选择答案" : RequestUtil.Instance.GetQueryString("unselectalltips");
        IpRepeatTips = Safe360.CheckData(RequestUtil.Instance.GetQueryString("iprepeattips")) ? "同一IP不能重复提交" : RequestUtil.Instance.GetQueryString("iprepeattips");
        SuccessUrl = Safe360.CheckData(RequestUtil.Instance.GetQueryString("successurl")) ? "#" : RequestUtil.Instance.GetQueryString("successurl"); 

        #endregion
        if (!IsPostBack)
        {
            #region 获取关联表的表名，主键
            
            IList<Column> listColumn = ServiceFactory.ColumnService.GetMarkListByColumnId(SurveyColumnId);
            foreach (Column col in listColumn)
            {
                //表名
                string tableName = DbHelper.CurrentDb.ExecuteScalar<string>("SELECT TableName FROM Whir_Dev_Model WHERE ModelID=@0", col.ModelId);
                if (col.MarkType == "Question")
                {
                    QuestionTableName = tableName;
                }
                else if (col.MarkType == "Answer")
                {
                    AnswerTableName = tableName;
                    AnswerColumnId = col.ColumnId;
                }
                else if (col.MarkType == "Detail")
                {
                    DetailTableName = tableName;
                }
                else
                {
                    SurveyTableName = tableName;
                }
            }
            #endregion
            BandList();
        }
    }

    /// <summary>
    /// 绑定问题列表
    /// </summary>
    private void BandList()
    {
        // 获取主题信息表.
        string sql = "SELECT * FROM {0} WHERE IsDel=0 AND Enable=1 AND TypeID=@0 AND BeginDate<=@1 and Enddate>=@1 ORDER BY Sort desc,CreateDate desc".FormatWith(SurveyTableName);
        var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(SurveyColumnId);
        if (column != null & column.WorkFlow != 0)//开启工作流
        {
            sql += " AND State=-1 ";
        }
        DataTable table = DbHelper.CurrentDb.Query(sql, SurveyColumnId, DateTime.Now).Tables[0];
        if (table.Rows.Count > 0)
        {
            TopicId = table.Rows[0][0].ToString().ToInt();
            string sqlQuestion = "SELECT * FROM {0} WHERE TopicID=@0 AND IsDel=0 ORDER BY Sort desc,CreateDate desc".FormatWith(QuestionTableName);
            DataTable dt = DbHelper.CurrentDb.Query(sqlQuestion, TopicId).Tables[0];
            rptQuestions.DataSource = dt;
            rptQuestions.DataBind();
        }
        else
        {
            panel1.Visible = false;
            ltlSurvey.Text = "当前问卷调查未启用或已过期";
        }
    }

    /// <summary>
    /// 绑定答案列表
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            try
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (null != drv)
                {
                    whir_system_UserControl_ContentControl_AnswerForm af = e.Item.FindControl("Answerform") as whir_system_UserControl_ContentControl_AnswerForm;
                    af.QuestionID = (drv[QuestionTableName + "_PID"]).ToInt();
                    af.TypeID = AnswerColumnId;
                    af.AnswerTableName = AnswerTableName;
                    af.QuestionType = drv["QuestionType"].ToBoolean();
                    af.LoadData();
                }
            }
            catch { }
        }

    }

}