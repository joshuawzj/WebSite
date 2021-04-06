using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Language;
using Whir.Security.Service;
public partial class Whir_System_Handler_Extension_AuditActivity : SysHandlerPageBase
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
        var activiTyId = RequestUtil.Instance.GetFormString("ActivityId").ToInt();
        var handlerResult = new HandlerResult();
        if (activiTyId > 0)
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
        var type = typeof(AuditActivity);
        var activity = ServiceFactory.SiteInfoService.SingleOrDefault<AuditActivity>(activiTyId) ?? ModelFactory<AuditActivity>.Insten();
        try
        {
            activity = GetPostObject(type, activity) as AuditActivity;
            if (activity.WorkflowId == 0)
                return new HandlerResult { Status = false, Message = "找不到相应的工作流".ToLang()};

            if (activity != null && (activity.PreActivityId == activity.NextActivityId && !(activity.PreActivityId == 0 && activity.NextActivityId == 0)))
            {
                return new HandlerResult { Status = false, Message = "上下节点不能为同一个节点".ToLang() };
            }
            if (activity != null && activity.ActivityId == 0)
            {
                if (IsExistActivityName(activity.ActivityName, 0, activity.WorkflowId)) //存在节点
                    return new HandlerResult { Status = false, Message = "该节点名称已存在".ToLang() };

                if (activity.PreActivityId != 0 &&
                    IsExistMultilActivity(activity.PreActivityId, true, activity.WorkflowId, 0))
                    return new HandlerResult { Status = false, Message = "上一节点已被占用".ToLang() };

                if (activity.NextActivityId != 0 && IsExistMultilActivity(activity.NextActivityId, false, activity.WorkflowId, 0))
                    return new HandlerResult { Status = false, Message = "下一节点已被占用".ToLang() };

                ServiceFactory.WorkFlowService.Save(activity);
                ChangeOther(activity); //同时变更其它关联节点
                ServiceFactory.OperateLogService.Save("添加工作流流程节点【{0}】".FormatWith(activity.ActivityName));

                return new HandlerResult { Status = true, Message = "保存成功".ToLang()};
            }
            else
            {
                if (activity != null && IsExistActivityName(activity.ActivityName, activity.ActivityId, activity.WorkflowId))
                    return new HandlerResult { Status = false, Message = "该节点名称已存在".ToLang() };

                if (activity != null && (activity.PreActivityId != 0 && IsExistMultilActivity(activity.PreActivityId, true, activity.WorkflowId, activity.ActivityId)))
                    return new HandlerResult { Status = false, Message = "上一节点已被占用".ToLang() };

                if (activity != null && (activity.NextActivityId != 0 && IsExistMultilActivity(activity.NextActivityId, false, activity.WorkflowId, activity.ActivityId)))
                    return new HandlerResult { Status = false, Message = "下一节点已被占用".ToLang() };

                //if (activity != null && activity.ResourceIds.Trim(',').Length > 0)
                //{
                //    activity.ResourceIds = "," + activity.ResourceIds.Trim(',') + ",";
                //}
                //else
                //{
                //    if (activity != null) activity.ResourceIds = ",1,2,";//开发者与超级管理员必须有所有权限
                //}

                ServiceFactory.WorkFlowService.Update(activity);
                ChangeOther(activity);//同时变更其它关联节点
                if (activity != null)
                    ServiceFactory.OperateLogService.Save("修改工作流流程节点名称【{0}】".FormatWith(activity.ActivityName));
                return new HandlerResult { Status = true, Message = "修改成功".ToLang()};
            }
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "修改失败".ToLang()};
        }
    }

    /// <summary>
    /// 更改关联节点
    /// </summary>
    /// <param name="model">当前节点</param>
    protected void ChangeOther(AuditActivity model)
    {
        if (model.PreActivityId != 0)
        {
            //把上一节点的下一节点更改为当前节点
            AuditActivity prevNode = ServiceFactory.AuditActivityService.SingleOrDefault<AuditActivity>(model.PreActivityId);
            if (prevNode != null)
            {
                prevNode.NextActivityId = model.ActivityId;
                ServiceFactory.WorkFlowService.Update(prevNode);
            }
        }
        if (model.NextActivityId != 0)
        {
            //把下一节点的上一节点更改为当前节点
            AuditActivity nextNode = ServiceFactory.AuditActivityService.SingleOrDefault<AuditActivity>(model.NextActivityId);
            if (nextNode != null)
            {
                nextNode.PreActivityId = model.ActivityId;
                ServiceFactory.WorkFlowService.Update(nextNode);
            }
        }
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
        int activiTyId = RequestUtil.Instance.GetFormString("ActivityId").ToInt();
        var auditActivity = ServiceFactory.SiteInfoService.SingleOrDefault<AuditActivity>(activiTyId);
        if (auditActivity == null)
            return new HandlerResult { Status = false, Message = "要删除的菜单数据不存在".ToLang()};

        ServiceFactory.SiteInfoService.Delete(auditActivity);
        return new HandlerResult { Status = true, Message = "删除成功".ToLang()};
    }


    /// <summary>
    /// 判断是否存在相同的节点名称
    /// </summary>
    /// <param name="activityName">节点名称</param>
    /// <param name="pId">节点父ID</param>
    /// <param name="workflowId">工作流ID</param>
    /// <returns></returns>
    protected bool IsExistActivityName(string activityName, int pId, int workflowId)
    {
        string sql = "SELECT COUNT(*) FROM Whir_Ext_AuditActivity WHERE ISDEL=0 AND ActivityName=@0 AND WorkflowId=@1";
        if (pId > 0)
        {
            sql += " AND ActivityId<>@2";
        }
        return ServiceFactory.WorkFlowService.ExecuteScalar<int>(sql, activityName, workflowId, pId) > 0;
    }

    /// <summary>
    /// 是否存在多个节点的上一节点或下一节点同时为同一节点
    /// </summary>
    /// <param name="activityId">节点ID</param>
    /// <param name="isPreActivityId">true：上节点，false：下一节点</param>
    /// <param name="workFlowId"></param>
    /// <param name="pId">主键</param>
    /// <returns></returns>
    protected bool IsExistMultilActivity(int activityId, bool isPreActivityId, int workFlowId, int pId)
    {
        string sql = "SELECT COUNT(*) FROM Whir_Ext_AuditActivity WHERE ISDEL=0 AND {0}=@0 AND WorkflowId=@1".FormatWith(isPreActivityId ? "PreActivityId" : "NextActivityId");
        if (pId > 0)
        {
            sql += " AND ActivityId<>@2";
        }

        int count = ServiceFactory.WorkFlowService.ExecuteScalar<int>(sql, activityId, workFlowId, pId);
        return count > 0;
    }
}