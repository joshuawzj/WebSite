using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Whir.Framework;

/// <summary>
///前台用户控件基类
/// </summary>
public class FrontUserControl : System.Web.UI.UserControl
{
    /// <summary>
    /// 站点根目录, 如: "/"或"/WebSite/"
    /// </summary>
    public string AppName { get { return WebUtil.Instance.AppPath(); } }

   

    /// <summary>
    /// 站点根目录，绝对路径
    /// </summary>
    public string AppAbsoluteRootPath
    {
        get
        {
            Uri uri = HttpContext.Current.Request.Url;
            string port = uri.Port == 80 ? string.Empty : ":" + uri.Port;
            if (HttpContext.Current.Request.ApplicationPath != null)
            {
                string webUrl = string.Format("{0}://{1}{2}", uri.Scheme, uri.Host, port) +
                                (HttpContext.Current.Request.ApplicationPath).Replace("//", "/");
                return webUrl;
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// 上传文件路径
    /// </summary>
    public string UploadFilePath { get { return AppName + AppSettingUtil.AppSettings["UploadFilePath"]; } }

    /// <summary>
    /// 转换文件路径为Web绝对路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetWebUrl(string path)
    {
        path = path.StartsWith("/") ? path.Substring(1) : path;
        Uri uri = HttpContext.Current.Request.Url;
        string port = uri.Port == 80 ? string.Empty : ":" + uri.Port;
        string webUrl = string.Format("{0}://{1}{2}", uri.Scheme, uri.Host, port) +
                        (HttpContext.Current.Request.ApplicationPath + "/" + path).Replace("//", "/");
        return webUrl;
    }
}