/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：option_edit.aspx.cs
 * 文件描述：站点栏目的表单输入项编辑页面所弹出的选项编辑页面, 用于编辑绑定文本的选项字段
 *          
 */

using System;
using System.Web.UI.WebControls;

using Whir.Framework;

public partial class Whir_System_Module_Column_Option_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 点击确定后的回调JS函数
    /// </summary>
    protected string JsCallback { get;set;}

    /// <summary>
    /// 要编辑的值
    /// </summary>
    protected string OptionValue { get; set; }

    /// <summary>
    /// 要编辑的文本
    /// </summary>
    protected string OptionText { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgeOpenPagePermission(IsDevUser);
        JsCallback = RequestUtil.Instance.GetQueryString("JsCallback");
        OptionValue = RequestUtil.Instance.GetQueryString("option_value");
        OptionText = RequestUtil.Instance.GetQueryString("option_text");
    }
}