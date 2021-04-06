<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="Users.aspx.cs" Inherits="Whir_System_Plugin_Wx_Users" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        a.btn-tag {
            display: block;
            line-height: 30px;
            width: 100%;
            cursor: pointer;
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
    <textarea id="tags-area" style="display: none;"><%=this.Tags %></textarea>
    <div class="space15"></div>
    <div class="content-wrap" style="padding: 0 14px;">
        <div class="row">
            <div class="col-lg-9">
                <div class="panel">
                    <div class="panel-heading">
                        <%="用户管理".ToLang()%>
                            <label style="float: right;"><%="当前公众号".ToLang() %>：<%=this.CurrentCredence!=null?this.CurrentCredence.AppName+"（"+this.CurrentCredence.AppId+"）":("<a class=\"btn-empty-wx\" href=\"WxCredence.aspx\">"+"未设置当前公众号，点击进行设置".ToLang()+"</a>")%></label>
                    </div>
                    <div class="panel-body">
                        <div class="actions btn-group pull-left" style="margin-right: 10px;">
                            <a id="btn-async" class="btn btn-white" href="javascript:void(0);"><%="同步所有用户信息".ToLang()%></a>
                            <a id="btn-show-all" class="btn btn-white" href="javascript:void(0);"><%="显示所有用户".ToLang()%></a>
                        </div>
                        <table id="Common_Table"></table>
                        <div class="space10"></div>
                        <div class="operate_foot">
                            <input type="hidden" id="hidChoose" />
                            <button id="btn-set-tag" type="button" class="btn btn-white"><span class="entypo-bookmarks"></span>&nbsp;<%="打标签".ToLang()%></button>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="panel">
                    <div class="panel-heading"><%="标签管理".ToLang()%></div>
                    <div class="panel-body">
                        <div class="actions btn-group pull-left">
                            <a id="btn-addTag" class="btn btn-white" href="javascript:void(0);"><%="添加标签".ToLang()%></a>
                        </div>
                        <table id="tags"></table>
                        <div class="space10"></div>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        var tagStores = {
            tags: [],
            tagPair: {},
            view: '',
            init: function (tags) {
                this.tagPair = {};
                this.view = '';
                $(tags).each(function (idx, item) {
                    tagStores.tagPair[item['id'].toString()] = item;
                });
                this.tags = tags;
            },
            get: function (id) {
                return this.tagPair[id];
            },
            getName: function (id) {
                var tag = this.get(id) || {};
                return tag['name'] || '';
            },
            getView: function (text) {
                if (!this.view) {
                    var views = [];
                    views.push('<div class="btn-group tag-set">');
                    views.push('    <button data-toggle="dropdown" class="btn btn-default dropdown-toggle" type="button">{{text}}&nbsp;<span class="caret"></span></button>');
                    views.push('    <ul role="menu" class="dropdown-menu">');
                    for (var i = 0; i < this.tags.length; i++) {
                        views.push('<li style="position:relative;margin:4px;">');
                        views.push('    <div data-val="' + this.tags[i]['id'] + '" class="icheckbox_flat-red" style="position: relative;"><input style="position: absolute; opacity: 0;" type="checkbox"><ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins></div>');
                        views.push('    <label style="position:absolute;left:24px;top:0px;" for="flat-checkbox-2">' + this.tags[i]['name'] + '</label>');
                        views.push('</li>');
                    }
                    views.push('    </ul>');
                    views.push('</div>');
                    this.view = views.join('');
                }
                return this.view.replace('{{text}}', text);
            }
        };
        tagStores.init($.parseJSON($('#tags-area').val() || '[]'));

        $(document).ready(function () {
            whir.checkbox.checkboxOnload("btn-set-tag", "hidChoose", "cb_Top", "cb_Position");

        });

        var $table = $('#Common_Table'), _that;

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?_action=GetUsers",
                dataType: "json",
                pagination: true, //分页
                sidePagination: "server", //服务端处理分页
                silent: true,
                clickToSelect: false,
                pageSize: 10,
                onColumnSwitch: function () {
                    SetTableStyleEvent();
                },
                SelectColumnEvent: function () {
                },
                columns: [
                    { title: '<input type="checkbox"  id="btSelectAll" />', field: 'Checkbox', width: 40, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetCheckbox(value, row, index); } },
                    {
                        title: '<%="头像".ToLang()%>', field: 'headimgurl', width: 60, align: 'center', valign: 'middle', formatter: function (value) {
                            return '<img src="' + value + '" style="width:50px;height:50px;" />';
                        }
                    },
                    { title: '<%="昵称".ToLang()%>', field: 'nickname', align: 'center', valign: 'middle' },
                    {
                        title: '<%="性别".ToLang()%>', field: 'sex', align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            switch (value) {
                                case 1:
                                    return '<%="男".ToLang()%>';
                                case 2:
                                    return '<%="女".ToLang()%>';
                                default:
                                    return '<%="保密".ToLang()%>'
                            }
                        }
                    },
                    {
                        title: '<%="标签".ToLang()%>', field: 'tagid_list', width: 100, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            var views = ['<div data-usr="' + row['openid'] + '" data-tag="' + value + '">'];
                            if (value) {
                                var text = tagStores.getName(value.split(',')[0]) || '<%="无标签".ToLang()%>';
                                views.push(tagStores.getView(text));
                            } else {
                                views.push(tagStores.getView('<%="无标签".ToLang()%>'));
                            }
                            return views.join('');
                        }
                    },
                    { title: '<%="备注".ToLang()%>', field: 'remark', align: 'center', valign: 'middle' },
                    {
                        title: '<%="关注时间".ToLang()%>', field: 'subscribe_time', width: 150, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            return whir.ajax.fixJsonDate(value);
                        }
                    },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 120, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ],
                onLoadError: function (data, mes) {
                    if (mes && mes.responseText) {
                        whir.toastr.warning(mes.responseText);
                    } else {
                        whir.toastr.error("<%="获取数据失败！".ToLang()%>");
                    }
                },
                onLoadSuccess: function (data) {
                    //设置样式 后期需修改
                    SetTableStyleEvent();
                    $('#Common_Table').find('div.icheckbox_flat-red').click(function () {
                        var data = {}, tag = $(this).attr('data-val'), user = $(this).parents('div[data-usr]:first').attr('data-usr');
                        if ($(this).hasClass('checked')) {
                            $(this).removeClass('checked');
                            data = {
                                _action: 'CancelTags',
                                tags: tag,
                                users: user
                            };
                        } else {
                            $(this).addClass('checked');
                            data = {
                                _action: 'SetTags',
                                tags: tag,
                                users: user
                            };
                        }
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
                    });
                    $('#Common_Table').find('div[data-tag]').each(function () {
                        for (var i = 0, tags = $(this).attr('data-tag').split(','); i < tags.length; i++) {
                            $(this).find('div[data-val="' + tags[i] + '"]').addClass('checked');
                        }
                    });
                }
            });
            whir.loading.remove();
        }

        //列表
        function initTags() {
            $('#tags').bootstrapTable({
                url: "<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?_action=GetTags",
                dataType: "json",
                pagination: false, //分页
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
                    {
                        title: '<%="名称".ToLang()%>', field: 'name', align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            return '<a class="btn-tag" data-tag="' + row['id'] + '">' + value + '（' + row.count + '）' + '</a>';
                        }
                    },
                    {
                        title: '<%="操作".ToLang()%>', field: 'Op', width: 100, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            var html = "";
                            html += '<div class="btn-group">';
                            html += '   <button onclick="updateTag(\'' + row.id + '\');" title="<%="编辑".ToLang()%>" type="button" class="btn btn-default"><span class="entypo-pencil"></span></button>';
                            html += '   <button onclick="removeTag(\'' + row.id + '\');" title="<%="删除".ToLang()%>" type="button" class="btn btn-danger"><span class="entypo-trash"></span></button>';
                            html += '</div>';
                            return html;
                        }
                    },
                ],
                onLoadSuccess: function (data) {
                    tagStores.init(data['rows']);
                    $('a.btn-tag').click(function () {
                        var tag = $(this).attr('data-tag');
                        reload('<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?_action=GetUsers&tag=' + tag);
                    });
                }
            });
            whir.loading.remove();
        }

        initTags();
        initTable();


        //重置表格样式
        var setTableStyle = function () {
            $(".sortTable tr[data-index]").removeClass();
            $(".sortTable tr:odd").addClass("tdBgColor tdColor");
            $(".sortTable tr:even").addClass("tdColor");

        };

        //获取多选框 HTML
        function GetCheckbox(value, row, index) {
            return '<input class="ckb-pretty" type="checkbox" name="btSelectItem" value="' + row.openid + '" />';

        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = "";
            html += '<a style="margin-right:2px;" class="btn btn-primary" onclick="refreshUser(\'' + row.openid + '\');" href="javascript:void(0);"><%="刷新".ToLang() %></a>';
            html += '<a class="btn btn-info" onclick="setRemark(\'' + row.openid + '\');" href="javascript:void(0);"><%="备注".ToLang() %></a>';
            return html;
        }

        function SetTableStyleEvent() {
            whir.checkbox.destroy();
            whir.skin.radio();
            $('#btSelectAll,input.ckb-pretty')
                .iCheck({
                    checkboxClass: 'icheckbox_flat-red',
                    radioClass: 'iradio_flat-red'
                });
            //绑定全选按钮事件
            $("#btSelectAll").next().click(function () {
                if ($("#btSelectAll").is(':checked')) {
                    whir.checkbox.selectAll('btn-set-tag', 'hidChoose', 'btSelectItem');
                } else {
                    whir.checkbox.cancelSelectAll('btn-set-tag', 'hidChoose', 'btSelectItem');
                }

            });

            $("input[name=btSelectItem]").each(function () {
                $(this).next().click(function () {
                    $("#hidChoose").val(whir.checkbox.getSelect('btSelectItem'));
                });
            });
        }

        function reload(url) {
            if (url) {
                $table.bootstrapTable('refresh', { url: url });
            } else {
                $table.bootstrapTable('refresh');
            }
            $('#btSelectAll').parent().removeClass('checked');
        }

        function reloadTags() {
            $('#tags').bootstrapTable('refresh');
            $('#btSelectAll').parent().removeClass('checked');
        }

        function getTagsData() {
            return $('#tags').bootstrapTable('getData');
        }

        function updateTag(id) {
            var views = [];
            views.push(' <div class="form-group">');
            views.push('    <input name="tag" placeholder="<%="请输入标签名称".ToLang()%>" class="form-control" type="text" maxlength="30"/>');
            views.push(' </div>');
            whir.dialog.confirm(views.join(''), function () {
                var tag = $('input[name="tag"]').val();
                if (tag == '') {
                    $('input[name="tag"]').focus();
                    return false;
                }
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                    data: {
                        _action: "UpdateTag",
                        tagid: id,
                        name: tag
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                            reloadTags();
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                });
                return false;
            });
            $('input[name="tag"]').focus();
        }

        function setRemark(id) {
            var views = [];
            views.push(' <div class="form-group">');
            views.push('    <input name="remark" placeholder="<%="请输入备注".ToLang()%>" class="form-control" type="text" maxlength="50"/>');
            views.push(' </div>');
            whir.dialog.confirm(views.join(''), function () {
                var remark = $('input[name="remark"]').val();
                if (remark == '') {
                    $('input[name="remark"]').focus();
                    return false;
                }
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                    data: {
                        _action: "SetUserRemark",
                        openid: id,
                        remark: remark
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

        function removeTag(id) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx",
                    {
                        data: {
                            _action: "RemoveTag",
                            tagid: id
                        },
                        success: function (response) {
                            whir.loading.remove();
                            whir.dialog.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message);
                                reloadTags();
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
                return false;
            });
        }

        function refreshUser(id) {
            whir.dialog.confirm("<%="刷新会从微信公众号同步信息，确定刷新吗？".ToLang() %>", function () {
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx",
                    {
                        data: {
                            _action: "SyncUser",
                            openid: id
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

        // 显示所有用户
        $('#btn-show-all').click(function () {
            reload('<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?_action=GetUsers');
        });

        // 添加标签
        $('#btn-addTag').click(function () {
            var views = [];
            views.push(' <div class="form-group">');
            views.push('    <input name="tag" placeholder="<%="请输入标签名称".ToLang()%>" class="form-control" type="text" maxlength="30"/>');
            views.push(' </div>');
            whir.dialog.confirm(views.join(''), function () {
                var tag = $('input[name="tag"]').val();
                if (tag == '') {
                    $('input[name="tag"]').focus();
                    return false;
                }
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                    data: {
                        _action: "AddTag",
                        name: tag
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.toastr.success(response.Message);
                            reloadTags();
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                });
                return false;
            });
            $('input[name="tag"]').focus();
        });

        // 同步所有用户信息
        $('#btn-async').click(function () {
            whir.dialog.confirm("<%="同步所有用户信息可能需要花较长时间，确定同步吗？".ToLang() %>", function () {
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                    data: {
                        _action: "SyncUsers"
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

        // 批量打标签
        $('#btn-set-tag').click(function () {
            if ($("#hidChoose").val() == "") {
                whir.toastr.warning("<%="请选择".ToLang() %>");
            } else {
                var tags = getTagsData(), views = [];
                views.push('<ul style="width:400px;overflow:hidden;">');
                $(tags).each(function (idx, item) {
                    views.push('<li style="float:left;width:33%;position:relative;height:20px;margin:8px 0;">');
                    views.push('    <div data-val="' + tags[idx]['id'] + '" class="icheckbox_flat-red" style="position: relative;"><input style="position: absolute; opacity: 0;" type="checkbox"><ins class="iCheck-helper" style="position: absolute; top: 0%; left: 0%; display: block; width: 100%; height: 100%; margin: 0px; padding: 0px; background: rgb(255, 255, 255) none repeat scroll 0% 0%; border: 0px none; opacity: 0;"></ins></div>');
                    views.push('    <label style="position:absolute;left:24px;top:0px;" for="flat-checkbox-2">' + tags[idx]['name'] + '</label>');
                    views.push('</li>');
                });
                views.push('</ul>');
                whir.dialog.confirm(views.join(''), function () {
                    var tags = [];
                    $('#movePanel').find('div.icheckbox_flat-red').each(function () {
                        if ($(this).hasClass('checked')) {
                            tags.push($(this).attr('data-val'));
                        }
                    });
                    if (tags.length == 0) {
                        whir.toastr.warning("<%="请选择".ToLang() %>");
                        return false;
                    }
                    if (tags.length > 3) {
                        whir.toastr.warning("<%="不能超过3个标签".ToLang() %>");
                        return false;
                    }
                    whir.dialog.remove();
                    whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                        data: {
                            _action: "SetTags",
                            tags: tags.join(','),
                            users: $('#hidChoose').val()
                        },
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
                $('#movePanel').find('div.icheckbox_flat-red').click(function () {
                    if ($(this).hasClass('checked')) {
                        $(this).removeClass('checked');
                    } else {
                        $(this).addClass('checked');
                    }
                });
            }
        });
    </script>
</asp:Content>

