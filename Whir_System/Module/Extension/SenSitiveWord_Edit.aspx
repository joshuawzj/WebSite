<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="sensitiveword_edit.aspx.cs" Inherits="Whir_System_Module_Extension_SensitiveWord_Edit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="SensitiveWordList.aspx" aria-expanded="true"><%="敏感词设置".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%=(SensitiveWordId==0?"添加敏感词":"编辑敏感词").ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Extension/SensitiveWord.aspx">
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="SensitiveWordName">
                            <%="敏感词：".ToLang() %>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="SensitiveWordName" name="SensitiveWordName" value="<%=CurrentSensitiveWord.SensitiveWordName%>"
                                class="form-control" required="true" minlength="2" maxlength="64" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10 ">
                            <input type="hidden" name="SensitiveWordId" value="<%=CurrentSensitiveWord.SensitiveWordId%>" />
                            <input type="hidden" name="CreateDate" value="<%=CurrentSensitiveWord.CreateDate.Value(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")%>" />
                            <input type="hidden" name="CreateUser" value="<%=CurrentSensitiveWord.CreateUser??CurrentUserName%>" />
                            <input type="hidden" name="UpdateDate" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>" />
                            <input type="hidden" name="UpdateUser" value="<%=CurrentUserName%>" />
                            <input type="hidden" name="_action" value="Save" />
                            <div class="btn-group">
                                <%if (IsCurrentRoleMenuRes("346"))
                                    { %>
                                <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                <%} %>
                            </div>
                            <a class="btn btn-white" href="sensitivewordlist.aspx"><%="返回".ToLang() %></a>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
     <script type="text/javascript">
         $("#SensitiveWordName").val('<%=CurrentSensitiveWord.SensitiveWordName%>');


         //提交内容
         var options = {
             fields: {
                 SensitiveWordName: {
                     validators: {
                         notEmpty: {
                             message: '<%="请输入敏感词".ToLang() %>'
                         }
                     }
                 }
             }
         };
         $('#formEdit').Validator(options,
         function (el) {
             var actionSuccess = el.attr("form-success");

             var $form = $("#formEdit");
             $form.post({
                 success: function (response) {
                     if (response.Status == true) {
                         actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "sensitivewordlist.aspx");

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
