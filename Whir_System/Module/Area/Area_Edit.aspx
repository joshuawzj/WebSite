<%@ Page Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Area_Edit.aspx.cs" Inherits="Whir_System_Module_Area_Area_Edit" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <form id="formSingle" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/Column.aspx">
                <div class="panel-body">
                    <ul class="nav nav-tabs">
                        <li><a href="AreaList.aspx" aria-expanded="true"><%="系统区域设置".ToLang()%></a></li>
                        <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%="编辑区域".ToLang()%></a></li>
                    </ul>
                    <div class="space15"></div>
                    <div class="tab-content">
                        <div id="single" class="tab-pane active">
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="Pid">
                                    <%="上级类别：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <select id="Pid" name="Pid" class="form-control">
                                        <option value=""><%="==" + "请选择".ToLang() + "==" %></option>
                                        <% foreach (var column in Alist)
                                           { %>
                                        <option value="<%=column.Id%>"><%=column.Name%></option>
                                        <% } %>
                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="Name">
                                    <%="区域名称：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <input id="Name" name="Name" maxlength="30" class="form-control" value="<%=CurrentArea.Name%>"
                                        required="true" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="EnName">
                                    <%="区域英文名称：".ToLang()%>
                                </div>
                                <div class="col-md-10 ">
                                    <input id="EnName" name="EnName" maxlength="30" class="form-control" value="<%=CurrentArea.EnName%>"
                                        required="true" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10 ">
                                    <input type="hidden" name="Id" value="<%=CurrentArea.Id%>" />
                                    <input type="hidden" name="_action" value="SaveArea" />
                                    <div class="btn-group">
                                        <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                        <% if (AreaId == 0)
                                            { %>
                                        <button form-action="submit" form-success="refresh" class="btn btn-info"><%= "提交并继续".ToLang() %></button>
                                        <% } %>
                                    </div>
                                    <a class="btn btn-white" href="AreaList.aspx"><%="返回".ToLang()%></a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        $("#Pid").val("<%=Pid %>");
        $('#formSingle').Validator(null, function (el) {
            var actionSuccess = el.attr("form-success");
            var $form = $("#formSingle");
            $form.post({
                success: function (response) {
                    if (response.Status == true) {
                        actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "AreaList.aspx");
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
