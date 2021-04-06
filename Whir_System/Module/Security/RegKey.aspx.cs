/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：regkey.aspx.cs
* 文件描述：授权页。 
*/

using System;

using Whir.Framework;
using Whir.Authorize;
using Whir.Config;
using System.IO;

public partial class whir_system_module_security_regkey : System.Web.UI.Page
{
    public string SystemPath = WebUtil.Instance.AppPath() + AppSettingUtil.GetString("SystemPath");
    public string UploadFilePath = WebUtil.Instance.AppPath() + AppSettingUtil.GetString("UploadFilePath");
    public Whir.Config.Models.SystemConfig SystemConfig = ConfigHelper.GetSystemConfig();

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var code = txtCode.Text.Trim();
        if (code.IsEmpty())
        {
            lbTips.Text = "请输入授权码";
            return;
        }
        if (code.Length > 10000)
        {
            lbTips.Text = "请输入正确的授权码";
            return;
        }

        try
        {
            if (CheckReg.IsReg(code))
            {  
                AppSettingUtil.SetAppSettings("RegKey", code);
                AppSettingUtil.SetAppSettings("IsOnline", "1");
				AppSettingUtil.SetAppSettings("EnableErrorPage", "1");
                EditErrorConfig();
                Response.Redirect("../../login.aspx");
            }
            lbTips.Text = "授权码错误";
        }
        catch
        {
            lbTips.Text = "保存授权码出错(可能没有更改文件的权限)";
        }
    }

    /// <summary>
    /// 自动修改测试站上的404页面、error页面的路径
    /// </summary>
    public void EditErrorConfig()
    {       
        var projectNum = AppSettingUtil.GetString("ProjectNum");
        
        if (projectNum.IsNotEmpty())
        {
            if (File.Exists(Server.MapPath("~/web.config")))
            {
                var str = File.ReadAllText(Server.MapPath("~/web.config"));
                str = str.Replace("\"/" + projectNum + "/404.aspx\"", "\"/404.aspx\"");
                str = str.Replace("\"/" + projectNum + "/Error.aspx\"", "\"/Error.aspx\"");
                File.WriteAllText(Server.MapPath("~/web.config"), str);
            }
            if (File.Exists(Server.MapPath("~/Config/CustomErrors.config")))
            {
                var str = File.ReadAllText(Server.MapPath("~/Config/CustomErrors.config"));
                str = str.Replace("\"/" + projectNum + "/404.aspx\"", "\"/404.aspx\"");
                File.WriteAllText(Server.MapPath("~/Config/CustomErrors.config"), str);
            }
            AppSettingUtil.SetAppSettings("ProjectNum", "");
        }
    }


}