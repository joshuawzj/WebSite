/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：PictureConfig.aspx.cs
 * 文件描述：水印设置页面
 */
using System;
using System.Web.UI.WebControls;
using System.Drawing.Text;
using System.Drawing;

using Whir.ezEIP.Web;
using Whir.Config;
using Whir.Config.Models;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class Whir_System_Module_Config_Picture : SysManagePageBase
{

    public string FontOption { get; set; }

    public PictureConfig PictureConfig { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            JudgePagePermission(IsCurrentRoleMenuRes("25"));
            InstalledFontCollection ifc = new InstalledFontCollection();
            FontFamily[] ff = ifc.Families;
            foreach (FontFamily f in ff)
            {
                FontOption += "<option value=" + f.Name.ToLang() + ">" + f.Name.ToLang() + "</option>";
            }
            LoadPictureConfigData();
        }
    }



    private void LoadPictureConfigData()
    {
        PictureConfig = ConfigHelper.GetPictureConfig();

    }

}
