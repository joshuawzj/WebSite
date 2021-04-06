/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：Main.aspx.cs
* 文件描述：后台首页。 
*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using Whir.Framework;
using Whir.Language;
using Whir.Security;
using Whir.Security.Domain;
using Whir.Security.Service;


public partial class whir_system_Main : Whir.ezEIP.Web.SysManagePageBase
{
    public LoginUser loginUser { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindUserInfo();
            ShowPasswordTips();//检测当前管理员密码是否为初始密码
        }
        GetRecord();
        EditErrorConfig();
    }

    /// <summary>
    /// 当前登录管理员的基本信息
    /// </summary>
    private void BindUserInfo()
    {
        loginUser = AuthenticateHelper.User;

        if (loginUser != null)
        {
            this.ltLastLoginTime.Text = loginUser.LoginName;
            if (string.IsNullOrEmpty(loginUser.LastLoginIP))
            {
                this.ltLastLoginIP.Text = "第一次登录".ToLang();
                this.ltLastLoginTime.Text = "";
            }
            else
            {
                if (loginUser.LastLoginIP == "fe80::3553:6985:f2e3:704a%12" || loginUser.LastLoginIP.ToLower().Contains("fe80") || loginUser.LastLoginIP.ToLower().Contains("::1"))
                {
                    this.ltLastLoginIP.Text = "127.0.0.1";
                }
                else
                {
                    this.ltLastLoginIP.Text = loginUser.LastLoginIP;
                }
                this.ltLastLoginTime.Text = loginUser.LastLoginTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
    }

    /// <summary>
    /// 上线后若初始密码没有改变则提醒
    /// </summary>
    private void ShowPasswordTips()
    {
        string url = HttpContext.Current.Request.Url.Host.ToStr().ToLower().Replace("http://", "");
        if (!url.Contains("localhost") && !url.Contains("127.0.0.1") && !url.Contains(".gzwhir.com") && !url.Contains(".chinaw3.com") && !url.StartsWith("192.") && !url.StartsWith("172.") && !url.StartsWith("10."))
        {
            Users user = ServiceFactory.UsersService.GetUserByLoginName(CurrentUserName);
            if (user != null)
            {
                List<string> initialPassword = new List<string>();
                initialPassword.Add(StrExt.GetSHA1Str("123456"));
                initialPassword.Add(StrExt.GetSHA1Str("root"));
                initialPassword.Add(StrExt.GetSHA1Str("root789"));
                initialPassword.Add(StrExt.GetSHA1Str("123456a"));

                if (initialPassword.Contains(user.Password))
                {
                    lbPassworTips.Text = "提示：您账号的密码与常用初始密码一致，必须重新设置登录密码".ToLang();
                    PnPassworTips.Visible = true;
                }
                else
                    lbPassworTips.Text = "温馨提示：为了您的帐户安全，请定期修改密码".ToLang();
            }
        }
        else
            lbPassworTips.Text = "温馨提示：为了您的帐户安全，请定期修改密码".ToLang();
    }

    /// <summary>
    /// 获取系统的相关信息
    /// </summary>
    private void GetRecord()
    {
        try
        {
            ltVersion.Text = "Website System " + ConfigurationManager.AppSettings["Version"];

            ltServerOS.Text = Environment.OSVersion.ToString();
            ltAspnetVer.Text = string.Concat(new object[] { Environment.Version.Major, ".", Environment.Version.Minor, Environment.Version.Build, ".", Environment.Version.Revision });
            ltDB.Text = Whir.Service.ServiceFactory.DbVersionService.GetDbVersion();

        }
        catch (Exception ex)
        {
            Alert("获取系统信息出错");
        }
    }

    /// <summary>
    /// 获取快捷菜单
    /// </summary>
    /// <returns></returns>
    public string GetQuickMenu()
    {
        string html = "";
        string[] defaultMenuIcon = "entypo-cog,entypo-user,fontawesome-list-ul,fontawesome-picture,fontawesome-envelope,entypo-search".Split(',');
        var quickMenuList = Whir.Service.ServiceFactory.QuickMenuService.GetList().ToList();
        for (int i = 0; i < quickMenuList.Count; i++)
        {
            if (quickMenuList[i].MenuName.IsEmpty())
                continue;
            string btnClass = "";
            switch (i % 4)
            {
                case 0:
                    btnClass = "btn-success";
                    break;
                case 1:
                    btnClass = "btn-danger";
                    break;
                case 2:
                    btnClass = "btn-primary ";
                    break;
                case 3:
                    btnClass = "btn-warning";
                    break;
                    //case 4:
                    //    btnClass = "btn-default";
                    //    break;
                    //case 5:
                    //    btnClass = "btn-info";
                    //    break;
            }
            html += "<div class=\"col-md-2\" style=\"margin-top:15px;\">";
            html += "   <a class=\"btn {1} btn-block btn_wap_margin btn-large hover-zoom font-size14\" href=\"{0}\">"
                .FormatWith(quickMenuList[i].Url, btnClass);
            html += "      <span class=\"{0} font-size60\" {1}></span>".FormatWith(
                quickMenuList[i].MenuIcon.IsNotEmpty() ? "quickico" : defaultMenuIcon[i % 6],
                quickMenuList[i].MenuIcon.IsNotEmpty()
                    ? "style=\"background:url(" + UploadFilePath + quickMenuList[i].MenuIcon + ")no-repeat center;\""
                    : "");
            html += "          <br />";
            html += quickMenuList[i].MenuName;
            html += "    </a>";
            html += "</div>   ";

        }
        return html;

    }

    /// <summary>
    /// 自动修改测试站上的404页面、error页面的路径
    /// </summary>
    public void EditErrorConfig()
    {
        try
        {
            var hostUrl = RequestUtil.Instance.GetHttpUrl().ToLower();
            var projectNum = AppSettingUtil.GetString("ProjectNum");
            var isOnline = AppSettingUtil.GetString("IsOnline");

            if (AppName.Length > 1 && projectNum != AppName.Trim('/'))
            {
                projectNum = AppName.Trim('/').IsEmpty() ? projectNum : AppName.Trim('/');
                AppSettingUtil.SetAppSettings("ProjectNum", projectNum);
                if (File.Exists(Server.MapPath("~/web.config")))
                {
                    var str = File.ReadAllText(Server.MapPath("~/web.config"));
                    str = str.Replace("\"/404.aspx\"", "\"/" + projectNum + "/404.aspx\"");
                    str = str.Replace("\"/Error.aspx\"", "\"/" + projectNum + "/Error.aspx\"");
                    File.WriteAllText(Server.MapPath("~/web.config"), str);
                }
                if (File.Exists(Server.MapPath("~/Config/CustomErrors.config")))
                {
                    var str = File.ReadAllText(Server.MapPath("~/Config/CustomErrors.config"));
                    str = str.Replace("\"/404.aspx\"", "\"/" + projectNum + "/404.aspx\"");
                    File.WriteAllText(Server.MapPath("~/Config/CustomErrors.config"), str);
                }
            }
            else if (AppName.Length <= 1 && projectNum.IsNotEmpty())
            {
                if (File.Exists(Server.MapPath("~/web.config")))
                {
                    var str = File.ReadAllText(Server.MapPath("~/web.config"));
                    str = str.Replace("\"/" + projectNum + "/404.aspx\"", "\"/404.aspx\"");
                    str = str.Replace("\"/" + projectNum + "/Error.aspx\"", "\"/Error.aspx\"");
                    File.WriteAllText(Server.MapPath("~/web.config"), str);
                }
                if (File.Exists(Server.MapPath("~/Config/CustomErrors.config")))
                {
                    var str = File.ReadAllText(Server.MapPath("~/Config/CustomErrors.config"));
                    str = str.Replace("\"/" + projectNum + "/404.aspx\"", "\"/404.aspx\"");
                    File.WriteAllText(Server.MapPath("~/Config/CustomErrors.config"), str);
                }
                AppSettingUtil.SetAppSettings("ProjectNum", "");
            }
        }
        catch (Exception ex)
        {
            LogHelper.Log(ex);
        }
    }

}