/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：auditactivitylist.aspx.cs
 * 文件描述：工作流流程节点列表页面
 *
 */
using System;
using System.Linq;
using System.Web.UI.WebControls;

using Whir.Service;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;

public partial class Whir_System_Module_Extension_AuditActivityList : Whir.ezEIP.Web.SysManagePageBase
{
    #region 属性
    /// <summary>
    /// 当所属工作流ID
    /// </summary>
    protected int WorkFlowId { get; set; }

    /// <summary>
    /// 是否具有添加流程节点权限
    /// </summary>
    protected bool IsAdd { get; set; }

    /// <summary>
    /// 是否具有编辑权限
    /// </summary>
    protected bool IsEdit { get; set; }

    /// <summary>
    /// 是否具有删除权限
    /// </summary>
    protected bool IsDelete { get; set; }

    public WorkFlow CurrentWorkFlow { get; set; }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        WorkFlowId = RequestUtil.Instance.GetQueryInt("workflowid", 0);

        //设置流程节点（添加流程节点、修改、删除）
        IsAdd = IsCurrentRoleMenuRes("342");
        IsEdit = IsCurrentRoleMenuRes("343");
        IsDelete = IsCurrentRoleMenuRes("344");

        CurrentWorkFlow = ServiceFactory.WorkFlowService.SingleOrDefault<WorkFlow>(WorkFlowId) ?? ModelFactory<WorkFlow>.Insten();
        
        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("345"));
            BindList();
        }
    }

    #region 绑定
    //绑定列表
    private void BindList()
    {
        var list = ServiceFactory.AuditActivityService.GetList(WorkFlowId);
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
                var model = ServiceFactory.AuditActivityService.SingleOrDefault<AuditActivity>(commandArgs.ToInt());
                if (ServiceFactory.AuditActivityService.DeleteAuditActivity(commandArgs.ToInt()) == 0)
                {
                    string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.error('删除失败', true, false)</script>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);

                }
                else
                {
                    ServiceFactory.OperateLogService.Save("删除节点【{0}】".FormatWith(model.ActivityName));
                    string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.success('删除成功', true, false)</script>";
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
                }
            }
            BindList();
        }
    }

    //行绑定时
    protected void rptList_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
        LinkButton lbtnDel = e.Item.FindControl("lbtnDel") as LinkButton;

        if (lbtnDel != null)
        {
            ConfirmDelete(lbtnDel);
        }
    }
    #endregion
}