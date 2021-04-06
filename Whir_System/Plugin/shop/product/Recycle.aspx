<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Recycle.aspx.cs" Inherits="whir_system_Plugin_shop_product_Recycle" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Register Src="~/whir_system/Plugin/shop/common/HeadContainer.ascx" TagName="HeadContainer"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
               <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li ><a href="productlist.aspx" ><%="商品管理".ToLang()%></a></li>
                    <li class="active"><a href="Recycle.aspx"data-toggle="tab" aria-expanded="true">
                        <span class="fontawesome-trash"></span>&nbsp;<%="回收站".ToLang()%>
                    </a>
                   </li>
                </ul>
                <br />
                 
                <table id="Common_Table" class="productlist_wap">
                </table>
                <div class="space10"></div>
                <input type="hidden" id="hidChoose" />
                <div class="operate_foot">
                    <div class="btn-group">
                        <a href="javascript:void(0);" id="a_reduction" class="btn btn-white"><%="批量还原".ToLang()%></a>
                        <a href="javascript:void(0);" id="a_dels" class="btn text-danger border-danger"><%="批量删除".ToLang()%></a>
                    </div>
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
                url: "<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx?_action=GetList&isdel=1",
                dataType: "json",
                filterControl: true,
                showExport: false,
                showSelectColumn: false,
                filterShowClear: false,//清空搜索按钮
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                onLoadSuccess: function () {
                    //设置样式 后期需修改
                    SetTableStyleEvent();
                },
                onColumnSwitch: function () {
                    //SetTableStyleEvent();
                },
                SelectColumnEvent: function () {
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'CourierID', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: 'Id', field: 'ProID', width: 50, align: 'center', valign: 'middle', formatter: function (value, row, index) { return ForSort(value, row, index); } },
                    { title: '<%="商品名称".ToLang()%>', field: 'ProName', width: 260, align: 'center', filterControl: '1', valign: 'middle' },
                    { title: '<%="商品分类".ToLang()%>', field: 'CategoryName', align: 'center', filterControl: '4', valign: 'middle' },
                    { title: '<%="价格".ToLang()%>', field: 'CostAmount', align: 'center', filterControl: '6', valign: 'middle' },
                    { title: '<%="可购状态".ToLang()%>', field: 'IsAllowBuy', align: 'center', filterControl: '9', valign: 'middle', formatter: function (value, row, index) { return GetPurchaseStatus(value, row, index); } },
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

        //实现拖拽排序
        function ForSort(value, row, index) {
            return ' <div sort="' + row.Sort + '" title="<%="点击可以拖拽排序".ToLang()%>">' + row.ProID + '</div> ';
        }

        initTable();
        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" name="btSelectItem" value="' + row.ProID + '" />';
        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            html += '<a name="lbDel" class="btn text-danger border-normal"  onclick="Deleted(' + row.ProID + ')" href="javascript:;"><%="删除".ToLang() %></a>';
            html += '<a name="lbReduction" class="btn btn-white"   onclick="reduction(' + row.ProID + ')" href="javascript:;"><%="还原".ToLang() %></a>';
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
                whir.dialog.confirm("<%="确认删除吗？".ToLang()%>", function () {
                    whir.dialog.remove();
                    whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx",
                        {
                            data: {
                                _action: "BatchDel",
                                ids: ids,
                                isdel: "1"
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
                });
            }
        })
        //单个删除
        function Deleted(id) {
            var dialog = whir.dialog.confirm("<%="确认删除吗？".ToLang()%>", function () {
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx", {
                    data: {
                        _action: "Del",
                        id: id,
                        isdel: "1"
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
        //批量还原
        $("#a_reduction").click(function () {
            var ids = getIds();
            if (ids == "") {
                whir.toastr.warning("<%="请选择".ToLang() %>");
            } else {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx",
                    {
                        data: {
                            _action: "BatchReduction",
                            ids: ids
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

        function reduction(id) {
            whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ProductListForm.aspx",
                  {
                      data: {
                          _action: "BatchReduction",
                          ids: id
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
