<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="language.aspx.cs" Inherits="whir_system_module_developer_language" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading"><%="多语言配置".ToLang()%></div>
            <div class="panel-body">
                <div class="actions btn-group pull-left">
                    <a href="language_Edit.aspx" id="aAdd" class="btn btn-white"><%="添加多语言".ToLang()%></a>
                </div>
                <br />
                <div id="toolbar"></div>
                <table id="Common_Table" class="All_list">
                </table>
            </div>

        </div>
    </div>
    <script type="text/javascript">


        var $table = $('#Common_Table'), _that;

        //列表  
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Developer/Language.aspx?_action=GetList",
                dataType: "json",
                pagination: true, //分页
                search: true, //显示搜索框
                sidePagination: "server", //服务端处理分页

                pageSize: 10,
                columns: [

                    { title: '<%="简体中文".ToLang()%>', field: 'CN', align: 'left', valign: 'middle' },
                    { title: '<%="繁体中文".ToLang()%>', field: 'HkText', align: 'left', valign: 'middle' },
                    { title: '<%="英文".ToLang()%>', field: 'EnText', align: 'left', valign: 'middle' },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 280, align: 'left', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ],
                onLoadError: function (data, mes) {
                    if (mes && mes.responseText) {
                        whir.toastr.warning(mes.responseText);
                    } else {
                        whir.toastr.error('<%="获取数据失败！".ToLang()%>');
                    }
                }

            });
            whir.loading.remove();
        }

        initTable();
         
        //重置表格样式
        var setTableStyle = function () {
            $(".sortTable tr[data-index]").removeClass();
            $(".sortTable tr:odd").addClass("tdBgColor tdColor");
            $(".sortTable tr:even").addClass("tdColor");

        };
         
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            html += '<a name="aEdit" class="btn btn-white" href="language_Edit.aspx?cn=' + row.CN +'"><%="编辑".ToLang() %></a>';
            html += '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(\'' + row.CN + '\')" href="javascript:;"><%="删除".ToLang()%></a>';
            html += '</div>';
            return html;
        }

        function reload() {
            $table.bootstrapTable('refresh');
        }

        //删除
        function Delete(cn) {
            
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                function () {
                    whir.dialog.remove();
                    whir.ajax.post("<%=SysPath%>Handler/Developer/Language.aspx",
                        {
                            data: {
                                _action: "Delete",
                                cn: cn
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

