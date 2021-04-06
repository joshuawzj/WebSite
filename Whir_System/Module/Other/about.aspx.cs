/*
* Copyright(c) 2009-2012 万户网络
* 文 件 名：about.aspx.cs
* 文件描述：关于EIP页面。 

*/

using System;
using System.Configuration;

public partial class whir_system_module_Other_about : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("266"));
        litVer.Text = "Whir ezEIP Website System " + ConfigurationManager.AppSettings["Version"];
      
    }
}