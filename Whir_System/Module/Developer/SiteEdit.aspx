<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SiteEdit.aspx.cs" Inherits="Whir_System_Module_Developer_SiteEdit" %>

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
                    <li><a href="SiteList.aspx" aria-expanded="true"><%="站点结构".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%="编辑站点".ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/Site.aspx">
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="SiteName">
                                <%="站点名称：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="SiteName" name="SiteName" value="<%=CurrentSite.SiteName%>"
                                    class="form-control"
                                    required="true"
                                    maxlength="24" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Path">
                                <%="站点目录：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="Path" name="Path" value="<%=CurrentSite.Path%>"
                                    class="form-control"
                                    required="true"
                                    validationexpression="[a-zA-Z0-9]{1,64}"
                                    maxlength="24" />
                            </div>
                        </div>
                         
                        <div class="form-group attr-edit-text">
                            <div class="col-md-2 text-right" for="MenuName">
                                <%="默认站点：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <ul class="list">
                                    <li>
                                        <input type="radio" id="IsDefault_True" name="IsDefault" value="1"  title="<%="网页默认存放在网站根目录".ToLang()%>"/>
                                        <label for="IsDefault_True" title="<%="网页默认存放在网站根目录".ToLang()%>"><%="是".ToLang()%></label>
                                    </li>
                                    <li>
                                        <input type="radio" id="IsDefault_False" name="IsDefault" value="0" title="<%="网页默认存放在站点目录".ToLang()%>" />
                                        <label for="IsDefault_False" title="<%="网页默认存放在站点目录".ToLang()%>"><%="否".ToLang()%></label>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="SiteId" value="<%=CurrentSite.SiteId%>" />
                                <input type="hidden" name="CreateDate" value="<%=CurrentSite.CreateDate.Value(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")%>" />
                                <input type="hidden" name="CreateUser" value="<%=CurrentSite.CreateUser??CurrentUserName%>" />
                                <input type="hidden" name="UpdateDate" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>" />
                                <input type="hidden" name="UpdateUser" value="<%=CurrentUserName%>" />
                                <input type="hidden" name="_action" value="Save" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang() %></button>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang() %></button>
                                </div>
                                <a class="btn btn-white" href="SiteList.aspx"><%="返回".ToLang() %></a>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    
    <script type="text/javascript">
        $("[name='IsRootPath'][value='<%=CurrentSite.IsRootPath.ToInt()%>']").prop("checked", "checked");
        $("[name='IsDefault'][value='<%=CurrentSite.IsDefault.ToInt()%>']").prop("checked", "checked");

        //提交内容
        $("[form-action='submit']").click(function () {
            var actionSuccess = $(this).attr("form-success");

            var $form = $("#formEdit");
            $form.post({
                success: function (response) {
                    if (response.Status == true) {
                        actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "SiteList.aspx");
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
