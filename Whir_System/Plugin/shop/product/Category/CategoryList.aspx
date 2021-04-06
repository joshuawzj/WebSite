<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="categorylist.aspx.cs" Inherits="whir_system_Plugin_shop_product_category_categorylist" %>

<%@ Register Src="~/whir_system/Plugin/shop/common/HeadContainer.ascx" TagName="HeadContainer"
    TagPrefix="uc1" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="<%=SysPath%>Res/assets/js/tree/tabelizer/jquery.tabelizer.js"></script>
    <link href="<%=SysPath%>Res/assets/js/tree/tabelizer/tabelizer.min.css" media="all" rel="stylesheet" type="text/css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div class="content-wrap">
            <div class="space15"></div>
            <div class="panel">
                <div class="panel-heading"><%="商品类别管理".ToLang()%></div>
                <div class="panel-body">
                    <div class="actions btn-group">
                        <a class="btn btn-white" href="category_edit.aspx" ><%="添加商品类别".ToLang()%></a>
                    </div>
                    <div class="tableCategory-table-body">
                    <table id="tableCategory" width="100%" class="controller table table-bordered table-noPadding">
                        <thead>
                            <tr data-level="header" class="">
                                <th><%="类别名称".ToLang()%></th>
                                <th><%="排序".ToLang()%></th>
                                <th><%="操作".ToLang()%></th>
                            </tr>
                        </thead>
                        <tbody>

                            <% foreach (var item in CategoryTree)
                               {
                            %>
                            <tr data-level="<%=item.ParentPath.ToInt(0)%>" id="level_<%=item.ParentPath.ToInt(0)%>_<%=item.CategoryID%>">
                                <td><%=item.CategoryName%></td>
                                <td>
                                    <input categoryid="<%=item.CategoryID %>" type="text" class="form-control" style="width: 135px;" value="<%=item.Sort %>" /></td>
                                <td class="">
                                    <div class="btn-group">
                                        <a class="btn btn-sm btn-white" href="category_edit.aspx?categoryid=<%=item.CategoryID%>"><%="编辑".ToLang()%></a>
                                        <a class="btn btn-sm text-danger border-normal" table-action="delete" table-data="<%=item.CategoryID%>"><%="删除".ToLang()%></a>
                                    </div>
                                </td>
                            </tr>
                            <%  } %>
                        </tbody>
                    </table>
                    </div>
                    <a id="btn_Sort" class="btn btn-sm btn-white" href="javascript:;"><%="排序".ToLang()%></a>
                   
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        //树形表格
        var tableMenu = $('#tableCategory').tabelize({
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
            var categoryID = $(this).attr("table-data");
            var dialog = whir.dialog.confirm("<%="确认删除吗？".ToLang()%>", function () {
                whir.loading.show();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/CategoryForm.aspx", {
                    data: {
                        _action: "DelCategory",
                        categoryID: categoryID
                    },
                    success: function (response) {
                        if (response.Status == true) {
                            whir.toastr.success(response.Message,true);
                            whir.dialog.remove();
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    }
                });
            });
        });
        //获取排序的值, 并赋值给hidSort
        $("#btn_Sort").click(function () {
            var result = "";
            $("#tableCategory input[type='text']").each(function () {
                var columnID = $(this).attr("categoryid");
                var sort = $(this).val();
                result += columnID + "|" + sort + ",";
            })
            if (result != '') {
                result = result.substring(0, result.length - 1);
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/CategoryForm.aspx", {
                    data: {
                        _action: "SortCategory",
                        strSort: result
                    },
                    success: function (response) {
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    }
                });
            }
        })
    </script>
</asp:Content>
