using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Whir.Domain;
using Whir.ezEIP.Web;
using Whir.Framework;
using Whir.Label.Dynamic;
using Whir.Label.Static;
using Whir.Repository;
using Whir.Service;


public partial class Whir_System_Module_Release_AsyncRelease : SysManagePageBase
{
    /// <summary>
    /// 参数实体类
    /// </summary>
    public class Parameter
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //GetLabel("<div>rrr</div><div PageSize=\"5\" TargetID=\"mylist1\" Footer=\"5\" >sds</div>", "div");
        //var dd = GetValue("<div PageSize=\"dfe5\" PageSize=\"rtert\" TargetID=\"mylist1\" Footer=\"5\" >sds</div>", "PageSize", "\"");
        Release();
    }

    public void Release()
    {

        var columnList = new ColumnService().GetList(CurrentSiteId);
        var site = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(CurrentSiteId) ?? ModelFactory<SiteInfo>.Insten();
        foreach (var column in columnList)
        {
            var httpUrl = RequestUtil.Instance.GetHttpUrl();

            DeleteRelease(site.SiteId, column.ColumnId, 0, 0, true);

            var model = new ModelService().GetModelByColumnId(column.ColumnId);
            if (model == null || column.CreateMode != 1)
                continue;

            string sql = "Select {0}_PID From {0} Where TypeId={1} And IsDel=0".FormatWith(model.TableName, column.ColumnId);
            var dataList = DbHelper.CurrentDb.Query<int>(sql).ToList();

            httpUrl = httpUrl + (site.IsRootPath ? "" : site.Path + "/") + column.Path;

            var defaultPath = column.DefaultTemp.IsEmpty() ? "" : httpUrl + "/index.aspx";
            var listPath = column.ListTemp.IsEmpty() ? "" : httpUrl + "/list.aspx";
            var contentPath = column.ContentTemp.IsEmpty() ? "" : httpUrl + "/info.aspx";
            //首页
            if (!defaultPath.IsEmpty())
            {
                var tempPath = "~/{0}/template/{1}".FormatWith(site.Path, column.DefaultTemp);
                tempPath = Server.MapPath(tempPath);
                if (!File.Exists(tempPath))
                    continue;

                var htmlUrl = GetHtmlUrl(defaultPath);
                CreateReleaseList(site.SiteId, column.ColumnId, 0, 0, defaultPath, htmlUrl);

                var releaseParameterList = DbHelper.CurrentDb.Query<ReleaseParameter>("Where ColumnId=@0 and TempType=1 ", column.ColumnId).ToList();
                var parameterList = GetParameterList(releaseParameterList);

                var htmlStr = File.ReadAllText(tempPath, Encoding.UTF8);
                var isPage = false;
                var pageSize = 0;

                var labelList = GetLabel(htmlStr, "wtl:list");
                foreach (var item in labelList)
                {
                    isPage = GetLabelValue(item, "needpage").ToBoolean(false);
                }
                if (isPage)
                {
                    labelList = GetLabel(htmlStr, "wtl:Pager");
                    foreach (var item in labelList)
                    {
                        pageSize = GetLabelValue(item, "PageSize").ToInt(10);
                    }
                }

                if (isPage)
                { }
                else
                {

                    //defaultPath += "?";
                    //foreach (var releaseInfo in releaseInfoList) //先把所有参数一起运算
                    //{
                    //    defaultPath += releaseInfo.ParameterName + "={" + releaseInfoList.IndexOf(releaseInfo) + "}&";
                    //}
                    //defaultPath = defaultPath.TrimEnd('&');
                    var path=defaultPath;

                    List<string> listUrl = new List<string>();// getUrlList(parameterList, path, key);
                 
                    foreach (var releaseInfo in releaseParameterList)
                    {
                        foreach (var item in parameterList.Where(p=>p.Key==releaseInfo.ParameterName))
                        {
                            path = defaultPath + "?{0}={1}".FormatWith(item.Key, item.Value);
                        }
                    }

                }
                //CreateHtml(defaultPath, htmlUrl);
            }
            //列表页
            if (!listPath.IsEmpty())
            {
                List<Parameter> parameterList = new List<Parameter>();
                var pageSize = 10;
                var releaseInfoList = DbHelper.CurrentDb.Query<ReleaseParameter>("Where ColumnId=@0 ", column.ColumnId).ToList();

                foreach (var item in parameterList)
                {

                }

                //if (dataList.Count > pageNum)
                //{
                //    var pageNum = (dataList.Rows.Count % pageSize) == 0 ? dataList.Rows.Count / pageSize : dataList.Rows.Count / pageSize + 1;
                //    for (int pageIndex = 1; pageIndex <= pageNum; pageIndex++)
                //    {
                //        var path = listPath + "?page=" + pageIndex;
                //        var htmlUrl = GetHtmlUrl(path);
                //        CreateReleaseList(site.SiteId, column.ColumnId, 0, 0, path, htmlUrl);
                //        //CreateHtml(path, htmlUrl);
                //    }
                //}
            }

            //详情页
            if (!contentPath.IsEmpty())
            {

                foreach (var itemId in dataList)
                {
                    var path = contentPath + "?itemId=" + itemId;
                    var htmlUrl = GetHtmlUrl(path);
                    CreateReleaseList(site.SiteId, column.ColumnId, 0, itemId, path, htmlUrl);
                    // CreateHtml(path, htmlUrl);
                }
            }
        }
        Response.Write("ok");
        Response.End();
    }


    public List<string> getUrlList(List<Parameter> parameterList, int url, string key)
    {
        List<string> urlList = new List<string>();
        foreach (var item in parameterList.Where(p => p.Key == key))
        {
            urlList.Add(url + "{0}={1}&".FormatWith(item.Key, item.Value));
        }
        return urlList;
    }

    /// <summary>
    /// 删除的静态页面记录
    /// </summary>
    /// <param name="siteId">站点</param>
    /// <param name="columnId">栏目</param>
    /// <param name="subjectId">子站、专题</param>
    /// <param name="itemId">item</param>
    /// <param name="isDelAll">是否删除所有记录，否则只删生成成功的记录</param>
    public void DeleteRelease(int siteId, int columnId, int subjectId, int itemId, bool isDelAll)
    {
        List<object> param = new List<object>();
        string sql = "Delete from Whir_Dev_Release Where 1=1";
        if (itemId != 0)
        {
            sql += " And ItemId=@{0}".FormatWith(param.Count);
            param.Add(itemId);
        }
        if (columnId != 0)
        {
            sql += " And ColumnId=@{0}".FormatWith(param.Count);
            param.Add(columnId);
        }
        if (subjectId != 0)
        {
            sql += " And SubjectId=@{0}".FormatWith(param.Count);
            param.Add(subjectId);
        }
        if (siteId != 0)
        {
            sql += " And SiteId=@{0}".FormatWith(param.Count);
            param.Add(siteId);
        }
        if (!isDelAll)
        {
            sql += " And IsSuccess=@{0}".FormatWith(param.Count);
            param.Add(isDelAll);
        }
        DbHelper.CurrentDb.Execute(sql, param.ToArray());
    }

    /// <summary>
    /// 创建静态页面发布列表
    /// </summary>
    public void CreateReleaseList(int siteId, int columnId, int subjectId, int itemId, string aspxUrl, string htmlUrl)
    {
        ReleaseInfo rInfo = new ReleaseInfo()
        {
            SiteId = siteId,
            ColumnId = columnId,
            SubjectId = subjectId,
            ItemId = itemId,
            AspxUrl = aspxUrl,
            ReleaseUrl = htmlUrl,
            IsSuccess = false
        };
        DbHelper.CurrentDb.Insert(rInfo);
        DbHelper.CurrentDb.Dispose();
    }

    /// <summary>
    /// 创建静态html文件
    /// </summary>
    /// <param name="aspxUrl"></param>
    /// <param name="htmlUrl"></param>
    /// <returns></returns>
    public bool CreateHtml(string aspxUrl, string htmlUrl)
    {

        string aspxSourceCode = new CollectionUtils().GetHttpPageCode(aspxUrl, Encoding.UTF8);

        if (aspxSourceCode != "$UrlIsFalse" && aspxSourceCode != "$GetFalse")
        {
            //写入静态文件
            string html = aspxSourceCode;
            string writePath = HttpContext.Current.Server.MapPath(new Uri(htmlUrl).AbsolutePath);
            FileSystemHelper.WriteFile(writePath, html);

            if (FileSystemHelper.IsFieldExist(writePath))
                return true;
        }
        return false;
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
    /// 获取模板页的分页控件、页数、sql
    /// </summary>
    /// <param name="aspxUrl"></param>
    /// <returns></returns>
    public int GetPageSizeByTemp(string tempPath)
    {

        int PageSize = 9999;
        string ShtmlContent = "";

        if (!File.Exists(tempPath))
            return PageSize;

        using (StreamReader sr = new StreamReader(tempPath))
        {
            ShtmlContent = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            // PageSize = GetContent(ShtmlContent).ToInt(10);
        }
        return PageSize;
    }

    /// <summary>
    /// 获取置标
    /// </summary>
    /// <param name="htmlStr">根据url抓取的html所有标记</param>
    /// <param name="label">置标</param>
    /// <returns></returns>
    public List<string> GetLabel(string htmlStr, string label)
    {
        List<string> labelList = new List<string>();
        Regex artReg = new Regex(@"(<{0})([^<]*)(</{0}>)".FormatWith(label), RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

        if (artReg.IsMatch(htmlStr))
        {
            MatchCollection matches = artReg.Matches(htmlStr);
            foreach (Match item in matches)
            {
                labelList.Add(item.Groups[0].Value);
            }
        }
        return labelList;
    }

    /// <summary>
    /// 获取置标 参数值
    /// </summary>
    /// <param name="htmlStr">根据url抓取的html所有标记</param>
    /// <param name="label">置标</param>
    /// <returns></returns>
    public string GetLabelValue(string label, string param)
    {
        Regex rg = new Regex("(?<=(" + param + "=\"))[.\\s\\S]*?(?=(\"))", RegexOptions.Multiline | RegexOptions.Singleline);
        return rg.Match(label).Value;
    }

    public List<Parameter> GetParameterList(List<ReleaseParameter> releaseInfoList)
    {
        List<Parameter> parameterList = new List<Parameter>();

        foreach (var item in releaseInfoList)
        {
            switch (item.ParameterSource.ToInt())
            {
                case 1:  //自定义 男|女
                    foreach (var bindStr in item.ParameterBind.Split('|'))
                    {
                        parameterList.Add(new Parameter() { Key = item.ParameterName, Value = bindStr });
                    }
                    break;
                case 2: //指定栏目的类别Id
                    var categoryColmun = new ColumnService().GetCategoryByMarkParentId(item.ParameterBind.ToInt(0));
                    if (categoryColmun != null)
                    {
                        var model = new ModelService().GetModelByColumnId(categoryColmun.ColumnId);
                        var sql = "Select {0}_PID From {0} Where TypeId={1} And IsDel=0".FormatWith(model.TableName, categoryColmun.ColumnId);
                        var list = DbHelper.CurrentDb.Query<int>(sql).ToList();
                        foreach (var categoryId in list)
                        {
                            parameterList.Add(new Parameter() { Key = item.ParameterName, Value = categoryId });
                        }
                    }
                    break;
                case 3://绑定sql
                    try
                    {
                        if (Regex.IsMatch(item.ParameterBind, "(delete|update|insert|alter|drop)")) //只能执行select语句
                            break;
                        var list = DbHelper.CurrentDb.Query<object>(item.ParameterBind).ToList();
                        foreach (var obj in list)
                        {
                            parameterList.Add(new Parameter() { Key = item.ParameterName, Value = obj.ToStr() });
                        }
                    }
                    catch
                    {
                    }
                    break;
                case 4: //数字范围 -13|99
                    if (item.ParameterBind.Contains("|"))
                    {
                        var start = item.ParameterBind.Split('|')[0].ToInt();
                        var end = item.ParameterBind.Split('|')[1].ToInt();
                        if (start > end)
                            break;
                        while (start <= end)
                        {
                            parameterList.Add(new Parameter() { Key = item.ParameterName, Value = start++ });
                        }
                    }
                    break;
                case 5://日期范围 2017-01-01|2017-02-01
                    if (item.ParameterBind.Contains("|"))
                    {
                        var start = item.ParameterBind.Split('|')[0].ToDateTime();
                        var end = item.ParameterBind.Split('|')[1].ToDateTime();
                        if (start > end)
                            break;
                        while (start <= end)
                        {
                            parameterList.Add(new Parameter() { Key = item.ParameterName, Value = start });
                            start = start.AddDays(1);
                        }
                    }
                    break;
            }
        }

        return parameterList;
    }

    public class PermutationAndCombination<T>
    {
        /// <summary>
        /// 交换两个变量
        /// </summary>
        /// <param name="a">变量1</param>
        /// <param name="b">变量2</param>
        public static void Swap(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        /// <summary>
        /// 递归算法求数组的组合(私有成员)
        /// </summary>
        /// <param name="list">返回的范型</param>
        /// <param name="t">所求数组</param>
        /// <param name="n">辅助变量</param>
        /// <param name="m">辅助变量</param>
        /// <param name="b">辅助数组</param>
        /// <param name="M">辅助变量M</param>
        private static void GetCombination(ref List<T[]> list, T[] t, int n, int m, int[] b, int M)
        {
            for (int i = n; i >= m; i--)
            {
                b[m - 1] = i - 1;
                if (m > 1)
                {
                    GetCombination(ref list, t, i - 1, m - 1, b, M);
                }
                else
                {
                    if (list == null)
                    {
                        list = new List<T[]>();
                    }
                    T[] temp = new T[M];
                    for (int j = 0; j < b.Length; j++)
                    {
                        temp[j] = t[b[j]];
                    }
                    list.Add(temp);
                }
            }
        }
        /// <summary>
        /// 递归算法求排列(私有成员)
        /// </summary>
        /// <param name="list">返回的列表</param>
        /// <param name="t">所求数组</param>
        /// <param name="startIndex">起始标号</param>
        /// <param name="endIndex">结束标号</param>
        private static void GetPermutation(ref List<T[]> list, T[] t, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
            {
                if (list == null)
                {
                    list = new List<T[]>();
                }
                T[] temp = new T[t.Length];
                t.CopyTo(temp, 0);
                list.Add(temp);
            }
            else
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    Swap(ref t[startIndex], ref t[i]);
                    GetPermutation(ref list, t, startIndex + 1, endIndex);
                    Swap(ref t[startIndex], ref t[i]);
                }
            }
        }
        /// <summary>
        /// 求从起始标号到结束标号的排列，其余元素不变
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="startIndex">起始标号</param>
        /// <param name="endIndex">结束标号</param>
        /// <returns>从起始标号到结束标号排列的范型</returns>
        public static List<T[]> GetPermutation(T[] t, int startIndex, int endIndex)
        {
            if (startIndex < 0 || endIndex > t.Length - 1)
            {
                return null;
            }
            List<T[]> list = new List<T[]>();
            GetPermutation(ref list, t, startIndex, endIndex);
            return list;
        }
        /// <summary>
        /// 返回数组所有元素的全排列
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <returns>全排列的范型</returns>
        public static List<T[]> GetPermutation(T[] t)
        {
            return GetPermutation(t, 0, t.Length - 1);
        }
        /// <summary>
        /// 求数组中n个元素的排列
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="n">元素个数</param>
        /// <returns>数组中n个元素的排列</returns>
        public static List<T[]> GetPermutation(T[] t, int n)
        {
            if (n > t.Length)
            {
                return null;
            }
            List<T[]> list = new List<T[]>();
            List<T[]> c = GetCombination(t, n);
            for (int i = 0; i < c.Count; i++)
            {
                List<T[]> l = new List<T[]>();
                GetPermutation(ref l, c[i], 0, n - 1);
                list.AddRange(l);
            }
            return list;
        }
        /// <summary>
        /// 求数组中n个元素的组合
        /// </summary>
        /// <param name="t">所求数组</param>
        /// <param name="n">元素个数</param>
        /// <returns>数组中n个元素的组合的范型</returns>
        public static List<T[]> GetCombination(T[] t, int n)
        {
            if (t.Length < n)
            {
                return null;
            }
            int[] temp = new int[n];
            List<T[]> list = new List<T[]>();
            GetCombination(ref list, t, t.Length, n, temp, n);
            return list;
        }
    }
}