/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：VoteInfo.ascx.cs
 * 文件描述：网上投票用户控件
 */
using System;
using System.Collections.Generic;

using Whir.Service;
using Whir.Domain;
using Whir.Framework;
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

public partial class label_vote_VoteInfo : System.Web.UI.UserControl
{
    #region 对外属性
    /// <summary>
    /// 网上投票栏目ID
    /// </summary>
    public int ColumnId { get; set; }

    #endregion

    #region 内部属性

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
    /// 总数
    /// </summary>
    protected int TotalCount { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        GetColumnInfo();
        if (AnswerTableName != "")
        {
            BandAnswers();
        }
    }

    #region 初始化，获取基本信息，为加载问题列表、答案列表作准备

    /// <summary>
    /// 初始化，从数据库栏目的基本信息
    /// </summary>
    private void GetColumnInfo()
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
        string SQL = "SELECT {0}_PID,Title FROM {0} WHERE IsDel=0 AND Enable=1 AND BeginDate<@0 AND EndDate>@0".FormatWith(VoteTableName);
        DataSet set = DbHelper.CurrentDb.Query(SQL, DateTime.Now);
        if (set.Tables.Count > 0 && set.Tables[0].Rows.Count > 0)
        {
            VoteID = set.Tables[0].Rows[0][0].ToStr().ToInt();
            ltlVoteTitle.Text = set.Tables[0].Rows[0][1].ToStr();
        }
    }

    #endregion

    #region 获取答案列表

    /// <summary>
    /// 绑定答案列表
    /// </summary>
    private void BandAnswers()
    {
        TotalCount = GetTotalCount();
        String SQL = "SELECT * FROM {0} WHERE TypeID={1} AND QuestionID={2} AND IsDel=0   ORDER BY Sort desc,CreateDate desc".FormatWith(AnswerTableName, AnswerColumnID, VoteID);
        DataSet set = DbHelper.CurrentDb.Query(SQL);
        rptAnswers.DataSource = set.Tables[0];
        rptAnswers.DataBind();
    }

    /// <summary>
    /// 获取调查的总数
    /// </summary>
    /// <returns></returns>
    protected int GetTotalCount()
    {
        string SQL = "SELECT SUM(AnswerCount) FROM {0} WHERE QuestionID = {1} AND TypeID={2}  ORDER BY Sort desc,CreateDate desc".FormatWith(AnswerTableName, VoteID, AnswerColumnID);
        object obj = DbHelper.CurrentDb.ExecuteScalar<object>(SQL);
        int count = obj.ToInt();
        return count;
    }

    #endregion
}