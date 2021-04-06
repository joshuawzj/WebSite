/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：loginout.aspx.cs
* 文件描述：退出登录。 
*/
using System;
using System.Web;

using Whir.Security.Service;
using Whir.Service;

public partial class whir_system_module_security_loginout : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        LogoutService logoutService = new LogoutService();
        logoutService.PreLogout += new EventHandler(logout_PreLogout);
        logoutService.Logout();
        Session.Clear();
        HttpContext.Current.Response.Redirect("../../Login.aspx"); 
    }
    void logout_PreLogout(object sender, EventArgs e)
    { 
        string loginUserName= (null != AuthenticateHelper.User) ? AuthenticateHelper.User.LoginName :"";
        Whir.Service.ServiceFactory.OperateLogService.Save(LogType.SystemRunLog, string.Format("【{0}】退出登录成功", loginUserName)); 
    }
}