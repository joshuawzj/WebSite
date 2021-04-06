/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：generationcolumnspage.aspx.cs
 * 文件描述：生成栏目内容页
 */
using System;
using System.Web.UI.WebControls;

using Whir.ezEIP.Web;

public partial class whir_system_module_release_generationcolumnspage : SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        Response.Write("静态置标使用，暂保留此页面！");
        Response.End();
    }

    /// <summary>
    /// 栏目文章生成方法
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Generat_Command(object sender, CommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "GeneratByNew":
                break;
            case "GeneratByDate":
                break;
            case "GeneratByIDs":
                break;
            case "GeneratByFirst":
                break;
            case "btnGeneratByAll":
                break;
            default:
                break;
        }
    }
}