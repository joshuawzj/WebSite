/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：resource.aspx.cs
 * 文件描述：获取站点的公共资源页面
 */

using System;

using Whir.Framework;

public partial class Whir_System_Module_Label_Resource : Whir.ezEIP.Web.SysManagePageBase
{
    public string Content { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            JudgePagePermission(IsDevUser);
            GetLableInfo();
        }
    }

    /// <summary>
    /// 获取置标的信息
    /// </summary>
    private void GetLableInfo()
    {
        string labelname = "resource";
        string strLabel = string.Empty;

         Content = string.Format("<wtl:{0} id=\"{0}{1}\"{2}></wtl:{0}>", labelname, Rand.Instance.Number(2), strLabel).ToLower();
    }
}
