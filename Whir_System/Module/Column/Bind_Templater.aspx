<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="bind_templater.aspx.cs" Inherits="whir_system_module_column_bind_templater" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="<%=SysPath %>res/js/whir/whir.TipMessage.js"></script>
    <script type="text/javascript" src="<%=SysPath %>res/js/jquery_treetable/jqtreetable.js"></script>
    <!--这个是表格显示的文件-->
    <link rel="stylesheet" type="text/css" href="<%=SysPath %>res/js/jquery_treetable/jqtreetable.css" />
    <style type="text/css">
        .table tbody td {
            padding: 0px !important;
            vertical-align: middle !important;
        }
    </style>
    <script type="text/javascript">
        whir.skin.checkbox(false); //取消美化checkbox
        whir.skin.radio(false); //取消美化radio

        $(function () {
            //表格显示
            if ($("#<%=hfStrMap.ClientID %>").val() == "") return false;
            var map = $("#<%=hfStrMap.ClientID %>").val().split(",");
            var options = {
                openImg: "<%=SysPath %>res/images/TreeTable/tv-collapsable.gif", shutImg: "<%=SysPath %>res/images/TreeTable/tv-expandable.gif",
                leafImg: "<%=SysPath %>res/images/TreeTable/tv-item.gif",
                lastOpenImg: "<%=SysPath %>res/images/TreeTable/tv-collapsable-last.gif",
                lastShutImg: "<%=SysPath %>res/images/TreeTable/tv-expandable-last.gif",
                lastLeafImg: "<%=SysPath %>res/images/TreeTable/tv-item-last.gif",
                vertLineImg: "<%=SysPath %>res/images/TreeTable/vertline.gif",
                blankImg: "<%=SysPath %>res/images/TreeTable/blank.gif",
                collapse: false,
                column: 0,
                striped: true,
                highlight: true,
                state: false
            };
            $("tbody[id^='tbdList']").jqTreeTable(map, options);

            //根据栏目类型Id返回栏目类型的名称
            $("tbody[res!='自定义子站'] tr").find("td").find(".modelname").each(function (i, item) {
                var te = $("#<%=dropColumn.ClientID %>").find("option[value='" + $(this).html().replace(/(^\s*)|(\s*$)/g, "") + "']").html();
                $(this).html(te);
            });

            //根据栏目类型Id返回栏目类型的名称
            $("#list3 tr").find("td").find(".modelname").each(function (i, item) {
                var te = $("#<%=ddpSubsite.ClientID %>").find("option[value='" + $(this).html() + "']").html();
                $(this).html(te);
            });

            //隐藏首页
            var indexTr = $("#tbdList0 tr:eq(0)");
            if (indexTr.find("td:eq(1)").attr("model") == "model-1") {
                indexTr.find("td:eq(1)").html("");
                indexTr.find("td:eq(3)").html("");
                indexTr.find("td:eq(4)").html("");
                indexTr.find("td:eq(6)").html("");
                indexTr.find("td:eq(7)").html(""); //首页没有编辑自动清除静态文件功能
            }

            //隐藏模版子站、专题、自定义子站首页
            $("tr[subid]").each(function () {
                if (parseInt($(this).attr("subid")) > 0) { //首页
                    $(this).find("td:eq(1)").html("");
                    $(this).find("td:eq(3)").html("");
                    $(this).find("td:eq(4)").html("");
                    $(this).find("td:eq(6)").html("");
                    //$(this).find("td:eq(7)").html("");//自动清除静态文件
                }
            });

            $(".tdMode").each(function () {
                var spanMode = $(this).find(".tempMode");
                $(this).html(spanMode); //隐藏树数据
                //判断是否外部链接
                var isLink = $(this).attr("url") == "" ? false : true;
                if (isLink) {
                    $(this).parent().find("td:gt(0)").html("");
                } else {//父节点处理
                    var pid = $(this).find("[columnpid]").attr("columnpid");
                    if (pid != "0") {
                        //$(".span" + pid).hide(); //隐藏生成方式
                        //$("[model='model" + pid + "']").html("");
                        //$("[model='model" + pid + "']").parent().find("[name='TdTemp']").html("");
                    }

                    //选中生成方式
                    var checkedValue = $(this).find("[ckdvalue]").attr("ckdvalue");
                    $(this).find("select").val(checkedValue);
                }
            });

            $("select[name^='sel-'].tempMode").change(function () {
                var val = $(this).val();
                switch (val) {
                    case "1": //静态模式
                        $(this).parents("td:eq(0)").next().next().find("input[name^='ckb-']").attr("disabled", false);
                        //$(this).parents("td:eq(0)").next().find("input[name^='ckb-']").attr("disabled", "");
                        break;
                    default:
                        $(this).parents("td:eq(0)").next().next().find("input[name^='ckb-']").attr("checked", false);
                        $(this).parents("td:eq(0)").next().next().find("input[name^='ckb-']").attr("disabled", "disabled");
                        break;
                }
            });
            $(":checkbox[name^='ckb-']").attr("title", "<%="编辑栏目内容时自动清理已生成的静态文件".ToLang()%>");

            //批量设置发布模式
            $("select[name='selAll']").change(function () {
                if ($(this).val() != "") {
                    $(this).parents("table").find("select[name^='sel-'].tempMode").val($(this).val());
                    $("input[name='ckbAll']").prop("checked", false);
                    $(this).parents("table").find("select[name^='sel-'].tempMode").trigger("change");
                }
            });

            //打开栏目别名
            $("input[name^='ckbShowColumnNameStage']").click(function () {
                if ($(this).val() == "0") {
                    $(this).parents("table").find("input[name='txtColumn']").hide();
                    $(this).parents("table").find("input[name='txtColumnNameStage']").show();
                } else {                                     
                    $(this).parents("table").find("input[name='txtColumn']").show();
                    $(this).parents("table").find("input[name='txtColumnNameStage']").hide();
                }
            });
            //打开栏目别名
            $("input[name^='ckbShowColumnNameStage']").next().click(function () {
                $(this).prev().prop("checked", true);
                if ($(this).prev().val() == "0") {
                    $(this).parents("table").find("input[name='txtColumn']").hide();
                    $(this).parents("table").find("input[name='txtColumnNameStage']").show();
                } else {
                    $(this).parents("table").find("input[name='txtColumn']").show();
                    $(this).parents("table").find("input[name='txtColumnNameStage']").hide();
                }
            });

            //批量开启类别
            $("input[name='ckbIsCategoryAll']").click(function () {
                $(this).parents("table").find("input[name^='ckbIsCategory-']").not(':disabled').prop("checked", $(this).is(":checked"));
            });
            //批量设置清理
            $("input[name='ckbAll']").click(function () {
                $(this).parents("table").find("input[name^='ckb-']").not(':disabled').prop("checked", $(this).is(":checked"));
            });

        });

        //站点显示与隐藏
        function showTab(id) {
            $(".tab_common span[id^='tab']").removeClass("show");
            $(".All_list tbody[id^='list']").hide();
            $("#tab" + id).addClass("show");
            $("#list" + id).show();
        }
        $(function () {
            var item = $("#tbdList0").find("tr");
            for (var i = 0; i < item.length; i++) {
                if (i % 2 == 0) {
                    item[i].style.backgroundColor = "#f0f0f0";
                }
            }
            item = $("#tbdList1").find("tr");
            for (var i = 0; i < item.length; i++) {
                if (i % 2 == 0) {
                    item[i].style.backgroundColor = "#f0f0f0";
                }
            }
            item = $("#tbdList2").find("tr");
            for (var i = 0; i < item.length; i++) {
                if (i % 2 == 0) {
                    item[i].style.backgroundColor = "#f0f0f0";
                }
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div class="content-wrap">
            <div class="space15">
            </div>
            <div class="panel">
                <div class="panel-body">
                    <ul class="nav nav-tabs" role="tablist">
                        <li role="presentation" class="active"><a data-toggle="tab" href="#list0"><%="内容管理".ToLang()%></a></li>
                        <li role="presentation"><a data-toggle="tab" href="#list1"><%="子站".ToLang()%></a></li>
                        <li role="presentation"><a data-toggle="tab" href="#list2"><%="专题".ToLang()%></a></li>
                    </ul>
                    <div class="space_5"></div>
                    <div class="tab-content">
                        <div id="list0" class="tab-pane active">
                            <div class="panel-body tableCategory-table-body">
                                <table class="table table-hover bind_templater_table">
                                    <thead>
                                        <tr>
                                            <th class="text-center" style="min-width: 150px">
                                                <input type="radio" name="ckbShowColumnNameStage" value="1" title="<%="显示栏目名称".ToLang()%>" checked="checked" /><span style="cursor: pointer"><%="栏目名称".ToLang()%></span> &nbsp;
                                                <input type="radio" name="ckbShowColumnNameStage" value="0" title="<%="显示栏目别名".ToLang()%>" /><span style="cursor: pointer"><%="栏目别名".ToLang()%> </span>
                                            </th>
                                            <th class="text-center"><%="功能模块".ToLang()%></th>
                                            <th class="text-center"><%="首页模板".ToLang()%></th>
                                            <th class="text-center"><%="列表模板".ToLang()%></th>
                                            <th class="text-center"><%="内容模板".ToLang()%></th>
                                            <th class="text-center"><%="生成模式".ToLang()%></th>
                                            <th class="text-center"><%="开启类别".ToLang()%></th>
                                            <th class="text-center"><%=ClearStaticHTML%></th>
                                        </tr>
                                        <tr>
                                            <th class="text-center" style="min-width: 150px"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center">
                                                <select name="selAll">
                                                    <option value=""><%="全选".ToLang()%></option>
                                                    <option value="0"><%=CreateModeNull%></option>
                                                    <option value="1"><%=CreateModeHtml%></option>
                                                    <option value="2"><%=CreateModelAspx%></option>
                                                </select>
                                            </th>
                                            <th class="text-center">
                                                <label>
                                                    <input type="checkbox" name="ckbIsCategoryAll" title="<%="是否开启类别管理".ToLang()%>" /></label>
                                            </th>
                                            <th class="text-center">
                                                <label>
                                                    <input type="checkbox" name="ckbAll" title="<%="编辑栏目内容时自动清理已生成的静态文件".ToLang()%>" /></label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody res="内容管理" id="tbdList0">
                                        <asp:Repeater ID="rptMainColumnList" runat="server">
                                            <ItemTemplate>
                                                <tr valign="middle">
                                                    <td>
                                                        <%--<div style="white-space: nowrap; line-height: 35px"><%# Eval("ColumnName")%></div>--%>
                                                        <input value="<%# Eval("ColumnName")%>" name="txtColumn" />
                                                        <input value="<%# Eval("ColumnNameStage")%>" name="txtColumnNameStage" style="display: none;" />
                                                        <input type="hidden" value="<%# Eval("ColumnId")%>" name="txtColumnId" />
                                                    </td>
                                                    <td model="model<%# Eval("ColumnId") %>" align="center">
                                                        <em></em>
                                                        <span class="modelname" modelid="<%#Eval("ModelId")%>"><%#Eval("ModelId")%></span>
                                                        <i class="model_delete icon-cross" valueplace="CloumnModel|<%# Eval("ColumnId") %>"
                                                            style="cursor: pointer;" linkurl='<%# Eval("OutUrl") %>'></i>
                                                        <span class="model_edit fontawesome-cog"
                                                            valueplace="CloumnModel|<%# Eval("ColumnId") %>"
                                                            style="cursor: pointer;" linkurl='<%# Eval("OutUrl") %>'></span>
                                                    </td>
                                                    <td name="TdTemp" align="center">
                                                        <em></em>
                                                        <span><%# getTempName(Eval("DefaultTemp").ToStr())%></span>
                                                        <i class="eip_delete icon-cross" valueplace="DefaultTemp|<%# Eval("ColumnId") %>"
                                                            style="cursor: pointer;"></i>
                                                        <span class="eip_edit fontawesome-cog" valueplace="DefaultTemp|<%# Eval("ColumnId") %>"
                                                            style="cursor: pointer;"></span>
                                                    </td>
                                                    <td name="TdTemp" align="center">
                                                        <em></em>
                                                        <span><%# getTempName(Eval("ListTemp").ToStr())%></span>
                                                        <i class="eip_delete icon-cross" valueplace="ListTemp|<%# Eval("ColumnId") %>"
                                                            style="cursor: pointer;"></i>
                                                        <span class="eip_edit fontawesome-cog" valueplace="ListTemp|<%# Eval("ColumnId") %>"
                                                            style="cursor: pointer;"></span>
                                                    </td>
                                                    <td name="TdTemp" align="center">
                                                        <em></em><span>
                                                            <%# getTempName(Eval("ContentTemp").ToStr())%>
                                                        </span><i class="eip_delete icon-cross" valueplace="ContentTemp|<%# Eval("ColumnId") %>"
                                                            style="cursor: pointer;"></i><span class="eip_edit fontawesome-cog" valueplace="ContentTemp|<%# Eval("ColumnId") %>"
                                                                style="cursor: pointer;"></span>
                                                    </td>
                                                    <td class="tdMode" align="center" url="<%# Eval("OutUrl") %>">
                                                        <select class="tempMode span<%# Eval("ColumnId") %>" name="sel-<%# Eval("ColumnId") %>" columnpid="<%# Eval("ParentId") %>" columnid="<%# Eval("ColumnId") %>" ckdvalue="<%# Eval("CreateMode") %>">
                                                            <option value="0"><%=CreateModeNull%></option>
                                                            <option value="1"><%=CreateModeHtml%></option>
                                                            <option value="2"><%=CreateModelAspx%></option>
                                                        </select>
                                                    </td>
                                                    <td align="center">
                                                        <label>
                                                            <input type="checkbox" name="ckbIsCategory-<%# Eval("ColumnId") %>" <%# Eval("IsCategory").ToBoolean()?"checked='checked'": (!Whir.Service.ServiceFactory.ColumnService.IsCategoryParent(Eval("ColumnId").ToInt())?"disabled='disabled'":"") %> /></label>
                                                    </td>
                                                    <td align="center">
                                                        <label>
                                                            <input type="checkbox" name="ckb-<%# Eval("ColumnId") %>" <%# Eval("IsAutoCleanupStaticFiles").ToBoolean()?"checked='checked'": (Eval("CreateMode").ToInt()!=1?"disabled='disabled'":"") %> /></label>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div id="list1" class="tab-pane ">
                            <div class="panel-body tableCategory-table-body">
                                <table class="table table-hover bind_templater_table">
                                    <thead>
                                        <tr>
                                            <th style="text-align: center; min-width: 150px">
                                                <input type="radio" name="ckbShowColumnNameStage1" value="1" title="<%="显示栏目名称".ToLang()%>" checked="checked" /><span style="cursor: pointer"><%="栏目名称".ToLang()%></span> &nbsp;
                                                <input type="radio" name="ckbShowColumnNameStage1" value="0" title="<%="显示栏目别名".ToLang()%>" /><span style="cursor: pointer"><%="栏目别名".ToLang()%> </span>
                                            </th>
                                            <th style="text-align: center;">
                                                <%="功能模块".ToLang()%>
                                            </th>
                                            <th style="text-align: center;">
                                                <%="首页模板".ToLang()%>
                                            </th>
                                            <th style="text-align: center;">
                                                <%="列表模板".ToLang()%>
                                            </th>
                                            <th style="text-align: center;">
                                                <%="内容模板".ToLang()%>
                                            </th>
                                            <th style="text-align: center;">
                                                <%="生成模式".ToLang()%>
                                            </th>
                                            <th class="text-center"><%="是否开启类别".ToLang()%></th>
                                            <th style="text-align: center;">
                                                <%=ClearStaticHTML%>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th class="text-center" style="min-width: 150px"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center">
                                                <select name="selAll">
                                                    <option value=""><%="全选".ToLang()%></option>
                                                    <option value="0"><%=CreateModeNull%></option>
                                                    <option value="1"><%=CreateModeHtml%></option>
                                                    <option value="2"><%=CreateModelAspx%></option>
                                                </select>
                                            </th>
                                            <th class="text-center">
                                                <label>
                                                    <input type="checkbox" name="ckbIsCategoryAll" title="<%="是否开启类别管理".ToLang()%>" /></label>
                                            </th>
                                            <th class="text-center">
                                                <label>
                                                    <input type="checkbox" name="ckbAll" title="<%="编辑栏目内容时自动清理已生成的静态文件".ToLang()%>" /></label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody res="模版子站" id="tbdList1">
                                        <asp:Repeater ID="rptSubTem" runat="server" OnItemDataBound="rptSubTem_ItemDataBound">
                                            <ItemTemplate>
                                                <tr valign="middle">
                                                    <td colspan="8">
                                                        <div style="white-space: nowrap; line-height: 35px"><%# Eval("SubjectClassName")%></div>
                                                        <asp:HiddenField ID="hidSubjectClassID" runat="server"
                                                            Value='<%# Eval("SubjectClassId") %>' />
                                                    </td>
                                                </tr>
                                                <asp:Repeater ID="rptList" runat="server">
                                                    <ItemTemplate>
                                                        <tr valign="middle" subid='<%# Eval("SubId") %>'>
                                                            <td>
                                                                <input value="<%# Eval("ColumnName")%>" name="txtColumn" />
                                                                <input value="<%# Eval("ColumnNameStage")%>" name="txtColumnNameStage" style="display: none;" />
                                                                <input type="hidden" value="<%# Eval("ColumnId")%>" name="txtColumnId" />
                                                            </td>
                                                            <td model="model<%# Eval("ColumnId") %>" align="center">
                                                                <em></em><span class="modelname">
                                                                    <%# Eval("ModelId")%></span> <i class="model_delete icon-cross icon-cross" valueplace="CloumnModel|<%# Eval("ColumnId") %>"
                                                                        style="cursor: pointer;" linkurl='<%# Eval("OutUrl") %>'></i><span class="model_edit fontawesome-cog"
                                                                            valueplace="CloumnModel|<%# Eval("ColumnId") %>" style="cursor: pointer;" linkurl='<%# Eval("OutUrl") %>'></span>
                                                            </td>
                                                            <td name="TdTemp" align="center">
                                                                <em></em><span>
                                                                    <%# getTempName(Eval("DefaultTemp").ToStr())%>
                                                                </span><i class="eip_delete icon-cross" valueplace="DefaultTemp|<%# Eval("ColumnId") %>"
                                                                    style="cursor: pointer;"></i><span class="eip_edit fontawesome-cog" valueplace="DefaultTemp|<%# Eval("ColumnId") %>"
                                                                        style="cursor: pointer;"></span>
                                                            </td>
                                                            <td name="TdTemp" align="center">
                                                                <em></em><span>
                                                                    <%# getTempName(Eval("ListTemp").ToStr())%>
                                                                </span><i class="eip_delete icon-cross" valueplace="ListTemp|<%# Eval("ColumnId") %>" style="cursor: pointer;"></i><span class="eip_edit fontawesome-cog" valueplace="ListTemp|<%# Eval("ColumnId") %>" style="cursor: pointer;"></span>
                                                            </td>
                                                            <td name="TdTemp" align="center">
                                                                <em></em><span>
                                                                    <%# getTempName(Eval("ContentTemp").ToStr())%>
                                                                </span><i class="eip_delete icon-cross" valueplace="ContentTemp|<%# Eval("ColumnId") %>"
                                                                    style="cursor: pointer;"></i><span class="eip_edit fontawesome-cog" valueplace="ContentTemp|<%# Eval("ColumnId") %>"
                                                                        style="cursor: pointer;"></span>
                                                            </td>
                                                            <td class="tdMode" align="center" url="<%# Eval("OutUrl") %>">
                                                                <select class="tempMode span<%# Eval("ColumnId") %>" name="sel-<%# Eval("ColumnId") %>" columnpid="<%# Eval("ParentId") %>" columnid="<%# Eval("ColumnId") %>" ckdvalue="<%# Eval("CreateMode") %>">
                                                                    <option value="0"><%=CreateModeNull%></option>
                                                                    <option value="1"><%=CreateModeHtml%></option>
                                                                    <option value="2"><%=CreateModelAspx%></option>
                                                                </select>
                                                            </td>
                                                            <td align="center">
                                                                <label>
                                                                    <input type="checkbox" name="ckbIsCategory-<%# Eval("ColumnId") %>" <%# Eval("IsCategory").ToBoolean()?"checked='checked'": (!Whir.Service.ServiceFactory.ColumnService.IsCategoryParent(Eval("ColumnId").ToInt())?"disabled='disabled'":"") %> /></label>
                                                            </td>
                                                            <td align="center">
                                                                <label>
                                                                    <input type="checkbox" name="ckb-<%# Eval("ColumnId") %>" <%# Eval("IsAutoCleanupStaticFiles").ToBoolean()?"checked='checked'": (Eval("CreateMode").ToInt()!=1?"disabled='disabled'":"") %> /></label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <div id="list2" class="tab-pane ">
                            <div class="panel-body tableCategory-table-body">
                                <table class="table table-hover bind_templater_table">
                                    <thead>
                                        <tr>
                                            <th style="text-align: center; min-width: 150px">
                                                <input type="radio" name="ckbShowColumnNameStage2" value="1" title="<%="显示栏目名称".ToLang()%>" checked="checked" /><span style="cursor: pointer"><%="栏目名称".ToLang()%></span> &nbsp;
                                                <input type="radio" name="ckbShowColumnNameStage2" value="0" title="<%="显示栏目别名".ToLang()%>" /><span style="cursor: pointer"><%="栏目别名".ToLang()%> </span>

                                            </th>
                                            <th style="text-align: center;">
                                                <%="功能模块".ToLang()%>
                                            </th>
                                            <th style="text-align: center;">
                                                <%="首页模板".ToLang()%>
                                            </th>
                                            <th style="text-align: center;">
                                                <%="列表模板".ToLang()%>
                                            </th>
                                            <th style="text-align: center;">
                                                <%="内容模板".ToLang()%>
                                            </th>
                                            <th style="text-align: center;">
                                                <%="生成模式".ToLang()%>
                                            </th>
                                            <th class="text-center"><%="是否开启类别".ToLang()%></th>
                                            <th style="text-align: center;">
                                                <%=ClearStaticHTML%>
                                            </th>
                                        </tr>
                                        <tr>
                                            <th class="text-center" style="min-width: 150px"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center"></th>
                                            <th class="text-center">
                                                <select name="selAll">
                                                    <option value=""><%="全选".ToLang()%></option>
                                                    <option value="0"><%=CreateModeNull%></option>
                                                    <option value="1"><%=CreateModeHtml%></option>
                                                    <option value="2"><%=CreateModelAspx%></option>
                                                </select>
                                            </th>
                                            <th class="text-center">
                                                <label>
                                                    <input type="checkbox" name="ckbIsCategoryAll" title="<%="是否开启类别管理".ToLang()%>" /></label>
                                            </th>
                                            <th class="text-center">
                                                <label>
                                                    <input type="checkbox" name="ckbAll" title="<%="编辑栏目内容时自动清理已生成的静态文件".ToLang()%>" /></label>
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody res="专题" id="tbdList2">
                                        <asp:Repeater ID="rptSubject" runat="server" OnItemDataBound="rptSubTem_ItemDataBound">
                                            <ItemTemplate>
                                                <tr valign="middle">
                                                    <td colspan="8">
                                                        <div style="white-space: nowrap; line-height: 35px"><%# Eval("SubjectClassName")%></div>
                                                        <asp:HiddenField ID="hidSubjectClassID" runat="server"
                                                            Value='<%# Eval("SubjectClassId") %>' />
                                                    </td>
                                                </tr>
                                                <asp:Repeater ID="rptList" runat="server">
                                                    <ItemTemplate>
                                                        <tr valign="middle" subid='<%# Eval("SubId") %>'>
                                                            <td>
                                                                <input value="<%# Eval("ColumnName")%>" name="txtColumn" />
                                                                <input value="<%# Eval("ColumnNameStage")%>" name="txtColumnNameStage" style="display: none;" />
                                                                <input type="hidden" value="<%# Eval("ColumnId")%>" name="txtColumnId" />
                                                            </td>
                                                            <td model="model<%# Eval("ColumnId") %>" align="center">
                                                                <em></em><span class="modelname">
                                                                    <%# Eval("ModelId")%></span> <i class="model_delete icon-cross" valueplace="CloumnModel|<%# Eval("ColumnId") %>"
                                                                        style="cursor: pointer;" linkurl='<%# Eval("OutUrl") %>'></i><span class="model_edit fontawesome-cog"
                                                                            valueplace="CloumnModel|<%# Eval("ColumnId") %>" style="cursor: pointer;" linkurl='<%# Eval("OutUrl") %>'></span>
                                                            </td>
                                                            <td name="TdTemp" align="center">
                                                                <em></em><span>
                                                                    <%# getTempName(Eval("DefaultTemp").ToStr())%>
                                                                </span><i class="eip_delete icon-cross" valueplace="DefaultTemp|<%# Eval("ColumnId") %>"
                                                                    style="cursor: pointer;"></i><span class="eip_edit fontawesome-cog" valueplace="DefaultTemp|<%# Eval("ColumnId") %>"
                                                                        style="cursor: pointer;"></span>
                                                            </td>
                                                            <td name="TdTemp" align="center">
                                                                <em></em><span>
                                                                    <%# getTempName(Eval("ListTemp").ToStr())%>
                                                                </span><i class="eip_delete icon-cross" valueplace="ListTemp|<%# Eval("ColumnId") %>" style="cursor: pointer;"></i><span class="eip_edit fontawesome-cog" valueplace="ListTemp|<%# Eval("ColumnId") %>" style="cursor: pointer;"></span>
                                                            </td>
                                                            <td name="TdTemp" align="center">
                                                                <em></em><span>
                                                                    <%# getTempName(Eval("ContentTemp").ToStr())%>
                                                                </span><i class="eip_delete icon-cross" valueplace="ContentTemp|<%# Eval("ColumnId") %>"
                                                                    style="cursor: pointer;"></i><span class="eip_edit fontawesome-cog" valueplace="ContentTemp|<%# Eval("ColumnId") %>"
                                                                        style="cursor: pointer;"></span>
                                                            </td>
                                                            <td class="tdMode" align="center" url="<%# Eval("OutUrl") %>">
                                                                <select class="tempMode span<%# Eval("ColumnId") %>" name="sel-<%# Eval("ColumnId") %>" columnpid="<%# Eval("ParentId") %>" columnid="<%# Eval("ColumnId") %>" ckdvalue="<%# Eval("CreateMode") %>">
                                                                    <option value="0"><%=CreateModeNull%></option>
                                                                    <option value="1"><%=CreateModeHtml%></option>
                                                                    <option value="2"><%=CreateModelAspx%></option>
                                                                </select>
                                                            </td>
                                                            <td align="center">
                                                                <label>
                                                                    <input type="checkbox" name="ckbIsCategory-<%# Eval("ColumnId") %>" <%# Eval("IsCategory").ToBoolean()?"checked='checked'": (!Whir.Service.ServiceFactory.ColumnService.IsCategoryParent(Eval("ColumnId").ToInt())?"disabled='disabled'":"") %> /></label>
                                                            </td>
                                                            <td align="center">
                                                                <label>
                                                                    <input type="checkbox" name="ckb-<%# Eval("ColumnId") %>" <%# Eval("IsAutoCleanupStaticFiles").ToBoolean()?"checked='checked'": (Eval("CreateMode").ToInt()!=1?"disabled='disabled'":"") %> /></label>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <br />
                        <asp:LinkButton ID="lbnSubmit" runat="server" CssClass="btn btn-info" OnClick="lbUpdate_Click"
                            OnClientClick="return GetCreateMode()" Style="float: right; margin-right: 20px;">
                           <%="保存".ToLang()%> 
                        </asp:LinkButton>
                    </div>
                    <script type="text/javascript">
                        $(".eip_delete").hide();
                        $(".model_delete").hide();
                        //加入模版选择下拉列表
                        $("span[class='eip_edit fontawesome-cog']").click(function () {
                            $(this).prevAll("em").html($("#DivDropTemplate").html());
                            $(this).prevAll("span").hide();
                            $(this).prevAll("i").show();
                            GetValue();
                            $(this).hide();
                        });

                        $(".eip_delete.icon-cross").click(function () {
                            $(this).prevAll("em").html('');
                            $(this).prevAll("span").show();
                            $(this).next("span").show();
                            GetValue();
                            $(this).hide();
                        });
                        //加入功能模块选项下拉列表
                        $("span[class='model_edit fontawesome-cog']").click(function () {
                            if ($(this).attr("linkurl") != "") {
                                TipMessage('<%="外部链接不能选择改变功能模块".ToLang() %>');
                            } else {
                                if ($(this).attr("res") == 'subsite') {//自定义子站
                                    $(this).prevAll("em").html($("#DivSubsiteColumn").html());
                                } else {
                                    $(this).prevAll("em").html($("#DivDropColumn").html());
                                }
                                var modelId = $(this).prevAll(".modelname").attr("modelid");
                                $(this).prevAll("em").find("select").val(modelId);
                                $(this).prevAll("span").hide();
                                $(this).prevAll("i").show();
                                GetColumnValue();
                                $(this).hide();
                            }
                        });
                        //取消功能模块选择
                        $(".model_delete.icon-cross").click(function () {
                            $(this).prevAll("em").html('');
                            $(this).prevAll("span").show();
                            $(this).next("span").show();
                            GetColumnValue();
                            $(this).hide();
                        });

                        function GetValue() {
                            var str = "";
                            $("select[name='ctl00$ContentPlaceHolder1$dropTemplate']").each(function () {
                                if (str == '') {
                                    str = $(this).parent().nextAll("span[valueplace]").attr("valueplace");
                                }
                                else {
                                    str = str + "," + $(this).parent().nextAll("span[valueplace]").attr("valueplace");
                                }
                            });
                            $("#<%= ColumnFieldValue.ClientID %>").val(str);
                        }

                        function GetColumnValue() {
                            var str = "";
                            $("select[whirType='module']").each(function () {
                                if (str == '') {
                                    str = $(this).parent().nextAll("span[valueplace]").attr("valueplace");
                                }
                                else {
                                    str = str + "," + $(this).parent().nextAll("span[valueplace]").attr("valueplace");
                                }
                            });
                            $("#<%= ModelFieldValue.ClientID %>").val(str);
                        }

                        //生成模式、是否编辑自动清除静态文件
                        function GetCreateMode() {
                            var str = "";
                            $("select[name^='sel-'].tempMode").each(function () {
                                var columnid = $(this).attr("columnid");
                                var createid = $(this).val();
                                var isCategory = $(this).parents(".tdMode:eq(0)").next().find("input[name^='ckbIsCategory-']:checked").length;
                                var isClearHtml = $(this).parents(".tdMode:eq(0)").next().next().find("input[name^='ckb-']:checked").length;
                                str += columnid + "|" + createid + "|" + isClearHtml + "|" + isCategory + ",";
                            });
                            $("#<%=hdCreateMode.ClientID %>").val(str);
                            return true;
                        }
                    </script>
                    <asp:HiddenField ID="ColumnFieldValue" runat="server" />
                    <asp:HiddenField ID="ModelFieldValue" runat="server" />
                    <asp:HiddenField ID="hfStrMap" runat="server" />


                    <asp:HiddenField ID="hdCreateMode" runat="server" />
                    <div style="display: none" id="DivDropTemplate">
                        <asp:DropDownList ID="dropTemplate" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div style="display: none" id="DivSubsiteColumn">
                        <asp:DropDownList ID="ddpSubsite" runat="server" whirType="module">
                        </asp:DropDownList>
                    </div>
                    <div style="display: none" id="DivDropColumn">
                        <asp:DropDownList ID="dropColumn" runat="server" whirType="module">
                        </asp:DropDownList>
                    </div>

                </div>
            </div>
        </div>
    </form>
</asp:Content>
