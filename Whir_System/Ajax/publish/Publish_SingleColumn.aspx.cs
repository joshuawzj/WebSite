using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Label;
using Whir.Service;

public partial class Whir_System_Ajax_publish_Publish_SingleColumn : SysManagePageBase
{
    public string RootPath { get; set; }
    public string HttpPath { get; set; }
    public bool IsDefault = false;
    public bool IsList = false;
    public bool IsContent = false;
    public static string LogPath = HttpContext.Current.Server.MapPath(string.Format("~/App_Data/protected/logs/{0}.log", DateTime.Now.ToString("yyyy-MM-dd")));

    public class ContentData
    {
        public string ContentPath { get; set; }
        public List<int> DataList { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            JudgeActionPermission(IsCurrentRoleMenuRes("363"), true);

            HttpPath = Request.Url.Scheme + "://" + Request.Url.Authority + "/" + AppName;
            RootPath = HttpContext.Current.Server.MapPath(AppName.Replace("//", "/"));

            int columnId = RequestUtil.Instance.GetQueryInt("columnid", 0);//负数表明是站点文件夹or 子站id 
            int subjectClassId = RequestUtil.Instance.GetQueryInt("classid", 0);  //子站类型id
            int menuId = RequestUtil.Instance.GetQueryInt("menuid", 0);  //模版子站、自定义子站
            IsDefault = RequestUtil.Instance.GetQueryInt("isdefault", 1).ToBoolean();
            IsList = RequestUtil.Instance.GetQueryInt("islist", 1).ToBoolean();
            IsContent = RequestUtil.Instance.GetQueryInt("iscontent", 1).ToBoolean();
            bool isAttach = RequestUtil.Instance.GetQueryInt("isattach", 1).ToBoolean();
            bool isHtml = RequestUtil.Instance.GetString("isHtml").ToInt(0).ToBoolean();


            List<Column> listColumn = new List<Column>();

            LabelHelper.Instance.ClearPublishStatus(); //清除发布状态的XML文件

            if ((menuId == 1 || menuId == 2) && columnId < 0)
            {
                var IndexColumn = ServiceFactory.ColumnService.GetSubjectIndexColumn(subjectClassId);
                if (IndexColumn != null)
                {
                    listColumn.Add(IndexColumn);
                }
                listColumn.AddRange(ServiceFactory.ColumnService.GetSubjectColumnList(0, subjectClassId, Math.Abs(columnId)).ToList());
                if (listColumn != null)
                {
                    SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(listColumn[0].SiteId);
                    //把include的公共文件变成ascx
                    LabelHelper.Instance.BuildInclude(999, 1, siteInfo);

                    LabelHelper.Instance.BuildSiteColumn(
                        listColumn,
                        siteInfo,
                        IsDefault,
                        IsList,
                        IsContent,
                        isAttach
                        );
                }
            }
            else
            {
                Column column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId);
                listColumn = ServiceFactory.ColumnService.GetAllChildrenColumn(column.ColumnId, column.SiteId).ToList();
                listColumn.Add(column);

                if (column != null)
                {
                    SiteInfo siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(column.SiteId);
                    //把include的公共文件变成ascx
                    LabelHelper.Instance.BuildInclude(999, 1, siteInfo);

                    LabelHelper.Instance.BuildColumn(
                        column,
                        siteInfo,
                        IsDefault,
                        IsList,
                        IsContent,
                        isAttach
                        );
                }
            }
            if (isHtml)
            {
                foreach (var column in listColumn)
                {
                    if (column != null)
                    {
                        Release(column.ColumnId);
                    }
                }
            }

