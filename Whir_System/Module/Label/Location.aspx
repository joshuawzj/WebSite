<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Location.aspx.cs" Inherits="Whir_System_Module_Label_Location" %>

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
                                                <div class="col-md-2 control-label" for="Separator">
                                                    <%=" 分隔的字符：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="Separator" name="Separator" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="lstIsDefault">
                                                    <%=" 是否加首页显示：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <ul class="list" id="text1">
                                                        <li>
                                                            <input type="radio" id="lstIsDefault_True" name="lstIsDefault" value="True" />
                                                            <label for="lstIsDefault_True">
                                                                是</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="lstIsDefault_False" name="lstIsDefault" value="False" />
                                                            <label for="lstIsDefault_False">
                                                                否</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="NullTip">
                                                    <%=" 没有数据时，提示语句：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="NullTip" name="NullTip" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="lstIsAutoLink">
                                                    <%=" 是否加自动链接：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <ul class="list" id="Ul1">
                                                        <li>
                                                            <input type="radio" id="lstIsAutoLink_True" name="lstIsAutoLink" value="True" />
                                                            <label for="lstIsAutoLink_True">
                                                                是</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="lstIsAutoLink_False" name="lstIsAutoLink" value="False" />
                                                            <label for="lstIsAutoLink_False">
                                                                否</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="DefaultValue">
                                                    <%=" 首页名字：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="DefaultValue" name="DefaultValue" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="lstIsFullPath">
                                                    <%=" 是否加自动链接：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <ul class="list" id="Ul2">
                                                        <li>
                                                            <input type="radio" id="lstIsFullPath_True" name="lstIsFullPath" value="True" />
                                                            <label for="lstIsFullPath_True">
                                                                是</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="lstIsFullPath_False" name="lstIsFullPath" value="False" />
                                                            <label for="lstIsFullPath_False">
                                                                否</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="lstIsSubColumn">
                                                    <%=" 是否子站：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <ul class="list" id="Ul3">
                                                        <li>
                                                            <input type="radio" id="lstIsSubColumn_True" name="lstIsSubColumn" value="True" />
                                                            <label for="lstIsSubColumn_True">
                                                                是</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="lstIsSubColumn_False" name="lstIsSubColumn" value="False" />
                                                            <label for="lstIsSubColumn_False">
                                                                否</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="SubID">
                                                    <%=" 子站Id：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="SubID" name="SubID" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="UrlParam">
                                                    <%=" 默认跳转路径：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="UrlParam" name="UrlParam" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="lstIsCategory">
                                                    <%=" 是否启用单独类别：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <ul class="list" id="Ul4">
                                                        <li>
                                                            <input type="radio" id="lstIsCategory_True" name="lstIsCategory" value="True" />
                                                            <label for="lstIsCategory_True">
                                                                是</label>
                                                        </li>
                                                        <li>
                                                            <input type="radio" id="lstIsCategory_False" name="lstIsCategory" value="False" />
                                                            <label for="lstIsCategory_False">
                                                                否</label>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="CategoryParam">
                                                    <%=" 类别参数名称：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="CategoryParam" name="CategoryParam" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-2 control-label" for="Field">
                                                    <%=" 类别显示字段值：".ToLang()%>
                                                </div>
                                                <div class="col-md-10 ">
                                                    <input type="text" class="form-control" id="Field" name="Field" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group" align="center">
                                                <input type="hidden" name="labelName" id="labelName" value="location" />
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
                                        ColumnId：栏目Id&nbsp;&nbsp;&nbsp;Separator：分隔的字符&nbsp;&nbsp;&nbsp;IsFullPath：是否显示全路径&nbsp;&nbsp;&nbsp;IsAutoLink：是否加自动链接&nbsp;&nbsp;&nbsp;NullTip：没有数据时，提示语句&nbsp;&nbsp;&nbsp;<br />
                                        IsDefault：是否加首页显示&nbsp;&nbsp;&nbsp;DefaultValue：首页名字&nbsp;&nbsp;&nbsp;IsSubColumn：是否子站&nbsp;&nbsp;&nbsp;SubId：子站Id<br />
                                        UrlParam：默认跳转的URL地址&nbsp;&nbsp;&nbsp;IsCategory：是否启用显示单独类别&nbsp;&nbsp;&nbsp;CategoryParam：类别参数名称&nbsp;&nbsp;&nbsp;Field：类别显示字段值
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
                            message: '<%="栏目为必填项".ToLang() %>'
                        }
                    }
                },
                SubID: {
                    validators: {
                        integer: {
                            message: '<%="子站ID为正整数".ToLang() %>'
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
