<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="attrlist.aspx.cs" Inherits="whir_system_Plugin_shop_product_attr_attrlist" %>

<%@ Register Src="../../common/HeadContainer.ascx" TagName="HeadContainer" TagPrefix="uc1" %>
<%@ Import Namespace="Whir.Language" %>
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
                <div class="panel-heading"><%="规格管理".ToLang()%></div>
                <div class="panel-body">
                    <div class="actions btn-group">
                        <a class="btn btn-white" href="attr_edit.aspx" aria-expanded="true"><i class="glyphicon glyphicon-plus"></i>&nbsp;<%="添加规格组".ToLang()%></a>
                    </div>

                    <div class="panel-body" style="padding: 0;">
                        <div class="row">
                            <div class="col-md-3">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <%="规格组".ToLang()%>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="site_select" id="attrList">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-9 ">
                                <div class="panel panel-default">
                                    <div class="panel-heading" id="editAttr">
                                        <%="规格值".ToLang()%>
                                    </div>
                                    <div class="panel-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="btn-group btn-group-attr-wap">
                                                    <a class="btn btn-white" onclick="addAttrValue();"><%="添加规格值".ToLang()%>
                                                    </a>
                                                    <a class="btn btn-white" onclick="editAttr();"><%="编辑此规格组".ToLang()%>
                                                    </a>
                                                    <a class="btn text-danger border-normal" onclick="delAttr();"><%="删除此规格组".ToLang()%>
                                                    </a>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <table id="Common_Table">
                                        </table>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" id="txtAttrID" value="" name="saveAttrID" />
    </form>
    <script type="text/javascript">
        $(document).ready(function () {

            $.get("<%=SysPath%>Handler/Plugin/shop/AttrForm.aspx?_action=GetList&limit=100&offset=1",
           function (data) {
               if (data) {
                   var rows = JSON.parse(data)
                   var html = "";
                   for (var i = 0; i < rows.rows.length; i++) {
                       if (i == 0) {
                           html += "<li class='list-group-item bg_color' value=" + rows.rows[i].AttrID + "  onclick='getAttrValue(this)'>";
                           html +=  rows.rows[i].SearchName + "</li>";
                           $("#txtAttrID").val(rows.rows[i].AttrID);
                           $("#editAttr").html(rows.rows[i].SearchName);
                           $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Plugin/shop/AttrForm.aspx?_action=GetAttrValueList&attrId=" + rows.rows[i].AttrID });
                       } else {
                           html += "<li class='list-group-item ' value=" + rows.rows[i].AttrID + "  onclick='getAttrValue(this)'>" + rows.rows[i].SearchName + "</a></li>";
                       }
                   }
                   if (html != "")
                       html = "<ul class='list-group'>" + html + "</ul>";
                   $("#attrList").html(html);

               }
           });
        });

        function getAttrValue(o) {

            $("#editAttr").html($(o).html());
            var id = $(o).attr("value");
            $("#txtAttrID").val(id);
            $(".site_select li").removeClass("bg_color");
            $(o).addClass("bg_color");
            
            $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Plugin/shop/AttrForm.aspx?_action=GetAttrValueList&attrId=" + id });
        }

        var $table = $('#Common_Table'), _that;

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Handler/Plugin/shop/AttrForm.aspx?_action=GetAttrValueList&attrId=0",
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
                        whir.toastr.error("<%="获取数据失败！".ToLang() %>");
                    }
                },
                onPageChange: function () {
                    var Id = $("#txtAttrID").val();
                    $table.bootstrapTable('refreshOptions', { url: "<%=SysPath%>Handler/Plugin/shop/AttrForm.aspx?_action=GetAttrValueList&attrId=" + Id });
                },
                onColumnSwitch: function () {
                    //SetTableStyleEvent();
                },
                SelectColumnEvent: function () {

                },
                columns: [
                    { title: '<%="名称".ToLang()%>', field: 'AttrValueName', align: 'left', valign: 'middle' },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 135, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ]
            });
            whir.loading.remove();
        }

        initTable();
        function GetOperation(value, row, index) {
            var html = "";
            var attrid = $("#txtAttrID").val();
            html += '<div class="btn-group">';
            html += '<a class="btn btn-white" href="attrvalue_edit.aspx?attrid=' + attrid + '&attrvalueid=' + row.AttrValueID + '"><%="编辑".ToLang()%></a>';
            html += '<a id="btnDel" class="btn text-danger border-normal" onclick="delv(' + row.AttrValueID + ',' + attrid + ')" ><%="删除".ToLang()%></a>';
            html += '</div>';
            return html;
        }
        //添加规格组
        function addAttrValue() {
            var attrid = $("#txtAttrID").val();
            if (attrid == '') {
                whir.toastr.error("<%="请选择要添加的规格组名称".ToLang()%>");
                return;
            }
            window.location.href = '<%=SysPath%>Plugin/shop/product/attr/attrvalue_edit.aspx?attrid=' + attrid;
        }
        //编辑规格组
        function editAttr() {
            var attrid = $("#txtAttrID").val();
            if (attrid == '') {
                whir.toastr.error("<%="请选择要编辑的规格组名称".ToLang()%>");
                return;
            }
            window.location.href = '<%=SysPath%>Plugin/shop/product/attr/attr_edit.aspx?attrid=' + attrid;
        }
        //删除规格组
        function delAttr() {
            var attrid = $("#txtAttrID").val();
            if (attrid == '') {
                whir.toastr.error("<%="请选择要删除的规格组名称".ToLang()%>");
                return;
            }
            var dialog = whir.dialog.confirm("<%="确认删除吗？".ToLang()%>", function () {
                whir.loading.show();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/AttrForm.aspx", {
                    data: {
                        _action: "DelAttr",
                        attrId: attrid
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
            });
        }
        //规格值删除事件
        function delv(attrValID, attrID) {
            var ids = attrValID + '|' + attrID;
            var dialog = whir.dialog.confirm("<%="确认删除吗？".ToLang()%>", function () {
                whir.loading.show();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/AttrForm.aspx", {
                    data: {
                        _action: "DelAttrValue",
                        ids: ids
                    },
                    success: function (response) {
                        if (response.Status == true) {
                            whir.toastr.success("<%="操作成功！".ToLang()%>");
                            window.location.href = response.Message
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
