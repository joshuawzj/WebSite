/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：option_select.aspx.cs
 * 文件描述：选项选择页面
 */

using System;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;

public partial class whir_system_ModuleMark_common_option_select : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 回传给父页面的JS函数名
    /// </summary>
    public string CallBack { get; set; }

    /// <summary>
    /// 排除掉的分类ID, 也排除此分类下的子分类
    /// </summary>
    public int ExceptId { get; set; }

    /// <summary>
    /// 当前的表单ID
    /// </summary>
    public int FormId { get; set; }

    /// <summary>
    /// 所属的子站ID
    /// </summary>
    public int SubjectId { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        CallBack = RequestUtil.Instance.GetQueryString("callback");
        ExceptId = RequestUtil.Instance.GetQueryInt("exceptid", 0);
        FormId = RequestUtil.Instance.GetQueryInt("formid", 0);
        SubjectId = RequestUtil.Instance.GetQueryInt("SubjectId", -99999);
    }
}