﻿<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Vote.aspx.cs" Inherits="Whir_System_Module_Label_Vote" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading">
                <%="置标生成器".ToLang()%>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-3">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                置标
                            </div>
                            <div id="paneltres" class="panel panels  ">
                                <div class="form-group">
                                    <whir:ColumnLeftTree runat="server" ID="ColumnLeftTree2"></whir:ColumnLeftTree>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-9">
                        <div id="panelcontent" class="panel panels  ">
                            <form id="formEdit" class="form-horizontal">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        条件
                                    </div>
                                    <div class="panel-body">
                                        <div class="form_center">
                                            <whir:SiteColumn ID="SiteColumn1" runat="server" />
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="SuccessfulTips">
                                                    <%=" 表单提交成功提示：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="SuccessfulTips" name="SuccessfulTips"
                                                        value="" regular="Intege" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="FailedTips">
                                                    <%=" 表单提交错误提示：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="FailedTips" name="FailedTips" value=""
                                                        regular="Intege" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="SubmitText">
                                                    <%=" 提交按钮文字：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="SubmitText" name="SubmitText" value=""
                                                        regular="Intege" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="IpRepeatTips">
                                                    <%=" IP重复提交提示：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="IpRepeatTips" name="IpRepeatTips" value=""
                                                        regular="Intege" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="UnSelectAllTips">
                                                    <%=" 未答完题就提交的提示：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="UnSelectAllTips" name="UnSelectAllTips"
                                                        value="" regular="Intege" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="SuccessUrl">
                                                    <%=" 提交表单跳转的页面：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="SuccessUrl" name="SuccessUrl" value=""
                                                        regular="Intege" />
                                                </div>
                                            </div>
                                            <div class="form-group" align="center">
                                                <input type="hidden" name="labelName" id="labelName" value="vote" />
                                                <input type="hidden" name="_action" id="_action" value="GetWtl" />
                                                <a id="lbtnGenerate" href="javascript:;" class="btn btn-primary"><b>开始生成</b></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </form>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    字段
                                </div>
                                <div class="panel-body">
                                    <div class="form_center">
                                        ColumnId：网上投票栏目Id&nbsp;&nbsp;&nbsp;SuccessfulTips：表单提交成功提示&nbsp;&nbsp;&nbsp;FailedTips：表单提交错误提示&nbsp;&nbsp;&nbsp;SubmitText：提交按钮文字<br />
                                        IpRepeatTips：提交按钮文字&nbsp;&nbsp;&nbsp;UnSelectAllTips：未答完题就提交的提示&nbsp;&nbsp;&nbsp;SuccessUrl：提交表单跳转的页面
                                    </div>
                                </div>
                            </div>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <%="生成置标：".ToLang() %>
                                </div>
                                <div class="panel-body">
                                    <div class="form_center">
                                        <textarea id="Content" name="Content" class="form-control" rows="8" width="100%"
                                            onchange="clip.setText(this.value)"></textarea>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $("#paneltres").height($("#panelcontent").height());
        var _fieldsoptions = {
            fields: {
                Site: {
                    validators: {
                        notEmpty: {
                            message: '<%="站点为必填项".ToLang() %>'
                        }
                    }
                }
            }
        };

        if (_fieldsoptions) {
            _fieldsoptions.submitButtons = "#lbtnGenerate";
        }

        $('#formEdit').Validator(_fieldsoptions,
            function () {
                $("#formEdit")
                    .post({
                        url: "<%=SysPath%>Handler/Module/Label/GetWtlString.aspx",
                        success:
                            function (response) {
                                if (response.Status === true) {
                                    $("#Content").val(response.Message);
                                } else {
                                    whir.toastr.error(response.Message);
                                }
                                $("#lbtnGenerate").removeAttr('disabled');
                                whir.loading.remove();
                                return false;
                            }
                    });
                return false;
            });

    </script>
</asp:Content>
