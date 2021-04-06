/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：ModuleManage_readme.aspx.cs
 * 文件描述： 查看插件的readme.txt文件
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Whir.Service;
using Whir.Framework;
using Whir.Domain;
using Whir.Language;

public partial class whir_system_module_developer_ModuleManage_readme : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsDevUser);
        int ID = RequestUtil.Instance.GetQueryInt("pluginid",0);
        Plugin Model = ServiceFactory.PluginService.SingleOrDefault<Plugin>(ID);
        if (Model != null)
        {
            string Path = HttpContext.Current.Server.MapPath("~/") + AppSettingUtil.GetString("SystemPath") + "Plugin/" + Model.ModuleFolder + "/readme.txt";
            if (FileSystemHelper.IsFieldExist(Path))
            {
                Response.Write(FileSystemHelper.ReadFile(Path));
            }
            else
            {
                Response.Write("没找到该插件的说明文档".ToLang());
            }
        }
        else
        {
            Response.Write("没找到该插件的说明文档".ToLang());

        }
    }
}