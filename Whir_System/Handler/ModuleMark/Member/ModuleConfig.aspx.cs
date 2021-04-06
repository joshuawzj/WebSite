using System;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config.Models;
using Whir.Config;
using Whir.Language;

public partial class Whir_System_Handler_ModuleMark_Member_ModuleConfig : SysHandlerPageBase
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
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("297"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }

        var type = typeof(MemberConfig);
        var workFlow = RequestUtil.Instance.GetFormString("WorkFlow").ToInt(0);
        var enableMemGroupImage = RequestUtil.Instance.GetFormString("EnableMemGroupImage").ToBoolean();
        var memberConfig = ConfigHelper.GetMemberConfig() ?? ModelFactory<MemberConfig>.Insten();
        try
        {
            memberConfig = GetPostObject(type, memberConfig) as MemberConfig;
            if (memberConfig != null)
            {
                memberConfig.Register = memberConfig.Register.Trim().Replace("\r\n", "<br/>");
                memberConfig.EnableMemGroupImage = enableMemGroupImage;
                memberConfig.Authentication = memberConfig.Authentication.Trim().Replace("\r\n", "<br/>");
                memberConfig.RetakePassword = memberConfig.RetakePassword.Trim().Replace("\r\n", "<br/>");
                XmlUtil.SerializerObject(ConfigHelper.GetAppConfigPath("MemberConfig.config"), type, memberConfig);

                //保存工作流配置
                Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(1);
                if (column != null)
                {
                    column.WorkFlow = workFlow;
                    ServiceFactory.ColumnService.Save(column);
                }
                ServiceFactory.OperateLogService.Save(
                    "修改模块，邮件认证内容【{0}】，会员注册协议【{1}】，密码找回邮件【{2}】".FormatWith(memberConfig.Authentication,
                        memberConfig.Register, memberConfig.RetakePassword));
            }
        }
        catch (Exception ex)
        {
            throw;
        }

        return new HandlerResult {Status = true, Message = "保存成功".ToLang() };
    }
}