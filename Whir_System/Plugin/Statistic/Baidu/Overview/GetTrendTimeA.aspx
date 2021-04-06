<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="GetTrendTimeA.aspx.cs" Inherits="Whir_System_Plugin_Statistic_Baidu_Overview_GetTimeTrendRpt" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading"><%="流量统计".ToLang()%></div>
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li class="active">
                        <a data-toggle="tab" aria-expanded="true">
                            <%="趋势分析".ToLang()%>
                        </a>
                    </li>
                    <li >
                        <a href="<%=SysPath%>Plugin/Statistic/Baidu/Overview/GetVisitToppageA.aspx">
                            <%="受访页面".ToLang()%>
                        </a>
                    </li>
                    <li>
                        <a href="<%=SysPath%>Plugin/Statistic/Baidu/Overview/GetVisitDistrictA.aspx">
                            <%="地域分析".ToLang()%>
                        </a>
                    </li>
                </ul>
                <div class="space15"></div>
                <div class="baidu-box">
                    <div class="search-box form">
                        <div class="btn-group">
                            <a href="javascript:;" data-start="<%=DateTime.Today.ToString("yyyy-MM-dd")%>" data-end="<%=DateTime.Today.ToString("yyyy-MM-dd")%>" class="btn btn-white active">
                                <%="今天".ToLang()%>
                            </a>
                            <a href="javascript:;" data-start="<%=DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")%>" data-end="<%=DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd")%>" class="btn btn-white">
                                <%="昨天".ToLang()%>
                            </a>
                            <a href="javascript:;" data-start="<%=DateTime.Today.AddDays(-6).ToString("yyyy-MM-dd")%>" data-end="<%=DateTime.Today.ToString("yyyy-MM-dd")%>" class="btn btn-white">
                                <%="7天内".ToLang()%>
                            </a>
                        </div>
                    </div>
                    <div class="chart-box">
                        <ul class="row">
                            <li class="col-md-4 col-lg-2"><span> <%="访客数(UV)".ToLang()%></span><p data-total="visitor_count">0</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span> <%="新访客数".ToLang()%></span><p data-total="new_visitor_count">0</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span> <%="IP数".ToLang()%></span><p data-total="ip_count">0</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span> <%="跳出率".ToLang()%></span><p data-total="bounce_ratio">0</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span> <%="平均访问时长".ToLang()%></span><p data-total="avg_visit_time">0</p>
                            </li>
                        </ul>
                        <hr />
                        <div class="chart" style="height: 600px;">
                            <div id="main" style="width: 100%; height: 400px;"></div>
                        </div>
                    </div>
                    <div class="grid">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="<%=SysPath%>Res/js/echarts.min.js"></script>
    <script src="<%=AppName%>Res/js/JSON-js-master/json2.js"></script>
    <script src="<%=SysPath%>Res/js/Whir/whir.bdstatistics.js"></script>
    <script src="<%=SysPath%>Res/js/Whir/whir.bdecharts.js"></script>
    <script type="text/javascript">
        var metrics = {
            pv_count: '<%="浏览量（PV）".ToLang()%>',
            pv_ratio: '<%="浏览量占比".ToLang()%>',
            visit_count: '<%="访客次数".ToLang()%>',
            visitor_count: '<%="访客数（UV）".ToLang()%>',
            new_visitor_count: '<%="新访客数".ToLang()%>',
            new_visitor_ratio: '<%="新访客比率".ToLang()%>',
            ip_count: '<%="IP数".ToLang()%>',
            bounce_ratio: '<%="跳出率".ToLang()%>',
            avg_visit_time: '<%="平均访问时长".ToLang()%>',
            avg_visit_pages: '<%="平均访问页数".ToLang()%>',
            trans_count: '<%="转化次数".ToLang()%>',
            trans_ratio: '<%="转化率".ToLang()%>',
            contri_pv: '<%="百度推荐贡献浏览量".ToLang()%>',
            avg_trans_cost: '<%="平均转化成本".ToLang()%>',
            income: '<%="收益".ToLang()%>',
            profit: '<%="利润".ToLang()%>',
            roi: '<%="投资回报率".ToLang()%>',
            visit1_count: '<%="入口页次数".ToLang()%>',
            outward_count: '<%="贡献下游浏览量".ToLang()%>',
            exit_count: '<%="退出页次数".ToLang()%>',
            average_stay_time: '<%="平均停留时长".ToLang()%>',
            exit_ratio: '<%="退出率".ToLang()%>'
        };

        var BDStatisticsUrl = '<%=SysPath%>Handler/Plugin/VisitStatistic.aspx';
        $(function () {
            var bdEcharts = whir.bdecharts('main');

            function initPageData(start, end) {
                function showCharts(data) {

                    var yMetrics = ['pv_count', 'visitor_count'];
                    var yTextData = whir.bdStatistics.helper.getMetricsText(yMetrics);
                    var yValueData = [];
                    var xTextData = [];

                    if (data) {
                        if (data.items && data.items.length > 0) {
                            (function initXTextData(xTexts) {
                                for (var i = xTexts.length - 1; i >= 0; i--) {
                                    xTextData.push(whir.bdStatistics.helper.getTrendTimeYText(xTexts[i]));
                                }
                            })(data.items[0]);
                        }

                        if (data.fields && data.items.length > 1) {
                            (function initYValue() {
                                for (var i = 0; i < yMetrics.length; i++) {
                                    var metrics = yMetrics[i];
                                    var valueIndex = data.fields.indexOf(metrics) - 1;
                                    var yValueItem = [];
                                    for (var y = data.items[1].length - 1; y >= 0; y--) {
                                        var dataItem = data.items[1][y];
                                        if (dataItem.length > valueIndex) {
                                            var value = dataItem[valueIndex];
                                            if (typeof value === 'number')
                                                yValueItem.push(value);
                                        }
                                    }
                                    yValueData.push({
                                        name: whir.bdStatistics.helper.getMetricsText(metrics),
                                        type: 'line',
                                        stack: ' <%="总量".ToLang()%>',
                                        areaStyle: { normal: {} },
                                        data: yValueItem
                                    });
                                }
                            })();
                        }

                        bdEcharts.lineStack({
                            title: ' <%="趋势分析".ToLang()%>',
                            xAxisData: xTextData,
                            yAxisData: yValueData,
                            yAxisTextData: yTextData
                        });
                    }
                }

                function showTotal(data) {
                    $('p[data-total]').each(function () {
                        var totalMetrics = $(this).attr('data-total');
                        if (data.fields && data.sum.length > 0) {
                            var valueIndex = data.fields.indexOf(totalMetrics) - 1;
                            var value = whir.bdStatistics.helper.getMetricsValue(totalMetrics,data.sum[0][valueIndex]);
                            
                            $(this).text(value);
                        }
                    });
                }

                bdEcharts.showLoading();
                whir.bdStatistics.GetTrendTimeA(start, end, function (data) {
                    bdEcharts.hideLoading();
                    showCharts(data);
                    showTotal(data);
                });
            }

            $('[data-start][data-end]').click(function () {
                $('[data-start][data-end].active').removeClass('active');
                $(this).addClass('active');

                var start = $(this).attr('data-start');
                var end = $(this).attr('data-end');

                initPageData(start, end);
            });

            initPageData("<%=DateTime.Today.ToString("yyyy-MM-dd")%>","<%=DateTime.Today.ToString("yyyy-MM-dd")%>");
        });
    </script>
</asp:Content>

