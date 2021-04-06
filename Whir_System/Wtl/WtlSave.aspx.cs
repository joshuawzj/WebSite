using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Framework;
using Whir.Language;

public partial class whir_system_wtl_WtlSave : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string html = Request.Form["html"];

        if (!string.IsNullOrEmpty(html))
        {
            try
            {
                string fileName = Request.Form["fileName"];
                string filePath = HttpContext.Current.Server.MapPath(AppName + "cn/template/" + fileName);
                if (File.Exists(filePath))
                {
                    html = HttpUtility.UrlDecode(html);
                    html = html.Replace(AppName + "cn/", "{$sitepath}");


                    FileUtil.Instance.WriteFile(filePath, html);


                    Response.Write(new Dictionary<string, object>
                    {
                        {"IsSuccess",true}
                    }.ToJson());
                }
                else
                {
                    Response.Write(new Dictionary<string, object>
                    {
                        {"IsSuccess",false},
                        {"Body","保存的文件不存在".ToLang()}
                    }.ToJson());
                }
            }
            catch (Exception ex)
            {
                Response.Write(new Dictionary<string, object>
                {
                    {"IsSuccess",false},
                    {"Body",ex.Message}
                }.ToJson());
            }

        }
        Response.End();
    }
}