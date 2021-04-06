/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：AnswerForm.ascx.cs
 * 文件描述：问题答案显示用户控件
 */
using System;
using System.Data;

using Whir.Repository;
using Whir.Framework;

public partial class whir_system_UserControl_ContentControl_AnswerForm : System.Web.UI.UserControl
{
    #region 对外开放公共属性
    /// <summary>
    /// 问题ID
    /// </summary>
    public int QuestionID { get; set; }

    /// <summary>
    /// 主题ID
    /// </summary>
    public int TypeID { get; set; }

    /// <summary>
    /// 问题答案表名
    /// </summary>
    public string AnswerTableName { get; set; }

    /// <summary>
    /// 问题类型 True:多选 False:单选
    /// </summary>
    public bool QuestionType { get; set; }

    /// <summary>
    /// 主键ID
    /// </summary>
    protected string PrimaryKeyName { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// 绑定答案到列表上
    /// </summary>
    public void LoadData()
    {
        PrimaryKeyName = AnswerTableName + "_PID";
        String SQL = "SELECT * FROM {0} WHERE TypeID={1} AND QuestionID={2} AND IsDel=0 Order By Sort DESC,CreateDate DESC";
        DataSet set = DbHelper.CurrentDb.Query(SQL.FormatWith(AnswerTableName, TypeID, QuestionID));
        if (set == null || set.Tables[0] == null)
        {
            return;
        }
        if (QuestionType)//多选
        {
            this.PlhCheckBoxList.Visible = true;
            this.cblList.DataSource = set.Tables[0];
            this.cblList.DataBind();
        }
        else//单选
        {
            this.PlhRadioList.Visible = true;
            rplist.DataSource = set.Tables[0];
            rplist.DataBind();
        }

    }
}