using System.Collections.Generic;
using Whir.Domain;
using Whir.Service;
using Whir.Framework;
using System.Net;
using System.Text;
using System.IO;
using System;

/// <summary>
/// BaiduTongJiHelper 的摘要说明
/// </summary>
public class BaiduTongJiHelper
{
    protected string UserName { get; set; }
    protected string Pwd { get; set; }
    protected string Token { get; set; }
    protected string SiteId { get; set; }

    protected Dictionary<string, string> RequestHeader
    {
        get
        {
            return new Dictionary<string, string>
            {
                {"account_type", "1"},
                {"username", UserName},
                {"password", Pwd},
                {"token", Token}
            };
        }
    }

    public BaiduTongJiHelper(string userName, string pwd, string token, string siteId)
    {
        UserName = userName;
        Pwd = pwd;
        Token = token;
        SiteId = siteId;
    }

    public BaiduTongJiHelper(int siteId)
    {
        var site = ServiceFactory.SiteInfoService.SingleOrDefault<SiteInfo>(siteId);
        if (site != null)
        {
            UserName = site.BaiduUserName;
            Pwd = site.BaiduPwd.DecodeBase64Reverse();
            Token = site.BaiduToken;
            SiteId = site.BaiduSiteId;
        }
    }

    protected string PostData(string requestUrl, Dictionary<string, string> requestBody)
    {
        var request = (HttpWebRequest)WebRequest.Create(requestUrl);
        request.Method = "POST";
        request.ContentType = "application/json;charset=UTF-8";

        var requestData = new
        {
            header = RequestHeader,
            body = requestBody
        }.ToJson();

        var load = Encoding.UTF8.GetBytes(requestData);
        request.ContentLength = load.Length;

        var result = "";
        using (var requestStream = request.GetRequestStream())
        {
            requestStream.Write(load, 0, load.Length);
            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            var reader = new StreamReader(responseStream, Encoding.UTF8);
            result = reader.ReadToEnd();
        }
        return result;
    }

    /// <summary>
    /// 获取百度账户下的所有站点信息
    /// </summary>
    /// <returns></returns>
    public string GetSites()
    {
        const string requestUrl = "https://api.baidu.com/json/tongji/v1/ReportService/getSiteList";
        return PostData(requestUrl, null);
    }


    public string GetStatisticsData(string siteId, string method, Dictionary<string, string> param)
    {

        param = param ?? new Dictionary<string, string>();
        param.Add("site_id", siteId);
        param.Add("method", method);

        const string requestUrl = "https://api.baidu.com/json/tongji/v1/ReportService/getData";
        return PostData(requestUrl, param);
    }


    /// <summary>
    /// 网站概况-趋势数据
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="metrics"></param>
    /// <returns></returns>
    public string GetTimeTrendRpt(DateTime start, DateTime end, string metrics)
    {
        return GetStatisticsData(SiteId, "overview/getTimeTrendRpt", new Dictionary<string, string>
        {
            {"start_date", start.ToString("yyyyMMdd")},
            {"end_date", end.ToString("yyyyMMdd")},
            {"metrics", metrics}
        });
    }

    public string GetTimeTrendRpt(DateTime start, DateTime end, string[] metrics)
    {
        return GetTimeTrendRpt(start, end, string.Join(",", metrics));
    }

    /// <summary>
    /// 趋势分析
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="metrics"></param>
    /// <param name="gran">时间粒度</param>
    /// <returns></returns>
    public string GetTrendTimeA(DateTime start, DateTime end, string metrics, string gran = Gran.hour)
    {
        return GetStatisticsData(SiteId, "trend/time/a", new Dictionary<string, string>
        {
            {"start_date", start.ToString("yyyyMMdd")},
            {"end_date", end.ToString("yyyyMMdd")},
            {"metrics", metrics},
            {"gran",gran }
        });
    }


    public string GetTrendTimeA(DateTime start, DateTime end, string[] metrics, string gran = Gran.hour)
    {
        return GetTrendTimeA(start, end, string.Join(",", metrics), gran);
    }

