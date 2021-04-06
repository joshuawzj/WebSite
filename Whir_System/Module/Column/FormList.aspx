<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="FormList.aspx.cs" Inherits="Whir_System_Module_Column_FormList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath %>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>
    <script type="text/javascript">
        //选中已有表单
        function getSelected() {
            var selected = "";
            $("#tbdFormList").find("input[type='checkbox']:checked").each(function () {
                var formId = $(this).attr("FormId");
                selected += formId + ",";
            });
            selected = selected.substring(0, selected.length - 1);
            $("#hidSelected").val(selected);
        }

        //获取排序的值, 并赋值给hidSort
        function getSort() {
            var result = "";
            $("#tbdFormList").find("input[type='text']").each(function () {
                var formId = $(this).attr("FormId");
                var sort = $(this).val();
                result += formId + "|" + sort + ",";
            });

            if (result != '') {
                result = result.substring(0, result.length - 1);
                $("#hidSort").val(result);
            } else {
                TipMessage('<%="请选择".ToLang()%>');
                return false;
            }
            return true;
        }

        //全选
        function selectAll(flag) {
            var flg = "check";
            if (!flag) {
                flg = "uncheck";
            }
            $("#cbxAll").iCheck(flg);
            $("#tbdFormList").find("input[type='checkbox']").iCheck(flg);
            getSelected();
        }

        $(function () {
            //表头全选
            $("#cbxAll").next().click(function () {
                var checked = $("#cbxAll").prop('checked');
                selectAll(checked);
            });
            $("input[name$='cbxSelected']").next().click(function () {
                getSelected();
            });


            //拖动排序
            $(".sortTable").sortable(
            {
                items: "tr[sort]",
                appendTo: 'parent',
                handle: '.pointer',
                stop: function (event, ui) {
                    saveSort(event, ui);
                },
                axis: 'y'
            });
        });

        //异步保存排序
        function saveSort(event, ui) {
            var intdata, intdata2;
            intdata = $("#tbdFormList tr").eq(ui.item[0].rowIndex - 1).find("td:first").attr("itemid");
            if (ui.item[0].rowIndex == 1) {
                intdata2 = intdata; //取sort字段的值desc
            } else {
                intdata2 = $("#tbdFormList tr").eq(ui.item[0].rowIndex - 2).find("td:first").attr("itemid");
            }
            var datas = { "formid1": intdata, "formid2": intdata2 };
            $.ajax({
                type: "get",
                url: "<%=SysPath %>ajax/developer/formsort.aspx?time=" + new Date().getMilliseconds(),
                data: datas,
                success: function (msg) {
                    if (msg == "") {
                        TipError('<%="排序失败".ToLang()%>');
                    } else {
                        setTableStyle();
                        whir.toastr.success('<%="排序成功".ToLang()%>');
                    }
                }
            });
        }

        //重置表格样式
        var setTableStyle = function () {
            $(".sortTable tr[sort]").removeClass();
            $(".sortTable tr:odd").addClass("tdBgColor tdColor");
            $(".sortTable tr:even").addClass("tdColor");

        };

        $(function () {
            $(".sortTable tr[sort='true'] td[sortTxt!='true']").dblclick(function () {
                location.href = $(this).parent().find(".aEdit").attr("href");
            });
        });

        //批量选中删除
        function deleteAction() {
            if ($("#hidSelected").val() == '') {
                TipMessage('<%="请选择".ToLang() %>');
                return false;
            }
            return true;
        }

        function reload() {
            window.location.href = window.location.href;
        }
        //打开批量推送页面
        function openPush() {
            if ($("#hidSelected").val() == '') {
                TipMessage('<%="请选择".ToLang() %>');
                return false;
            }
            else {
                whir.dialog.frame('<%="选择栏目推送".ToLang()%>', "<%=SysPath %>Module/Column/FormColumn_Select.aspx?columnid=<%= ColumnId %>&selectedtype=checkbox&callback=copyAction", null, 800, 600);
                return false;
            }

        }

        function copyAction(json) {
            json = eval('(' + json + ')');
            var column = "";
            $(json)
                .each(function (idx, item) {
                    column += item.id + ",";
                });
            if (column.length > 0)
                column = column.substring(0, column.length - 1);
            $("#hidSelectedOpen").val(column);
            whir.ajax.post("<%=SysPath%>Handler/Module/Column/Form.aspx",
                {
                    data: {
                        _action: "PushForm",
                        selectedOpen: $("#hidSelectedOpen").val(),
                        selected: $("#hidSelected").val()
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.dialog.remove();
                            whir.toastr.success(response.Message);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                }
            );
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="ColumnList.aspx" aria-expanded="true"><%="栏目管理".ToLang()%></a></li>
                    <% foreach (var item in ListColumnP)
                       {
                           string cla = "";
                            if (item.ColumnId == ColumnId)
                       {
                           cla = "class=\"active\"";
                    }%>
                    <li <%=cla %>>
                        <a href="Formlist.aspx?columnid=<%=item.ColumnId%>"><%=item.ColumnName%>-表单</a></li>
                    <%  } %>
                </ul>
                <div class="space15"></div>
                <div class="actions btn-group">
                    <a class="btn btn-white" href="Form_Edit.aspx?columnid=<%= ColumnId %>"><%="添加表单".ToLang()%></a> 
                    <a class="btn btn-white" onclick="whir.dialog.frame('<%="批量添加表单".ToLang()%>', '<%=SysPath %>Module/Column/Form_BatchAdd.aspx?columnid=<%=ColumnId%>',null,700,650);" href="javascript:void()"><%="批量添加表单输入项".ToLang()%></a>
                </div>

                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="controller table table-bordered sortTable ui-sortable">
                    <tr>
                        <th>
                            <div class="th-inner"><input type="checkbox" id="cbxAll" name="cb_Top" /></div>
                        </th>
                        <th style="width: 163px;"><%="排序".ToLang()%></th>
                        <th>Id</th>
                        <th><%="数据库列名".ToLang()%></th>
                        <th><%="表单名".ToLang()%></th>
                        <th><%="表单类型".ToLang()%></th>
                        <th><%="系统字段".ToLang()%></th>
                        <th><%="隐藏状态".ToLang()%></th>
                        <th width="280px"><%="操作".ToLang()%></th>
                    </tr>
                    <tbody id="tbdFormList">
                        <asp:Repeater ID="rptFormList" runat="server">
                            <ItemTemplate>
                                <tr sort="true">
                                    <td sortid='<%# Eval("Sort") %>' itemid='<%# Eval("FormId") %>'>
                                        <input type="checkbox" id="cbxSelected" runat="server" formid='<%# Eval("FormId") %>'
                                            onclick="getSelected();" />
                                    </td>
                                    <td sorttxt="true">
                                        <input type="text" style="width: 133px" id="txtSort" class="form-control form-search-control" value='<%# Eval("Sort") %>'
                                            formid='<%# Eval("FormId") %>'></input>
                                    </td>
                                    <td class="pointer" style="cursor: move;">
                                        <%# Eval("FormId") %>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litFieldName" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        <%# Eval("FieldAlias") %>
                                    </td>
                                    <td>
                                        <asp:Literal ID="litTypeName" runat="server"></asp:Literal>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Literal ID="litIsSystemField" runat="server"></asp:Literal>
                                    </td>
                                    <td style="text-align: center; vertical-align: middle;">
                                        <asp:Literal ID="litIsHidden" runat="server"></asp:Literal>
                                    </td>
                                    <td>
                                        <div class="btn-group">
                                            <a class="btn btn-sm btn-white aEdit" href='form_edit.aspx?columnid=<%= ColumnId %>&formid=<%# Eval("FormId") %>'><%="编辑".ToLang()%></a>
                                            <a class="btn btn-sm btn-white" name="ListShow" action="ListShow" islistshow="<%# Eval("IsListShow") %>" href="javascript:;" formid="<%# Eval("FormId") %>"><%="列表显示".ToLang()%></a>
                                            <a class="btn btn-sm btn-white" name="SearchShow" action="SearchShow" issearchshow="<%# Eval("IsSearchShow") %>" href="javascript:;" formid="<%# Eval("FormId") %>"><%="搜索显示".ToLang()%></a>
                                            <asp:PlaceHolder ID="lbnDelete" runat="server">
                                                <a name="lbnDelete" class="btn btn-sm text-danger border-normal" href="javascript:;" onclick="Delete(<%# Eval("FormId") %>)"><%="删除".ToLang()%></a>
                                            </asp:PlaceHolder>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
                <div class="operate_foot">
                    <input type="hidden" id="hidSelected" />
                    <input type="hidden" id="hidSort" />
                    <input type="hidden" id="hidSelectedOpen" />
                    <div class="btn-group">
                        <a href="javascript:selectAll(true);" class="btn btn-sm btn-white"><%="全选".ToLang()%></a>
                        <a href="javascript:selectAll(false);" class="btn btn-sm btn-white"><%="取消".ToLang()%></a>
                    </div>
                    <div class="btn-group">
                        <a id="lbnSort" class="btn btn-sm btn-white" href="javascript:;"><%="排序".ToLang()%></a>
                        <a href="javascript:Entity();" class="btn btn-sm btn-white"><%="生成栏目实体".ToLang()%></a>
                        <a href="javascript:openPush();" class="btn btn-sm btn-white"><%="表单推送".ToLang()%></a>
                    </div>
                    <a id="lbnDelete" class="btn btn-sm text-danger border-danger" href="javascript:;"><span class="fontawesome-trash"></span><%="批量删除".ToLang()%></a>
                </div>
            </div>
            
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function() {
                //设置编辑样式
                $("#tbdFormList a[name=ListShow]").each(function() {
                    var isListShow = $(this).attr("IsListShow").toLowerCase();
                    $(this).removeClass();
                    if (isListShow === "true") {
                        $(this).html("<%="列表隐藏".ToLang()%>");
                        $(this).addClass("btn btn-sm text-danger border-normal");
                    } else {
                        $(this).html("<%="列表显示".ToLang()%>");
                        $(this).addClass("btn btn-sm btn-white");
                    }
                });
            $("#tbdFormList a[name=SearchShow]").each(function () {
                    var isSearchShow = $(this).attr("IsSearchShow").toLowerCase();
                    $(this).removeClass();
                    if (isSearchShow === "true") {
                        $(this).html("<%="搜索隐藏".ToLang()%>");
                        $(this).addClass("btn btn-sm text-danger border-normal");
                    } else {
                        $(this).html("<%="搜索显示".ToLang()%>");
                        $(this).addClass("btn btn-sm btn-white");
                    }
                });
            });

                 //列表显示/掩藏
        $("#tbdFormList a[name=ListShow],a[name=SearchShow]").click(function() {
            var isshow = $(this).attr("IsListShow") ? $(this).attr("IsListShow").toLowerCase() : $(this).attr("IsSearchShow").toLowerCase();
            var isList = $(this).attr("name") === "ListShow";
            var el = this;
            whir.ajax.post("<%=SysPath%>Handler/Module/Column/Form.aspx", {
                    data: {
                        _action: $(this).attr("Action"),
                        FormId: $(this).attr("formid")
                    },
                    success: function(response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            $(el).removeClass();
                            if (isList) {
                                if (isshow === "false") {
                                    $(el).html("<%="列表隐藏".ToLang()%>");
                                    $(el).addClass("btn btn-sm btn-info");
                                    $(el).attr("IsListShow", "true");
                                } else {
                                    $(el).html("<%="列表显示".ToLang()%>");
                                    $(el).addClass("btn btn-sm btn-primary");
                                    $(el).attr("IsListShow", "false");
                                }
                            } else {
                                if (isshow === "false") {
                                    $(el).html("<%="搜索显示".ToLang()%>");
                                    $(el).addClass("btn btn-sm btn-info");
                                    $(el).attr("IsSearchShow", "true");
                                } else {
                                    $(el).html("<%="搜索隐藏".ToLang()%>");
                                    $(el).addClass("btn btn-sm btn-primary");
                                    $(el).attr("IsSearchShow", "false");
                                }
                            }
                            whir.toastr.success(response.Message);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                }
            );
            return false;
        });

        function Delete(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function() {
                whir.ajax.post("<%=SysPath%>Handler/Module/Column/Form.aspx",
                    {
                        data: {
                            _action: "Delete",
                            FormId: id
                        },
                        success: function(response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message, true, false);
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
                return false;
            });
        }

        $("#lbnDelete").click(function() {
            if (deleteAction()) {
                whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                    function() {
                        whir.ajax.post("<%=SysPath%>Handler/Module/Column/Form.aspx",
                            {
                                data: {
                                    _action: "DelAll",
                                    selected: $("#hidSelected").val()
                                },
                                success: function(response) {
                                    whir.loading.remove();
                                    if (response.Status == true) {
                                        whir.toastr.success(response.Message);
                                        setTimeout(
                                            location.href = document.URL,
                                            1000);
                                    } else {
                                        whir.toastr.error(response.Message);
                                    }
                                }
                            }
                        );
                        return false;
                    });
            }

        });
        $("#lbnSort").click(function() {
            if (getSort()) {
                whir.ajax.post("<%=SysPath%>Handler/Module/Column/Form.aspx",
                    {
                        data: {
                            _action: "Sort",
                            strSort: $("#hidSort").val()
                        },
                        success: function(response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message, true);
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
                return false;
            }

        });

        function Entity() {
            whir.ajax.post("<%=SysPath%>Handler/Module/Column/Form.aspx", {
                    data: {
                        _action: "Entity",
                        ColumnId: '<%=ColumnId %>'
                    },
                    success: function(response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message, true, false);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                }
            );
            return false;
        }
    </script>
</asp:Content>
