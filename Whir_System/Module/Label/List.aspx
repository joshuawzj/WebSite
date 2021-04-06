<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="List.aspx.cs" Inherits="Whir_System_Module_Label_List" %>

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
                                                <div class="col-md-4 control-label" for="CategoryID">
                                                    <%=" 类别ID：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="CategoryID" name="CategoryID" value=""
                                                        regular="Intege" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4 control-label" for="Where">
                                                    <%=" Where语句：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="Where" name="Where" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4 control-label" for="Count">
                                                    <%=" 记录数：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="Count" name="Count" value="1" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4 control-label" for="Order">
                                                    <%=" 排序语句：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="Order" name="Order" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4 control-label" for="NullTip">
                                                    <%=" 提示语句：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="NullTip" name="NullTip" value="暂无信息" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4 control-label" for="Sql">
                                                    <%=" Sql语句：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="Sql" name="Sql" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4 control-label" for="EnabledTop">
                                                    <%=" 启用头条：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <select id="EnabledTop" name="EnabledTop" class="form-control">
                                                        <option value="False">
                                                            <%=" 不启用：".ToLang()%></option>
                                                        <option value="True">
                                                            <%=" 启用：".ToLang()%></option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="form-group" name="divTopInfoOne">
                                                <div class="col-md-4 control-label" for="TopCount">
                                                    <%=" 头条记录数：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="TopCount" name="TopCount" value="1" />
                                                </div>
                                            </div>
                                            <div class="form-group" name="divTopInfoOne">
                                                <div class="col-md-4 control-label" for="TopWhere">
                                                    <%=" 头条Where语句：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="TopWhere" name="TopWhere" value="1" />
                                                </div>
                                            </div>
                                            <div class="form-group" name="divTopInfoTwo">
                                                <div class="col-md-4 control-label" for="TopWhere">
                                                    <%=" 每页都显示头条：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <select id="Topalwayshow" name="Topalwayshow" class="form-control">
                                                        <option value="False">
                                                            <%=" 不启用：".ToLang()%></option>
                                                        <option value="True">
                                                            <%=" 启用：".ToLang()%></option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="form-group" name="divTopInfoTwo">
                                                <div class="col-md-4 control-label" for="TopOrder">
                                                    <%=" 头条排序语句：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="TopOrder" name="TopOrder" value="" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div class="col-md-4 control-label" for="NeedPage">
                                                    <%=" 启用分页：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <select id="NeedPage" name="NeedPage" class="form-control">
                                                        <option value="False">
                                                            <%=" 不启用：".ToLang()%></option>
                                                        <option value="True">
                                                            <%=" 启用：".ToLang()%></option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div class="form-group" name="divPager">
                                                <div class="col-md-4 control-label" for="PageSize">
                                                    <%=" 每页显示记录数：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="PageSize" name="PageSize" value="5" />
                                                </div>
                                            </div>
                                            <div class="form-group" name="divPager">
                                                <div class="col-md-4 control-label" for="Footer">
                                                    <%="显示页码个数：".ToLang()%>
                                                </div>
                                                <div class="col-md-8 ">
                                                    <input type="text" class="form-control" id="Footer" name="Footer" value="5" />
                                                </div>
                                            </div>
                                            <div class="form-group" align="center">
                                                <div class="col-md-offset-4 col-md-6 ">
                                                    <input type="hidden" name="labelName" id="labelName" value="List" />
                                                    <input type="hidden" name="_action" id="_action" value="GetWtl" />
                                                    <input type="hidden" name="hfTop" id="hfTop" value="" />
                                                    <input type="hidden" name="hfPager" id="hfPager" value="" />
                                                    <a id="lbtnGenerate" href="javascript:;" class="btn btn-info btn-block"><b>开始生成</b></a>
                                                </div>
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
                                        CategoryId：类别Id&nbsp;&nbsp;&nbsp;NullTip：没有数据时，提示语句&nbsp;&nbsp;&nbsp;Sql：Sql语句&nbsp;&nbsp;&nbsp;Where：条件语句，不需要带“where”<br />
                                        Order：排序语句，不带“order by ”&nbsp;&nbsp;&nbsp;Count：列表记录数&nbsp;&nbsp;&nbsp;ColumnId：栏目Id&nbsp;&nbsp;&nbsp;TopWhere：头条过滤条件语句，不需要带“where”<br />
                                        TopKeyName：主键名称，如果不指定则头条信息会出现在非头条信息中&nbsp;&nbsp;&nbsp;TopOrder：头条排序语句，不带“order by”&nbsp;&nbsp;&nbsp;NeedPage：是否需要分页<br />
                                        TopCount：头条记录数&nbsp;&nbsp;&nbsp;TopAlwayShow：是否每页都显示头条
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
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <%="字段描述：".ToLang()%>
                                </div>
                                <div class="panel-body">
                                    <div class="form_center">
                                        <div id="divColumnFields"></div>
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
                CategoryID: {
                    validators: {
                        integer: {
                            message: '<%="类别ID为正整数".ToLang() %>'
                        }
                    }
                },
                Count: {
                    validators: {
                        integer: {
                            message: '<%="记录数为正整数".ToLang() %>'
                        }
                    }
                },
                TopCount: {
                    validators: {
                        integer: {
                            message: '<%="头条记录数为正整数".ToLang() %>'
                        }
                    }
                },
                Footer: {
                    validators: {
                        integer: {
                            message: '<%="显示页码个数为正整数".ToLang() %>'
                        }
                    }
                },
                PageSize: {
                    validators: {
                        integer: {
                            message: '<%="每页显示记录数为正整数".ToLang() %>'
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
                                    if (response.Message.split('|@|').length == 2) {
                                        $("#Content").val(response.Message.split('|@|')[0]);
                                        $("#divColumnFields").html(response.Message.split('|@|')[1]);
                                    } else {
                                        $("#Content").val(response.Message);
                                        $("#divColumnFields").html("");
                                    }
                                    $("#paneltres").height($("#panelcontent").height());
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
            var hfTop = $("#hfTop");
            var hfPager = $("hfPager");

            if (hfTop.val() == "True") {
                $("[name=divTopInfoOne]").css("display", "");
                $("[name=divTopInfoTwo]").css("display", "");
            }
            else {
                $("[name=divTopInfoOne]").css("display", "none");
                $("[name=divTopInfoTwo]").css("display", "none");
            }
            if (hfPager.val() == "True") {
                $("[name=divPager]").css("display", "");
            } else {
                $("[name=divPager]").css("display", "none");
            }

            //启用头条控制显示隐藏
            $("#EnabledTop").change(function () {
                if ($(this).val() == "True") {
                    $("[name=divTopInfoOne]").show();
                    $("[name=divTopInfoTwo]").show();
                    hfTop.val("True");
                }
                else {
                    $("[name=divTopInfoOne]").hide();
                    $("[name=divTopInfoTwo]").hide();
                    hfTop.val("False");
                }
            });
            //启用分页控制显示隐藏
            $("#NeedPage").change(function () {
                if ($(this).val() == "True") {
                    $("[name=divPager]").show();
                    hfPager.val("True");
                }
                else {
                    $("[name=divPager]").hide();
                    hfPager.val("False");
                }
            });
        });
    </script>
</asp:Content>
