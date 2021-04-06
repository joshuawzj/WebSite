using System;
using System.Collections.Generic;
using System.Linq;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config;
using Whir.Language;
using System.Text;


public partial class Whir_System_Handler_Config_SendEmailRecord : SysHandlerPageBase
{
    string _cmd = RequestUtil.Instance.GetFormString("cmd");
    string _title = RequestUtil.Instance.GetFormString("Title");
    string _emailContent = RequestUtil.Instance.GetFormString("content");
    int _recordId = RequestUtil.Instance.GetFormString("recordId").ToInt(0);
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
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("366"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        if (_emailContent.Trim().IsEmpty())
        {
            return new HandlerResult { Status = false, Message = "邮件内容不能为空".ToLang() };
        }
        int successNum = 0, failNum = 0;//记录发送成功、失败数
        switch (_cmd)
        {
            case "resendall":
                ServiceFactory.SendEmailRecordService.ResendAllEamil(_recordId, _title, _emailContent, out successNum, out failNum);
                break;
            case "resendfail":
                ServiceFactory.SendEmailRecordService.ResendFailEamil(_recordId, _title, _emailContent, out successNum, out failNum);
                break;
        }
        return new HandlerResult { Status = true, Message = "已经发送，成功{0}条，失败{1}条".ToLang().FormatWith(successNum, failNum)};
    }
}