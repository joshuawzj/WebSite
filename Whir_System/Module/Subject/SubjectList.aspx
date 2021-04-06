<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="subjectlist.aspx.cs" Inherits="Whir_System_Module_Subject_SubjectList" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="<%=SysPath%>Res/assets/js/tree/tabelizer/tabelizer.min.css" media="all" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%=GuangliLink[0]%></div>

            <div class="panel-body">
                <% if (IsDevUser)
                    {%>
                <div class="actions btn-group">
                    <a class="btn btn-white" href="<%=AddLink[1] %>"><%=AddLink[0]%></a>
                </div>
                <%   } %>


                <div class="tableCategory-table-body">
                    <table id="tableColumn" width="100%" class="controller table table-bordered table-noPadding">
                        <thead>
                            <tr data-level="header" class="lheader contracted">
                                <th style="width: 45%"><%="名称".ToLang()%></th>
                                <th style="width: 10%"><%="排序".ToLang()%></th>
                                <th style="width: 45%"><%="操作".ToLang()%></th>
                            </tr>
                        </thead>
                        <tbody>
                            <% int leve = 1;
                            %>
                            <% foreach (var column in SubjectClass)
                                {
                                    int levesub = 2;
                                    int levecln = 3;
                                    int nodeleve = 1;
                            %>
                            <tr data-level="<%=leve %>" id="level_<%=leve %>_<%=column.SubjectClassId%>">
                                <td>
                                    <%=column.SubjectClassName%> 
                                </td>
                                <td>
                                    <input class="form-control apid_sort" flag="0" style="width: 120px;" apid='<%=column.SubjectClassId%>' value='<%=column.Sort%>' />
                                </td>
                                <td>
                                    <div class="btn-group">
                                        <% if (IsRoleHaveSubjectRes("subjectclass", "修改", column.SubjectClassId, column.SiteId, 0))
                                            {%>
                                        <a class="btn btn-sm btn-white" href='SubjectClass_Edit.aspx?subjecttypeid=<%= column.SubjectTypeId %>&subjectclassid=<%=column.SubjectClassId %>'><%="编辑".ToLang()%></a>

                                        <%  } %>
                                        <% string addName = SubjectTypeId == 1 ? "添加子站".ToLang() : "添加专题".ToLang(); %>
                                        <% if (IsRoleHaveSubjectRes("subjectclass", SubjectTypeId == 1 ? "添加子站" : "添加专题", column.SubjectClassId, column.SiteId, 0))
                                            {%>
                                        <a class="btn btn-sm btn-white" href='Subject_Edit.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%=column.SubjectClassId %>'><%=addName %></a>

                                        <%  } %>
                                        <% if (IsDevUser)
                                            { %>
                                        <a class="btn btn-sm btn-white" href='SubjectColumnlist.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%=column.SubjectClassId %>&time=<%=DateTime.Now.Millisecond %>'><%="管理栏目".ToLang()%></a>
                                        <a class="btn btn-sm text-danger border-normal" href="javascript:;" onclick="DeleteSubjectClass(<%=column.SubjectClassId %>)"><%="删除".ToLang()%></a>
                                        <% } %>
                                    </div>
                                </td>
                            </tr>
                            <% foreach (var item in ServiceFactory.SubjectService.GetListBySubjectClassId(column.SubjectClassId))
                                {

                            %>
                            <tr data-level="<%=levesub%>" id="level_<%=levesub%>_<%=item.SubjectId%>">
                                <td>
                                    <%=item.SubjectName%>  <% 
                                  if (CurrentUser.IsDeveloper)
                                       {%>
                                    &nbsp;-&nbsp;<span class="text-danger pull-right"><%=item.SubjectId%></span>
                                    <%  } %>
                                </td>
                                <td>
                                    <input class="form-control apid_sort" flag="1" style="width: 120px;" apid='<%=item.SubjectId%>' value='<%=item.Sort%>' />
                                </td>
                                <td>
                                    <div class="btn-group">
                                        <% if (IsRoleHaveSubjectRes("subject", "修改", item.SubjectId, column.SiteId, item.SubjectId))
                                            {%>
                                        <a class="btn btn-sm btn-white" href='Subject_Edit.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%=column.SubjectClassId %>&subjectid=<%=item.SubjectId %>'><%="编辑".ToLang()%></a>

                                        <%  } %>
                                        <% if (IsRoleHaveSubjectRes("subject", "SEO设置", item.SubjectId, column.SiteId, item.SubjectId))
                                            {%>
                                        <a class="btn btn-sm btn-white" href='SubjectSeo.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%=column.SubjectClassId %>&subjectid=<%=item.SubjectId %>'><%="SEO设置".ToLang()%></a>

                                        <%  } %>

                                        <% if (IsRoleHaveSubjectRes("subject", "删除", item.SubjectId, column.SiteId, item.SubjectId))
                                            { %>
                                        <a class="btn btn-sm text-danger border-normal" href="javascript:;" onclick="DeleteSubject(<%=item.SubjectId %>)"><%="删除".ToLang()%></a>
                                        <% } %>
                                    </div>
                                </td>
                            </tr>

                            <% foreach (var cl in GetNodeColumns(ServiceFactory.ColumnService.GetSubjectColumnList(0, item.SubjectClassId, item.SubjectId), 0, levecln))
                                {%>
                            <tr data-level="<%=cl.LevelNum%>" id="level_<%=cl.LevelNum%>_<%=cl.ColumnId%>_<%=nodeleve %>">
                                <td>
                                    <%=cl.ColumnName.Replace("└","").Replace("─", "").Replace("├", "").Replace("│", "").Replace("　", "")%>
                                    <% if (CurrentUser.IsDeveloper)
                                        {%>
                                &nbsp;-&nbsp;<span class="text-danger pull-right"><%=cl.ColumnId%></span>
                                    <%  } %>
                               
                                </td>
                                <td>
                                    <input class="form-control apid_sort" flag="2" style="width: 120px;" apid='<%=cl.ColumnId%>' value='<%=cl.Sort%>' />
                                </td>
                                <td>
                                    <div class="btn-group">
                                        <% if (IsRoleHaveSubjectRes("subjectcolumn", "栏目修改", cl.ColumnId, cl.SiteId, item.SubjectId))
                                            {%>
                                        <a class="btn btn-sm btn-white" href='<%="SubjectColumn_edit.aspx?subjecttypeid={0}&subjectclassid={1}&columnid={2}&subjectid={3}".FormatWith(SubjectTypeId,item.SubjectClassId,cl.ColumnId,item.SubjectId ) %>&type=base'><%="编辑".ToLang()%></a>

                                        <%  } %>
                                    </div>
                                </td>
                            </tr>

                            <% nodeleve++;
                                } %>

                            <% 
                                } %>

                            <% 
                                } %>
                        </tbody>
                    </table>
                </div>
                <% if (IsDevUser||IsSuperUser)
                    {%>
                <a class="btn btn-white" href="javascript:;" onclick="collectApid_Sort()"><%="排序".ToLang()%></a>
                <%  } %>
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

        function DeleteSubject(id) {
            if (!id) {
                whir.toastr.warning("<%="参数错误".ToLang() %>");
            } else {
                whir.dialog.confirm('<%="确定删除该记录吗？".ToLang() %>',
                    function () {
                        whir.ajax.post("<%=SysPath%>Handler/Common/Subject.aspx?_action=DeleteSubject",
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

        function DeleteSubjectClass(id) {
            if (!id) {
                whir.toastr.warning("<%="参数错误".ToLang() %>");
            } else {
                whir.dialog.confirm('<%="确定删除该记录吗？".ToLang() %>',
                    function () {
                        whir.ajax.post("<%=SysPath%>Handler/Common/Subject.aspx?_action=DeleteSubjectClass",
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

        //获取主键Id
        function collectApid_Sort() {
            var apidsort = "";
            $(".apid_sort")
                .each(function () {
                    apidsort += $(this).attr("flag") + "|" + $(this).attr("apid") + "|" + $(this).val() + ",";
                });
            if (apidsort == "") {
                whir.toastr.info('<%="无数据可排序".ToLang() %>');
                return false;
            }
            whir.ajax.post("<%=SysPath%>Handler/Common/Subject.aspx?_action=SortSubject",
                {
                    data: {
                        apidsort: apidsort
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
        }

    </script>
</asp:Content>
