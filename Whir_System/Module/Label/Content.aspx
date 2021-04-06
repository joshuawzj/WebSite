<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Content.aspx.cs" Inherits="Whir_System_Module_Label_Content" %>

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
                            <div id="paneltres" class="panel panels ">
                                <div class="form-group">
                                    <whir:ColumnLeftTree runat="server" ID="ColumnLeftTree1"></whir:ColumnLeftTree>
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
                                            <div class="form-group">
                                                <div class="col-md-3 control-label" for="Field">
                                                    <%=" 字段名称：".ToLang()%>
                                                </div>
                                                <div class="col-md-9 ">
                                                    <input type="text" class="form-control" id="Field" name="Field" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-3 control-label" for="Length">
                                                    <%=" 限制文字长度：".ToLang()%>
                                                </div>
                                                <div class="col-md-9 ">
                                                    <input type="text" class="form-control" id="Length" name="Length" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-3 control-label" for="Where">
                                                    <%=" Where语句：".ToLang()%>
                                                </div>
                                                <div class="col-md-9 ">
                                                    <input type="text" class="form-control" id="Where" name="Where" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-3 control-label" for="lstAutoLink">
                                                    <%=" 是否为栏目加上链接：".ToLang()%>
                                                </div>
                                                <div class="col-md-9 ">
                                                    <ul class="list" id="text1">
                                                        <li>
                                                            <input type="radio" id="lstAutoLink_True" name="lstAutoLink" value="True" />
                                                            <label for="lstAutoLink_True">
                                                                是</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="lstAutoLink_False" name="lstAutoLink" value="False" />
                                                            <label for="lstAutoLink_False">
                                                                否</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-3 control-label" for="Type">
                                                    <%=" 上下篇：".ToLang()%>
                                                </div>
                                                <div class="col-md-9 ">
                                                    <select id="Type" name="Type" class="form-control">
                                                        <option value="">==请选择==</option>
                                                        <option value="prepage">上一篇</option>
                                                        <option value="nextpage">下一篇</option>
                                                    </select>
                                                    <input type="hidden" id="hfType" name="hfType" />
                                                </div>
                                            </div>
                                            <div class="form-group" id="divleft">
                                                <div class="col-md-3 control-label" for="LeftText">
                                                    <%=" 显示内容的左边：".ToLang()%>
                                                </div>
                                                <div class="col-md-9 ">
                                                    <input type="text" class="form-control" id="LeftText" name="LeftText" />
                                                </div>
                                            </div>
                                            <div class="form-group" id="divright">
                                                <div class="col-md-3 control-label" for="RightText">
                                                    <%=" 显示内容的右边：".ToLang()%>
                                                </div>
                                                <div class="col-md-9 ">
                                                    <input type="text" class="form-control" id="RightText" name="RightText" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-3 control-label" for="Sql">
                                                    <%=" Sql语句：".ToLang()%>
                                                </div>
                                                <div class="col-md-9 ">
                                                    <input type="text" class="form-control" id="Sql" name="Sql" />
                                                </div>
                                            </div>
                                            <div class="form-group" align="center">
                                                <input type="hidden" name="labelName" id="labelName" value="content" />
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
                                        Field：字段名称&nbsp;&nbsp;&nbsp;Type：prepage上一篇&nbsp;&nbsp;&nbsp;nextpage下一篇&nbsp;&nbsp;&nbsp;IsAutoLink：是否为栏目加上链接
                        <br />
                                        LeftText：显示内容的左边&nbsp;&nbsp;&nbsp;RightText：显示内容的右边&nbsp;&nbsp;&nbsp;Length：限制文字长度
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
        //提交内容
        var _fieldsoptions = {
            fields: {
                Type: {
                    validators: {
                        notEmpty: {
                            message: '<%="上下篇为必填项".ToLang() %>'
                        }
                    }
                },
                Length: {
                    validators: {
                        integer: {
                            message: '<%="限制文字长度为正整数".ToLang() %>'
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
    <script type="text/javascript">
        $(function () {
            var hfType = $("#hfType");

            if (hfType.val() == "left") {
                //显示上一篇
                $("#divleft").show();

                //隐藏下一篇
                $("#divright").hide();
            }
            else if (hfType.val() == "right") {
                //隐藏上一篇
                $("#divleft").hide();

                //显示下一篇
                $("#divright").show();
            }
            else {
                //隐藏上一篇
                $("#divleft").hide();
                //隐藏下一篇
                $("#divright").hide();
            }


            $("#Type").change(function () {

                if ($(this).val() == "prepage") {

                    //显示上一篇
                    $("#divleft").show();

                    //隐藏下一篇
                    $("#divright").hide();
                    hfType.val("left");
                }
                else if ($(this).val() == "nextpage") {
                    //隐藏上一篇
                    $("#divleft").hide();

                    //显示下一篇
                    $("#divright").show();
                    hfType.val("right");
                }
                else { //为==请选择==
                    //隐藏上一篇
                    $("#divleft").hide();
                    //隐藏下一篇
                    $("#divright").hide();
                    hfType.val("");
                }
            });
        });
    </script>
</asp:Content>
