<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="subjectcolumnlist.aspx.cs" Inherits="whir_system_module_subject_subjectcolumnlist" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Domain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath%>Res/assets/js/tree/tabelizer/tabelizer.min.css" media="all"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript">

        //获取排序的值, 并赋值给hidSort
        function getSort() {
            var result = "";
            $(".apid_sort").each(function () {
                var columnId = $(this).attr("ColumnId");
                var sort = $(this).val();
                result += columnId + "|" + sort + ",";
            });

            if (result != '') {
                result = result.substring(0, result.length - 1);
                whir.ajax.post("<%=SysPath%>Handler/Common/Subject.aspx?_action=SortColumn",
                    {
                        data: {
                            apidsort: result
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

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-heading">
                <%=GuangliLink[0]%>
            </div>
            <div class="panel-body">
                <div class="actions btn-group">
                    <%if (IsDevUser)
                    { %><a class="btn btn-white" href='SubjectColumn_Edit.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>'>
                        <%="添加栏目".ToLang()%></a>
                    <%} %>
                    <% if (IsMove)
                        { %>
                    <a class="btn btn-white" href='ColumnMove.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>'>
                        <%="栏目移动".ToLang()%></a>
                    <%  } %>
                    <%if (IsDevUser)
                        { %>
                    <a class="btn btn-white" href="<%=AddLink[1] %>">
                        <%=AddLink[0]%></a>
                    <%} %>
                </div>
                <table id="tableColumn" width="100%" class="controller table table-bordered table-noPadding">
                    <thead>
                        <tr data-level="header" class="lheader contracted">
                            <th style="width: 40%">
                                <%="栏目名称".ToLang()%>
                            </th>
                            <th style="width: 10%">
                                <%="功能模块".ToLang()%>
                            </th>
                            <th style="width: 10%">
                                <%="排序".ToLang()%>
                            </th>

                            <th style="width: 40%">
                                <%="管理操作".ToLang()%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <% foreach (var column in Columns)
                            {%>
                        <tr data-level="<%=column.LevelNum %>" id="level_<%=column.LevelNum %>_<%=column.ColumnId%>">
                            <td>
                                <%=column.ColumnName.Replace("├─", "").Replace("│", "").Replace("└─", "").Replace("　", "")%>
                                <% if (IsDevUser)
                                    {%>
                                 &nbsp;-&nbsp;<span class="text-danger pull-right"><%=column.ColumnId%></span>
                                <%  } %>
                            </td>
                            <td>
                                <% Model model = ServiceFactory.ModelService.SingleOrDefault<Model>(column.ModelId);%>
                                <% if (model != null)
                                    {%>
                                <%=model.ModelName%>
                                <% }%>
                            </td>
                            <td>
                                <input class="form-control apid_sort" flag="0" style="width: 120px;" columnid='<%=column.ColumnId%>'
                                    value='<%=column.Sort%>' />
                            </td>
                            <td>
                                <div class="btn-group">
                                    <%if (IsDevUser)
                                    {%>
                                    <a class="btn btn-sm btn-white" href='SubjectColumn_edit.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&parentid=<%=column.ColumnId %>'>
                                        <%="添加子栏目".ToLang()%></a>
                                    <% }%>
                                    <% string editlink = ""; string formlink = "";
                                        if (!column.OutUrl.IsEmpty())
                                            editlink = "subjectcolumn_link.aspx?subjecttypeid={0}&subjectclassid={1}&columnid={2}".FormatWith(
                                                    SubjectTypeId, SubjectClassId, column.ColumnId
                                                );
                                        else
                                            editlink = "subjectcolumn_edit.aspx?subjecttypeid={0}&subjectclassid={1}&columnid={2}&subjectId={3}&Type=base".FormatWith(
                                                    SubjectTypeId, SubjectClassId, column.ColumnId, SubjectId
                                                );
                                        formlink = "subjectformlist.aspx?subjecttypeid={0}&subjectclassid={1}&columnid={2}&subjectId={3}".FormatWith(
                                            SubjectTypeId, SubjectClassId, column.ColumnId, SubjectId
                                            );

                                    %>
                                    <% if (IsRoleHaveSubjectRes("subjectcolumn", "栏目修改", column.ColumnId, column.SiteId, SubjectId))
                                        {%>
                                    <a class="btn btn-sm btn-white" href='<%=editlink %>'><%="编辑".ToLang()%></a>
                                    <%}%>
                                    <%if (column.ModelId != 0 && IsDevUser)
                                        {%>
                                    <a class="btn btn-sm btn-white" href='<%=formlink %>'><%="表单管理".ToLang()%></a>
                                    <%} %>
                                    <% if (IsRoleHaveSubjectRes("subjectcolumn", "栏目删除", column.ColumnId, column.SiteId, SubjectId))
                                        { %>
                                    <a class="btn btn-sm text-danger border-normal" href="javascript:;" onclick="DeleteColumn(<%=column.ColumnId %>)"><%="删除".ToLang()%></a>
                                    <%} %>
                                </div>
                            </td>
                        </tr>
                        <%   } %>
                    </tbody>
                </table>
                <%if (IsDevUser)
                        { %>
                   <a class="btn btn-white" href="javascript:;" onclick="getSort()"><%="排序".ToLang()%></a>
                    <%} %>
               
            </div>
        </div>
    </div>
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery.tabelizer.js"></script>
    <script type="text/javascript">
        //树形表格
        var tableColumn = $('#tableColumn').tabelize({
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
        //打开复制栏目页面 暂时不开放
        function openCopy(columnId) {
            <%--<a class="btn btn-sm btn-primary" href="javascript:;" onclick="openCopy('<%=column.ColumnId%>');"><%="复制".ToLang()%></a>--%>
            //whir.dialog.frame('<%="目标栏目信息".ToLang()%>', "<%=SysPath%>module/column/sitecolumn_copy.aspx?columnid=" + columnId, null, 500, 400);
        }

        function DeleteColumn(id) {
            if (!id) {
                whir.toastr.warning("<%="参数错误".ToLang() %>");
            } else {
                whir.dialog.confirm('<%="确定删除该记录吗？".ToLang() %>',
                    function () {
                        whir.ajax.post("<%=SysPath%>Handler/Common/Subject.aspx?_action=DeleteColumn",
                            {
                                data: {
                                    id: id
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
                        whir.dialog.remove();
                        return false;

                    });

            }
            return false;
        }

    </script>
</asp:Content>
