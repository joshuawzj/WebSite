/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：filemange.aspx.cs
 * 文件描述：文件库管理页面, 上传文件选择页面
 *
 */


using System;
using Whir.Config;
using Whir.Config.Models;


public partial class Whir_System_Plugin_FileManage_FileManage : Whir.ezEIP.Web.SysManagePageBase
{


    /// <summary>
    /// 可上传文件后缀名, 以英文逗号分隔开
    /// </summary>
    protected string EnableExtensionNames { get; private set; }
    /// <summary>
    /// 弹窗选文件只显示指定类型文件，格式： ".jpg,.png,.gif,.bmp"
    /// </summary>
    protected string AcceptType { get; private set; }

    /// <summary>
    /// 可上传文件后缀名, 'doc','xls','txt'
    /// </summary>
    protected string AllowFileType { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("15"));

        UploadConfig uploadConfig = ConfigHelper.GetUploadConfig();

        foreach (string extension in uploadConfig.AllowFileType.Split('|'))
        {
            AllowFileType += "'" + extension + "'" + ",";
            EnableExtensionNames += extension + ",";
            AcceptType += "." + extension + ",";
        }
        AllowFileType = AllowFileType.TrimEnd(',');
        AcceptType = AcceptType.TrimEnd(',');
    }

}