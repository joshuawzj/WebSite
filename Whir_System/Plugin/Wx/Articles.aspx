<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="Articles.aspx.cs" Inherits="Whir_System_Plugin_Wx_Articles" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        div.data-articles {
            width: 100%;
        }

            div.data-articles div.article {
                border: 1px solid #d0d0d0;
                border-radius: 3px;
                float: left;
                height: 122px;
                margin: 1%;
                width: 31.3%;
            }

                div.data-articles div.article:hover {
                    border-color: #ff6a00;
                }

                div.data-articles div.article span {
                    display: block;
                    width: 98%;
                    margin: 2px 1% 2px;
                    height: 88px;
                    background-position: center center;
                    background-size: cover;
                }

                div.data-articles div.article p {
                    line-height: 24px;
                    word-break: keep-all;
                    text-overflow: ellipsis;
                    text-align: center;
                    overflow: hidden;
                    border-top: 1px dashed #d0d0d0;
                }

        span.data-avatar {
            width: 250px;
            height: 150px;
            background-position: center center;
            background-size: cover;
            display: block;
            background-repeat: no-repeat;
        }

            span.data-avatar:hover {
                background-size: contain;
            }
    </style>


    <script src="<%=SysPath %>res/js/jquery_sorttable/core.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/widget.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/mouse.js" type="text/javascript"></script>
    <script src="<%=SysPath %>res/js/jquery_sorttable/sortable.js" type="text/javascript"></script>
    <script type="text/javascript">
        //批量选中
        function selectAction() {
            if (!whir.checkbox.isSelect('cb_Position')) {
                TipMessage('<%="请选择".ToLang()%>');
                return false;
            }
            return true;
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-heading">
                <%="图文素材".ToLang()%>
                <label style="float: right;"><%="当前公众号".ToLang() %>：<%=this.CurrentCredence!=null?this.CurrentCredence.AppName+"（"+this.CurrentCredence.AppId+"）":("<a class=\"btn-empty-wx\" href=\"WxCredence.aspx\">"+"未设置当前公众号，点击进行设置".ToLang()+"</a>")%></label>
            </div>
            <div class="panel-body">
                <div class="actions btn-group pull-left" style="margin-right: 10px;">
                    <a class="btn btn-white" href="EditArticle.aspx"><span class="fontawesome-plus"></span>&nbsp;<%="添加图文素材".ToLang()%></a>
            </div>
            <div class="actions btn-group pull-left">
                <a id="btn-async" class="btn btn-white" href="javascript:void(0);"><%="同步图文素材".ToLang()%></a>
            </div>
            <table id="Common_Table"></table>
            <div class="space10"></div>
            <div class="operate_foot">
                <input type="hidden" id="hidChoose" />
                <a id="lbDel" class="btn text-danger border-danger"><%="批量删除".ToLang()%></a>
            </div>
            <div class="clear"></div>
        </div>
    </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            whir.checkbox.checkboxOnload("lbDel", "hidChoose", "cb_Top", "cb_Position");
        });

        var $table = $('#Common_Table'), _that;

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?_action=GetMedias&media=news",
                dataType: "json",
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                pageSize: 10,
                onLoadSuccess: function () {
                    //设置样式 后期需修改
                    SetTableStyleEvent();
                },
                onLoadError: function (data, mes) {
                    if (mes && mes.responseText) {
                        whir.toastr.warning(mes.responseText);
                    } else {
                        whir.toastr.error("<%="获取数据失败！".ToLang()%>");
                    }
                },
                onColumnSwitch: function () {
                    SetTableStyleEvent();
                },
                SelectColumnEvent: function () {
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'Checkbox', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    { title: '<%="标题".ToLang()%>', field: 'Title', align: 'center', valign: 'middle' },
                    {
                        title: '<%="封面".ToLang()%>', field: 'ThumbMediaURL', width: 250, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            return '<span class="data-avatar" style="background-image:url(' + value + ')"></span>';
                        }
                    },
                    {
                        title: '<%="多页".ToLang()%>', field: 'Children', width: 400, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            if (value && value.length > 0) {
                                for (var i = 0, rows = value, views = []; i < rows.length; i++) {
                                    views.push('<div class="article">');
                                    views.push('    <span style="background-image:url(' + rows[i]['ThumbMediaURL'] + ')"></span>');
                                    views.push('    <p title="' + rows[i]['Title'] + '">' + rows[i]['Title'] + '</p>');
                                    views.push('</div>');
                                }
                                return '<div class="data-articles">' + views.join('') + '</div>';
                            } else {
                                return '<%="无".ToLang()%>';
                            }
                        }
                    },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 120, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ]
            });
            whir.loading.remove();
        }

        initTable();


        //重置表格样式
        var setTableStyle = function () {
            $(".sortTable tr[data-index]").removeClass();
            $(".sortTable tr:odd").addClass("tdBgColor tdColor");
            $(".sortTable tr:even").addClass("tdColor");
        };

        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input type="checkbox" name="btSelectItem" value="' + row.MediaId + '" />';
        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group-vertical">';
            html += '<a name="aEdit" class="btn btn-white" href="EditArticle.aspx?media=' + row.MediaId + '"><%="编辑".ToLang() %></a>';
            html += '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(\'' + row.MediaId + '\');" href="javascript:;"><%="删除".ToLang()%></a>';
            html += '</div>';
            return html;
        }
        function SetTableStyleEvent() {
            whir.checkbox.destroy();
            whir.skin.radio();
            whir.skin.checkbox();
            //绑定全选按钮事件
            $("#btSelectAll").next().click(function () {
                if ($("#btSelectAll").is(':checked')) {
                    whir.checkbox.selectAll('lbDel', 'hidChoose', 'btSelectItem');
                } else {
                    whir.checkbox.cancelSelectAll('lbDel', 'hidChoose', 'btSelectItem');
                }

            });

            $("input[name=btSelectItem]").each(function () {
                $(this).next().click(function () {
                    $("#hidChoose").val(whir.checkbox.getSelect('btSelectItem'));
                });
            });
        }


        function reload() {
            $table.bootstrapTable('refresh');
        }

        function Delete(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                    data: {
                        _action: "RemoveMedia",
                        medias: id,
                        media: 'news'
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                            reload();
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                });
                return false;
            });
        }

        $("#lbDel").click(function () {
            if ($("#hidChoose").val() == "") {
                whir.toastr.warning("<%="请选择要删除的记录".ToLang() %>");
            } else {
                whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                    whir.dialog.remove();
                    whir.loading.show();
                    whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                        data: {
                            _action: "RemoveMedia",
                            medias: $("#hidChoose").val(),
                            media: 'news'
                        },
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                reload();
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    });
                    return false;
                });
            }
        });

        // 同步图文素材
        $('#btn-async').click(function () {
            whir.dialog.confirm("<%="同步所有图文素材信息可能需要花较长时间，确定同步吗？".ToLang() %>", function () {
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                    data: {
                        _action: 'SyncMedias',
                        media: 'news'
                    },
                    success: function (response) {
                        whir.loading.remove();
                        whir.dialog.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                });
                return false;
            });
        });

    </script>
</asp:Content>

