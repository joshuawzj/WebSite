<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="subjectform_edit.aspx.cs" Inherits="whir_system_module_subject_subjectform_edit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="<%=SysPath%>res/js/Whir/whir.FormField.js" type="text/javascript"></script>
    <style type="text/css">
        .col-md-2 {
            padding-left: 10px;
            padding-right: 10px;
        }
    </style>
    <script type="text/javascript">
        var _AllowPicType = '<%=UploadConfig.AllowPicType %>', _AllowFileType = '<%=UploadConfig.AllowFileType %>';
        //选择已有字段, 批量添加到表单中
        function addFieldToForm() {
            var selected = "";
            $("#tbdFieldList")
                .find("input[type='checkbox']:checked")
                .each(function () {
                    var fieldId = $(this).attr("FieldId");
                    selected += fieldId + ",";
                });
            if (selected == "") {
                TipMessage('<%="请选择".ToLang()%>');
                return false;
            } else {
                selected = selected.substring(0, selected.length - 1);
                $("#hidSelected").val(selected);
                whir.ajax.post("<%=SysPath%>Handler/Module/Column/Form.aspx",
                    {
                        data: {
                            _action: "AddFieldToForm",
                            selected: selected,
                            MaxLength: $("#MaxLength").val(),
                            ColumnId: '<%=ColumnId %>'
                        },
                        success: function (response) {
                            whir.loading.remove();
                            if (response.Status == true) {
                                whir.toastr.success(response.Message, true, false);
                            } else {
                                whir.toastr.error(response.Message);
                            }
                        }
                    }
                );
                return false;
            }
        }

        //打开批量推送页面
        function openPush() {
            var selected = "";
            $("#tbdFieldList").find("input[type='checkbox']:checked").each(function () {
                var fieldId = $(this).attr("FieldId");
                selected += fieldId + ",";
            });
            if (selected == "") {
                TipMessage('<%="请选择".ToLang()%>');
                return false;
            }
            else {
                selected = selected.substring(0, selected.length - 1);
                $("#hidSelected").val(selected);
                whir.dialog.frame('<%="选择栏目推送".ToLang()%>', "<%=SysPath%>Module/Column/FormColumn_Select.aspx?columnid=<%= ColumnId %>&selectedtype=checkbox&callback=copyAction", null, 800, 600);
                return false;
            }

        }

        function copyAction(json) {
            json = eval('(' + json + ')');
            var column = "";
            $(json).each(function (idx, item) {
                column += item.id + ",";
            });
            if (column.length > 0)
                column = column.substring(0, column.length - 1);
            $("#hidSelectedOpen").val(column);
            whir.ajax.post("<%=SysPath%>Handler/Module/Column/Form.aspx",
                {
                    data: {
                        _action: "PushForm",
                        selectedOpen: $("#hidSelectedOpen").val(),
                        selected: $("#hidSelected").val()
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            whir.dialog.remove();
                            whir.toastr.success(response.Message);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                }
            );
            return false;
        }

        //全选
        function selectAll(flag) {
            var flg = "check";
            if (!flag) {
                flg = "uncheck";
            }
            $("#cbxAll").iCheck(flg);
            $("#tbdFieldList").find("input[type='checkbox']").iCheck(flg);
        }

        $(function () {
            //表头全选
            $("#cbxAll").next().click(function () {
                var checked = $("#cbxAll").prop('checked');
                selectAll(checked);
            });
            $('#DateDefaultValue').focus(function () { WdatePicker({ lang: '<%=Lang %>', dateFmt: $("#DateFormat").val() }); });
        });

        $(function () {

            //数据库字段名自动翻译
            $("#FieldName").blur(function () {
                var fieldAlias = $("#FieldAlias");
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
                            whir.toastr.warning("翻译失败");
                        }
                    });
                }
            });

            //表单别名自动翻译
            $("#FieldAlias").blur(function () {
                var fieldName = $("#FieldName");
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
                                whir.toastr.warning("翻译失败");
                            }
                        });

                    }
            });
        });

        //打开调用关联
        function openLinkAttr() {
            var dialogUrl = "<%=SysPath%>Module/Column/LinkAttr_Select.aspx?FieldId=<%=FieldId %>&FormId=<%=FormId %>";
            var targetControl = $(this).attr("for");
            var dialog = whir.dialog.frame("调用属性", dialogUrl, function () {
                var frameDocument = dialog.find("iframe").contents();
                var checked = frameDocument.find("input[type='radio'][name='linkattr']:checked");
                if (checked.length != 1) {
                    whir.toastr.warning("请选择一个属性");
                    return;
                }
                $(document.getElementById(targetControl)).val(checked.val());
                GetLinkAttr(checked.val());
                dialog.remove();
            }, 800);
        }

        //异步加载数据
        function GetLinkAttr(id) {
            whir.ajax.post("<%=SysPath%>Handler/Module/Column/Form.aspx",
                {
                    data: {
                        _action: "LinkAttr",
                        LinkFormId: id
                    },
                    success: function (response) {
                        whir.loading.remove();
                        if (response.Status == true) {
                            var result = eval('(' + response.Message + ')');
                            SetFieldValue(result);
                        } else {
                            whir.toastr.error(response.Message);
                        }
                    }
                }
            );
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="SubjectList.aspx?subjecttypeid=<%=SubjectTypeId %>" aria-expanded="true"><%= SubjectTypeId == 1 ? "子站管理".ToLang() : "专题管理".ToLang()%></a></li>
                    <li><a href="SubjectFormList.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&columnid=<%= ColumnId %>" aria-expanded="true"><%=ColumnName%>-<%="表单".ToLang()%></a></li>
                    <li class="active"><a href="<%=Request.Url%>" aria-expanded="true">编辑表单</a></li>
                </ul>
                <div class="space15"></div>

                <asp:PlaceHolder ID="phFields" runat="server">
                    <div class="panel panel-default panels col-md-4" style="width: 49%; margin-right: 5px; padding-left: 0px; padding-right: 0px">
                        <div class="panel-heading">
                            <%="已有表单项".ToLang()%>
                        </div>
                        <div class="panel-body">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="controller table table-bordered table-noPadding">
                                <tr class="trClass">
                                    <th>
                                        <div class="th-inner ">
                                            <input type="checkbox" id="cbxAll" />
                                        </div>
                                    </th>
                                    <th>Id
                                    </th>
                                    <th>
                                        <%="字段名".ToLang()%>
                                    </th>
                                    <th>
                                        <%="表单别名".ToLang()%>
                                    </th>
                                    <th>
                                        <%="表单类型".ToLang()%>
                                    </th>
                                    <th>
                                        <%="编辑".ToLang()%>
                                    </th>
                                </tr>
                                <tbody id="tbdFieldList">
                                    <asp:Repeater ID="rptFieldList" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <input type="checkbox" id='cbx<%# Eval("FieldId") %>' fieldid='<%# Eval("FieldId") %>' />
                                                </td>
                                                <td>
                                                    <%# Eval("FieldId") %>
                                                </td>
                                                <td>
                                                    <%# Eval("FieldName") %>
                                                </td>
                                                <td>
                                                    <%# Eval("FieldAlias") %>
                                                </td>
                                                <td>
                                                    <asp:Literal ID="litFieldType" runat="server"></asp:Literal>
                                                </td>
                                                <td>
                                                    <a href='javascript:GetLinkAttr(<%# Eval("FieldId") %>);' class="fontawesome-double-angle-right"></a>

                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <asp:PlaceHolder ID="phNoField" runat="server" Visible="false">
                                        <tr id="no_data">
                                            <td colspan="6" height="200px" align="center">
                                                <%="无可用字段".ToLang()%>
                                            </td>
                                        </tr>
                                    </asp:PlaceHolder>
                                </tbody>
                            </table>
                            <div class="operate_foot">
                                <input type="hidden" name="hidSelected" id="hidSelected" />
                                <input type="hidden" name="hidSelectedOpen" id="hidSelectedOpen" />
                                <div class="btn-group">
                                    <a href="javascript:selectAll(true);" class="btn btn-sm btn-white">
                                        <%="全选".ToLang()%></a> <a href="javascript:selectAll(false);" class="btn btn-sm btn-white">
                                            <%="取消".ToLang()%></a>
                                </div>
                                <div class="btn-group">
                                    <a href="javascript:addFieldToForm();" class="btn btn-sm btn-white">
                                        <%="批量添加".ToLang()%></a> <a href="javascript:openPush();" class="btn btn-sm btn-white">
                                            <%="表单推送".ToLang()%></a>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
                <div class="panel panel-default panels col-md-4" style="width: 50%; padding-right: 0px; padding-left: 0px">
                    <div class="panel-heading">
                        <%="表单项信息".ToLang()%>
                    </div>
                    <div class="panel-body">
                        <div class="form_center" style="width: 100%;">
                            <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Module/Column/Form.aspx">
                                <div class="form-group">
                                    <div class="col-md-3 control-label" for="FieldType">
                                        <%="表单类型：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <select id="FieldType" required="true" name="FieldType" class="form-control">
                                            <option value="">==请选择==</option>
                                            <option value="1">
                                                <%="单行文本".ToLang()%></option>
                                            <option value="2">
                                                <%="多行文本".ToLang()%></option>
                                            <option value="3">
                                                <%="HTML".ToLang()%></option>
                                            <option value="4">
                                                <%="选项".ToLang()%></option>
                                            <option value="5">
                                                <%="数字".ToLang()%></option>
                                            <option value="6">
                                                <%="货币".ToLang()%></option>
                                            <option value="7">
                                                <%="日期和时间".ToLang()%></option>
                                            <%--<option value="8"><%="超链接".ToLang()%></asp:ListItem>--%>
                                            <option value="9">
                                                <%="是/否".ToLang()%></option>
                                            <option value="10">
                                                <%="图片".ToLang()%></option>
                                            <option value="11">
                                                <%="文件".ToLang()%></option>
                                            <option value="13">
                                                <%="地区".ToLang()%></option>
                                            <option value="14">
                                                <%="密码型字段".ToLang()%></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-3 control-label" for="FieldName">
                                        <%="数据库字段名：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <div class="input-group ">
                                            <input id="FieldName" name="FieldName" class="form-control" value="" required="true"
                                                maxlength="20" validationexpression="[a-zA-Z]{1}[a-zA-Z0-9]{0,19}" errormessage="<%="请输入不多于20个字符的字母数字组合，以字母开头".ToLang() %>" />
                                            <span class="input-group-addon pointer" id="divLinkAttr" onclick=" openLinkAttr();" for="hidLinkFormId">
                                                <%="调用属性".ToLang() %></span>
                                        </div>
                                        <input type="hidden" id="hidLinkFormId" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-3 control-label" for="FieldAlias">
                                        <%="表单别名：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="FieldAlias" name="FieldAlias" class="form-control" value="" required="true"
                                            maxlength="20" />
                                    </div>
                                </div>
                                <div class="form-group" name="divIsAllowNull">
                                    <div class="col-md-3 text-right" for="IsAllowNull">
                                        <%="是否允许为空：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
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
                                <div class="form-group" name="divIsOnly">
                                    <div class="col-md-3 text-right" for="IsOnly">
                                        <%="是否唯一：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="IsOnly_True" name="IsOnly" value="1" />
                                                <label for="IsOnly_True">
                                                    <%="是".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="IsOnly_False" checked="checked" name="IsOnly" value="0" />
                                                <label for="IsOnly_False">
                                                    <%="否".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divIsReadOnly">
                                    <div class="col-md-3 text-right" for="IsReadOnly">
                                        <%="是否只读：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="IsReadOnly_True" name="IsReadOnly" value="1" />
                                                <label for="IsReadOnly_True">
                                                    <%="是".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="IsReadOnly_False" checked="checked" name="IsReadOnly" value="0" />
                                                <label for="IsReadOnly_False">
                                                    <%="否".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divDefaultValue">
                                    <div class="col-md-3 control-label" for="DefaultValue">
                                        <%="默认值：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="DefaultValue" name="DefaultValue" class="form-control" value="" maxlength="100" />
                                    </div>
                                </div>
                                <div class="form-group" name="divMaxLength">
                                    <div class="col-md-3 control-label" for="MaxLength">
                                        <%="最大长度：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="MaxLength" name="MaxLength" class="form-control" validationexpression="\d+$"
                                            errormessage="<%="必须为非负整数".ToLang()%>" value="250" maxlength="5" />
                                        <span class="note_gray">&nbsp;&nbsp;<%="值为空或为0时，长度不受限制".ToLang()%></span>
                                    </div>
                                </div>
                                <div class="form-group" name="divIsColor">
                                    <div class="col-md-3 text-right" for="IsReadOnly">
                                        <%="取色器：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="IsColor_True" name="IsColor" value="1" />
                                                <label for="IsColor_True">
                                                    <%="启用".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="IsColor_False" checked="checked" name="IsColor" value="0" />
                                                <label for="IsColor_False">
                                                    <%="不启用".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divIsBold">
                                    <div class="col-md-3 text-right" for="IsReadOnly">
                                        <%="加粗控件：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="IsBold_True" name="IsBold" value="1" />
                                                <label for="IsBold_True">
                                                    <%="启用".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="IsBold_False" checked="checked" name="IsBold" value="0" />
                                                <label for="IsBold_False">
                                                    <%="不启用".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divLengthCalc">
                                    <div class="col-md-3 text-right" for="IsLengthCalc">
                                        <%="自动计算长度：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="LengthCalc_True" name="IsLengthCalc" value="1" />
                                                <label for="LengthCalc_True">
                                                    <%="启用".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="LengthCalc_False" checked="checked" name="IsLengthCalc" value="0" />
                                                <label for="LengthCalc_False">
                                                    <%="不启用".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-3 control-label" for="TipText">
                                        <%="提示文字：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="TipText" name="TipText" class="form-control" value="" maxlength="200" />
                                    </div>
                                </div>
                                <div class="form-group" name="divValidateText">
                                    <div class="col-md-3 control-label" for="ValidateText">
                                        <%="错误提示文字：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="ValidateText" name="ValidateText" class="form-control" value="" maxlength="200" />
                                    </div>
                                </div>
                                <div class="form-group" name="divValidateType">
                                    <div class="col-md-3 control-label" for="ValidateType">
                                        <%="验证类型：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <select id="ValidateType" name="ValidateType" class="form-control">
                                            <option value="">
                                                <%="==请选择==".ToLang()%></option>
                                            <option value="<%="电子邮箱".ToLang()%>">
                                                <%="电子邮箱".ToLang()%></option>
                                            <option value="<%="手机号码".ToLang()%>">
                                                <%="手机号码".ToLang()%></option>
                                            <option value="<%="电话号码".ToLang()%>">
                                                <%="电话号码".ToLang()%></option>
                                            <option value="<%="手机号码/电话号码".ToLang()%>">
                                                <%="手机号码/电话号码".ToLang()%></option>
                                            <option value="<%="QQ".ToLang()%>">
                                                <%="QQ".ToLang()%></option>
                                            <option value="<%="身份证号码".ToLang()%>">
                                                <%="身份证号码".ToLang()%></option>
                                            <option value="<%="金额".ToLang()%>">
                                                <%="金额".ToLang()%></option>
                                            <option value="<%="数字".ToLang()%>">
                                                <%="数字".ToLang()%></option>
                                            <option value="<%="整数".ToLang()%>">
                                                <%="整数".ToLang()%></option>
                                            <option value="<%="正整数".ToLang()%>">
                                                <%="正整数".ToLang()%></option>
                                            <option value="<%="负整数".ToLang()%>">
                                                <%="负整数".ToLang()%></option>
                                            <option value="<%="非负整数".ToLang()%>">
                                                <%="非负整数".ToLang()%></option>
                                            <option value="<%="日期".ToLang()%>">
                                                <%="日期".ToLang()%></option>
                                            <option value="<%="日期时间".ToLang()%>">
                                                <%="日期时间".ToLang()%></option>
                                            <option value="<%="中文".ToLang()%>">
                                                <%="中文".ToLang()%></option>
                                            <option value="<%="英文".ToLang()%>">
                                                <%="英文".ToLang()%></option>
                                            <option value="<%="Url地址".ToLang()%>">
                                                <%="Url地址".ToLang()%></option>
                                            <option value="<%="安全字符".ToLang()%>">
                                                <%="安全字符".ToLang()%></option>
                                            <option value="<%="长度".ToLang()%>">
                                                <%="长度".ToLang()%></option>
                                            <option value="<%="邮编".ToLang()%>">
                                                <%="邮编".ToLang()%></option>
                                            <option value="<%="IP地址".ToLang()%>">
                                                <%="IP地址".ToLang()%></option>
                                            <option value="<%="自定义表达式".ToLang()%>">
                                                <%="自定义表达式".ToLang()%></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group" name="divValidateExpression">
                                    <div class="col-md-3 control-label" for="ValidateExpression">
                                        <%="验证正则：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="ValidateExpression" name="ValidateExpression" class="form-control" value=""
                                            maxlength="400" />
                                    </div>
                                </div>
                                <div class="form-group" name="divWidth">
                                    <div class="col-md-3 control-label" for="Width">
                                        <%="宽度：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="Width" name="Width" class="form-control" value="" validationexpression="\d*"
                                            maxlength="4" />
                                    </div>
                                </div>
                                <div class="form-group" name="divHeight" style="display: none">
                                    <div class="col-md-3 control-label" for="Height">
                                        <%="高度：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="Height" name="Height" class="form-control" value="" validationexpression="\d*"
                                            maxlength="4" />
                                    </div>
                                </div>
                                <div class="form-group" name="divContentPagingParam" style="display: none">
                                    <div class="col-md-3 control-label" for="ContentPagingParam">
                                        <%="内容分页参数：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="ContentPagingParam" name="ContentPagingParam" class="form-control" value=""
                                            validationexpression=".{0,250}" errormessage="<%="最多只能输入250个字符".ToLang()%>" maxlength="300" />
                                    </div>
                                </div>
                                <div class="form-group" name="divIsOpenCss" style="display: none">
                                    <div class="col-md-3 text-right" for="IsOpenCss">
                                        <%="是否引用Css：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="IsOpenCss_True" checked="checked" name="IsOpenCss" value="1" />
                                                <label for="IsOpenCss_True">
                                                    <%="是".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="IsOpenCss_False" name="IsOpenCss" value="0" />
                                                <label for="IsOpenCss_False">
                                                    <%="否".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divCssPath" style="display: none">
                                    <div class="col-md-3 control-label" for="ContentPagingParam">
                                        <%="引用Css路径：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="CssPath" name="CssPath" class="form-control" title="/站点目录/css/css_whir.css" maxlength="256" />
                                    </div>
                                </div>
                                <div class="form-group" name="divBindType" style="display: none">
                                    <div class="col-md-3 text-right" for="BindType">
                                        <%="绑定类型：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
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
                                            <li>
                                                <input type="radio" id="BindType_4" name="BindType" value="4" />
                                                <label for="BindType_4">
                                                    <%="指定当前栏目".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divBindText" style="display: none">
                                    <div class="col-md-3 control-label" for="ContentPagingParam">
                                        <%="文本选项：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <select id="seBindText" name="seBindText" multiple class="form-control" size="6">
                                        </select>
                                        <input type="hidden" name="BindText" id="BindText" />
                                        <div class="btn-group">
                                            <input type="button" value='<%="添加选项".ToLang()%>' id="btnAddOption" class="btn btn-white" />
                                            <input type="button" value='<%="编辑选项".ToLang()%>' id="btnModifyOption" class="btn btn-white" />
                                            <input type="button" value='<%="删除选项".ToLang()%>' id="btnDeleteOption" class="btn text-danger border-normal" />
                                        </div>
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
                                    <div class="col-md-3 control-label" for="BindSQL">
                                        <%="绑定的SQL脚本：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="BindSQL" name="BindSQL" class="form-control" value="" maxlength="512"
                                            validationexpression="^.{0,512}$" />
                                    </div>
                                </div>
                                <div class="form-group" name="divBindLevel" style="display: none">
                                    <div class="col-md-3 control-label" for="BindTable">
                                        <%="绑定的表名：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="BindTable" name="BindTable" class="form-control" value="" maxlength="200"
                                            validationexpression="^.{0,200}$" />
                                    </div>
                                </div>
                                <div class="form-group" name="divBindLevel" style="display: none">
                                    <div class="col-md-3 control-label" for="BindKeyId">
                                        <%="绑定的栏目Id：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="BindKeyId" name="BindKeyId" class="form-control" value="" maxlength="200"
                                            validationexpression="^.{0,200}$" />
                                    </div>
                                </div>
                                <div class="form-group" name="divField" style="display: none">
                                    <div class="col-md-3 control-label" for="BindTextField">
                                        <%="绑定的文本字段：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input name="BindTextField" id="BindTextField" class="form-control" value="" maxlength="300"
                                            validationexpression="^.{0,200}$" />
                                    </div>
                                </div>
                                <div class="form-group" name="divField" style="display: none">
                                    <div class="col-md-3 control-label" for="BindValueField">
                                        <%="绑定的值字段：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input name="BindValueField" id="BindValueField" class="form-control" value="" maxlength="200"
                                            validationexpression="^.{0,200}$" />
                                    </div>
                                </div>
                                <div class="form-group" name="divBindSelectedType" style="display: none">
                                    <div class="col-md-3 text-right" for="SelectedType">
                                        <%="显示选项：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="SelectedType_1" checked="checked" name="SelectedType" value="1" />
                                                <label for="SelectedType_1">
                                                    <%="下拉列表框".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="SelectedType_2" name="SelectedType" value="2" />
                                                <label for="SelectedType_2">
                                                    <%="单选按钮组".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="SelectedType_3" name="SelectedType" value="3" />
                                                <label for="SelectedType_3">
                                                    <%="多选按钮组".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="SelectedType_5" name="SelectedType" value="5" />
                                                <label for="SelectedType_5">
                                                    <%="方块弹窗单选".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="SelectedType_6" name="SelectedType" value="6" />
                                                <label for="SelectedType_6">
                                                    <%="方块弹窗多选".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divRepeatColumn" style="display: none">
                                    <div class="col-md-3 control-label" for="RepeatColumn">
                                        <%="每行显示数：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="Text2" name="RepeatColumn" class="form-control" value="" maxlength="3"
                                            validationexpression="^[0-9]*[1-9][0-9]*$" />
                                    </div>
                                </div>
                                <div class="form-group" name="divDateFormat" style="display: none">
                                    <div class="col-md-3 text-right" for="DateFormat">
                                        <%="显示格式：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="DateFormat_1" checked="checked" name="DateFormat" value="yyyy-mm-dd hh:ii:ss" />
                                                <label for="DateFormat_1">
                                                    <%="类似2012-08-25 17:16:30".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="DateFormat_2" name="DateFormat" value="yyyy-mm-dd" />
                                                <label for="DateFormat_2">
                                                    <%="类似2012-08-25".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divDateFormat" style="display: none">
                                    <div class="col-md-3 text-right" for="DateDefaultValue">
                                        <%="默认值：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="DateDefaultValue_1" name="DateDefaultValue" value="1" />
                                                <label for="DateDefaultValue_1">
                                                    <%="无".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="DateDefaultValue_2" checked="checked" name="DateDefaultValue"
                                                    value="2" />
                                                <label for="DateDefaultValue_2">
                                                    <%="当前时间".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="DateDefaultValue_3" name="DateDefaultValue" value="3" />
                                                <label for="DateDefaultValue_3">
                                                    <%="指定日期".ToLang()%></label>
                                            </li>
                                        </ul>
                                        <input id="txtDateDefaultValue" name="txtDateDefaultValue" class="form-control" style="display: none"
                                            value="" />
                                    </div>
                                </div>
                                <div class="form-group" name="divBoolDefaultValue" style="display: none">
                                    <div class="col-md-3 control-label" for="BoolDefaultValue">
                                        <%="默认值：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <select id="BoolDefaultValue" required="true" name="BoolDefaultValue" class="form-control">
                                            <option value="1">
                                                <%="是".ToLang()%></option>
                                            <option value="2" checked="checked">
                                                <%="否".ToLang()%></option>
                                        </select>
                                    </div>
                                </div>
                                <div class="form-group" name="divImageUploadMode" style="display: none">
                                    <div class="col-md-3 text-right" for="ImageUploadMode">
                                        <%="上传模式：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="ImageUploadMode_1" name="ImageUploadMode" value="1" />
                                                <label for="ImageUploadMode_1">
                                                    <%="单图片".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="ImageUploadMode_2" checked="checked" name="ImageUploadMode"
                                                    value="2" />
                                                <label for="ImageUploadMode_2">
                                                    <%="多图片".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divImageUploadMode" style="display: none">
                                    <div class="col-md-3 text-right" for="IsWatermark">
                                        <%="是否水印：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="IsWatermark_1" name="IsWatermark" value="0" />
                                                <label for="IsWatermark_1">
                                                    <%="否".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="IsWatermark_2" checked="checked" name="IsWatermark" value="1" />
                                                <label for="IsWatermark_2">
                                                    <%="是".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divFileUploadMode" style="display: none">
                                    <div class="col-md-3 text-right" for="FileUploadMode">
                                        <%="上传模式：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="FileUploadMode_1" checked="checked" name="FileUploadMode"
                                                    value="1" />
                                                <label for="FileUploadMode_1">
                                                    <%="单文件".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="FileUploadMode_2" name="FileUploadMode" value="2" />
                                                <label for="FileUploadMode_2">
                                                    <%="多文件".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group" name="divFileExts" style="display: none">
                                    <div class="col-md-3 control-label" for="FileExts">
                                        <%="文件扩展名：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <input id="FileExts" name="FileExts" class="form-control" value="" maxlength="200"
                                            validationexpression="^[A-z|0-9]{0,200}$" />
                                        <span class="note_gray">
                                            <%="多个文件类型之间用‘|’隔开".ToLang()%></span>
                                    </div>
                                </div>
                                <div class="form-group" name="divArea" style="display: none">
                                    <div class="col-md-3 text-right" for="ShowLevel">
                                        <%="显示格式：".ToLang()%>
                                    </div>
                                    <div class="col-md-9 ">
                                        <ul class="list">
                                            <li>
                                                <input type="radio" id="Area_1" name="ShowLevel" value="1" />
                                                <label for="Area_1">
                                                    <%="显示一级(省)".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="Area_2" name="ShowLevel" value="2" />
                                                <label for="Area_2">
                                                    <%="显示二级(省市)".ToLang()%></label>
                                            </li>
                                            <li>
                                                <input type="radio" id="Area_3" checked="checked" name="ShowLevel" value="3" />
                                                <label for="Area_3">
                                                    <%="显示三级(省市区)".ToLang()%></label>
                                            </li>
                                        </ul>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-offset-2 col-md-9 ">
                                        <input type="hidden" name="ColumnId" value="<%=ColumnId%>" />
                                        <input type="hidden" name="FormId" value="<%=FormId%>" />
                                        <input type="hidden" name="FieldId" id="FieldId" value="<%=FieldId%>" />
                                        <input type="hidden" name="_action" value="Save" />
                                        <div class="btn-group">
                                            <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                            <% if (FormId == 0)
                                                { %>
                                            <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                            <%  } %>
                                        </div>
                                        <a class="btn btn-white" href="SubjectFormList.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&columnid=<%= ColumnId %>"><%="返回".ToLang()%></a>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
                                            if ("<%=FieldId %>" != "0" || "<%=FormId %>" != "0") {
                                                $("[name=FieldType]").attr("disabled", true);
                                                $("[name=FieldName]").attr("readonly", true);

                                                //$("#divLinkAttr")
                                                //    .click(function () {
                                                //        openLinkAttr();
                                                //    });

                                                //字段管理
                                                ShowPlaceHolderByFieldType("1");
                                            } else {

                                                $("#divLinkAttr").addClass("disabled");
                                            }
                                            //绑定编辑项值
                                            if ("<%=Forms.FormId %>" == "0" && '<%=FormId %>' != '0') {
                                                whir.toastr.warning('<%="未找到FormID为{0}的表单输入对象,SubjectFormList.aspx?subjecttypeid=SubjectTypeId&subjectclassid=SubjectClassId&columnid=ColumnId".ToLang().FormatWith(FormId,ColumnId)%>');
                                            }
                                            else if ("<%=Field.FieldId %>" == "0" && '<%=FieldId %>' != '0') {
                                                whir.toastr
                                                    .warning('<%="为找到FieldID为{0}的字段对象,SubjectFormList.aspx?subjecttypeid=SubjectTypeId&subjectclassid=SubjectClassId&columnid=ColumnId".ToLang().FormatWith(Forms.FieldId,ColumnId)%>');
                                            } else if ("<%=FieldId %>" != "0" || "<%=FormId %>" != "0") {
                                                ShowPlaceHolderByFieldType('<%=Field.FieldType %>');
                                                $("#FieldType").val('<%=Field.FieldType %>');
                                                $("#FieldType").attr("disabled", true);
                                                $("#FieldName").val('<%=Field.FieldName %>');
                                            $("#FieldName").attr("readonly", true);

                                            $("#FieldAlias").val('<%=Forms.FieldAlias %>');
            $('[name=IsAllowNull][value=<%=Forms.IsAllowNull.ToStr().ToLower()=="true"?"1":"0" %>]').prop("checked", "checked");
            $('[name=IsOnly][value=<%=Forms.IsOnly.ToStr().ToLower()=="true"?"1":"0" %>]').prop("checked", "checked");
            $('[name=IsReadOnly][value=<%=Forms.IsReadOnly.ToStr().ToLower()=="true"?"1":"0" %>]').prop("checked", "checked");
            $("#MaxLength").val('<%=Forms.MaxLength>0?Forms.MaxLength.ToStr():"" %>');
            $('[name=IsColor][value=<%=Forms.IsColor.ToStr().ToLower()=="true"?"1":"0" %>]').prop("checked", "checked");
            $('[name=IsBold][value=<%=Forms.IsBold.ToStr().ToLower()=="true"?"1":"0" %>]').prop("checked", "checked");
            $('[name=IsLengthCalc][value=<%=Forms.IsLengthCalc.ToStr().ToLower()=="true"?"1":"0" %>]').prop("checked", "checked");

            $("#TipText").val('<%=Forms.TipText %>');
            $("#ValidateText").val('<%=Forms.ValidateText %>');
            $("#Width").val('<%=Forms.Width %>');
            $("#Height").val('<%=Forms.Height %>');
            $("#ContentPagingParam").val('<%=Forms.ContentPagingParam %>');
            $("#DefaultValue").val('<%=Forms.DefaultValue %>');
            $("#ValidateType").val('<%=Forms.ValidateType %>');
            $("#ValidateExpression").val('<%= Forms.ValidateExpression!=null&&Forms.ValidateExpression.Contains("\\")?Forms.ValidateExpression.Replace("\\","\\\\"):Forms.ValidateExpression %>');
            switch ('<%=Field.FieldType %>') {
                case "3":
                    $("#CssPath").val('<%=Forms.CssPath%>');
                    $('[name=IsOpenCss][value=<%=Forms.IsOpenCss?"1":"0" %>]')
                        .prop("checked", "checked");
                    break;
                case "4":
                    // 选项
                    if ('<%=FormOption.FormOptionId %>' == "0" || '<%=FormOption.FormOptionId %>' == '') {
                        break;
                    } else {
                        $('[name=BindType][value=<%=FormOption.BindType %>]').prop("checked", "checked");

                        ShowPlaceHolderByBindType();

                        if ('<%=FormOption.BindType %>' == '1') {
                            $("#BindText").val('<%=FormOption.BindText %>');
                            if ('<%=FormOption.BindText %>' != "") {
                                var BindText = '<%=FormOption.BindText %>'.split(',');
                                readHiddenToListbox();
                            }

                        } else if ('<%=FormOption.BindType %>' == '2') {
                            $("#BindSQL").val("<%=FormOption.BindSql %>");
                            $("#BindTextField").val('<%=FormOption.BindTextField %>');
                            $("#BindValueField").val('<%=FormOption.BindValueField %>');
                        } else if ('<%=FormOption.BindType %>' == '3') {
                            $("#BindTable").val('<%=FormOption.BindTable %>');
                            $("#BindKeyId").val('<%=FormOption.BindKeyId %>');
                            $("#BindTextField").val('<%=FormOption.BindTextField %>');
                            $("#BindValueField").val('<%=FormOption.BindValueField %>');
                        }

                        $('[name=SelectedType][value=<%=FormOption.SelectedType %>]').prop("checked", "checked");

                        if ('<%=FormOption.SelectedType %>' == "1" || '<%=FormOption.SelectedType %>' == "5") {
                            $("#divRepeatColumn").hide();
                        } else {
                            $("#divRepeatColumn").show();
                        }
                        $("#RepeatColumn").val('<%=FormOption.RepeatColumn %>');
                    }

                    break;
                case "7":
                    // 日期与时间
                    if ('<%=FormDate.FormDateId %>' == "0" || '<%=FormDate.FormDateId %>' == '') {
                        break;
                    } else {
                        $('[name=DateFormat]')
                            .each(function () {
                                if ($(this).val() == '<%=FormDate.DateFormat %>') {
                                    $(this).prop("checked", "checked");
                                }
                            });
                        switch ('<%=Forms.DefaultValue%>') {
                            case "1":
                            case "2":
                                $('[name=DateDefaultValue][value=<%=Forms.DefaultValue %>]').prop("checked", "checked");
                                break;
                            default:
                                $('[name=DateDefaultValue][value=3]').prop("checked", "checked");
                                $("#txtDateDefaultValue").val('<%=Forms.DefaultValue %>').show();
                                break;
                        }
                    }
                    break;
                case "9":
                    // 是/否
                    $('[name=BoolDefaultValue][value=<%=Forms.DefaultValue %>]').prop("checked", "checked");
                    break;
                case "10":
                    // 图片
                    if ('<%=FormUpload.FormUploadId %>' == "0" || '<%=FormUpload.FormUploadId %>' == '') {
                        break;
                    } else {
                        $('[name=ImageUploadMode][value=<%=FormUpload.UploadMode %>]').prop("checked", "checked");
                        $('[name=IsWatermark][value=<%=FormUpload.IsWaterMark.ToStr().ToLower()=="true"?"1":"0" %>]').prop("checked", "checked");
                        $("#FileExts").val('<%=FormUpload.FileExts %>');
                        $("#TipText").attr("required", true);
                        $("#Width").attr("required", true);
                        $("#Height").attr("required", true);
                    }

                    break;
                case "11":
                    // 文件
                    if ('<%=FileFormUpload.FormUploadId %>' == "0" || '<%=FileFormUpload.FormUploadId %>' == '') {
                        break;
                    } else {
                        $('[name=FileUploadMode][value=<%=FileFormUpload.UploadMode %>]').prop("checked", "checked");
                        $("#FileExts").val('<%=FileFormUpload.FileExts %>');
                        $("#TipText").attr("required", true);
                        $("#Width").attr("required", false);
                        $("#Height").attr("required", false);
                    }
                    break;
                case "13":
                    // 地区
                    if ('<%=FormArea.FormAreaId %>' == "0" || '<%=FormArea.FormAreaId %>' == '') {
                        break;
                    } else {
                        $('[name=ShowLevel][value=<%=FormArea.ShowLevel %>]').prop("checked", "checked");
                    }
                    break;
            }

            //系统字段除了字段名称之外的信息都不可修改
            $("#IsAllowNull").attr("readonly", '<%=Field.IsSystemField.ToStr().ToLower() %>' == false);
            $("#DefaultValue").attr("readonly", '<%=Field.IsSystemField.ToStr().ToLower() %>' == false);
            $("#IsColor").attr("readonly", '<%=Field.IsSystemField.ToStr().ToLower() %>' == false);
            $("#IsBold").attr("readonly", '<%=Field.IsSystemField.ToStr().ToLower() %>' == false);
            $("#IsLengthCalc").attr("readonly", '<%=Field.IsSystemField.ToStr().ToLower() %>' == false);
                                            }

                                            //提交内容
                                            var options = {
                                                fields: {
                                                    FieldName: {
                                                        validators: {
                                                            notEmpty: {
                                                                message: '<%="栏目名称为必填项".ToLang() %>'
                                                            }, regexp: {
                                                                regexp: /^[A-Za-z_][A-Za-z0-9_]{1,20}$/,
                                                                message: '<%="请输入不多于20个字符的字母数字组合，以字母开头".ToLang() %>'
                                                            }
                                                        }
                                                    }, FieldAlias: {
                                                        validators: {
                                                            notEmpty: {
                                                                message: '<%="表单别名为必填项".ToLang() %>'
                                                            }
                                                        }
                                                    },
                                                    Width: {
                                                        validators: {
                                                            integer: {
                                                                message: '<%="请输入正整数".ToLang() %>'
                                                        }
                                                    }
                                                },
                                                Height: {
                                                    validators: {
                                                        integer: {
                                                            message: '<%="请输入正整数".ToLang() %>'
                                                            }
                                                        }
                                                    }
                                                }
                                            };
                                            $('#formEdit').Validator(options,
                                                function (el) {
                                                    var actionSuccess = el.attr("form-success");

                                                    var $form = $("#formEdit");
                                                    $("[name=FieldType]").attr("disabled", false);
                                                    $form.post({
                                                        success: function (response) {
                                                            if (response.Status == true) {
                                                                actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "SubjectFormList.aspx?subjecttypeid=<%= SubjectTypeId %>&subjectclassid=<%= SubjectClassId %>&columnid=<%= ColumnId %>");
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
