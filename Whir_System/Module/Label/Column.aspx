<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Column.aspx.cs" Inherits="Whir_System_Module_Label_Column" %>

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
                        <div id="panelcontent" class="panel panels ">
                            <form id="formEdit" class="form-horizontal">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        条件
                                    </div>
                                    <div class="panel-body">
                                        <div class="form_center">
                                            <whir:SiteColumn ID="SiteColumn1" runat="server" />
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="ddlSite">
                                                    <%=" 字段名称：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="Field" name="Field" value="ColumnName"
                                                        regular="Intege" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="ddlSite">
                                                    <%=" 是否获取指定栏目的父级栏目：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <ul class="list" id="text1">
                                                        <li>
                                                            <input type="radio" id="lstParent_True" name="lstParent" value="True" />
                                                            <label for="lstParent_True">
                                                                是</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="lstParent_False" name="lstParent" value="False" />
                                                            <label for="lstParent_False">
                                                                否</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="ddlSite">
                                                    <%=" 限制文字长度：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="Length" name="Length" value="" regular="Intege" />
                                                </div>
                                            </div>
                                            <div class="form-group" align="center">
                                                <input type="hidden" name="labelName" id="labelName" value="Column" />
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
                                        ColumnId：栏目Id&nbsp;&nbsp;&nbsp;Field：字段名称&nbsp;&nbsp;&nbsp;Length：限制文字长度&nbsp;&nbsp;&nbsp;Parent：获取指定栏目的父级栏目
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
                },
                Column: {
                    validators: {
                        notEmpty: {
                            message: '<%="站点栏目为必填项".ToLang() %>'
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
</asp:Content>
