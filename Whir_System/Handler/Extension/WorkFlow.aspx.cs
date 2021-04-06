using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class Whir_System_Handler_Extension_WorkFlow : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        int flowId = RequestUtil.Instance.GetFormString("WorkFlowId").ToInt();
        var handlerResult = new HandlerResult();
        if (flowId > 0)
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("343"));
        }
        else
        {
            SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("342"));
        }
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //反射获取表单字段数据
        var type = typeof(WorkFlow);
        var workFlow = ServiceFactory.SiteInfoService.SingleOrDefault<WorkFlow>(flowId) ?? ModelFactory<WorkFlow>.Insten();
        try
        {
            workFlow = GetPostObject(type, workFlow) as WorkFlow;
            ServiceFactory.SiteInfoService.Save(workFlow);
        }
        catch (Exception ex)
        {
            throw;
        }
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Del()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("344"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int flowId = RequestUtil.Instance.GetFormString("workflowid").ToInt();
        var workflow = ServiceFactory.SiteInfoService.SingleOrDefault<WorkFlow>(flowId);
        if (workflow == null)
            return new HandlerResult { Status = false, Message = "要删除的菜单数据不存在".ToLang() };

        ServiceFactory.SiteInfoService.Delete(workflow);
        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }
}