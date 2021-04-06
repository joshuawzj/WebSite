<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="searchlist.aspx.cs" Inherits="whir_system_Plugin_shop_product_search_searchlist" %>
<%@ Import Namespace="Whir.Language" %>
<%@ Register Src="../../common/HeadContainer.ascx" TagName="HeadContainer" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
       
    <style type="text/css">
        .site_select li
        {
            cursor:pointer;
        }
        .bg_color
        {
            color: #fff;
            background-color: #428bca;
        }
    </style>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server">
        <div class="content-wrap">
            <div class="space15"></div>
            <div class="panel">
                <div class="panel-heading"><%="搜选项管理".ToLang()%></div>
                <div class="panel-body">
                    <div class="actions btn-group">
                        <a class="btn btn-white" href="search_edit.aspx" aria-expanded="true"><i class="glyphicon glyphicon-plus"></i>&nbsp;<%="添加搜选项组".ToLang()%></a>
                    </div>
                    <div class="panel-body" style="padding: 0;">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="panel panel-default">
                                    <div class="panel-heading"><%="搜选项组".ToLang()%></div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="site_select" id="searchList"></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-9">
                                <div class="panel panel-default">
                                    <div class="panel-heading"  id="editSearch"><%="搜选项值".ToLang()%></div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="btn-group btn-group-attr-wap">
                                                    <a class="btn btn-white" onclick="addSearchValue();">
                                                        <%="添加搜选项值".ToLang()%>
                                                    </a>
                                                    <a class="btn btn-white" onclick="editSearch();">
                                                        <%="编辑此搜选项组".ToLang()%>
                                                    </a>
                                                    <a class="btn text-danger border-normal" onclick="delSearch();">
                                                        <%="删除此搜选项组".ToLang()%>
                                                    </a>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <table id="Common_Table"></table>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
         <input type="hidden" id="txtSearchID" value="" name="saveSearchID" />
    </form>
    <script type="text/javascript">
        $(document).ready(function () {

            $.get("<%=SysPath%>Handler/Plugin/shop/SearchForm.aspx?_action=GetList&limit=100&offset=1",
           function (data) {
               if (data) {
                   var rows = JSON.parse(data)
                   var html = "";
                   for (var i = 0; i < rows.rows.length; i++) {
                       if (i == 0) {
                           html += "<li class='list-group-item bg_color' value=" + rows.rows[i].SearchID + "  onclick='getAttrValue(this)'>";
                           html += rows.rows[i].SearchName + "</li>";
                           $("#txtSearchID").val(rows.rows[i].SearchID);
                           $("#editSearch").html(rows.rows[i].SearchName);
                           $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Plugin/shop/SearchForm.aspx?_action=GetSearchValueList&searchId=" + rows.rows[i].SearchID });
                       } else {
                           html += "<li class='list-group-item ' value=" + rows.rows[i].SearchID + "  onclick='getAttrValue(this)'>" + rows.rows[i].SearchName + "</a></li>";
                       }
                   }
                   if (html != "")
                       html = "<ul class='list-group'>" + html + "</ul>";
                   $("#searchList").html(html);

               }
           });
        });

        function getAttrValue(o) {

            $("#editSearch").html($(o).html());
            var id = $(o).attr("value");
            $("#txtSearchID").val(id);
            $(".site_select li").removeClass("bg_color");
            $(o).addClass("bg_color");

            $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Plugin/shop/SearchForm.aspx?_action=GetSearchValueList&searchId=" + id });
        }

        var $table = $('#Common_Table'), _that;
       

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Plugin/shop/SearchForm.aspx?_action=GetSearchValueList&searchId=0",
                dataType: "json",
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                pageSize: 10,
                onLoadSuccess: function () {
                    //设置样式 后期需修改
                    //SetTableStyleEvent();
                },
                onLoadError: function (data, mes) {
                    if (mes && mes.responseText) {
                        whir.toastr.warning(mes.responseText);
                    } else {
                        whir.toastr.error("<%="获取数据失败！".ToLang()%>");
                    }
                },
                onPageChange: function () {
                    var path = $("#txtSearchID").val();
                    $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Plugin/shop/SearchForm.aspx?_action=GetSearchValueList&searchId=" + path });
                },
                onColumnSwitch: function () {
                    //SetTableStyleEvent();
                },
                SelectColumnEvent: function () {

                },
                columns: [
                    { title: '<%="名称".ToLang()%>', field: 'SearchValueName', align: 'left', valign: 'middle' },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 135, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ]
            });
            whir.loading.remove();
        }

        initTable();
        function GetOperation(value, row, index) {
            var html = "";
            var searchid = $("#txtSearchID").val();
            html += '<div class="btn-group">';
            html += '<a class="btn btn-white" href="searchvalue_edit.aspx?searchid=' + searchid + '&searchvalueid=' + row.SearchValueID + '"><%="编辑".ToLang()%></a>';
            html += '<a id="btnDel" class="btn text-danger border-normal" onclick="delv(' + row.SearchValueID + ',' + searchid + ')" ><%="删除".ToLang()%></a>';
            html += '</div>';
            return html;
        }
        //添加搜选项值
        function addSearchValue() {
            var searchid = $("#txtSearchID").val();
            if (searchid == '') {
                whir.toastr.error("<%="请选择要添加的搜选项组名称".ToLang()%>");
                return;
            }
            window.location.href = '<%=SysPath%>Plugin/shop/product/search/searchvalue_edit.aspx?searchid=' + searchid;
        }
        //编辑搜选项组
        function editSearch() {
            var searchid = $("#txtSearchID").val();
            if (searchid == '') {
                whir.toastr.error("<%="请选择要编辑的搜选项组名称".ToLang()%>");
                return;
            }
            window.location.href = '<%=SysPath%>Plugin/shop/product/search/search_edit.aspx?searchid=' + searchid;
        }
        //搜选项组删除
        function delSearch() {
            var searchid = $("#txtSearchID").val();
            if (searchid == '') {
                whir.toastr.error("<%="请选择要删除的搜选项组名称".ToLang()%>");
                return;
            }
            var dialog = whir.dialog.confirm("<%="确认删除吗？".ToLang()%>", function () {
                whir.loading.show();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/SearchForm.aspx", {
                    data: {
                        _action: "DelSearch",
                        searchid: searchid
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
        }
        //搜选项值删除事件
        function delv(attrValId, attrId) {
            var ids = attrValId + '|' + attrId;
            var dialog = whir.dialog.confirm("<%="确认删除吗？".ToLang()%>", function () {
                whir.loading.show();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/SearchForm.aspx", {
                    data: {
                        _action: "DelSearchValue",
                        ids: ids
                    },
                    success: function (response) {
                        if (response.Status == true) {
                            whir.toastr.success("<%="操作成功".ToLang()%>");
                            window.location.href = response.Message;
                        } else {
                            whir.toastr.error(response.Message);
                        }
                        whir.loading.remove();
                    }
                });
            });
        }
    </script>
</asp:Content>
