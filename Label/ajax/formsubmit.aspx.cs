/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：formsubmit.aspx.cs
 * 文件描述：表单数据接收操作类
 */
using System;
using System.Web;
using System.Linq;

using Whir.Service;
using Whir.Domain;
using Whir.Framework;
using Whir.Repository;
using Whir.ezEIP.Web.HttpHandlers;
using System.Collections.Generic;


public partial class label_ajax_formsubmit : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        //表单ID
        int formid = RequestUtil.Instance.GetQueryInt("formid", 0);
        int columnID = RequestUtil.Instance.GetQueryInt("columnid", 0);
        //提交表单主键
        int submitID = RequestUtil.Instance.GetQueryInt("submitid", 0);
        string postBackTips = "{{type:'{0}',msg:'{1}',returnurl:'{2}'}}";
        SubmitForm Model = ServiceFactory.SubmitFormService.SingleOrDefault<SubmitForm>(submitID);
        try
        {
            if (Model != null)
            {
                #region 验证码验证

                if (Model.IsAuthCode)
                {
                    if (this.Session[CheckCodeHandler.CheckCode_Key] == null)
                    {
                        //验证码无效
                        postBackTips = string.Format(postBackTips, 0, Model.AuthCodeErrTip, "false");
                        Response.Write(postBackTips);
                        Response.End();
                        return;
                    }
                    string checkCode = this.Session[CheckCodeHandler.CheckCode_Key].ToString();
                    string codeId = submitID + "_code";
                    string backCode = HttpContext.Current.Request.Form[codeId];

                    if (!checkCode.Equals(backCode, StringComparison.OrdinalIgnoreCase))
                    {
                        //请输入正确的验证码
                        postBackTips = string.Format(postBackTips, 0, Model.AuthCodeErrTip, "false");
                        Response.Write(postBackTips);
                        Response.End();
                        return;
                    }
                    //用完验证码，应该清空，重新生成新的验证，对应的前台页面也应该刷新验证码
                    this.Session[CheckCodeHandler.CheckCode_Key] = null;
                }

                #endregion

                #region 操作表单数据，向数据库插入表单数据

                Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnID);
                if (columnID != 0)
                {
                    //邮箱信息
                    string emailContent = "";
                    IList<KeyValuePair<string, string>> onlydata = new List<KeyValuePair<string, string>>();
                    //SQL参数
                    List<object> parems = new List<object>();
                    string sql = string.Empty, msg = string.Empty;
                    //获取表单内容并返回SQL插入语句
                    if (ContentHelper.GetHtmlFormData(column.ModelId, columnID, Model, out parems, out emailContent, out onlydata, out sql, out msg))
                    {
                        if (Model.ColumnId == 1 && Model.MemberType == (int)SubmitFormMemType.Personal)//会员资料更新操作
                        {
                            #region 会员资料更新
                            if (WebUser.IsLogin())
                            {
                                int memberId = WebUser.GetUserValue("whir_mem_member_pid").ToInt();
                                parems.Add(memberId);
                            }
                            else
                            {
                                Response.Write(postBackTips.FormatWith(0, "帐号未登录", "true"));
                                return;
                            }
                            #endregion 会员资料更新
                        }
                        else if (Model.ColumnId == 1 && Model.MemberType == (int)SubmitFormMemType.Regist)//注册需要做唯一性验证，验证不通过表明是前台恶搞行为
                        {
                            #region 会员注册

                            bool validatPass = false;
                            if (onlydata.Count >= 2)
                            {
                                var loginNameItem = onlydata.Where(p => p.Key.ToLower() == "loginname" && !p.Value.IsEmpty());
                                if (loginNameItem.Count() > 0)
                                {
                                    var item = loginNameItem.SingleOrDefault();
                                    bool isExitLoginName = WebUser.IsExist("LoginName", item.Value.Trim());//验证码用户名
                                    if (!isExitLoginName)
                                    {
                                        var emailItem = onlydata.Where(p => p.Key.ToLower() == "email" && !p.Value.IsEmpty());
                                        if (emailItem.Count() > 0)
                                        {
                                            var item2 = emailItem.SingleOrDefault();
                                            validatPass = !WebUser.IsExist("Email", item2.Value.Trim());//验证码email
                                        }
                                        else
                                            validatPass = true;
                                    }

                                }
                            }
                            if (!validatPass)
                            {
                                Response.Write(postBackTips.FormatWith(0, "用户名或安全邮箱验证不通过", "true"));
                                return;
                            }
                            #endregion 会员注册
                        }
                        int count = DbHelper.CurrentDb.Execute(sql, parems.ToArray());
                        if (count > 0)
                        {
                            postBackTips = string.Format(postBackTips, 1, Model.SuccessfulTip, Model.SuccessAction);
                            //数据操作成功，发送邮件通知接收人
                            if (!Model.ReceiveEmail.IsEmpty())
                            {

                                if (Model.IsSubmitDataMsg)
                                {
                                    string[] emails = Model.ReceiveEmail.Split(';');
                                    foreach (string email in emails)
                                    {
                                        if (email != "")
                                        {
                                            SendEmailHelper.SendEmail(email, Model.EmailTitle, emailContent);
                                        }
                                    }
                                }
                                else
                                {
                                    string[] emails = Model.ReceiveEmail.Split(';');
                                    foreach (string email in emails)
                                    {
                                        if (email != "")
                                        {
                                            SendEmailHelper.SendEmail(email, Model.EmailTitle, Model.EmailContent);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                        postBackTips = string.Format(postBackTips, 0, msg + "字段不能为空！", "false");

                }
                #endregion
            }
            else
            {
                postBackTips = string.Format(postBackTips, 0, Model.UnKnowFailingTip, "false");
            }
        }
        catch (Exception ex)
        {
            switch (ex.Message)
            {
                case "FileTypeErr":
                    postBackTips = string.Format(postBackTips, 0, Model.FileTypeErrTip, "false");
                    break;
                case "FileMaxErr":
                    postBackTips = string.Format(postBackTips, 0, Model.OverFileMaxErrTip, "false");
                    break;
                case "ImgTypeErr":
                    postBackTips = string.Format(postBackTips, 0, Model.ImageTypeErrTip, "false");
                    break;
                case "ImgMaxErr":
                    postBackTips = string.Format(postBackTips, 0, Model.OverImgMaxErrTip, "false");
                    break;
                default:
                    postBackTips = string.Format(postBackTips, 0, Model.UnKnowFailingTip, "false");
                    break;
            }
        }
        Response.Write(postBackTips);
        Response.End();
    }

}