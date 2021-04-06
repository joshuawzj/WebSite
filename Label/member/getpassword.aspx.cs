/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：getpassword.cs
* 文件描述：找回密码处理页面
*/
using System;

using Whir.Framework;
using Whir.ezEIP.Web.HttpHandlers;
using Whir.Repository;

public partial class label_member_getpassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string userName = RequestUtil.Instance.GetFormString("username");
        string email = RequestUtil.Instance.GetFormString("email");
        string code = RequestUtil.Instance.GetFormString("code");
        string result = string.Empty;//结果

        if (this.Session[CheckCodeHandler.CheckCode_Key] == null)
        {
            //验证码无效
            result = "验证码无效";
        }
        else
        {
            string checkCode = this.Session[CheckCodeHandler.CheckCode_Key].ToString();
            if (checkCode.Equals(code, StringComparison.OrdinalIgnoreCase))
            {
                var table = DbHelper.CurrentDb.Query("SELECT Whir_Mem_Member_PID FROM Whir_Mem_Member WHERE LoginName=@0 AND Email=@1", userName, email).Tables[0];
                if (table.Rows.Count > 0)
                {
                    int userId = table.Rows[0]["Whir_Mem_Member_PID"].ToInt();
                    bool isSend = WebUser.SendRetakePwd(userId, email);
                    if (isSend)
                    {
                        result = "密码已发送到您的邮箱，请注意查收";
                    }
                    else
                    {
                        result = "找回密码失败，请重试";
                    }
                }
                else
                {
                    result = "用户名与安全邮箱不匹配";
                }
            }
            else
            {
                result = "验证码不正确";
            }
        }
        Response.Write(result);
        Response.End();
    }
}