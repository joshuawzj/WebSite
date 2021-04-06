using System;
using System.IO;
using System.Web.UI;
using Whir.Domain;
using Whir.Framework;
using Whir.Language;
using Whir.Service;
using Whir.ezEIP.Web.HttpModules;

public partial class whir_system_ajax_FontIndexRedirect : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Clear();
        SiteInfo site = new SiteInfoService().SingleOrDefault<SiteInfo>(SiteInfoHelper.SiteInfo.SiteId) ?? new SiteInfo();
        string errMsg;
        string index = "~/";

        if (!site.IsDefault)
        {
            if (site.CreateMode == 1)
            {
                index += site.Path + "/index.html";
            }
            else if (site.CreateMode == 2)
            {
                index += site.Path + "/index.aspx";
            }
            else
            {
                errMsg = BaseHttpModule.GetErrMsg("系统参数配置错误",
                                                  "对不起，您当前站点首页配置为不生成，您不能访问网站首页，请到后台：\"内 容 > 栏目 > 栏目管理 > 网站首页\"修改生成设置".
                                                      ToLang());
                Response.Write(errMsg);
                Response.End();
            }
        }
        else
        {
            if (site.CreateMode == 1)
            {
                index += "index.html";
            }
            else if (site.CreateMode == 2)
            {
                index += "index.aspx";
            }
            else
            {
                errMsg = BaseHttpModule.GetErrMsg("系统参数配置错误",
                                                  "对不起，您当前站点首页配置为不生成，您不能访问网站首页，请到后台：“内 容 > 栏目 > 栏目管理 > 网站首页”修改生成设置".
                                                      ToLang());
                Response.Write(errMsg);
                Response.End();
            }
        }
        if (index.EndsWith(".html"))
        {
            Response.Redirect(index);
        }
        else
        {
            if (File.Exists(Server.MapPath(index)))
            {
                Response.Redirect(index);
            }
            else
            {
                errMsg = BaseHttpModule.GetErrMsg("网站首页尚未发布",
                                                  "对不起，网站首页尚未发布，您不能访问网站首页，请到后台：“内 容 > 生成 > 生成整站”进行发布操作。".ToLang());
                Response.Write(errMsg);
            }
        }
        Response.End();
    }
}