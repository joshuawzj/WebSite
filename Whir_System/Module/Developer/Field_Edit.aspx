<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="field_edit.aspx.cs" Inherits="whir_system_module_developer_field_edit" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="ModelList.aspx" aria-expanded="true"><%="数据模型".ToLang() %></a></li>
                    <li><a href="FieldList.aspx?ModelId=<%=ModelId%>" aria-expanded="true"><%=Model.ModelName+" - 字段".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true">编辑字段</a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                 <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/Field.aspx">
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="FieldType"><%="字段类型：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <select id="FieldType" required="true" <%=FieldId>0?" disabled=\"disabled\"":"" %> name="FieldType" class="form-control">
                                <option value="1"><%="单行文本".ToLang()%></option>
                                <option value="2"><%="多行文本".ToLang()%></option>
                                <option value="3"><%="HTML".ToLang()%></option>
                                <option value="4"><%="选项".ToLang()%></option>
                                <option value="5"><%="数字".ToLang()%></option>
                                <option value="6"><%="货币".ToLang()%></option>
                                <option value="7"><%="日期和时间".ToLang()%></option>
                                <%--<option value="8"><%="超链接".ToLang()%></asp:ListItem>--%>
                                <option value="9"><%="是/否".ToLang()%></option>
                                <option value="10"><%="图片".ToLang()%></option>
                                <option value="11"><%="文件".ToLang()%></option>
                                <option value="13"><%="地区".ToLang()%></option>
                                <option value="14"><%="密码型字段".ToLang()%></option>
                            </select>
                        </div>
                    </div> 
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="FieldName"><%="数据库字段名：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <input id="FieldName"  <%=FieldId>0?" disabled=\"disabled\"":"" %>  name="FieldName" class="form-control" value="<%=Field.FieldName %>" required="true" 
                                maxlength="20"  errormessage="<%="请输入不多于20个字符的字母数字组合，以字母开头".ToLang() %>" />
                        </div>
                            </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="FieldAlias"><%="字段名称：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <input id="FieldAlias" name="FieldAlias" class="form-control" value="<%=Field.FieldAlias %>" required="true" maxlength="20" />
                        </div>
                      </div>
                    <div class="form-group">
                        <div class="col-md-2 text-right" for="FieldAlias"><%="是否系统字段：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <ul class="list">
                                <li>
                                    <input type="radio" id="IsSystemField_True" <%=Field.IsSystemField?"checked=\"checked\"":"" %> name="IsSystemField" value="1" />
                                    <label for="IsSystemField_True"><%="是".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="IsSystemField_False" <%=!Field.IsSystemField?"checked=\"checked\"":"" %> name="IsSystemField" value="0" />
                                    <label for="IsSystemField_False"><%="否".ToLang()%></label>
                                </li>
                            </ul>

                        </div>
                     </div>
                    <div class="form-group">
                            <div class="col-md-2 text-right" for="FieldAlias"><%="是否隐藏：".ToLang()%></div> 
                            <div class="col-md-10 ">
                                <ul class="list">
                                    <li>
                                        <input type="radio" id="IsHidden_True" <%=Field.IsHidden?"checked=\"checked\"":"" %> name="IsHidden" value="1" />
                                        <label for="IsHidden_True">
                                            <%="是".ToLang()%></label>
                                    </li>
                                    <li>
                                        <input type="radio" id="IsHidden_False" <%=!Field.IsHidden?"checked=\"checked\"":"" %> name="IsHidden" value="0" />
                                        <label for="IsHidden_False">
                                            <%="否".ToLang()%></label>
                                    </li>
                                </ul>

                            </div>
                        </div>
                     <div class="form-group">
                            <div class="col-md-2 text-right" for="IsDefaultForm"><%="默认加到表单：".ToLang()%></div> 
                            <div class="col-md-10 ">
                                <ul class="list">
                                    <li>
                                        <input type="radio" id="IsDefaultForm_True" <%=Field.IsDefaultForm?"checked=\"checked\"":"" %> name="IsDefaultForm" value="1" />
                                        <label for="IsDefaultForm_True">
                                            <%="是".ToLang()%></label>
                                    </li>
                                    <li>
                                        <input type="radio" id="IsDefaultForm_False" <%=!Field.IsDefaultForm?"checked=\"checked\"":"" %> name="IsDefaultForm" value="0" />
                                        <label for="IsDefaultForm_False">
                                            <%="否".ToLang()%></label>
                                    </li>
                                </ul>

                            </div>
                        </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10 ">
                            <input type="hidden" name="ModelId" value="<%=ModelId%>" />
                            <input type="hidden" name="FieldID" value="<%=FieldId%>" />
                            <input type="hidden" name="_action" value="Save" />
                            <div class="btn-group">
                                <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                            </div>
                            <a class="btn btn-white" href="FieldList.aspx?ModelId=<%=ModelId%>"><%="返回".ToLang()%></a>
                        </div>
                    </div>
                 </form>
               </div>
            </div>
        </div>
    </div>
    
    <script>

        $(function () {
            //绑定值
            $("#FieldType").val('<%=Field.FieldType%>');

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
                            fieldAlias.val(msg);
                            fieldAlias.change();
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
                            fieldName.val(msg);
                            fieldName.change();
                        },
                        error: function (response) {
                            whir.toastr.warning("翻译失败");
                        }
                    });

                }
            });
        });

        //提交内容
        var options = {
            fields: {
                FieldName: {
                    validators: {
                        notEmpty: {
                            message: '<%="数据库字段名为必填项".ToLang() %>'
                        }, regexp: {
                            regexp: /^[A-Za-z_][A-Za-z0-9_]{1,20}$/,
                            message: '<%="请输入不多于20个字符的字母数字组合，以字母开头".ToLang() %>'
                        }
                    }
                 },
                FieldAlias: {
                   validators: {
                       notEmpty: {
                           message: '<%="字段名称为必填项".ToLang() %>'
                       }
                   }
               }
           }
       };

       $('#formEdit').Validator(options,
            function (el) {
                var actionSuccess = el.attr("form-success");
                var $form = $("#formEdit");
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "FieldList.aspx?ModelId=<%=ModelId%>");
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
</asp:content>
