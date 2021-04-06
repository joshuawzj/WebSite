<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="whir_system_Main" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrap">
        <div class="space15"></div>
        <asp:Panel ID="PnPassworTips" runat="server">
            <div class="alert alert-danger no-opacity">
                <button data-dismiss="alert" class="close" type="button">×</button>
                <span class="entypo-attention"></span>
                <asp:Label ID="lbPassworTips" runat="server" Text="温馨提示：为了您的帐户安全，请定期修改密码"></asp:Label>
            </div>
        </asp:Panel>
        <div class="row">
            <div class="col-md-4">
                <ul class="list-group">
                    <li class="list-group-item"><span class="entypo-user"></span>&nbsp;<%="个人信息".ToLang()%></li>
                    <li class="list-group-item text-center">
                        <img src="<%=UploadFilePath+ loginUser.Logo %>" onerror="this.src='<%=SysPath %>res/images/defaultLogo.png'" alt="" class="img-circle img-profile" width="128" height="128" />
                    </li>
                    <li class="list-group-item"><%="您好，".ToLang()%><span class="user"><%=CurrentUserName%></span></li>
                    <li class="list-group-item">
                        <span class="pull-right">
                            <strong><%=CurrentUserRolesName %></strong>
                        </span>
                        <%="所属角色：".ToLang()%>
                    </li>
                    <li class="list-group-item">
                        <span class="pull-right">
                            <strong>
                                <asp:Literal ID="ltLastLoginTime" runat="server"></asp:Literal></strong>
                        </span>
                        <%="上次登录时间：".ToLang()%>
                    </li>
                    <li class="list-group-item">
                        <span class="pull-right">
                            <strong>
                                <asp:Literal ID="ltLastLoginIP" runat="server"></asp:Literal></strong>
                        </span>
                        <%="上次登录IP：".ToLang()%>
                    </li>
                </ul>
            </div>
            <div class="col-md-8">
                <ul class="list-group">
                    <li class="list-group-item"><span class="fontawesome-info-sign"></span>&nbsp;<%="安全提示".ToLang()%></li>
                    <li class="list-group-item padding-left30 line-height25">
                        <span class="fontawesome-angle-right"></span>&nbsp;<%="您的数据库数据注意及时备份，建议隔一段时间备份数据".ToLang()%><br />
                        <span class="fontawesome-angle-right"></span>&nbsp;<%="您的网站不要部署在系统磁盘，建议您部署在非系统磁盘上".ToLang()%><br />
                        <span class="fontawesome-angle-right"></span>&nbsp;<%="您的站点下的文件注意及时查杀木马，防止木马、病毒、黑客攻击您的网站".ToLang()%><br />
                        <span class="fontawesome-angle-right"></span>&nbsp;<%="您部署网站时，注意权限的限制，尽可能做到给用户最低的权限".ToLang()%>
                    </li>
                </ul>
                <ul class="list-group">
                    <li class="list-group-item"><span class="entypo-monitor"></span>&nbsp;<%="系统信息".ToLang()%></li>
                    <li class="list-group-item padding-left30 line-height25">
                        <span class="icon-code"></span>&nbsp;<%="程序版本：".ToLang()%><asp:Literal ID="ltVersion" runat="server"></asp:Literal><br />
                        <span class="fontawesome-desktop"></span>&nbsp;<%="服务器系统：".ToLang()%><asp:Literal ID="ltServerOS" runat="server"></asp:Literal><br />
                        <span class="entypo-flow-tree"></span>&nbsp;<%=".NET框架版本：".ToLang()%><asp:Literal ID="ltAspnetVer" runat="server"></asp:Literal><br />
                        <span class="entypo-database"></span>&nbsp;&nbsp;<%="数据库：".ToLang()%><asp:Literal ID="ltDB" runat="server"></asp:Literal>
                    </li>
                </ul>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <ul class="list-group">
                    <li class="list-group-item"><span class="entypo-link"></span>&nbsp;<%="快捷链接".ToLang()%></li>
                    <li class="list-group-item">
                        <div class="row">
                            <%=GetQuickMenu()%>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <%="技术支持：".ToLang()%>
                <a class="" href="http://www.wanhu.com.cn"><span>ezEIP</span></a>
                <a class="" href="http://www.wanhu.com.cn"><span>ezSHOP</span></a>
                <a class="" href="http://www.whir.net"><span>ezOFFICE</span></a>
            </div>
        </div>
        <div class="space15"></div>
    </div>

</asp:Content>

