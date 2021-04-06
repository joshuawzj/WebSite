using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
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

public partial class Whir_System_Ajax_publish_AutoPublishHtmlPage : SysManagePageBase
{
    public string RootPath { get; set; }
    public string HttpPath { get; set; }
    public int SiteId { get; set; }
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
            string columnIDs = RequestUtil.Instance.GetString("columnids");
            SiteId = RequestUtil.Instance.GetString("siteid").ToInt(0);
            IsDefault = RequestUtil.Instance.GetString("isdefault").ToInt(1).ToBoolean();
            IsList = RequestUtil.Instance.GetString("islist").ToInt(1).ToBoolean();
            IsContent = RequestUtil.Instance.GetString("iscontent").ToInt(1).ToBoolean();

            string[] arrColumnId = columnIDs.Split(',').Where(p => !p.Trim().IsEmpty()).ToArray(); //内容不为空

            bool isHtml = RequestUtil.Instance.GetString("isHtml").ToInt(0).ToBoolean();
            if (isHtml)
            {
                foreach (string strColumnId in arrColumnId as string[])
                {
                    int columnId = strColumnId.ToInt(0);
                    var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId);
                    if (column != null)
                    {
                        Release(column.ColumnId);
                        Thread.Sleep(10);
                    }
                }
            }

            //Response.Write("完成");
        }
        catch (Exception ex)
        {
            LogHelper.Log(LogPath, ex);
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

        var site = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(SiteId) ?? new SiteInfo();

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

            //var htmlStr = File.ReadAllText(tempPath, Encoding.UTF8);

            //var pageSize = GetPageSize(htmlStr);

            //if (dataList.Count() > pageSize && pageSize != 0)
            //{
            //    var pageNum = (dataList.Count() % pageSize) == 0 ? dataList.Count() / pageSize : dataList.Count() / pageSize + 1;
            //    for (int pageIndex = 1; pageIndex <= pageNum; pageIndex++)
            //    {
            //        var path = listPath + "?page=" + pageIndex;
            //        var htmlUrl = GetHtmlUrl(path);
            //        CreateHtml(path, htmlUrl);

            //    }
            //}
        }

        //详情页
        if (!contentPath.IsEmpty() && IsContent)
        {
            if (dataList.Count > 25)
            {
                int i = 0;
                int len = 25;
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
    /// 创建静态html文件
    /// </summary>
    /// <param name="aspxUrl"></param>
    /// <param name="htmlUrl"></param>
    /// <returns></returns>
    public void CreateHtml(string aspxUrl, string htmlUrl)
    {
        try
        {

            if (HttpContext.Current != null)
            {
                LogHelper.Log(LogPath, "11111正在生成静态页：" + htmlUrl);
                string writePath = RootPath + htmlUrl.ToStr().Replace(HttpPath, "");
                StreamWriter sw = new StreamWriter(writePath, false, Encoding.UTF8);
                HttpContext.Current.Server.Execute("/" + aspxUrl.Replace(HttpPath, ""), sw);
                sw.Close();
            }
            else
            {
                LogHelper.Log(LogPath, "22222正在生成静态页：" + htmlUrl);
                var MyWebClient = new WebClient();
                MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                MyWebClient.Encoding = Encoding.UTF8;
                MyWebClient.DownloadStringAsync(new Uri(aspxUrl));//.DownloadString(url);
                MyWebClient.DownloadStringCompleted += MyWebClient_DownloadStringCompleted;
                MyWebClient.BaseAddress = RootPath + htmlUrl.ToStr().Replace(HttpPath, "");
                MyWebClient.Dispose();

                //string aspxSourceCode = new CollectionUtils().GetHttpPageCode(aspxUrl.ToStr(), Encoding.UTF8);
                //if (aspxSourceCode != "$UrlIsFalse" && aspxSourceCode != "$GetFalse")
                //{
                //    //写入静态文件
                //    string html = aspxSourceCode;
                //    string writePath = RootPath + htmlUrl.ToStr().Replace(HttpPath, "");
                //    FileSystemHelper.WriteFile(writePath, html);
                //}
                //else
                //{
                //    LogHelper.Log(LogPath,"抓取网页错误：" + htmlUrl);
                //}
            }
            Thread.Sleep(10);
        }
        catch (Exception ex)
        {
            LogHelper.Log(LogPath, ex);
        }
    }

    private void MyWebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {

        //写入静态文件
        string html = e.Result;
        string writePath = ((System.Net.WebClient)sender).BaseAddress;
        FileSystemHelper.WriteFile(writePath, html);
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

    /// <summary>
    /// 获取置标分页数
    /// </summary>
    /// <param name="htmlStr">根据url抓取的html所有标记</param>
    /// <returns></returns>
    public int GetPageSize(string htmlStr)
    {
        List<string> labelList = new List<string>();
        Regex artReg = new Regex(@"<wtl:pager(.*?)pagesize=""([0-9]+)""", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        if (artReg.IsMatch(htmlStr.ToLower()))
        {
            MatchCollection matches = artReg.Matches(htmlStr.ToLower());
            foreach (Match item in matches)
            {
                return item.Groups[2].Value.ToInt(0);
            }
        }
        return 0;
    }


}
 
 