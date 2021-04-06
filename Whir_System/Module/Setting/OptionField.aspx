<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="OptionField.aspx.cs" Inherits="Whir_System_Module_Setting_OptionField" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
   <div class="content-wrap" id="OptionField">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="字段检查".ToLang()%></div>
            <div class="panel-body">
            <div class="tableCategory-table-body">
                <table width="100%" id="tableList" class="controller table table-bordered table-noPadding">
                    <thead>
                <tr class="trClass">
                    <th>
                        <%="栏目".ToLang() %>
                    </th>
                    <th>
                        <%="数据库字段名".ToLang()%>
                    </th>
                    <th>
                        <%="表单别名".ToLang() %>
                    </th>
                    <th>
                        <%="绑定类型".ToLang()%>
                    </th>
                </tr>
                 </thead>
                   <tbody>
                <asp:Repeater ID="rpList" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("ColumnName") %>
                                <em style="font-family: 宋体; color: #ff5a00;font-style: normal; font-size: 12px;">[<%# Eval("ColumnId") %>]</em>
                            </td>
                            <td>
                                <%# Eval("FieldName") %>
                            </td>
                            <td>
                                <%# Eval("FieldAlias") %>
                            </td>
                            <td>
                                <%# Tran(Eval("BindType").ToString()) %>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                  </tbody>
            </table>
            </div>
            <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
      
        <div class="pages">
            <whir:WhirPager ID="AspNetPager1" runat="server" OnPageChanged="PageChanged" 
                NumericButtonCount="5" PageSize="10"
                UrlPaging="True" AlwaysShow="True" 
                LayoutType="Div" CssClass="btn-group pageNumA"
                CustomInfoClass="page_total" ShowCustomInfoSection="Left" 
                PagingButtonsClass="btn btn-white" CurrentPageButtonClass="btn btn-primary"
                FirstLastButtonsClass="btn btn-white first-last" PrevNextButtonsClass="btn btn-white prev-next"
                >
            </whir:WhirPager>
        </div>
        </div>
    </div>
    </div>
</asp:content>
