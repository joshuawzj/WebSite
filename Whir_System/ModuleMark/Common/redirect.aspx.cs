
/*
 * Copyright © 2009-2017 万户网络技术有限公司
 * 文 件 名：redirect.aspx.cs
 * 文件描述：跳转页面的媒介页面
 */

using System;

using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.Language;

public partial class whir_system_ModuleMark_common_redirect : Whir.ezEIP.Web.SysManagePageBase
{
    /// <summary>
    /// 要跳转的栏目ID
    /// </summary>
    protected int ColumnId { get; set; }
    /// <summary>
    /// 子站、专题栏目
    /// </summary>
    protected int SujbectID { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        ColumnId = WebUtil.Instance.GetQueryInt("columnid", 0);
        SujbectID = WebUtil.Instance.GetQueryInt("subjectid", 0);

        Module module = ServiceFactory.ModuleService.GetModuleByColumnID(ColumnId);
        if (module != null)
        {
            string url = string.Empty;
            if (SujbectID == 0)
            {
                url = module.ModuleUrl.Contains("?")
                                ? "{0}&columnid={1}&time={2}".FormatWith(module.ModuleUrl, ColumnId, DateTime.Now.Millisecond)
                                : "{0}?columnid={1}&time={2}".FormatWith(module.ModuleUrl, ColumnId, DateTime.Now.Millisecond);
            }
            else
            {
                url = module.ModuleUrl.Contains("?")
                ? "{0}&columnid={1}&subjectid={2}&time={3}".FormatWith(module.ModuleUrl, ColumnId, SujbectID, DateTime.Now.Millisecond)
                : "{0}?columnid={1}&subjectid={2}&time={3}".FormatWith(module.ModuleUrl, ColumnId, SujbectID, DateTime.Now.Millisecond);
            }
            Response.Redirect(SysPath + url);
        }
        else
        {
            Column model = ServiceFactory.ColumnService.SingleOrDefault<Column>(ColumnId);
            if (model != null && model.OutUrl.ToStr() != "")//外部链接跳转
            {
                if (model.OutUrl.ToStr().IsUrl())
                {
                    Response.Redirect(model.OutUrl.ToStr().Trim());
                }
                else
                {
                    Response.Redirect(RequestUtil.Instance.GetHttpUrl() + model.OutUrl.ToStr().Trim());
                }

            }
        }

        litRedirect.Text = "未找到对应的页面".ToLang();
    }
}