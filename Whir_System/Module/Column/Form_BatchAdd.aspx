<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/DialogMasterPage.master"
    AutoEventWireup="true" CodeFile="Form_BatchAdd.aspx.cs" Inherits="Whir_System_Module_Column_Form_BatchAdd" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="<%=SysPath %>res/js/ArtTemplate/artTemplate.js" type="text/javascript"> </script>
    <script type="text/javascript">
        $(function () {
            //添加字段
            $(".btnAdd").click(function () {
                var html = template.render('fieldTemplate', "");
                $("#tbdFieldList").append(html);
                bindEvent(); //删除字段
            });

            //页面初始化
            $(".btnAdd").trigger("click");
            bindEvent();
        });

        function bindEvent() {
            //删除按钮
            $(".btnDel").unbind("click");
            $(".btnDel").click(function () {
                var del = this;
                whir.dialog.confirm('<%="确定要删除该字段吗?".ToLang() %>',
                    function() {
                        var tr = $(del).parent().parent();
                        tr.remove();
                        whir.dialog.remove();
                    });
            });

            //数据库字段名自动翻译
            $(".txtFieldName").blur(function () {
                var fieldAlias = $(this).parent().next().find(".txtFieldNote");
                if ($(this).val() != "" & fieldAlias.val() == "") {
                    fieldAlias.val("...");
                    $.ajax({
                        type: "GET",
                        url: "<%=SysPath %>ajax/common/translater.aspx",
                        data: "source=" + encodeURI($(this).val()) + "&from=en&to=zh-cn",
                        success: function (msg) {
                            fieldAlias.val(msg).change();
                        },
                        error: function (response) {
                            whir.toastr.warning("<%="翻译失败".ToLang() %>");
                        }
                    });

                }
            });

            //表单别名自动翻译
            $(".txtFieldNote").blur(function () {
                var fieldName = $(this).parent().prev().find(".txtFieldName");
                if ($(this).val() != "" & fieldName.val() == "") {
                    fieldName.val("...");
                    $.ajax({
                        type: "GET",
                        url: "<%=SysPath %>ajax/common/translater.aspx",
                        data: "source=" + encodeURI($(this).val()) + "&from=zh-cn&to=en&camelCase=true",
                        success: function (msg) {
                            fieldName.val(msg).change();
                        },
                        error: function (response) {
                            whir.toastr.warning("<%="翻译失败".ToLang() %>");
                        }
                    });

                }
            });

            //处理备注文本框回车键
            $(".txtFieldNote").unbind('keydown');
            $(".txtFieldNote").bind('keydown', function (e) {
                var key = e.which;
                if (key == 13) {
                    e.preventDefault();
                    $("#tbdFieldList").find(".txtFieldNote").removeClass("text_common_hover");
                    $(this).trigger("blur");

                    if ($(this).parent().parent().next().find(".txtFieldNote").length == 0) {
                        $(".btnAdd").trigger("click");
                    }
                    var nextNote = $(this).parent().parent().next().find(".txtFieldNote");
                    nextNote.focus();
                    nextNote.addClass("text_common_hover");

                }
            });
        }
    </script>
    <script id="fieldTemplate" type="text/html">        
        <tr class='tr_items'>
            <td style='padding-left: 0px;'>
                <input type='text' class='txtFieldName form-control' />
            </td>
            <td style='padding-left: 0px;'>
                <input type='text' class='txtFieldNote form-control' />
            </td>
            <td style='padding-left: 0px;'>
                <select class='dropFieldType form-control'>
                    <option selected='selected' value='1'><%="单行文本".ToLang() %></option>
                    <option value='2'><%="多行文本".ToLang() %></option>
                    <option value='3'><%="HTML".ToLang() %></option>
                    <option value='5'><%="数字".ToLang() %></option>
                    <option value='6'><%="货币".ToLang() %></option> 
                    <option value='9'><%="是/否".ToLang() %></option> 
                    <option value='14'><%="密码型字段".ToLang() %></option>
                </select>
            </td>
            <td style='text-align: center'>
                <a href='javascript:void(0)' class='btnDel btn btn-xs text-danger border-normal'><%="移除".ToLang()%></a> 
            </td>
        </tr>
    </script>
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="alert alert-danger no-opacity">
            <button data-dismiss="alert" class="close" type="button">×</button>
            <span class="entypo-attention"></span>
            <span><%="说明：批量添加的字段，其字段设置将采用与该字段类型对应的默认设置.".ToLang() %></span>
        </div>

        <div class="actions">
            <a class="btnAdd btn btn-white" href="javascript:void(0)"><%= "添加".ToLang() %></a>
        </div>
        <div class="tableCategory-table-body">
        <table width="100%" border="0" cellspacing="0" cellpadding="0"
            class="controller table table-bordered table-noPadding">
            <tr class="trClass">
                <th style="width: 24%;">
                    <%= "字段名".ToLang() %>
                </th>
                <th style="width: 24%">
                    <%= "表单别名".ToLang() %>
                </th>
                <th style="width: 24%">
                    <%= "表单类型".ToLang() %>
                </th>
                <th style="text-align: center; width: 26%">
                    <%= "操作".ToLang() %>
                </th>
            </tr>
            <tbody id="tbdFieldList">
            </tbody>
        </table>
        </div>
        <div class="operate_foot" style="display: none">
            <span id="btnSave"><b><%="批量添加到表单".ToLang() %></b></span>
        </div>
    </div>
    <script type="text/javascript">
        $(function() {
            //保存按钮
             $("[name=_dialog] .btn-primary", parent.document)
                .click(function() {
                if (checkFiledInput()) {
                    $(this).attr({ "disabled": "disabled" }); //防止重复提交

                    var xml = "<root>";
                    var columnId = <%= ColumnId %>;
                    var trs = $("#tbdFieldList tr");
                    for (var i = 0; i < trs.length; i++) {
                        xml += "<item>";
                        var name = $("#tbdFieldList tr").eq(i).find(".txtFieldName").val();
                        var note = $("#tbdFieldList tr").eq(i).find(".txtFieldNote").val();
                        var type = $("#tbdFieldList tr").eq(i).find(".dropFieldType").val();
                        xml += "<name>" + escape(name) + "</name>";
                        xml += "<note>" + escape(note) + "</note>";
                        xml += "<type>" + escape(type) + "</type>";
                        xml += "</item>";
                    }

                    xml += "</root>";
                    $.ajax({
                        type: "POST",
                        url: "<%=SysPath %>ajax/common/formFieldBatchAdd.aspx",
                        data: "columnId=" + encodeURI(columnId) + "&xml=" + xml,
                        async: false,
                        success: function(msg) {
                            switch (msg) {
                            case "ok":
                                window.parent.whir.toastr.success('<%="操作成功!".ToLang() %>',true,true);
                                window.parent.whir.dialog.remove();
                                break;
                            case "err":
                                window.parent.whir.toastr.error('<%="操作失败!".ToLang() %>');
                                $("[name=_dialog] .btn-primary", parent.document).removeAttr("disabled");
                                break;
                            default:
                                window.parent.whir.toastr.error(msg);
                                $("[name=_dialog] .btn-primary", parent.document).removeAttr("disabled");
                                break;
                            }
                        }
                    });

                } else {
                    $("[name=_dialog] .btn-primary", parent.document).removeAttr("disabled");
                }
            });
        });

        Array.prototype.have = function(e) {
            for (var i = 0, j; j = this[i]; i++) {
                if (j == e) {
                    return true;
                }
            }
            return false;
        };

        function checkFiledInput() {
            //数据验证
            var nameArr = [];
            
            var trs = $("#tbdFieldList tr");
            if (trs.length < 1) {
                TipError('<%="未添加任何字段!".ToLang() %>');
                return false;
            } else {
                var rx = /[a-zA-Z]{1}[a-zA-Z0-9]{0,19}/;
                for (var i = 0; i < trs.length; i++) {

                    var name = $("#tbdFieldList tr").eq(i).find(".txtFieldName");
                    var matches = rx.exec(name.val());

                    //字段名验证 
                    var nameValue = name.val().replace(/(^\s*)|(\s*$)/g, "");
                    if (nameValue == "") {
                        name.focus();
                        TipError('<%="字段名不能为空!".ToLang() %>');
                        return false;
                    } else if (matches == null || nameValue != matches[0]) {
                        name.focus();
                        TipError('<%="请输入不多于20个字符的字母数字组合，并以字母开头!".ToLang() %>');
                        name.val("");
                        return false;
                    } else if (nameArr.have(nameValue.toLowerCase())) {
                        name.focus();
                        TipError('<%="存在相同字段名的字段，请检查字段名输入!".ToLang() %>');
                        return false;
                    } else {
                        nameArr.push(nameValue.toLowerCase());
                    }

                    //表单别名验证
                    var note = $("#tbdFieldList tr").eq(i).find(".txtFieldNote");
                    if (note.val() == "") {
                        note.focus();
                        TipError('<%="表单别名不能为空!".ToLang() %>');
                        return false;
                    } else if (note.val().length > 20) {
                        note.focus();
                        TipError('<%="表单别名不能超过20个字符!".ToLang() %>');
                        return false;
                    }

                }
            }
            return true;
        }
    </script>
</asp:Content>
