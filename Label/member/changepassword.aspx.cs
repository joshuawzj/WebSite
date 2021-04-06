/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：changepassword.cs
* 文件描述：更改密码处理页面
*/
using System;

using Whir.Framework;

public partial class label_member_changepassword : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //-1:帐号未登录，0：修改密码失败，1：原密码不正确，2：修改成功
        string state = "-1";
        bool isLogin = WebUser.IsLogin();

        if (isLogin)
        {
            string password = RequestUtil.Instance.GetFormString("password");//密码
            string newPassword = RequestUtil.Instance.GetFormString("newpassword");//密码
            if (!string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(newPassword))
            {
                state = WebUser.ChangPassword(password, newPassword);
            }
        }
        Response.Write(state);
        Response.End();

    }
}