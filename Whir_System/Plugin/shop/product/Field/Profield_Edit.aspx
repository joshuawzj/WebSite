<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    CodeFile="profield_edit.aspx.cs" Inherits="whir_system_Plugin_shop_product_field_profield_edit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Register Src="~/whir_system/Plugin/shop/common/HeadContainer.ascx" TagName="HeadContainer" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath%>res/js/Whir/whir.shopField.js" type="text/javascript"></script>
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="profieldlist.aspx" aria-expanded="true"><%="自定义属性管理".ToLang()%></a></li>
                    <li class="active"><a href="profield_edit.aspx" data-toggle="tab" aria-expanded="true"><%=ProcessStr %></a></li>
                </ul>
                <div class="space15"></div>
                <form id="proFieldEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/shop/FieldForm.aspx">
                   <div class="panel panel-default panels">

                        <div class="panel-heading">
                            <%="数据库信息".ToLang()%>
                        </div>
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="divFieldName">
                                    <%="数据库字段名称：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input id="FieldName" name="FieldName" class="form-control" value="" required="true"
                                        maxlength="20" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="divIsAllowNull">
                                    <%="是否允许为空：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <ul class="list">
                                        <li>
                                            <input type="radio" id="IsAllowNull_True" checked="checked" name="IsAllowNull" value="1" />
                                            <label for="IsAllowNull_True">
                                                <%="是".ToLang()%></label>
                                        </li>
                                        <li>
                                            <input type="radio" id="IsAllowNull_False" name="IsAllowNull" value="0" />
                                            <label for="IsAllowNull_False">
                                                <%="否".ToLang()%></label>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="divDefaultValue">
                                    <%="默认值：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input id="DefaultValue" name="DefaultValue" class="form-control" value="" maxlength="100" />
                                </div>
                            </div>
                        </div>
                        <div class="panel-heading">
                            <%="前台展示信息".ToLang()%>
                        </div>
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="divFieldAlias">
                                    <%="属性名称：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input id="FieldAlias" name="FieldAlias" class="form-control" value="" maxlength="100" required="true" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="divShowType">
                                    <%="展示形式：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <select id="ShowType" required="true" name="ShowType" class="form-control">
                                        <option value="1">
                                            <%="单行文本".ToLang()%></option>
                                        <option value="2">
                                            <%="单选".ToLang()%></option>
                                        <option value="3">
                                            <%="多选".ToLang()%></option>
                                        <option value="4">
                                            <%="多行文本".ToLang()%></option>
                                        <option value="5">
                                            <%="下拉框".ToLang()%></option>
                                        <option value="6">
                                            <%="HTML".ToLang()%></option>
                                        <option value="7">
                                            <%="图片".ToLang()%></option>
                                         <option value="8">
                                            <%="文件".ToLang()%></option>
                                    </select>
                                </div>
                            </div>
                            <div class="form-group" name="divBindType" style="display: none">
                                <div class="col-md-2 text-right" for="BindType">
                                    <%="绑定类型：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <ul class="list">
                                        <li>
                                            <input type="radio" id="BindType_1" checked="checked" name="BindType" value="1" />
                                            <label for="BindType_1">
                                                <%="绑定文本".ToLang()%></label>
                                        </li>
                                        <li>
                                            <input type="radio" id="BindType_2" name="BindType" value="2" />
                                            <label for="BindType_2">
                                                <%="绑定SQL".ToLang()%></label>
                                        </li>
                                        <li>
                                            <input type="radio" id="BindType_3" name="BindType" value="3" />
                                            <label for="BindType_3">
                                                <%="绑定多级类别".ToLang()%></label>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="form-group" name="divBindText" style="display: none">
                                <div class="col-md-2 control-label" for="ContentPagingParam">
                                    <%="文本选项：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <select id="seBindText" name="seBindText"  multiple class="form-control" size="6">
                                    </select>
                                    <input type="hidden" name="BindText" id="BindText" />
                                    <input type="button" value='<%="添加选项".ToLang()%>' id="btnAddOption" class="btn btn-white" />
                                    <input type="button" value='<%="编辑选项".ToLang()%>' id="btnModifyOption" class="btn btn-white" />
                                    <input type="button" value='<%="删除选项".ToLang()%>' id="btnDeleteOption" class="btn text-danger  border-normal" />
                                    <script type="text/javascript">

                                        $("#btnAddOption").click(function () {
                                            whir.dialog.frame('<%="添加选项".ToLang()%>', "<%=SysPath%>Module/Column/Option_Edit.aspx?JsCallback=addOption", "", 500, 200);
                                        });

                                        $("#btnModifyOption").click(function () {
                                            var selected = $("#seBindText option:selected");
                                            if (selected.length <= 0) {
                                                TipMessage('<%="请选择".ToLang()%>');
                                                return;
                                            }
                                            var optionValue = escape(selected.val());
                                            var optionText = escape(selected.text().split('|')[1]);
                                            var url = "<%=SysPath%>Module/Column/Option_Edit.aspx?JsCallback=modifyOption&option_value=" + optionValue + "&option_text=" + optionText;
                                            whir.dialog.frame('<%="编辑选项".ToLang()%>', url, "", 500, 200);
                                        });

                                        $("#btnDeleteOption").click(function () {
                                            var selected = $("#seBindText option:selected");
                                            if (selected.length <= 0) {
                                                TipMessage('<%="请选择".ToLang()%>');
                                                return;
                                            }
                                            selected.remove();
                                            readListboxToHidden();
                                        });

                                        function addOption(optionValue, optionText) {
                                            $("#seBindText").append("<option value='" + optionValue + "'>" + optionValue + "|" + optionText + "</option>");
                                            readListboxToHidden();
                                            whir.dialog.remove();
                                        }

                                        function modifyOption(optionValue, optionText) {
                                            $("#seBindText option:selected").val(optionValue).text(optionValue + "|" + optionText);
                                            readListboxToHidden();
                                            whir.dialog.remove();
                                        }

                                        //把ListBox里面的内容读取并赋值到Hidden
                                        function readListboxToHidden() {
                                            var options = "";
                                            $("#seBindText option").each(function () {
                                                options += $(this).text() + ",";
                                            });
                                            if (options.length > 0) {
                                                options = options.substring(0, options.length - 1);
                                                $("#BindText").val(options);
                                            }
                                        }

                                        //把Hidden的值填充到listbox，避免页面回传后listbox中的数据丢失
                                        function readHiddenToListbox() {
                                            var options = $("#BindText").val().split(',');
                                            if (options != '') {
                                                $(options).each(function (idx, item) {
                                                    var value = item.split('|')[0];
                                                    var text = item.split('|')[1];

                                                    var exsit = false;
                                                    $("#seBindText option[value='" + value + "']").each(function (idx2, item2) {
                                                        if ($(item2).html() == value + "|" + text)
                                                            exsit = true;
                                                    });

                                                    if (!exsit) {
                                                        addOption(value, text);
                                                    }
                                                });
                                            }
                                        }
                                        readHiddenToListbox();

                                    </script>
                                </div>
                            </div>
                            <div class="form-group" name="divBindSQL" style="display: none">
                                <div class="col-md-2 control-label" for="BindSQL">
                                    <%="绑定的SQL脚本：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input id="BindSQL" name="BindSQL" class="form-control" value="" maxlength="512" validationexpression="^.{0,512}$" />
                                </div>
                            </div>
                            <div class="form-group" name="divBindLevel" style="display: none">
                                <div class="col-md-2 control-label" for="BindTable">
                                    <%="绑定的表名：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input id="BindTable" name="BindTable" class="form-control" value=""
                                        maxlength="200" validationexpression="^.{0,200}$" />
                                </div>
                            </div>
                            <div class="form-group" name="divBindLevel" style="display: none">
                                <div class="col-md-2 control-label" for="BindKeyId">
                                    <%="绑定的栏目Id：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input id="BindKeyId" name="BindKeyId" class="form-control" value=""
                                        maxlength="200" validationexpression="^.{0,200}$" />
                                </div>
                            </div>
                            <div class="form-group" name="divField" style="display: none">
                                <div class="col-md-2 control-label" for="BindTextField">
                                    <%="绑定的文本字段：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input name="BindTextField" id="BindTextField" class="form-control" value=""
                                        maxlength="300" validationexpression="^.{0,200}$" />
                                </div>
                            </div>
                            <div class="form-group" name="divField" style="display: none">
                                <div class="col-md-2 control-label" for="BindValueField">
                                    <%="绑定的值字段：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input name="BindValueField" id="BindValueField" class="form-control" value=""
                                        maxlength="200" validationexpression="^.{0,200}$" />
                                </div>
                            </div>
                            <div class="form-group" name="divRepeatColumn" style="display: none">
                                <div class="col-md-2 control-label" for="RepeatColumn">
                                    <%="每行显示数：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input name="RepeatColumn"  class="form-control" value="" maxlength="3" min="1"
                                        validationexpression="^[0-9]*[1-9][0-9]*$" />
                                </div>
                            </div>
                            <div class="form-group" name="divValidateType">
                                <div class="col-md-2 control-label" for="ValidateType">
                                    <%="验证类型：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <select id="ValidateType" name="ValidateType" class="form-control">
                                        <option value="0"><%="==请选择==".ToLang()%></option>
                                        <option value="1"><%="电子邮箱".ToLang()%></option>
                                        <option value="2"><%="手机号码".ToLang()%></option>
                                        <option value="3"><%="电话号码".ToLang()%></option>
                                        <option value="4"><%="身份证号码".ToLang()%></option>
                                        <option value="5"><%="金额".ToLang()%></option>
                                        <option value="6"><%="数字".ToLang()%></option>
                                        <option value="7"><%="整数".ToLang()%></option>
                                        <option value="8"><%="非负整数".ToLang()%></option>
                                        <option value="9"><%="日期".ToLang()%></option>
                                        <option value="10"><%="日期时间".ToLang()%></option>
                                        <option value="11"><%="中文".ToLang()%></option>
                                        <option value="12"><%="英文".ToLang()%></option>
                                        <option value="13"><%="Url地址".ToLang()%></option>
                                        <option value="14"><%="安全字符".ToLang()%></option>
                                        <option value="15"><%="自定义表达式".ToLang()%></option>
                                    </select>
                                </div>
                            </div>
                            <div class="form-group" name="divValidateExpression">
                                <div class="col-md-2 control-label" for="ValidateExpression">
                                    <%="验证正则：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input id="ValidateExpression" name="ValidateExpression" class="form-control" value=""
                                        maxlength="400" />
                                </div>
                            </div>
                            <div class="form-group" name="divValidateExpression">
                                <div class="col-md-2 control-label" for="ValidateText">
                                    <%="错误提示文字：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input id="ValidateText" name="ValidateText" class="form-control" value=""
                                        maxlength="400" />
                                </div>
                            </div>
                            <div class="form-group" name="divValidateExpression">
                                <div class="col-md-2 control-label" for="IsUsing">
                                    <%="是否启用：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <ul class="list">
                                        <li>
                                            <input type="radio" id="IsUsing_True" checked="checked" name="IsUsing" value="1" />
                                            <label for="IsUsing_True">
                                                <%="启用".ToLang()%></label>
                                        </li>
                                        <li>
                                            <input type="radio" id="IsUsing_False" name="IsUsing" value="0" />
                                            <label for="IsUsing_False">
                                                <%="不启用".ToLang()%></label>
                                        </li>
                                    </ul>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-8 ">
                                    <input type="hidden" name="FieldID" id="FieldID" value="<%=FieldID%>" />
                                    <input type="hidden" name="_action" value="Save" />
                                    <div class="btn-group">
                                        <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                        <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                    </div>
                                    <a class="btn btn-white" href="profieldlist.aspx"><%="返回".ToLang()%></a>
                                </div>
                            </div>
                        </div>

                    </div>
                </form>

            </div>
        </div>
    </div>
    <script type="text/javascript">
        if ("<%=FieldID %>" != "0") {
            $("[name=ShowType]").attr("disabled", true);
            $("[name=FieldName]").attr("readonly", true);
            //字段管理
            ShowPlaceHolderByFieldType("1");
        }
        if ("<%=FieldID %>" != "0" && "<%=Field.FieldID%>" != "0") {
            ShowPlaceHolderByFieldType('<%=Field.ShowType %>');
            $("#FieldName").val('<%=Field.FieldName%>');
            $("#FieldName").attr("readonly", true);
            $('[name=IsAllowNull][value=<%=Field.IsAllowNull.ToStr().ToLower()=="true"?"1":"0" %>]').prop("checked", "checked");
            $("#DefaultValue").val('<%=Field.DefaultValue%>');
            $("#FieldAlias").val('<%=Field.FieldAlias%>');
            switch ('<%=Field.ShowType %>') {
                case "2":
                case "3":
                case "5":
                    // 选项
                    if ('<%=Field.ShowType %>' == "0" || '<%=Field.ShowType  %>' == '') {
                        break;
                    } else {
                        $('[name=BindType][value=<%=Field.BindType %>]').prop("checked", "checked");

                    ShowPlaceHolderByBindType();

                    if ('<%=Field.BindType %>' == '1') {
                        $("#BindText").val('<%=Field.BindText %>');
                        if ('<%=Field.BindText %>' != "") {
                            var BindText = '<%=Field.BindText %>'.split(',');
                            readHiddenToListbox();
                        }

                    } else if ('<%=Field.BindType %>' == '2') {
                        $("#BindSQL").val('<%=Field.BindSql %>');
                        $("#BindTextField").val('<%=Field.BindTextField %>');
                        $("#BindValueField").val('<%=Field.BindValueField %>');
                    } else if ('<%=Field.BindType %>' == '3') {
                        $("#BindTable").val('<%=Field.BindTable %>');
                        $("#BindKeyId").val('<%=Field.BindKeyID %>');
                        $("#BindTextField").val('<%=Field.BindTextField %>');
                        $("#BindValueField").val('<%=Field.BindValueField %>');
                    }
                        var selectedType = $("select[name=ShowType]").find("option:selected").val();
                        if (selectedType == "2" || selectedType == "3")
                            $("[name=divRepeatColumn]").show();
                        else
                            $("[name=divRepeatColumn]").hide();
                        $("#RepeatColumn").val('<%=Field.RepeatColumn %>');
                }
                    break;
            }
            $("#ShowType").val('<%=Field.ShowType%>');
            $("#ValidateType").val('<%=Field.ValidateType %>');
            $("#ValidateExpression").val('<%=Field.ValidateExpression %>');
            $("#ValidateText").val('<%=Field.ValidateText %>');
            $('[name=IsUsing][value=<%=Field.IsUsing.ToStr().ToLower()=="true"?"1":"0" %>]').prop("checked", "checked");
        }
        //提交内容
        var options = {
            fields: {
                FieldName: {
                    validators: {
                         regexp: {
                            regexp: /^[A-Za-z_][A-Za-z0-9_]{1,20}$/,
                            message: '<%="请输入不多于20个字符的字母数字组合，以字母开头".ToLang() %>'
                        }
                    }
                }
            }
        };
        $('#proFieldEdit').Validator(options,
            function (el) {
                var actionSuccess = el.attr("form-success");

                var $form = $("#proFieldEdit");
                $("[name=ShowType]").attr("disabled", false);
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "profieldlist.aspx");

                         } else {
                             whir.toastr.error(response.Message);
                         }
                         whir.loading.remove();
                     },
                     error: function (response) {
                         whir.toastr.error(response.Message);
                         whir.loading.remove();
                     }
                 });
                 return false;
             });
    </script>
</asp:Content>
