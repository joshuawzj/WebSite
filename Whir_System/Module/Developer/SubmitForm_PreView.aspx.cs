/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：submitform_preview.aspx.cs
 * 文件描述：提交表单预览页面
 */
using System;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;
using Whir.Language;

public partial class Whir_System_Module_Developer_SubmitForm_Preiew : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 提交表单ID
    /// </summary>
    protected int SubmitId { get; set; }
    /// <summary>
    /// 地区JS名称
    /// </summary>
    protected string JsName { get; set; }

    public bool isTemplatePreview { get; set; } 

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        LanguageType language = LanguageHelper.GetCurrentUseLanguage();
        switch (language)
        {
            case LanguageType.英文: JsName = "AreaData_min_en.js";
                break;
            default: JsName = "AreaData_min_cn.js";
                break;
        }
        SubmitId = RequestUtil.Instance.GetQueryInt("submitid", 0);
          isTemplatePreview = RequestUtil.Instance.GetQueryInt("istemplatepreview", -1).ToBoolean();
        if (isTemplatePreview)
        {
             //如果为true 则在前台执行代码
        }
        else
        {
            SubmitForm submitForm = ServiceFactory.SubmitFormService.SingleOrDefault<SubmitForm>(SubmitId);
            if (submitForm == null || string.IsNullOrEmpty(submitForm.InvokeCode))
            {
                litData.Text = "无数据".ToLang();
            }
            else
            {
                litData.Text = submitForm.InvokeCode;
            }
        }
    }
}