/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：SystemLoginOut.aspx.cs
* 文件描述：点击锁屏后退出登录。 
*/
using System;

using Whir.Security.Service;

public partial class whir_system_ajax_SystemLoginOut : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LogoutService logoutService = new LogoutService(); 
        logoutService.Logout();
        Response.Write("true");
    }
}