/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：UploadConfig.aspx.cs
 * 文件描述：上传文件设置页面
 */
using System;
using Whir.ezEIP.Web;
using Whir.Config;
using Whir.Config.Models;

public partial class Whir_System_Module_Config_Upload : SysManagePageBase
{
    public UploadConfig UploadConfig { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("24"));
            LoadUploadConfigData();
        }
    }

    /// <summary>
    /// 初始化配置信息
    /// </summary>
    private void LoadUploadConfigData()
    {
        UploadConfig = ConfigHelper.GetUploadConfig();

    }
}
