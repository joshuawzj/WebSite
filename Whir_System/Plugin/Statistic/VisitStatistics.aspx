<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="VisitStatistics.aspx.cs" Inherits="Whir_System_Plugin_Statistic_VisitStatistics"
    ValidateRequest="false" %>

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
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/VisitStatistic.aspx">
                        <div class="form-group">
                            <div class="col-md-2 text-right" for="FlowEnable">
                                <%="是否启用：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <ul class="list" id="text1">
                                    <li>
                                        <input type="radio" id="VisitStatisticEnable_True" name="VisitStatisticEnable" value="1" <%=SiteInfo.VisitStatisticEnable?"checked='checked'":""%> />
                                        <label for="VisitStatisticEnable_True"><%="启用".ToLang()%></label>
                                    </li>
                                    <li>
                                        <input type="radio" id="VisitStatisticEnable_False" name="VisitStatisticEnable" value="0" <%=SiteInfo.VisitStatisticEnable?"":"checked='checked'"%> />
                                        <label for="VisitStatisticEnable_False"><%="不启用".ToLang()%></label>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="VisitStatisticCode"><%="引用代码：".ToLang()%></div>
                            <div class="col-md-10 ">
                                <textarea id="VisitStatisticCode" name="VisitStatisticCode"
                                    rows="10" maxlength="10000"
                                    class="form-control"><%=SiteInfo.VisitStatisticCode%></textarea>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="BaiduUsername"><%="百度统计账号".ToLang()%>：</div>
                            <div class="col-md-5">
                                <input type="text" class="form-control" name="BaiduUserName" placeholder="<%="百度统计账号".ToLang()%>" value="<%=SiteInfo.BaiduUserName%>" />
                            </div>
                            <div class="col-md-5">
                                <input type="password" class="form-control" name="BaiduPwd" placeholder="<%="百度统计密码".ToLang()%>" value="<%=SiteInfo.BaiduPwd.IsEmpty()?"":SiteInfo.BaiduPwd.DecodeBase64Reverse()%>" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="BaiduToken"><%="百度统计Token".ToLang()%>：</div>
                            <div class="col-md-10">
                                <input type="text" class="form-control" name="BaiduToken" value="<%=SiteInfo.BaiduToken%>" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="BaiduSiteId"><%="百度统计站点".ToLang()%>：</div>
                            <div class="col-md-10">
                                <select id="sltBaiduSiteId" name="BaiduSiteId" class="form-control">
                                    <option><%="==请选择==".ToLang()%></option>
                                </select>
                                <textarea class="hidden" name="BaiduSitesJson"><%=SiteInfo.BaiduSitesJson%></textarea>
                                <script type="text/javascript">$(function () { loadSites(<%=SiteInfo.BaiduSitesJson%>); });</script>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="_action" value="Save" />
                                <%if (IsCurrentRoleMenuRes("319"))
                                    { %>
                                <div class="btn-group">
                                    <button type="button" form-action="submit" form-success="refresh" class="btn btn-info "><%="保存".ToLang() %></button>
                                    <button type="button" form-action="submit" form-success="jump" class="btn btn-info "><%="保存并查看当前站点统计信息".ToLang() %></button>

                                </div>
                                <a class="btn btn-white" href="javascript:;" onclick="getSites();"><%="获取统计站点".ToLang()%></a>
                                <%} %>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function getSites() {
            $.ajax({
                type: "POST",
                url: "<%=SysPath%>Handler/Plugin/VisitStatistic.aspx",
                data: {
                    _action: "GetBaiduSites",
                    BaiduUserName: $("[name='BaiduUserName']").val(),
                    BaiduPwd: $("[name='BaiduPwd']").val(),
                    BaiduToken: $("[name='BaiduToken']").val()
                },
                success: function (response) {
                    response = eval("(" + response + ")");
                    if (response.Status == true) {
                        response = eval("(" + response.Message + ")");
                        if (response.header.desc == "system failure") {
                            whir.toastr.error(response.header.failures[0].code + "&nbsp;" + response.header.failures[0].message);
                        } else {
                            var sites = response.body.data[0].list;
                            $("[name='BaiduSitesJson']").val(JSON.stringify(sites));
                            loadSites(sites,
                                function () {
                                    whir.toastr.success("<%="操作成功".ToLang()%>");
                                });
                        }
                    }
                    console.log(response);
                }
            });
        }
        function loadSites(sitesJson, callback) {
            var html = "";
            $(sitesJson).each(function (idx, item) {
                var domain = item.domain, siteId = item.site_id;
                html += '<option value="' + siteId + '">' + domain + '</option>';
            });
            $("#sltBaiduSiteId").html(html);
            $("option[value='<%=SiteInfo.BaiduSiteId%>']").attr("selected", true);
            if (callback) {
                callback();
            }
        }

        $("[form-action='submit']").click(function () {
            var actionSuccess = $(this).attr("form-success");
            var $form = $("#formEdit");
            $form.post({
                success: function (response) {
                    if (response.Status == true) {
                        actionSuccess == "refresh" ?
                            whir.toastr.success(response.Message, true, false) :
                            whir.toastr.success(response.Message, true, false, "Baidu/Overview/GetTrendTimeA.aspx");

                    } else {
                        whir.toastr.error(response.Message);
                    }
                    whir.loading.remove();
                },
                error: function (response) {
                    whir.toastr.error(response.Message);
                    whir.loading.remove();
                }
            });
            return false;
        });
    </script>
</asp:Content>
