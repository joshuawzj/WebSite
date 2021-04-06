/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：_404.aspx.cs
 * 文件描述：自定义404页面，用于前台静态页发布
 */

using System;
using System.Web;
using Whir.Framework;
using Whir.Label.Static;
using Whir.Service;


public partial class _404 : System.Web.UI.Page{

    protected string SysPath = (WebUtil.Instance.AppPath() + AppSettingUtil.AppSettings["SystemPath"]).ToLower();

    protected void Page_Load(object sender, EventArgs e)
    {

        //通过400、404错误进来的页面，格式：http://host/404.aspx?404;http://host/123.html
        string reqPath = HttpUtility.UrlDecode(Request.QueryString.ToStr()).ToLower();//.Replace("404%3b", "").Replace("400%3b", "");
        if (reqPath.IsEmpty() || reqPath.StartsWith("aspxerrorpath"))
        {
            Response.StatusCode = 404;
            return;
        }
        reqPath = reqPath.Contains(";") ? reqPath.Split(';')[1] : reqPath;
        var url = new Uri(Server.UrlDecode(reqPath));
        reqPath = url.LocalPath;


        if (reqPath.EndsWith(".html") && !reqPath.Contains(SysPath.ToLower())) //要排除 编辑器的页面)
        {
            //安全检查
            Safe360.Procress();

            //以下两个方法只能同时开放一个
            CreateStaticPage(reqPath);
            //HtmlToAspx(reqPath);
        }
        Response.StatusCode = 404;

    }

    /// <summary>
    /// 生成静态页
    /// </summary>
    private void CreateStaticPage(string reqPath)
    {
        #region 生成静态页
        if (reqPath.IsNotEmpty())
        {
            string reqDir = Server.MapPath(reqPath);

            if (!reqPath.ToLower().Contains(SysPath) && !FileSystemHelper.IsFieldExist(reqDir))
            {
                bool writeSuccess = StaticLabelHelper.Instance.Build(reqPath);
                if (writeSuccess)
                {
                    Server.Transfer(reqPath);
                    return;
                }
                else
                    Response.StatusCode = 404;
            }
            else
                Response.StatusCode = 404;
        }
        else
            Response.StatusCode = 404;
        #endregion
    }

    /// <summary>
    /// 伪静态访问html页面
    /// </summary>
    private void HtmlToAspx(string reqPath)
    {
        #region 伪静态处理
        if (reqPath.IsNotEmpty())
        {
            bool canCreateHtml;
            string aspxPath = Whir.Label.Static.StaticLabelHelper.Instance.GetAspxPath(reqPath, out canCreateHtml);

            string filePath = aspxPath;
            if(aspxPath.Contains("?"))
                filePath = aspxPath.Substring(0, aspxPath.IndexOf('?'));
            if (!FileSystemHelper.IsFieldExist(Server.MapPath(filePath)))
            {
                Response.StatusCode = 404;
                return;
            }
            Server.Transfer(aspxPath);
            return;
        }
        else
            Response.StatusCode = 404;
        #endregion
    }

}