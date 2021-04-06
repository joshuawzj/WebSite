using System;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Security;
using Whir.ezEIP.Web.HttpHandlers;
using Whir.Security.Service;
using Whir.Service;
using Whir.Language;

public partial class whir_system_Handler_Security_User : SysHandlerPageBase
{
    private const string REMEMBER_USERNAME_COOKIEKEY = "{5E73B429-3E4A-43EB-9A34-05EFAE8C5DC1}";
    private const string LOGINTIME_SESSION = "{72943b2f-c150-4d4e-a9fb-0717c6f1b925 }";

    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 更换皮肤
    /// </summary>
    /// <returns></returns>
    public HandlerResult ModifySkin()
    {
        string systemSkin = RequestUtil.Instance.GetFormString("SystemSkin");

        if (CurrentUser.UserId == 0)
            return new HandlerResult { Status = false, Message = "当前用户不存在".ToLang() };

        CurrentUser.SystemSkin = systemSkin;

        Whir.Security.ServiceFactory.UsersService.Update(CurrentUser);

        return new HandlerResult { Status = true, Message = "更换皮肤成功".ToLang() };
    }

    /// <summary>
    /// 每30天修改一次 修改时长可在web.config配置
    /// </summary>
    /// <returns></returns>
    public HandlerResult ChangePwdFor30Day()
    {
        string userName = RequestUtil.Instance.GetFormString("UserName");
        string oldPassword = RequestUtil.Instance.GetFormString("txtOldPassword");
        string newPassword = RequestUtil.Instance.GetFormString("txtNewPassWord");
        //js base64加密后的密码，需要解密
        oldPassword = oldPassword.DecodeBase64();
        newPassword = newPassword.DecodeBase64();
        if (userName.IsEmpty())
            return new HandlerResult { Status = false, Message = "找不到记录".ToLang() };
        if (oldPassword.IsEmpty())
            return new HandlerResult { Status = false, Message = "请输入旧密码".ToLang() };
        if (newPassword.IsEmpty())
            return new HandlerResult { Status = false, Message = "请输入新密码".ToLang() };

        if (!newPassword.IsGoodPwd() && AppSettingUtil.GetString("IsStrongPassword").ToBoolean(true))
            return new HandlerResult { Status = false, Message = "密码必须包含数字、字母、特殊符号，长度在6-20位".ToLang() };

        oldPassword = StrExt.GetSHA1Str(oldPassword);
        newPassword = StrExt.GetSHA1Str(newPassword);
        var user = Whir.Security.ServiceFactory.UsersService.GetUserByLoginName(userName);
        if (user == null)
            return new HandlerResult { Status = false, Message = "旧密码输入错误或用户不存在，如果尝试{0}次后仍然失败请{1}分钟后重试！".FormatWith(AppSettingUtil.GetInt32("ErrorCount"), AppSettingUtil.GetInt32("LockTime")) };
        else if (user.ErrorCount >= AppSettingUtil.GetInt32("ErrorCount") && (DateTime.Now - user.ErrorLastTime.ToDateTime()).TotalMinutes <= 30)
        {
            return new HandlerResult { Status = false, Message = "旧密码输入错误或用户不存在，如果尝试{0}次后仍然失败请{1}分钟后重试！".FormatWith(AppSettingUtil.GetInt32("ErrorCount"), AppSettingUtil.GetInt32("LockTime")) };
        }
        else if (user.Password != oldPassword)
        {
            user.ErrorCount = user.ErrorCount + 1;
            user.ErrorLastTime = DateTime.Now;
            Whir.Security.ServiceFactory.UsersService.Update(user);
            return new HandlerResult { Status = false, Message = "旧密码输入错误或用户不存在，如果尝试{0}次后仍然失败请{1}分钟后重试！".FormatWith(AppSettingUtil.GetInt32("ErrorCount"), AppSettingUtil.GetInt32("LockTime")) };
        }

        if (oldPassword == newPassword)
            return new HandlerResult { Status = false, Message = "旧密码输入错误或用户不存在，如果尝试{0}次后仍然失败请{1}分钟后重试！".FormatWith(AppSettingUtil.GetInt32("ErrorCount"), AppSettingUtil.GetInt32("LockTime")) };
        user.Password = newPassword;
        user.LastPasswordDate = DateTime.Now;
        user.ErrorCount = 0;
        Whir.Repository.DbHelper.CurrentDb.Update(user);

        Whir.Service.ServiceFactory.OperateLogService.Save("【{0}】管理员每{1}天修改登录密码成功".FormatWith(user.LoginName, AppSettingUtil.GetInt32("ChangePwdDays")));

        return new HandlerResult { Status = true, Message = "修改密码成功".ToLang() };

    }

    #region  异步登录

