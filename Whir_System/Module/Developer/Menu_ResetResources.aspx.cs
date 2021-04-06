/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：menu_resetResources.aspx.cs
 * 文件描述： 重置菜单资源
 */

using System;
using System.Web.UI;

using Whir.Language;

public partial class whir_system_module_extension_menu_resetResources : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
    }
    /// <summary>
    /// 开始重置
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReset_Click(object sender, EventArgs e)
    {
        //bool Result = Whir.Security.ServiceFactory.ResourcesService.ResetMenuResources();
        //if (Result)
        //{
        //    ScriptManager.RegisterStartupScript(upReset, this.GetType(), "upReset", "TipMessage('" + "重置完成".ToLang() + "');", true);
        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(upReset, this.GetType(), "upReset2", "TipError('" + "部分操作失败".ToLang() + "');", true);
        //}

    }
}