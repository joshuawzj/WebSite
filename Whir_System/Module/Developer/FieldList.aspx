<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="fieldlist.aspx.cs" Inherits="whir_system_module_developer_modelfieldlist" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <form runat="server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="ModeLlist.aspx"><%="功能模型".ToLang()%></a></li>
                    <li class="active">
                        <a href='FieldList.aspx?ModelId=<%= ModelId %>'><%=Model.ModelName+ " - 字段".ToLang()%></a>
                    </li>
                </ul>
                <div class="space15"></div>
                <div class="actions btn-group">
                      <a href='Field_Edit.aspx?modelid=<%= ModelId %>' class="btn btn-white"><%="添加字段".ToLang()%></a>
                </div>
                <div class="All_list" style="clear:both;">
                <div class="tableCategory-table-body">
                    <table width="100%" class="controller table table-bordered table-noPadding">
                        <tr style="height: 40px">
                            <th width="70px">Id</th>
                            <th>
                                <%="数据库字段名".ToLang()%>
                            </th>
                            <th>
                                <%="字段名称".ToLang()%>
                            </th>
                            <th>
                                <%="字段类型".ToLang()%>
                            </th>
                            <th>
                                <%="是否系统字段".ToLang()%>
                            </th>
                            <th>
                                <%="是否隐藏".ToLang()%>
                            </th>
                            <th>
                                <%="通用标题".ToLang()%>
                            </th>
                            <th>
                                <%="操作".ToLang()%>
                            </th>
                        </tr>
                        <% foreach (var field in AllFieldList)
                           {
                        %>
                        <tr  style="height: 40px">
                        <td>
                            <%=field.FieldId %>
                        </td>
                        <td>
                            <%=field.FieldName %>
                        </td>
                        <td>
                            <%=field.FieldAlias %>  
                        </td>
                        <td>
                                <%=Whir.Service. ServiceFactory.FieldService.GetFieldTypeName(field.FieldType)%> 
                        </td>
                        <td>
                            <%=GetTrueOrFalseImg(field.IsSystemField) %>
                        </td>
                        <td>
                            <%= GetTrueOrFalseImg(field.IsHidden) %>
                        </td>
                        <td>
                            <%= GetTrueOrFalseImg(field.IsWorkFlowTitle) %>
                        </td>
                        <td >
                            <div class="btn-group">
                            <%  if (field.FieldType == 1 || field.FieldType == 2 || field.FieldType == 3)
                                {
                                if (field.IsWorkFlowTitle)
                                {%> 
                                <a class="btn btn-sm text-danger border-normal" onclick="set('<%=field.FieldId %>',0)"><%="取消为通用标题".ToLang()%></a>
                                <%}
                                    else
                                    {%>
                                    <a class="btn btn-sm text-success border-normal" onclick="set('<%=field.FieldId %>',1)"><%="设置为通用标题".ToLang()%></a>
                                    <%
                                    }
                                }%>
                                <a class="btn btn-sm btn-white" href="Field_Edit.aspx?FieldId=<%=field.FieldId%>&ModelID=<%=ModelId%>"><%="编辑".ToLang()%></a>
                                <%if(!field.IsSystemField)
                                  { %>
                                 <a class="btn btn-sm text-danger border-normal" onclick="del(<%=field.FieldId%>)"><%="删除".ToLang()%></a>
                                <%} %>
                            </div>
                        </td>
                      </tr>
                    <% }
                %>
                </table>
                </div>
                </div>
            </div>
        </div>
    </div>
    </form>
    <script>
        //删除
        function del(fieldId) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                function () {
                    whir.ajax.post('<%= SysPath%>Handler/Developer/Field.aspx', {
                         data: {
                             _action: "Del",
                             FieldId: fieldId,
                         },
                         success: function (result) {
                             if (result.Status) {
                                 whir.toastr.success(result.Message, true, false);
                             } else {
                                 whir.toastr.warning(result.Message);
                             }
                             whir.loading.remove();
                         }
                     });
                 });
         }
        //设置为通用标题
        function set(fieldId,value) {
            whir.ajax.post('<%= SysPath%>Handler/Developer/Field.aspx', {
                data: {
                    _action: "Update",
                    FieldId: fieldId,
                    IsWorkFlowTitle: value
                },
                success: function (result) {
                    if (result.Status) {
                        whir.toastr.success(result.Message, true, false);
                    } else {
                        whir.toastr.warning(result.Message);
                    }
                    whir.loading.remove();
                }
            });
        }

    </script>
</asp:content>
