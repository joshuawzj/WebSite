<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="systemloglist.aspx.cs" Inherits="Whir_System_Module_log_systemloglist" %>
    <%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../../res/js/DatePicker/WdatePicker.js" type="text/javascript"></script>
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
    <div class="mainbox">
        <dl class="title_column">
            <a href="operationloglist.aspx"><b><%="网站操作日志".ToLang() %></b></a> <em class="line"></em><a href="templateloglist.aspx">
                <b><%="模板操作日志".ToLang() %></b></a> <em class="line"></em><a href="systemloglist.aspx" class="aSelect">
                    <b><%="系统运行日志".ToLang() %></b></a>
        </dl>
        <div class="line_border">
        </div>
        <div class="All_list">
            <table id="ta_list" width="100%" border="0" cellspacing="0" cellpadding="0" class="Table">
                <tr class="biaoti">
                    <th width="20" align="center">
                      <div class="th-inner ">  <input id="chkAll" name="cb_Top" type="checkbox" onclick="whir.checkbox.selectAll(this,'cb_Position');"
                            title="<%="全选/全不选".ToLang()%>" /></div>
                    </th>
                    <th width="50px">
                        Id
                    </th>
                    <th width="140">
                        <%="发生时间".ToLang() %>
                    </th>
                    <th width="80">
                        <%="操作人".ToLang() %>
                    </th>
                    <th>
                        <%="描述".ToLang() %>
                    </th>
                    <th >
                        <%="操作".ToLang() %>
                    </th>
                </tr>
                <asp:Repeater ID="rptOperationLogs" runat="server" OnItemDataBound="rptOperationLogs_ItemDataBound"
                    OnItemCommand="rptOperationLogs_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <input type="checkbox" name="cb_Position" value='<%# Eval("OperateId")%>' />
                            </td>
                            <td>
                                <%# Eval("OperateId")%>
                            </td>
                            <td>
                                <%# Eval("CreateDate")%>
                            </td>
                            <td>
                                <%# Eval("CreateUser")%>
                            </td>
                            <td>
                                <%# Eval("Description") %>
                            </td>
                            <td width="<%#LanguageHelper.GetSplitValue("40|40|70").ToInt() %>px">
                            <%if (IsCurrentRoleMenuRes("334"))
                              { %>
                                <asp:LinkButton ID="lbDelete" name="lbDelete" CommandName="del" CommandArgument='<%# Eval("OperateId") %>' runat="server">删除</asp:LinkButton>
                            <%} %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
               <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
        </div>
        <div class="pages">
            <whir:whirpager id="AspNetPager1" runat="server" onpagechanged="PageChanged" numericbuttoncount="5"
                urlpaging="True" alwaysshow="True" layouttype="Table" pagesize="10" showcustominfosection="Left"
                currentpagebuttonclass="" custominfoclass="Page_total" custominfosectionwidth=""
                custominfostyle=""></whir:whirpager>
        </div>
        <div class="operate_foot">
            <a href="javascript:whir.checkbox.selectAll(null,'cb_Position');"><%="全选".ToLang() %></a>/<a href="javascript:whir.checkbox.cancelSelectAll(null,'cb_Position');"><%="取消".ToLang() %></a>
            <%if (IsCurrentRoleMenuRes("334"))
              { %>
            <asp:LinkButton ID="lbDel" runat="server" OnCommand="Operate_Commad" CommandArgument="del"
                CssClass="aLink"><b>批量删除</b></asp:LinkButton>
            <asp:LinkButton ID="lbClear" CssClass="aLink" OnCommand="Operate_Commad" CommandArgument="clear"
                runat="server"><b><%="清空日志".ToLang() %></b></asp:LinkButton>
                <%} %>
        </div>
    </div>
</asp:Content>
