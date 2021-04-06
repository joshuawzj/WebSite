<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="WorkFlow_Edit.aspx.cs" Inherits="Whir_System_Module_Extension_Workflow_Edit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading"></div>
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="WorkFlowList.aspx" aria-expanded="true"><%="工作流设置".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%=(WorkFlowId==0? "添加工作流":"编辑工作流").ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Extension/WorkFlow.aspx">
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="WorkFlowName">
                            <%="工作流名称：".ToLang()%>
                        </div>
                        <div class="col-md-9 ">
                            <input type="text" id="WorkFlowName" name="WorkFlowName" value="<%=CurrentWorkFlow.WorkFlowName%>"
                                class="form-control" required="true" maxlength="64" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="BeforeEditType">
                            <%="审核前可否修改：".ToLang()%>
                        </div>
                        <div class="col-md-9 ">
                            <select id="BeforeEditType" name="BeforeEditType" class="form-control">
                                <option value="0"><%="仅发布者可以修改".ToLang()%></option>
                                <option value="1"><%="都不可以修改".ToLang()%></option>
                                <option value="2" selected="True"><%="都可以修改".ToLang()%></option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="BeforeDelType">
                            <%="审核前可否删除：".ToLang()%>
                        </div>
                        <div class="col-md-9">
                            <select id="BeforeDelType" name="BeforeDelType" class="form-control">
                                <option value="0"><%="仅发布者可以删除".ToLang()%></option>
                                <option value="1"><%="都不可以删除".ToLang()%></option>
                                <option value="2" selected="True"><%="都可以删除".ToLang()%></option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="EditType">
                            <%="在审核中可否修改：".ToLang()%>
                        </div>
                        <div class="col-md-9">
                            <select id="EditType" name="EditType" class="form-control">
                                <option value="0" selected="True"><%="仅审核者可以修改".ToLang()%></option>
                                <option value="2"><%="仅发布者可以修改".ToLang()%></option>
                                <option value="3"><%="仅发布者与审核者可以修改".ToLang()%></option>
                                <option value="1"><%="都不可以修改".ToLang()%></option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="DelType">
                            <%="在审核中可否删除：".ToLang()%>
                        </div>
                        <div class="col-md-9">
                            <select id="DelType" name="DelType" class="form-control" data-toggle="tooltip"
                                data-placement="top" title="<%="当处于为第一个审核流程节点时，会综合“审核前可否删除”获取最大的删除权限".ToLang()%>">
                                <option value="0" selected="True"><%="仅审核者可以删除".ToLang()%></option>
                                <option value="2"><%="仅发布者可以删除".ToLang()%></option>
                                <option value="3"><%="仅发布者与审核者可以删除".ToLang()%></option>
                                <option value="1"><%="不可以删除".ToLang()%></option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="PassEditType">
                            <%="审核通过后可否修改：".ToLang()%>
                        </div>
                        <div class="col-md-9">
                            <select id="PassEditType" name="PassEditType" class="form-control">
                                <option value="0" selected="True"><%="仅最后审核者可以修改".ToLang()%></option>
                                <option value="2"><%="仅发布者可以修改".ToLang()%></option>
                                <option value="3"><%="仅发布者与最后审核者可以修改".ToLang()%></option>
                                <option value="1"><%="都不可以".ToLang()%></option>
                                <option value="4"><%="都可以修改".ToLang()%></option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="PassDelType">
                            <%="审核通过后可否删除：".ToLang()%>
                        </div>
                        <div class="col-md-9">
                            <select id="PassDelType" name="PassDelType" class="form-control">
                                <option value="0" selected="True"><%="仅最后审核者可以删除".ToLang()%></option>
                                <option value="2"><%="仅发布者可以删除".ToLang()%></option>
                                <option value="3"><%="仅发布者与最后审核者可以删除".ToLang()%></option>
                                <option value="1"><%="都不可以".ToLang()%></option>
                                <option value="4"><%="都可以删除".ToLang()%></option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="PassEditStateType">
                            <%="审核通过后修改：".ToLang()%>
                        </div>
                        <div class="col-md-9">
                            <select id="PassEditStateType" name="PassEditStateType" class="form-control">
                                <option value="0" selected="True"><%="保持不变".ToLang()%></option>
                                <option value="1"><%="返回最初审核节点".ToLang()%></option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="ReturnEditType">
                            <%="退审后是否可以修改：".ToLang()%>
                        </div>
                        <div class="col-md-9">
                            <select id="ReturnEditType" name="ReturnEditType" class="form-control">
                                <option value="0"><%="仅发布者可以修改".ToLang()%></option>
                                <option value="1"><%="都不可以修改".ToLang()%></option>
                                <option value="2" selected="True"><%="都可以修改".ToLang()%></option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="ReturnDelType">
                            <%="退审后是否可以删除：".ToLang()%>
                        </div>
                        <div class="col-md-9">
                            <select id="ReturnDelType" name="ReturnDelType" class="form-control">
                                <option value="0"><%="仅发布者可以删除".ToLang()%></option>
                                <option value="1"><%="都不可以删除".ToLang()%></option>
                                <option value="2" selected="True"><%="都可以删除".ToLang()%></option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="ReturnToFlowType">
                            <%="退审后转审核：".ToLang()%>
                        </div>
                        <div class="col-md-9">
                            <select id="ReturnToFlowType" name="ReturnToFlowType" class="form-control">
                                <option value="0" selected="True"><%="仅发布者可以转".ToLang()%></option>
                                <option value="1"><%="都不可以转".ToLang()%></option>
                                <option value="2"><%="都可以转".ToLang()%></option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-3 control-label" for="Description">
                            <%="描述：".ToLang()%>
                        </div>
                        <div class="col-md-9">
                            <textarea id="Description" name="Description" class="form-control" required="true" maxlength="100"><%=CurrentWorkFlow.Description%></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-3 col-md-9 ">
                            <input type="hidden" name="WorkFlowId" value="<%=CurrentWorkFlow.WorkFlowId%>" />
                            <input type="hidden" name="CreateDate" value="<%=CurrentWorkFlow.CreateDate.Value(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")%>" />
                            <input type="hidden" name="CreateUser" value="<%=CurrentWorkFlow.CreateUser??CurrentUserName%>" />
                            <input type="hidden" name="UpdateDate" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>" />
                            <input type="hidden" name="UpdateUser" value="<%=CurrentUserName%>" />
                            <input type="hidden" name="_action" value="Save" />
                            <div class="btn-group">
                                <%if (IsCurrentRoleMenuRes("342"))
                                    { %>
                                <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                <%} %>
                            </div>
                            <a class="btn btn-white" href="workflowlist.aspx"><%="返回".ToLang() %></a>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
     <script type="text/javascript">
         $("#WorkFlowName").val('<%=CurrentWorkFlow.WorkFlowName%>');
         $("#Description").val('<%=CurrentWorkFlow.Description%>');
         $("#BeforeEditType").val('<%=CurrentWorkFlow.BeforeEditType%>');
         $("#BeforeDelType").val('<%=CurrentWorkFlow.BeforeDelType%>');
         $("#EditType").val('<%=CurrentWorkFlow.EditType%>');
         $("#DelType").val('<%=CurrentWorkFlow.DelType%>');
         $("#PassEditType").val('<%=CurrentWorkFlow.PassEditType%>');
         $("#PassDelType").val('<%=CurrentWorkFlow.PassDelType%>');
         $("#PassEditStateType").val('<%=CurrentWorkFlow.PassEditStateType%>');
         $("#ReturnEditType").val('<%=CurrentWorkFlow.ReturnEditType%>');
         $("#ReturnDelType").val('<%=CurrentWorkFlow.ReturnDelType%>');
         $("#ReturnToFlowType").val('<%=CurrentWorkFlow.ReturnToFlowType%>');


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
                         actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "workflowlist.aspx");

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
