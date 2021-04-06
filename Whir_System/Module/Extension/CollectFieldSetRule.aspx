<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="CollectFieldSetRule.aspx.cs" Inherits="Whir_System_Module_Extension_CollectFieldSetRule" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var WebCode = "";
        $(function () {
            getWebPageCode();
        });

        function getWebPageCode() {
            $("#HTMLCode").val("<%="源码加载中...".ToLang()%>");
            var url = $("#Url").val();
            $.ajax({
                type: "POST",
                url: "<%=SysPath %>ajax/extension/GetWebPageCode.aspx",
                data: { weburl: url },
                success: function (data) {
                    WebCode = data;
                    $("#HTMLCode").val(data);
                },
                error: function (msg) {
                    $("#HTMLCode").val("");
                }
            });
        }

        function GetFieldCode() {
            var listStartCode = $("#TextStart").val();
            var listEndCode = $("#TextEnd").val();
            var indexStart = WebCode.indexOf(listStartCode);
            if (indexStart > -1) {
                var step1Code = WebCode.substring(indexStart + listStartCode.length);
                var indexEnd = step1Code.indexOf(listEndCode);
                if (indexEnd > -1) {
                    var step2Code = step1Code.substring(0, indexEnd);
                    $("#HTMLCode").val(step2Code);
                }
            }

        }

        $(function () {

            $("[name=_dialog] .btn-primary", parent.document).click(function () {

                if ($("#TextStart").val() != "" && $("#TextEnd").val() != "") {
                    $.ajax({
                        type: "POST",
                        url: "<%=SysPath %>Handler/Extension/Collect.aspx",
                        data: { _action: 'Finish', formid: <%= FormId%>, collectid:<%= CollectId%> , TextStart: $("#TextStart").val(), TextEnd: $("#TextEnd").val() },
                        success: function(response) {
                            response = eval('(' + response + ')');
                            if (response.Status == true) {
                                window.parent.whir.toastr.success(response.Message);
                                window.parent.whir.dialog.remove();
                            } else {
                                window.parent.whir.dialog.alert(response.Message);
                            }

                        },
                        error: function(msg) {

                        }
                    });
                } else
                    window.parent.whir.toastr.warning('<%="字段代码不能为空！".ToLang()%>');

                return false;
            });
         });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel">
        <div class="panel-body">
            <div class="row">
                <div class="col-md-9">
                    <div class="input-group">
                        <input type="url" id="Url" value="<%=Link %>" name="WebUrl" class="form-control" required="true"
                            maxlength="64" />
                            <a class="btn btn-white input-group-addon " onclick="getWebPageCode()"><%="获取源码".ToLang()%></a>
                    </div>
                    <div class="space15"></div>
                    <textarea id="HTMLCode" name="HTMLCode" class="form-control" data-toggle="tooltip" data-placement="top"
                        style="width: 100%; height: 442px;"></textarea>
                </div>
                <div class="col-md-3">
                    <div class="space15"></div>
                    <div class="space15"></div>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" sizcache="1" sizset="0">
                        <tr>
                            <td><%="字段开始代码：".ToLang()%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <textarea id="TextStart" style="height: 120px" name="TextStart" required="true" class="form-control" style="width: 95%; height: 180px;"><%=CurrenField.TextStart%></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td><%="字段结束代码：".ToLang()%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <textarea style="height: 120px" id="TextEnd" name="TextEnd" required="true" class="form-control" style="width: 95%; height: 180px;"><%=CurrenField.TextEnd%></textarea>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="space15"></div>
                                <a class="btn btn-white" onclick="GetFieldCode()"><b><%="测试字段".ToLang()%></b></a>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
