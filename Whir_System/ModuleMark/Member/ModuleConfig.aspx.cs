/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：moduleconfig.aspx.cs
 * 文件描述：会员模块配置
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using System.Linq;

using Whir.Config.Models;
using Whir.Config;
using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_ModuleMark_Member_ModuleConfig : Whir.ezEIP.Web.SysManagePageBase
{
    public MemberConfig CurrentMemberConfig { get; set; }
    public List<WorkFlow> WorkFlows { get; set; }
    public int CurrentWorkFlowId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("10"));
            LoadInfo();
            BindWorkFolw();
        }
    }

    /// <summary>
    /// 加载绑定信息
    /// </summary>
    private void LoadInfo()
    {
        CurrentMemberConfig = ConfigHelper.GetMemberConfig();//获取已经保存的值
        CurrentMemberConfig.Register = CurrentMemberConfig.Register.Replace("<br/>", "\r\n");
        CurrentMemberConfig.Authentication = CurrentMemberConfig.Authentication.Replace("<br/>", "\r\n");
        CurrentMemberConfig.RetakePassword = CurrentMemberConfig.RetakePassword.Replace("<br/>", "\r\n");
        StringBuilder builder = new StringBuilder("可用变量：".ToLang());
        IList<Field> listField = ServiceFactory.FieldService.Query<Field>("SELECT field.* FROM Whir_Dev_Field field INNER JOIN Whir_Dev_Form form ON field.FieldID=form.FieldID WHERE field.ModelID=1 AND field.IsHidden=0").ToList();
        foreach (var item in listField)
        {
            builder.Append("<spen>  " + item.FieldAlias + " - </spen>");
            builder.Append("<a href=\"javascript:Insert('{" + item.FieldName + "}',1)\">{" + item.FieldName + "}，</a>");
        }
        string text = "";
        if (builder.Length > "可用变量：".ToLang().Length)
        {
            text = builder.ToString().Substring(0, builder.Length - 5) + "</a>";
        }
        Div_Mark.InnerHtml = text;

    }

    /// <summary>
    /// 绑定工作流
    /// </summary>
    private void BindWorkFolw()
    {
        WorkFlows = ServiceFactory.WorkFlowService.GetList().ToList();
        Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(1);
        if (column != null)
        {
            CurrentWorkFlowId = column.WorkFlow;
        }
    }
}