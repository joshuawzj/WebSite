<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="WorkFlowList.aspx.cs" Inherits="Whir_System_Module_Extension_WorkflowList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="工作流设置".ToLang()%></div>
            <div class="panel-body">
                <form id="Form1" runat="server">
                    <div class="actions">
                        <%if (IsCurrentRoleMenuRes("342"))
                            { %>
                        <a href="WorkFlow_Edit.aspx" class="btn btn-white"><%="添加工作流".ToLang()%></a>
                        <%} %>
                    </div>
                <div class="tableCategory-table-body">
                <table  width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-bordered table-noPadding workflowlist_wap">
                    <thead>
                        <tr class="trClass">
                            <th width="50px">Id</th>
                            <th><%="工作流名称".ToLang()%></th>
                            <th><%="描述".ToLang()%></th>
                            <th><%="添加时间".ToLang()%></th>
                            <th><%="管理操作".ToLang()%></th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td><%#Eval("WorkFlowId")%></td>
                                    <td><%#Eval("WorkFlowName")%></td>
                                    <td><%#Eval("Description")%></td>
                                    <td><%#Eval("CreateDate","{0:yyyy-MM-dd HH:mm:ss}")%></td>
                                    <td>
                                        <div class="btn-group">
                                            <%if (IsCurrentRoleMenuRes("345"))
                                                { %>
                                            <a class="btn btn-sm btn-white" name="aSetWorkflowNode" href="AuditActivityList.aspx?workflowid=<%#Eval("WorkFlowId")%>"><%="设置流程节点".ToLang()%></a>
                                            <%} %>
                                            <%if (IsCurrentRoleMenuRes("343"))
                                                { %>
                                            <a class="btn btn-sm btn-white" name="aEdit" href="WorkFlow_Edit.aspx?workflowid=<%#Eval("WorkFlowId")%>"><%="编辑".ToLang()%></a>
                                            <%} %>
                                            <%if (IsCurrentRoleMenuRes("344"))
                                                { %>
                                            <asp:LinkButton CssClass="btn btn-sm text-danger border-normal" ID="lbtnDel" name="lbtnDel" runat="server"
                                                CommandName="del" CommandArgument='<%# Eval("WorkFlowId") %>'><%="删除".ToLang()%></asp:LinkButton>
                                            <%} %>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                </div>
                <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
                </form>
            </div>
        </div>
    </div>
</asp:Content>
