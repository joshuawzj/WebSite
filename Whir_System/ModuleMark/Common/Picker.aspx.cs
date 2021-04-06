/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：image_select.aspx.cs
 * 文件描述：上传图片选择页面
 */

using System;
using Whir.Config;
using Whir.Config.Models;
using Whir.Framework;
using Whir.Domain;
using Whir.Service;

public partial class whir_system_ModuleMark_common_Picker : Whir.ezEIP.Web.SysManagePageBase
{

    /// <summary>
    /// 是否多选
    /// </summary>
    protected string IsMultiple { get; set; }

    /// <summary>
    /// 存放已选择项的控件Id
    /// </summary>
    protected string HidChooseId { get; set; }

    /// <summary>
    /// 上传控件Id
    /// </summary>
    protected string ControlId { get; set; }

    /// <summary>
    /// 允许的格式
    /// </summary>
    protected string FileExts { get; set; }

    /// <summary>
    /// 是否图片 1-图片，0-文件
    /// </summary>
    protected int IsPic { get; set; }

    /// <summary>
    /// 当前控件是哪个form弹出
    /// </summary>
    protected string FormId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        FormId = RequestString("formid").IsEmpty() ? "formEdit" : RequestString("formid");
        IsMultiple = "checkbox";
        IsMultiple = RequestString("isMultiple").IsEmpty() ? IsMultiple : RequestString("isMultiple");
        HidChooseId = RequestString("HidChooseId");
        IsPic = RequestString("IsPic").ToInt();
        ControlId = RequestString("ControlId");
        var formId = ControlId.Replace("file", "").Replace("image", "");
        FormUpload formUpload = Whir.Service.ServiceFactory.FormUploadService.GetFormUploadByFormId(formId.ToInt());
        if (formUpload != null)
            FileExts = formUpload.FileExts;

    }

}