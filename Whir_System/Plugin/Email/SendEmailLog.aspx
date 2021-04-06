<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SendEmailLog.aspx.cs" Inherits="Whir_System_Plugin_Email_SendEmailLog" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        //批量选中
        function selectAction() {
            if (!whir.checkbox.isSelect('cb_Position')) {
                TipMessage('请选择');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form id="Form1" runat="server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
             
            <div class="panel-body">
                 
                 <ul class="nav nav-tabs">
                     <li><a href="EmailMass.aspx" aria-expanded="true"><%="邮件群发".ToLang()%></a></li>
                     <li class="active"><a  data-toggle="tab" aria-expanded="true"><%="发送记录".ToLang()%></a></li>
                  </ul>
                <br />
                <div class="tableCategory-table-body">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-bordered table-noPadding">
                    <thead>
                        <tr>
                            <th width="120" style="min-width:100px;"><%="发送方式".ToLang() %></th>
                            <th ><%="邮件标题".ToLang() %></th>
                            <th width="120" style="min-width:90px;"><%="状态".ToLang() %></th>
                            <th width="180" style="min-width:180px;"><%="发送时间".ToLang() %></th>
                            <th width="220"><%="操作".ToLang() %></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rpList" OnItemDataBound="rpList_ItemDataBound" OnItemCommand="rpList_ItemCommand">
                            <ItemTemplate>
                                <tr>
                                    <td style="min-width:100px;">
                                        <%# ChangeCategory(Eval("Category").ToInt())%>
                                    </td>
                                    <td style="min-width:100px;">
                                        <%#Eval("Title")%>
                                    </td>
                                    <td>
                                        <%# ChangeSendState(Eval("SendState").ToInt())%>
                                    </td>
                                    <td>
                                        <%# Eval("CreateDate")%>
                                    </td>
                                    <td   style="min-width:100px;">
                                        <div class="btn-group">
                                            <%if (IsCurrentRoleMenuRes("365"))
                                                { %>
                                            <asp:LinkButton CssClass="btn btn-sm btn-white" ID="lbResend"
                                                CommandName="resend" CommandArgument='<%# Eval("ModelId")%>'
                                                runat="server" Text='<%# Eval("SendState").ToInt()==3?"发送".ToLang():"重新发送".ToLang() %>'> 
                                            </asp:LinkButton>
                                            <%} %>
                                            <%if (IsCurrentRoleMenuRes("366"))
                                                { %>
                                            <a class="btn btn-sm btn-white" href="SendEmailLogEdit.aspx?RecordId=<%# Eval("ModelId") %>"><%="编辑".ToLang()%></a>
                                            <%} %>
                                            <%if (IsCurrentRoleMenuRes("367"))
                                                { %>
                                            <asp:LinkButton ID="lbDel" CssClass="btn btn-sm text-danger border-normal" CommandName="del" CommandArgument='<%# Eval("ModelId")%>' runat="server" Text="删除"></asp:LinkButton>
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
            </div>
        </div>
    </div>
    </form>
</asp:Content>
