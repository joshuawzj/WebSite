/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：submitform_template.aspx.cs
 * 文件描述：提交表单自定义模版页面
 */
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_Module_Developer_SubmitForm_Template : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 表单ID
    /// </summary>
    protected int SubmitId { get; set; }

    protected string BackUrl { get; set; }

    protected SubmitForm CurrentSubmitForm { get; set; }

    protected List<ListItem> ChildColumn { get; set; }

    protected string Column { get; set; }
    protected IList<Column> Columnlist { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        SubmitId = RequestUtil.Instance.GetQueryInt("submitid", 0);
        BackUrl = RequestUtil.Instance.GetQueryString("backurl");
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            BindInfo();
        }
    }

    /// <summary>
    /// 绑定信息
    /// </summary>
    private void BindInfo()
    {
        CurrentSubmitForm = ServiceFactory.SubmitFormService.SingleOrDefault<SubmitForm>(SubmitId) ??
                            ModelFactory<SubmitForm>.Insten();
        ChildColumn = new List<ListItem>();
        if (CurrentSubmitForm != null)
        {
            lblName.Text = CurrentSubmitForm.Name;
            spanRemark.InnerText = "<wtl:webform formid=\"{0}\"></wtl:webform>".FormatWith(SubmitId);
            InvokeCode.InnerText = CurrentSubmitForm.InvokeCode;
            if (CurrentSubmitForm.ColumnId < 0)
            {
                ChildColumn.Add(new ListItem("==请选择==".ToLang(), ""));
                ChildColumn.Add(new ListItem("会员登录".ToLang(), "-1"));
                ChildColumn.Add(new ListItem("会员注册".ToLang(), "-2"));
                ChildColumn.Add(new ListItem("更改密码".ToLang(), "-3"));
                ChildColumn.Add(new ListItem("个人资料更改".ToLang(), "-4"));
                ChildColumn.Add(new ListItem("是否登录".ToLang(), "-5"));
                ChildColumn.Add(new ListItem("找回密码".ToLang(), "-6"));
                Column = "会员系统";
            }
            else if (CurrentSubmitForm.ColumnId == 0)
            {
                ChildColumn.Add(new ListItem("==请选择==".ToLang(), ""));
            }
            else
            {
                var m = ServiceFactory.ColumnService.SingleOrDefault<Column>(CurrentSubmitForm.ColumnId);
                Column = m == null ? "" : m.ColumnName;

                IList<Column> list = ServiceFactory.SubmitFormService.GetColumnsByMarkParentID(CurrentSubmitForm.ColumnId);
                Column colNull = new Column();
                colNull.ColumnId = -1;
                colNull.ColumnName = "==请选择==".ToLang();
                list.Insert(0, colNull);
                Columnlist= list;
            }
        }
    }

   
}