<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Metakey.aspx.cs" Inherits="Whir_System_Module_Label_Metakey" %>

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
                                                <div class="col-md-2 control-label" for="ParamName">
                                                    <%=" 参数：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="ParamName" name="ParamName" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group" align="center">
                                                <input type="hidden" name="labelName" id="labelName" value="Metakey" />
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
                                        Type：类型&nbsp;&nbsp;&nbsp;ColumnId：栏目Id&nbsp;&nbsp;&nbsp;ParamName：参数
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
