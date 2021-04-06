/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：memberselect.aspx.cs
 * 文件描述：邮件群发页面选择会员页面
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Service;
using System.Data;
using Whir.Repository;
using System.Text;
using Whir.Framework;
using Whir.Language;

public partial class Whir_system_Plugin_Email_MemberSelect : Whir.ezEIP.Web.SysManagePageBase
{
    public string JsCallback { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        JsCallback = RequestUtil.Instance.GetQueryString("jscallback");
        //固定1为会员栏目
        contentManager1.ColumnId = 1;
        contentManager1.SelectedType = SelectedType.CheckBox;
        contentManager1.IsDel = false;
        contentManager1.IsShowDelete = false;
        contentManager1.IsShowEdit = false;
        contentManager1.IsOpenFrame = true;
        contentManager1.Where = "";

    }
    protected void Save_Click(object sender, EventArgs e)
    {
        string ids = hidSelected.Value;
        string SQL = "SELECT Whir_Mem_Member_PID Id,Email,LoginName FROM Whir_Mem_Member WHERE Whir_Mem_Member_PID IN(" + ids + ") Order By ID DESC";
        DataSet set = DbHelper.CurrentDb.Query(SQL);
        //构建JSON数据
        StringBuilder jsonBuilder = new StringBuilder();
        string json = "";
        if (set.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in set.Tables[0].Rows)
            {
                jsonBuilder.Append("{'id':'");
                jsonBuilder.Append(row[0]);
                jsonBuilder.Append("','email':'");
                jsonBuilder.Append(row[1]);
                jsonBuilder.Append("','name':'");
                jsonBuilder.Append(row[2]);
                jsonBuilder.Append("'},");
            }
            json = jsonBuilder.ToString().Substring(0, jsonBuilder.Length - 1);//去除“,”
        }
        string script = "<script>callback(\""+json+"\");</script>";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "", script);
    }
}