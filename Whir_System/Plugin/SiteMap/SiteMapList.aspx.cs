/*
 * Copyright © 2009-2012 万户网络技术有限公司
 * 文 件 名：sitemaplist.aspx.cs
 * 文件描述：sitemap列表页面
 * 
 * 创建标识: ketz 2013-08-19
 * 
 * 修改标识：
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using Whir.Framework;
using Whir.Domain;
using Whir.Service;
using Whir.Language;
using Whir.SiteMap;

public partial class Whir_System_Plugin_SiteMap_SiteMapList : Whir.ezEIP.Web.SysManagePageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        JudgePagePermission(IsCurrentRoleMenuRes("239"));
    }

    /// <summary>
    /// 生成SiteMap操作按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lbnAdd_Command(object sender, CommandEventArgs e)
    {
        if (!IsCurrentRoleMenuRes("369"))
        {
            string script = "<script language=\"javascript\" defer=\"defer\">whir.toastr.warning('{0}')</script>".FormatWith(NoPermissionMsg.ToLang());
            Page.ClientScript.RegisterStartupScript(this.GetType(), "", script);
        }
        else
        {
            UrlCollection urlCollection = new UrlCollection();

            string requestUrl = "http://" + HttpContext.Current.Request.Url.Host.ToStr();
            if (!Request.Url.Port.ToStr().IsEmpty())
            {
                requestUrl += ":" + Request.Url.Port.ToStr();
            }
            IList<Whir.Domain.Column> columns = ServiceFactory.ColumnService.GetList().Where(p => p.ModelId != 0 && p.ModuleMark != "" && p.CreateMode != 0).ToList();
            IList<SiteInfo> siteinfos = ServiceFactory.SiteInfoService.GetList().ToList();

            //迭代记录集
            foreach (Column column in columns)
            {
                if (column.CreateMode > 0)
                {
                    if (column.DefaultTemp.IsNotEmpty())
                    {
                        string indexUrl = requestUrl + ServiceFactory.ColumnService.GetColumnListLink(column.ColumnId, "", 1);
                        urlCollection.Add(
                            new Whir.SiteMap.Url(
                                new Uri(indexUrl),
                                    DateTime.Today,
                                    ChangeFrequency.Weekly,
                                    0.9M
                                    )
                                    );
                    }
                    if (column.ListTemp.IsNotEmpty())
                    {
                        string listUrl = requestUrl + ServiceFactory.ColumnService.GetColumnListLink(column.ColumnId, "", 2);
                        urlCollection.Add(
                            new Whir.SiteMap.Url(
                                new Uri(listUrl),
                                    DateTime.Today,
                                    ChangeFrequency.Weekly,
                                    0.9M
                                    )
                                    );
                    }
                    if (column.ContentTemp.IsNotEmpty())
                    {
                        string infoUrl = requestUrl + ServiceFactory.ColumnService.GetColumnListLink(column.ColumnId, "", 3);
                        urlCollection.Add(
                            new Whir.SiteMap.Url(
                                new Uri(infoUrl),
                                    DateTime.Today,
                                    ChangeFrequency.Weekly,
                                    0.9M
                                    )
                                    );
                    }
                }
            }

            //判断文件夹是否存在，不存在却生成路径
            if (!Directory.Exists(Server.MapPath(UploadFilePath + "file")))
            {
                Directory.CreateDirectory(Server.MapPath(UploadFilePath + "file"));
            }
            if (!File.Exists(Server.MapPath(UploadFilePath + "file/sitemap.xml")))
            {
                using (File.Create(MapPath(UploadFilePath + "file/sitemap.xml")))
                {

                }
            }

            //生成XML文件
            XmlSerializer serializer = new XmlSerializer(typeof(UrlCollection));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(Server.MapPath("~/uploadfiles/file/sitemap.xml"), Encoding.UTF8);
            //设置输出格式
            xmlTextWriter.Indentation = 4;
            xmlTextWriter.Formatting = Formatting.Indented;

            serializer.Serialize(xmlTextWriter, urlCollection);
            xmlTextWriter.Close();

            //生成HTML文件
            //BuildHTML(dtSource);


            //通下流读写把文件下载到本地
            if (!Whir.Framework.FileUtil.Instance.DownFile("sitemap.xml", Server.MapPath("~/UploadFiles/file/sitemap.xml")))
            {
                Alert("下载文件出错！".ToLang(), true);
            }
        }
    }
}