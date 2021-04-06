/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：SystemLogin.aspx.cs
* 文件描述：点击锁屏后重新登录。 
*/
using System;

using Whir.Framework;
using Whir.Security.Service;
using Whir.Security;
using Whir.Language;

public partial class whir_system_ajax_SystemLogin : System.Web.UI.Page
{
    private const string LOGINTIME_COOKIEKEY = "{72943b2f-c150-4d4e-a9fb-0717c6f1b9290}";
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (GetLoginTime() > 9)
        {
            Response.Write("-1");//密码输入次数超过10次，将跳转到登录页面登录
            CookieUtil.Instance.RemoveCookie(LOGINTIME_COOKIEKEY);
            Response.End();
        }
        string LoginName = RequestUtil.Instance.GetQueryString("username");
        string PassWord = RequestUtil.Instance.GetQueryString("pwd");

        if (LoginName == "")
        {
            Response.Write("用户名不能为空".ToLang()); 
            Response.End();
        }
        if (PassWord == "")
        {
            Response.Write("密码不能为空".ToLang()); 
            Response.End();
        }
        LoginService loginService = new LoginService();
        loginService.LoginSuccessOrFailure += new LoginEventHandler(login_OnLoginSuccessOrFailure);
        loginService.Login(LoginName, PassWord, AppSettingUtil.GetString("SystemPath"));
    }

    /// <summary>
    /// 登录后事件处理.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="username"></param>
    protected void login_OnLoginSuccessOrFailure(object sender, LoginEventArgs e)
    {
        string Msg = "";
        switch (e.Status)
        {
            case Whir.Security.LoginStatus.LicenseInvalid:
                Msg = "未授权".ToLang();
                SetLoginTime();
                break;
            case Whir.Security.LoginStatus.AccountNotFound:
                Msg = "找不到该账号".ToLang();
                 SetLoginTime();
                break;
            case Whir.Security.LoginStatus.AccountDisabled:
                Msg = "登录失败，用户被禁用".ToLang();
                SetLoginTime();
                break;
            case Whir.Security.LoginStatus.InvalidPassword:
                Msg = "登录失败，密码错误".ToLang();
                SetLoginTime();
                break;
            case Whir.Security.LoginStatus.Success:
                Msg = "true";
                break;
            default:
                Msg = "未知错误".ToLang();
                SetLoginTime();
                break;
        }
        Response.Write(Msg);
    }
    /// <summary>
    /// 得到当前登陆失败的次数
    /// </summary>
    /// <returns></returns>
    private int GetLoginTime()
    {
        if (null != CookieUtil.Instance.GetCookie(LOGINTIME_COOKIEKEY))
        {
            return CookieUtil.Instance.GetCookie(LOGINTIME_COOKIEKEY).Value.ToInt();
        }
        else
        {
            return 0;
        }
    }
        /// <summary>
    /// 设置登陆次数的Cookies
    /// </summary>
    private void SetLoginTime()
    {
        if (CookieUtil.Instance.GetCookie(LOGINTIME_COOKIEKEY) == null)
        {
            CookieUtil.Instance.AddCookie(LOGINTIME_COOKIEKEY, "1", DateTime.Now.AddHours(1));
        }
        else
        {
            int NewTimesValue = CookieUtil.Instance.GetCookie(LOGINTIME_COOKIEKEY).Value.ToInt() + 1;
            CookieUtil.Instance.RemoveCookie(LOGINTIME_COOKIEKEY);
            CookieUtil.Instance.AddCookie(LOGINTIME_COOKIEKEY, NewTimesValue.ToStr(), DateTime.Now.AddHours(10)); //有效时间1小时
        }
    }
}