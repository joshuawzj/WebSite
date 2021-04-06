<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="GetTimeTrendRpt.aspx.cs" Inherits="Whir_System_Plugin_Statistic_Baidu_Overview_GetTimeTrendRpt" %>

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
                    <li class="active"><a data-toggle="tab" aria-expanded="true"><%="趋势分析".ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="baidu-box">
                    <div class="search-box form">
                        <div class="btn-group">
                            <a href="javascrip:;" data-start="<%=DateTime.Today.ToString("yyyyMMdd")%>" data-end="<%=DateTime.Today.ToString("yyyyMMdd")%>" class="btn btn-white">
                                <%="今天".ToLang()%>
                            </a>
                            <a href="javascrip:;" data-start="<%=DateTime.Today.AddDays(-1).ToString("yyyyMMdd")%>" data-end="<%=DateTime.Today.AddDays(-1).ToString("yyyyMMdd")%>" class="btn btn-white">
                                <%="昨天".ToLang()%>
                            </a>
                            <a href="javascrip:;" data-start="<%=DateTime.Today.AddDays(-7).ToString("yyyyMMdd")%>" data-end="<%=DateTime.Today.ToString("yyyyMMdd")%>" class="btn btn-white">
                                <%="7天内".ToLang()%>
                            </a>
                        </div>
                    </div>
                    <div class="chart-box">
                        <ul class="row">
                            <li class="col-md-4 col-lg-2"><span>访客数(UV)</span><p>24754</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span>新访客数</span><p>24754</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span>IP数</span><p>24754</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span>跳出率</span><p>24754</p>
                            </li>
                            <li class="col-md-4 col-lg-2"><span>平均访问时长</span><p>24754</p>
                            </li>
                        </ul>
                        <hr />
                        <div class="chart">
                        </div>
                    </div>
                    <div class="grid">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var start = "<%=DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")%>";
        var end = "<%=DateTime.Today.ToString("yyyy-MM-dd")%>";

        function getTimeTrendRpt() {
            $.ajax({
                type: "POST",
                url: "<%=SysPath%>Handler/Plugin/VisitStatistic.aspx",
                data: {
                    _action: "GetTimeTrendRpt",
                    start: start,
                    end: end
                },
                success: function(response) {
                    response = eval("(" + response + ")");
                    if (response.Status == true) {
                        response = eval("(" + response.Message + ")");
                        $(response).each(function(idx, item) {
                            item = eval("(" + item + ")");
                            if (item.header.desc == "system failure") {
                                whir.toastr.error(item.header.failures[0].code + "&nbsp;" + item.header.failures[0].message);
                                return true;
                            }

                            console.log(item);
                        });
                    }
                }
            });
        }
    </script>
</asp:Content>

