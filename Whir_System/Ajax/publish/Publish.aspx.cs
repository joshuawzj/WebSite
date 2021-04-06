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

public partial class Whir_System_Ajax_publish_Publish : SysManagePageBase
{
    public string RootPath { get; set; }
    public string HttpPath { get; set; }
    public int SiteId { get; set; }
    public bool IsDefault = false;
    public bool IsList = false;
    public bool IsContent = false;

    public static List<string> AllPageList = new List<string>();

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

            HttpPath = WebUtil.Instance.AppAbsoluteRootPath()+"/";
            RootPath = HttpContext.Current.Server.MapPath(AppName.Replace("//", "/"));
            string columnIDs = RequestUtil.Instance.GetString("columnid");
            SiteId = RequestUtil.Instance.GetString("siteid").ToInt(0);
            bool isHome = RequestUtil.Instance.GetString("ishome").ToInt(1).ToBoolean();
            IsDefault = RequestUtil.Instance.GetString("isdefault").ToInt(1).ToBoolean();
            IsList = RequestUtil.Instance.GetString("islist").ToInt(1).ToBoolean();
            IsContent = RequestUtil.Instance.GetString("iscontent").ToInt(1).ToBoolean();
            bool isAttach = RequestUtil.Instance.GetString("isattach").ToInt(1).ToBoolean();
            bool isInclude = RequestUtil.Instance.GetString("isInclude").ToInt(1).ToBoolean();
            bool isSubColumns = RequestUtil.Instance.GetString("isSubColumns").ToInt(0).ToBoolean();
            bool isHtml = RequestUtil.Instance.GetString("isHtml").ToInt(0).ToBoolean();

            var siteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(SiteId);
            LabelHelper.Instance.ClearPublishStatus(); //清除发布状态的XML文件

            var arrColumnId = columnIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(); //内容不为空
            int count = 0;
            if (IsDefault || IsList || IsContent)
                count = arrColumnId.Count;
            int index = 0;
            List<Column> listColumn = new List<Column>();
            if (columnIDs == "0")
            {
                #region 发布整站
                listColumn = ServiceFactory.ColumnService.GetAllChildrenColumn(0, SiteId).ToList();
                if (isSubColumns)
                {
                    List<Column> subColumns = ServiceFactory.ColumnService.GetSubjectAndSubsiteColumns(SiteId);
                    //站点下专题、模版子站、自定义子站所有栏目
                    listColumn.AddRange(subColumns);
                }
                arrColumnId = listColumn.Select(p => p.ColumnId.ToStr()).ToList();
                count = listColumn.Count;
                //把include的公共文件变成ascx，首页
                count += 2;
                IList<Attached> listAttached = ServiceFactory.AttachedService.GetList("SiteId=" + SiteId);
                foreach (Attached attach in listAttached)
                {
                    count++;
                    index = index + 1;
                    LabelHelper.Instance.BuildAttach(count, index, attach.AttachedId);
                }

                index = index + 1;
                LabelHelper.Instance.BuildInclude(count, index, siteInfo);

                index = index + 1;
                LabelHelper.Instance.BuildHome(count, index, siteInfo);

                //生成栏目相关
                foreach (Column column in listColumn)
                {
                    index = index + 1;
                    if (column != null)
                    {
                        LabelHelper.Instance.BuildColumn(
                            count,
                            index,
                            column,
                            siteInfo,
                            true,
                            true,
                            true,
                            false
                            );
                    }
                }
                #endregion
            }
            else
            {
                //把include的公共文件变成ascx
                if (isInclude)
                {
                    count += 1;
                }
                if (isHome)
                {
                    //发布站点首页
                    count += 1;
                }

                if (isInclude)
                {
                    index = index + 1;
                    LabelHelper.Instance.BuildInclude(count, index, siteInfo);
                }

                if (isHome)
                {
                    index = index + 1;
                    LabelHelper.Instance.BuildHome(count, 2, siteInfo);
                }

                //生成栏目相关
                listColumn = ServiceFactory.ColumnService.GetListByColumnIds(arrColumnId.Select(p => p.ToInt()).ToArray()).ToList();
                foreach (var column in listColumn)
                {
                    index = index + 1;
                    if (column != null)
                    {
                        LabelHelper.Instance.BuildColumn(
                            count,
                            index,
                            column,
                            siteInfo,
                            IsDefault,
                            IsList,
                            IsContent,
                            isAttach
                            );
                    }
                }
            }
            Thread.Sleep(50);

            #region 生成静态页操作

