/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：EmailConfig.aspx.cs
 * 文件描述：邮箱配置页面
 */
using System;
using System.Web.Mail;

using Whir.Config;
using Whir.Framework;
using Whir.Config.Models;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_Module_Config_EmailConfig : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 保存密码
    /// </summary>
    public string EmailPwD
    {
        get
        {
            if (ViewState["EmailPwD"] != null)
                return ViewState["EmailPwD"].ToString();
            else
                return "";
        }
        set { ViewState["EmailPwD"] = value; }
    }

    protected EmailConfig EmailConfig { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("22"));
            EmailConfig = ConfigHelper.GetEmailConfig();
        }
    }
}
