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
using System.Text.RegularExpressions;
using Whir.Repository;

public partial class Whir_System_Handler_Config_EmailMass : SysHandlerPageBase
{
    string _cmd = RequestUtil.Instance.GetFormString("cmd");
    string _title = RequestUtil.Instance.GetFormString("Title");
    string _emailContent = RequestUtil.Instance.GetFormString("content");
    string _sendType = RequestUtil.Instance.GetFormString("SendType");
    string _hidMembers = RequestUtil.Instance.GetFormString("hid_members");
    string _memberGroup = RequestUtil.Instance.GetFormString("MemberGroup");
    string _emails = RequestUtil.Instance.GetFormString("Emails");

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
        HandlerResult temp = new HandlerResult();
        if (_emailContent.Trim().IsEmpty())
        {
            return new HandlerResult {Status = false, Message = "邮件内容不能为空".ToLang()};
        }
        switch (_cmd)
        {
            case "later": //保存以后发送

                temp = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("349"));
                if (temp.Status)
                {
                    return new HandlerResult { Status = false, Message = temp.Message };
                }
                temp = SaveDb();
                break;
            case "now": //立即发送
                temp = SysManagePageBase.JudgeActionPermission(SysManagePageBase.IsCurrentRoleMenuRes("365"));
                if (temp.Status)
                {
                    return new HandlerResult { Status = false, Message = temp.Message };
                }
                temp = SendNow();
                break;
        }
        return temp;
    }

    ///// <summary>
    ///// 发送
    ///// </summary>
    protected HandlerResult SendNow()
    {
        int successNum = 0, failNum = 0;//发送成功、失败的数量

        string successIds = "", failIds = "";//发送成功的会员ID，发送失败的会员Id

        StringBuilder successMemberIds = new StringBuilder();//发送成功的会员ID
        StringBuilder failMemberIds = new StringBuilder();//发送失败的会员Id
        StringBuilder successEmails = new StringBuilder();
        StringBuilder failEmails = new StringBuilder();

        var emailInfo = ConfigHelper.GetEmailConfig();//源邮箱配置信息

        SendEmailRecord model = ModelFactory<SendEmailRecord>.Insten();
        if (_sendType.Equals("0"))
        {
            #region 按选择会员发送

            string memberData = _hidMembers;

            //会员ID、Email对
            string[] idEmailArr = memberData.Split(',');
            foreach (string idEmail in idEmailArr)
            {
                string[] keyValue = idEmail.Split('|');
                if (keyValue[0] == "" || keyValue[1] == "")
                {
                    continue;
                }
                //真正的发送内容
                string sendContent = ServiceFactory.MemberService.ParseEmailContent(_emailContent, keyValue[0].ToInt());
               
                if (EmailHelper.Send(keyValue[1], _title, sendContent))
                {
                    successNum++;
                    successMemberIds.Append(keyValue[0] + ",");//记录会员Id
                    successEmails.Append(keyValue[1] + ";");//记录会员email
                }
                else//发送失败
                {
                    failNum++;
                    failMemberIds.Append(keyValue[0] + ",");//记录会员Id
                    failEmails.Append(keyValue[1] + ";");//记录会员email
                }
                
                //发送邮件
                //if (SendEmailHelper.SendEmail(_title, sendContent, emailInfo.Email, emailInfo.Email, emailInfo.Password, keyValue[1], emailInfo.SMTP, emailInfo.Port.ToInt()))
                //{
                //    successNum++;
                //    successMemberIds.Append(keyValue[0] + ",");//记录会员Id
                //    successEmails.Append(keyValue[1] + ";");//记录会员email
                //}
                //else//发送失败
                //{
                //    failNum++;
                //    failMemberIds.Append(keyValue[0] + ",");//记录会员Id
                //    failEmails.Append(keyValue[1] + ";");//记录会员email
                //}
            }
            #endregion

            if (successNum == 0 && failNum == 0)
            {
                return new HandlerResult {Status = false, Message = "选择的会员都没有填写邮箱地址，无法发送邮件".ToLang()};
            }
            else
            {
                return new HandlerResult { Status = true, Message = "发送成功".ToLang() };
            }
            model.Category = 0;
        }
        else if (_sendType.Equals("1"))
        {
            #region 按会员类别发送

            int groupId = _memberGroup.ToInt();//会员类别编号
            IDictionary<int, string> listEmail = ServiceFactory.MemberService.GetEmailByMembertype(groupId);
            foreach (KeyValuePair<int, string> item in listEmail)
            {
                //真正的发送内容
                string sendContent = ServiceFactory.MemberService.ParseEmailContent(_emailContent, item.Key);

                //发送邮件
                if (EmailHelper.Send(item.Value, _title, sendContent))
                {
                    successNum++;
                    successMemberIds.Append(item.Key + ",");//记录会员Id
                    successEmails.Append(item.Value + ";");//记录会员email
                }
                else//发送失败
                {
                    failNum++;
                    failMemberIds.Append(item.Key + ",");//记录会员Id
                    failEmails.Append(item.Value + ";");//记录会员email
                }
            }
            #endregion

            if (successNum == 0 && failNum == 0)//该会员组下没有会员或会员都没有邮箱地址
            {
                return new HandlerResult { Status = false, Message = "选择的会员都没有填写邮箱地址，无法发送邮件".ToLang() };
            }
            else
            {
                return new HandlerResult { Status = true, Message = "发送成功".ToLang() };
            }
            model.Category = 1;
            model.MemberGroupId = groupId;//会员组ID
        }
        else
        {
            #region 指定邮箱发送
            string[] list = _emails.Split(';');
            //发送邮件
            foreach (var email in list)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    if (EmailHelper.Send(email, _title, _emailContent))
                    {
                        successNum++;
                        successEmails.Append(email + ";");
                    }
                    else//发送失败
                    {
                        failNum++;
                        failEmails.Append(email + ";");
                    }
                }
            }
            #endregion
            model.Category = 2;
        }
        # region 发送记录字段信息处理
        if (successMemberIds.Length > 0)
        {
            successIds = successMemberIds.ToString().Substring(0, successMemberIds.Length - 1);//去除“,”
        }
        if (failMemberIds.Length > 0)
        {
            failIds = failMemberIds.ToString().Substring(0, failMemberIds.Length - 1);//去除“,”
        }
        #endregion

        model.SuccessEmail = successEmails.ToStr();
        model.FailEmail = failEmails.ToStr();
        model.Title = _title;
        model.Content = _emailContent;
        model.SuccessMemberIds = successIds;
        model.FailMemberIds = failIds;
        if (failNum == 0)
        {
            model.SendState = 1;//全部发送成功
        }
        else if (successNum == 0)
        {
            model.SendState = 0;//发送失败
        }
        else
        {
            model.SendState = 2;//部分成功
        }
        //记录发送日记
        ServiceFactory.SendEmailRecordService.Save(model);
        //Alert("已经发送，成功" + SuccessNum + "条，失败" + FailNum + "条");
        return new HandlerResult { Status = true, Message = "已经发送，成功{0}条，失败{1}条".ToLang().FormatWith(successNum, failNum) };
    }

     protected HandlerResult SaveDb()
    {
        SendEmailRecord model = ModelFactory<SendEmailRecord>.Insten();
        model.SendState = 3;//未发送
        //标题
        model.Title = _title.Trim();
        //内容
        //Model.Content = txtContent.Text.Trim();
       
        model.Content = _emailContent;

        //指定方式，邮箱
        _emails = _emails.Trim();
        //会员类别
        int groupId = _memberGroup.ToInt();//会员类别编号
        //选择会员
        string memberData = _hidMembers;
        switch (_sendType)
        {
            case "0":
                #region //指定会员

                string ids = "";
                StringBuilder buidler = new StringBuilder();
                StringBuilder emailBuidler = new StringBuilder();
                string[] idEmailArr = memberData.Split(',');
                foreach (string idEmail in idEmailArr)
                {
                    string[] keyValue = idEmail.Split('|');
                    if (keyValue[0] != "")
                    {
                        buidler.Append(keyValue[0]);
                        buidler.Append(",");
                        emailBuidler.Append(keyValue[1]);
                        emailBuidler.Append(";");
                    }
                }
                if (buidler.Length > 0)
                {
                    ids = buidler.ToString().Substring(0, buidler.Length - 1);
                    _emails = emailBuidler.ToString();
                }

                if (_emails.Replace(";", "") == "")//选择的会员都没有填写邮箱
                {
                    return new HandlerResult { Status = false, Message = "选择的会员都没有填写邮箱地址，邮件内容不作保存".ToLang() };
                }
                model.Category = 0;
                model.FailMemberIds = ids;
                model.FailEmail = _emails;

                #endregion
                break;
            case "1":
                #region //会员类别
                int haveEmailCount = ServiceFactory.MemberService.GetEmailByMembertype(groupId).Count();
                if (haveEmailCount <= 0)
                {
                    return new HandlerResult { Status = false, Message = "选择的会员都没有填写邮箱地址，邮件内容不作保存".ToLang() };
                }
                model.Category = 1;
                model.MemberGroupId = groupId;
                #endregion
                break;
            case "2"://指定邮箱
                model.Category = 2;
                model.FailEmail = _emails;
                break;
        }
      
        if (EmailHelper.Send(_emails, _title, _emailContent))
        {
            DbHelper.CurrentDb.Save(model);
            return new HandlerResult { Status = true, Message = "发送成功".ToLang() };
        }
        else//发送失败
        {
            return new HandlerResult { Status = true, Message = "发送失败".ToLang() };
        }
      
    }
}