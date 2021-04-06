/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：workflow_edit .aspx.cs
* 文件描述：工作流编辑页。 
*/
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;

using Whir.Framework;
using Whir.Service;
using Whir.Domain;
using Whir.Language;



public partial class Whir_System_Module_Extension_Workflow_Edit : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当前编辑的工作流ID
    /// </summary>
    protected int WorkFlowId { get; set; }

    #endregion
    protected WorkFlow CurrentWorkFlow { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        WorkFlowId = RequestUtil.Instance.GetQueryInt("workflowid", 0);

        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("342") || IsCurrentRoleMenuRes("343"));
            BindDate();
        }
    }

    /// <summary>
    /// 绑定信息
    /// </summary>
    private void BindDate()
    {
        CurrentWorkFlow = ServiceFactory.WorkFlowService.SingleOrDefault<WorkFlow>(WorkFlowId)
            ?? ModelFactory<WorkFlow>.Insten();
    }

}