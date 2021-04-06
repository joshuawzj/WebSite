using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Security.Domain;
using Whir.Service;

public partial class whir_system_Handler_Developer_QuickMenu : SysHandlerPageBase
{
    SysManagePageBase SysManagePageBase = new SysManagePageBase();
    

    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult GetList()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("379"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        var list = ServiceFactory.QuickMenuService.GetList();

        string data = list.ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", list.Count(), data);
        Response.Clear();
        Response.Write(json);
        Response.End();

        return new HandlerResult { Status = true, Message = ""};
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("379"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int menuId = RequestUtil.Instance.GetFormString("MenuId").ToInt();

        //反射获取表单字段数据
        var type = typeof(QuickMenu);
        var menu = ServiceFactory.QuickMenuService.SingleOrDefault<QuickMenu>(menuId) ?? ModelFactory<QuickMenu>.Insten();
        menu = GetPostObject(type, menu) as QuickMenu;

        ServiceFactory.QuickMenuService.Save(menu);
       
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 删除数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Del()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("379"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int menuId = RequestUtil.Instance.GetFormString("MenuId").ToInt();
        var menu = ServiceFactory.QuickMenuService.SingleOrDefault<QuickMenu>(menuId);
        if (menu == null)
            return new HandlerResult { Status = false, Message = "该记录已不存在".ToLang() };
         
        ServiceFactory.QuickMenuService.Delete(menu);

       

        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }

    /// <summary>
    /// 删除多条数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult DelAll()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("379"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var menuIds = RequestUtil.Instance.GetFormString("MenuIds");

        ServiceFactory.QuickMenuService.Delete("Where Id in (@0)", menuIds.Split(',').Select(p=>p.ToInt()));

        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }

}