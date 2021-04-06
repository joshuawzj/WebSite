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
using System.Data;

using Whir.ezEIP.Web.HttpHandlers;
using Whir.Framework;
using Whir.Repository;
using Whir.Service;

public partial class Shop_member_forgotpassword : Shop.Common.PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void rbWay_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbWay.SelectedValue == "1")//通过用户名找回密码
        {
            ltNameEmail.Text = "用户名：";
            txtNameEmail.Regular = Whir.Framework.TextBox.RegularEnum.Never;
        }
        else//通过安全邮箱找回密码
        {
            ltNameEmail.Text = "安全邮箱：";
            txtNameEmail.Regular = Whir.Framework.TextBox.RegularEnum.Email;
        }
    }
    protected void btnNext_Click(object sender, EventArgs e)
    {
        string msg = "";//提示
        string nameEmail = txtNameEmail.Text.Trim();
        if (this.Session[CheckCodeHandler.CheckCode_Key] == null)
        {
            msg = "验证码无效";
        }
        else
        {
            string checkCode = this.Session[CheckCodeHandler.CheckCode_Key].ToString();
            string codeStr = txtCode.Text.Trim();

            if (!checkCode.Equals(codeStr, StringComparison.OrdinalIgnoreCase))
            {
                msg = "验证码不正确";
            }
            else
            {
                string SQL = "SELECT WHIR_MEM_MEMBER_PID, LoginName,Email FROM WHIR_MEM_MEMBER WHERE {0}=@0";
                if (rbWay.SelectedValue == "1")//通过用户名找回密码
                {
                    SQL = SQL.FormatWith("LoginName");
                }
                else//通过安全邮箱找回密码
                {
                    SQL = SQL.FormatWith("Email");
                }

                DataTable table = DbHelper.CurrentDb.Query(SQL, nameEmail).Tables[0];

                if (table.Rows.Count > 0)
                {
                    ltMsgStart.Text = "";
                    hdUserID.Value = table.Rows[0][0].ToStr();
                    ltUserName.Text = table.Rows[0][1].ToStr();
                    ltEmail.Text = table.Rows[0][2].ToStr();
                    phMiddle.Visible = true;
                    phStart.Visible = false;
                }
                else
                {
                    msg = "没有找到用户";
                }
            }
        }
        ltMsgStart.Text = "<span style='color:red;'>{0}</span>".FormatWith(msg);
    }

    protected void btnSendEmail_Click(object sender, EventArgs e)
    {
        bool isSuccess = false;
        try
        {
            int userId = hdUserID.Value.ToInt();
            string emailMessage = ServiceFactory.MemberService.MemberRetakePassword(userId);
            isSuccess = SendEmailHelper.SendEmail(ltEmail.Text, "找回密码邮件", emailMessage);
        }
        catch (Exception ex)
        {

        }
        phEnd.Visible = true;
        phMiddle.Visible = false;
        if (isSuccess)
        {
            divSuccess.Visible = true;
            divfailure.Visible = false;
        }
        else
        {
            divSuccess.Visible = false;
            divfailure.Visible = true;
        }
    }
}