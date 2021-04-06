<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DetailsForm.ascx.cs" Inherits="whir_system_UserControl_ContentControl_DetailsForm" %>
    <asp:Repeater ID="rptFormLeft" runat="server">
        <ItemTemplate>
            <div class="form-group" runat="server" id="divgroup">
                <div class="col-md-3  control-label" >
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
            <div class="  form-group  " runat="server" id="divgroup">
                <div  class="col-md-5  control-label" >
                    <%# Eval("FieldAlias") %>：
                </div>
                <div   class="col-md-7"  >
                    <label runat="server" id="tdDynamicFormInput">
                    </label>
                </div>
                 <div class="clear" ></div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

  