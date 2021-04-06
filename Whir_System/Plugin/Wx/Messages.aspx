<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="Messages.aspx.cs" Inherits="Whir_System_Plugin_Wx_Messages" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


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
                <%="消息管理".ToLang()%>
                <label style="float: right;"><%="当前公众号".ToLang() %>：<%=this.CurrentCredence!=null?this.CurrentCredence.AppName+"（"+this.CurrentCredence.AppId+"）":("<a class=\"btn-empty-wx\" href=\"WxCredence.aspx\">"+"未设置当前公众号，点击进行设置".ToLang()+"</a>")%></label>
            </div>
            <div class="panel-body">
                <table id="Common_Table">
                </table>
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
                url: "<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?_action=GetMessages",
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
                        { title: '<%="昵称".ToLang()%>', field: 'NickName', width: 100, align: 'center', valign: 'middle' },
                        {
                            title: '<%="头像".ToLang()%>', field: 'Avatar', width: 60, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                                return '<img src="' + value + '" style="width:50px;height:50px;" />';
                            }
                        },
                        { title: '<%="性别".ToLang()%>', field: 'Sex', width: 60, align: 'center', valign: 'middle' },
                        {
                            title: '<%="信息".ToLang()%>', field: 'FormData', align: 'center', valign: 'middle', formatter: function (value, row, index) {
                                var data = $.parseJSON(value || '{}'), msgType = data['MsgType'];
                                switch (msgType) {
                                    case 'text':
                                        return '<%="（回复文本）".ToLang()%>' + data['Content'];
                                    case 'event':
                                        return '<%="（触发事件）".ToLang()%>' + (data['Event'] == 'subscribe' ? '<%="关注".ToLang()%>' : '<%="取消关注".ToLang()%>');
                                    default:
                                        return '-';
                                }
                            }
                        },
                        {
                            title: '<%="操作".ToLang()%>', field: 'Op', width: 100, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                                return GetOperation(value, row, index);
                            }
                        }
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
                return '<input type="checkbox" name="btSelectItem" value="' + row.MessageId + '" />';
            }
            //获取操作HTML
            function GetOperation(value, row, index) {
                var html = "";
                html += '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(\'' + row.MessageId + '\');" href="javascript:;"><%="删除".ToLang()%></a>';
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
                    whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx",
                        {
                            data: {
                                _action: "RemoveMessages",
                                items: id
                            },
                            success: function (response) {
                                whir.loading.remove();
                                whir.dialog.remove();
                                if (response.Status == true) {
                                    whir.toastr.success(response.Message);
                                    reload();
                                } else {
                                    whir.toastr.error(response.Message);
                                }
                            }
                        }
                    );
                    return false;
                });
            }

            $("#lbDel").click(function () {
                if ($("#hidChoose").val() == "") {
                    whir.toastr.warning("<%="请选择要删除的数据行！".ToLang() %>");
                } else {
                    whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                        whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                            data: {
                                _action: "RemoveMessages",
                                items: $("#hidChoose").val()
                            },
                            success: function (response) {
                                whir.loading.remove();
                                whir.dialog.remove();
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

        </script>
</asp:Content>

