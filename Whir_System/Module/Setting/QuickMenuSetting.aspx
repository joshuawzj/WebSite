<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="QuickMenuSetting.aspx.cs" Inherits="Whir_System_Module_Setting_QuickMenuSetting" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  
    <script src="<%=SysPath %>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
         <div class="panel-heading"><%="快捷菜单管理".ToLang()%></div>
            <div class="panel-body">
                <div class="actions btn-group pull-left">
                     <%if (IsCurrentRoleMenuRes("379"))
                       { %>
                    <a class="btn btn-white" href="QuickMenu_Edit.aspx"><%="添加快捷菜单".ToLang()%></a>
                     <%} %>
                </div>
                <div class="member-list-last">
                 <table id="Common_Table">
                   </table> 
                   </div>
                  <div class="space10"></div>
                <div class="operate_foot">
                    <input type="hidden" id="hidChoose"/>
                    
                    <%if (IsCurrentRoleMenuRes("379"))
                      { %>
                    <a id="lbDel" class="btn text-danger border-danger"><%="批量删除".ToLang()%></a>
                     <%} %>
                </div>
                 <div class="clear"></div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            whir.checkbox.checkboxOnload("lbDel", "hidChoose", "cb_Top", "cb_Position");

        });

        var $table = $('#Common_Table'), _that;

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Developer/QuickMenu.aspx?_action=GetList",
                dataType: "json",
                pagination: false, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                onLoadSuccess: function () {
                    //设置样式 后期需修改
                    SetTableStyleEvent();
                },
                onLoadError: function (data, mes) {
                    if (mes && mes.responseText) {
                        whir.toastr.warning(mes.responseText);
                    } else {
                        whir.toastr.error("<%="获取数据失败！".ToLang() %>");
                    }
                },
                onColumnSwitch: function () {
                    SetTableStyleEvent();
                },
                SelectColumnEvent: function () {
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'Checkbox', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: 'Id', field: 'Id', width: 60, align: 'center', valign: 'middle' },
                    { title: '<%="快捷菜单名称".ToLang()%>', field: 'MenuName', align: 'left', valign: 'middle' },
                    { title: '<%="链接地址".ToLang()%>', field: 'Url', align: 'left', valign: 'middle' },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 135, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ]

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
        
        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" name="btSelectItem" value="' + row.Id + '" />';

        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = "<div class='btn-group'>";
           <%if (IsCurrentRoleMenuRes("303"))
            { %>
            html += '<a name="aEdit" class="btn btn-white" href="QuickMenu_Edit.aspx?Id=' + row.Id + '"><%="编辑".ToLang() %></a>';
            <%} %>
            <%if (IsCurrentRoleMenuRes("304"))
            { %>
            html += '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(' + row.Id + ')" href="javascript:;"><%="删除".ToLang()%></a>';
            <%} %>
            html += "</div>";
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


        function reload() {
            $table.bootstrapTable('refresh');
        }

        function Delete(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                whir.ajax.post("<%=SysPath%>Handler/Developer/QuickMenu.aspx",
                    {
                        data: {
                            _action: "Del",
                            MenuId: id
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
                whir.dialog.remove();
                return false;
            });
        }

        $("#lbDel").click(function () {
            if ($("#hidChoose").val() == "") {
                whir.toastr.warning("<%="请选择要删除的数据行！".ToLang() %>");
            } else {
                whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                    whir.ajax.post("<%=SysPath%>Handler/Developer/QuickMenu.aspx", {
                        data: {
                            _action: "DelAll",
                            MenuIds: $("#hidChoose").val()
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
                    });
                    whir.dialog.remove();
                    return false;
                });
            }
        });

    </script>
</asp:Content>
