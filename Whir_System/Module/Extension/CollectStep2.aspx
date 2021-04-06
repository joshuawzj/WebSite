<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="CollectStep2.aspx.cs" Inherits="Whir_System_Module_Extension_CollectStep2" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        h3 {
            text-align: center;
            font-size: 12px;
        }

            h3 span {
                margin: 0px 10px 0px 10px;
            }
    </style>
    <script type="text/javascript">
        var WebCode = "";
        var ListCode = "";
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

        function GetListCode() {
            var listStartCode = $("#ListStart").val();
            var listEndCode = $("#ListEnd").val();
            var step1Code = WebCode.split(listStartCode)[1];
            var step2Code = step1Code.split(listEndCode)[0];
            $("#HTMLCode").val(step2Code);
            ListCode = step2Code;
        }

        function GetlistInfo(type) {
            var linkStartCode = $("#LinkStart").val();
            var linkEndCode = $("#LinkEnd").val();
            if (type == 2) {
                linkStartCode = $("#TitleStart").val();
                linkEndCode = $("#TitleEnd").val();
            }
            var listUrl = "";
            var tempListCode = ListCode;
            
            if (tempListCode != "" & linkStartCode != "" & linkEndCode != "") {
                var exsit = true;
                while (exsit) {
                    var startIndex = tempListCode.indexOf(linkStartCode);
                    if (startIndex != -1) {
                        startIndex += linkStartCode.length; //加上自己的长度
                        var linkExt = tempListCode.substring(startIndex);
                        var endIndex = linkExt.indexOf(linkEndCode);
                        if (endIndex != -1) {
                            listUrl += linkExt.substring(0, endIndex) + "\n";
                            tempListCode = linkExt.substring(endIndex + linkEndCode.length);
                        } else {
                            exsit = false;
                        }
                    } else {
                        exsit = false;
                    }
                }
                $("#HTMLCode").val(listUrl);
            } else {
                whir.toastr.error("<%="先执行测试列表、请填写相应内容<br/>再次操作！".ToLang()%>");
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
              <div class="panel-heading" align='center'>
                <span><%="第一步：基本设置".ToLang()%></span>&nbsp;
                <i class="fa fa-lg fa-angle-right"></i>&nbsp;
                <span  class="text-danger"><b><%="第二步：列表页规则设置".ToLang()%></b></span>&nbsp;
                <i class="fa fa-lg fa-angle-right"></i>&nbsp;
                <span><%="第三步：内容页规则设置".ToLang()%></span>&nbsp;
                <i class="fa fa-lg fa-angle-right"></i>&nbsp;<span><%="完成".ToLang()%></span>
            </div>
           
            <div class="panel-body">
                <div class="All_table">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Extension/Collect.aspx">
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading"><%="项目名称：".ToLang()%><asp:Label ID="lblItemName" runat="server"></asp:Label></div>
                                    <div class="panel-body">
                                        <div class="input-group">
                                            <input type="url" id="Url" value="<%=CurrentCollect.WebUrl %>" name="WebUrl" class="form-control" required="true" maxlength="64" />
                                            <a class="btn btn-white input-group-addon " onclick="getWebPageCode()"><%="获取源码".ToLang()%></a>
                                        </div>
                                        <div class="space15"></div>
                                        <textarea id="HTMLCode" name="HTMLCode" class="form-control" data-toggle="tooltip" data-placement="top"
                                            style="width: 100%; height: 442px;"></textarea>
                                    </div>
                                </div>
                            </div>

                            <div class="col-sm-6">
                                <div class="panel panel-default">
                                    <div class="panel-heading"><%="列表设置".ToLang()%></div>
                                    <div class="panel-body">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0" sizcache="1" sizset="0">
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" sizcache="1" sizset="0">
                                                        <tr>
                                                            <td><%="列表开始代码：".ToLang()%>
                                                            </td>
                                                            <td><%="标题开始代码：".ToLang()%>
                                                            </td>
                                                            <td><%="链接开始代码：".ToLang()%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <textarea id="ListStart" class="form-control" name="ListStartCode" required="true" style="width: 95%; height: 180px;"><%=CurrentCollect.ListStartCode %></textarea>
                                                            </td>
                                                            <td>
                                                                <textarea id="TitleStart" class="form-control" name="TitleStartCode" required="true" style="width: 95%; height: 180px;"><%=CurrentCollect.TitleStartCode %></textarea>
                                                            </td>
                                                            <td>
                                                                <textarea id="LinkStart" class="form-control" name="LinkStartCode" required="true" style="width: 95%; height: 180px;"><%=CurrentCollect.LinkStartCode %></textarea>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><%="列表结束代码：".ToLang()%>
                                                            </td>
                                                            <td><%="标题结束代码：".ToLang()%>
                                                            </td>
                                                            <td><%="链接结束代码：".ToLang()%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <textarea id="ListEnd" class="form-control" name="ListEndCode" required="true" style="width: 95%; height: 180px;"><%=CurrentCollect.ListEndCode %></textarea>
                                                            </td>

                                                            <td>
                                                                <textarea id="TitleEnd" class="form-control" name="TitleEndCode" required="true" style="width: 95%; height: 180px;"><%=CurrentCollect.TitleEndCode %></textarea>
                                                            </td>
                                                            <td>
                                                                <textarea id="LinkEnd" class="form-control" name="LinkEndCode" required="true" style="width: 95%; height: 180px;"><%=CurrentCollect.LinkEndCode %></textarea>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <a class="btn btn-white" onclick="GetListCode()"><b><%="测试列表".ToLang()%></b></a>
                                                            </td>
                                                            <td>
                                                                <a class="btn btn-white" onclick="GetlistInfo(2)"><b><%="测试标题".ToLang()%></b></a>
                                                            </td>
                                                            <td>
                                                                <a class="btn btn-white" onclick="GetlistInfo(1)"><b><%="测试链接".ToLang()%></b></a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="button_submit_div text-center">
                            <input type="hidden" name="_action" value="Next" />
                            <input type="hidden" name="CollectId" value="<%=CollectId %>" />
                            <div class="btn-group">
                                <a class="btn btn-info" href="CollectStep1.aspx?collectid=<%=CollectId %>"><%="上一步".ToLang()%></a>
                                <button form-action="submit" form-success="refresh" class="btn btn-info"><%="下一步".ToLang()%></button>
                            </div>
                            <a class="btn text-danger border-danger" href="CollectList.aspx"><%="返回采集管理".ToLang()%></a>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //提交内容
        $("[form-action='submit']").click(function () {
            var strRegex =
                "^((https|http|)://)?[a-z0-9A-Z]{3}\.[a-z0-9A-Z][a-z0-9A-Z]{0,61}?[a-z0-9A-Z]\.com|net|cn|cc (:s[0-9]{1-4})?/$";
            var re = new RegExp(strRegex);
            if (re.test($("#Url").val())) {
                var actionSuccess = $(this).attr("form-success");
                var $form = $("#formEdit");
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            location.href = "CollectStep3.aspx?collectid=<%=CollectId %>";
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
            } else {
                whir.dialog.alert("<%="Url地址格式不正确".ToLang()%>");
                return false;
            }
        });
    </script>
</asp:Content>
