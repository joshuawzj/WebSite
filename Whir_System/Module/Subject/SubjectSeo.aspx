<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SubjectSeo.aspx.cs" Inherits="Whir_System_Module_Subject_SubjectSeo" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading">
                <%="子站SEO设置".ToLang()%>
            </div>
            <div class="panel-body">
                <div class="form_center">
                    <div class="tab-content">
                        <div id="SeoConfig" class="tab-pane active">
                            <form id="formSeoConfig" class="form-horizontal" form-url="<%=SysPath%>Handler/Setting/SeoSetting.aspx">

                                <div class="form-group">
                                    <div class="col-md-2 control-label" for="MetaTitle">
                                        <%="SEO标题：".ToLang()%>
                                    </div>
                                    <div class="col-md-10 ">
                                        <input type="text" id="MetaTitle" name="MetaTitle" value="<%=CurrentSubject.MetaTitle %>"
                                            class="form-control" maxlength="200" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2 control-label" for="MetaKeyword">
                                        <%="SEO关键字：".ToLang()%>
                                    </div>
                                    <div class="col-md-10 ">
                                        <textarea id="MetaKeyword" name="MetaKeyword" rows="4" class="form-control" maxlength="200"><%=CurrentSubject.MetaKeyword %></textarea>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-2 control-label" for="MetaDesc">
                                        <%="SEO描述：".ToLang()%>
                                    </div>
                                    <div class="col-md-10 ">
                                        <textarea id="MetaDesc" name="MetaDesc" rows="4" class="form-control" maxlength="1000"><%=CurrentSubject.MetaDesc %></textarea>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-offset-2 col-md-10 ">
                                        <input type="hidden" name="_action" value="SaveSubjectSeo" />
                                        <input type="hidden" name="SubjectTypeId" value="<%=SubjectTypeId%>" />
                                        <input type="hidden" name="SubjectId" value="<%=CurrentSubject.SubjectId%>" />
                                        <input type="hidden" name="SubjectClassId" value="<%=CurrentSubject.SubjectClassId==0?SubjectClassId:CurrentSubject.SubjectClassId%>" />
                                        <button form-action="submit" form-success="refresh" class="btn btn-info"><%="保存".ToLang() %></button>
                                       <a class="btn btn-white" href="SubjectList.aspx?subjecttypeid=<%=SubjectTypeId%>"><%="返回".ToLang()%></a>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <script type="text/javascript">
        $("#SiteId").val("<%=SiteId %>");

        $('#formSeoConfig').Validator(null,
             function () {
                 var actionSuccess = $(this).attr("form-success");
                 var $form = $("#formSeoConfig");
                 $form.post({
                     success: function (response) {
                         if (response.Status == true) {
                             whir.toastr.success(response.Message, true, false, "SubjectList.aspx");
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
