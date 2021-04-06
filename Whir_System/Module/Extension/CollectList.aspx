<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="CollectList.aspx.cs" Inherits="Whir_System_Module_Extension_Collectlist" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function CollectNews(collectid) {
            var opts = {
                title: '<%="开始采集".ToLang()%>',
                content: '',
                ok: function (dialog) {
                },
                cancel: function (dialog) { dialog.close(); },
                okText: '<%="确定".ToLang()%>',
                cancelText: '<%="关闭".ToLang()%>',
                showOk: false,
                showCancel: true,
                iframe: {
                    url: "CollectInfo.aspx?collectid=" + collectid,
                    width: 900,
                    height: 580,
                    scroll: true
                },
                zIndex: 1003
            };
            whir.dialog.show(opts);
        }

        //批量选中
        function selectAction() {
            if (!whir.checkbox.isSelect('cb_Position')) {
                TipMessage('<%="请选择".ToLang()%>');
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
     
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="采集管理".ToLang()%></div>
            <div class="panel-body">
                <div class="actions btn-group pull-left">
                    <%if (IsCurrentRoleMenuRes("353"))
                        { %>
                    <a class="btn btn-white" href="CollectStep1.aspx"><%="添加采集项目".ToLang()%></a>
                    <%} %>
                </div>

                <form class="form-horizontal">
                    <table id="Common_Table"></table>
                    <div class="space15"></div>
                    <input type="hidden" id="hidChoose" />
                    <%if (IsCurrentRoleMenuRes("355"))
                        { %>
                    <button id="sort" class="btn btn-white"><%="排序".ToLang()%></button>
                    <%} %>
                    <%if (IsCurrentRoleMenuRes("352"))
                        { %>
                    <button id="remove" class="btn text-danger border-danger"><%="批量删除".ToLang()%></button>
                    <%} %>
                </form>

            </div>
        </div>
    </div>
    <script type="text/javascript">

        var $table = $('#Common_Table'), _that;

        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Extension/Collect.aspx?_action=GetList",
                dataType: "json",
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                pageSize: 10,
                columns: [
                   { title: '<input type="checkbox"  id="btSelectAll" />', field: 'Checkbox', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    {
                        title: '<%="排序".ToLang()%>', field: 'Sort', align: 'center', valign: 'middle', width: "115px", formatter: function (value, row, index) {
                            return ' <input type="text" class="form-control input-sm apid_sort" apid="' + row.Sort + '" value="' + value + '" /> ';
                        }
                    },
                    { title: '<%="项目名称".ToLang()%>', field: 'ItemName', align: 'center', valign: 'middle' },
                    { title: '<%="采集地址".ToLang()%>', field: 'WebUrl', align: 'center', valign: 'middle' },
                    { title: '<%="入库栏目".ToLang()%>', field: 'ColumnName', align: 'center', valign: 'middle' },
                    {
                        title: '<%="操作".ToLang()%>',
                        field: 'CollectId',
                        align: 'center',
                        valign: 'middle',
                        formatter: function(value, row, index) {
                            var e = '<div class="btn-group">';
                            <%if (IsCurrentRoleMenuRes("354"))
                            { %>
                            e += ' <a class="btn btn-white" href="javascript:;" onclick="CollectNews(\'' +
                                row.CollectId +
                                '\')"><%="开始采集".ToLang()%></a> ';
                            <%} %>
                            <%if (IsCurrentRoleMenuRes("351"))
                            { %>
                            e += '<a name="aEdit" class="btn btn-white" href="CollectStep1.aspx?collectid=' +
                                row.CollectId +
                                '"><%="编辑".ToLang()%></a> ';
                            <%} %>
                            e += '</div>';
                            return e;
                        }
                    }
                ],
                onLoadSuccess: function () {
                    //设置样式 后期需修改
                    whir.checkbox.destroy();
                    whir.skin.radio();
                    whir.skin.checkbox();
                    //whir.checkbox.checkboxOnload('remove', 'hidChoose');
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
                },
                onLoadError: function (data,mes) {
                     if(mes && mes.responseText){
                       whir.toastr.warning(mes.responseText);
                     }else{
                         whir.toastr.error("<%="获取数据失败！".ToLang()%>");
                    }
                }
            });
            }

            initTable();

            //获取多选框 HTML
            function GetCheckbox(value, row, index) {
                return '<input type="checkbox" name="btSelectItem" value="' + row.CollectId + '" />';

            }
    </script>
    <script type="text/javascript">
        $("#sort").click(function() {
            whir.ajax.post("<%=SysPath%>Handler/Extension/Collect.aspx?_action=Sort", {
                    data: {
                        SortIds: collectApid_Sort()
                    },
                    success: function(response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                            $table.bootstrapTable('refresh');
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                }
            );
            return false;
        });

        $("#remove").click(function() {
            if ($("#hidChoose").val() == "") {
                whir.toastr.warning("<%="请选择要删除的采集任务！".ToLang() %>");
            } else {
                whir.dialog.confirm("<%="确认删除吗？".ToLang()%>", function() {
                    whir.ajax.post("<%=SysPath%>Handler/Extension/Collect.aspx?_action=Delete",
                        {
                            data: {
                                ChooseIds: $("#hidChoose").val()
                            },
                            success: function(response) {
                                whir.loading.remove();
                                whir.dialog.remove();
                                if (response.Status == true) {
                                    whir.toastr.success(response.Message);
                                    $table.bootstrapTable('refresh');
                                } else {
                                    whir.toastr.error(response.Message);
                                }
                            }
                        }
                    );
                });
                return false;
            }
        });

        //获取主键Id
        function collectApid_Sort() {
            var apidsort = "";
            $(".apid_sort").each(function (i) {
                if (i == 0) {
                    apidsort += $(this).attr("apid") + "|" + $(this).val();
                } else {
                    apidsort += "," + $(this).attr("apid") + "|" + $(this).val();
                }
            });
            return apidsort;
        }
    </script>
</asp:content>

 