<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="UploadConfig.aspx.cs" Inherits="Whir_System_Module_Config_Upload" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        table td
        {
            border-bottom: 1px solid #EAEFF2;
        }

        table td
        {
            padding: 9px 4px;
        }
    </style>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <div class="panel">
                    <ul class="nav nav-tabs">
                        <li class="active"><a data-toggle="tab" href="#upload" aria-expanded="true"><%="上传配置".ToLang()%></a></li>
                        <li class=""><a data-toggle="tab" href="#uploadftp" aria-expanded="false"><%="FTP配置".ToLang()%></a></li>
                    </ul>
                    <div class="space15"></div>
                    <div class="panel-body">
                        <div class="form_center">
                            <div class="tab-content">
                                <div id="upload" class="tab-pane active">
                                    <form id="formupload" class="form-horizontal" form-url="<%=SysPath%>Handler/Config/UploadConfig.aspx">
                                        <div class="form-group">
                                            <div class="col-md-3 text-right" for="IsRename">
                                                <%="是否重命名文件：".ToLang()%>
                                            </div>
                                            <div class="col-md-9">
                                                <ul class="list">
                                                    <li>
                                                        <input type="radio" id="IsRename_True" name="IsRename" value="1" />
                                                        <label for="IsRename_True"><%="是".ToLang() %></label>
                                                    </li>
                                                    <li>
                                                        <input type="radio" id="IsRename_False" name="IsRename" value="0" />
                                                        <label for="IsRename_False"><%="否".ToLang()%></label>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-3 text-right" for="UpLoadType">
                                                <%="上传方式：".ToLang()%>
                                            </div>
                                            <div class="col-md-9">
                                                <ul class="list">
                                                    <li>
                                                        <input type="radio" id="UpLoadType_True" name="UpLoadType" value="0" />
                                                        <label for="UpLoadType_True"><%="Http".ToLang()%></label>
                                                    </li>
                                                    <li>
                                                        <input type="radio" id="UpLoadType_False" name="UpLoadType" value="1" />
                                                        <label for="UpLoadType_False"><%="Ftp".ToLang()%></label>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-3 text-right" for="SaveFileNameType">
                                                <%="上传文件保存方式：".ToLang()%>
                                            </div>
                                            <div class="col-md-9">
                                                <ul class="list">
                                                    <li>
                                                        <input type="radio" id="SaveFileNameType_True" name="SaveFileNameType" value="0" />
                                                        <label for="SaveFileNameType_True"><%="按日期时间存入".ToLang()%></label>
                                                    </li>
                                                    <li>
                                                        <input type="radio" id="SaveFileNameType_False" name="SaveFileNameType" value="1" />
                                                        <label for="SaveFileNameType_False"><%="按随机数存入".ToLang()%></label>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-3 text-right" for="DirectoryType">
                                                <%="上传文件存放目录：".ToLang()%>
                                            </div>
                                            <div class="col-md-9">
                                                <ul class="list">
                                                    <li>
                                                        <input type="radio" id="DirectoryType_True" name="DirectoryType" value="0" />
                                                        <label for="DirectoryType_True"><%="按天存放".ToLang()%></label>
                                                    </li>
                                                    <li>
                                                        <input type="radio" id="DirectoryType_False" name="DirectoryType" value="1" />
                                                        <label for="DirectoryType_False"><%="按月存放".ToLang()%></label>
                                                    </li>
                                                </ul>
                                            </div>
                                        </div> 
                                        <div class="form-group">
                                            <div class="col-md-3 control-label" for="MaxPicSize">
                                                <%="上传图片最大容量：".ToLang()%>
                                            </div>
                                            <div class="col-md-9">
                                                <input type="number" id="MaxPicSize" name="MaxPicSize" value="<%=UploadConfig.MaxPicSize %>"  min="0"  max="2097151"
                                                    class="form-control" required="true" maxlength="8"
                                                    data-toggle="tooltip" data-placement="top" title="<%="单位：KB".ToLang()%>" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-3 control-label" for="MaxFileSize">
                                                <%="上传文件最大容量：".ToLang()%>
                                            </div>
                                            <div class="col-md-9">
                                                <input type="number" id="MaxFileSize" name="MaxFileSize" value="<%=UploadConfig.MaxFileSize %>"  min="0" max="2097151"
                                                    class="form-control" required="true"  maxlength="8"  
                                                    data-toggle="tooltip" data-placement="top" title="<%="单位：KB".ToLang()%>" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-offset-3 col-md-6 ">
                                                <input type="hidden" name="_action" value="Save" />
                                                <%if (IsCurrentRoleMenuRes("339"))
                                                    { %>
                                                <button form-action="submit" form-success="refresh" class="btn btn-info btn-block">
                                                    <%="保存".ToLang()%></button>
                                                <%} %>
                                            </div>
                                        </div>
                                    </form>
                                </div>
                                <div id="uploadftp" class="tab-pane">
                                    <form id="formuploadftp" class="form-horizontal" form-url="<%=SysPath%>Handler/Config/UploadConfig.aspx">
                                        <div class="form-group">
                                            <div class="col-md-3 control-label" width='<%=LanguageHelper.GetSplitValue("100px|100px|130px")%>'
                                                for="FtpAddress">
                                                <%="FTP地址：".ToLang()%>
                                            </div>
                                            <div class="col-md-9">
                                                <div class="input-group">
                                                    <span class="input-group-addon">ftp://</span>
                                                    <input type="text" id="FtpAddress" name="FtpAddress" value="<%=UploadConfig.FtpAddress %>"
                                                        class="form-control" required="true" maxlength="100" data-toggle="tooltip" data-placement="top"
                                                        title="<%="站点根目录所在的FTP路径,填写示例192.168.1.10:21/web，须加端口".ToLang()%>" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-3 control-label" for="FtpUserName">
                                                <%="FTP用户名：".ToLang()%>
                                            </div>
                                            <div class="col-md-9">
                                                <input type="text" id="FtpUserName" name="FtpUserName" value="<%=UploadConfig.FtpUserName %>"
                                                    class="form-control" required="true" maxlength="50" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-3 control-label" for="FtpPassword">
                                                <%="FTP密码：".ToLang()%>
                                            </div>
                                            <div class="col-md-9">
                                                <input type="password" id="FtpPassword" name="FtpPassword" value="<%=UploadConfig.FtpPassword %>"
                                                    class="form-control" required="true" maxlength="50" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-offset-3 col-md-9 ">
                                                <%if (IsCurrentRoleMenuRes("339"))
                                                    { %>
                                                <button form-action="submittext" form-success="refresh" class="btn btn-info">
                                                    <%="测试".ToLang()%></button>
                                                <%} %>
                                                <%if (IsCurrentRoleMenuRes("364"))
                                                    { %>
                                                <button form-action="submit" form-success="refresh" class="btn btn-info">
                                                    <%="保存".ToLang()%></button>
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
        </div>
        <script type="text/javascript">

            //提交内容
            var options = {
                fields: {
                   
                }
            };
            $('#formuploadftp').Validator(options,
            function (el) {
                var actionSuccess = el.attr("form-success");
                var $form = $("#formuploadftp");
                var data = $form.serialize() + "&_action=SaveFtp";
                $form.post({
                    data: data,
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

            options = {
                submitButtons: '[form-action="submittext"]',
                fields: {
                    
                }
            };
            $('#formuploadftp').Validator(options,
                 function (el) {
                     var actionSuccess = el.attr("form-success");
                     var $form = $("#formuploadftp");
                     var data = $form.serialize() + "&_action=Test";
                     $form.post({
                         data: data,
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
    </div>
    <script type="text/javascript">

        //绑定值
        var IsRename = "<%=UploadConfig.IsRename%>";
        if (IsRename == "True") {
            IsRename = "1";
        } else {
            IsRename = "0";
        }
        $("[name='IsRename'][value='" + IsRename + "']").prop("checked", "checked");
        $("[name='UpLoadType'][value=<%=UploadConfig.UpLoadType%>]").prop("checked", "checked");
        $("[name='SaveFileNameType'][value=<%=UploadConfig.SaveFileNameType%>]").prop("checked", "checked");
        $("[name='DirectoryType'][value=<%=UploadConfig.DirectoryType%>]").prop("checked", "checked");
        
        //提交内容
        var options = {
            fields: {
                MaxPicSize: {
                    validators: {
                        integer: {
                            message: '<%="上传图片最大容量只能是数字".ToLang() %>'
                        }
                    }
                }
                , MaxFileSize: {
                    validators: {
                        integer: {
                            message: '<%="上传文件最大容量只能是数字".ToLang() %>'
                        }
                    }
                }
            }
        };
        $('#formupload').Validator(options,
             function () {
                 var actionSuccess = $(this).attr("form-success");
                 var $form = $("#formupload");
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
</asp:content>
