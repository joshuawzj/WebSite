<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Column_Link.aspx.cs" Inherits="Whir_System_Module_Column_Column_Link" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Service" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="ColumnList.aspx" aria-expanded="true"><%="栏目结构".ToLang()%></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true">
                        <asp:Literal runat="server" ID="litProccess"></asp:Literal></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/Column.aspx">
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="ParentId">
                                <%="上级栏目：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <select id="ParentId" name="ParentId" class="form-control">
                                    <option value="0">
                                        <%="==" + "请选择".ToLang() + "=="%></option>
                                    <% foreach (var column in Columns)
                                        {
                                    %>
                                    <option value="<%=column.ColumnId%>">
                                        <%=column.ColumnName%></option>
                                    <%  }   %>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="ColumnName">
                                <%="栏目名称：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="ColumnName" name="ColumnName" value="<%=Column.ColumnName%>" class="form-control" maxlength="100" />
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="OutUrl">
                                <%="链接地址：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="OutUrl" name="OutUrl" value="<%=Column.OutUrl%>" class="form-control" maxlength="1024" />
                                <span class="note_gray">
                                    <%="以“http://”开始代表外网地址，以“/”开始代表本站根目录，最多可输入1024个字符".ToLang() %></span>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 text-right" for="IsShow"><%="是否前台显示：".ToLang()%></div>
                            <div class="col-md-10 ">
                                <ul class="list">
                                    <li>
                                        <input type="radio" id="IsShow_True" name="IsShow" value="1" <%=Column.IsShow?"checked=\"checked\"":"" %> />
                                        <label for="IsShow_True"><%="是".ToLang()%></label>
                                    </li>
                                    <li>
                                        <input type="radio" id="IsShow_False" name="IsShow" value="0" <%=!Column.IsShow?"checked=\"checked\"":"" %> />
                                        <label for="IsShow_False"><%="否".ToLang()%></label>
                                    </li>
                                </ul>
                            </div>
                        </div>
                        <div class="form-group" id="div_submit" runat="server">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="ColumnId" value="<%=Column.ColumnId%>" />
                                <input type="hidden" name="_action" value="SaveOutLink" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <% if (PageMode == EnumPageMode.Insert)
                                        { %>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%= "提交并继续".ToLang() %></button>
                                    <% } %>
                                </div>
                                <a class="btn btn-white" href="ColumnList.aspx"><%="返回".ToLang()%></a>
                            </div>
                        </div>
                        <div class="form-group" id="div_save" runat="server">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="ColumnId" value="<%=Column.ColumnId%>" />
                                <input type="hidden" name="_action" value="SaveOutLink" />
                                <button form-action="submit" form-success="back" class="btn btn-primary">
                                    <%="保存".ToLang()%></button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $("#ParentId").val("<%=Column.ParentId %>");
        var options = {
            fields: {
                OutUrl: {
                    validators: {
                        notEmpty: {
                            message: '<%="链接地址为必填项".ToLang() %>'
                        }
                    }
                }, ColumnName: {
                    validators: {
                        notEmpty: {
                            message: '<%="栏目名称为必填项".ToLang() %>'
                        }
                    }
                }
            }
        };
        $('#formEdit').Validator(options,
            function (el) {
                var actionSuccess = el.attr("form-success");
                var $form = $('#formEdit');
                $form.post({
                    success: function (response) {
                        if (response.Status == true) {
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "ColumnList.aspx");
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
