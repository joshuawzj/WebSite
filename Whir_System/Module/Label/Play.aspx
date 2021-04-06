<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Play.aspx.cs" Inherits="Whir_System_Module_Label_Play" %>

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
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="PlayFile">
                                                    <%=" 文件名称：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="PlayFile" name="PlayFile" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="AutoList">
                                                    <%=" 自动播放：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <ul class="list" id="Ul4">
                                                        <li>
                                                            <input type="radio" checked="checked" id="AutoList_True" name="AutoList" value="True" />
                                                            <label for="AutoList_True">
                                                                开启</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="AutoList_False" name="AutoList" value="False" />
                                                            <label for="AutoList_False">
                                                                关闭</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="Width">
                                                    <%=" 宽度：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="Width" name="Width" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="Height">
                                                    <%=" 高度：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="Height" name="Height" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="lstShowErrorFile">
                                                    <%=" 是否显示错误文件：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <ul class="list" id="Ul1">
                                                        <li>
                                                            <input type="radio" checked="checked" id="lstShowErrorFile_True" name="lstShowErrorFile" value="True" />
                                                            <label for="lstShowErrorFile_True">
                                                                是</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="lstShowErrorFile_False" name="lstShowErrorFile" value="False" />
                                                            <label for="lstShowErrorFile_False">
                                                                否</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group" id="divErrorOne">
                                                <div class="col-md-2 control-label" for="ErrorFileName">
                                                    <%=" 错误文件名称：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="ErrorFileName" name="ErrorFileName" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group" align="center">
                                                <input type="hidden" name="labelName" id="labelName" value="play" />
                                                <input type="hidden" name="_action" id="_action" value="GetWtl" />
                                                <a id="lbtnGenerate" href="javascript:;" class="btn btn-primary"><b>开始生成</b></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </form>
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    置标属性描述
                                </div>
                                <div class="panel-body">
                                    <div class="form_center">
                                        <div class="padding_10">
                                            FileName：播放文件名称 &nbsp;&nbsp;&nbsp; IsAutoPlay：是否自动播放&nbsp;&nbsp;&nbsp; Width：显示的宽度<br />
                                            Height：显示的高度&nbsp;&nbsp;&nbsp; IsShowError：是否显示错误文件 &nbsp;&nbsp;&nbsp; ErrorFile：错误文件名称
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
    </div>
    <script type="text/javascript">
        $("#paneltres").height($("#panelcontent").height());
        var _fieldsoptions = {
            fields: {
                PlayFile: {
                    validators: {
                        notEmpty: {
                            message: '<%="文件名称为必填项".ToLang() %>'
                        }
                    }
                },
                Width: {
                    validators: {
                        integer: {
                            message: '<%="宽度为正整数".ToLang() %>'
                        }
                    }
                },
                Height: {
                    validators: {
                        integer: {
                            message: '<%="高度为正整数".ToLang() %>'
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
            var divErrorOne = $("#divErrorOne");
            var isShowErrorFile = $("[name=lstShowErrorFile]");
            isShowErrorFile.next().click(function () {
                if ($("[name=lstShowErrorFile]").prop('checked')) {
                    divErrorOne.css("display", "");  //显示
                } else {
                    divErrorOne.css("display", "none");  //隐藏
                }
            });
            isShowErrorFile.parent().next().click(function () {
                if ($("[name=lstShowErrorFile]").prop('checked')) {
                    divErrorOne.css("display", ""); //显示
                } else {
                    divErrorOne.css("display", "none"); //隐藏
                }
            });

        });
    </script>
</asp:Content>
