<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="ModuleConfig.aspx.cs" Inherits="Whir_System_ModuleMark_Member_ModuleConfig"
    ValidateRequest="false" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Label.Dynamic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function Insert(str, flag) {
            var obj;
            if (flag) {
                obj = document.getElementById('RetakePassword');
            } else {
                obj = document.getElementById('Authentication');
            }
            if (document.selection) {
                obj.focus();
                var sel = document.selection.createRange();
                document.selection.empty();
                sel.text = str;
            } else {
                var prefix, main, suffix;
                prefix = obj.value.substring(0, obj.selectionStart);
                main = obj.value.substring(obj.selectionStart, obj.selectionEnd);
                suffix = obj.value.substring(obj.selectionEnd);
                obj.value = prefix + str + suffix;
            }
            obj.focus();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading">
                <%="会员设置".ToLang()%></div>
            <div class="panel-body">
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/ModuleMark/Member/ModuleConfig.aspx">
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Register">
                            <%="会员注册协议：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <textarea id="Register" name="Register" maxlength="5000" rows="12" class="form-control"><%=CurrentMemberConfig.Register%></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Authentication">
                            <%="邮件认证内容：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <div>
                                <%="可用变量：".ToLang() %><span>
                                    <%="点击认证地址".ToLang() %>
                                    - </span><a href="javascript:Insert('{Click}',0)">{Click}</a></div>
                            <textarea id="Authentication" name="Authentication" maxlength="5000" rows="12" class="form-control"><%=CurrentMemberConfig.Authentication%></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="RetakePassword">
                            <%="密码找回邮件：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <div>
                                <div id="Div_Mark" style="width: 90%;" runat="Server">
                                </div>
                                <textarea id="RetakePassword" name="RetakePassword" maxlength="5000" rows="12" class="form-control"><%=CurrentMemberConfig.RetakePassword%></textarea>
                            </div>
                        </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="WorkFlow">
                                <%="工作流定义：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <select id="WorkFlow" name="WorkFlow" class="form-control">
                                    <option value="0"><%="==请选择==".ToLang()%></option>
                                    <% foreach (var item in WorkFlows)
                                       {
                                           var check = "";
                                    %>
                                    <% if (item.WorkFlowId == CurrentWorkFlowId)
                                       {
                                           check = "selected=\"selected\"";
                                       } %>
                                    <option <%=check %> value="<%=item.WorkFlowId%>">
                                        <%=item.WorkFlowName%></option>
                                    <%  }   %>
                                </select>
                                <span class="note_gray"><%="修改工作流会导致该栏目信息所有审核中的状态重置".ToLang() %></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 text-right" for="WorkFlow">
                                <%="启用会员组图标：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="checkbox" id="EnableMemGroupImage" name="EnableMemGroupImage" value="1"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="_action" value="Save" />
                                 <%if (IsCurrentRoleMenuRes("297"))
                                     { %>
                                <button form-action="submit" form-success="refresh" class="btn btn-info btn-block"><%="保存".ToLang() %></button>
                                <%} %>
                            </div>
                        </div>
                    </form>
                    </div>
                </div>
            </div>
        </div>
         <script type="text/javascript">
             //绑定
             var EnableMemGroupImage = "<%=CurrentMemberConfig.EnableMemGroupImage%>";
             if (EnableMemGroupImage == "True") {
                 $("[name='EnableMemGroupImage']").prop("checked", "checked");
             }
             var options = {
                 fields: {
                     Register: {
                         validators: {
                             notEmpty: {
                                 message: '<%="会员注册协议为必填项".ToLang() %>'
                             }
                         }
                     }, Authentication: {
                         validators: {
                             notEmpty: {
                                 message: '<%="邮件认证内容为必填项".ToLang() %>'
                             }
                         }
                     }, RetakePassword: {
                         validators: {
                             notEmpty: {
                                 message: '<%="密码找回邮件为必填项".ToLang() %>'
                             }
                         }
                     }
                 }
             };
             $('#formEdit').Validator(options,
                 function () {
                     var actionSuccess = $(this).attr("form-success");
                     var $form = $("#formEdit");
                     $form.post({
                         success: function (response) {
                             if (response.Status == true) {
                                 whir.toastr.success(response.Message);
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
