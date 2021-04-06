<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="Images.aspx.cs" Inherits="Whir_System_Plugin_Wx_Images" %>

<%@ Import Namespace="Whir.Language" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"> 
    <style type="text/css">
        .file-live-thumbs {
            height: 300px;
            overflow-y: auto;
        }

        .bg-warning {
            display: none;
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
    <div class="space15"></div>
    <div class="content-wrap">
        <div class="row">
            <div class="col-lg-12">
                <div class="panel">
                    <div class="panel-heading">
                        <%="图片素材".ToLang()%>
                <label style="float: right;"><%="当前公众号".ToLang() %>：<%=this.CurrentCredence!=null?this.CurrentCredence.AppName+"（"+this.CurrentCredence.AppId+"）":("<a class=\"btn-empty-wx\" href=\"WxCredence.aspx\">"+"未设置当前公众号，点击进行设置".ToLang()+"</a>")%></label>
                    </div>
                    <div class="panel-body">
                        <div style="margin: 0 10px 0 0;" class="actions btn-group pull-left" title="<%="2M，支持bmp/png/jpeg/jpg/gif格式".ToLang()%>">
                            <input id="data-file" name="data-file" type="file" />
                        </div>
                        <div class="actions btn-group pull-left" style="margin-right: 10px;">
                            <a id="btn-async" class="btn btn-white" href="javascript:void(0);"><%="同步图片素材".ToLang()%></a>
                        </div>
                        <table id="Common_Table">
                        </table>
                        <div class="space10"></div>
                        <div class="operate_foot">
                            <input type="hidden" id="hidChoose" />
                            <a id="btn-remove" class="btn text-danger border-danger"><%="批量删除".ToLang()%></a>
                        </div>
                        <div class="clear"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">

        $(document).ready(function () {
            whir.checkbox.checkboxOnload("btn-set-tag", "hidChoose", "cb_Top", "cb_Position");
        });

        var $table = $('#Common_Table'), _that;

        //列表
        function initTable() {
            $table.bootstrapTable({
                url: "<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?_action=GetMedias&media=image",
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
                        title: '<%="图片".ToLang()%>', field: 'LocalURL', width: 60, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            return '<img src="' + (value || row['WebURL']) + '" style="max-width:50px;max-height:50px;" />';
                        }
                    },
                    { title: '<%="标题".ToLang()%>', field: 'Title', width: 150, align: 'center', valign: 'middle' },
                    { title: '<%="名称".ToLang()%>', field: 'MediaName', align: 'center', valign: 'middle' },
                    {
                        title: '<%="更新日期".ToLang()%>', field: 'UpdateDate', width: 150, align: 'center', valign: 'middle', formatter: function (value, row, index) {
                            return whir.ajax.fixJsonDate(value);
                        }
                    },
                    { title: '<%="操作".ToLang()%>', field: 'Op', width: 120, align: 'center', valign: 'middle', formatter: function (value, row, index) { return GetOperation(value, row, index); } },
                ],
                onLoadSuccess: function (data) {
                    //设置样式 后期需修改
                    SetTableStyleEvent();

                },
                onLoadError: function (data, mes) {
                    if (mes && mes.responseText) {
                        whir.toastr.warning(mes.responseText);
                    } else {
                        whir.toastr.error("<%="获取数据失败！".ToLang()%>");
                    }
                }
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
            return '<input class="ckb-pretty" type="checkbox" name="btSelectItem" value="' + row.MediaId + '" />';
        }
        //获取操作HTML
        function GetOperation(value, row, index) {
            var html = '<div class="btn-group">';
            html += '<a class="btn btn-white" onclick="setName(\'' + row.MediaId + '\');" href="javascript:void(0);"><%="编辑".ToLang() %></a>';
            html += '<a class="btn text-danger border-normal" onclick="removeMedia(\'' + row.MediaId + '\');" href="javascript:void(0);"><%="删除".ToLang() %></a>';
            html += '</div>';
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

        function reload() {
            $table.bootstrapTable('refresh');
            $('#btSelectAll').parent().removeClass('checked');
        }

        // 上传图片
       // $.fn.fileinputLocales.zh.browseLabel = '<%="上传图片".ToLang()%>';
        $('#data-file').fileinput({
            language: '<%=GetLoginUserLanguage()%>',
            uploadUrl: "<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx?",
            allowedFileExtensions: ['jpg', 'jpeg', 'png', 'gif', 'bmp'],
            initialCaption: '<%="支持格式：".ToLang()%>bmp/png/jpeg/jpg/gif',
            previewClass: "bg-warning",
            initialPreviewAsData: true,
            initialPreviewFileType: 'image',
            noPicker: true,
            showCancel: false,
            showRemove: false,
            //showPreview: false,
            showUpload: false,
            showCaption: false,
            showUploadedThumbs: false,
            uploadExtraData: { _action: 'UploadMedia', media: 'image' }
        }).on("filebatchselected", function (event, files) {
            $(this).fileinput("upload");
        }).on("fileuploaded", function (event, data) {
            if (data.response.Status && data.response.Message) {
                reload();
            } else {
                whir.toastr.error(data.response.Message);
            }
            $('div.kv-upload-progress').hide();
        });

        // 同步所有图片素材
        $('#btn-async').click(function () {
            whir.dialog.confirm("<%="同步所有图片素材信息可能需要花较长时间，确定同步吗？".ToLang() %>", function () {
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                    data: {
                        _action: 'SyncMedias',
                        media: 'image'
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

        // 批量删除
        $('#btn-remove').click(function () {
            if ($("#hidChoose").val() == "") {
                whir.toastr.warning("<%="请选择要删除的记录".ToLang() %>");
            } else {
                removeMedia($("#hidChoose").val());
            }
        });

        function removeMedia(medias) {
            whir.dialog.confirm("<%="确认要删除吗？".ToLang() %>", function () {
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                    data: {
                        _action: "RemoveMedia",
                        medias: medias,
                        media: 'image'
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

        function setName(id) {
            var views = [];
            views.push(' <div class="form-group">');
            views.push('    <input name="name" placeholder="<%="请输入名称".ToLang()%>" class="form-control" type="text" maxlength="100"/>');
            views.push(' </div>');
            whir.dialog.confirm(views.join(''), function () {
                var name = $('input[name="name"]').val();
                if (name == '') {
                    $('input[name="name"]').focus();
                    return false;
                }
                whir.dialog.remove();
                whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                    data: {
                        _action: "SetMedia",
                        media: id,
                        title: name
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
            whir.ajax.post("<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx", {
                data: {
                    _action: 'GetMedia',
                    media: id
                },
                success: function (response) {
                    whir.loading.remove();
                    if (response.Status == true) {
                        var data = $.parseJSON(response['Message'] || '{}');
                        $('input[name="name"]').val(data['Title']).focus();
                    } else {
                        whir.toastr.error(response.Message);
                    }
                }
            });
        }
    </script>
</asp:Content>

