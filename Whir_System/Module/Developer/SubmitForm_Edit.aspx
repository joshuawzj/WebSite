<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SubmitForm_Edit.aspx.cs" Inherits="Whir_System_Module_Developer_SubmitForm_Edit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Service" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            var txtReceiveEmailId = "ReceiveEmail";
            $("#" + txtReceiveEmailId).focusout(
            function () {
                var value = $.trim($("#" + txtReceiveEmailId).val());
                var regExp = new RegExp("[^;]$");
                if (value != "" && regExp.test(value)) {
                    $("#" + txtReceiveEmailId).val(value + ";"); //自动为邮件添加分号结尾
                }
                $("#" + txtReceiveEmailId).blur();
            });

            $("#IsSubmitDataMsg").change(function () {
                var value = $("#IsSubmitDataMsg").find("input:checked").val();
                if (value == '1') {
                    $("#tr_emailcontent").hide();
                } else {
                    $("#tr_emailcontent").show();
                }
            }
            );
            if ($("#IsSubmitDataMsg").find("input:checked").val() == '1') {
                $("#tr_emailcontent").hide();
            } else { $("#tr_emailcontent").show(); }
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="SubmitFormList.aspx" aria-expanded="true"><%="提交表单生成器".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%="编辑表单".ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/SubmitForm.aspx">
                    <div class="form-group">
                        <div class="col-md-4 control-label" for="Name">
                            <%="表单名称：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="Name" name="Name" class="form-control" required="true"
                                maxlength="30" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4 text-right" for="IsVerify">
                            <%="需要审核：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <ul class="list">
                                <li>
                                    <input type="radio" id="IsVerify_False" checked="checked" name="IsVerify" value="0" />
                                    <label for="IsVerify_False"><%="否".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="IsVerify_True" name="IsVerify" value="1" />
                                    <label for="IsVerify_True"><%="是".ToLang()%></label>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <% if (IsDevUser)
                       {%>
                        <div class="form-group">
                        <div class="col-md-4 text-right" for="IsAuthCode">
                            <%="显示验证码：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <ul class="list">
                                <li>
                                    <input type="radio" id="IsAuthCode_False" checked="checked" name="IsAuthCode" value="0" />
                                    <label for="IsAuthCode_False"><%="否".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="IsAuthCode_True" name="IsAuthCode" value="1" />
                                    <label for="IsAuthCode_True"><%="是".ToLang()%></label>
                                </li>
                            </ul>
                        </div>
                    </div>
                      <%  } %>
                   
                    <div class="form-group">
                        <div class="col-md-4 text-right" for="SuccessAction">
                            <%="表单成功提交后：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <ul class="list">
                                <li>
                                    <input type="radio" id="SuccessAction_0" name="SuccessAction" value="0" />
                                    <label for="SuccessAction_0"><%="不刷新".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="SuccessAction_1" name="SuccessAction" value="1" />
                                    <label for="SuccessAction_1"><%="刷新".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="SuccessAction_2" name="SuccessAction" value="2" />
                                    <label for="SuccessAction_2"><%="跳转到指定页面".ToLang()%></label>
                                </li>
                            </ul>
                            <input type="text" id="ActionPage" style="display: none;" name="ActionPage" class="form-control" maxlength="2048" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4 control-label" for="ReceiveEmail">
                            <%="接收邮箱：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="ReceiveEmail" name="ReceiveEmail" class="form-control" maxlength="2048"
                                data-toggle="tooltip" data-placement="top" title="<%="表单提交成后台接收提醒的邮箱，多个邮箱用“;”分隔".ToLang()%>" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4 control-label" for="EmailTitle">
                            <%="接收邮件标题：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="EmailTitle" name="EmailTitle" class="form-control" maxlength="50" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4 text-right" for="IsSubmitDataMsg">
                            <%="邮件内容模式：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <ul class="list">
                                <li>
                                    <input type="radio" id="IsSubmitDataMsg_True" name="IsSubmitDataMsg" value="1" />
                                    <label for="IsSubmitDataMsg_True"><%="表单数据".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="IsSubmitDataMsg_False" checked="checked" name="IsSubmitDataMsg" value="0" />
                                    <label for="IsSubmitDataMsg_False"><%="手动填写".ToLang()%></label>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="form-group" id="divEmailContent">
                        <div class="col-md-4 control-label" for="EmailTitle">
                            <%="接收邮件内容：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <textarea id="EmailContent" rows="5" class="form-control" name="EmailContent" maxlength="200" ></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4 control-label" for="SuccessfulTip">
                            <%="提交成功提示：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="SuccessfulTip" name="SuccessfulTip" value="提交成功" class="form-control" maxlength="30"  required="true" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4 control-label" for="UnKnowFailingTip">
                            <%="未知提交失败原因提示：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="UnKnowFailingTip" value="提交失败" name="UnKnowFailingTip" class="form-control"
                                maxlength="50"  required="true" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4 control-label" for="FileMaxSize">
                            <%="上传文件最大容量：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="number" id="FileMaxSize" value="10240" name="FileMaxSize" class="form-control"  required="true" 
                                maxlength="8"  data-toggle="tooltip" data-placement="top" title="<%="单位：KB".ToLang()%>" />
                        </div>
                    </div>
                    <div class="form-group" style="display:none">
                        <div class="col-md-4 control-label" for="ImageTypeErrTip">
                            <%="上传图片格式不正确提示：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="ImageTypeErrTip" value="<%="上传图片格式不正确".ToLang()%>" name="ImageTypeErrTip" class="form-control"
                                maxlength="50" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4 control-label" for="FileTypeErrTip">
                            <%="上传文件格式不正确提示：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="FileTypeErrTip" value="<%="上传文件格式不正确".ToLang()%>" name="FileTypeErrTip" class="form-control"
                                maxlength="50"  required="true" />
                        </div>
                    </div>
                    <div class="form-group" style="display:none">
                        <div class="col-md-4 control-label" for="OverImgMaxErrTip">
                            <%="上传图片超过最大容量提示：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="OverImgMaxErrTip" value="<%="上传图片太大了".ToLang()%>" name="OverImgMaxErrTip" class="form-control"
                                maxlength="50" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4 control-label" for="OverFileMaxErrTip">
                            <%="上传文件超过最大容量提示：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="OverFileMaxErrTip" value="<%="上传文件太大了".ToLang()%>" name="OverFileMaxErrTip" class="form-control"
                                maxlength="50" required="true"  />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-4 control-label" for="AuthCodeErrTip">
                            <%="验证码不正确提示：".ToLang()%>
                        </div>
                        <div class="col-md-8">
                            <input type="text" id="AuthCodeErrTip" value="<%="验证码不正确".ToLang()%>" name="AuthCodeErrTip" class="form-control"
                                maxlength="30" required="true"  />
                        </div>
                    </div>
                        <div class="form-group">
                            <div class="col-md-offset-4 col-md-8 ">
                                <input type="hidden" name="SubmitId" value="<%=CurrentSubmitForm.SubmitId%>" />
                                <input type="hidden" name="_action" value="Save" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <% if (PageMode == EnumPageMode.Insert)
                                        { %>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                    <% } %>
                                </div>
                                <a class="btn btn-white" href="SubmitFormList.aspx"><%="返回".ToLang()%></a>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //点击事件
        $(document).ready(function () {
            $("input[name='SuccessAction']").next().click(function () {
                Change();

            });
            $("input[name='SuccessAction']").parent().next().click(function () {
                Change();

            });

            $("input[name='IsSubmitDataMsg']").next().click(function () {
                ChangeEmail();

            });
            $("input[name='IsSubmitDataMsg']").parent().next().click(function () {
                ChangeEmail();

            });
        });

        function Change() {
            if ($("input[name=SuccessAction]:checked").val() == "2") {
                $("#ActionPage").show();
            } else {
                $("#ActionPage").hide();
            }

        }
        function ChangeEmail() {
            if ($("input[name=IsSubmitDataMsg]:checked").val() == "0") {
                $("#divEmailContent").show();
            } else {
                $("#divEmailContent").hide();
            }

        }

        //绑定值
         <% if (PageMode == EnumPageMode.Update)
                               { %>
        $("[name='SubmitId']").val("<%=CurrentSubmitForm.SubmitId%>");
        $("[name='Name']").val("<%=CurrentSubmitForm.Name%>"); 
        var IsVerify = "<%=CurrentSubmitForm.IsVerify%>";
        if (IsVerify == "True") {
            IsVerify = "1";
        } else {
            IsVerify = "0";
        }
        $("[name='IsVerify'][value='" + IsVerify + "']").prop("checked", "checked");
        var IsAuthCode = "<%=CurrentSubmitForm.IsAuthCode%>";
        if (IsAuthCode == "True") {
            IsAuthCode = "1";
        } else {
            IsAuthCode = "0";
        }
        $("[name='IsAuthCode'][value='" + IsAuthCode + "']").prop("checked", "checked");


        $("[name='SuccessfulTip']").val("<%=CurrentSubmitForm.SuccessfulTip%>");

        $("[name='UnKnowFailingTip']").val("<%=CurrentSubmitForm.UnKnowFailingTip%>");
        $("[name='OverImgMaxErrTip']").val("<%=CurrentSubmitForm.OverImgMaxErrTip%>");
        $("[name='OverFileMaxErrTip']").val("<%=CurrentSubmitForm.OverFileMaxErrTip%>");
        $("[name='ImageTypeErrTip']").val("<%=CurrentSubmitForm.ImageTypeErrTip%>");
        $("[name='FileTypeErrTip']").val("<%=CurrentSubmitForm.FileTypeErrTip%>");
        $("[name='AuthCodeErrTip']").val("<%=CurrentSubmitForm.AuthCodeErrTip%>");
        $("[name='EmailTitle']").val("<%=CurrentSubmitForm.EmailTitle%>");
        $("[name='EmailContent']").val("<%=CurrentSubmitForm.EmailContent%>");

        if ("<%=CurrentSubmitForm.SuccessAction%>" == "false") {
            $("[name='SuccessAction'][value=0]").prop("checked", "checked");
        } else if ("<%=CurrentSubmitForm.SuccessAction%>" == "true") {
            $("[name='SuccessAction'][value=1]").prop("checked", "checked");
        } else {
            $("[name='SuccessAction'][value=2]").prop("checked", "checked");
            $("[name='ActionPage']").val("<%=CurrentSubmitForm.SuccessAction%>");
            $("[name='ActionPage']").show();
        }

        $("[name='ReceiveEmail']").val("<%=CurrentSubmitForm.ReceiveEmail%>");
        var IsSubmitDataMsg = "<%=CurrentSubmitForm.IsSubmitDataMsg%>";
        if (IsSubmitDataMsg == "True") {
            IsSubmitDataMsg = "1";
            $("#divEmailContent").hide();
        } else {
            IsSubmitDataMsg = "0";
            $("#divEmailContent").show();
        }
        $("[name='IsSubmitDataMsg'][value='" + IsSubmitDataMsg + "']").prop("checked", "checked");

        $("[name='FileMaxSize']").val("<%=CurrentSubmitForm.FileMaxSize%>");
         <% } %>

        var options = {
            fields: {
                 FileMaxSize: {
                    validators: {
                         
                        between: {
                            min: 1,
                            max: 2097151,
                            message: '<%="请输入1-2097151之间的整数".ToLang() %>'
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
                             actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "SubmitFormList.aspx");

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
