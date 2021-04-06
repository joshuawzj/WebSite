using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mail;
using Whir.Config;
using Whir.ezEIP.Web;

/// <summary>
/// EmailHelper 的摘要说明
/// </summary>
public class EmailHelper
{
    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="toEmail"></param>
    /// <returns></returns>
    public static bool Send(string toEmail,string title,string content)
    {
        try
        {
            MatchCollection matchs = Regex.Matches(content, "<img[^>]*src=(\'|\")(.*?)\\1[^>]*>");
            foreach (Match item in matchs)
            {
                string imapath = item.Groups[2].Value;
                if (!imapath.StartsWith("http"))
                {
                    content = content.Replace(imapath, "https://www.guangyancaijing.com" + imapath);
                }
            }
            MatchCollection matchs2 = Regex.Matches(content, "<a[^>]*href=(\'|\")(.*?)\\1[^>]*>");
            foreach (Match item in matchs2)
            {
                string href = item.Groups[2].Value;
                if (!href.StartsWith("http"))
                {
                    content = content.Replace(href, "https://www.guangyancaijing.com" + href);
                }
            }
            SysManagePageBase SysManagePageBase = new SysManagePageBase();
    var handlerResult = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("350"));
            if (handlerResult.Status)
            {
                return false;
            }
            var model = ConfigHelper.GetEmailConfig();
            
            var msg = new MailMessage
            {
                From = "光盐财经<" + model.Email + ">",
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
            System.Web.Mail.SmtpMail.SmtpServer = model.SMTP;
            System.Web.Mail.SmtpMail.Send(msg);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
   
}