<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="auditactivity_edit.aspx.cs" Inherits="Whir_System_Module_Extension_WorkFlow_Edit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="workflowlist.aspx" aria-expanded="true"><%="工作流设置".ToLang()%></a></li>
                    <li><a href="auditactivitylist.aspx?workflowid=<%=WorkFlowId %>" aria-expanded="true"><%=CurrentWorkFlow.WorkFlowName+" - 流程节点".ToLang()%> </a></li>
                    <li class="active"><a data-toggle="tab" href="#single" aria-expanded="true"><%=(ActivityId==0? "添加流程节点" :"编辑流程节点").ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Extension/AuditActivity.aspx">
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="ActivityName">
                                <%="节点名称：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="ActivityName" name="ActivityName" value=""
                                    class="form-control" required="true" maxlength="64" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="PreActivityId">
                                <%="上一节点：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <select id="PreActivityId" name="PreActivityId" class="form-control">
                                    <%=PreActivityOption %>
                                </select>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-2 control-label" for="NextActivityId">
                                <%="下一节点：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <select id="NextActivityId" name="NextActivityId" class="form-control">
                                    <%=NextActivityOption%>
                                </select>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="ActivityId" value="<%=CurrentAuditActivity.ActivityId%>" />
                                <input type="hidden" name="WorkflowId" value="<%=CurrentAuditActivity.WorkflowId%>" />
                                <input type="hidden" name="CreateDate" value="<%=CurrentAuditActivity.CreateDate.Value(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")%>" />
                                <input type="hidden" name="CreateUser" value="<%=CurrentAuditActivity.CreateUser??CurrentUserName%>" />
                                <input type="hidden" name="UpdateDate" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>" />
                                <input type="hidden" name="UpdateUser" value="<%=CurrentUserName%>" />
                                <input type="hidden" name="_action" value="Save" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang() %></button>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang() %></button>
                                </div>
                                <a class="btn btn-white" href="auditactivitylist.aspx?workflowid=<%=WorkFlowId %>"><%="返回".ToLang() %></a>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $("#ActivityId").val('<%=CurrentAuditActivity.ActivityId%>');
        $("#ActivityName").val('<%=CurrentAuditActivity.ActivityName%>');
        $("#WorkflowId").val('<%=CurrentAuditActivity.WorkflowId%>');
        $("#PreActivityId").val('<%=CurrentAuditActivity.PreActivityId%>');
        $("#NextActivityId").val('<%=CurrentAuditActivity.NextActivityId%>');



        //提交内容
        var options = {
            fields: {
            }
        };
     $('#formEdit').Validator(options,
         function (el) {
             var actionSuccess = el.attr("form-success");

             var $form = $("#formEdit");
             $form.post({
                 success: function (response) {
                     if (response.Status == true) {
                         actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "auditactivitylist.aspx?workflowid=<%=WorkFlowId %>");
                     } else {
                         whir.toastr.error(response.Message);
                     }
                     whir.loading.remove();
                 },
                 error: function (response) {
                     whir.toastr.error(response.Message);
                     whir.loading.remove();
                 }
             });
             return false;
         });
    </script>
</asp:Content>

