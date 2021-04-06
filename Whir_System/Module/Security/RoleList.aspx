<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="RoleList.aspx.cs" Inherits="Whir_System_Module_Setting_RoleList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath%>Res/assets/js/tree/tabelizer/tabelizer.min.css" media="all" rel="stylesheet" type="text/css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading"><%="角色管理".ToLang()%></div>
            <div class="panel-body">
                <div class="actions btn-group">
                    <%if (IsCurrentRoleMenuRes("324"))
                      { %>
                    <a href="RoleEdit.aspx" class="btn btn-white"><%="添加角色".ToLang()%></a>
                    <%} %>
                </div>
                <div class="tableCategory-table-body">
                <table width="100%" id="tableList" class="controller table table-bordered table-noPadding">
                    <thead>
                     <tr data-level="header" class="">
                        <th>
                            <%="角色名称".ToLang()%>
                        </th>
                        <th>
                            <%="角色描述".ToLang()%>
                        </th>
                        <th>
                            <%="管理操作".ToLang()%>
                        </th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">
                            <ItemTemplate>
                                <tr data-level="<%#Eval("Depth")%>" id="level_<%#Eval("Depth")%>_<%#Eval("RoleId")%>">
                                    <td style="min-width:180px">
                                        <%#Eval("RoleName")%>[<%#Eval("RoleId")%>]
                                    </td>
                                    <td style="min-width:130px">
                                        <%#Eval("Remarks")%>
                                    </td>
                                    <td width='<%=LanguageHelper.GetSplitValue("560px|380px|550px")%>' style="min-width:300px">
                                        <div class="btn-group">
                                         <%if (IsCurrentRoleMenuRes("324"))
                                           { %>
                                            <asp:PlaceHolder ID="phAddChildren" runat="server">
                                                <a class="btn btn-white" name="aAddChildren" href="RoleEdit.aspx?ParentRoleId=<%# Eval("RoleId")%>"><%="添加下级角色".ToLang()%></a>
                                            </asp:PlaceHolder>
                                            <%} %>
                                            <asp:PlaceHolder ID="phSQ" runat="server" Visible="false">
                                                <a class="btn btn-info" name="aSQ" href="../developer/Jurisdiction_menu.aspx"><%="分配权限".ToLang()%></a>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder ID="phCX" runat="server">
                                                <%if (IsCurrentRoleMenuRes("328"))
                                                    { %>
                                                <a class="btn btn-white" name="aMenuAccess" href="javascript:void(0);" onclick="whir.dialog.frame('<%="菜单权限".ToLang()%>', 'Jurisdiction_menu.aspx?roleid=<%# Eval("RoleId")  %>','cx',450,500);"><%="菜单权限".ToLang()%></a>
                                                <%} %>
                                                <%if (IsCurrentRoleMenuRes("329"))
                                                    { %>
                                                <a class="btn btn-white" name="aColumnAccess" href="Jurisdiction_column.aspx?roleid=<%# Eval("RoleId")%>"><%="栏目权限".ToLang()%></a>
                                                <%} %>
                                            </asp:PlaceHolder>
                                            <%if (IsCurrentRoleMenuRes("325"))
                                                { %>
                                            <a name="aMemberManagement" class="btn btn-white" href="adminlist.aspx?rolesid=<%# Eval("RoleId") %>&type=2"><%="成员管理".ToLang()%></a>
                                            <%} %>
                                            
                                            <%if (IsCurrentRoleMenuRes("326"))
                                                { %>
                                            <a class="btn btn-white" name="aEdit" href="RoleEdit.aspx?rolesid=<%# Eval("RoleId")  %>"><%="编辑".ToLang()%></a>
                                            <%} %>
                                            <%if (IsCurrentRoleMenuRes("327"))
                                                { %>
                                            <asp:PlaceHolder ID="lbtnDel" runat="server">
                                                <a name="lbtnDel" onclick="Delete(<%# Eval("RoleId") %>)" class="btn text-danger border-normal"><%="删除".ToLang()%></a>
                                            </asp:PlaceHolder>
                                            <%} %>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
                </div>
                <input type="hidden" id="hfStrMap" name="hfStrMap"/>
            </div>
        </div>
    </div>
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery.tabelizer.js"></script>
    <script type="text/javascript">
        //树形表格
        var tableMenu = $('#tableList').tabelize({
            /*onRowClick : function(){
            alert('test');
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
        //删除
        function Delete(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                function () {
                    whir.dialog.remove();
                    whir.ajax.post("<%=SysPath%>Handler/Module/Security/Roles.aspx",
                        {
                            data: {
                                _action: "Delete",
                                roleId: id
                            },
                            success: function (response) {
                                whir.loading.remove();
                                if (response.Status == true) {
                                    whir.toastr.success(response.Message, true, false);
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
