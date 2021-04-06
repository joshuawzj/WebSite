/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：
 * 文件描述：
 * 
 * 创建标识: liuyong 2012-02-07
 * 
 * 修改标识：
 */
using System;
using System.Web;
using System.Web.UI;

using Whir.Framework;
using Whir.Service;
using Whir.Repository;

public partial class Shop_member_email : Shop.Common.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            WebUser.IsLogin("shop/member/login.aspx");
            ltOldEmail.Text = WebUser.GetUserValue("Email");
            if (string.IsNullOrEmpty(ltOldEmail.Text))
            {
                ltOldEmail.Text = "尚未绑定邮箱，请在修改邮箱项输入邮箱进行绑定";
            }
        }
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        string changeEmail = txtTakeEmail.Text.Trim();
        string randomNum = Rand.Instance.Str(10);
        string http = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
        http += WebUtil.Instance.AppPath() + "shop/member/emailsuccess.aspx?loginname={0}&email={1}&r={2}".FormatWith(WebUser.GetUserValue("LoginName"), changeEmail, randomNum);
        SendEmailHelper.SendEmail(WebUser.GetUserValue("email"), "更换安全邮箱", "<a href=\"{0}\">{0}</a>".FormatWith(http));
        //随机数放进
        DbHelper.CurrentDb.Execute("Update Whir_Mem_Member Set RandomNum=@0 WHERE Whir_Mem_Member_PID=@1", randomNum, WebUser.GetUserValue("Whir_Mem_Member_PID"));
        string script = "<script language=\"javascript\" defer=\"defer\">alert('更改安全邮箱需身份验证，验证邮件已发送，请注意查收！');</script>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
    }
}