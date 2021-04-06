/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：seosetting.aspx.cs
 * 文件描述：基本配置页面
 *
 *          1. 站点群的SEO配置, 版权和备案号配置
 *          2. 系统基本配置, 后台登录页面LOGO, 后台主页面LOGO, 分页大小, 分页范围等配置
 */

using System;

using Whir.Config;
using Whir.Config.Models;
using Whir.Framework;

public partial class Whir_System_Module_Setting_Setting : Whir.ezEIP.Web.SysManagePageBase
{
   
    protected SystemConfig SystemConfig { get; set; }
    
    protected bool IsRemoveColor = AppSettingUtil.AppSettings["IsRemoveColor"].ToBoolean();
    protected string EditorCode { get; set; }
    
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("265"));
        BindAuthorize();  //获取ewebeditor编辑器的授权码
        
        BindSysSetting();
    }

    

    //绑定系统配置
    private void BindSysSetting()
    {
        SystemConfig = ConfigHelper.GetSystemConfig();
        
         
    }
 
    /// <summary>
    /// 获取EwebEditor的授权码
    /// </summary>
    private void BindAuthorize()
    {
        string sFileName = AppName + "Editor/EWebEditor/ASPX/config.aspx";
        string str = EWebEditorHelper.ReadFile(sFileName);
        EWebEditorHelper.sLicense = EWebEditorHelper.GetConfigString("License", str);
        EditorCode = EWebEditorHelper.InHTML(EWebEditorHelper.sLicense).ToStr();
    }

  

}