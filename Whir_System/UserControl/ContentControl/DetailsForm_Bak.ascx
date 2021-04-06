<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DetailsForm_Bak.ascx.cs"
    Inherits="whir_system_UserControl_ContentControl_DetailsForm_Bak" %>
    <%@ Import Namespace="Whir.Language" %>
<asp:Repeater ID="rptFormLeft" runat="server">
    <ItemTemplate>
        <div class="form-group" runat="server" id="divgroup">
            <div class="col-md-3   control-label" >
                <%# Eval("FieldAlias") %>：
            </div>
            <div class="col-md-9" >
                <label runat="server" id="tdDynamicFormInput">
                </label>
            </div>
              <div class="clear" ></div>
        </div>
    </ItemTemplate>
</asp:Repeater>
<asp:Repeater ID="rptFormRight" runat="server">
    <ItemTemplate>
        <div class="form-group" runat="server" id="divgroup">
            <div class="col-md-6   control-label" >
                <%# Eval("FieldAlias") %>：
            </div>
            <div class="col-md-6" >
                <label runat="server" id="tdDynamicFormInput">
                </label>
            </div>
              <div class="clear" ></div>
        </div>
    </ItemTemplate>
</asp:Repeater>
