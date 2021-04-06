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
            JudgePagePermission(IsDevUser);
            BindTemplateFileList();
            BindIncludeFileList();
        }
    }
    
    //绑定当前Template站点的模板文件列表
    private void BindTemplateFileList()
    {
        string templatePath = "{0}{1}/template/".FormatWith(AppName, CurrentSitePath);
        string templateFullPath = Server.MapPath(templatePath);
        if (Directory.Exists(templateFullPath))
        {
            var files = Directory.GetFiles(templateFullPath);
            AllTemplateJson = files.Select(p => new
            {
                fileName = new FileInfo(p).Name,
                filePath = new FileInfo(p).Name,
                fileSize = new FileInfo(p).Length/1024 + "kb",
                createDate = File.GetCreationTime(p).ToString("yyyy-MM-dd HH:mm:ss"),
                lastWriteDate = File.GetLastWriteTime(p).ToString("yyyy-MM-dd HH:mm:ss")
            }).ToJson();
        }
    }

    //绑定当前Include站点的模板文件列表
    private void BindIncludeFileList()
    {
        string templatePath = "{0}{1}/template/include/".FormatWith(AppName, CurrentSitePath);
        string templateFullPath = Server.MapPath(templatePath);
        if (Directory.Exists(templateFullPath))
        {
            var files = Directory.GetFiles(templateFullPath);
            AllIncludeJson = files.Select(p => new
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