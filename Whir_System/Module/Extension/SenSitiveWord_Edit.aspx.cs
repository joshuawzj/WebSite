/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：sensitiveword_edit.aspx.cs
 * 文件描述：敏感词操作（新增和编辑）页面
 *
 *          1.编辑敏感词和新增敏感词
 */

using System;

using Whir.Framework;
using Whir.Service;
using Whir.Domain;

public partial class Whir_System_Module_Extension_SensitiveWord_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 当前编辑的敏感词ID
    /// </summary>
    protected int SensitiveWordId { get; set; }

    protected SensitiveWord CurrentSensitiveWord { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        SensitiveWordId = RequestUtil.Instance.GetQueryInt("sensitivewordid", 0);
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("346") || IsCurrentRoleMenuRes("347"));
            BindData();
        }
    }

    /// <summary>
    /// 绑定当前编辑的敏感词
    /// </summary>
    private void BindData()
    {
        CurrentSensitiveWord = ServiceFactory.SensitiveWordService.SingleOrDefault<SensitiveWord>(SensitiveWordId) ?? ModelFactory<SensitiveWord>.Insten();
    }
}