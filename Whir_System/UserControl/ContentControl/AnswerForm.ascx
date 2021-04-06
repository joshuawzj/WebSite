<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AnswerForm.ascx.cs" Inherits="whir_system_UserControl_ContentControl_AnswerForm" %>
<asp:PlaceHolder ID="PlhRadioList" runat="server" Visible="false">
    <ul class="list">
        <asp:Repeater ID="rplist" runat="server">
            <ItemTemplate>
                <li>
                    <input type="radio" id="ctl00_ContentPlaceHolder1_rptQuestions_ctl00_AnswerForm1_rblList_<%#Eval(PrimaryKeyName) %>"
                        name="ctl00_ContentPlaceHolder_<%# QuestionID %>" value="<%#Eval(PrimaryKeyName) %>" />
                    <label for="ctl00_ContentPlaceHolder1_rptQuestions_ctl00_AnswerForm1_rblList_<%#Eval(PrimaryKeyName) %>">
                        <%#Eval("Name")%></label>
                </li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</asp:PlaceHolder>
<asp:PlaceHolder ID="PlhCheckBoxList" runat="server" Visible="false">
    <ul class="list">
        <asp:Repeater ID="cblList" runat="server">
            <ItemTemplate>
                <li>
                    <input id="rptQuestions_ctl00_Answerform_cblList_<%#Eval(PrimaryKeyName) %>" type="checkbox"
                        name="rptQuestions_ctl00_Answerform_cblList_<%#Eval(PrimaryKeyName) %>" value='<%#Eval(PrimaryKeyName) %>' />
                    <label for="rptQuestions_ctl00_Answerform_cblList_<%#Eval(PrimaryKeyName) %>">
                        <%#Eval("Name")%></label></li>
            </ItemTemplate>
        </asp:Repeater>
    </ul>
</asp:PlaceHolder>
