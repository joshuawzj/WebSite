/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：preview.aspx.cs
 * 文件描述：网上投票Ajax调用返回调查信息文件
 */

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using Whir.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

public partial class label_vote_preview : Page
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
    /// 网站根目录
    /// </summary>
    protected string AppName { get { return WebUtil.Instance.AppPath(); } }

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
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        #region 获取多语言提示

        SubmitText = Safe360.CheckData(RequestUtil.Instance.GetQueryString("submittext")) ? "提交" : RequestUtil.Instance.GetQueryString("submittext");
        SuccessfulTips = Safe360.CheckData(RequestUtil.Instance.GetQueryString("successfultips")) ? "提交成功" : RequestUtil.Instance.GetQueryString("successfultips");
        FailedTips = Safe360.CheckData(RequestUtil.Instance.GetQueryString("failedtips")) ? "提交失败" : RequestUtil.Instance.GetQueryString("failedtips");
        UnSelectAllTips = Safe360.CheckData(RequestUtil.Instance.GetQueryString("unselectalltips")) ? "所有问题必须选择答案" : RequestUtil.Instance.GetQueryString("unselectalltips");
        IpRepeatTips = Safe360.CheckData(RequestUtil.Instance.GetQueryString("iprepeattips")) ? "同一IP不能重复提交" : RequestUtil.Instance.GetQueryString("iprepeattips");
        SuccessUrl = Safe360.CheckData(RequestUtil.Instance.GetQueryString("successurl")) ? "#" : RequestUtil.Instance.GetQueryString("successurl"); 

        #endregion

        Init();
        if (VoteID <= 0)
        {
            panel1.Visible = false;
            ltlTip.Text = "当前没有网上投票";
        }
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
        string voteTableName;
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
        string sql = "SELECT * FROM {0} WHERE TypeID=@0 AND Enable=1 AND IsDel=0 AND  BeginDate<@1 AND Enddate>@1  ORDER BY Sort desc,CreateDate desc".FormatWith(voteTableName);
        DataSet set = DbHelper.CurrentDb.Query(sql, ColumnId, DateTime.Now);
        if (set == null || set.Tables[0].Rows.Count <= 0)
        {
            ltlVoteTitle.Text = "当前网上投票已过期";
            tr_btn.Visible = false;
            return;
        }
        var table = set.Tables[0];
        ltlVoteTitle.Text = table.Rows[0]["Title"].ToStr();
        QuestionType = table.Rows[0]["QuestionType"].ToBoolean();
        VoteID = table.Rows[0]["{0}_PID".FormatWith(voteTableName)].ToStr().ToInt();
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