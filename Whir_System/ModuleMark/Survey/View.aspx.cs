/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：问卷调查查看页面.aspx.cs
 * 文件描述：问卷调查查看页面
 */

using System;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Repository;
using Whir.ezEIP.Web;
using System.Data;
using System.Collections.Generic;

public partial class Whir_System_ModuleMark_Survey_View : SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    /// <summary>
    /// 主栏目ModelID
    /// </summary>
    protected int ModelID { get; set; }

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
    /// 明细ModelID
    /// </summary>
    protected int DetailModelId { get; set; }

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
    /// 明细表名
    /// </summary>
    protected string DetailTableName { get; set; }

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }

    /// <summary>
    /// 明细主键ID
    /// </summary>
    protected int ItemID { get; set; }

    /// <summary>
    /// 答案IDs
    /// </summary>
    protected string Ids { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        ItemID = RequestUtil.Instance.GetQueryInt("itemid", 0);
        if (!IsPostBack)
        {
            TopicID = RequestUtil.Instance.GetQueryInt("topicId", 0);
            ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
            // 获取调查主题信息.
            Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
            if (null == column) return;
            InitIDs();
            LoadTableName();
            LoadTitleInfo();
            LoadQuestions();

            //加载AnswerIds
            string sql = "SELECT AnswerIds FROM {0} WHERE {0}_PID=@0".FormatWith(DetailTableName);
            Ids = DbHelper.CurrentDb.ExecuteScalar<string>(sql, ItemID);
        }
    }

    #region 初始化为获取问题列表，问题答案列表所需要的信息作准备
    /// <summary>
    /// 初始化获取问题栏目ID、答案栏目ID
    /// </summary>
    private void InitIDs()
    {
        QuestionColumnID = AnswerColumnID = 0;
        IList<Column> list = ServiceFactory.ColumnService.GetMarkListByColumnId(ColumnId);
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
                case "Detail":
                    DetailModelId = column.ModelId;
                    break;
                case "":
                case null:
                    ColumnId = column.ColumnId;
                    ModelID = column.ModelId;
                    break;
            }
        }
    }

    /// <summary>
    /// 获取调查预览页面表头
    /// </summary>
    private void LoadTitleInfo()
    {
        string SQL = "SELECT Title FROM {0} WHERE {0}_PID={1} AND IsDel=0".FormatWith(SurveyTableName, TopicID);
       string titleStr = DbHelper.CurrentDb.ExecuteScalar<string>(SQL);
       ltlSurveyTitle.Text = titleStr;
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
        Model modelDetail = ServiceFactory.ModelService.SingleOrDefault<Model>(DetailModelId);
        if (modelDetail != null)
        {
            DetailTableName = modelDetail.TableName;
        }
        Model modelMain = ServiceFactory.ModelService.SingleOrDefault<Model>(ModelID);
        if (modelMain != null)
        {
            SurveyTableName = modelMain.TableName;
        }
    }

    #endregion

    #region 获取问题列表、获取答案列表
    /// <summary>
    /// 绑定问题
    /// </summary>
    private void LoadQuestions()
    {
        Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(QuestionModelID);
        string SQL = "SELECT * FROM {0} WHERE TypeID={1} AND TopicID={2} AND IsDel=0 Order By Sort ASC".FormatWith(model.TableName, QuestionColumnID, TopicID);
        DataSet set = DbHelper.CurrentDb.Query(SQL);
        rptQuestions.DataSource = DbHelper.CurrentDb.Query(SQL).Tables[0];
        rptQuestions.DataBind();

    }

    //绑定答案
    protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            try
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (null != drv)
                {
                    whir_system_UserControl_ContentControl_AnswerForm af = e.Item.FindControl("AnswerForm1") as whir_system_UserControl_ContentControl_AnswerForm;

                    af.QuestionID = (drv[QuestionTableName + "_PID"]).ToInt();
                    af.TypeID = AnswerColumnID;
                    af.AnswerTableName = AnswerTableName;
                    af.QuestionType = drv["QuestionType"].ToBoolean();

                    af.LoadData();
                }
            }
            catch
            { }
        }
    }

    #endregion
}