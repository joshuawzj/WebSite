<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="AttachedRelease.aspx.cs" Inherits="whir_system_module_release_attachedrelease" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  <script>
        //批量选中发布
        function relase() {
            //获取选择的值
            if ($("#hidSelected").val() == "") {
                whir.toastr.warning("<%="请选择".ToLang() %>");
                return false;
            }
            AttachedAll($("#hidSelected").val());
            return true;
        }

        $(document).ready(function () {
            whir.checkbox.checkboxOnload("lbnRelease", "hidSelected", "cb_Top", "cbx_item");
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading">
                <%="附带模板设置".ToLang()%>
            </div>
            <div class="panel-body">
                <div class="actions btn-group pull-left">
                    <a class="btn btn-white" href="AttachedRelease_Edit.aspx"><%="添加发布".ToLang()%></a>
                </div>
                <table id="Common_Table" class="AttachedRelease_table"></table>

                <input type="hidden" id="hidSort" />
                <input type="hidden" id="hidSelected" />
                <div class="space15"></div>
                <div class="operate_foot">
                    <button id="lbnRelease" class="btn btn-white" onclick="return relase();">
                        <%="批量发布".ToLang()%>
                    </button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        var $table = $('#Common_Table'), _that;

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Module/Release/Release.aspx?_action=GetList",
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
                onColumnSwitch: function () {
                    SetTableStyleEvent();
                },
                SelectColumnEvent: function () { 
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'Checkbox', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: '<%="名称".ToLang()%>', field: 'Names', width: 120, align: 'left', valign: 'middle' },
                    { title: '<%="所属栏目".ToLang()%>', field: 'ColumnName', width: 120, align: 'left', valign: 'middle' },
                    { title: '<%="模板文件".ToLang()%>', field: 'TemplateUrl', align: 'left', valign: 'middle' },
                    { title: '<%="生成文件".ToLang()%>', field: 'CreateFileUrl', align: 'left', valign: 'middle' },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 280, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ]

            });
            whir.loading.remove();
        }

        initTable();
 
        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" name="btSelectItem" primaryvalue="' + row.AttachedId + '"   value="' + row.AttachedId + '" />';

        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            html += '<a name="aEdit" class="btn btn-white"  onclick="AttachedCreate(' + row.AttachedId + ')" href="javascript:;"><%="生成".ToLang() %></a>';
            html += '<a name="aEdit" class="btn btn-white" href="<%=AppName%>' + row.CreateFileUrl + '" target="_blank"><%="预览".ToLang() %></a>';
            html += '<a name="aEdit" class="btn btn-white" href="attachedrelease_edit.aspx?attachedid=' + row.AttachedId + '"><%="编辑".ToLang() %></a>';
            html += '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(' + row.AttachedId + ')" href="javascript:;"><%="删除".ToLang()%></a>';
            html += '</div>';
            return html;
        }
        function SetTableStyleEvent() {
            whir.checkbox.destroy();
            whir.skin.radio();
            whir.skin.checkbox();
            //绑定全选按钮事件
            $("#btSelectAll").next().click(function () {
                if ($("#btSelectAll").is(':checked')) {
                    whir.checkbox.selectAll('lbDel', 'hidSelected', 'btSelectItem');
                } else {
                    whir.checkbox.cancelSelectAll('lbDel', 'hidSelected', 'btSelectItem');
                }

            });

            $("input[name=btSelectItem]").each(function () {
                $(this).next().click(function () {
                    $("#hidSelected").val(whir.checkbox.getSelect('btSelectItem'));
                });
            });
        }
        
        function reload() {
            $table.bootstrapTable('refresh');
        }
        
        //发布
        function AttachedCreate(id) {
            whir.ajax.post("<%=SysPath%>Handler/Module/Release/Release.aspx", {
                    data: {
                        _action: "AttachedCreate",
                        AttachedId: id
                    },
                    success: function(response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                }
            );
            return false;
        }

        //删除
        function Delete(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                function() {
                    whir.dialog.remove();
                    whir.ajax.post("<%=SysPath%>Handler/Module/Release/Release.aspx", {
                            data: {
                                _action: "Delete",
                                AttachedId: id
                            },
                            success: function(response) {
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

        //批量发布
        function AttachedAll(ids) {
            whir.ajax.post("<%=SysPath%>Handler/Module/Release/Release.aspx", {
                    data: {
                        _action: "AttachedAll",
                        ids: ids
                    },
                    success: function(response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                }
            );
            return false;
        }
    </script>
</asp:content>
