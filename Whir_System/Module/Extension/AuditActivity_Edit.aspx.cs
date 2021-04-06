/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：workflow_edit .aspx.cs
* 文件描述：工作流编辑页。 
*/
using System;
using System.Web.UI.WebControls;

using Whir.Framework;
using Whir.Security.Service;
using Whir.Service;
using Whir.Domain;
using Whir.Language;
using System.Collections.Generic;


public partial class Whir_System_Module_Extension_WorkFlow_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前编辑的流程节点ID
    /// </summary>
    protected int ActivityId { get; set; }
    /// <summary>
    /// 当所属工作流ID
    /// </summary>
    protected int WorkFlowId { get; set; }

    /// <summary>
    /// 绑定角色字符串
    /// </summary>
    protected string CheckBoxRoles { get; set; }

    //当前流程
    protected AuditActivity CurrentAuditActivity { get; set; }

    //上一流程
    protected string PreActivityOption { get; set; }

    //下一流程
    protected string NextActivityOption { get; set; }

    public WorkFlow CurrentWorkFlow { get; set; }
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {
        ActivityId = RequestUtil.Instance.GetQueryInt("activityid", 0);
        WorkFlowId = RequestUtil.Instance.GetQueryInt("workflowid", 0);
        CurrentWorkFlow = ServiceFactory.WorkFlowService.SingleOrDefault<WorkFlow>(WorkFlowId) ?? ModelFactory<WorkFlow>.Insten();
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("345") && (IsCurrentRoleMenuRes("342") || IsCurrentRoleMenuRes("343")));
            BindDate();
            if (ActivityId != 0)
            {
                PageMode = EnumPageMode.Update;

            }
            //绑定上下节点,要放在BinDate后
            BindActivity();

        }
    }

    /// <summary>
    /// 绑定信息
    /// </summary>
    private void BindDate()
    {
        CurrentAuditActivity = ServiceFactory.AuditActivityService.SingleOrDefault<AuditActivity>(ActivityId) ?? ModelFactory<AuditActivity>.Insten();
        CurrentAuditActivity.WorkflowId = WorkFlowId;
    }
    
    /// <summary>
    /// 绑定上下节点
    /// </summary>
    private void BindActivity()
    {
        string option = "<option value=\"0\">" + "无".ToLang() + "</option>";
        var auditActivity =
            ServiceFactory.AuditActivityService.Query<AuditActivity>(
                "WHERE IsDel=0 AND WorkflowId=@0 AND ActivityId!=@1 ORDER BY sort ASC ", WorkFlowId, ActivityId);
        foreach (var item in auditActivity)
        {
            option += "<option value=\"" + item.ActivityId + "\">" + item.ActivityName + "</option>";
        }
        PreActivityOption = NextActivityOption = option;
    }


    /// <summary>
    /// 更改关联节点
    /// </summary>
    /// <param name="model">当前节点</param>
    private void ChangeOther(AuditActivity model)
    {
        if (model.PreActivityId != 0)
        {
            //把上一节点的下一节点更改为当前节点
            AuditActivity preModel = ServiceFactory.AuditActivityService.SingleOrDefault<AuditActivity>(model.PreActivityId);
            if (preModel != null)
            {
                preModel.NextActivityId = model.ActivityId;
                ServiceFactory.WorkFlowService.Update(preModel);
            }
        }
        if (model.NextActivityId != 0)
        {
            //把下一节点的上一节点更改为当前节点
            AuditActivity nextModel = ServiceFactory.AuditActivityService.SingleOrDefault<AuditActivity>(model.NextActivityId);
            if (nextModel != null)
            {
                nextModel.PreActivityId = model.ActivityId;
                ServiceFactory.WorkFlowService.Update(nextModel);
            }
        }
    }
}