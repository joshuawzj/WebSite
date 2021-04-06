<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="about.aspx.cs" Inherits="whir_system_module_Other_about" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .panel-body p{ text-indent: 2em;line-height: 27px;padding-left: 10px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="关于".ToLang()%>&nbsp;ezEIP&nbsp;<%=AppSettingUtil.GetString("Version")%></div>
            <div class="panel-body">
                <div class="logo">
                    <p>
                        <%="ezEIP万户网络网站管理系统是万户网络集{0}年网站建设经验，根据企业实际情况度身打造的一套一站式网上解决方案。拥有成熟的后台管理系统和人性化的前台用户体验。在延续万户网络网站建设产品“量身定制、随需而变”特性的基础上，同时兼顾不同行业客户运营中的不同需求，提供差异化创新型平台解决方案。".ToLang().FormatWith(DateTime.Now.Year-1997)%>
                    </p>
                    <p>
                        <%="版本：".ToLang()%><asp:Literal ID="litVer" runat="server"></asp:Literal>
                    </p>
                    <p>
                        <%="官网：".ToLang()%>http://www.wanhu.com.cn
                    </p>
                    <p>
                        <%="全国统一免费服务热线：".ToLang()%>400-888-0035
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