            //返回存放发布状态的XML路径
            Response.Write(LabelHelper.BuildXmlPath);
        }
        catch (Exception ex)
        {
            LogHelper.Log(ex);
        }

    }


    /// <summary>
    /// 发布静态页
    /// </summary>
    public void Release(object columnId)
    {
        var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId.ToInt());

        var model = new ModelService().GetModelByColumnId(column.ColumnId);
        if (model == null || column.CreateMode != 1)
            return;

        var site = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(column.SiteId) ?? new SiteInfo();

        string sql = "Select top 100 {0}_PID From {0} Where TypeId={1} And IsDel=0  Order BY UpdateDate DESC".FormatWith(model.TableName, column.ColumnId);
        var dataList = ServiceFactory.ColumnService.Query<int>(sql).ToList();

        var httpUrl = HttpPath;
        httpUrl = httpUrl + (site.IsRootPath ? "" : site.Path + "/") + column.Path;

        var defaultPath = column.DefaultTemp.IsEmpty() ? "" : httpUrl + "/index.aspx";
        var listPath = column.ListTemp.IsEmpty() ? "" : httpUrl + "/list.aspx";
        var contentPath = column.ContentTemp.IsEmpty() ? "" : httpUrl + "/info.aspx";
        //首页
        if (!defaultPath.IsEmpty() && IsDefault)
        {

            var htmlUrl = GetHtmlUrl(defaultPath);
            CreateHtml(defaultPath, htmlUrl);

            var tempPath = "~/{0}/template/{1}".FormatWith(site.Path, column.DefaultTemp);
            tempPath = Server.MapPath(tempPath);
            if (!File.Exists(tempPath))
                return;

            //var htmlStr = File.ReadAllText(tempPath, Encoding.UTF8);

            //var pageSize = GetPageSize(htmlStr);

            //if (dataList.Count() > pageSize && pageSize != 0)
            //{
            //    var pageNum = (dataList.Count() % pageSize) == 0 ? dataList.Count() / pageSize : dataList.Count() / pageSize + 1;
            //    for (int pageIndex = 1; pageIndex <= pageNum; pageIndex++)
            //    {
            //        var path = defaultPath + "?page=" + pageIndex;
            //        htmlUrl = GetHtmlUrl(path);
            //        CreateHtml(path, htmlUrl);

            //    }
            //}
        }
        //列表页
        if (!listPath.IsEmpty() && IsList)
        {
            var tempPath = "~/{0}/template/{1}".FormatWith(site.Path, column.ListTemp);
            tempPath = Server.MapPath(tempPath);
            if (!File.Exists(tempPath))
                return;

            var path = listPath;
            var htmlUrl = GetHtmlUrl(path);
            CreateHtml(path, htmlUrl);
        }

        //详情页
        if (!contentPath.IsEmpty() && IsContent)
        {
            if (dataList.Count > 50)
            {
                int i = 0;
                int len = 20;
                while (i * len < dataList.Count)
                {
                    int count = (i + 1) * len > dataList.Count ? dataList.Count - i * len : len;
                    var tempList = dataList.GetRange(i * len, count);
                    ContentData contentData = new ContentData() { ContentPath = contentPath, DataList = tempList };
                    i++;

                    Thread t = new Thread(new ParameterizedThreadStart(MultiCreateHtml));
                    t.Start(contentData);
                    Thread.Sleep(10);
                }
            }
            else
            {
                foreach (var itemId in dataList)
                {
                    var path = contentPath + "?itemId=" + itemId;
                    var htmlUrl = GetHtmlUrl(path);
                    CreateHtml(path, htmlUrl);
                }
            }
        }

    }

    /// <summary>
    /// 创建静态html文件
    /// </summary>
    /// <param name="aspxUrl"></param>
    /// <param name="htmlUrl"></param>
    /// <returns></returns>
    public void CreateHtml(string aspxUrl, string htmlUrl)
    {
        try
        {
            // LogHelper.Log(LogPath, "正在生成静态页：" + htmlUrl);

            string aspxSourceCode = new CollectionUtils().GetHttpPageCode(aspxUrl.ToStr(), Encoding.UTF8);
            if (aspxSourceCode != "$UrlIsFalse" && aspxSourceCode != "$GetFalse")
            {
                //写入静态文件
                string html = aspxSourceCode;
                string writePath = RootPath + htmlUrl.ToStr().Replace(HttpPath, "");
                FileSystemHelper.WriteFile(writePath, html);
            }
            else
            {
                LogHelper.Log(LogPath, "抓取网页错误：" + htmlUrl);
            }

            Thread.Sleep(10);
        }
        catch (Exception ex)
        {
            LogHelper.Log(LogPath, ex);
        }
    }

    /// <summary>
    /// 多线程发布静态页
    /// </summary>
    /// <param name="data"></param>
    public void MultiCreateHtml(object data)
    {
        var contentData = data as ContentData;
        foreach (var itemId in contentData.DataList)
        {

            var path = contentData.ContentPath + "?itemId=" + itemId;
            var htmlUrl = GetHtmlUrl(path);
            CreateHtml(path, htmlUrl);

        }
    }

    /// <summary>
    /// 根据aspx地址返回html地址
    /// </summary>
    /// <param name="aspxUrl"></param>
    /// <returns></returns>
    public string GetHtmlUrl(string aspxUrl)
    {
        var url = new Uri(aspxUrl);
        var newUrl = aspxUrl.Substring(0, aspxUrl.LastIndexOf(".aspx"));

        NameValueCollection collection = HttpUtility.ParseQueryString(url.Query);
        for (int index = 0; index < collection.Count; index++)
        {
            newUrl += "_" + collection.Keys[index];
            newUrl += "_" + collection[index];
        }
        return newUrl + ".html";

    }

}