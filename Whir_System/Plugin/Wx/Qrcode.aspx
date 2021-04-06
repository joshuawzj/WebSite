<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="Qrcode.aspx.cs" Inherits="Whir_System_Plugin_Wx_Qrcode" %>

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
                <%="二维码管理".ToLang()%>
                            <label style="float: right;"><%="当前公众号".ToLang() %>：<%=this.CurrentCredence!=null?this.CurrentCredence.AppName+"（"+this.CurrentCredence.AppId+"）":("<a class=\"btn-empty-wx\" href=\"WxCredence.aspx\">"+"未设置当前公众号，点击进行设置".ToLang()+"</a>")%></label>
            </div>
            <div class="panel-body">
                <div class="actions btn-group pull-left">
                    <a id="btn-create" class="btn btn-white" href="javascript:void(0);"><%="生成二维码".ToLang()%></a>
                </div>
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
                url: "<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?_action=GetQrcodes",
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
                    { title: '<%="二维码".ToLang()%>', field: 'Ticket', width: 180, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            return '<img src="https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=' + value + '" style="width:180px;height:180px;" />';
                        }
                    },
                    { title: '<%="描述".ToLang()%>', field: 'Summary', align: 'center', valign: 'middle' },
                    { title: '<%="创建日期".ToLang()%>', field: 'CreateDate', width: 150, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            return whir.ajax.fixJsonDate(value);
                        }
                    },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 100, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
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
            return '<input type="checkbox" name="btSelectItem" value="' + row.Whir_Wx_QrcodeId + '" />';

        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = "";
            html += '<a name="lbDelete" class="btn text-danger border-normal" onclick="Delete(\'' + row.Whir_Wx_QrcodeId + '\');" href="javascript:;"><%="删除".ToLang()%></a>';
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
                            _action: "RemoveQrcodes",
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

        // 创建二维码
        $('#btn-create').click(function () {
            var views = [];
            views.push('<ul style="width:100%; max-width:400px;overflow:hidden;">');

            views.push('<li style="float:left;width:50%;position:relative;height:20px;margin:8px 0;">');
            views.push('    <div data-val="1" class="iradio_flat-red checked" style="position: relative;"><input style="position: absolute; opacity: 0;" type="checkbox"><ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins></div>');
            views.push('    <label style="position:absolute;left:24px;top:0px;" for="flat-checkbox-2"><%="临时二维码".ToLang()%></label>');
            views.push('</li>');
            views.push('<li style="float:left;width:50%;position:relative;height:20px;margin:8px 0;">');
            views.push('    <div data-val="2" class="iradio_flat-red" style="position: relative;"><input style="position: absolute; opacity: 0;" type="checkbox"><ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins></div>');
            views.push('    <label style="position:absolute;left:24px;top:0px;" for="flat-checkbox-2"><%="永久二维码".ToLang()%></label>');
            views.push('</li>');

            views.push('</ul>');
            views.push(' <div class="form-group" style="margin-top:16px;">');
            views.push('    <textarea name="Summary" placeholder="<%="请输入二维码用途描述".ToLang()%>" class="form-control" type="text" maxlength="100"></textarea');
            views.push(' </div>');
            views.push(' <div class="form-group" style="margin-top:4px;">');
            views.push('    <input name="Keyword" placeholder="<%="关键词，可与自动回复里面的关键词匹配".ToLang()%>" class="form-control" type="text" maxlength="20"/>');
            views.push(' </div>');
            views.push(' <div id="container-date" class="input-group date form_datetime col-md-5" style="margin-top:4px;">');
            views.push('    <input name="Expired" placeholder="<%="过期时间，临时二维码不能超过30天过期时间".ToLang()%>" class="form-control" type="text" maxlength="30"/>');
            views.push('    <span class="input-group-addon"><span class="glyphicon glyphicon-remove"></span></span>');
            views.push('    <span class="input-group-addon"><span class="glyphicon glyphicon-time"></span></span>');
            views.push(' </div>');
            whir.dialog.confirm('<div id="ui-upload-form" style="width:100%;max-width:400px;">' + views.join('') + '</div>', function () {
                var dataVal = $('#movePanel').find('div.checked').attr('data-val'), data = { _action: 'AddQrcode', CodeType: dataVal };
                if (dataVal == '1') {
                    data['Expired'] = $.trim($('input[name="Expired"]').val());
                    if (data['Expired'] == '') {
                        whir.toastr.error('<%="请填写过期时间".ToLang()%>');
                        return false;
                    }
                }
                data['Summary'] = $.trim($('textarea[name="Summary"]').val());
                data['Keyword'] = $.trim($('input[name="Keyword"]').val());
                if (data['Summary'] == '') {
                    whir.toastr.error('<%="请填写用途描述".ToLang()%>');
                    return false;
                }
                if (data['Keyword'] == '') {
                    whir.toastr.error('<%="请填写关键词".ToLang()%>');
                    return false;
                }
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                    data: data,
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                            reload();
                            reloadTags();
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                });
                return false;
            }, 'dialog-set-tag');
            $('#movePanel').find('div.iradio_flat-red').click(function () {
                $(this).addClass('checked').parent().siblings().find('div.iradio_flat-red').removeClass('checked');
                if ($(this).attr('data-val') == '1') {
                    $('#movePanel').find('div.expired').show();
                } else {
                    $('#movePanel').find('div.expired').hide();
                }
            });
            $('#container-date').datetimepicker({
                format: 'yyyy-mm-dd hh:ii:ss',
                todayBtn: 1,
                startView: 2,
                minView: 0,
                maxView: 4,
                autoclose: 1,
                todayHighlight: 1,
                forceParse: false
            });
        });

        $("#lbDel").click(function () {
            if ($("#hidChoose").val() == "") {
                whir.toastr.warning("<%="请选择要删除的记录".ToLang() %>");
            } else {
                whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                    whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                        data: {
                            _action: "RemoveQrcodes",
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

