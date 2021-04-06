using System;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config;
using Whir.Config.Models;
using Whir.Language;
using System.Text.RegularExpressions;


public partial class Whir_System_Handler_Config_SecurityConfig : SysHandlerPageBase
{
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
        //反射获取表单字段数据
        var type = typeof(SystemConfig);
        var model = ConfigHelper.GetSystemConfig() ?? ModelFactory<SystemConfig>.Insten();
        try
        {
            model = GetPostObject(type, model) as SystemConfig;
            //服务器端校验  
            if (model != null && !string.IsNullOrEmpty(model.DNS))  //如果配置中设置DNS为空，那么就没有校验
            {
                //http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?
                bool isOk = Regex.IsMatch(model.DNS, @"([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
                if (!isOk)
                {
                    return new HandlerResult { Status = false, Message = "URL格式错误，例如：".ToLang() + "www.whir.net" };
                }
            }

            //最低不能低于43200分钟
            if (model != null && model.TimeOut < 10)
            {
                return new HandlerResult { Status = false, Message = "后台登陆超时，最低不能低于10分钟".ToLang() };
            }

            if (model != null && model.TimeOut > 43200)
            {
                return new HandlerResult { Status = false, Message = "后台登陆超时，最高不能高于43200分钟".ToLang() };
            }

            XmlUtil.SerializerObject(ConfigHelper.GetAppConfigPath("SystemConfig.config"), type, model);
            //记录操作日志
            if (model != null)
            {
                //记录操作日志
                ServiceFactory.OperateLogService.Save(string.Format("修改，安全配置，后台访问域名" + "【{0}】".FormatWith(model.DNS)));
            }
            return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
        }
        catch (Exception ex)
        {
            return new HandlerResult { Status = false, Message = "保存失败".ToLang() };
        }
    }

    

}