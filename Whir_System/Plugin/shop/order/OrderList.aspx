<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="OrderList.aspx.cs" Inherits="whir_system_Plugin_shop_order_OrderList" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Shop.Domain" %>
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
                <div class="panel-body">
                    <ul class="nav nav-tabs">
                        <li class="active"><a href="javascript:void(0)" rel="0" data-toggle="tab" aria-expanded="true"><%="全部订单".ToLang()%></a></li>
                        <li><a href="javascript:void(0)" rel="1" aria-expanded="true"><%="未支付".ToLang()%></a></li>
                        <li><a href="javascript:void(0)" rel="2" aria-expanded="true"><%="已支付".ToLang()%></a></li>
                        <li><a href="javascript:void(0)" rel="3" aria-expanded="true"><%="未完成".ToLang()%></a></li>
                        <li><a href="javascript:void(0)" rel="4" aria-expanded="true"><%="交易完成".ToLang()%></a></li>
                        <li><a href="javascript:void(0)" rel="5" aria-expanded="true"><%="取消".ToLang()%></a></li>
                        <li>
                            <asp:LinkButton ID="lbExport" runat="server" OnClick="lbExport_Click"><%="导出数据".ToLang()%></asp:LinkButton></li>
                    </ul>
                    <br />

                    <table id="Common_Table">
                    </table>
                    <div class="space10"></div>
                    <input type="hidden" id="hidChoose" />
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
                url: "<%=SysPath%>Handler/Plugin/shop/OrderForm.aspx?_action=GetList",
                dataType: "json",
                filterControl: true,
                showExport: false,
                showSelectColumn: false,
                filterShowClear: false,//清空搜索按钮
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
                onClickRow: function (row, $element) {
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: '', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    //{ title: '', field: 'OrderID', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetImage(value, row, index); } },
                    { title: '<%="订单编号".ToLang()%>', field: 'OrderNo', width: 100, align: 'left', filterControl: '1', valign: 'middle' },
                    { title: '<%="交易日期".ToLang()%>', field: 'CreateDate', width: 160, align: 'center', filterControl: '7', valign: 'middle', sortable: true, filterControl: '7', format: 'yyyy-mm-dd hh:ii:ss', formatter: function (value, row, index) { return GetDateTimeFormat(value, row, index, 'yyyy-MM-dd HH:mm:ss'); } },
                    { title: '<%="商品总额".ToLang()%>', field: 'ProductAmount', align: 'center', filterControl: '6', valign: 'middle', formatter: function (value, row, index) { return GetAmountStr(value, row, index); } },
                    { title: '<%="应付金额".ToLang()%>', field: 'PayAmount', align: 'center', filterControl: '6', valign: 'middle', formatter: function (value, row, index) { return GetAmountStr(value, row, index); } },
                    { title: '<%="支付状态".ToLang()%>', field: 'IsPaid', align: 'center', filterControl: '9', filterData:'json:{"0": "<%="未支付".ToLang()%>","1": "<%="已支付".ToLang()%>"}', valign: 'middle', formatter: function (value, row, index) { return GetPaid(value, row, index); } },
                    { title: '<%="支付方式".ToLang()%>', field: 'PaymentName', width: 160, align: 'center', filterControl: '9',filterData: unescape('<%=PaymentListjson%>'), valign: 'middle' },
                    { title: '<%="订单状态".ToLang()%>', field: 'Status', align: 'center', filterControl: '9', filterData:'json:{"-1": "<%="已取消".ToLang()%>","0": "<%="交易完成".ToLang()%>","1": "<%="未完成".ToLang()%>"}', valign: 'middle', formatter: function (value, row, index) { return GetStatus(value, row, index); } },
                    { title: '<%="购买会员".ToLang()%>', field: 'LoginName', align: 'center', filterControl: '1', valign: 'middle' },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 110, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } }
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
            return '<input type="checkbox" name="btSelectItem" value="' + row.OrderID + '" />';
        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            html += '<select class="form-control" style="width:110px; height:33px" name="selectOPerate" onchange="selecClick(this)"><option value="-1"><%="请选择".ToLang()%></option>';
            if (row.IsCancel) {
                html += '<option rel="' + row.OrderID + '" value="view"><%="查看/编辑".ToLang()%></option>';
            }
            else {
                if (row.Status == 0)//已完成
                {
                    html += '<option rel="' + row.OrderID + '" value="view"><%="查看/编辑".ToLang()%></option>';
                    html += '<option rel="' + row.OrderID + '" value="pay"><%="支付".ToLang()%></option>';
                    html += '<option rel="' + row.OrderID + '" value="unfinish"><%="未完成".ToLang()%></option>';
                    html += '<option rel="' + row.OrderID + '" value="cancel"><%="取消".ToLang()%></option>';
                } else {
                    if (row.IsPaid) {
                        html += '<option rel="' + row.OrderID + '" value="view"><%="查看/编辑".ToLang()%></option>';
                        html += '<option rel="' + row.OrderID + '" value="unpay"><%="未支付".ToLang()%></option>';
                        html += '<option rel="' + row.OrderID + '" value="unfinish"><%="未完成".ToLang()%></option>';
                        html += '<option rel="' + row.OrderID + '" value="finish"><%="交易完成".ToLang()%></option>';
                        html += '<option rel="' + row.OrderID + '" value="cancel"><%="取消".ToLang()%></option>';
                    }
                    else {
                        if (row.PaymentID > 0) {
                            html += '<option rel="' + row.OrderID + '" value="view"><%="查看/编辑".ToLang()%></option>';
                            html += '<option rel="' + row.OrderID + '" value="pay"><%="支付".ToLang()%></option>';
                            html += '<option rel="' + row.OrderID + '" value="unfinish"><%="未完成".ToLang()%></option>';
                            html += '<option rel="' + row.OrderID + '" value="cancel"><%="取消".ToLang()%></option>';
                        }
                        else {
                            html += '<option rel="' + row.OrderID + '" value="view"><%="查看/编辑".ToLang()%></option>';
                            html += '<option rel="' + row.OrderID + '" value="pay"><%="支付".ToLang()%></option>';
                            html += '<option rel="' + row.OrderID + '" value="unfinish"><%="未完成".ToLang()%></option>';
                            html += '<option rel="' + row.OrderID + '" value="finish"><%="交易完成".ToLang()%></option>';
                            html += '<option rel="' + row.OrderID + '" value="cancel"><%="取消".ToLang()%></option>';
                        }
                    }
                }
               
            }
            html += '</select>';
            html += '</div>';
            return html;

        }
        //时间格式
        function GetDateTimeFormat(value, row, index, format) {
            if (format == "yyyy-mm-dd") {
                return whir.ajax.fixJsonDate(value, "-");
            }
            return whir.ajax.fixJsonDate(value);;
        }
        function reload() {
            whir.dialog.remove();
            $table.bootstrapTable('refresh');
        }
        function GetImage(value, row, index) {
            return ' <img src="../res/images/2013_icon_jia.gif" class="imgIcon" />';
        }
        //获取订单状态
        function GetStatus(value, row, index) {
            if (row.IsCancel) {
                return "<%="已取消".ToLang()%>";
            }
            switch (row.Status) {
                case 0:
                    return "<%="交易完成".ToLang()%>";
                default:
                    return "<%="未完成".ToLang()%>";
            }
        }
        //获取支付状态
        function GetPaid(value, row, index) {
            if (row.IsPaid == 1) {
                return "<%="已支付".ToLang()%>";
            }
            else {
                return "<%="未支付".ToLang()%>";
            }
        }
        //获取金额方法
        function GetAmountStr(value, row, index) {
            return "<b style='color:red;'>￥" + parseFloat(value).toFixed(2) + "</b>";
        }
        //头部状态事件
        $(".nav-tabs li a").each(function () {
            $(this).click(function () {
                $(".nav-tabs li").removeClass("active");
                $(".nav-tabs li a").removeAttr("data-toggle");
                $(this).parent().addClass("active");
                $(this).attr("data-toggle", "true");
                var ordertype = $(this).attr("rel");
                $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Plugin/shop/OrderForm.aspx?_action=GetList&ordertype=" + ordertype });
            })
        })
        //操作事件
        function selecClick(obj) {
            var operate = $(obj).find("option:selected").val();
            if (operate != "-1") {
                var val = $(obj).find("option:selected").attr("rel");
                if (operate == "view") {
                    window.location.href = "Order_Edit.aspx?orderid=" + val;
                }
                else {
                    var orderidActions = val + '|' + operate;
                    var dialog = whir.dialog.confirm("<%="确认订单状态改为".ToLang()%> " + $(obj).find("option:selected").text() + "<%="吗？".ToLang()%>", function () {
                        whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/OrderForm.aspx", {
                            data: {
                                _action: "UpdateOrderStatus",
                                orderidAction: orderidActions
                            },
                            success: function (response) {
                                if (response.Status == true) {
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
            }

        }
        //搜索
        function search() {
            var ordertype = '';
            $(".nav-tabs li a").each(function () {
                if ($(this).attr("data-toggle") == "true") {
                    ordertype = $(this).attr("rel");
                }
            })
            var orderNo = $("#OrderNo").val();
            var memberName = $("#MemberLoginName").val();
            var proAmountMin = $("#ProductAmountMin").val();
            var proAmountMax = $("#ProductAmountMax").val();
            var courierId = $("#courierId option:selected").val();
            var TakeName = $("#TakeName").val();
            var payAmountMin = $("#PayAmountMin").val();
            var payAmountMax = $("#PayAmountMax").val();
            var paymentId = $("#paymentID option:selected").val();
            var memberType = $("#memberType option:selected").val();
            var startdate = $("#StartDate").val();
            var enddate = $("#EndDate").val();
            $table.bootstrapTable('refreshOptions', {
                url: "<%=SysPath%>Handler/Plugin/shop/OrderForm.aspx?_action=GetList&ordertype=" + ordertype
                + "&orderNo=" + orderNo + "&memberName=" + memberName + "&proAmountMin=" + proAmountMin + "&proAmountMax=" + proAmountMax
                + "&courierId=" + courierId + "&TakeName=" + TakeName + "&payAmountMin=" + payAmountMin + "&payAmountMax=" + payAmountMax
                + "&paymentId=" + paymentId + "&memberType=" + memberType + "&startdate=" + startdate + "&enddate=" + enddate
            });
        }
        //清除搜索
        function ClearSearch() {
            $("input[class*='form-control']").val("");
            $("select").val("");
        }
    </script>
</asp:Content>
