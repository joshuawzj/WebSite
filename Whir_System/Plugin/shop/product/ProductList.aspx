<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="productlist.aspx.cs" Inherits="whir_system_Plugin_shop_product_productlist" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Register Src="~/whir_system/Plugin/shop/common/HeadContainer.ascx" TagName="HeadContainer"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
    <script src="<%=SysPath %>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li class="active"><a href="productlist.aspx" data-toggle="tab" aria-expanded="true"><%="商品管理".ToLang()%></a></li>
                    <li><a href="Recycle.aspx">
                        <span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%>
                    </a>
                    </li>
                </ul>
                <br />
                <div class="actions btn-group pull-left">
                    <a class="btn btn-white" href="product_edit.aspx">
                        <i class="glyphicon glyphicon-plus"></i>&nbsp;<%= "添加商品".ToLang() %>
                    </a>
                    <a class="btn btn-white" runat="server" href="javascript:;" onclick="exportData();">
                        <i class="icon icon-export"></i>&nbsp;<%= "导出数据".ToLang() %>
                    </a>
                </div>

                <table id="Common_Table" class="productlist_wap">
                </table>
                <div class="space10"></div>
                <input type="hidden" id="hidChoose" />
                <div class="operate_foot">
                    <a href="javascript:void(0);" id="a_dels" class="btn text-danger border-danger"><%="批量删除".ToLang()%></a>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document)
            .ready(function () {
                whir.checkbox.checkboxOnload("lbDel", "hidChoose", "cb_Top", "cb_Position");
            });
        var $table = $('#Common_Table'), _that;
        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx?_action=GetList",
                dataType: "json",
                filterControl: true,
                showColumns: false,
                showSelectColumn: false,
                showRefresh: true,  //刷新按钮
                filterShowClear: true,//清空搜索按钮
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                pageSize: 10,
                onLoadSuccess: function () {
                    //设置样式 后期需修改
                    SetTableStyleEvent();
                },
                onLoadError: function (data, mes) {
                    if (mes && mes.responseText) {
                        whir.toastr.warning(mes.responseText);
                    } else {
                        whir.toastr.error("<%="获取数据失败！".ToLang()%>");
                    }
                },
                onColumnSwitch: function () {
                    //SetTableStyleEvent();
                },
                SelectColumnEvent: function () {
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: '', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: 'Id', field: 'ProID', width: 50, align: 'center', valign: 'middle', formatter: function (value, row, index) { return ForSort(value, row, index); } },
                    { title: '<%="商品名称".ToLang()%>', field: 'ProName', width: 260, align: 'center', filterControl: '1', valign: 'middle' },
                    { title: '<%="商品分类".ToLang()%>', field: 'CategoryName', align: 'center', filterControl: '4', filterData: unescape('<%=CategoryNameJson%>'), valign: 'middle' },
                    { title: '<%="价格".ToLang()%>', field: 'CostAmount', align: 'center', filterControl: '6', valign: 'middle' },
                    { title: '<%="可购状态".ToLang()%>', field: 'IsAllowBuy', align: 'center', filterControl: '9', filterData:'json:{"0": "<%="否".ToLang()%>","1": "<%="是".ToLang()%>"}', valign: 'middle', formatter: function (value, row, index) { return GetPurchaseStatus(value, row, index); } },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 300, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } }
                ]

            });
            whir.loading.remove();
        }
        function SetTableStyleEvent() {
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
        }

        $(function () {
            //拖动排序
            $("#Common_Table").sortable(
                {
                    items: "tr[data-index]",
                    appendTo: 'parent',
                    handle: '.dragCursor',
                    stop: function (event, ui) {
                        saveSort(event, ui);
                    },
                    axis: 'y'
                });
        });

        //异步保存排序
        function saveSort(event, ui) {

            var ids = "";
            $(".dragCursor").each(function () {
                ids += $(this).html() + ",";
            });

            whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx",
                {
                    data: {
                        _action: "SortDrag",
                        Ids: ids,
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success("<%="排序成功".ToLang()%>");
                              reload();
                          } else {
                              whir.toastr.error(response.Message);
                          }
                      }
                });

        }
        //批量排序
        $("#a_Sort").click(function () {
            var strSort = getSort();
            if (strSort == "") {
                whir.toastr.warning("<%="请选择".ToLang() %>");
            } else {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx",
                    {
                        data: {
                            _action: "BatchSort",
                            strSort: strSort
                        },
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                reload();
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
            }
        })

        //实现拖拽排序
        function ForSort(value, row, index) {
            return ' <div  class="dragCursor" sort="' + row.Sort + '"  title="<%="点击可以拖拽排序".ToLang()%>">' + row.ProID + '</div> ';
        }

        initTable();
        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" name="btSelectItem" value="' + row.ProID + '" />';
        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            html += '<a name="lbEdite" class="btn btn-white"  href="<%=SysPath%>Plugin/shop/product/product_edit.aspx?proid=' + row.ProID + '"><%="编辑".ToLang() %></a>';
            html += '<a name="lbDel" class="btn text-danger border-normal"  onclick="Deleted(' + row.ProID + ')" href="javascript:;"><%="删除".ToLang() %></a>';
            html += '</div>';
            return html;
        }
        //获取排序操作HTML
        function GetCheckSort(value, row, index) {
            var html = '<div class="btn-group">';
            html = "<input type=\"text\" class='form-control' style='width:145px;' value='" + row.Sort + "' proid='" + row.ProID + "'/>";
            html += '</div>';
            return html;
        }
        //获取购买状态
        function GetPurchaseStatus(value, row, index) {
            var str = "";
            if (!row.IsAllowBuy) {
                str = "<%="否".ToLang()%>";
            }
            else {
                str = "<%="是".ToLang()%>";
            }
            return str;
        }
        function reload() {
            $table.bootstrapTable('refresh');
        }


        //批量删除
        $("#a_dels").click(function () {
            var ids = getIds();
            if (ids == "") {
                whir.toastr.warning("<%="请选择".ToLang() %>");
            } else {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx",
                    {
                        data: {
                            _action: "BatchDel",
                            ids: ids,
                            isdel: ""
                        },
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                whir.dialog.remove();
                                reload();

                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
            }
        })
        //单个删除
        function Deleted(id) {
            var dialog = whir.dialog.confirm("<%="确认删除吗？".ToLang()%>", function () {
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx", {
                    data: {
                        _action: "Del",
                        id: id,
                        isdel: ""
                    },
                    success: function (response) {
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                            whir.dialog.remove();
                            reload();
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    }
                });
            });
        }

        function exportData() {
            var filter = JSON.stringify(_that.filterColumnsPartial, null);
            var url = "<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx?_action=ExportClick&filter=" + filter;

            $.get(url, function (data) {
                if (data !== "")
                    window.location.href = data;
                else
                    whir.toastr.error("<%="导出数据失败".ToLang()%>");
            });
        }

        function getSort() {
            var result = '';
            $("#Common_Table input[name='btSelectItem']").each(function () {
                if ($(this).is(":checked")) {
                    var columnID = $(this).val();
                    var sort = $(this).parent().next().find("input").val();
                    result += columnID + "|" + sort + ",";
                }
            })
            if (result != '') {
                result = result.substring(0, result.length - 1);
            }
            return result;
        }
        function getIds() {
            var ids = "";
            $("#Common_Table input[name='btSelectItem']").each(function () {
                if ($(this).is(":checked")) {
                    ids += $(this).val() + ",";
                }
            })
            if (ids != "") {
                ids = ids.substring(0, ids.length - 1);
            }
            return ids;
        }
    </script>
</asp:Content>
