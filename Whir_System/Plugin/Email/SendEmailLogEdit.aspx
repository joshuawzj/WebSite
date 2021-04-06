<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SendEmailLogEdit.aspx.cs" Inherits="Whir_System_Plugin_Email_SendEmailLogEdit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Controls.UI.Controls" %>
<%@ Import Namespace="Whir.Domain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <script type="text/javascript" language="javascript">
         //插入标签
        function Insert(insertTxt, q) {
            if ($("#ifmEWebEditor-1").length > 0) {//ewebeditor
                var wd = document.getElementById("ifmEWebEditor-1").contentWindow;
                wd.focus();
                wd.appendHTML(insertTxt);
            } else if (typeof (editorContent) != "undefined") {//kindeditor
                editorContent.focus();
                editorContent.insertHtml(insertTxt);
            } else if (typeof (ueContent) != "undefined") {//ueditor,wueditor
                ueContent.focus();
                ueContent.execCommand('inserthtml', insertTxt)
            }
        }

        function checkFail() {
            var failEmail = $("#FailEmail").val();
            if (failEmail == '') {
                TipError('<%="发送失败，邮箱为空不能发送".ToLang() %>');
                return false;
            }
            else if ($("#Title").val() == '') {
                TipMessage('<%="邮件标题不能为空".ToLang() %>');
                return false;
            }
            else {
                __doPostBack('btnSendFail', '');
            }
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="EmailMass.aspx" aria-expanded="true"><%="邮件群发".ToLang()%></a></li>
                    <li><a href="SendEmailLog.aspx" aria-expanded="true"><%="发送记录".ToLang()%></a></li>
                    <li class="active"><a data-toggle="tab" href="<%=Request.Url%>" aria-expanded="true"><%="编辑发送记录".ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/SendEmailRecord.aspx">
                        <div class="form-group">
                            <div class="col-md-2 text-right" for="SendType">
                                <%="发送方式：".ToLang() %>
                            </div>
                            <div class="col-md-10 ">
                                <asp:Literal ID="ltCatgory" runat="server"></asp:Literal><asp:Literal ID="ltGroupName"
                                    runat="server"></asp:Literal>
                            </div>
                        </div>
                        <div class="form-group" id="div_byMember">
                            <div class="col-md-2 control-label" for="SuccessEmail">
                                <%="已发送邮箱：".ToLang() %>
                            </div>
                            <div class="col-md-10 ">
                                <textarea id="SuccessEmail" name="SuccessEmail" value="" class="form-control" maxlength="2000"><%=CurrentRecord.SuccessEmail%></textarea>
                            </div>
                        </div>
                        <div class="form-group" id="div1">
                            <div class="col-md-2 control-label" for="FailEmail">
                                <%="未发送邮箱：".ToLang() %>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="FailEmail" name="FailEmail" value="<%=CurrentRecord.FailEmail%>" class="form-control"
                                    readonly='readonly' rows="5" validationexpression="(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*;)*"
                                    maxlength="600" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Title">
                                <%="邮件标题：".ToLang() %>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="Title" name="Title" value="<%=CurrentRecord.Title%>" class="form-control" required="true"
                                    maxlength="30" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Title">
                                <%="常用标签：".ToLang() %>
                            </div>
                            <div class="col-md-10 ">
                                <div id="Div_Mark" runat="Server">
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Content">
                                <%="邮件内容：".ToLang() %>
                            </div>
                            <div class="col-md-10 ">
                                <div id="Div2" runat="Server"></div>
                                <%=new Editer(new Column(), new Form() { DefaultValue = "", FormId = -1 }, new Field{FieldAlias = "邮件内容",FieldName = "Content",}, RegularEnum.Never).Render(CurrentRecord.Content) %>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="_action" value="Save" />
                                <input type="hidden" name="recordId" value="<%=CurrentRecord.ModelId%>" />
                                <div class="btn-group">
                                    <button type="button" form-action="savesubmit" form-success="refresh" class="btn btn-info"><%="重新发送所有".ToLang()%></button>
                                    <button type="button" form-action="submit" form-success="refresh" class="btn btn-info"><%="只发送未成功邮箱".ToLang()%></button>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //重新发送所有
        $("[form-action='savesubmit']").click(function() {
            var $form = $("#formEdit");
            var data = $("#formEdit").serialize() + "&cmd=resendall";
            var actionSuccess = $(this).attr("form-success");
            subMit($form, actionSuccess, data);
        });

        function subMit(form, actionSuccess, data) {
            if ($("#Title").val() === "") {
                whir.toastr.error("<%="标题不能为空".ToLang() %>");
            } else {
                var $form = form;
                $form.post({
                    data: data,
                    success: function (response) {
                        if (response.Status == true) {
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                        return false;
                    },
                    error: function(response) {
                        whir.toastr.erro(response.Message);
                        whir.loading.remove();
                    }
                });
                return false;
            }
            return false;
        }
        //只发送未成功邮箱
        $("[form-action='submit']").click(function() {
            var actionSuccess = $(this).attr("form-success");
            var $form = $("#formEdit");
            var data = $form.serialize() + "&cmd=resendfail";
            subMit($form, actionSuccess, data);
        });
    </script>
</asp:Content>
