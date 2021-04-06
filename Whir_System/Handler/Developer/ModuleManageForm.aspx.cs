using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Whir.Service;
using Whir.Framework;
using Whir.Domain;
using Whir.Language;
using Whir.ezEIP.Web;
public partial class Whir_System_Handler_Developer_ModuleManageForm : SysHandlerPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Del()
    {
        int submitId = RequestUtil.Instance.GetFormString("submitId").ToInt();
        Plugin Model = ServiceFactory.PluginService.SingleOrDefault<Plugin>(submitId);
        if (Model != null)
        {
            string Msg = ServiceFactory.PluginService.DeletePlugin(Model);
            if (Msg.Length > 0)
            {
                return new HandlerResult { Status = false, Message = "删除失败".ToLang() };
            }
            else
            {
                ServiceFactory.MenuService.Delete(Model);
                //操作日志
                ServiceFactory.OperateLogService.Save("删除模块，名称【{0}】".FormatWith(Model.ModuleName));
            }
        }
        return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
    }

    /// <summary>
    /// 安装
    /// </summary>
    /// <returns></returns>
    public HandlerResult Install()
    {
        int submitId = RequestUtil.Instance.GetFormString("submitId").ToInt();
        Plugin Model = ServiceFactory.PluginService.SingleOrDefault<Plugin>(submitId);
        if (Model != null)
        {
            string Msg = ServiceFactory.PluginService.InstallPlugin(Model);
            if (Msg.Length > 0)
            {
                return new HandlerResult { Status = false, Message = Msg };
            }
            else
            {
                Model.IsInstall = true;
                Model.IsHadInstall = true;
                ServiceFactory.MenuService.Update(Model);
            }
        }
        return new HandlerResult { Status = true, Message = "安装成功".ToLang() };
    }
    /// <summary>
    /// 卸载
    /// </summary>
    public HandlerResult Uninstall()
    {
        int submitId = RequestUtil.Instance.GetFormString("submitId").ToInt();
        Plugin Model = ServiceFactory.PluginService.SingleOrDefault<Plugin>(submitId);
        if (Model != null)
        {
            string Msg = ServiceFactory.PluginService.UnInstallPlugin(Model);
            if (Msg.Length > 0)
            {
                return new HandlerResult { Status = false, Message = Msg };
            }
            else
            {
                Model.IsInstall = false;
                ServiceFactory.MenuService.Update(Model);
            }
        }
        return new HandlerResult { Status = true, Message = "卸载成功".ToLang() };
    }

    /// <summary>
    /// 获取列表数据
    /// </summary>
    public void GetList()
    {
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 10) / pageSize + 1;
        int total = 0;
        var list = ServiceFactory.PluginService.Page(pageIndex, pageSize, " WHERE IsDel=0 ORDER BY sort ASC ");
        string data = list.Items.ToJson();
        total = list.TotalItems.ToInt();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);
    }
}