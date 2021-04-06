<%@ Control Language="C#" AutoEventWireup="true" CodeFile="VoteInfo.ascx.cs" Inherits="label_vote_VoteInfo" %>
<b>
    <asp:Literal ID="ltlVoteTitle" runat="server"></asp:Literal>
</b>
<table width="100%" border="0" cellspacing="0" cellpadding="3">
    <asp:Repeater ID="rptAnswers" runat="server">
        <ItemTemplate>
            <tr>
                <td style="width: 50%;">
                    <span style="padding-left: 20px;"></span>
                    <%# Container.ItemIndex + 1 + "． " + Eval("Name")%>
                </td>
                <td>
                    <span class="progressbar">
                        <%# Eval("AnswerCount") %>
                    </span><span style="display: none;">
                        <%#TotalCount%>
                    </span>
                </td>
            </tr>
        </ItemTemplate>
    </asp:Repeater>
</table>
