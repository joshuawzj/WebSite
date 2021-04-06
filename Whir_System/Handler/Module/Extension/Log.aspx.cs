using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config;
using Whir.Language;
using System.Configuration;
using Whir.Service;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

public partial class Whir_System_Handler_Module_Extension_Log : SysHandlerPageBase
{
    protected readonly SysManagePageBase SysManagePageBase = new SysManagePageBase();
    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetString("_action");
        Exec(this, action);
    }
     

    /// <summary>
    /// 清空数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Clear()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("334"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int operateType = RequestUtil.Instance.GetFormString("logType").ToInt(0);
        DateTime startDate = RequestUtil.Instance.GetFormString("StartDate").ToDateTime();
        DateTime endDate = RequestUtil.Instance.GetFormString("EndDate").ToDateTime();
        ServiceFactory.OperateLogService.DeleteByType(operateType, startDate, endDate);
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <returns></returns>
    public HandlerResult Delete()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("334"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        int opreateId = RequestUtil.Instance.GetFormString("logId").ToInt(0);
        bool isSuccess = new OperateLogService().Delete(opreateId);
        if (isSuccess)
        {
            return new HandlerResult { Status = true, Message = "删除成功".ToLang() };
        }
        else
        {
            return new HandlerResult { Status = false, Message = "删除失败".ToLang() };
        }

    }
    /// <summary>
    /// 删除选择
    /// </summary>
    /// <returns></returns>
    public HandlerResult DeleteAll()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("334"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        string ids = RequestUtil.Instance.GetFormString("logIds");
        foreach (string id in ids.Split(','))
        {
            ServiceFactory.OperateLogService.Delete(id.ToInt());
        }
        return new HandlerResult { Status = true, Message = "操作成功".ToLang() };

    }

    /// <summary>
    /// 日志列表
    /// </summary>
    public void GetList()
    {
        SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("39"), true);
        int pageSize = Whir.ezEIP.BasePage.RequestInt32("limit", 10);
        int pageIndex = Whir.ezEIP.BasePage.RequestInt32("offset", 0) / pageSize + 1;
        int OperateType = RequestUtil.Instance.GetString("Type").ToInt(0);
         
        string sql = " Where Type =@0 ";
 
        int i = 1;
        var parms = new List<object>();
        parms.Add(OperateType);
        string filter = RequestUtil.Instance.GetQueryString("filter");
        Dictionary<string, string> searchDic = ToDictionary(filter);
        foreach (var kv in searchDic)
        {
            if (kv.Value.Contains("<&&<"))
            {
                sql += " and {0} between @{1} and @{2} ".FormatWith(kv.Key, i++, i++);
                parms.Add(Regex.Split(kv.Value, "<&&<")[0]);
                parms.Add(Regex.Split(kv.Value, "<&&<")[1]);
            }
            else
            {
                sql += " and {0} like '%'+@{1}+'%' ".FormatWith(kv.Key, i++);
                parms.Add(kv.Value);
            }
        }
        if (!SysManagePageBase.IsDevUser)
        {
            string devUserStr = Whir.Security.ServiceFactory.UsersService.GetUserStrByRoleId(1);//开发者
            if (devUserStr != "")
            {
                sql += " AND CreateUser NOT IN({0}) ".FormatWith(devUserStr);
            }
        }
         
        sql += " Order By Sort desc";
        var pageData = ServiceFactory.OperateLogService.Page(pageIndex, pageSize, sql, parms.ToArray());
        long total = pageData.TotalItems;
        string data = pageData.Items.ToJson();
        string json = string.Format("{{\"total\":{0},\"rows\":{1}}}", total, data);
        Response.Clear();
        Response.Write(json);

    }
    private Dictionary<string, string> ToDictionary(string str)
    {
        try
        {
            if (str.IsEmpty())
                return new Dictionary<string, string>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Deserialize<Dictionary<string, string>>(str);
        }
        catch
        {
            return new Dictionary<string, string>();
        }
    }

}