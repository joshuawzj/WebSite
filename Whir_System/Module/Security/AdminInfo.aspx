<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="admininfo.aspx.cs" Inherits="whir_system_module_security_admininfo" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading"><%="个人资料".ToLang()%></div>
            <div class="panel-body">
                <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Module/Security/Admin.aspx">
                    <div class="form-group">
                        <div class="col-md-2 text-right">
                            <%="用户名：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <asp:Literal ID="ltLoginName" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 text-right">
                            <%="最后登录时间：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <asp:Literal ID="ltLastLoginTime" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 text-right">
                            <%="最后登录IP：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <asp:Literal ID="ltLastLoginIP" runat="server"></asp:Literal>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label">
                            <%="真实姓名：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="RealName" name="RealName" value="<%=CurrenUsers.RealName %>"
                                class="form-control" required="true" maxlength="50" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label">
                            <%="E-mail：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="Email" name="Email" value="<%=CurrenUsers.Email %>" class="form-control"
                                required="true" maxlength="50" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Logo">
                            <%="头像：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input id="txt_file" value="" name="txt_file" type="file" class="file-loading" for="Logo" />
                            <input type="hidden" id="Logo" value="<%=CurrenUsers.Logo %>" name="Logo" />
                            <span class="note_gray"><%="修改头像在下次登录生效。".ToLang()%></span>
                            <script type="text/javascript">
                                var config = {
                                    uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%=UploadFilePath+"logo/"%>&fileName=",
                                    previewFileType: "any",
                                    language: '<%=GetLoginUserLanguage()%>',
                                allowedFileExtensions: [<%=AllowPicType %>],
                                maxFileCount: 1,
                                previewClass: "bg-warning",
                                initialCaption: '<%="建议尺寸:".ToLang()%>128×128',
                                initialPreviewAsData: true,
                                initialPreviewFileType: 'image',
                                pickerUrl:'<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=SystemLoginLogo&ControlId=txt_file',
                                isPic: true
                                };
                                if ("<%=CurrenUsers.Logo %>") {
                                    config = {
                                        uploadUrl: "<%=SysPath%>Ajax/Extension/uploadimages.aspx?FormId=0&savePath=<%=UploadFilePath+"logo/"%>&fileName=",
                                        previewFileType: "any",
                                        language: '<%=GetLoginUserLanguage()%>',
                                    allowedFileExtensions: ["jpg", "png"],
                                    maxFileCount: 1,
                                    previewClass: "bg-warning",
                                    initialPreviewFileType: 'image',
                                    initialPreviewAsData: true,
                                    initialPreview: "<%=UploadFilePath+CurrenUsers.Logo %>",
                                    initialPreviewConfig: [{ caption: '', size: 0, name: '<%=CurrenUsers.Logo%>', key: 0 }],
                                    pickerUrl:'<%=SysPath %>ModuleMark/Common/Picker.aspx?isPic=1&isMultiple=radio&HidChooseId=Logo&ControlId=txt_file',
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
                                                $("#Logo").val(data.response.Msg);
                                            }
                                        }).on("filecleared", function (event, data) {
                                            $("#Logo").val("");
                                        });

                                });
                            </script>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label">
                            <%="语言版本：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <select id="SystemLanguage" name="SystemLanguage" class="form-control">
                                <option value="1"><%=LanguageType.简体中文 %></option>
                                <option value="2"><%=LanguageType.繁体中文 %></option>
                                <option value="3"><%=LanguageType.英文 %></option>
                            </select>
                            <span class="note_gray"><%="修改语言版本在下次登录生效。".ToLang()%></span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-8">
                            <input type="hidden" name="UserId" value="<%=CurrenUsers.UserId %>" />
                            <input type="hidden" name="_action" value="UpdateUserInfo" />
                            <button form-action="submit" class="btn btn-info btn-block"><%="保存".ToLang()%></button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
    <script type="text/javascript">
                                //绑定值
                                $("#SystemLanguage").val("<%=CurrenUsers.SystemLanguage %>");

                                var options = {
                                    fields: {
                                        Email: {
                                            validators: {
                                                emailAddress: {
                                                    message: '<%="请输入正确的邮件地址".ToLang() %>'
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
