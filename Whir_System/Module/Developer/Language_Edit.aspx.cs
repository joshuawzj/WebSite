/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：language_edit.aspx.cs
 * 文件描述：多语言添加和编辑页
 */

using System;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class whir_system_module_developer_language_edit : Whir.ezEIP.Web.SysManagePageBase
{
    protected string CN { get; set; }
    protected Language Language { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        CN = RequestUtil.Instance.GetQueryString("CN");
        Language = LanguageHelper.GetLanguage(CN);
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            if (!CN.IsEmpty())
            {
                Language = LanguageHelper.GetLanguage(CN);
            }
            else
                Language = new Language();
        }
    }
 
}