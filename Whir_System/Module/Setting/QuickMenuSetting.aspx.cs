/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：seosetting.aspx.cs
 * 文件描述：基本配置页面
 *
 *          
 */

using System;

using Whir.Config;
using Whir.Config.Models;

public partial class Whir_System_Module_Setting_QuickMenuSetting : Whir.ezEIP.Web.SysManagePageBase
{
   
    protected SystemConfig SystemConfig { get; set; }
 
    
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("378"));
        BindSysSetting();
    }

    

    //绑定系统配置
    private void BindSysSetting()
    {
        SystemConfig = ConfigHelper.GetSystemConfig();
        
         
    }
 
   

}