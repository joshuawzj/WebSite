<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="MemberGroup_Edit.aspx.cs" Inherits="Whir_System_ModuleMark_Member_MemberGroup_Edit" %>

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
                        <li><a href="MemberGroupList.aspx" aria-expanded="true"><%="会员分组".ToLang() %></a></li>
                        <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%=GroupId==-1? "添加会员组".ToLang():"编辑会员组".ToLang()%></a></li>
                    </ul>
                <div class="space15"></div>
                <div class="form_center">
                            <div id="single" class="tab-pane active">
                                <form id="formEdit" enctype="multipart/form-data" runat="server" class="form-horizontal"
                                form-url="">
                                <div class="form-group">
                                    <div class="col-md-2 control-label" for="GroupName"><%="会员组名称：".ToLang()%></div>
                                    <div class="col-md-10 ">
                                        <input type="text" id="GroupName" name="GroupName" value="<%=CurrentMemberGroup.GroupName %>"
                                            class="form-control" required="true" maxlength="20" />
                                    </div>
                                </div>
                                <div class="form-group" id="trImageUrl" runat="server">
                                    <div class="col-md-2  control-label" for="ImageUrl">
                                        <%="会员组图标：".ToLang()%></div>
                                    <div class="col-md-10 ">
                                        <input id="txt_file" value="" name="txt_file" type="file" class="file-loading"  for="ImageUrl"/>
                                        <script>
                                        var config = {
                                            uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages_form.aspx?FormId=0&controlID=txt_file&image=",
                                            previewFileType: "any",
                                            initialCaption: '支持格式：jpg|png|gif',
                                            language: '<%=GetLoginUserLanguage()%>',
                                            allowedFileExtensions: ['jpg','png','gif'],
                                            previewClass: "bg-warning",
                                            initialPreviewAsData: true,
                                            initialPreviewFileType: 'image',
                                            pickerUrl: '<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=ImageUrl&ControlId=txt_file',
                                            isPic: true
                                           
                                        };
                             
                                      if ("<%=ImageUrl %>") {
                                          config = {
                                              uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages_form.aspx?FormId=0&controlID=txt_file&image=",
                                              previewFileType: "any",
                                              initialCaption: '支持格式：jpg|png|gif',
                                              language: '<%=GetLoginUserLanguage()%>',
                                              allowedFileExtensions: ['jpg', 'png', 'gif'],
                                              previewClass: "bg-warning",
                                              initialPreviewFileType: 'image',
                                              initialPreviewAsData: true,
                                              initialPreview: "<%=UploadFilePath+ImageUrl%>",
                                              initialPreviewConfig: [{ caption: '', size: 0, name: '<%=ImageUrl%>', key: 0 }],
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
                                                            $("#ImageUrl").val(data.response.Msg).change();
                                                        }
                                                    }).on("fileclear",
                                                    function (event, data) {
                                                        $("#ImageUrl").val("");
                                                    });

                                            });
                                        </script>
                                        <input type="hidden" id="ImageUrl" name="ImageUrl"  value="<%=CurrentMemberGroup.ImageUrl %>" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-offset-2 col-md-10 ">
                                        <input type="hidden" name="GroupId" value="<%=CurrentMemberGroup.GroupId%>" />
                                        <input type="hidden" name="_action" value="Save" />
                                        <div class="btn-group">
                                            <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                            <% if (PageMode == EnumPageMode.Insert)
                                                { %>
                                            <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                            <% } %>
                                        </div>
                                        <a class="btn btn-white" href="membergrouplist.aspx"><%="返回".ToLang()%></a>
                                    </div>
                                </div>
                                </form>
                            </div>
                        </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function UploadSuccessFn(value) {
            if (value) {
                $("#ImageUrl").val(value);
            }
        }

        var options = {
            fields: {
              
            }
        };
        $("#formEdit").Validator(options,
             function (el) {
                 var actionSuccess = el.attr("form-success");
                 var $form = $("#formEdit");
                 $form.attr("form-url", "<%=SysPath%>Handler/ModuleMark/Member/MemberGroup.aspx");
                 if ($("#ImageUrl").val() == "") {
                     whir.toastr.error("请上传会员组图标");
                     return false;
                 }
                 $form.post({
                     success: function (response) {
                         if (response.Status == true) {
                             actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "MemberGroupList.aspx");
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
                 if (e) {
                     e.preventDefault();
                 }
                 return false;
             });
    </script>
</asp:Content>
