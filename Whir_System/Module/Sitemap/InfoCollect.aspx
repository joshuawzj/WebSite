<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="infocollect.aspx.cs" Inherits="whir_system_module_sitemap_infocollect" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs" role="tablist">
                <li class="active"><a  data-toggle="tab"  href="#siteInfo" onclick="tabLoadData(1);">
                    <%="站点信息统计".ToLang()%></a> </li>
                <li class=""><a  data-toggle="tab"   href="#userInfo" onclick="tabLoadData(2);">
                    <%="管理员信息统计".ToLang()%></a></li>
                <li class=""><a data-toggle="tab"  href="#roleInfo" onclick="tabLoadData(3);">
                    <%="角色信息统计".ToLang()%></a></li>
            </ul>
            <br />
                <table class="InfoCollect-table">
                    <tr style="vertical-align: middle;">
                        <td>
                            <%="发布时间".ToLang() %>
                            &nbsp;
                        </td>
                        <td>
                            <input type="text" class="form-control form_datetime text_date" id="txtBeginTime"
                                name="txtBeginTime" />
                        </td>
                        <td>
                            &nbsp;-&nbsp;
                        </td>
                        <td>
                            <input type="text" class="form-control form_datetime text_date" id="txtEndTime" name="txtEndTime" />
                        </td>
                        <td>
                            &nbsp; <a id="btnSearch" class="btn btn-sm btn-primary" href="javascript:Search();">
                                <%="搜索".ToLang()%></a>
                        </td>
                    </tr>
                </table>
                <br/>
                <div class="tab-content">
                    <div id="siteInfo" class="tab-pane active">
                        <table width="100%" class="table table-bordered table-noPadding">
                            <thead>
                                <tr class="trClass">
                                    <th>
                                        <%="站点名称".ToLang()%>
                                    </th>
                                    <th>
                                        <%="数量".ToLang()%>
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="tbdSiteInfo">
                            </tbody>
                        </table>
                    </div>
                    <div id="userInfo" class="tab-pane">
                        <table width="100%" class="table table-bordered table-noPadding">
                            <thead>
                                <tr class="trClass">
                                    <th>
                                        <%="管理员名称".ToLang()%>
                                    </th>
                                    <th>
                                        <%="数量".ToLang()%>
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="tbUserInfo">
                            </tbody>
                        </table>
                    </div>
                    <div id="roleInfo" class="tab-pane">
                        <table width="100%" class="table table-bordered table-noPadding">
                            <thead>
                                <tr class="trClass">
                                    <th>
                                        <%="角色名称".ToLang()%>
                                    </th>
                                    <th>
                                        <%="数量".ToLang()%>
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="tbRoleInfo">
                            </tbody>
                        </table>
                    </div>
                </div>
                <div class="operate_head">
                    <%-- <asp:LinkButton ID="btnExport" runat="server" OnClick="btnExport_Click" CssClass="aLink"><em><img src="<%=SysPath %>res/images/button_submit_icon_4.gif"></em><b><%="导出".ToLang() %></b>
                    </asp:LinkButton>--%>
                </div>
            </div>
        </div>
    </div>
    </form>
     
    <script type="text/javascript">
       
        // 日历
        $('.form_datetime').datetimepicker({
            minView: "month", //选择日期后，不会再跳转去选择时分秒 
            format: 'yyyy-mm-dd',
            todayBtn: 1,
            autoclose: 1
        });

        $(document).ready(function () {
            getSiteInfoCollect();
            
        });
        //切换tab加载数据
        function tabLoadData(value) {
            if (value==1)
                getSiteInfoCollect();
            if (value == 2)
                getUserInfoCoolect();
            if (value == 3)
                getRoleInfoCollect();
        }

        function Search() {
            if ($(".active").find("a").attr("href") == "#siteInfo")
            getSiteInfoCollect();
            if ($(".active").find("a").attr("href") == "#userInfo")
            getUserInfoCoolect();
            if ($(".active").find("a").attr("href") == "#roleInfo")
            getRoleInfoCollect();
        }

        //获取站点信息统计
        function getSiteInfoCollect() {
            whir.ajax.post('<%= SystemPath%>Handler/Sitemap/InfoCollect.aspx', {
                data: {
                    _action: "GetSiteInfoCollect",
                    begintime: $("#txtBeginTime").val(),
                    endtime: $("#txtEndTime").val()
                },
                success: function (result) {
                    if (result.Status) {
                        whir.loading.remove();
                        var html = "";
                        var siteInfo = eval(result.Message);
                        for (var i = 0; i < siteInfo.length; i++) {
                            html += "<tr><td>";
                            html += siteInfo[i].SiteName;
                            html += "</td><td>";
                            html += siteInfo[i].Count;
                            html += "</td><tr>";
                        }
                        $("#tbdSiteInfo").html(html);
                    }
                }
            });
            whir.loading.remove();
        }
        //获取管理员信息统计
        function getUserInfoCoolect() {
            whir.ajax.post('<%= SystemPath%>Handler/Sitemap/InfoCollect.aspx', {
                data: {
                    _action: "GetUserInfoCoolect",
                    begintime: $("#txtBeginTime").val(),
                    endtime: $("#txtEndTime").val()
                },
                success: function (result) {
                    if (result.Status) {
                        whir.loading.remove();
                        var html = "";
                        var siteInfo = eval(result.Message);
                        for (var i = 0; i < siteInfo.length; i++) {
                            html += "<tr><td>";
                            html += siteInfo[i].LoginName;
                            html += "</td><td>";
                            html += siteInfo[i].Count;
                            html += "</td><tr>";
                        }

                        $("#tbUserInfo").html(html);
                    }
                }
            }); 
            whir.loading.remove();

        }

        //获取角色信息统计
        function getRoleInfoCollect() {
            whir.ajax.post('<%= SystemPath%>Handler/Sitemap/InfoCollect.aspx', {
                data: {
                    _action: "GetRoleInfoCollect",
                    begintime: $("#txtBeginTime").val(),
                    endtime: $("#txtEndTime").val()
                },
                success: function (result) {
                    if (result.Status) {
                        whir.loading.remove();
                        var html = "";
                        var siteInfo = eval(result.Message);
                        for (var i = 0; i < siteInfo.length; i++) {
                            html += "<tr><td>";
                            html += siteInfo[i].RoleName;
                            html += "</td><td>";
                            html += siteInfo[i].Count;
                            html += "</td><tr>";
                        }

                        $("#tbRoleInfo").html(html);
                    }
                }
            });
            whir.loading.remove();
        }

         
    </script>
</asp:Content>
