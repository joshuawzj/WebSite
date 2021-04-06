/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：login.aspx.cs
* 文件描述：管理员登录页。 
*/
using System;

using Whir.Framework;
using Whir.ezEIP.Web.HttpHandlers;
using Whir.Security.Service;
using Whir.Security;
using Whir.Config;
using Whir.Service;
using Whir.ezEIP.Web;


public partial class whir_system_ezEIP_Login : System.Web.UI.Page
{
    #region 属性
    public string AppPath = WebUtil.Instance.AppPath() + AppSettingUtil.GetString("SystemPath");
    protected const string REMEMBER_USERNAME_COOKIEKEY = "{5E73B429-3E4A-43EB-9A34-05EFAE8C5DC1}";
    protected const string LOGINTIME_COOKIEKEY = "{72943b2f-c150-4d4e-a9fb-0717c6f1b925 }";

    //后台相对路径 
    public string SystemPath = WebUtil.Instance.AppPath() + AppSettingUtil.GetString("SystemPath");
    public string UploadFilePath = WebUtil.Instance.AppPath() + AppSettingUtil.GetString("UploadFilePath");
    public Whir.Config.Models.SystemConfig SystemConfig = ConfigHelper.GetSystemConfig();

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        //检查可否访问后台路径
        CheckUrlSafe();

        AuthenticateHelper.ClearLoginCache();//清掉旧的登录缓存

    }

    #region 检查可否访问后台路径
    private void CheckUrlSafe()
    {
        try
        {
            //判断当前的域名
            string dns = ConfigHelper.GetSystemConfig().DNS.ToStr();
            string currentHost = Request.Url.Host;
            if (!currentHost.Contains("127.0.0.1") && !currentHost.ToLower().Contains("localhost"))
            {
                if (!string.IsNullOrEmpty(dns))
                {
                    if (!currentHost.Contains(dns.ToLower()))
                    {
                        Response.Redirect("Error.aspx?msg=亲，当前请求的页面不存在");
                    }
                }
            }
        }
        catch
        {
            Response.Redirect("Error.aspx?msg=亲，当前请求的页面不存在");
        }
    }
    #endregion

}