            if (isHtml)
            {
                AllPageList = new List<string>();
                LogHelper.Log("开始生成静态页：" + string.Join(",", arrColumnId));
                if (isHome)
                {
                    if (siteInfo.IsDefault)
                        CreateHtml("/index.aspx", "/index.html");
                    else
                        CreateHtml(siteInfo.Path + "/index.aspx", siteInfo.Path + "/index.html");
                }
                foreach (var column in listColumn)
                {
                    if (column != null)
                    {
                        Release(column.ColumnId);
                    }
                }
                AllPageList = AllPageList.Distinct().ToList();
                var len = 50;
                if (AllPageList.Count > len)
                {
                    len = AllPageList.Count / 20;//最多同时跑20条线程
                    int i = 0;
                    while (i * len <= AllPageList.Count)
                    {
                        count = (i + 1) * len > AllPageList.Count ? AllPageList.Count - i * len : len;
                        var tempList = AllPageList.GetRange(i * len, count);

                        i++;
                        Thread t = new Thread(new ParameterizedThreadStart(CreateHtml));
                        t.Start(tempList);
                    }
                }
                else
                {
                    foreach (var aspxUrl in AllPageList)
                    {
                        var htmlUrl = GetHtmlUrl(aspxUrl);
                        CreateHtml(aspxUrl, htmlUrl);
                    }
                }

            }

            #endregion

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
        try
        {
            var column = ServiceFactory.ColumnService.SingleOrDefault<Column>(columnId.ToInt());

            var model = new ModelService().GetModelByColumnId(column.ColumnId);
            if (model == null || column.CreateMode != 1)
                return;

            var site = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(SiteId) ?? new SiteInfo();
            //获取最近更新的50条记录和排序在最前面的50条记录
            string sql = @"Select top 50 {0}_PID From {0} Where TypeId={1} And IsDel=0  Order BY UpdateDate DESC"
                          .FormatWith(model.TableName, column.ColumnId);
            var dataList = ServiceFactory.ColumnService.Query<int>(sql).ToList();
            sql = @"Select top 50 {0}_PID From {0} Where TypeId={1} And IsDel=0  Order BY Sort DESC,Createdate DESC"
                          .FormatWith(model.TableName, column.ColumnId);
            dataList.AddRange(ServiceFactory.ColumnService.Query<int>(sql).ToList());

            var httpUrl = HttpPath;
            httpUrl = httpUrl + (site.IsRootPath ? "" : site.Path + "/") + column.Path;

            var defaultPath = column.DefaultTemp.IsEmpty() ? "" : httpUrl + "/index.aspx";
            var listPath = column.ListTemp.IsEmpty() ? "" : httpUrl + "/list.aspx";
            var contentPath = column.ContentTemp.IsEmpty() ? "" : httpUrl + "/info.aspx";
            //首页
            if (!defaultPath.IsEmpty() && IsDefault)
            {               
                AllPageList.Add(defaultPath);
            }

            //列表页 只生成列表首页，不知道where条件
            if (!listPath.IsEmpty() && IsList)
            {
                AllPageList.Add(listPath);
            }

            //详情页
            if (!contentPath.IsEmpty() && IsContent)
            {
                foreach (var itemId in dataList)
                {
                    var path = contentPath + "?itemId=" + itemId;
                    AllPageList.Add(path);
                }
            }
        }
        catch (Exception ex)
        {
            LogHelper.Log(LogPath, ex);
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
            LogHelper.Log(LogPath, "正在生成静态页：" + htmlUrl);
            if (HttpContext.Current != null)
            {
                string writePath = RootPath + htmlUrl.ToStr().Replace(HttpPath, "");
                StreamWriter sw = new StreamWriter(writePath, false, Encoding.UTF8);
                try
                {
                    HttpContext.Current.Server.Execute(AppName + aspxUrl.Replace(HttpPath, ""), sw);
                }
                catch (Exception ex)
                {
                    LogHelper.Log(LogPath, ex);
                }
                finally
                {
                    sw.Close();
                    if (File.Exists(writePath))
                    {
                        if (new FileInfo(writePath).Length < 4)
                            File.Delete(writePath);
                    }
                }
            }
            else
            {
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

            }
        }
        catch (Exception ex)
        {
            LogHelper.Log(LogPath, ex);
        }
    }

    /// <summary>
    /// 创建静态html文件
    /// </summary>
    /// <param name="aspxUrl"></param>
    /// <returns></returns>
    public void CreateHtml(object tempList)
    {
        foreach (var aspxUrl in tempList as List<string>)
        {
            var htmlUrl = GetHtmlUrl(aspxUrl);
            CreateHtml(aspxUrl, htmlUrl);
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
        try
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
        catch
        {
            return "";
        }
    }


}