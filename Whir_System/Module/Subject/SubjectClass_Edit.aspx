<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="subjectclass_edit.aspx.cs" Inherits="whir_system_module_subject_subjectclass_edit" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading">
                <%=GuangliLink[0]%>
            </div>
            <div class="panel-body">
                <div class="form_center">
                    <div id="single" class="tab-pane active">
                        <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Common/Subject.aspx">
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="SubjectClassName">
                                    <%="类型名称：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <input id="SubjectClassName" name="SubjectClassName" class="form-control" maxlength="30"
                                        value="<%=CurrentSubjectClass.SubjectClassName%>" required="true" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="Path"><%="英文目录：".ToLang()%></div>
                                <div class="col-md-10 ">
                                    <input id="Path" name="Path" class="form-control" value="<%=CurrentColumn.Path%>" required="true" maxlength="128" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10 ">
                                    <input type="hidden" name="_action" value="SaveSubjectClass" />
                                    <input type="hidden" name="SubjectClassId" value="<%=CurrentSubjectClass.SubjectClassId%>" />
                                    <input type="hidden" name="SubjectTypeId" value="<%=SubjectTypeId%>" />
                                    <div class="btn-group">
                                        <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                        <% if (SubjectClassId == 0)
                                            {%>
                                        <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                        <% } %>
                                    </div>
                                    <a class="btn btn-white" href="SubjectList.aspx?subjecttypeid=<%= SubjectTypeId %>"><%="返回".ToLang()%></a>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $('#formEdit').Validator(null,
            function (el) {
                var actionSuccess = el.attr("form-success");
                var $form = $("#formEdit");
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "SubjectList.aspx?subjecttypeid=<%= SubjectTypeId %>");

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
