<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Backup.aspx.cs" Inherits="Whir_System_Module_Extension_Backup" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading">
                <%="数据库备份".ToLang()%></div>
            <div class="panel-body">
                <div class="actions btn-group">
                    
                         <%if (IsCurrentRoleMenuRes("333"))
                           { %>
                        <a id="aAdd" href="Backup_Edit.aspx" class="btn btn-white"><%="新建备份".ToLang()%></a>
                        <%} %>
                </div>
                <table id="ta_list" width="100%" border="0" cellspacing="0" cellpadding="0" class="controller table table-bordered table-noPadding">
                    <tr class="biaoti">
                        <th width="200px">
                            <%="备份名称".ToLang()%>
                        </th>
                        <th>
                            <%="备注".ToLang()%>
                        </th>
                        <th width="200px">
                            <%="备份时间".ToLang() %>
                        </th>
                        <th >
                            <%="操作".ToLang() %>
                        </th>
                    </tr>
                    <asp:Repeater ID="rptBackup" runat="server" 
                        OnItemDataBound="rptBackup_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <%# Eval("BackupName")%>
                                </td>
                                <td>
                                    <%# Eval("Remark")%>
                                </td>
                                <td>
                                    <%# Eval("CreateDate") %>
                                </td>
                                <td>
                                 <%if (IsCurrentRoleMenuRes("330"))
                                   { %>
                                     <a name="lbtnRestore" href="javascript:;"
                                        onclick="Restore(<%# Eval("BackupId") %>)" class="btn btn-info"><%="还原".ToLang() %></a>
                                    <%} %>
                                    <%if (IsCurrentRoleMenuRes("331"))
                                      { %>
                                    <asp:Literal ID="litDownload" runat="server"></asp:Literal>
                                    <%} %>
                                    <%if (IsCurrentRoleMenuRes("332"))
                                      { %>
                                     <a name="lbtnDel" href="javascript:;"
                                        onclick="Delete(<%# Eval("BackupId") %>)" class="btn text-danger border-normal"><%="删除".ToLang() %></a>
                                    
                                    <%} %>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
                <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $("a[name='aNon']").click(function () {
            TipMessage('<%="当前需要下载的备份数据不存在".ToLang() %>');
        });
    </script>
    <script type="text/javascript">
        $(function() {
            $("a[name='lbtnDownload']")
                .click(function() {

                    //发送异步请求
                    var aLink = $(this);
                    $.get("<%=SysPath %>ajax/Extension/Backup.aspx",
                        { backupid: $(this).attr("BackupId") },
                        function(data) {
                            if (data != "no") {
                                aLink.attr("href", data);
                            } else {
                                TipMessage("<%=DownloadMsg %>");
                            }
                        });
                });
        });
         //还原
        function Restore(id) {
            whir.dialog.confirm("<%="确认要还原吗？".ToLang() %>",
                function() {
                     whir.dialog.remove();
                     whir.ajax.post("<%=SysPath%>Handler/Module/Extension/Backup.aspx",
                        {
                            data: {
                                _action: "Restore",
                                backupId: id
                            },
                            success: function(response) {
                                whir.loading.remove();
                                if (response.Status == true) {
                                    whir.toastr.success(response.Message, true, false);
                                } else {
                                    whir.toastr.error(response.Message);
                                }
                            }
                        }
                    );
                    return false;
                });
        }
         //删除
        function Delete(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                function() {
                     whir.dialog.remove();
                      whir.ajax.post("<%=SysPath%>Handler/Module/Extension/Backup.aspx",
                        {
                            data: {
                                _action: "Delete",
                                backupId: id
                            },
                            success: function(response) {
                                whir.loading.remove();
                                if (response.Status == true) {
                                    whir.toastr.success(response.Message, true, false);
                                } else {
                                    whir.toastr.error(response.Message);
                                }
                            }
                        }
                    );
                    return false;
                });
        }
    </script>
</asp:Content>
