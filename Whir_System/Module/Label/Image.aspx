<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Image.aspx.cs" Inherits="Whir_System_Module_Label_Image" %>

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
                        <div id="panelcontent" class="panel panels">
                            <form id="formEdit" class="form-horizontal">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        条件
                                    </div>
                                    <div class="panel-body">
                                        <div class="form_center">
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="ItemID">
                                                    <%=" 文件地址：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="FileName" name="FieldName" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="ItemID">
                                                    <%=" 错误文件名称：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="ErrorFile" name="ErrorFile" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="ItemID">
                                                    <%=" 是否显示错误文件：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <ul class="list" id="text1">
                                                        <li>
                                                            <input type="radio" id="lstIsShowError_True" name="lstIsShowError" value="True" />
                                                            <label for="lstIsShowError_True">
                                                                是</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="lstIsShowError_False" name="lstIsShowError" value="False" />
                                                            <label for="lstIsShowError_False">
                                                                否</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="Width">
                                                    <%=" 宽度：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="Width" name="Width" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="Height">
                                                    <%=" 高度：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="Height" name="Height" />
                                                </div>
                                            </div>
                                            <div class="form-group" align="center">
                                                <input type="hidden" name="labelName" id="labelName" value="Image" />
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
                                        FileName：文件地址&nbsp;&nbsp;&nbsp;ErrorFile：错误文件名称&nbsp;&nbsp;&nbsp;IsShowError：是否显示错误文件&nbsp;&nbsp;&nbsp;Width：宽度&nbsp;&nbsp;&nbsp;Height：宽度
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
                Width: {
                    validators: {
                        integer: {
                            message: '<%="高度为正整数".ToLang() %>'
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
</asp:Content>
