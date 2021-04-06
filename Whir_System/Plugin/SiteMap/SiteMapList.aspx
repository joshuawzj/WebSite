<%@ Page Title="敏感词" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SiteMapList.aspx.cs" Inherits="Whir_System_Plugin_SiteMap_SiteMapList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div class="content-wrap">
            <div class="space15">
            </div>
            <div class="panel">
                <div class="panel-heading"><%="关于站点地图".ToLang()%></div>
                <div class="panel-body">

                    <div><%# DateTime.Now %>
                        <p>
                            <%= "通过站点地图，Google 可以在您的网站上找到通过其他方式可能无法发现的网页。用最简单的话来讲，XML 站点地图（一般就叫站点地图）就是您网站上各网页的列表。创建并提交站点地图有助于确保Google 了解您网站上的所有网页，包括 Google 在正常抓取过程中可能找不到的网址。".ToLang() %>
                        </p>
                        <div class='lightbulb'>
                            <%= "您可以根据".ToLang() %><a href='http://www.sitemaps.org' target='_blank'><%= "站点地图协议".ToLang() %></a><%= "创建站点地图，也可以提交文本文件或 RSS/Atom 供稿作为站点地图。".ToLang() %><a
                                href='https://support.google.com/webmasters/answer/183668?hl=zh-Hans&ref_topic=8476' target='_blank'><%= "如何创建站点地图".ToLang() %></a>。
                        </div>
                        <p><%= "在以下情况下，站点地图特别有用".ToLang() %>：</p>
                        <ul>
                            <li>1、<%= "网站含动态内容".ToLang() %>。</li>
                            <li>2、<%= "您的网站中包含在 Googlebot 抓取过程中不易发现的网页，例如含有富 AJAX 或图片内容的网页".ToLang() %>。</li>
                            <li>3、<%= "网站为新网站且指向该网站的链接不多。（Googlebot 会跟随链接从一个网页到另一个网页抓取网页，因此，如果您的网站没有很好地链接，我们可能很难发现它。）".ToLang() %></li>
                            <li>4、<%= "网站有大量内容页存档，这些内容页彼此之间没有很好地链接，或根本就没有链接。".ToLang() %></li>
                        </ul>
                        <p>
                            <%= "Google 不保证一定会抓取所有网址并将其编入索引。但是，我们会使用站点地图中的数据了解网站的结构，这样可以让我们改进抓取工具的计划，并在日后能更好地对网站进行抓取。大多数情况下，网站管理员会因提交站点地图而受益，而不会为此受到处罚。".ToLang() %>
                        </p>
                        <p>
                            <%= "Google 遵守".ToLang() %><a href='http://www.sitemaps.org' target='_blank'>sitemaps.org</a>
                            <%= "所定义的站点地图协议 0.9。因此，使用站点地图协议0.9 为 Google 创建的站点地图和其他采用 sitemaps.org 标准的搜索引擎兼容".ToLang() %>。
                        </p>
                        <p>
                            <%= "站点文件生成后，把文件提交给".ToLang() %><a href='https://support.google.com/webmasters/answer/183669?hl=zh-Hans&ref_topic=8476' target='_blank'><%= " Google".ToLang() %></a>。<%= "这样可以让 Google 向您提供实用的状态信息和统计信息".ToLang() %>。
                        </p>
                    </div>
                </div>
            </div>
            <div class="operate_head">
                <span>
                    <%if (IsCurrentRoleMenuRes("369"))
                        { %>
                    <asp:LinkButton ID="lbnAdd" runat="server" CssClass="btn btn-primary" OnCommand="lbnAdd_Command" CommandName="Sort"><b><%="生成SiteMap".ToLang() %></b></asp:LinkButton>
                    <%} %>
                </span>
            </div>
        </div>
    </form>
</asp:Content>
