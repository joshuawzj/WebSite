/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：workflowlist.aspx.cs
 * 文件描述：工作流列表页面
 *
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Service;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;

public partial class Whir_System_Module_Extension_WorkflowList : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 是否具有添加工作流权限
    /// </summary>
    protected bool IsAdd { get; set; }

    /// <summary>
    /// 是否具有设置流程节点权限
    /// </summary>
    protected bool IsSetWorkflowNode { get; set; }

    /// <summary>
    /// 是否具有编辑权限
    /// </summary>
    protected bool IsEdit { get; set; }

    /// <summary>
    /// 是否具有删除权限
    /// </summary>
    protected bool IsDelete { get; set; }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
      
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("34"));
            BindList();
        }
    }

    #region 绑定
    //绑定列表
    private void BindList()
    {
        var list = ServiceFactory.WorkFlowService.Query<WorkFlow>(" WHERE IsDel=0 ORDER BY CreateDate DESC,sort ASC ");
        rptList.DataSource = list;
        rptList.DataBind();

        ltNoRecord.Text = list.Any() ? "" : "找不到记录".ToLang();
    }

    #endregion

    #region 事件
    //行事件
    protected void rptList_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string commandArgs = e.CommandArgument.ToStr();
        if (!IsCurrentRoleMenuRes("344"))
        {
            string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
        }
        else
        {
            //删除
            if (e.CommandName.Equals("del"))
            {
                if (ServiceFactory.WorkFlowService.DeleteWorkFlow(commandArgs.ToInt()) == 0)
                {
                    ErrorAlert("删除失败".ToLang());
                }
                else
                {
                    #region 删除工作流节点
                    var list = ServiceFactory.AuditActivityService.GetList(commandArgs.ToInt());
                    foreach (AuditActivity auditActivity in list)
                    {
                        ServiceFactory.AuditActivityService.DeleteAuditActivity(auditActivity.ActivityId.ToInt());
                    }
                    #endregion

                    var model = ServiceFactory.WorkFlowService.SingleOrDefault<WorkFlow>(commandArgs.ToInt());
                    if (null != model)
                        //记录操作日志
                        ServiceFactory.OperateLogService.Save("删除工作流【{0}】".FormatWith(model.WorkFlowName));
                }
            }
            BindList();
        }
    }

    //行绑定时
    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton lbtnDel = e.Item.FindControl("lbtnDel") as LinkButton;

            if (lbtnDel != null)
            {
                ConfirmDelete(lbtnDel);
            }
        }
    }
    #endregion
}