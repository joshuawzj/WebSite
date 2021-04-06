<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="workflowlist.aspx.cs" Inherits="whir_system_ModuleMark_common_workflowlist" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath%>Res/assets/js/bootstrap-table/src/extensions/filter-control/bootstrap-table-filter-control.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrap">
        <div class="space15"></div>

        <div class="panel">
            <div class="panel-heading"><%="待审核信息".ToLang()%></div>
            <div class="panel-body">
                <table id="Common_Table"></table>
                <div class="space10"></div>
                <input type="hidden" id="hidChoose" />

                <div class="operate_foot">
                    <%if (IsCurrentRoleMenuRes("377"))
                        { %>
                    <a href="javascript:void(0);" id="a_Del" class="btn text-danger border-danger"><%="批量删除".ToLang()%></a>
                    <%} %>
                    <div class="btn-group">
                        <%if (IsCurrentRoleMenuRes("373"))
                            { %>
                        <a href="javascript:void(0);" id="a_Audit" class="btn btn-white"><%="批量审核".ToLang()%></a>
                        <%} %>
                        <%if (IsCurrentRoleMenuRes("374"))
                            { %>
                        <a href="javascript:void(0);" id="a_Back" class="btn btn-white"><%="批量退审".ToLang()%></a>
                        <%} %>
                    </div>
                </div>

                <input type="hidden" id="hidSelected" name="hidSelected" />
                <input type="hidden" id="hidReturnReason" name="hidReturnReason" />
            </div>
        </div>
    </div>
    <script type="text/javascript">

        function clearSearchBar() {
            $("#Title").val("");
            $("#Column").val(0);
            $("#txtstartimes").val("");
            $("#txtendtimes").val("");

        }

        var $table = $('#Common_Table'), _that;

        function initTable() {

            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Common/UnauditedList.aspx?_action=GetList&title=&columnid=&startime=&endtime=",
                dataType: "json",
                <%if (IsCurrentRoleMenuRes("372"))
        {%>
                filterControl: true,
                <%}
        else
        { %>
                filterControl: false,
                <%}%>
                showExport: false,
                showSelectColumn: false,
                filterShowClear: false,//清空搜索按钮
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                pageSize: 10,
                onLoadSuccess: function () {
                    loadSuccess();
                },
                onLoadError: function (data, mes) {
                    if (mes && mes.responseText) {
                        whir.toastr.warning(mes.responseText);
                    } else {
                        whir.toastr.error("获取数据失败！");
                    }
                },
                <%=Columns %>
            });
        }
        function loadSuccess() {
            //设置样式 后期需修改
            whir.checkbox.destroy();
            whir.skin.radio();
            whir.skin.checkbox();
            //绑定全选按钮事件
            $("#btSelectAll").next().click(function () {
                if ($("#btSelectAll").is(':checked')) {
                    whir.checkbox.selectAll('lbDel', 'hidChoose', 'btSelectItem');
                } else {
                    whir.checkbox.cancelSelectAll('lbDel', 'hidChoose', 'btSelectItem');
                }
            });

            $("input[name=btSelectItem]").each(function () {
                $(this).next().click(function () {
                    $("#hidChoose").val(whir.checkbox.getSelect('btSelectItem'));
                });
            });
            var columnStr = "";
            var columnList = [];
            $(".bootstrap-table-filter-control-ColumnName option").each(function () {
                columnList.push($(this).attr("value"));
            });

            var columnListCtl = $(".bootstrap-table-filter-control-ColumnName");//.empty();

            <%foreach (var column in ColumnsList)
        {%>
            if (columnList.indexOf('<%=column.ColumnName%>') == -1)
                columnListCtl.append("<option value='<%=column.ColumnName%>'><%=column.ColumnName%></option>");//添加option
            <%}%>

        }

        initTable();

        function search() {
            $table.bootstrapTable('refresh');
        }

        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" name="btSelectItem" value="' + row.Id + '" />';
        }

        function GetOperation(value, row, index) {
            var e = '<div class="btn-group-vertical">';
            if ("<%=IsShowView.ToStr().ToLower() %>" == "true") {
                e += ' <a class="btn btn-white" href="javascript:preview(' + row.typeid + ', ' + row.Id + ')" ><%="预览".ToLang()%></a> ';
            }
            if ("<%=IsShowEdit.ToStr().ToLower() %>" == "true") {
                e += '<a name="aEdit" class="btn btn-white" href="content_edit.aspx?columnid=' + row.typeid + '&subjectid=' + row.subjectid + '&itemid=' + row.Id + '&BackPageUrl=' + location.href + '"><%="编辑".ToLang()%></a> ';
            }
            if ("<%=IsShowDelete.ToStr().ToLower() %>" == "true") {
                e += ' <a class="btn text-danger border-normal"   onclick="Del(' + row.Id + ',' + row.typeid + ',' + row.subjectid + ')"  href="javascript:;" ><%="删除".ToLang()%></a> ';
            }
            e += '</div>';
            return e;
        }

        //字符串转日期格式，strDate要转为日期格式的字符串
        function getDate(strDate) {
            var date = eval('new Date(' + strDate.replace(/\d+(?=-[^-]+$)/,
                function (a) { return parseInt(a, 10) - 1; }).match(/\d+/g) + ')');
            return date;
        }

        function Del(id, typeid, subjectId) {
            if (!id || !typeid) {
                whir.toastr.warning("<%="参数错误".ToLang() %>");
            } else {
                whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                    whir.ajax.post("<%=SysPath%>Handler/Common/UnauditedList.aspx?_action=SingleDelete",
                        {
                            data: {
                                key: id + "|" + typeid,
                                SubjectID: subjectId
                            },
                            success: function (response) {
                                whir.loading.remove();
                                if (response.Status == true) {
                                    whir.toastr.success(response.Message);
                                    whir.dialog.remove();
                                    search();
                                } else {
                                    whir.toastr.error(response.Message);
                                }
                            }
                        });
                    return false;
                });
            }
        }

        $("#a_Audit").click(function () {
            var keys = getKeys();
            if (keys.length < 1) {
                whir.toastr.warning("<%="请选择".ToLang() %>");
            } else {
                whir.dialog.confirm("<%="确定通过审核？".ToLang() %>", function () {
                    whir.ajax.post("<%=SysPath%>Handler/Common/UnauditedList.aspx?_action=BatchEvent",
                            {
                                data: {
                                    cmd: "passflow",
                                    cbPosition: keys,
                                    reason: ""
                                },
                                success: function (response) {
                                    whir.loading.remove();
                                    if (response.Status == true) {
                                        whir.toastr.success(response.Message);
                                        search();
                                        whir.dialog.remove();
                                    } else {
                                        whir.toastr.error(response.Message);
                                        whir.dialog.remove();
                                    }
                                }
                            });
                    });
                    return false;
                }
        });

            $("#a_Del").click(function () {
                var keys = getKeys();
                if (keys.length < 1) {
                    whir.toastr.warning("<%="请选择要删除的数据行！".ToLang() %>");
            } else {
                whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                    whir.ajax.post("<%=SysPath%>Handler/Common/UnauditedList.aspx?_action=BatchEvent",
                                {
                                    data: {
                                        cmd: "del",
                                        cbPosition: keys,
                                        reason: ""
                                    },
                                    success: function (response) {
                                        whir.loading.remove();
                                        if (response.Status == true) {
                                            whir.toastr.success(response.Message);
                                            whir.dialog.remove();
                                            search();
                                        } else {
                                            whir.toastr.error(response.Message);
                                            whir.dialog.remove();
                                        }
                                    }
                                });
                            return false;
                        });
                    }
        });

                $("#a_Back").click(function () {
                    openReturnReason();
                });

                ///获取选择的行的主键(多个)
                function getKeys() {
                    var keys = "", ids = "," + $("#hidChoose").val() + ",";
                    var TrData = $table.bootstrapTable('getData');
                    $(TrData).each(function (i, item) {
                        var itemId = "," + $(item)[0].Id + ",";
                        if (ids.indexOf(itemId) >= 0) {
                            keys += $(item)[0].Id + "|" + $(item)[0].typeid + "|" + $(item)[0].workflow + "|" + $(item)[0].state + "|" + $(item)[0].subjectid + ",";
                        }
                    });

                    if (keys.length > 0) {
                        return keys.substring(0, keys.length - 1);
                    }
                    return "";
                }


                //查看页
                function preview(typeid, id) {
                    var TrData = $table.bootstrapTable('getData');
                    $(TrData).each(function (i, item) {
                        if (item.Id == id && item.typeid == typeid) {
                            whir.dialog.frame('<%="预览".ToLang()%>', "<%=SysPath%>ModuleMark/common/workflowdetails.aspx?columnid=" + typeid + "&itemid=" + id + "&workflow=" + item.workflow + "&state=" + item.state + "&time=" + new Date().getTime(), null, 1100, 600, false);
                        return;
                    }
                });
            }

            //打开退审页面
            function openReturnReason() {
                var keys = getKeys();
                if (keys.length < 1) {
                    whir.toastr.warning("<%="请选择".ToLang() %>");
                   return;
               }

               whir.dialog.frame('<%="退审理由".ToLang()%>', "WorkFlowReasonBatch.aspx?cbPosition=" + keys + "&time=" + new Date().getTime(), null, 500, 300);

            return false;
        }

        //时间格式
        function GetDateTimeFormat(value, row, index, format) {
            if (format == "yyyy-mm-dd") {
                return whir.ajax.fixJsonDate(value, "-");
            }
            return whir.ajax.fixJsonDate(value);;
        }

    </script>

</asp:Content>
