using System;
using Whir.Domain;
using Whir.Framework;
using Whir.Service;
using Whir.ezEIP.Web;
using Whir.Language;


public partial class Whir_System_Handler_Pligin_VisitStatistic : SysHandlerPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        var action = RequestUtil.Instance.GetFormString("_action");
        Exec(this, action);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <returns></returns>
    public HandlerResult Save()
    {
        //从配置文件中获取信息
        var type = typeof(SiteInfo);
        var model = SiteInfoHelper.SiteInfo ?? ModelFactory<SiteInfo>.Insten();
        model = GetPostObject(type, model) as SiteInfo;
        var pwd = RequestUtil.Instance.GetFormString("BaiduPwd");
        model.BaiduPwd = pwd.IsEmpty() ? "" : pwd.EncodeBase64Reverse();
        if (model.BaiduUserName.IsEmpty())
        {
            model.BaiduPwd = "";
            model.BaiduSiteId = "";
            model.BaiduSitesJson = "";
            model.BaiduToken = "";
        }

        ServiceFactory.SiteInfoService.Update(model);

        ServiceFactory.OperateLogService.Save("修改统计应用代码【{0}】,流量统计是否启用【{1}】".FormatWith(model.VisitStatisticCode, model.VisitStatisticEnable));

        return new HandlerResult { Status = true, Message = "保存成功".ToLang() };
    }

    public HandlerResult GetBaiduSites()
    {
        var baiduUserName = RequestUtil.Instance.GetFormString("BaiduUserName");
        var baiduPwd = RequestUtil.Instance.GetFormString("BaiduPwd");
        var baiduToken = RequestUtil.Instance.GetFormString("BaiduToken");

        var helper = new BaiduTongJiHelper(baiduUserName, baiduPwd, baiduToken, "");
        return new HandlerResult { Status = true, Message = helper.GetSites() };
    }

    public HandlerResult GetTimeTrendRpt()
    {
        var start = RequestUtil.Instance.GetFormString("start").ToDateTime();
        var end = RequestUtil.Instance.GetFormString("end").ToDateTime();

        var site = SiteInfoHelper.SiteInfo;
        var helper = new BaiduTongJiHelper(site.BaiduUserName, site.BaiduPwd.DecodeBase64Reverse(), site.BaiduToken, site.BaiduSiteId);
        var result = helper.GetTimeTrendRpt(start, end, new[] { Metrics.pv_count, Metrics.visitor_count, Metrics.ip_count, Metrics.bounce_ratio, Metrics.avg_visit_time });
        return new HandlerResult { Status = true, Message = result };
    }

    public HandlerResult GetTrendTimeA()
    {
        var start = RequestUtil.Instance.GetFormString("start").ToDateTime();
        var end = RequestUtil.Instance.GetFormString("end").ToDateTime();
        var gran = Gran.hour;
        if ((end - start).TotalDays > 1)
        {
            gran = Gran.day;
        }

        var site = SiteInfoHelper.SiteInfo;
        var helper = new BaiduTongJiHelper(site.BaiduUserName, site.BaiduPwd.DecodeBase64Reverse(), site.BaiduToken, site.BaiduSiteId);
        var result = helper.GetTrendTimeA(start, end, new[] { Metrics.pv_count, Metrics.visitor_count, Metrics.new_visitor_count, Metrics.ip_count, Metrics.bounce_ratio, Metrics.avg_visit_time }, gran);
        return new HandlerResult { Status = true, Message = result };
    }


    public HandlerResult GetVisitToppageA()
    {
        var start = RequestUtil.Instance.GetFormString("start").ToDateTime();
        var end = RequestUtil.Instance.GetFormString("end").ToDateTime();
        var startIndex = RequestUtil.Instance.GetFormString("start_index").ToLong();

        var site = SiteInfoHelper.SiteInfo;
        var helper = new BaiduTongJiHelper(site.BaiduUserName, site.BaiduPwd.DecodeBase64Reverse(), site.BaiduToken, site.BaiduSiteId);
        var result = helper.GetVisitToppageA(start, end, startIndex,
            new[]
            {
                Metrics.pv_count,
                Metrics.visitor_count,
                Metrics.ip_count,
                Metrics.visit1_count,
                Metrics.outward_count,
                Metrics.exit_count,
                Metrics.average_stay_time,
                Metrics.exit_ratio
            });
        return new HandlerResult { Status = true, Message = result };
    }

    public HandlerResult GetVisitDistrictA()
    {
        var start = RequestUtil.Instance.GetFormString("start").ToDateTime();
        var end = RequestUtil.Instance.GetFormString("end").ToDateTime();
        var isWorld = RequestUtil.Instance.GetFormString("isWorld").ToBoolean();

        var site = SiteInfoHelper.SiteInfo;
        var helper = new BaiduTongJiHelper(site.BaiduUserName, site.BaiduPwd.DecodeBase64Reverse(), site.BaiduToken, site.BaiduSiteId);
        var result = helper.GetVisitDistrictA(start, end, isWorld,
            new[]
            {
                Metrics.pv_count,
                Metrics.pv_ratio,
                Metrics.visit_count,
                Metrics.visitor_count,
                Metrics.new_visitor_count,
                Metrics.new_visitor_ratio,
                Metrics.ip_count,
                Metrics.bounce_ratio,
                Metrics.avg_visit_time,
                Metrics.avg_visit_pages,
                Metrics.trans_count,
                Metrics.trans_ratio
            });
        return new HandlerResult { Status = true, Message = result };
    }
}