<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WorkFlowLogs.ascx.cs" Inherits="whir_system_UserControl_ContentControl_WorkFlowLogs" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:PlaceHolder ID="phShowLogs" runat="server">
   <dl class="audit_log">
        <h4><%="审核日志".ToLang()%></h4>
        <ul>
            <asp:Repeater ID="rptList" runat="server">
            <ItemTemplate>
             <li><span><%#Eval("CreateDate","{0:yyyy-MM-dd HH:mm:ss}") %></span><br /><%#Eval("CreateUser")%>:<%#Eval("Describe").ToStr().Replace("审核", "审核".ToLang()).Replace("通过", "通过".ToLang())%></li>
            </ItemTemplate>
            </asp:Repeater> 
        </ul>
    </dl>
</asp:PlaceHolder>