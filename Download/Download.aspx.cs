/*
 * Copyright © 2009-2018 万户网络技术有限公司
 * 文 件 名：Download.aspx.cs
 * 文件描述：文件下载页面/10/8
 */
using System;
using System.IO;
using System.Web;
using Whir.ezEIP;
using Whir.Service;
using Whir.Framework;
using System.Text;

public partial class Download_Download : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var model = ServiceFactory.DownloadService.GetDownloadByGuId(RequestUtil.Instance.GetString("Guid"));
            if (model != null)
            {
                string filePath = Server.MapPath(UploadFilePath + model.Path);
                if (CheckFilePath(filePath))
                {
                    if (File.Exists(filePath))
                    {
                        ServiceFactory.DownloadService.SetDownloadsAndDelete(model);
                        //处理特殊字符
                        string fileName = model.FileName;
                        string browser = HttpContext.Current.Request.UserAgent.ToUpper();
                        if (!(browser.Contains("FIREFOX") || browser.Contains("CHROME") || browser.Contains("SAFARI"))) {
                            fileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8).Replace("\\+", "%20");//解决加号变空格
                        }

                        //以字符流的形式下载文件
                        using (FileStream fs = new FileStream(filePath, FileMode.Open))
                        {
                            byte[] bytes = new byte[(int)fs.Length];
                            fs.Read(bytes, 0, bytes.Length);
                            fs.Close();
                            Response.Charset = "GB2312";
                            Response.ContentType = "application/octet-stream;";
                            //通知浏览器下载文件而不是打开
                            Response.AddHeader("Content-Disposition",
                                               "attachment; filename=\""+ fileName + "\""
                                               );//解决 FIREFOX有空格保存文件名被截断
                            if (bytes.Length > 0)
                            {
                                Response.BinaryWrite(bytes);
                            }
                            Response.Flush();
                            Response.End();
                        }
                    }
                    else
                    {
                        WriteMsg("对不起，文件不存在！");
                    }
                }
                else
                {
                    WriteMsg("对不起，访问出错，系统文件不允许下载！");
                }
            }
            else {
                WriteMsg("对不起，没有找到文件，请联系网站管理员！");
            }
        }
        catch (Exception ex)
        {
            WriteMsg("对不起，访问出错，请联系网站管理员！");
        }
    }
    public void WriteMsg(string msg)
    {
        Response.Clear();
        string template = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>
                    <html xmlns='http://www.w3.org/1999/xhtml'>
                    <head>
                        <title>{$msg}</title>
                        <style type='text/css'>
                            .errMsg { width: 80%; background-color: lightgoldenrodyellow; color: brown;padding: 15px; font-size: 12px; margin: 20px auto; border-radius: 5px; }
                        </style>
                    </head>
                    <body>
                        <div class='errMsg'>
                             {$msg}
                        </div>
                    </body>
                    </html>";
        template = template.Replace("{$msg}", msg);
        Response.Write(template);
        HttpContext.Current.ApplicationInstance.CompleteRequest();
    }

    /// <summary>
    /// 限制文件下载
    /// </summary> 
    /// <param name="filePath"></param>
    public static bool CheckFilePath(string filePath)
    {
        string[] forbidenDir = { "Ajax", "App_Code", "App_Data", "Bin", "cn", "config", "Editor", "label", "mobile", "Payment", "res", "Shop", "UserControl", "whir_system" };
        string[] forbidenFiles = { "Global.asax", "SqlMap.config", "web.config", "Release.sln", "robots.txt" };

        //第一步：检查后缀名
        bool allow = true;
        string extension = Path.GetExtension(filePath);
        if (extension != null)
        {
            extension = extension.Replace(".", "").ToLower();
            switch (extension)
            {
                case "aspx":
                case "ashx":
                case "config":
                case "asax":
                case "sln":
                case "cs":
                    allow = false;
                    break;
            }
        }

        if (allow)
        {
            //第二步：检查文件夹
            foreach (string dir in forbidenDir)
            {
                if (filePath.ToLower().Replace("\\", "/").IndexOf("/" + dir.ToLower() + "/", StringComparison.Ordinal) > 0)
                {
                    allow = false;
                    break;
                }
            }
            if (allow)
            {
                //第三步：检查文件
                foreach (string file in forbidenFiles)
                {
                    if (filePath.ToLower().EndsWith(file.ToLower()))
                    {
                        allow = false;
                        break;
                    }
                }
            }
        }
        return allow;
    }
}