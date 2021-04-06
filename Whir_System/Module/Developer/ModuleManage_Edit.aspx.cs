/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：ModuleManage_edit.aspx.cs
 * 文件描述： 添加模块
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using Whir.Service;
using Whir.Framework;
using Whir.Config.Models;
using Whir.Domain;
using Whir.Language;


public partial class whir_system_module_developer_ModuleManage_edit : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
    /// <summary>
    /// 获取上传的模块包里与系统有同样文件名的文件
    /// </summary>
    /// <param name="folderPath">模块解压后的文件夹（绝对路径）</param>
    /// <returns></returns>
    private string GetSameSystemFileName(string folderPath)
    {
        string Result = "";//所有与系统存在相同文件名的文件
        List<string> FileList = GetFileName(folderPath);

        string SystePath = Server.MapPath("~/");
        string SystemName = "";
        foreach (string f in FileList)
        {
            folderPath = folderPath.Replace("\\", "/");
            SystemName = f.Replace(folderPath, SystePath);
            if (File.Exists(SystemName))
            {
                Result += SystemName.Replace(SystePath, "") + "</br>";
            }
        }
        return Result;
    }

    /// <summary>
    /// 获取指定文件夹所有文件（包括子目录）
    /// </summary>
    /// <param name="folderPath">绝对路径，如e:\web\uploadfiles\moduletemp\2012082518230747\</param>
    /// <returns></returns>
    private List<string> GetFileName(string folderPath)
    {
        List<string> FileList = new List<string>();

        DirectoryInfo di = new System.IO.DirectoryInfo(folderPath);
        FileInfo[] fiMore = di.GetFiles();
        DirectoryInfo[] diMore = di.GetDirectories();

        foreach (FileInfo fi in fiMore)
        {
            FileList.Add(fi.FullName.Replace("\\", "/"));
        }

        foreach (DirectoryInfo d in diMore)
        {
            FileList.AddRange(GetFileName(d.FullName));
        }

        return FileList;


    }
}

public class JsonObject
{
    public bool Result { get; set; }
    public string Msg { get; set; }
}