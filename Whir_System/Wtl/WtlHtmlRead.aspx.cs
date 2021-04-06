using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;

public partial class whir_system_wtl_WtlHtmlRead : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string fileName = Request.QueryString["fileName"];

        if (!string.IsNullOrEmpty(fileName))
        {
            fileName = HttpContext.Current.Server.MapPath(AppName + "cn/wtl/" + fileName);

            if (File.Exists(fileName))
            {
                string html = FileUtil.Instance.ReadFile(fileName);

                html = html.Replace("href=\"css/", "href=\"" + AppName + "cn/css/");
                html = html.Replace("src=\"js/", "src=\"" + AppName + "cn/js/");
                html = html.Replace("url(uploadfiles/", "url(" + AppName + "uploadfiles/");
                html = html.Replace("url('uploadfiles/", "url('" + AppName + "uploadfiles/");
                html = html.Replace("src=\"uploadfiles/", "src=\"" + AppName + "uploadfiles/");
                html = html.Replace("src=\"images/", "src=\"" + AppName + "cn/images/");
                Response.Write(html);
            }
            else
            {
                Response.Write("访问的页面不存在");
            }
            Response.End();
        }

    }
}