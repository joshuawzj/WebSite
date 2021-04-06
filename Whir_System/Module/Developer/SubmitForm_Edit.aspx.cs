/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：SubmitForm_edit.cs
 * 文件描述：添加、编辑页
 */
using System;

using Whir.Domain;
using Whir.Service;
using Whir.Framework;

public partial class Whir_System_Module_Developer_SubmitForm_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 表单ID
    /// </summary>
    protected int SubmitId { get; set; }

    public SubmitForm CurrentSubmitForm { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        SubmitId = RequestUtil.Instance.GetQueryInt("submitid", 0);
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser || IsSuperUser);
            BandInfo();
            if (SubmitId > 0)//编辑
            {
                PageMode = EnumPageMode.Update;
            }
            
        }    }

    /// <summary>
    /// 绑定编辑信息
    /// </summary>
    protected void BandInfo()
    {
        CurrentSubmitForm = ServiceFactory.SubmitFormService.SingleOrDefault<SubmitForm>(SubmitId) ?? ModelFactory<SubmitForm>.Insten();
        
    }
}