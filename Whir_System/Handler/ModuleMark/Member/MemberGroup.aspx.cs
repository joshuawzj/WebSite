using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Service;

public partial class Whir_System_Handler_ModuleMark_Member_MemberGroup : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        int groupId = RequestUtil.Instance.GetFormString("GroupId").ToInt();
        string groupName = RequestUtil.Instance.GetFormString("GroupName");
        var handlerResult = new HandlerResult();
        if (groupId > 0)
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("303"));
        }
        else
        {
            handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("299"));
        }
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        //验证是否重名
        if (ServiceFactory.MemberGroupService.IsExist(groupName, groupId > 0 ? true : false))
        {
            return new HandlerResult { Status = false, Message = "会员组名称已存在".ToLang() };

        }
        //反射获取表单字段数据
        var type = typeof(MemberGroup);
        var memberGroup = ServiceFactory.SiteInfoService.SingleOrDefault<MemberGroup>(groupId) ?? ModelFactory<MemberGroup>.Insten();
        try
        {
            memberGroup = GetPostObject(type, memberGroup) as MemberGroup;
            ServiceFactory.SiteInfoService.Save(memberGroup);
            //添加操作日志记录
            if (groupId > 0)
            {
                ServiceFactory.MemberGroupService.SaveLog(memberGroup, "update");
            }
            else
            {
                ServiceFactory.MemberGroupService.SaveLog(memberGroup, "insert");
            }
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
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("304"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int memberGroupId = RequestUtil.Instance.GetFormString("memberGroupId").ToInt();
        var memberGroup = ServiceFactory.MemberGroupService.SingleOrDefault<MemberGroup>(memberGroupId);
        if (memberGroup == null)
            return new HandlerResult { Status = false, Message = "要删除的数据不存在".ToLang() };

        ServiceFactory.MemberGroupService.Delete(memberGroup);
        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult DelAll()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("304"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        string ids = RequestUtil.Instance.GetFormString("selected").ToStr().Trim();
        foreach (var id in ids.Split(','))
        {
            int result = ServiceFactory.MemberGroupService.Delete<MemberGroup>(id.ToInt());
            if (result > 0)
                //添加操作日志
                ServiceFactory.MemberGroupService.SaveLog(id.ToInt(), "delete");
        }

        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }


    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("11"), true);

        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        var pageData = ServiceFactory.MemberGroupService.Page(pageIndex, pageSize, "Where IsDel=0 Order By Sort Desc, CreateDate Desc");

        long total = pageData.TotalItems;
        string data = pageData.Items.ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }

}