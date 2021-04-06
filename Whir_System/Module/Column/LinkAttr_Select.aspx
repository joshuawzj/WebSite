<%@ Page Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="LinkAttr_Select.aspx.cs" Inherits="Whir_System_Module_Column_LinkAttr_Select" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <div class="form_center" style="width: 100%">
                    <div class="form-group">
                    <ul class="list">
                        <asp:Repeater runat="server" ID="rptLinkList">
                            <ItemTemplate>
                                <li>
                                        <input id='formid<%# Eval("FormId") %>' name="linkattr" type="radio" value="<%# Eval("FormId") %>" />
                                        <label for="formid<%# Eval("FormId") %>">
                                            <%# Eval("ColumnName") %>[<%#Eval("ColumnId") %>][<%# Eval("FieldAlias")%>]</label>
                                </li>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Literal ID="ltNodata" runat="server"></asp:Literal>
                    </ul>
                            </div>
                    
                </div>
            </div>
        </div>
    </div>
</asp:Content>
