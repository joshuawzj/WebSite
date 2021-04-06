<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="EmailMass.aspx.cs" Inherits="Whir_System_Plugin_Email_EmailMass"
    ValidateRequest="false" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Controls.UI.Controls" %>
<%@ Import Namespace="Whir.Domain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        $(function () {
            var txtEmailsId = "Emails";
            $("#" + txtEmailsId).focusout(
                function () {
                    var value = $.trim($("#" + txtEmailsId).val());
                    var regExp = new RegExp("[^;]$");
                    if (value !== "" && regExp.test(value)) {
                        $("#" + txtEmailsId).val(value + ";"); //自动为邮件添加分号结尾
                    }
                    $("#" + txtEmailsId).blur();
                });
        });

        function select_member() {
            whir.dialog.frame('<%="选择会员".ToLang() %>', 'MemberSelect.aspx?jscallback=fill_member&user=gq', "", 1000, 600);
        }

        //选择会员后显示会员
        function fill_member(json) {
            var mes = "";
            for (var i = 0; i < json.length; i++) {
                mes += "<input name='member' type='checkbox' checked='true' email='" + json[i]["Email"] + "' value='" + json[i]["Id"] + "'>" + json[i]["LoginName"];
            }

            $("#divs_members").html(mes);
        }

        function setMemberInHidden() {
            var data = "";
            $("#divs_members input[name='member']").each(function () {
                if ($(this).attr("checked")) {
                    data += $(this).val() + "|" + $(this).attr("email") + ",";
                }
            });
            if (data.length > 0) {
                data = data.substring(0, data.length - 1);
            }
            else {
                whir.dialog.alert("<%="请选择会员！".ToLang() %>");
                return false;
            }
            $("[name=hid_members]").val(data);
            return true;
        }
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
                ueContent.execCommand('inserthtml', insertTxt);
            }
        }
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            
            <div class="panel-body">
                 <ul class="nav nav-tabs">
                     <li class="active"><a data-toggle="tab" href="#single" aria-expanded="true"><%="邮件群发".ToLang()%></a></li>
                     <li ><a  href="SendEmailLog.aspx" aria-expanded="true"><%="发送记录".ToLang()%></a></li>
                  </ul>
                <br />
                 
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/EmailMass.aspx">
                    <div class="form-group">
                        <div class="col-md-2 text-right" for="SendType">
                            <%="发送方式：".ToLang() %>
                        </div>
                        <div class="col-md-10 ">
                            <ul class="list">
                                <li>
                                    <input type="radio" id="SendType_0" checked="checked" name="SendType" value="0" />
                                    <label for="SendType_0">
                                        <%="按选择会员".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="SendType_1" name="SendType" value="1" />
                                    <label for="SendType_1">
                                        <%="按会员组".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="SendType_2" name="SendType" value="2" />
                                    <label for="SendType_2">
                                        <%="指定邮箱".ToLang()%></label>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="form-group" id="div_members">
                        <div class="col-md-2 control-label" for="Member">
                        </div>
                        <div class="col-md-10 " id="divs_members">
                        </div>
                    </div>
                    <div class="form-group" id="div_byMember">
                        <div class="col-md-2 control-label" for="Member">
                            <%="选择会员：".ToLang() %>
                        </div>
                        <div class="col-md-10 ">
                            <a class="btn btn-white" href="javascript:select_member()" title="<%="点击选择会员".ToLang() %>"><%="请选择".ToLang() %></a>
                            <input id="hdMembers" type="hidden" value="" runat="server" />
                            <input id="hdjson" type="hidden" value="" runat="server" />
                        </div>
                    </div>
                    <script type="text/javascript">
                        function select_member1() {
                            BootstrapDialog.show({
                                title:"<%="选择会员".ToLang() %>",
                                message: $('<div></div>').load('memberselect.aspx'),
                                size: BootstrapDialog.SIZE_WIDE,
                                buttons: [
                                    {
                                        label: '<%="选择".ToLang() %>',
                                        cssClass: 'btn-primary',
                                        action: function(dialogItself) {
                                            if ($table.bootstrapTable('getSelections').length == 0) {
                                                whir.dialog.alert("<%="请选择会员！".ToLang() %>");

                                            } else {
                                                fill_member($table.bootstrapTable('getSelections'));
                                                dialogItself.close();
                                            }
                                        }
                                    }, {
                                        label: '<%="取消".ToLang() %>',
                                        action: function(dialogItself) {
                                            dialogItself.close();
                                        }
                                    }
                                ]
                            });
                        };
   
                    </script>
                    <div class="form-group" id="div_byGroup" style="display: none">
                        <div class="col-md-2 control-label" for="MemberGroup">
                            <%="选择会员组：".ToLang() %>
                        </div>
                        <div class="col-md-10 ">
                            <select id="MemberGroup" name="MemberGroup" class="form-control">
                                <%=OptionMemberGroup%>
                            </select>
                        </div>
                    </div>
                    <div class="form-group" id="div_byEmails" style="display: none">
                        <div class="col-md-2 control-label" for="Emails">
                            <%="指定邮箱：".ToLang() %>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="Emails" name="Emails" value="" class="form-control"
                                validationexpression="(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*;)*" data-toggle="tooltip"
                                data-placement="top" title="<%="邮箱地址用英文“;”分号隔开".ToLang() %>" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Title">
                            <%="邮件标题：".ToLang() %>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="Title" name="Title" value="" class="form-control" required="true"
                                maxlength="30" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Content">
                            <%="邮件内容：".ToLang() %>
                        </div>
                        <div class="col-md-10 ">
                            <div id="Div_Mark" runat="Server">
                            </div>
                            <%=new Editer(new Column(), new Form() { DefaultValue = "", FormId = -1 }, new Field{FieldAlias = "邮件内容",FieldName = "Content",}, RegularEnum.Never).Render("") %>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10 ">
                            <input type="hidden" name="_action" value="Save" />
                            <input type="hidden" name="hid_members" />
                            <%if (IsCurrentRoleMenuRes("349"))
                              { %>
                            <button form-action="savesubmit" form-success="refresh" class="btn btn-info">
                                <%="保存以后发送".ToLang()%></button>
                                <%} %>
                                 <%if (IsCurrentRoleMenuRes("365"))
                                   { %>
                            <button form-action="submit" form-success="refresh" class="btn btn-info">
                                <%="立即发送".ToLang()%></button>
                                <%} %>
                                
                        </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //选择事件
        $(document).ready(function () {
            $("input[name='SendType']").next().click(function () {
                Change();

            });
            $("input[name='SendType']").parent().next().click(function () {
                Change();

            });
        });
        function Change() {
            if ($("input[name=SendType]:checked").val() === "0") {
                $("#div_byMember").show();
                $("#div_byGroup").hide();
                $("#div_byEmails").hide();
                $("#div_members").show();
            } else if ($("input[name=SendType]:checked").val() === "1") {
                $("#div_byMember").hide();
                $("#div_byGroup").show();
                $("#div_byEmails").hide();
                $("#div_members").hide();
            } else {
                $("#div_byMember").hide();
                $("#div_byGroup").hide();
                $("#div_byEmails").show();
                $("#div_members").hide();
            }
        }

        //保存以后发送
        $("[form-action='savesubmit']").click(function() {
            var data;
            var actionSuccess = $(this).attr("form-success");
            var $form = $("#formEdit");
            if ($("input[name=SendType]:checked").val() === "0") {
                if (setMemberInHidden()) {
                    data = $form.serialize() + "&cmd=later";
                    return subMit($form, actionSuccess, data);
                }
            } else {
                data = $form.serialize() + "&cmd=later";
               return subMit($form, actionSuccess, data);
            }
        });

        function subMit(form, actionSuccess, data) {
            if ($("#Title").val() === "") {
                whir.toastr.error("<%="标题不能为空".ToLang() %>");
                return false;
            } else {
                var $form = form;
                $form.post({
                    data: data,
                    success: function(response) {
                        if (response.Status == true) {
                           whir.toastr.success(response.Message);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    },
                    error: function(response) {
                        whir.toastr.error(response.Message);
                        whir.loading.remove();
                    }
                });
                return false;
            }
            return false;
        }
        //立即发送

        $("[form-action='submit']").click(function () {
            var data;
            var actionSuccess = $(this).attr("form-success");
            var $form = $("#formEdit");
            if ($("input[name=SendType]:checked").val() === "0") {
                if (setMemberInHidden()) {
                    data = $form.serialize() + "&cmd=now";
                    return subMit($form, actionSuccess, data);
                }
            } else {
                data = $form.serialize() + "&cmd=now";
                return subMit($form, actionSuccess, data);
            }
           
        });
    </script>
</asp:Content>
