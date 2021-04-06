<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="ModuleManageList.aspx.cs" Inherits="whir_system_module_developer_ModuleManageList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="管理模块".ToLang()%></div>
            <div class="panel-body">
                <div class="actions btn-group">
                    <a class="btn btn-white" href="ModuleManage_edit.aspx"><%="添加模块".ToLang()%></a>
                </div>
                <table id="Common_Table" class="productlist_wap"></table>
                <div class="space10"></div>
                <div class="operate_foot">
                    <input type="hidden" id="hidChoose" />
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function() {
            //whir.checkbox.checkboxOnload("lbDel", "hidChoose", "cb_Top", "cb_Position");
        });
        var $table = $('#Common_Table'), _that;

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Developer/ModuleManageForm.aspx?_action=GetList",
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
                       whir.toastr.error("获取数据失败！");
                    }
                },
                onColumnSwitch: function () {
                    //SetTableStyleEvent();
                },
                SelectColumnEvent: function () {
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'Checkbox', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: 'Id', field: 'PluginId', width: 60, align: 'left', valign: 'middle' },
                    { title: '模块名称', field: 'ModuleName', align: 'left', valign: 'middle' },
                    { title: '模块目录', field: 'ModuleFolder', align: 'left', valign: 'middle' },
                    { title: '版本号', field: 'ModuleVersion', align: 'left', valign: 'middle' },
                    { title: '添加时间', field: 'CreateDate', align: 'center', valign: 'middle', sortable: true, filterControl: '7', format: 'yyyy-mm-dd hh:ii:ss', formatter: function (value, row, index) { return GetDateTimeFormat(value, row, index, 'yyyy-MM-dd HH:mm:ss'); } },
                    { title: '说明', field: 'Op', width: 300, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } }
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
            return '<input type="checkbox" name="btSelectItem" value="' + row.SubmitId + '" />';

        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            if (row.IsInstall) {
                html += '<a name="lbUninstall" class="btn text-danger border-normal" onclick="uninstall(' + row.PluginId + ')" href="javascript:;"><%="卸载".ToLang() %></a>';
                html += '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(' + row.PluginId + ')" href="javascript:;"><%="删除".ToLang()%></a>';
                html += '</div>';
            }
            else {
                html += '<a name="lbInstall" class="btn text-success border-normal" onclick="install(' + row.PluginId + ')" href="javascript:;"><%="安装".ToLang()%></a>';
                html += '<a name="lbDelete" class="btn text-danger border-nromal" onclick="Delete(' + row.PluginId + ')" href="javascript:;"><%="删除".ToLang()%></a>';
                html += '</div>';
            }
            return html;
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
        //时间格式
        function GetDateTimeFormat(value, row, index, format) {
            if (format == "yyyy-mm-dd") {
                return whir.ajax.fixJsonDate(value, "-");
            }
            return whir.ajax.fixJsonDate(value);;
        }

        function reload() {
            $table.bootstrapTable('refresh');
        }
        //安装
        function install(id) {
            whir.dialog.confirm("<%="确认要安装吗？".ToLang() %>",
                 function () {
                     whir.dialog.remove();
                     whir.ajax.post("<%=SysPath%>Handler/Developer/ModuleManageForm.aspx",
                     {
                         data: {
                             _action: "Install",
                             submitId: id
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
                     return false;
                 });
        }
        //卸载
        function uninstall(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                             function () {
                                 whir.dialog.remove();
                                 whir.ajax.post("<%=SysPath%>Handler/Developer/ModuleManageForm.aspx",
                     {
                         data: {
                             _action: "Uninstall",
                             submitId: id
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
                     return false;
                 });
        }
        function Delete(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                 function () {
                     whir.dialog.remove();
                     whir.ajax.post("<%=SysPath%>Handler/Developer/ModuleManageForm.aspx",
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

