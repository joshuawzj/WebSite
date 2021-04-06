<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="modellist.aspx.cs" Inherits="Whir_System_Module_Developer_ModelList" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <link href="<%=SysPath%>Res/assets/js/tree/tabelizer/tabelizer.min.css" media="all" rel="stylesheet" type="text/css">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading"><%="功能模型".ToLang()%></div>
            <div class="panel-body">
                <div class="actions">
                    <a class="btn btn-white" href="ModelEdit.aspx"><%="添加模型".ToLang()%></a>
                </div>
                <div class="All_list tableCategory-table-body">
                <table id="tableModel" width="100%" class="controller table table-bordered table-noPadding">
                    <thead>
                        <tr>
                            <th><%="功能模块名称".ToLang()%></th>
                            <th><%="功能模块标识".ToLang()%></th>
                            <th><%="表名".ToLang()%></th>
                            <th><%="操作".ToLang()%></th>
                        </tr>
                    </thead>
                    <tbody>
                        <% foreach (var model in ModelTreeList)
                           {
                        %>
                        <tr data-level="<%=model.ParentId==0?"1":"2"%>" id="level_<%=model.ParentId==0?"1":"2"%>_<%=model.ModelId%>">
                            <td><%=model.ModelName%></td>
                            <td><%=model.ModuleMark%></td>
                            <td><%=model.TableName%></td>
                            <td>
                                <div class="btn-group">
                                    <a class="btn btn-sm btn-white" href="ModelEdit.aspx?ModelId=<%=model.ModelId%>"><%="编辑".ToLang()%></a>
                                    <a class="btn btn-sm btn-white" href="FieldList.aspx?ModelId=<%=model.ModelId%>"><%="字段管理".ToLang()%></a>
                                    <a class="btn btn-sm btn-white" onclick="entity(<%=model.ModelId %>)"><%="生成实体".ToLang()%></a>
                                    <a class="btn btn-sm text-danger border-normal" onclick="del(<%=model.ModelId %>)"><%="删除".ToLang()%></a>
                                </div>
                            </td>
                        </tr>
                        <% } %>
                    </tbody>
                </table>
                </div>
            </div>
        </div>
    </div>
    
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery.tabelizer.js"></script>    <script type="text/javascript">
        //树形表格
        var tableModel = $('#tableModel').tabelize({
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
        //删除
        function del(modelId) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>",
                function () {
                    whir.ajax.post('<%= SysPath%>Handler/Developer/Model.aspx', {
                        data: {
                            _action: "Del",
                            ModelId: modelId,
                        },
                        success: function (result) {
                            if (result.Status) {
                                whir.toastr.success(result.Message, true, false, "ModelList.aspx");

                            } else {
                                whir.toastr.warning(result.Message);
                            }
                            whir.loading.remove();
                        }
                    });
                });
        }
        //创建实体
        function entity(modelId) {
            whir.ajax.post('<%= SysPath%>Handler/Developer/Model.aspx', {
                data: {
                    _action: "Entity",
                    ModelId: modelId,
                },
                success: function (result) {
                    if (result.Status) {
                        whir.toastr.success(result.Message);

                    } else {
                        whir.toastr.warning(result.Message);
                    }
                    whir.loading.remove();
                }
            });
        }
    </script>
</asp:content>
