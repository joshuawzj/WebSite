<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="paymentlist.aspx.cs" Inherits="whir_system_Plugin_shop_order_payment_paymentlist" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div class="content-wrap">
            <div class="space15"></div>
            <div class="panel">
                <div class="panel-heading">
                    <%="支付方式管理".ToLang()%>
                </div>
                <div class="panel-body">
                    <table id="Common_Table" class="payment_wap"></table>
                    <div class="space10"></div>
                    <input type="hidden" id="hidChoose" />
                    <div class="operate_foot">
                        <div class="btn-group">
                            <a href="javascript:void(0);" id="a_Start" class="btn btn-white"><%="批量启用".ToLang()%></a>
                            <a href="javascript:void(0);" id="a_Stop" class="btn text-danger border-danger"><%="批量禁用".ToLang()%></a>
                        </div>
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
                url: "<%=SysPath%>Handler/Plugin/shop/PaymentForm.aspx?_action=GetList",
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
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'id', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: '<%="支付方式名称".ToLang()%>', field: 'name', width: 260, align: 'left', valign: 'middle' },
                    { title: '<%="是否启用".ToLang()%>', field: 'isopen', align: 'left', valign: 'middle', formatter: function (value, row, index) { return GetIsOpen(value, row, index); } },
                    { title: '<%="描述".ToLang()%>', field: 'introduce', align: 'left', valign: 'middle' },
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


        initTable();
        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" name="btSelectItem" value="' + row.id + '" />';
        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            html += '<a name="lbEdite" class="btn btn-white"  href="<%=SysPath%>Plugin/shop/order/payment/payment_edit.aspx?ID=' + row.id + '"><%="编辑".ToLang() %></a>';
            if (row.isopen == 1) {
                html += '<a name="lbEnabled" class="btn text-danger border-normal" onclick="Disabled(' + row.id + ')" href="javascript:;"><%="禁用".ToLang()%></a>';
            }
            else {
                html += '<a name="lbEnabled" class="btn btn-white" onclick="Enabled(' + row.id + ')" href="javascript:;"><%="启用".ToLang()%></a>';
            }
            html += '</div>';
            return html;
        }
        function reload() {
            $table.bootstrapTable('refresh');
        }
        //是否启用
        function GetIsOpen(value, row, index) {
            var html = "";
            if (row.isopen == 1) {
                html = '<span class="fontawesome-ok text-success"></span>';
            }
            else {
                html = '<span class="fontawesome-remove text-danger"></span>';
            }
            return html;
        }
        //启用
        function Enabled(id) {
            whir.dialog.remove();
            whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/PaymentForm.aspx",
              {
                  data: {
                      _action: "PaymentOperating",
                      id: id,
                      operName: "start"
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
        //禁用
        function Disabled(id) {
            whir.dialog.remove();
            whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/PaymentForm.aspx",
               {
                   data: {
                       _action: "PaymentOperating",
                       id: id,
                       operName: "stop"
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
        //批量启用
        $("#a_Start").click(function () {
            var ids = getIds();
            if (ids == "") {
                whir.toastr.warning("<%="请选择".ToLang() %>");
            } else {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/PaymentForm.aspx",
                    {
                        data: {
                            _action: "PaymentOperatingList",
                            ids: ids,
                            operName: "start"
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
        //批量禁用
        $("#a_Stop").click(function () {
            var ids = getIds();
            if (ids == "") {
                whir.toastr.warning("<%="请选择".ToLang() %>");
            } else {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/PaymentForm.aspx",
                    {
                        data: {
                            _action: "PaymentOperatingList",
                            ids: ids,
                            operName: "stop"
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
    </script>
</asp:Content>
