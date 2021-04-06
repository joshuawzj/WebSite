
/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：uploadimages.aspx.cs
 * 文件描述：图片库管理页面, 上传图片选择页面
 */

using System;
using Whir.Config;
using Whir.Config.Models;


public partial class Whir_System_Module_Extension_UploadImages : Whir.ezEIP.Web.SysManagePageBase
{

    /// <summary>
    /// 可上传文件后缀名, 以英文逗号分隔开
    /// </summary>
    protected string EnableExtensionNames { get; private set; }

    /// <summary>
    /// 可上传文件后缀名, 'jpg','png','gif','bmp'
    /// </summary>
    protected string AllowPicType { get; private set; }

    /// <summary>
    /// 弹窗选文件只显示指定类型文件，格式： ".jpg,.png,.gif,.bmp"
    /// </summary>
    protected string AcceptPicType { get; private set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("32"));

        UploadConfig uploadConfig = ConfigHelper.GetUploadConfig();

        foreach (string extension in uploadConfig.AllowPicType.Split('|'))
        {
            AllowPicType += "'" + extension + "'" + ",";
            EnableExtensionNames += extension + ",";
            AcceptPicType += "." + extension + ",";
        }
        AllowPicType = AllowPicType.TrimEnd(',');
        AcceptPicType = AcceptPicType.TrimEnd(',');
    }

}