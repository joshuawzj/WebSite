<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="ColumnList.aspx.cs" Inherits="Whir_System_Module_Column_ColumnList" %>

<%@ Import Namespace="System.Diagnostics" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Domain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath%>Res/assets/js/tree/tabelizer/tabelizer.min.css" media="all" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="栏目结构".ToLang()%></div>

            <div class="panel-body">
                <div class="actions">
                    <div class="btn-group">
                        <%if (IsDevUser)
                            {%>
                        <a class="btn btn-white" href="ColumnEdit.aspx"><%="添加栏目".ToLang()%></a>
                        <a class="btn btn-white" href="Column_Link.aspx"><%="添加外部链接".ToLang()%></a>
                        <%} %>
                    </div>
                    <%if (IsDevUser)
                        {%>
                    <a class="btn btn-white" href="SiteHome.aspx"><%="网站首页".ToLang()%></a>
                    <%} %>
                </div>

            <div class="tableCategory-table-body">
                <table id="tableColumn" width="100%" class="controller table table-bordered table-noPadding">
                    <thead>
                        <tr data-level="header" class="">
                            <th><%="栏目名称".ToLang()%></th>
                            <th><%="功能模块".ToLang()%></th>
                            <th width="80px"><%="排序".ToLang()%></th>
                            <th><%="操作".ToLang()%></th>
                        </tr>
                    </thead>
                    <tbody id="tbdColumnList">
                        <% foreach (var column in ColumnTreeList)
                            {
                        %>
                        <tr data-level="<%=column.LevelNum%>" id="level_<%=column.LevelNum%>_<%=column.ColumnId%>">
                            <td>
                                <%=column.ColumnName.Replace("└","").Replace("─", "").Replace("├", "").Replace("│", "").Replace("　", "")%>
                                <% if (CurrentUser.IsDeveloper)
                                    {%>
                                &nbsp;-&nbsp;<span class="text-danger pull-right"><%=column.ColumnId%></span>
                                <%  } %>
                            </td>
                            <td>
                                <%Model first = AllModel.FirstOrDefault(p => p.ModelId == column.ModelId);%><% =first == null ? "" : first.ModelName%>
                            </td>
                            <td>
                                <input class="form-control" type="text" columnid="<%=column.ColumnId%>" style="width: 120px;" value='<%=column.Sort%>' <% =(!IsDevUser && column.ParentId == 0) ? " readonly=\"readonly\"" : "" %>  />
                            </td>
                            <td>
                                <div class="btn-group">
                                    <%if (IsDevUser)
                                        {%>
                                    <a class="btn btn-sm btn-white" href='ColumnEdit.aspx?parentid=<%=column.ColumnId %>'>
                                        <%="添加子栏目".ToLang()%></a>
                                    <% }%>
                                    <%if (IsRoleHaveColumnRes("栏目修改", column.ColumnId, -1))
                                        {%>
                                    <a class="btn btn-sm btn-white" href='<%= string.IsNullOrEmpty(column.OutUrl) ? "ColumnEdit.aspx" : "Column_Link.aspx"%><%="?ColumnId=" + column.ColumnId%>&type=base'><%="编辑".ToLang()%></a>
                                    <% }%>
                                    <%if (IsRoleHaveColumnRes("复制", column.ColumnId, -1))
                                        {
                                            if (!IsDevUser && column.ParentId == 0) continue; //非root角色 第一级栏目不显示复制按钮%>
                                        <a class="btn btn-sm btn-white" href="javascript:;" onclick="openCopy('<%=column.ColumnId%>');"><%="复制".ToLang()%></a>
                                    <% }%>
                                    <%if (column.ModelId != 0 && IsDevUser)
                                        {%>
                                    <a class="btn btn-sm btn-white" href="FormList.aspx?ColumnId=<%=column.ColumnId%>"><%="表单管理".ToLang()%></a>
                                    <%} %>
                                </div>
                                <%if (IsRoleHaveColumnRes("栏目删除", column.ColumnId, -1))
                                    {%>
                                <a class="btn btn-sm text-danger border-normal" href="javascript:;" onclick="Delete(<%=column.ColumnId%>)"><%="删除".ToLang()%></a>  <% }%>
                            </td>
                        </tr>
                        <%} %>
                    </tbody>
                </table>
                </div> 
                <div class="btn-group">
                    <a class="btn btn-sm btn-white" href='javascript:;' onclick="getSort()"><%="排序".ToLang()%></a>
                </div> 
            </div>

        </div>
    </div>
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery.tabelizer.js"></script>
    <script type="text/javascript">
        //树形表格
        var tableColumn = $('#tableColumn').tabelize({
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

        //打开复制栏目页面
        function openCopy(columnId) {
            whir.dialog.frame('<%="目标栏目信息".ToLang()%>', "<%=SysPath%>module/column/sitecolumn_copy.aspx?columnid=" + columnId, null, 500, 400);
        }

        //获取排序的值, 并赋值给hidSort
        function getSort() {
            var result = "";
            $("#tbdColumnList").find("input[type='text']").each(function () {
                var ID = $(this).attr("columnid");
                var sort = $(this).val();
                result += ID + "|" + sort + ",";
            });

            if (result != '') {
                result = result.substring(0, result.length - 1);
                whir.ajax.post("<%=SysPath%>Handler/Developer/Column.aspx",
                    {
                        data: {
                            _action: "SortColumn",
                            Sort: result
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
            } else {
                TipMessage('<%="无数据".ToLang()%>');
                return false;
            }
            return false;
        }

        function Delete(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                 function () {
                     whir.ajax.post("<%=SysPath%>Handler/Developer/Column.aspx",
                        {
                            data: {
                                _action: "DeleteColumn",
                                Id: id
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
