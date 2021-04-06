<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SurveyInfo.ascx.cs" Inherits="label_survey_SurveyInfo" %>
<b>
    <asp:Literal ID="ltlSurveyTitle" runat="server"></asp:Literal>
</b>
<table width="100%" border="0" cellspacing="0" cellpadding="3">
    <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
        <ItemTemplate>
            <tr>
                <td>
                    <%# Container.ItemIndex + 1 + "． " + Eval("Name")%>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                        <asp:Repeater ID="rptAnswers" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td style="width: 50%;">
                                        <span style="padding-left: 20px;"></span>
                                        <%# Eval("Name")%>
                                    </td>
                                    <td>
                                        <span class="progressbar">
                                            <%# Eval("AnswerCount") %></span> <span style="display: none;">
                                                <%#TotalCount %></span>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
