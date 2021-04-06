using System;

using Whir.Framework;

public partial class whir_system_ajax_extension_GetWebPageCode : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = RequestUtil.Instance.GetFormString("weburl");
        Response.Write(HttpOperater.GetHtml(url));
    }
}