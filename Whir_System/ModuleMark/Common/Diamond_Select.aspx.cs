/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：diamond_select.cs
 * 文件描述：类别选项选择器
 */
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;

public partial class whir_system_ModuleMark_common_diamond_select : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 是否多选 默认否 单选
    /// </summary>
    protected bool IsMultiple { get; set; }
    /// <summary>
    ///  回调js函数
    /// </summary>
    public string CallBack { get; set; }

    /// <summary>
    /// 表单ID
    /// </summary>
    public int FormId { get; set; }
    /// <summary>
    /// 字段ID
    /// </summary>
    public int FieldId { get; set; }
    /// <summary>
    /// 选项ID
    /// </summary>
    public string FormOptionId { get; set; }

    /// <summary>
    /// 子站ID
    /// </summary>
    public int SubjectId { get; set; }

    #endregion 属性

    protected void Page_Load(object sender, EventArgs e)
    {
        FormId = RequestUtil.Instance.GetQueryInt("formid", 0);
        FieldId = RequestUtil.Instance.GetQueryInt("fieldid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("subjectid", 0);
        FormOptionId = RequestUtil.Instance.GetQueryString("value");
        CallBack = RequestUtil.Instance.GetQueryString("CallBack");

        if (!IsPostBack)
        {
            FormOption formOption = ServiceFactory.FormOptionService.GetFormOptionByFormID(FormId);
            if (formOption == null)
                IsMultiple = false;
            else
            {
                if (formOption.SelectedType == 3 || formOption.SelectedType == 6)
                    IsMultiple = true;
                else
                    IsMultiple = false;
            }
        }
    }


}