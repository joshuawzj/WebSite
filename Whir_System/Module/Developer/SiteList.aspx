<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SiteList.aspx.cs" Inherits="Whir_System_Module_Developer_SiteList" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="站点结构".ToLang()%></div>
            <div class="panel-body">
                <div class="actions">
                    <a href="SiteEdit.aspx" class="btn btn-white"><%="添加站点".ToLang()%></a>
                </div>
                <div class="tableCategory-table-body">
                <table width="100%" class="table table-bordered table-noPadding">
                    <thead>
                        <tr class="trClass">
                            <th width="50px">Id</th>
                            <th style="min-width:100px;"><%="站点名称".ToLang() %></th>
                            <th style="min-width:100px;"><%="站点目录".ToLang()%></th>
                            <th style="min-width:100px;"><%="默认站点".ToLang()%></th>
                            <th><%="操作".ToLang()%></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptMultiSite" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("SiteId") %></td>
                                    <td><%#Eval("SiteName") %></td>
                                    <td><%#Eval("Path") %></td>
                                    <td>
                                        <span class="text-primary <%#Eval("IsDefault").ToBoolean()?"fontawesome-ok":""%>"></span>
                                    </td>
                                    <td style="min-width:250px;">
                                        <div class="btn-group">
                                            <a class="btn btn-sm btn-white" href='javascript:void(0);' onclick="openOptionsField('<%# Eval("SiteId") %>');"><%="字段检查".ToLang()%></a>
                                            <a class="btn btn-sm btn-white" href='SiteEdit.aspx?siteid=<%# Eval("SiteId") %>'><%="编辑".ToLang()%></a>
                                            <a class="btn btn-sm btn-white" href="javascript:;" onclick="openCopy('<%# Eval("SiteId") %>');"><%="复制".ToLang()%></a>
                                            <a class="btn btn-sm text-danger border-normal" href="javascript:;" onclick="Delet('<%# Eval("SiteId") %>');"><%="删除".ToLang()%></a>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                </div>
                <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //打开复制栏目页面
        function openCopy(siteId) {
            whir.dialog.frame('<%="请输入站点名称".ToLang() %>', "<%=SysPath%>Module/Column/SiteColumn_Copy.aspx?siteid=" + siteId, null, 500, 400);
        }

        function openOptionsField(siteId) {
            whir.dialog.frame('<%="字段检查".ToLang() %>', "<%=SysPath%>Module/Setting/OptionField.aspx?siteid=" + siteId, null, 600, 500);
        }

        function Delet(id) {
            if (!id) {
                whir.toastr.warning("<%="参数错误".ToLang() %>");
             } else {
                 whir.dialog.confirm('<%="确定要删除吗?".ToLang()%>',function () {
                         whir.ajax.post("<%=SysPath%>Handler/Developer/Site.aspx",
                            {
                                data: {
                                    SiteId: id,
                                    _action: "Delete"
                                },
                                success: function (response) {
                                    whir.loading.remove();
                                    if (response.Status == true) {
                                        whir.toastr.success(response.Message, true);
                                    } else {
                                        whir.toastr.error(response.Message);
                                    }
                                }
                            }
                        );
                        whir.dialog.remove();
                        return false;
                    });
            }
            return false;
        }
    </script>
</asp:Content>
