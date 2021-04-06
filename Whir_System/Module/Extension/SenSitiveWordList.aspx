<%@ Page Title="敏感词" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="sensitivewordlist.aspx.cs" Inherits="Whir_System_Module_Extension_SensitiveWord_List" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        //批量选中删除
        function deleteAction() {
            getSelected();
            if ($("#<%= hidSelected.ClientID %>").val() == '') {
                TipMessage('<%="请选择".ToLang()%>');
                return false;
            }
            return true;
        }
        //获取选择的值
        function getSelected() {
            var selected = whir.checkbox.getSelect("cb_Log");
            $("#<%= hidSelected.ClientID %>").val(selected);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading">
                <%="敏感词设置".ToLang()%>
            </div>
            <div class="panel-body">
                <form id="Form1" runat="server">
                    <div class="actions">
                        <%if (IsCurrentRoleMenuRes("346"))
                            { %>
                        <a href="SenSitiveWord_Edit.aspx" class="btn btn-white">
                            <%="添加敏感词".ToLang()%></a>
                        <%} %>
                    </div>
                    <div class="tableCategory-table-body">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-bordered table-noPadding">
                            <thead>
                                <tr class="biaoti">
                                    <th width="50px">Id
                                    </th>
                                    <th>
                                        <%="敏感词".ToLang() %>
                                    </th>
                                    <th width="80px">
                                        <%="操作者".ToLang() %>
                                    </th>
                                    <th width="180px">
                                        <%="创建时间".ToLang() %>
                                    </th>
                                    <th>
                                        <%="操作".ToLang() %>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptSensitiveWordList" runat="server" OnItemCommand="rptList_ItemCommand"
                                    OnItemDataBound="rptList_ItemDataBound">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# Eval("SensitiveWordId")%>
                                            </td>
                                            <td style="min-width: 150px;">
                                                <%# Eval("SensitiveWordName")%>
                                            </td>
                                            <td>
                                                <%# Eval("CreateUser")%>
                                            </td>
                                            <td style="min-width: 200px;">
                                                <%# Eval("CreateDate") %>
                                            </td>
                                            <%-- <td width="80px">--%>
                                            <td style="min-width: 130px;">
                                                <div class="bth-group">
                                                <%if (IsCurrentRoleMenuRes("347"))
                                                    { %>
                                                <a name="aEdit" class="btn btn-sm btn-white" href="SenSitiveWord_Edit.aspx?sensitivewordid=<%# Eval("SensitiveWordId")  %>">
                                                    <%="编辑".ToLang()%></a>
                                                <%} %>
                                                <%if (IsCurrentRoleMenuRes("180"))
                                                    { %>
                                                <asp:LinkButton ID="lbtnDel" CssClass="btn btn-sm text-danger border-normal" name="lbtnDel" runat="server" CommandName="del" CommandArgument='<%# Eval("SensitiveWordId") %>'><%="删除".ToLang() %></asp:LinkButton>
                                                <%} %>
                                                </div>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
                    <input type="hidden" runat="server" id="hidSelected" />
                </form>
            </div>
        </div>
    </div>
</asp:Content>