    /// <summary>
    /// 受访页面
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <param name="startIndex"></param>
    /// <param name="metrics"></param>
    /// <returns></returns>
    public string GetVisitToppageA(DateTime start, DateTime end, long startIndex, string[] metrics)
    {
        return GetVisitToppageA(start, end, startIndex, string.Join(",", metrics));
    }

    public string GetVisitToppageA(DateTime start, DateTime end, long startIndex, string metrics)
    {
        return GetStatisticsData(SiteId, "visit/toppage/a", new Dictionary<string, string>
        {
            {"start_date", start.ToString("yyyyMMdd")},
            {"end_date", end.ToString("yyyyMMdd")},
            {"metrics", metrics},
            {"start_index", startIndex.ToString() }
        });
    }

    public string GetVisitDistrictA(DateTime start, DateTime end, bool isWorld, string[] metrics)
    {
        return GetVisitDistrictA(start, end, isWorld, string.Join(",", metrics));
    }

    public string GetVisitDistrictA(DateTime start, DateTime end, bool isWorld, string metrics)
    {
        return GetStatisticsData(SiteId, isWorld ? "visit/world/a" : "visit/district/a", new Dictionary<string, string>
        {
            {"start_date", start.ToString("yyyyMMdd")},
            {"end_date", end.ToString("yyyyMMdd")},
            {"metrics", metrics}
        });
    }
}

public class Metrics
{
    /// <summary>
    /// 浏览量（PV）
    /// </summary>
    public const string pv_count = "pv_count";

    /// <summary>
    /// 浏览量占比，%
    /// </summary>
    public const string pv_ratio = "pv_ratio";
    /// <summary>
    /// 访客次数
    /// </summary>
    public const string visit_count = "visit_count";
    /// <summary>
    /// 访客数（UV）
    /// </summary>
    public const string visitor_count = "visitor_count";
    /// <summary>
    /// 新访客数
    /// </summary>
    public const string new_visitor_count = "new_visitor_count";
    /// <summary>
    /// 新访客比率，%
    /// </summary>
    public const string new_visitor_ratio = "new_visitor_ratio";
    /// <summary>
    /// IP数
    /// </summary>
    public const string ip_count = "ip_count";
    /// <summary>
    /// 跳出率，%
    /// </summary>
    public const string bounce_ratio = "bounce_ratio";
    /// <summary>
    /// 平均访问时长，秒
    /// </summary>
    public const string avg_visit_time = "avg_visit_time";
    /// <summary>
    /// 平均访问页数
    /// </summary>
    public const string avg_visit_pages = "avg_visit_pages";
    /// <summary>
    /// 转化次数
    /// </summary>
    public const string trans_count = "trans_count";
    /// <summary>
    /// 转化率，%
    /// </summary>
    public const string trans_ratio = "trans_ratio";
    /// <summary>
    /// 百度推荐贡献浏览量
    /// </summary>
    public const string contri_pv = "contri_pv";
    /// <summary>
    /// 平均转化成本，元
    /// </summary>
    public const string avg_trans_cost = "avg_trans_cost";
    /// <summary>
    /// 收益，元
    /// </summary>
    public const string income = "income";
    /// <summary>
    /// 利润，元
    /// </summary>
    public const string profit = "profit";
    /// <summary>
    /// 投资回报率，%
    /// </summary>
    public const string roi = "roi";
    /// <summary>
    /// 入口页次数
    /// </summary>
    public const string visit1_count = "visit1_count";
    /// <summary>
    /// 贡献下游浏览量
    /// </summary>
    public const string outward_count = "outward_count";
    /// <summary>
    /// 退出页次数
    /// </summary>
    public const string exit_count = "exit_count";
    /// <summary>
    /// 平均停留时长
    /// </summary>
    public const string average_stay_time = "average_stay_time";
    /// <summary>
    /// 退出率
    /// </summary>
    public const string exit_ratio = "exit_ratio";
}

public class Gran
{
    public const string day = "day";

    public const string hour = "hour";

    public const string week = "week";

    public const string month = "month";
}