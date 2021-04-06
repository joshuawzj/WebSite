/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：form_edit.aspx.cs
 * 文件描述：站点栏目的表单输入项编辑页面
 *          
 */

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using Whir.Config.Models;
using Whir.Config;

public partial class Whir_System_Module_Column_Form_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    #region URL参数

    /// <summary>
    /// 当前的主栏目ID
    /// </summary>
    protected int ColumnId { get; set; }

    protected Column CurrentColumn { get; set; }

    /// <summary>
    /// 当前编辑的表单输入项ID
    /// </summary>
    protected int FormId { get; set; }

    /// <summary>
    /// 左侧列表选中要使用的字段ID,并正在编辑的此字段为表单输入项
    /// </summary>
    protected int FieldId { get; set; }

    //语言
    public string Lang { get; set; }

    public UploadConfig UploadConfig { get; set; }

    public Form Forms { get; set; }
    public Field Field { get; set; }
    public FormOption FormOption { get; set; }
    public FormDate FormDate { get; set; }
    public FormUpload FormUpload { get; set; }
    public FormUpload FileFormUpload { get; set; }
    public FormArea FormArea { get; set; }

    #endregion URL参数

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        ColumnId = RequestUtil.Instance.GetQueryInt("columnid", 0);
        FormId = RequestUtil.Instance.GetQueryInt("formid", 0);
        FieldId = RequestUtil.Instance.GetQueryInt("fieldid", 0);
        UploadConfig = ConfigHelper.GetUploadConfig();

        if (!IsPostBack)
        {
            if (ColumnId != 0)
            {
                BindFieldList();
            }

            if (FormId != 0)
            {
                //编辑表单状态
                phFields.Visible = false; //编辑表单时, 不显示已有字段

            }
        }
        BindFormInfo();
        #region 获取语言，时间格式
        LanguageType language = LanguageHelper.GetCurrentUseLanguage();
        switch (language)
        {
            case LanguageType.英文: Lang = "en";
                break;
            case LanguageType.繁体中文: Lang = "zh-tw";
                break;
            default:
                Lang = "zh-cn";
                break;
        }
        #endregion
    }

    #region 左侧已有字段列表

    //绑定未使用字段
    private void BindFieldList()
    {
        CurrentColumn = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId) ?? ModelFactory<Column>.Insten();

        IList<Field> listField = ServiceFactory.FieldService.GetUnusedByColumnID(ColumnId);
        if (listField.Count > 0)
        {
            rptFieldList.ItemDataBound += new RepeaterItemEventHandler(FieldList_ItemDataBound);
            rptFieldList.DataSource = listField;
            rptFieldList.DataBind();
        }
        else
        {
            phNoField.Visible = true;
        }
    }

    /// <summary>
    /// 字段列表行绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void FieldList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            Field field = e.Item.DataItem as Field;
            if (field == null) return;

            Literal litFieldType = e.Item.FindControl("litFieldType") as Literal;

            if (litFieldType != null)
            {
                litFieldType.Text = ServiceFactory.FieldService.GetFieldTypeName(field.FieldType).ToLang();
            }
        }
    }

    #endregion 左侧已有字段列表


    //绑定正在编辑的表单输入项
    private void BindFormInfo()
    {
        Forms = ServiceFactory.FormService.SingleOrDefault<Form>(FormId) ?? ModelFactory<Form>.Insten();
        Field = ServiceFactory.FieldService.GetFieldByFormId(FormId) ?? ModelFactory<Field>.Insten();

        FormOption = ServiceFactory.FormOptionService.GetFormOptionByFormID(Forms.FormId) ?? ModelFactory<FormOption>.Insten();
        FormDate = ServiceFactory.FormDateService.GetFormDateByFormID(Forms.FormId) ?? ModelFactory<FormDate>.Insten();
        FormUpload = ServiceFactory.FormUploadService.GetFormUploadByFormId(Forms.FormId) ?? ModelFactory<FormUpload>.Insten();
        FileFormUpload = ServiceFactory.FormUploadService.GetFormUploadByFormId(Forms.FormId) ?? ModelFactory<FormUpload>.Insten();

        FormArea = ServiceFactory.FormAreaService.GetFormAreaByFormID(Forms.FormId) ?? ModelFactory<FormArea>.Insten();
    }
}