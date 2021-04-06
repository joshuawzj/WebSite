using System;
using Whir.Config;
using Whir.Framework;

public partial class Whir_System_Module_Security_ChangePwd : System.Web.UI.Page
{
    public string SystemPath = WebUtil.Instance.AppPath() + AppSettingUtil.GetString("SystemPath");
    public string UploadFilePath = WebUtil.Instance.AppPath() + AppSettingUtil.GetString("UploadFilePath");
    public Whir.Config.Models.SystemConfig SystemConfig = ConfigHelper.GetSystemConfig();

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}