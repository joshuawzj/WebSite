/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：emailmass.aspx.cs
 * 文件描述：邮件群发页面
 */
using System;
using System.Text;
using System.Linq;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Collections.Generic;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;
using Whir.Config;
using Whir.Language;
using System.Text.RegularExpressions;
using System.Data;
using Whir.Repository;

public partial class Whir_System_Plugin_Email_EmailMass : Whir.ezEIP.Web.SysManagePageBase
{
    public string OptionMemberGroup { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack) return;
        JudgePagePermission(IsCurrentRoleMenuRes("14"));
        BandTag();
        LoadMemberGroup();
    }

    /// <summary>
    /// 绑定常用标签
    /// </summary>
    protected void BandTag()
    {
        StringBuilder builder = new StringBuilder("可用变量：".ToLang());
        var data = ServiceFactory.FieldService.Query("SELECT field.FieldName,form.FieldAlias FROM Whir_Dev_Field field INNER JOIN Whir_Dev_Form form ON field.FieldID=form.FieldID WHERE field.ModelID=1 AND field.IsHidden=0").Tables[0];
        for (int i = 0; i < data.Rows.Count; i++)
        {
            builder.Append("<spen>  " + data.Rows[i]["FieldAlias"].ToStr() + " - </spen>");
            builder.Append("<a href=\"javascript:Insert('{" + data.Rows[i]["FieldName"].ToStr() + "}',1)\">{" + data.Rows[i]["FieldName"].ToStr() + "}，</a>");
        }
        var str = builder.ToStr().Substring(0, builder.Length - 5) + "</a> ";
        Div_Mark.InnerHtml = str;
    }

    /// <summary>
    /// 获取会员类别列表
    /// </summary>
    private void LoadMemberGroup()
    {
        IList<MemberGroup> list = ServiceFactory.MemberGroupService.GetList().ToList();
        OptionMemberGroup += "<option value=\"0\">" + "==请选择==".ToLang() + "</option>";
        foreach (var item in list)
        {
            OptionMemberGroup += "<option value=\"" + item.GroupId + "\">" + item.GroupName.ToLang() + "</option>";
        }
    }
}
