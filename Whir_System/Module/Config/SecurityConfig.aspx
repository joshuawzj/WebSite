<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SecurityConfig.aspx.cs" Inherits="whir_system_module_config_SecurityConfig" %>

<%@ Register Assembly="Whir.Framework" Namespace="Whir.Framework" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%@ Import Namespace="Whir.Language" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="安全配置".ToLang()%></div>
            <div class="panel-body">
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Config/SecurityConfig.aspx">
                        <div class="form-group">
                            <div class="col-md-2 text-right" for="OpenLog"><%="记录操作日志：".ToLang()%></div>
                            <div class="col-md-10 ">
                                <ul class="list" id="text1">
                                    <li>
                                        <input type="radio" id="OpenLog_True" name="OpenLog" value="1" />
                                        <label for="OpenLog_True"><%="是".ToLang()%></label>
                                    </li>
                                    <li>
                                        <input type="radio" id="OpenLog_False" name="OpenLog" value="0" />
                                        <label for="OpenLog_False"><%="否".ToLang()%></label>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 text-right" for="SingleLogin"><%="开启单用户登录：".ToLang()%></div>
                            <div class="col-md-10 ">
                                <ul class="list">
                                    <li>
                                        <input type="radio" id="SingleLogin_True" name="SingleLogin" value="1" />
                                        <label for="SingleLogin_True"><%="是".ToLang()%></label>
                                    </li>
                                    <li>
                                        <input type="radio" id="SingleLogin_False" name="SingleLogin" value="0" />
                                        <label for="SingleLogin_False"><%="否".ToLang()%></label>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="DNS">
                                <%="后台访问域名：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <div class="input-group">
                                    <span class="input-group-addon">http://</span>
                                    <input type="text" id="DNS" name="DNS" value="<%=SystemConfig.DNS%>" class="form-control" maxlength="64" />
                                </div>
                                <span class="note_gray" style="display: block; white-space: normal; word-break: break-all"><%="例如：www.xxx.com。若访问不了后台域名可修改：".ToLang() %><%=SysPath %><%="Config/SystemConfig.config文件，把DNS节点里的域名删除即可。".ToLang() %></span>
                                <span class="note_gray text-danger"><%="注：确定后台访问域名能访问得到，否则会导致访问不了后台的结果，请谨慎。".ToLang() %></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="TimeOut">
                                <%="后台登陆超时：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <div class="input-group">
                                    <input type="text" id="TimeOut" name="TimeOut" required="true" value="<%=SystemConfig.TimeOut%>"
                                        class="form-control" maxlength="50" />
                                    <span class="input-group-addon"><%="分钟".ToLang()%></span>
                                </div>
                                <span class="note_gray"><%="后台在不操作情况下超过多少分钟需要重新登录的时间".ToLang() %></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="_action" value="Save" />
                                <%if (IsCurrentRoleMenuRes("335"))
                                    { %>
                                <button form-action="submit" form-success="refresh" class="btn btn-info btn-block"><%="保存".ToLang()%></button>
                                <%} %>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //绑定值
        $("[name='OpenLog'][value=<%=SystemConfig.OpenLog%>]").prop("checked", "checked");
        $("[name='SingleLogin'][value=<%=SystemConfig.SingleLogin%>]").prop("checked", "checked");

        var options = {
            fields: {
                DNS: {
                    validators: {
                        regexp: {
                            regexp: /^\s*([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)?\s*$/,
                            message: '<%="格式错误".ToLang() %>'
                        }
                    }
                },
                TimeOut: {
                    validators: {
                        regexp: {
                            regexp: /^([1-9]\d|[1-3]\d{1,4}|4[0-2]\d{1,3}|43[0-1]\d{0,2}|[5-9]\d{2,3}|432|4320|43200)$/,
                            message: '<%="值有效范围10~43200".ToLang() %>'
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
