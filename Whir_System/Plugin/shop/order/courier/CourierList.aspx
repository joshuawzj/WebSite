<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="courierlist.aspx.cs" Inherits="whir_system_Plugin_shop_order_courier_courierlist" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Register Src="../../common/HeadContainer.ascx" TagName="HeadContainer" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div class="content-wrap">
            <div class="space15"></div>
            <div class="panel">
                <div class="panel-heading">
                    <%="配送方式管理".ToLang()%>
                </div>
                <div class="panel-body">
                    <div class="actions btn-group">
                        <a class="btn btn-white" href="courier_edit.aspx" ><%="添加配送方式".ToLang()%></a>
                    </div>
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
                url: "<%=SysPath%>Handler/Plugin/shop/CourierForm.aspx?_action=GetList",
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
                    { title: '<%="配送方式名称".ToLang()%>', field: 'CourierName', width: 260, align: 'left', valign: 'middle' },
                    { title: '<%="快递公司代码".ToLang()%>', field: 'Com', align: 'left', valign: 'middle' },
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
            return '<input type="checkbox" name="btSelectItem" value="' + row.CourierID + '" />';
        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            html += '<a name="lbEdite" class="btn btn-white"  href="<%=SysPath%>Plugin/shop/order/courier/courier_edit.aspx?courierid=' + row.CourierID + '"><%="编辑".ToLang() %></a>';
            html += '<a name="lbDel" class="btn text-danger border-normal"  onclick="Deleted(' + row.CourierID + ')" href="javascript:;"><%="删除".ToLang() %></a>';
            html += '</div>';
            return html;
        }
        function reload() {
            $table.bootstrapTable('refresh');
        }
        //删除
        function Deleted(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                function () {
                    whir.dialog.remove();
                    whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/CourierForm.aspx",
                     {
                         data: {
                             _action: "Del",
                             submitId: id
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
                    return false;
                });
            }
    </script>
</asp:Content>

