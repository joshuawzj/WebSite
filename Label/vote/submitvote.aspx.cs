/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：submitvote.aspx.cs
 * 文件描述：网上投票答案接收出来类
 */
using System;
using System.Collections.Generic;
using System.Data;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;
using Whir.Repository;

public partial class label_vote_submitvote : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            #region 属性
            string Result = "0";//1=成功 0=失败 2=开启了IP限制且已提交过
            int TypeID = 0;//明细栏目ID

            int TopicID = RequestUtil.Instance.GetQueryInt("topicid", 0);            //主题ID
            string content = RequestUtil.Instance.GetQueryString("content");//网上投票内容
            int columnId = RequestUtil.Instance.GetQueryInt("columnid", 0);//网上投票栏目ID
            string Ids = RequestUtil.Instance.GetQueryString("ids").Trim(',');

            string VoteTableName = "";//主题表名
            string AnswerTableName = "";//答案表名
            string DetailTableName = "";//明细表名

            string ip = WebUtil.Instance.GetIP();//用户IP地址

            #endregion

            #region 获取关联表的表名，主键

            IList<Column> listColumn = ServiceFactory.ColumnService.GetMarkListByColumnId(columnId);
            foreach (Column col in listColumn)
            {
                string tableName = DbHelper.CurrentDb.ExecuteScalar<string>("SELECT TableName FROM Whir_Dev_Model WHERE ModelID=@0", col.ModelId);
                if (col.MarkType == "Answer")
                {
                    AnswerTableName = tableName;
                }
                else if (col.MarkType == "Detail")
                {
                    TypeID = col.ColumnId;
                    DetailTableName = tableName;
                }
                else
                {
                    VoteTableName = tableName;
                }
            }
            #endregion

            if (columnId == 0)
            {
                Response.Write(0);
                return;
            }

            if (Ids.Length > 0 && TopicID != 0)
            {
                bool IsPass = true;
                #region 判断IP是否受限

                string SQLSurvey = "SELECT * FROM {0} WHERE {0}_PID=@0".FormatWith(VoteTableName);
                DataTable dtVote = DbHelper.CurrentDb.Query(SQLSurvey, TopicID).Tables[0];
                if (dtVote.Rows.Count > 0)
                {
                    //IP受限
                    if (dtVote.Rows[0]["IsIPLimit"].ToBoolean())
                    {
                        string SQLDetail = "SELECT Count(*) FROM {0} WHERE TopicID=@0 AND IP=@1".FormatWith(DetailTableName);
                        int count = DbHelper.CurrentDb.ExecuteScalar<int>(SQLDetail, TopicID, ip);
                        if (count > 0)
                        {
                            IsPass = false;
                        }
                    }
                }
                #endregion
                if (IsPass)
                {
                    #region 向数据表插入数据

                    string SQLAnswer = "UPDATE {0} SET AnswerCount = case when AnswerCount is null then 1 else AnswerCount + 1 end WHERE {0}_PID = @0".FormatWith(AnswerTableName);
                    foreach (string i in Ids.Split(','))
                    {
                        DbHelper.CurrentDb.Execute(SQLAnswer, i.ToInt());//票数相应加1
                    }

                    SurveyHelper.AddDetail(DetailTableName, TopicID, ip, content, Ids, TypeID, "root");//添加明细
                    SurveyHelper.UpdateTotalCount(VoteTableName, TopicID);
                    Result = "1";
                    #endregion
                }
                else
                {
                    Result = "2";
                }
            }
            else
            {
                Result = "0";
            }
            Response.Write(Result);
        }
        catch (Exception ex)
        {
            Response.Write(0);
        }
    }
}