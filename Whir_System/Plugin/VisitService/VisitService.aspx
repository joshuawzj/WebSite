<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="VisitService.aspx.cs" Inherits="Whir_System_Plugin_VisitService_VisitService"
    ValidateRequest="false" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading"><%="在线客服".ToLang()%></div>
            <div class="panel-body">
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/VisitService.aspx">
                    <div class="form-group">
                        <div class="col-md-2 text-right" for="VisitServiceMode"><%="开启模式：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <ul class="list">
                                <li>
                                    <input type="radio" id="VisitServiceMode_0" name="VisitServiceMode" value="0" <%=SiteInfo.VisitServiceMode==0?"checked='checked'":""%> />
                                    <label for="VisitServiceMode_0"><%="不启用".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="VisitServiceMode_1" name="VisitServiceMode" value="1" <%=SiteInfo.VisitServiceMode==1?"checked='checked'":""%> />
                                    <label for="VisitServiceMode_1"><%="第三方客服脚本".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="VisitServiceMode_2" name="VisitServiceMode" value="2" <%=SiteInfo.VisitServiceMode==2?"checked='checked'":""%> />
                                    <label for="VisitServiceMode_2"><%="QQ客服".ToLang()%></label>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="VisitServiceCode"><%="客服内容：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <textarea id="VisitServiceCode" name="VisitServiceCode" 
                                rows="10" maxlength="10000" 
                                class="form-control"><%=SiteInfo.VisitServiceCode%></textarea>
                            <span class="note_gray"><%="允许输入多个，以回车换行隔开".ToLang()%></span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10">
                            <input type="hidden" name="_action" value="Save" />
                            <%if (IsCurrentRoleMenuRes("318"))
                                { %>
                            <button type="button" form-action="submit" form-success="refresh" class="btn btn-info btn-block">
                                <%="保存".ToLang()%></button>
                            <%} %>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
     <script type="text/javascript">
         $('#formEdit').Validator(null,
             function() {
                 var actionSuccess = $(this).attr("form-success");
                 var $form = $("#formEdit");
                 $form.post({
                     success: function(response) {
                         if (response.Status == true) {
                             whir.toastr.success(response.Message);
                         } else {
                             whir.toastr.error(response.Message);
                         }
                         whir.loading.remove();
                     },
                     error: function(response) {
                         whir.toastr.error(response.Message);
                         whir.loading.remove();
                     }
                 });
                 return false;
             });
     </script>
</asp:Content>
