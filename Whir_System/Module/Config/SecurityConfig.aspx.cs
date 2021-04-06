/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：SecurityConfig.aspx.cs
 * 文件描述：系统安全配置页面
 */
using System;
using System.IO;
using Whir.Config;
using Whir.Config.Models;
using Whir.Framework;

public partial class whir_system_module_config_SecurityConfig : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 打开
    /// </summary>
    protected string Open { get; set; }

    /// <summary>
    /// 关闭
    /// </summary>
    protected string Close { get; set; }

    protected SystemConfig SystemConfig { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("23"));
        SystemConfig = ConfigHelper.GetSystemConfig();
        
    }
 
}