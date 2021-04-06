<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="killgymhorse.aspx.cs" Inherits="whir_system_module_extension_killgymhorse" %>

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
                <div class="panel panel-default">
                    <div class="panel-heading">
                        &nbsp;<%="注意事项：".ToLang()%>
                    </div>
                    <div class="panel-body">
                        1、<%="有条件的用户把“".ToLang()%><%=UploadFilePath%><%="”目录设置为不允许执行脚本。".ToLang() %><br />
                        2、<%="本检测程序以开发模式为标准，如果您的网站目录包含其它系统，此检测程序可能会产生错误判断。".ToLang()%><br />
                        3、<%="检测程序会跳过对缓存目录的检测，为了安全起见，检测完成后建议清空目录缓存。".ToLang()%><br />
                        4、<%="asp、php、jsp格式文件默认全部归为可疑问题。".ToLang()%><br />
                        5、<%="检查需要花较多时间，请耐心等待。".ToLang()%><br />
                        6、<%="检测结果不存在100%，需要人工对可疑文件进行确认。".ToLang()%>
                    </div>
                </div>
                <div class="form-horizontal">
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="MetaTitle">
                            <%="文件类型：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <div class="input-group uploadimages-input-group">
                                <asp:TextBox ID="txtFileType" runat="server" CssClass="form-control"
                                    value="aspx|ascx|js"></asp:TextBox>
                                <span class="input-group-addon"><%="格式类似“aspx|ascx|js”，以“|”分隔".ToLang()%></span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="MetaTitle">
                            <%="木马代码特征：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                             <asp:TextBox ID="txtCodeType" runat="server" CssClass="form-control"
                                        value="com|system|exec|eval|escapeshell|cmd|passthru|base64_decode|gzuncompress"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10 ">
                         <%if (IsCurrentRoleMenuRes("336"))
                           { %>
                            <asp:LinkButton ID="lbtnCheck" runat="server" class="btn btn-info" OnClick="lbtnCheck_Click"><%="开始检测".ToLang()%></asp:LinkButton>
                            <%} %>
                             <%if (IsCurrentRoleMenuRes("337"))
                               { %>
                            <asp:LinkButton ID="lbtnClearCache" runat="server" class="btn text-danger border-danger" OnClick="lbtnClearCache_Click"><%="清除缓存".ToLang()%></asp:LinkButton>
                        <%} %>
                        </div>
                    </div>
                </div>
                <div class="panel panel-body">
                
                <div class="tableCategory-table-body">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-bordered table-noPadding">
                        <tr>
                            <th style=" min-width:150px;"><%="可疑文件路径".ToLang()%></th>
                            <th width="300px" style=" min-width:60px;"><%="说明".ToLang()%></th>
                            <th width="120px" style=" min-width:60px;"><%="操作".ToLang() %></th>
                        </tr>
                        <asp:Repeater ID="rptFileInfos" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td style=" min-width:150px;">
                                        <%# Eval("FileName")%>
                                    </td>
                                    <td>
                                        <%# Eval("Gymhorse")%>
                                    </td>
                                    <td style=" min-width:170px;">
                                        <a target='_blank' href='file_edit.aspx?filepath=/<%#UrlEncode(Eval("FileName")) %>'><%="阅读代码".ToLang()%></a> 
                                        <a href='http://www.wanhu.com.cn/'><%="解决".ToLang()%></a>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                    </div>
                    <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
                </div>
            </div>
        </div>
    </div>
    </form>
</asp:Content>
