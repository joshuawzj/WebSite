/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：Download.aspx.cs
 * 文件描述：文件下载页面/10/8
 *          zhangqs 2014-5-7 10:00:53 修改：限制文件下载类型和范围；
 */
using System;
using System.IO;
using System.Web;
using Whir.Framework;

public partial class whir_system_module_extension_Download : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //文件目录
        string fileDirc = Server.MapPath(RequestUtil.Instance.GetString("path"));
        //文件名称
        string fileName = RequestUtil.Instance.GetString("name");
        string fileShortName = Path.GetFileName(fileName);
        //文件地址
        string filePath = fileDirc + fileName;
        if (!string.IsNullOrEmpty(fileDirc) && !string.IsNullOrEmpty(fileName))
        {
            if (CheckFilePath(filePath))
            {
                if (File.Exists(filePath))
                {
                    //以字符流的形式下载文件
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        byte[] bytes = new byte[(int)fs.Length];
                        fs.Read(bytes, 0, bytes.Length);
                        fs.Close();
                        Response.ContentType = "application/octet-stream";
                        //通知浏览器下载文件而不是打开
                        Response.AddHeader("Content-Disposition",
                                           "attachment; filename=" +
                                           HttpUtility.UrlEncode(fileShortName, System.Text.Encoding.UTF8));
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
        Response.End();
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