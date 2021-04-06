<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="site_search.aspx.cs" Inherits="whir_system_module_sitemap_SiteSearch"
    Title="搜索登记页" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading"><%="搜索引擎登记".ToLang()%></div>

            <div class="panel-body">

                <div class="note_yellow">
                    <%="将您的网站登记到各大搜索引擎中，将非常有利于您网站的推广。".ToLang()%><br />
                    <%="以下是常见搜索引擎的登记入口，打开后填写您的网站资料即可能被这些搜索引擎收录。".ToLang() %>
                </div>
                <div class="search_engine">
                    <ul>
                        <li><span><a href="http://www.google.cn/intl/zh-CN/add_url.html" target="_blank">
                            <img src="../../res/images/img_google.jpg" /></a></span> <span><a href="http://www.google.cn/intl/zh-CN/add_url.html"
                                target="_blank">
                                <%="谷歌".ToLang() %></a></span> </li>
                        <li><span><a href="https://ziyuan.baidu.com/linksubmit/url" target="_blank">
                            <img src="../../res/images/img_baidu.jpg" /></a></span> <span><a href="https://ziyuan.baidu.com/linksubmit/url"
                                target="_blank">
                                <%="百度".ToLang() %></a></span> </li>
                        <li><span><a href="https://www.bing.com/toolbox/submit-site-url" target="_blank">
                            <img src="../../res/images/img_yahoo.jpg" /></a></span> <span><a href="https://www.bing.com/toolbox/submit-site-url"
                                target="_blank">
                                <%="雅虎中国".ToLang() %></a></span> </li>
                        <li><span><a href="https://www.bing.com/toolbox/submit-site-url" target="_blank">
                            <img src="../../res/images/img_bing.jpg" /></a></span> <span><a href="https://www.bing.com/toolbox/submit-site-url"
                                target="_blank">
                                <%="必应".ToLang() %></a></span> </li>
                        <li><span><a href="http://info.so.360.cn/site_submit.html" target="_blank">
                            <img src="../../res/images/img_360.png" /></a></span> <span><a href="http://info.so.360.cn/site_submit.html"
                                target="_blank">
                                <%="360搜索".ToLang() %></a></span> </li>
                        <li><span><a href="http://fankui.help.sogou.com/" target="_blank">
                            <img src="../../res/images/img_sogou.jpg" /></a></span> <span><a href="http://fankui.help.sogou.com/"
                                target="_blank">
                                <%="搜狗".ToLang() %></a></span> </li>
                    </ul>
                    <div class="clear">
                    </div>
                </div>
            </div>

        </div>
    </div>
</asp:Content>
