/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：template_select.aspx.cs
 * 文件描述：模板选择页面
 *          
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.ezEIP.Web;
using System.Linq;

public partial class Whir_System_Module_Column_TemplateSelector : SysManagePageBase
{

    protected string AllTemplateJson { get; set; }
    protected string AllIncludeJson { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgeOpenPagePermission(IsDevUser);
            BindTemplateFileList();
            BindIncludeFileList();
        }
    }

    //绑定当前Template站点的模板文件列表
    private void BindTemplateFileList()
    {
        string templatePath = "{0}{1}/template/".FormatWith(AppName, CurrentSitePath);
        string includePath = "{0}{1}/template/include/".FormatWith(AppName, CurrentSitePath);
        string templateFullPath = Server.MapPath(templatePath);
       
        if (Directory.Exists(templateFullPath))
        {
              var list = new List<string>();
             GetFiles(templateFullPath, list, Server.MapPath(includePath).Trim('\\'));
             AllTemplateJson = list.Select(p => new
            {
                fileName = new FileInfo(p).FullName.Replace(templateFullPath,""),
                filePath = new FileInfo(p).Name,
                fileSize = new FileInfo(p).Length / 1024 + "kb",
                createDate = File.GetCreationTime(p).ToString("yyyy-MM-dd HH:mm:ss"),
                lastWriteDate = File.GetLastWriteTime(p).ToString("yyyy-MM-dd HH:mm:ss")
            }).ToJson();
        }
    }


    /// <summary>
    /// 获取模版文件
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="list"></param>
    private void GetFiles(string dir, List<string> list, string except)
    {
        if (dir == except)
            return;
        var files = Directory.GetFiles(dir);
        var allowFiles = files.Where(p => (new FileInfo(p).Extension == ".shtml" || new FileInfo(p).Extension == ".html" || new FileInfo(p).Extension == ".xml"));
        //添加文件
        list.AddRange(allowFiles);
        //如果是目录，则递归
        var directories = new DirectoryInfo(dir).GetDirectories();
        foreach (DirectoryInfo item in directories)
        {
            GetFiles(item.FullName, list, except);
        }
         
    }

    //绑定当前Include站点的模板文件列表
    private void BindIncludeFileList()
    {
        string templatePath = "{0}{1}/template/include/".FormatWith(AppName, CurrentSitePath);
        string templateFullPath = Server.MapPath(templatePath);
        if (Directory.Exists(templateFullPath))
        {
            var list = new List<string>();
            GetFiles(templateFullPath, list, "");
            AllIncludeJson = list.Select(p => new
            {
                fileName = new FileInfo(p).Name,
                filePath = "include/" + new FileInfo(p).Name,
                fileSize = new FileInfo(p).Length / 1024 + "kb",
                createDate = File.GetCreationTime(p).ToString("yyyy-MM-dd HH:mm:ss"),
                lastWriteDate = File.GetLastWriteTime(p).ToString("yyyy-MM-dd HH:mm:ss")
            }).ToJson();
        }
    }


}