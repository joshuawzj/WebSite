<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/whir_system/master/SystemMasterPage.master" CodeFile="arealist.aspx.cs" Inherits="whir_system_module_area_arealist" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Label.Dynamic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <link href="<%=SysPath%>Res/assets/js/tree/tabelizer/tabelizer.min.css" media="all" rel="stylesheet" type="text/css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="系统区域设置".ToLang()%></div>
            
            <div class="panel-body">
                <div class="actions btn-group">
                    <% if (IsDevUser)
                       {%>
                            <a  class="btn btn-white" href="Area_Edit.aspx"><%="添加区域".ToLang()%></a>
                       <%  } %>
                </div>
                <div class="tableCategory-table-body AreaList-table">
                <table id="tableColumn" width="100%" class="controller table table-bordered table-noPadding">
                    <thead>
                        <tr data-level="header" class="">
                            <th><%="区域名称".ToLang()%></th>
                            <th width="80px"><%="排序".ToLang()%></th>
                            <th><%="操作".ToLang()%></th>
                        </tr>
                    </thead>
                    <tbody id="tbdColumnList">
                    <% foreach (var area in Areaslist)
                            {
                        %>
                         <tr data-level="<%=area.LevelNum%>" id="level_<%=area.LevelNum%>_<%=area.Id%>">
                            <td>
                                <%=area.Name.ToString().Replace("├─", "").Replace("│", "").Replace("└─", "").Replace("　", "")%>
                                <% if (IsDevRole)
                                    {
                                %>
                                &nbsp;-&nbsp;<span class="text-danger"><%=area.Id%></span>
                                <%  } %>
                            </td>
                            <td>
                                <input class="form-control" type="text" style="width: 120px;" areaid="<%=area.Id%>" value='<%=area.Sort%>'/>
                            </td>
                            <td>
                                <div class="btn-group">
                                    <a class="btn btn-sm btn-white" href='Area_Edit.aspx?id=<%=area.Id  %>&pid=<%=area.Pid %> '><%="编辑".ToLang()%></a>
                                    <a class="btn btn-sm text-danger border-normal" href="javascript:;" onclick="Delete(<%=area.Id %>)"><%="删除".ToLang()%></a>
                                </div>
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
                
            },
            onAfterRowClick: function () {
                //console.log('onAfterRowClick');
            }
        });

        //获取排序的值, 并赋值给hidSort
        function getSort() {
            var result = "";
            $("#tbdColumnList").find("input[type='text']").each(function () {
                var ID = $(this).attr("areaid");
                var sort = $(this).val();
                result += ID + "|" + sort + ",";
            });

            if (result != '') {
                result = result.substring(0, result.length - 1);
                whir.ajax.post("<%=SysPath%>Handler/Developer/Column.aspx",
                    {
                        data: {
                            _action: "SortArea",
                            Sort: result
                        },
                        success: function(response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                 whir.toastr.success(response.Message,true,false);
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
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function() {
                whir.ajax.post("<%=SysPath%>Handler/Developer/Column.aspx",
                    {
                        data: {
                            _action: "DeleteArea",
                            Id: id
                        },
                        success: function(response) {
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
