<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="paymentlist.aspx.cs" Inherits="whir_system_Plugin_payment_paymentlist" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <dl class="title_column">
            <a href="paymentlist.aspx" class="aSelect"><b><%="支付方式管理".ToLang() %></b></a>
        </dl>
        <div class="line_border">
        </div>
        <div class="All_list">
            <table id="ta_list" width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <th width="5%">
                      <div class="th-inner ">  <input id="chkAll" name="cb_Top" type="checkbox" onclick="whir.checkbox.selectAll(this,'cb_Position');" title="<%="全选/全不选".ToLang()%>" /></div>
                    </th>
                    <th width="200px"><%="支付方式名称".ToLang() %></th>
                    <th width="80px"><%="是否启用".ToLang() %></th>
                    <th><%="描述".ToLang() %></th>
                    <th width="100px"><%="操作".ToLang() %></th>
                </tr>
                <asp:Repeater ID="rpPayment" runat="server" OnItemCommand="rpPayment_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="checkbox" name="cb_Position" value='<%# Eval("id")%>' />
                            </td>
                            <td>
                                <%# Eval("name")%>
                            </td>
                            <td>
                                <%# GetTrueOrFalseImg(Eval("IsOpen"))%>
                            </td>
                            <td>
                                <%# Eval("introduce")%>
                            </td>
                            <td>
                                <a href='payment_edit.aspx?ID=<%#Eval("id") %>'>[<%="编辑".ToLang() %>]</a>
                                <asp:LinkButton ID="lbStart" CommandName="start" CommandArgument='<%# Eval("id")%>' runat="server" Text='<%# Eval("isopen").ToBoolean()?"["+"禁用".ToLang()+"]":"["+"启用".ToLang()+"]"%>'></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
              <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
        </div>
        <div class="operate_foot">
            <a href="javascript:whir.checkbox.selectAll(null,'cb_Position');"><%="全选".ToLang() %></a>/<a href="javascript:whir.checkbox.cancelSelectAll(null,'cb_Position');"><%="取消".ToLang() %></a>
            <asp:LinkButton ID="lbStart" runat="server" CommandName="start" CssClass="aLink"
                OnCommand="Link_Command"><b><%="是否启用".ToLang()%></b></asp:LinkButton>
            <asp:LinkButton ID="lbStop" runat="server" CommandName="stop" CssClass="aLink" OnCommand="Link_Command"><b><%="批量禁用".ToLang() %></b></asp:LinkButton>
        </div>
    </div>
</asp:Content>
