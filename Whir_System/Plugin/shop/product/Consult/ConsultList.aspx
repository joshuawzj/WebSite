<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="consultlist.aspx.cs" Inherits="whir_system_Plugin_shop_product_consult_consultlist" %>

<%@ Import Namespace="Whir.Language" %>
 <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
         
    <style type="text/css">
        .afterTr table td {
            background: #f4f7f8;
            border-left: solid 1px #ddd;
            border-top: solid 1px #ddd;
            border-bottom: none;
            padding-top: 5px;
            padding-bottom: 5px;
            line-height: 18px;
        }
    </style>
    <script type="text/javascript">
        function search() {
           var proName = $("#ProName").val();
            var consult = $("#Consult").val();
            var state = $("#State").find("option:selected").val();
            var consultuser = $("#ConsultUser").val();
            if (state == "<%="==请选择==".ToLang()%>")
                state = "-2";
            $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Plugin/shop/ConsultForm.aspx?_action=GetList&ProNameKey=" + proName + "&ConsultKey=" + consult + "&ConsultUserKey=" + consultuser + "&State=" + state });
        }
        $(function () {
            //搜索选清空
            $("#btnClear").click(function () {
                $("#ProName").val("");
                $("#Consult").val("");
                $("#ConsultUser").val("");
                $("#State option").first().attr("selected", "selected");
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div class="content-wrap">
            <div class="space15"></div>
            <div class="panel">
                <div class="panel-heading"><%="咨询管理".ToLang()%></div>
                <div class="panel-body">
                 <table id="Common_Table" class="ConsultList_wap">
                </table>
                <div class="space10"></div>
                <input type="hidden" id="hidChoose" />
                    <div class="operate_foot">
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
                url: "<%=SysPath%>Handler/Plugin/shop/ConsultForm.aspx?_action=GetList",
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
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'ConsultID', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: '<%="咨询商品".ToLang()%>', field: 'ProName', width: 260, filterControl: '1', align: 'left', valign: 'middle' },
                    { title: '<%="咨询内容".ToLang()%>', field: 'Consult', align: 'center', filterControl: '1', valign: 'middle' },
                    { title: '<%="是否审核".ToLang()%>', field: 'State', align: 'center', filterControl: '9', filterData:'json:{"0": "<%="否".ToLang()%>","1": "<%="是".ToLang()%>"}', valign: 'middle', formatter: function (value, row, index) { return GetStateStr(value, row, index); } },
                    { title: '<%="咨询人".ToLang()%>', field: 'LoginName', align: 'center', filterControl: '1', valign: 'middle' },
                    { title: '<%="咨询时间".ToLang()%>', field: 'CreateDate', align: 'center', filterControl: '7', valign: 'middle', sortable: true,  format: 'yyyy-mm-dd hh:ii:ss', formatter: function (value, row, index) { return GetDateTimeFormat(value, row, index, 'yyyy-MM-dd HH:mm:ss'); } },
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
            return '<input type="checkbox" name="btSelectItem" value="' + row.ConsultID + '" />';
        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            html += '<a name="lbDetail" class="btn btn-white"  onclick="detail(\'' + row.LoginName + '\',' + row.ConsultID + ')" href="javascript:;"><%="详细".ToLang() %></a>';
            html += '<a name="lbDel" class="btn btn-info text-danger border-danger"  onclick="deleted(' + row.ConsultID + ')" href="javascript:;"><%="删除".ToLang() %></a>';
            html += '</div>';
            return html;
        }
        //获取审核状态
        function GetStateStr(value, row, index) {
            var str = '';
            if (row.State == 1) {
                str = '<%="是".ToLang()%>';
            }
            else {
                str = '<%="否".ToLang()%>';
            }
            return str;
        }
        
        //时间格式
        function GetDateTimeFormat(value, row, index, format) {
            if (format == "yyyy-mm-dd") {
                return whir.ajax.fixJsonDate(value, "-");
            }
            return whir.ajax.fixJsonDate(value);
        }
        function reload() {
            $table.bootstrapTable('refresh');
        }
       
        //删除
        function deleted(consultId) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
              function () {
                  whir.dialog.remove();
                  whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ConsultForm.aspx",
                     {
                         data: {
                             _action: "Del",
                             consultId: consultId
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
          //批量删除
        $("#a_dels").click(function() {
            var ids = getIds();
            if (ids == "") {
                whir.toastr.warning("<%="请选择".ToLang() %>");
            } else {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/ConsultForm.aspx",
                    {
                        data: {
                            _action: "BatchDel",
                            ids: ids
                        },
                        success: function(response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                reload();
                                initTable(); //重新加载数据
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
            }
        });

        function detail(consultUser, consultId) {
            whir.dialog.frame('<%="详细".ToLang() %>', "<%=AppName%>Whir_System/Plugin/shop/product/Consult/ConsultDetail.aspx?ConsultUser=" + consultUser + "&ConsultID=" + consultId + "&time=" + new Date(), "", 1100, 500);
 
        }


        function getIds() {
            var ids = "";
            $("#Common_Table input[name='btSelectItem']").each(function() {
                if ($(this).is(":checked")) {
                    ids += $(this).val() + ",";
                }
            });
            if (ids != "") {
                ids = ids.substring(0, ids.length - 1);
            }
            return ids;
        }
    </script>
</asp:Content>
