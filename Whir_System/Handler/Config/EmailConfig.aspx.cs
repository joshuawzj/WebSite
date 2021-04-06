using System;
using System.Linq;
using System.Web.Mail;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Service;
using Whir.Config;
using Whir.Config.Models;
using Whir.Language;


public partial class Whir_System_Handler_Config_EmailConfig : SysHandlerPageBase
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
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("340"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        //反射获取表单字段数据
        var type = typeof(EmailConfig);
        var model = ConfigHelper.GetEmailConfig() ?? ModelFactory<EmailConfig>.Insten();
        try
        {
            model = GetPostObject(type, model) as EmailConfig;
            XmlUtil.SerializerObject(ConfigHelper.GetAppConfigPath("EmailConfig.config"), type, model);
            //记录操作日志
            if (model != null)
            {
                ServiceFactory.OperateLogService.Save("修改邮箱配置，邮件服务器【{0}】，发件人邮箱【{1}】，用户名【{2}】".FormatWith(
                    model.SMTP,
                    model.Email,
                    model.UserName));
            }
        }
        catch (Exception ex)
        {
            throw;
        }
        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    /// <summary>
    /// 测试发送
    /// </summary>
    public HandlerResult Test()
    {
        var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("341"));
        if (handlerResult.Status)
        {
            return new HandlerResult { Status = false, Message = handlerResult.Message };
        }
        var msg = Send(RequestUtil.Instance.GetString("EmailCs"));

        return msg.ToBoolean()
            ? new HandlerResult { Status = true, Message = "发送成功".ToLang() }
            : new HandlerResult { Status = false, Message = msg };
    }


    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="toEmail"></param>
    /// <returns></returns>
    public string Send(string toEmail)
    {
        try
        {
            var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("350"));
            if (handlerResult.Status)
            {
                return handlerResult.Message;
            }
            var model = ConfigHelper.GetEmailConfig();
            var title = "ezEIPV5.0后台邮件，发送时间：" + DateTime.Now.ToString();
            var content = "这是一封由ezEIPV5.0后台发送的邮件，发送时间：" + DateTime.Now.ToString();
            var msg = new MailMessage
            {
                From = "zsf<"+model.Email+">",
                To = toEmail,
                BodyFormat = MailFormat.Html,
                BodyEncoding = System.Text.Encoding.UTF8,
                Subject = title,
                Body = content
            };
            msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", model.Email);//发信人的用户名
            msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", model.Password);//发信人的密码
            if (model.Port != "25")
            {
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", model.Port);
                msg.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
            }
            SmtpMail.SmtpServer = model.SMTP;
            SmtpMail.Send(msg);
            return "1";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

}