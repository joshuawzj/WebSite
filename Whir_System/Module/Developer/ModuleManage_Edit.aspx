<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="ModuleManage_edit.aspx.cs" Inherits="whir_system_module_developer_ModuleManage_edit" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="false">
    </asp:ScriptManager>
    <form runat="server">
        <div class="content-wrap">
            <div class="space15"></div>
            <div class="panel">
                <div class="panel-body">
                    <ul class="nav nav-tabs">
                        <li><a href="ModuleManageList.aspx" aria-expanded="true">模块管理</a></li>
                        <li class="active"><a href="ModuleManage_edit.aspx" data-toggle="tab" aria-expanded="true">添加模块</a></li>
                    </ul>
                    <div class="space15"></div>
                    <div class="form_center">
                        <div class="col-md-12">
                            <div class="alert alert-info">
                                <%="模块包以.zip为扩展名，上传成功后会自动解压".ToLang()%><br />
                                <%="模块包里的文件存放要以规定的格式存放与命名，详细说明".ToLang()%>
                            </div>
                            <input id="txt_file" value="" name="txt_file" type="file" class="file-loading" />
                            <script type="text/javascript">
                                var config = {
                                    uploadUrl: "<%=SysPath%>ajax/developer/ModuleManageUpload.aspx?FormId=0&savePath=<%=UploadFilePath %>ModuleTemp/&fileName=",
                                    previewFileType: "any",
                                    language: '<%=GetLoginUserLanguage()%>',
                                    allowedFileExtensions: ['zip', 'ZIP'],
                                    initialCaption: '支持格式：zip',
                                    noPicker: true,
                                    showPreview: false,
                                    isPic: false
                                };
                                $(function () { 
                                    $("#txt_file").fileinput(config).on("filebatchselected",
                                        function(event, files) {
                                            $(this).fileinput("upload");
                                        }).on("fileuploaded",
                                        function(event, data) {
                                            if (data.response && data.response.Result == true) {
                                                whir.toastr.success(data.response.Msg);
                                            }
                                        }).on("fileclear",
                                        function(event, data) {

                                        });
                                });
                            </script>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
