<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    CodeFile="profieldlist.aspx.cs" Inherits="whir_system_Plugin_shop_field_profieldlist" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Register Src="~/whir_system/Plugin/shop/common/HeadContainer.ascx" TagName="HeadContainer"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div class="content-wrap">
            <div class="space15">
            </div>
            <div class="panel">
                <div class="panel-heading"><%="自定义属性".ToLang()%></div>
                <div class="panel-body">
                    <div class="actions btn-group">
                        <a class="btn btn-white" href="profield_edit.aspx" aria-expanded="true"><%="添加自定义属性".ToLang()%></a>
                    </div>
                    <table id="Common_Table"></table>
                    <div class="space10"></div>
                    <input type="hidden" id="hidChoose" />
                    <div class="operate_foot">
                      
                        <a href="javascript:void(0);" id="a_Sort" class="btn btn-white"><%="排序".ToLang()%></a>
                        <a href="javascript:void(0);" id="a_dels" class="btn text-danger border-danger"><%="批量删除".ToLang()%></a>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        $(document)
      .ready(function () {
          whir.checkbox.checkboxOnload("lbDel", "hidChoose", "cb_Top", "cb_Position");
      });
        var $table = $('#Common_Table'), _that;
        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Plugin/shop/FieldForm.aspx?_action=GetList",
                dataType: "json",
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                pageSize: 10,
                onLoadSuccess: function () {
                    //设置样式 后期需修改
                    SetTableStyleEvent();
                },
                onLoadError: function (data,mes) {
                     if(mes && mes.responseText){
                       whir.toastr.warning(mes.responseText);
                     }else{
                         whir.toastr.error("<%="获取数据失败！".ToLang()%>");
                    }
                },
                onColumnSwitch: function () {
                    //SetTableStyleEvent();
                },
                SelectColumnEvent: function () {
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'ConsultID', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: '<%="排序".ToLang()%>', field: 'Sort', width: 260, align: 'left', valign: 'middle', formatter: function (value, row, index) { return GetSort(value, row, index); } },
                    { title: '<%="属性名称".ToLang()%>', field: 'FieldAlias', width: 260, align: 'left', valign: 'middle' },
                    { title: '<%="展示形式".ToLang()%>', field: 'ShowType', align: 'center', valign: 'middle', formatter: function (value, row, index) { return getShowTypeStr(value, row, index); } },
                    { title: '<%="默认值".ToLang()%>', field: 'DefaultValue', align: 'center', valign: 'middle' },
                    { title: '<%="是否启用".ToLang()%>', field: 'IsUsing', align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetIsUsing(value, row, index); } },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 300, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } }
                ]

            });
            whir.loading.remove();
        }

        initTable();

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

        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" rel="' + row.Sort + '" name="btSelectItem" value="' + row.FieldID + '" />';
        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            html += '<a class="btn btn-white" href="profield_edit.aspx?fieldid=' + row.FieldID + '"><%="编辑".ToLang()%></a>';
            html += '<a name="lbDel" class="btn text-danger border-normal"  onclick="Deleted(' + row.FieldID + ')" href="javascript:;"><%="删除".ToLang() %></a>';
            html += '</div>';
            return html;
        }
        function GetSort(value, row, index) {
            return '<input type="text"  class="form-control" data-id="' + row.FieldID + '" value="' + row.Sort + '"/>';
        }
        function GetIsUsing(value, row, index) {
            var str = '';
            if (row.IsUsing) {
                str = '<%="是".ToLang()%>';
            }
            else {
                str = '<%="否".ToLang()%>';
            }
            return str;
        }
        //展现形式
        function getShowTypeStr(value, row, index) {
            switch (row.ShowType) {
                case 1:
                    return "<%="单行文本".ToLang()%>";
                    break;
                case 2:
                    return "<%="单选".ToLang()%>";
                    break;
                case 3:
                    return "<%="多选".ToLang()%>";
                    break;
                case 4:
                    return "<%="多行文本".ToLang()%>";
                    break;
                case 5:
                    return "<%="下拉框".ToLang()%>";
                    break;
                case 6:
                    return "<%="HTML".ToLang()%>";
                    break;
                case 7:
                    return "<%="图片".ToLang()%>";
                    break;
                case 8:
                    return "<%="文件".ToLang()%>";
                    break;
                default:
                    return "未知类型";
                    break;
            }
        }
        function reload() {
            $table.bootstrapTable('refresh');
        }
        //删除事件
        function Deleted(fieldID) {
            var dialog = whir.dialog.confirm("<%="确认删除吗？".ToLang()%>", function () {
                whir.loading.show();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/FieldForm.aspx", {
                    data: {
                        _action: "Del",
                        FieldID: fieldID
                    },
                    success: function (response) {
                        if (response.Status == true) {
                            whir.dialog.remove();
                            whir.toastr.success(response.Message);
                            reload();
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    }
                });
            });
        }
        //批量排序
        $("#a_Sort").click(function () {
            var ids = getSort();
            if (ids == "") {
                whir.toastr.warning("<%="请选择".ToLang() %>");
              } else {
                  whir.dialog.remove();
                  whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/FieldForm.aspx",
                    {
                        data: {
                            _action: "BatchSort",
                            ids: ids
                        },
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                reload();
                                initTable();//重新加载数据
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
            }
        })
        //批量删除
        $("#a_dels").click(function () {
            var ids = getIds();
            if (ids == "") {
                whir.toastr.warning("<%="请选择".ToLang() %>");
            } else {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/FieldForm.aspx",
                    {
                        data: {
                            _action: "BatchDel",
                            ids: ids
                        },
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                reload();
                                initTable();//重新加载数据
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
              }
        })
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
        function getSort() {
            var result = "";
            $("#Common_Table input[name='btSelectItem']").each(function () {
                if ($(this).is(":checked")) {
                    var id = $(this).val();
                    var sort = $("input[data-id='" + id + "']").val();
                    result += id + "|" + sort + ",";
                }
            })
            if (result != '') {
                result = result.substring(0, result.length - 1);
            }
            return result;
        }
    </script>
</asp:Content>
