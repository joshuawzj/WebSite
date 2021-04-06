<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="auditactivitylist.aspx.cs" Inherits="Whir_System_Module_Extension_AuditActivityList" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> 
    <script type="text/javascript">
        $(function () {
            var isAdd = '<%=IsAdd %>'.toLowerCase() == 'true' ? true : false;
            var isDelete = '<%=IsDelete %>'.toLocaleLowerCase() == 'true' ? true : false;
            var isEdit = '<%=IsEdit %>'.toLocaleLowerCase() == 'true' ? true : false;

            //是否具有添加流程节点功能
            if (isAdd)
                $("#aAdd").css("display", "");
            else
                $("#aAdd").css("display", "none");

            //是否具有修改功能
            if (isEdit)
                whir.control.getAllControlByNameToEnabled("a", "name", "aEdit");
            else
                whir.control.getAllControlByNameToDisabled("a", "name", "aEdit");
            
            //是否具有删除功能
            if (isDelete)
                whir.control.getAllControlByNameToEnabled("a", "name", "lbtnDel");
            else
                whir.control.getAllControlByNameToDisabled("a", "name", "lbtnDel");
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="workflowlist.aspx" aria-expanded="true"><%="工作流设置".ToLang()%></a></li>
                    <li class="active"><a data-toggle="tab" href="#single" aria-expanded="true"><%=CurrentWorkFlow.WorkFlowName+" - 流程节点".ToLang()%> </a></li>
                </ul>
                <div class="space15"></div>
                <form id="Form1" runat="server">
                    <div class="actions">
                        <a href="AuditActivity_Edit.aspx?workflowid=<%=RequestUtil.Instance.GetQueryInt("workflowid",0) %>" class="btn btn-white"><%="添加流程节点".ToLang()%></a>
                    </div>

                <div class="tableCategory-table-body">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="table table-bordered table-noPadding">
                        <thead>
                            <tr class="trClass">
                                <th>Id</th>
                                <th><%="节点名称".ToLang()%></th>
                                <th><%="上一节点".ToLang()%></th>
                                <th><%="下一节点".ToLang()%></th>
                                <th><%="添加时间".ToLang()%></th>
                                <th><%="管理操作".ToLang()%></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand" OnItemDataBound="rptList_ItemDataBound">
                                <ItemTemplate>
                                    <tr>
                                        <td><%#Eval("ActivityId")%></td>
                                        <td style="min-width:100px;"><%#Eval("ActivityName")%></td>
                                        <td style="min-width:130px;">
                                            <%#Eval("PreActivityName").ToStr() == "" ? "无".ToLang() : Eval("PreActivityName")%>
                                        </td>
                                        <td style="min-width:130px;">
                                            <%#Eval("NextActivityName").ToStr() == "" ? "无".ToLang() : Eval("NextActivityName")%>
                                        </td>
                                        <td>
                                            <span style="white-space: nowrap;"><%#Eval("CreateDate","{0:yyyy-MM-dd HH:mm:ss}")%></span>
                                        </td>
                                        <td style="min-width:140px;">
                                            <div class="btn-group">
                                                <a href="AuditActivity_Edit.aspx?activityid=<%# Eval("ActivityId")  %>&workflowid=<%# Eval("WorkflowId")  %>" class="btn btn-sm btn-white"><%="编辑".ToLang()%></a>
                                                <asp:LinkButton ID="lbtnDel" name="lbtnDel" runat="server" CommandName="del" CssClass="btn btn-sm text-danger border-normal" CommandArgument='<%# Eval("ActivityId") %>'> <%="删除".ToLang()%></asp:LinkButton>
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
