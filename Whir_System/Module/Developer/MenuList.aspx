<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="MenuList.aspx.cs" Inherits="Whir_System_Module_Developer_MenuList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <link href="<%=SysPath%>Res/assets/js/tree/tabelizer/tabelizer.min.css" media="all" rel="stylesheet" type="text/css">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="菜单管理".ToLang()%></div>
            <div class="panel-body">
                <div class="actions">
                    <a class="btn btn-white" href="MenuEdit.aspx"><%="添加菜单".ToLang()%></a>
                </div>
        <div class="tableCategory-table-body">
                <table id="tableMenu" width="100%" class="controller table table-bordered table-noPadding">
                    <thead>
                        <tr data-level="header" class="">
                            <th><%="菜单名称".ToLang()%></th>
                            <th><%="菜单ID".ToLang()%></th>
                            <th><%="菜单类型".ToLang()%></th>
                            <th><%="链接地址".ToLang()%></th>
                            <th><%="操作".ToLang()%></th>
                        </tr>
                    </thead>
                    <tbody>

                        <% foreach (var menu in MenuTreeList)
                            {
                        %>
                        <tr data-level="<%=menu.Level%>" id="level_<%=menu.Level%>_<%=menu.MenuId%>">
                            
                            <td><%=menu.MenuName%></td>
                              <td><%=menu.MenuId%></td>
                            <td class=""><%=menu.MenuType%></td>
                            <td class="text-left"><%=menu.Url%></td>
                            <td class="">
                                <div class="btn-group">
                                    <a class="btn btn-sm btn-white" href="MenuEdit.aspx?menuId=<%=menu.MenuId%>"><%="编辑".ToLang()%></a>
                                    <a class="btn btn-sm text-danger border-normal" table-action="delete" table-data="<%=menu.MenuId%>"><%="删除".ToLang()%></a>
                                </div>
                            </td>
                        </tr>
                        <%  } %>
                        
                    </tbody>
                </table>
                </div>
            </div>
        </div>
    </div>
    
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery.tabelizer.js"></script>    <script type="text/javascript">
        //树形表格
        var tableMenu = $('#tableMenu').tabelize({
            /*onRowClick : function(){
                 
            }*/
            fullRowClickable: false,
            onReady: function () {
                console.log('ready');
            },
            onBeforeRowClick: function () {
                //console.log('onBeforeRowClick');
            },
            onAfterRowClick: function () {
                //console.log('onAfterRowClick');
            }
        });

        //删除事件
        $("[table-action='delete']").click(function () {
            var menuId = $(this).attr("table-data");
            var dialog = whir.dialog.confirm("确认删除吗？", function () {
                whir.loading.show();
                whir.ajax.post("<%=SysPath%>Handler/Developer/Menu.aspx", {
                    data: {
                        _action: "Del",
                        menuId: menuId
                    },
                    success: function (response) {
                        if (response.Status == true) {
                            whir.toastr.success(response.Message, true, false);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    }
                });
            });
        });
    </script>
</asp:Content>
