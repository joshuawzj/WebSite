<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="language_edit.aspx.cs" Inherits="whir_system_module_developer_language_edit" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">

            <div class="panel-body">

                <ul class="nav nav-tabs">
                    <li><a aria-expanded="true" href="Language.aspx"><%= "多语言配置".ToLang()%></a></li>
                    <li class="active"><a aria-expanded="true" data-toggle="tab" href="#single"><%= (CN ==""?"添加多语言":"编辑多语言").ToLang()%></a></li>
                </ul>
                <br />

                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/Language.aspx">

                        <div class="form-group">
                            <div class="col-md-2 control-label" for="RoleName">
                                <%="简体中文".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <textarea id="CN" class="form-control" name="CN" required="true" type="text" rows="5"><%=Language.CN %></textarea>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="RoleName">
                                <%="繁体中文".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <textarea id="HkText" class="form-control" name="HkText" required="true" type="text" rows="5"><%=Language.HkText %></textarea>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="RoleName">
                                <%="英文".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <textarea id="EnText" class="form-control" name="EnText" required="true" type="text" rows="5"><%=Language.EnText %></textarea>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">

                                <input name="_action" type="hidden" value="Save" />
                                <div class="btn-group">
                                    <button class="btn btn-info" form-action="submit" form-success="back"><%="提交并返回".ToLang()%></button>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                </div>
                                <a class="btn btn-white" href="language.aspx"><%="返回".ToLang()%></a>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var options = {
            fields: {
                CN: {
                    validators: {
                        notEmpty: {
                            message: '<%="该字段为必填项".ToLang() %>'
                        }
                    }
                },
                HkText: {
                    validators: {
                        notEmpty: {
                            message: '<%="该字段为必填项".ToLang() %>'
                        }
                    }
                },
                EnText: {
                    validators: {
                        notEmpty: {
                            message: '<%="该字段为必填项".ToLang() %>'
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
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "language.aspx");
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    },
                    error: function (response) {
                        whir.loading.remove();
                    }
                });
                return false;
            });

        //数据库字段名自动翻译
        $("#CN").blur(function () {
            if ($(this).val() != "") {
                $.ajax({
                    type: "GET",
                    url: "<%=SysPath %>ajax/common/translater.aspx",
                    data: "source=" + encodeURI($(this).val()) + "&from=zh-cn&to=en",
                    success: function (msg) {
                        $("#EnText").val(msg).change();
                    },
                    error: function (response) {
                        whir.toastr.warning("<%="翻译失败".ToLang() %>");
                    }
                });
                $.ajax({
                    type: "GET",
                    url: "<%=SysPath %>ajax/common/translater.aspx",
                    data: "source=" + encodeURI($(this).val()) + "&from=zh-cn&to=zh-tw",
                    success: function (msg) {
                        $("#HkText").val(msg).change();
                    },
                    error: function (response) {
                        whir.toastr.warning("<%="翻译失败".ToLang() %>");
                    }
                });
            }
        });
    </script>
</asp:Content>
