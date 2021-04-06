<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SeoSetting.aspx.cs" Inherits="Whir_System_Module_Setting_SeoSetting" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading">
                <%="站点设置".ToLang()%></div>
            <div class="panel-body">
                <div class="form_center">
                    <div class="tab-content">
                        <div id="SeoConfig" class="tab-pane active">
                            <form id="formSeoConfig" class="form-horizontal" form-url="<%=SysPath%>Handler/Setting/SeoSetting.aspx">
                             
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="MetaTitle">
                                    <%="网站标题：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <input type="text" id="MetaTitle" name="MetaTitle" value="<%=CurrentSiteInfo.MetaTitle %>"
                                        class="form-control" maxlength="200" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="MetaKeyword">
                                    <%="网站关键字：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <textarea id="MetaKeyword" name="MetaKeyword" rows="4" class="form-control" maxlength="200"><%=CurrentSiteInfo.MetaKeyword %></textarea>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="MetaDesc">
                                    <%="网站描述：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <textarea id="MetaDesc" name="MetaDesc" rows="4" class="form-control" maxlength="1000"><%=CurrentSiteInfo.MetaDesc %></textarea>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="CopyRight">
                                    <%="版权：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <textarea id="CopyRight" name="CopyRight" rows="4" class="form-control" maxlength="2048"><%=CurrentSiteInfo.CopyRight %></textarea>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="Icp">
                                    <%="备案号：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <input type="text" id="Icp" name="Icp" value="<%=CurrentSiteInfo.Icp %>" class="form-control"
                                        maxlength="60" />
                                </div>
                            </div>
                                 <div class="form-group">
                                <div class="col-md-2 control-label" for="NullTip">
                                    <%="无数据显示：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <%=NullTipHtml %>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10 ">
                                    <input type="hidden" name="_action" value="SaveSeoSetting" />
                                     <input type="hidden" name="Site" value="<%=CurrentSiteInfo.SiteId %>" />
                                      
                                   <%if (IsCurrentRoleMenuRes("298"))
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
     
        </div>
    </div>
    <script type="text/javascript">
        $("#SiteId").val("<%=SiteId %>");
        

        $("#Site")
            .change(function() {
                $.post("<%=SysPath %>Handler/Setting/SeoSetting.aspx?time=" + new Date().getMilliseconds(),
                { siteId: $("#Site").val(), _action: "GetSeoSeting" },
                function (data) {
                    var result = eval("(" + data + ")");
                    if (result.Status) {
                        var info = eval("(" + d.Message + ")");
                        $("#MetaTitle").val(info.MetaTitle);
                        $("#MetaKeyword").val(info.MetaKeyword);
                        $("#MetaDesc").val(info.MetaDesc);
                        $("#CopyRight").val(info.CopyRight);
                        $("#Icp").val(info.Icp);
                    }
                });
            });
        
        $('#formSeoConfig').Validator(null,
             function () {
                 var actionSuccess = $(this).attr("form-success");
                 var $form = $("#formSeoConfig");
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