    /// <summary>
    /// 异步登录
    /// </summary>
    public HandlerResult Login()
    {
        string loginName = RequestUtil.Instance.GetFormString("UserName");
        string password = RequestUtil.Instance.GetFormString("Password");
        string code = RequestUtil.Instance.GetFormString("verifiyCode");
        string isRemember = RequestUtil.Instance.GetFormString("isRemember");
        //js base64加密后的密码，需要解密
        password = password.DecodeBase64();
        if (loginName.IsEmpty() || password.IsEmpty())
            return new HandlerResult { Status = false, Message = "请填写账号、密码".ToLang() };

        // 输入过滤验证.
        var isSafe = loginName.IsSafeSql();
        if (isSafe)
            return new HandlerResult { Status = false, Message = "用户名含非法字符，请输入合法的用户名".ToLang() };

        if (VerifyCheckCode(code))
        {
            var loginService = new LoginService();
            var status = loginService.Login(loginName, password, AppSettingUtil.GetString("SystemPath"));
            Session[CheckCodeHandler.CheckCode_Key] = null; //清空验证码的session
            HandlerResult hr;
            switch (status)
            {
                case LoginStatus.LicenseInvalid:
                    SetLoginTime();
                    hr = new HandlerResult { Status = false, Message = "-1".ToLang() }; //登录失败，未授权
                    break;
                case LoginStatus.AccountNotFound:
                    SetLoginTime();
                    hr = new HandlerResult { Status = false, Message = "登录失败，用户名或密码错误".ToLang() };
                    break;
                case LoginStatus.AccountDisabled:
                    SetLoginTime();
                    hr = new HandlerResult { Status = false, Message = "登录失败，用户被禁用".ToLang() };
                    break;
                case LoginStatus.InvalidPassword:
                    SetLoginTime();
                    hr = new HandlerResult { Status = false, Message = "登录失败，用户名或密码错误".ToLang() };
                    break;
                case LoginStatus.NotAction:
                    SetLoginTime();
                    hr = new HandlerResult { Status = false, Message = "登录失败，用户限制登录，请{0}分钟后重试".FormatWith(AppSettingUtil.GetInt32("LockTime")).ToLang() };
                    break;
                case LoginStatus.NeedChangePassword:
                    SetLoginTime();
                    hr = new HandlerResult { Status = false, Message = "-2".ToLang() };
                    break;
                case LoginStatus.Success:
                    Whir.Service.ServiceFactory.OperateLogService.Save(
                        LogType.SystemRunLog,
                        "【{0}】登录成功".FormatWith(loginName));
                    Session[LOGINTIME_SESSION] = 0;
                    RememberLoginName(isRemember == "1", loginName);
                    return new HandlerResult { Status = true, Message = "ok".ToLang() };
                default:
                    hr = new HandlerResult { Status = false, Message = "未知错误".ToLang() };
                    break;

            }

            if (status != LoginStatus.Success)
            {
                Whir.Service.ServiceFactory.OperateLogService.Save(
                    LogType.SystemRunLog,
                    "后台-用户名:{0},密码:{1},登录失败".FormatWith(loginName, password));
            }

            return hr;
        }
        else
        {
            return new HandlerResult { Status = false, Message = "请输入正确的验证码".ToLang() };
        }

    }

    /// <summary>
    /// 获取记住的用户名
    /// </summary>
    public HandlerResult GetLoginName()
    {
        if (null != Request.Cookies[REMEMBER_USERNAME_COOKIEKEY] &&
                      !String.IsNullOrEmpty(Request.Cookies[REMEMBER_USERNAME_COOKIEKEY].Value))
        {
            return new HandlerResult { Status = true, Message = Request.Cookies[REMEMBER_USERNAME_COOKIEKEY].Value };

        }
        return new HandlerResult { Status = false, Message = "".ToLang() };
    }

    /// <summary>
    /// 是否显示验证码
    /// </summary>
    public HandlerResult IsShowCode()
    {
        if (GetLoginTime() > 2)
        {
            return new HandlerResult { Status = true, Message = "请输入验证码".ToLang() };

        }
        return new HandlerResult { Status = false, Message = "".ToLang() };
    }


    #region 记住用户名
    /// <summary>
    /// 记住用户名
    /// </summary>
    private void RememberLoginName(bool isRemember, string loginName)
    {
        //选中记住用户名时设置COOKIE,用以登录的用户名.
        if (isRemember)
        {
            if (null != CookieUtil.Instance.GetCookie(REMEMBER_USERNAME_COOKIEKEY))
            {
                CookieUtil.Instance.RemoveCookie(REMEMBER_USERNAME_COOKIEKEY);
            }
            CookieUtil.Instance.AddCookie(REMEMBER_USERNAME_COOKIEKEY, loginName, DateTime.Now.AddMonths(1));//记住用户名有效时间1个月
        }
        else
        {
            if (null != CookieUtil.Instance.GetCookie(REMEMBER_USERNAME_COOKIEKEY))
                CookieUtil.Instance.RemoveCookie(REMEMBER_USERNAME_COOKIEKEY);
        }
    }

    #endregion

    /// <summary>
    /// 检查验证码是否正确.
    /// </summary>
    public bool VerifyCheckCode(string code)
    {
        if (GetLoginTime() > 2)
        {
            if (this.Session[CheckCodeHandler.CheckCode_Key] == null)
            {
                return false;
            }
            string checkCode = this.Session[CheckCodeHandler.CheckCode_Key].ToString();
            return checkCode.Equals(code, StringComparison.OrdinalIgnoreCase);
        }
        return true;
    }


    #region 登录次数

    /// <summary>
    /// 得到当前登陆失败的次数
    /// </summary>
    /// <returns></returns>
    private int GetLoginTime()
    {
        return null != Session[LOGINTIME_SESSION] ? Session[LOGINTIME_SESSION].ToInt() : 0;
    }

    /// <summary>
    /// 设置登陆次数
    /// </summary>
    private void SetLoginTime()
    {
        if (Session[LOGINTIME_SESSION] == null)
        {
            Session[LOGINTIME_SESSION] = 1;
        }
        else
        {
            Session[LOGINTIME_SESSION] = Session[LOGINTIME_SESSION].ToInt() + 1;
        }
    }
    #endregion

    #endregion

}