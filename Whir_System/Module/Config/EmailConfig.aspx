<%@ Page Language="C#" MasterPageFile="~/Whir_System/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="EmailConfig.aspx.cs" Inherits="Whir_System_Module_Config_EmailConfig" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function emailValidator() {
            var testEmail = $("#EmailCs").val();
            var result = $.trim(testEmail);
            if (result == "") {
                return 0; //不能为空
            }
            var reg = new RegExp("^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$");
            if (reg.test(testEmail)) {
                return 1;
            } else { return -1; }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="row">
            <div class="col-sm-6">
                <div class="panel">
                    <div class="panel-heading">
                        <%="邮箱参数".ToLang()%>
                    </div>
                    <div class="panel-body">
                        <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Config/EmailConfig.aspx">
                            <div class="form-group">
                                <div class="col-md-3 control-label" for="Smtp">
                                    <%="邮件服务器：".ToLang() %>
                                </div>
                                <div class="col-md-9">
                                    <input type="text" id="Smtp" name="Smtp" value="<%=EmailConfig.SMTP%>" class="form-control"
                                        required="true" maxlength="64" data-toggle="tooltip" data-placement="top" title="<%="用来发送邮件的SMTP服务器，如果你不清楚此参数含义，请联系你的空间商".ToLang() %>" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label" for="Port">
                                    <%="端口：".ToLang() %>
                                </div>
                                <div class="col-md-9">
                                    <input type="number" id="Port" name="Port" value="<%=EmailConfig.Port%>" class="form-control"
                                        required="true" maxlength="8" data-toggle="tooltip" data-placement="top" title="<%="端口号必须是非负整正数，默认是25端口".ToLang() %>" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label" for="Email">
                                    <%="发件人邮箱：".ToLang() %>
                                </div>
                                <div class="col-md-9">
                                    <input type="email" id="Email" name="Email" value="<%=EmailConfig.Email%>" class="form-control"
                                        required="true" maxlength="64" data-toggle="tooltip" data-placement="top" title="<%="发件人邮箱".ToLang() %>" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label" for="UserName">
                                    <%="用户名：".ToLang() %>
                                </div>
                                <div class="col-md-9">
                                    <input type="text" id="UserName" name="UserName" value="<%=EmailConfig.UserName%>"
                                        class="form-control" required="true" maxlength="64" data-toggle="tooltip" data-placement="top"
                                        title="<%="发件人邮箱的用户名，一般与邮箱地址一致".ToLang() %>" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-3 control-label" for="Password">
                                    <%="密码：".ToLang() %>
                                </div>
                                <div class="col-md-9">
                                    <input type="password" id="Password" name="Password" value="<%=EmailConfig.Password%>"
                                        class="form-control" required="true" maxlength="16" data-toggle="tooltip" data-placement="top"
                                        title=" <%="发件人邮箱的密码".ToLang() %> " />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-3 col-md-9">
                                    <input type="hidden" name="_action" value="Save" />
                                    <%if (IsCurrentRoleMenuRes("340"))
                                        { %>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info btn-block"><%="保存".ToLang()%></button>
                                    <%} %>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="panel">
                    <div class="panel-heading">
                        <%="邮箱测试".ToLang() %>
                    </div>
                    <div class="panel-body">
                        <form id="FormTest" class="form-horizontal" form-url="<%=SysPath%>Handler/Config/EmailConfig.aspx">
                            <div class="form-group">
                                <div class="col-md-3 control-label" for="EmailCs">
                                    <%="收件邮箱：".ToLang() %>
                                </div>
                                <div class="col-md-9 ">
                                    <input type="email" id="EmailCs" name="EmailCs" value="" class="form-control" required="true"
                                        maxlength="64" data-toggle="tooltip" data-placement="top" title="<%="用来接受邮件的邮箱地址".ToLang() %>" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-3 col-md-9 ">
                                    <input type="hidden" name="_action" value="Test" />
                                    <%if (IsCurrentRoleMenuRes("341"))
                                        { %>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info btn-block">
                                        <%="测试发送".ToLang()%></button>
                                    <%} %>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $('[data-toggle="tooltip"]').tooltip();
            })
        </script>

        <script type="text/javascript">
            var options = {
                fields: {
                    Smtp: {
                        validators: {
                            regexp: {
                                regexp: /^[a-zA-Z0-9_\-\.]+$/,
                                message: '<%="只能字符、数字、下划线、中划线".ToLang() %>'
                        }

                    }
                }, Email: {
                    validators: {
                        emailAddress: {
                            message: '<%="邮箱格式不正确".ToLang() %>'
                        }
                    }
                }
                , UserName: {
                    
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

            options = {
                fields: {
                    EmailCs: {
                        validators: {
                            emailAddress: {
                                message: '<%="邮箱格式不正确".ToLang() %>'
                            }
                        }
                    }
                }
            };

            //提交内容
            $('#FormTest')
                .Validator(options,
                function () {
                    var actionSuccess = $(this).attr("form-success");

                    var $form = $("#FormTest");
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


            function CheckEmail() {
                var result = emailvalidator();
                var msg = '';
                if (result == 1) {
                    return true;
                } else {
                    msg = result == 0 ? '<%="请输入收件邮箱".ToLang() %>' : '<%="邮箱格式不正确".ToLang() %>';
                whir.toastr.error(msg);
                return false;
            }
            }
        </script>
    </div>
</asp:Content>
