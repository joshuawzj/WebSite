<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="QuickMenu_Edit.aspx.cs" Inherits="Whir_System_Module_Setting_QuickMenu_Edit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Service" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath %>res/js/jquery.form.js" type="text/javascript"></script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="QuickMenuSetting.aspx" aria-expanded="true"><%="快捷菜单".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%=CurrentMenu.Id==0? "添加快捷菜单".ToLang():"编辑快捷菜单".ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <div id="single" class="tab-pane active">
                        <form id="formEdit" enctype="multipart/form-data" runat="server" class="form-horizontal"
                            form-url="">
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="MenuName"><%="菜单名称：".ToLang()%></div>
                                <div class="col-md-10 ">
                                    <input type="text" id="MenuName" name="MenuName" value="<%=CurrentMenu.MenuName %>"
                                        class="form-control" required="true" maxlength="20" />
                                </div>
                            </div>
                            <div class="form-group ">
                                <div class="col-md-2 control-label" for="Url">
                                    <%="链接地址：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <input type="text" name="Url" value="<%=CurrentMenu.Url %>"  required="true"
                                        class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2  control-label" for="MenuIcon">
                                    <%="菜单图标：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <input id="txt_file" value="" name="txt_file" type="file" class="file-loading" for="MenuIcon" />
                                    <script>
                                        var config = {
                                            uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages_form.aspx?FormId=0&controlID=txt_file&image=",
                                            previewFileType: "any",
                                            initialCaption: '<%="支持格式：".ToLang()%>jpg|png|gif；<%="分辨率".ToLang()%>：60*50；',
                                            language: '<%=GetLoginUserLanguage()%>',
                                            allowedFileExtensions: ['jpg', 'png', 'gif'],
                                            previewClass: "bg-warning",
                                            initialPreviewAsData: true,
                                            initialPreviewFileType: 'image',
                                            pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=ImageUrl&ControlId=txt_file',
                                            isPic: true

                                        };

                                        if ("<%=CurrentMenu.MenuIcon %>") {
                                            config = {
                                                uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages_form.aspx?FormId=0&controlID=txt_file&image=",
                                                previewFileType: "any",
                                                initialCaption: '<%="支持格式：".ToLang()%>jpg|png|gif；<%="分辨率".ToLang()%>：60*50；',
                                                language: '<%=GetLoginUserLanguage()%>',
                                                allowedFileExtensions: ['jpg', 'png', 'gif'],
                                                previewClass: "bg-warning",
                                                initialPreviewFileType: 'image',
                                                initialPreviewAsData: true,
                                                initialPreview: "<%=UploadFilePath+CurrentMenu.MenuIcon%>",
                                                initialPreviewConfig: [{ caption: '', size: 0, name: '<%=CurrentMenu.MenuIcon%>', key: 0 }],
                                                pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=ImageUrl&ControlId=txt_file',
                                                isPic: true
                                            };
                                        };
                                       $(function () { 
                                            $("#txt_file").fileinput(config).on("filebatchselected",
                                                function (event, files) {
                                                    $(this).fileinput("upload");
                                                }).on("fileuploaded",
                                                function (event, data) {
                                                    if (data.response && data.response.Result == true) {
                                                        $("#MenuIcon").val(data.response.Msg).change();
                                                    }
                                                }).on("fileclear",
                                                function (event, data) {
                                                    $("#ImageUrl").val("");
                                                });

                                        });
                                    </script>
                                    <input type="hidden" id="MenuIcon" name="MenuIcon" value="<%=CurrentMenu.MenuIcon %>" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10 ">
                                    <input type="hidden" name="Id" value="<%=CurrentMenu.Id%>" />
                                    <input type="hidden" name="_action" value="Save" />
                                    <div class="btn-group">
                                        <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                        <% if (PageMode == EnumPageMode.Insert)
                                            { %>
                                        <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                        <% } %>
                                    </div>
                                    <a class="btn btn-white" href="QuickMenuSetting.aspx"><%="返回".ToLang()%></a>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

    var options = {
        
    };

     $("#formEdit").Validator(options,
         function(el) {
             var actionSuccess = el.attr("form-success");
             var $form = $("#formEdit");
             $form.attr("form-url", "<%=SysPath%>Handler/Developer/QuickMenu.aspx");

             $form.post({
                 success: function(response) {
                     if (response.Status == true) {
                         actionSuccess == "refresh"
                             ? whir.toastr.success(response.Message, true, false)
                             : whir.toastr.success(response.Message,
                                 true,
                                 false,
                                 "QuickMenuSetting.aspx");
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
             if (e) {
                 e.preventDefault();
             }
             return false;
         });
    </script>
</asp:Content>
