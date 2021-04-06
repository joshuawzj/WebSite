<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="AdminList.aspx.cs" Inherits="Whir_System_Module_Setting_AdminList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     
    <script src="<%=SysPath %>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <%if (Whir.Framework.RequestUtil.Instance.GetQueryInt("type", 0) != 2)
                { %>
            <div class="panel-heading"><%="管理员管理".ToLang()%></div>
            <%} %>
            <div class="panel-body">

                <%if (Whir.Framework.RequestUtil.Instance.GetQueryInt("type", 0) != 2)
                    { %>
                <%if (IsCurrentRoleMenuRes("320"))
                    { %>
                <div class="actions btn-group">
                    <a href="Admin_Edit.aspx" id="aAdd" class="btn btn-white"><%="添加管理员".ToLang()%></a>
                </div>
                <%} %>
                <%}
                    else
                    {%>

                <ul class="nav nav-tabs">
                    <li><a href="RoleList.aspx" aria-expanded="true"><%="角色管理".ToLang()%></a></li>
                    <li class="active"><a data-toggle="tab" href="#single" aria-expanded="true"><%= CurrentRole.RoleName+" - 成员管理".ToLang()%></a></li>
                </ul>
                <br />

                <%} %>

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
                url: "<%=SysPath%>Handler/Module/Security/Admin.aspx?_action=GetList&rolesid=<%=Whir.Framework.RequestUtil.Instance.GetQueryInt("rolesid", 0)%>",
                dataType: "json",
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                pageSize: 10,
                columns: [
                    { title: 'Id', field: 'UserId', width: 50, align: 'center', valign: 'middle', formatter: function (value, row, index) { return ForSort(value, row, index); } },
                    { title: '<%="用户名".ToLang()%>', field: 'LoginName', align: 'left', valign: 'middle' },
                    { title: '<%="真实姓名".ToLang()%>', field: 'RealName', align: 'left', valign: 'middle' },
                    { title: '<%="所属角色".ToLang()%>', field: 'RolesName', align: 'left', valign: 'middle' },
                    { title: '<%="电子邮箱".ToLang()%>', field: 'Email', align: 'left', valign: 'middle' },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 280, align: 'left', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ],
                onLoadError: function (data,mes) {
                     if(mes && mes.responseText){
                       whir.toastr.warning(mes.responseText);
                     }else{
                         whir.toastr.error("<%="获取数据失败！".ToLang() %>");
                    }
                }

            });
            whir.loading.remove();
        }

        initTable();

        $(function () {

            //拖动排序
            $("#Common_Table").sortable(
            {
                items: "tr[data-index]",
                appendTo: 'parent',
                handle: '.dragCursor',
                stop: function (event, ui) {
                    saveSort(event, ui);
                },
                axis: 'y'
            });
        });

        //异步保存排序
        function saveSort(event, ui) {

            var ids = "";
            $(".dragCursor").each(function () {
                ids += $(this).html() + ",";
            });

            whir.ajax.post("<%=SysPath%>Handler/Common/Sort.aspx",
                  {
                      data: {
                          _action: "SortUser",
                          Ids: ids,
                      },
                      success: function (response) {
                          whir.loading.remove();
                          if (response.Status == true) {
                              whir.toastr.success("<%="排序成功".ToLang()%>");
                              reload();
                          } else {
                              whir.toastr.error(response.Message);
                          }
                      }
                  });

        }

        //重置表格样式
        var setTableStyle = function () {
            $(".sortTable tr[data-index]").removeClass();
            $(".sortTable tr:odd").addClass("tdBgColor tdColor");
            $(".sortTable tr:even").addClass("tdColor");

        };


        //实现拖拽排序
        function ForSort(value, row, index) {
            <%if (IsCurrentRoleMenuRes("380"))
        { %>
            return ' <div  class="dragCursor" sort="' + row.Sort + '" title="<%="点击可以拖拽排序".ToLang()%>" >' + row.UserId + '</div> ';
            <%}
        else
        { %>
            return ' <div  sort="' + row.Sort + '"  title="<%="点击可以拖拽排序".ToLang()%>">' + row.UserId + '</div> ';
            <%}%>

        }


        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            <%if (IsCurrentRoleMenuRes("322"))
            { %>
            html += '<a name="aEdit" class="btn btn-white" href="Admin_Edit.aspx?userid=' + row.UserId + '&type=<%=Whir.Framework.RequestUtil.Instance.GetQueryInt("type",0) %>&';
            html += 'rolesid=<%=Whir.Framework.RequestUtil.Instance.GetQueryInt("rolesid",0) %>"><%="编辑".ToLang() %></a>';
            <%} %>
            <%if (IsCurrentRoleMenuRes("323"))
            { %>
                <%if (IsCurrentRoleMenuRes("321"))
                { %>
            if (row.UserId != 1) {
                if (row.State == 0)
                    html += '<a name="aEdit" class="btn text-danger border-normal" onclick="Disabled(' + row.UserId + ')" href="javascript:;"><%="禁用".ToLang() %></a>';
                else
                    html += '<a name="aEdit" class="btn btn-white" onclick="Enabled(' + row.UserId + ')"  href="javascript:;"><%="启用".ToLang() %></a>';
            }
                <%} %>
            
            if (row.UserId != 1) {
                html += '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(' + row.UserId + ')" href="javascript:;"><%="删除".ToLang()%></a>';
            }
            <%} %>
            html += '</div>';
            return html;
        }

        function reload() {
            $table.bootstrapTable('refresh');
        }

        //禁用
        function Disabled(id) {
            whir.ajax.post("<%=SysPath%>Handler/Module/Security/Admin.aspx", {
                    data: {
                        _action: "Disabled",
                        userid: id
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
        }

        //启用
        function Enabled(id) {
            whir.ajax.post("<%=SysPath%>Handler/Module/Security/Admin.aspx", {
                    data: {
                        _action: "Enabled",
                        userid: id
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
        }

        //删除
        function Delete(id) {
            if (id == 1) {
                whir.toastr.error("<%="该用户不允许删除！".ToLang() %>");
                return false;
            }
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                function () {
                    whir.dialog.remove();
                    whir.ajax.post("<%=SysPath%>Handler/Module/Security/Admin.aspx",
                        {
                            data: {
                                _action: "Delete",
                                userid: id
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
