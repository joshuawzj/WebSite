<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Category.aspx.cs" Inherits="Whir_System_Module_Label_Category" %>

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
                        <div id="panelcontent" class="panel panels"  >
                            <form id="formEdit" class="form-horizontal">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        条件
                                    </div>
                                    <div class="panel-body">
                                        <div class="form_center">
                                            <whir:SiteColumn ID="SiteColumn2" runat="server" />
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="ddlSite">
                                                    <%=" 所属父Id：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="ParentID" name="ParentID" regular="Intege" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="ddlSite">
                                                    <%=" 当前栏目是否关联附属类别栏目：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <ul class="list" id="text1">
                                                        <li>
                                                            <input type="radio" id="IsParentColumnId_True" name="IsParentColumnId" value="True" />
                                                            <label for="IsParentColumnId_True">
                                                                是</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="IsParentColumnId_False" name="IsParentColumnId" value="False" />
                                                            <label for="IsParentColumnId_False">
                                                                否</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="ddlSite">
                                                    <%=" SQL语句：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <textarea id="SQL" name="SQL" class="form-control" rows="8" width="100%"></textarea>
                                                </div>
                                            </div>
                                            <div class="form-group" align="center">
                                                <input type="hidden" name="labelName" id="labelName" value="category" />
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
                                        ColumnId：栏目Id&nbsp;&nbsp;&nbsp;ParentId：所属父Id&nbsp;&nbsp;&nbsp;IsParentColumnId：当前栏目是否关联附属类别栏目&nbsp;&nbsp;&nbsp;SQL：SQL语句
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
                Site: {
                    validators: {
                        notEmpty: {
                            message: '<%="站点为必填项".ToLang() %>'
                        }
                    }
                },
                ParentID: {
                    validators: {
                        integer: {
                            message: '<%="所属父Id为正整数".ToLang() %>'
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
