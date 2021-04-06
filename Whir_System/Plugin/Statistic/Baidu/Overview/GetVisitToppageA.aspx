<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" CodeFile="GetVisitToppageA.aspx.cs" Inherits="Whir_System_Plugin_Statistic_Baidu_Overview_GetVisitToppageA" %>


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
                    <li>
                        <a href="<%=SysPath%>Plugin/Statistic/Baidu/Overview/GetTrendTimeA.aspx">
                            <%="趋势分析".ToLang()%>
                        </a>
                    </li>
                    <li class="active">
                        <a data-toggle="tab" aria-expanded="true">
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
                            <li class="col-md-4 col-lg-2"><span><%="浏览量（PV）".ToLang()%></span><p data-total="pv_count">0</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span><%="贡献下游浏览量".ToLang()%></span><p data-total="outward_count">0</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span><%="平均停留时长".ToLang()%></span><p data-total="average_stay_time">0</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span><%="退出率".ToLang()%></span><p data-total="exit_ratio">0</p>
                            </li>
                        </ul>
                        <hr />
                          <table width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-bordered table-noPadding">
                            <thead>
                                <tr>
                                    <th rowspan="2"><%="URL".ToLang()%></th>
                                    <th><%="网站基础指标".ToLang()%></th>
                                    <th colspan="3"><%="流量质量指标".ToLang()%></th>
                                </tr>
                                <tr>
                                    <th><%="浏览量（PV）".ToLang()%></th>
                                    <th><%="贡献下游浏览量".ToLang()%></th>
                                    <th><%="平均停留时长".ToLang()%></th>
                                    <th><%="退出率".ToLang()%></th>
                                </tr>
                            </thead>
                            <tbody id="data-value-tbody">
                                <tr style="display: none;" id="data-value-template">
                                    <td data-value="visit_page_title"></td>
                                    <td data-value="pv_count"></td>
                                    <td data-value="outward_count"></td>
                                    <td data-value="average_stay_time"></td>
                                    <td data-value="exit_ratio"></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="grid">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="<%=AppName%>Res/js/JSON-js-master/json2.js"></script>
    <script src="<%=SysPath%>Res/js/Whir/whir.bdstatistics.js"></script>
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

            function initPageData(start, end, startIndex) {
                $('#data-value-tbody tr:gt(0)').remove();

                function initTableData(data) {
                    if (data.items[0].length === data.items[1].length) {
                        function getPageUrl(pageItem) {
                            if (pageItem instanceof Array) {
                                return pageItem[0].name;
                            }
                            return '';
                        }

                        var fields = data.fields;
                        var templateTd = $('#data-value-template td');
                        for (var i = 0; i < data.items[0].length; i++) {
                            var tr = $('<tr>');
                            (function (tr, i) {
                                templateTd.each(function () {
                                    var metrics = $(this).attr('data-value');

                                    var td = $('<td>');
                                    var dataItem = data.items[1][i];

                                    switch (metrics) {
                                        case 'visit_page_title':
                                            td.text(getPageUrl(data.items[0][i]));
                                            break;
                                        default:
                                            var valueIndex = fields.indexOf(metrics) - 1;
                                            if (valueIndex > -1) {
                                                var value = whir.bdStatistics.helper.getMetricsValue(metrics, dataItem[valueIndex]);

                                                td.text(value);
                                            }
                                            break;
                                    }
                                    tr.append(td);

                                });
                            })(tr, i);

                            $('#data-value-tbody').append(tr);
                        }
                    }
                }
                function initTotalData(data) {
                    $('[data-total]').each(function () {
                        var metrics = $(this).attr('data-total');
                        var valueIndex = data.fields.indexOf(metrics) - 1;
                        if (valueIndex > -1 && data.sum && data.sum[0].length > 0) {
                            var value = whir.bdStatistics.helper.getMetricsValue(metrics, data.sum[0][valueIndex]);

                            $(this).text(value);
                        }
                    });
                }

                whir.bdStatistics.GetVisitToppageA(start, end, startIndex, function (data) {
                    initTableData(data);
                    initTotalData(data);
                });
            }

            $('[data-start][data-end]').click(function () {
                $('[data-start][data-end].active').removeClass('active');
                $(this).addClass('active');

                var start = $(this).attr('data-start');
                var end = $(this).attr('data-end');

                initPageData(start, end, 0);
            });

            initPageData("<%=DateTime.Today.ToString("yyyy-MM-dd")%>","<%=DateTime.Today.ToString("yyyy-MM-dd")%>");
        });
    </script>
</asp:Content>
