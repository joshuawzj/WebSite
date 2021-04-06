/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：preview.aspx.cs
 * 文件描述：网上投票预览页面
 */
using System;
using System.Collections.Generic;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Repository;
using System.Data;

public partial class Whir_System_ModuleMark_Vote_Preview : Whir.ezEIP.Web.SysManagePageBase
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

    protected bool QuestionType = false;

    /// <summary>
    /// 子站、专题栏目ID
    /// </summary>
    protected int SubjectId { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        VoteID = RequestUtil.Instance.GetQueryInt("voteid", 0);

        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);

        Init();
        if (AnswerTableName != "")
        {
            BandAnswer();
        }
    }

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
        int voteModelId = 0, answerModelId = 0;
        string voteTableName = string.Empty;
        foreach (Column column in list)
        {
            switch (column.MarkType)
            {
                case null:
                    voteModelId = column.ModelId;
                    break;
                case "Answer":
                    AnswerColumnID = column.ColumnId;
                    answerModelId = column.ModelId;
                    break;
            }
        }
        //主题表名
        Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(voteModelId);
        if (model != null)
        {
            voteTableName = model.TableName;
        }
        else
        {
            return;
        }
        //答案表名
        Model model2 = ServiceFactory.ModelService.SingleOrDefault<Model>(answerModelId);
        if (model2 != null)
        {
            AnswerTableName = model2.TableName;
        }
        else
        {
            return;
        }
        //获取调查主题
        string sql = "SELECT * FROM {0} WHERE TypeID={1} AND {0}_PID=@0 AND IsDel=0".FormatWith(voteTableName, ColumnId);
        DataSet set = DbHelper.CurrentDb.Query(sql, VoteID);
        if (set == null || set.Tables[0].Rows.Count <= 0)
        {
            return;
        }
        var table = set.Tables[0];
        ltlSurveyTitle.Text = table.Rows[0]["Title"].ToStr();
        QuestionType = set.Tables[0].Rows[0]["QuestionType"].ToBoolean();
    }

    /// <summary>
    /// 绑定答案列表
    /// </summary>
    protected void BandAnswer()
    {
        AnswerForm1.QuestionID = VoteID;
        AnswerForm1.TypeID = AnswerColumnID;
        AnswerForm1.AnswerTableName = AnswerTableName;
        AnswerForm1.QuestionType = QuestionType;

        AnswerForm1.LoadData();
    }
}