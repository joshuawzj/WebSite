<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="AttachedRelease_Edit.aspx.cs" Inherits="whir_system_module_release_attachedrelease_edit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Service" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="AttachedRelease.aspx" aria-expanded="true"><%="附带模板设置".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%="编辑模板设置".ToLang() %></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Module/Release/Release.aspx">
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Names"><%="名称：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <input id="txtNames" name="Names" class="form-control" value="<%=CurrentAttached.Names%>"
                                required="true" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="ColumnId"><%="所属栏目：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <select id="ColumnId" name="ColumnId" class="form-control">
                                <option value=""><%="请选择".ToLang()%></option>
                                <% foreach (var model in ServiceFactory.ColumnService.GetList(0, CurrentSiteId))
                                   {
                                %>
                                <option value="<%=model.ColumnId%>">
                                    <%=model.ColumnName%></option>
                                <%  }   %>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="TemplateUrl"><%="模板文件：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <div class="input-group ">
                                <input id="TemplateUrl" name="TemplateUrl" class="form-control" readonly="readonly" value="<%=CurrentAttached.TemplateUrl%>" required="true" />
                                <span class="input-group-addon pointer" for="TemplateUrl" dialog-url="<%=SysPath%>module/column/TemplateSelector.aspx?jscallback=fill_txtDefaultTemp"><%="选择".ToLang()%></span>
                                <span class="input-group-addon pointer" onclick="$('#TemplateUrl').val('');"><%="清空".ToLang()%></span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="CreateFileUrl"><%="生成文件：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <input id="txtCreateFileUrl" name="CreateFileUrl" class="form-control" value="<%=CurrentAttached.CreateFileUrl%>"
                                required="true" />
                            <%="页面名称不可用index、list或info开头，".ToLang()%>
                            <%="页面名称不可包含下划线，".ToLang()%>
                            <%="可输入路径，相于站点目录，如gsxw/aboutindex.html".ToLang()%>
                        </div>
                    </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="AttachedId" value="<%=CurrentAttached.AttachedId%>" />
                                <input type="hidden" name="_action" value="Save" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <% if (CurrentAttached.AttachedId == 0)
                                        { %>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%= "提交并继续".ToLang() %></button>
                                    <% } %>
                                </div>
                                <a class="btn btn-white" href="attachedrelease.aspx"><%="返回".ToLang()%></a>
                            </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //弹出层
        $("[dialog-url]").click(function () {
            var dialogUrl = $(this).attr("dialog-url");
            var targetControl = $(this).attr("for");

            var dialog = whir.dialog.frame("选择模板", dialogUrl, function () {
                var frameDocument = dialog.find("iframe").contents();
                var checked = frameDocument.find("input[type='radio'][name='template']:checked");
                if (checked.length != 1) {
                    whir.toastr.warning("请选择一个模板");
                    return;
                }
                $(document.getElementById(targetControl)).val(checked.val());
                dialog.remove();
            }, 800);
        });
        $("[name='ColumnId']").val("<%=CurrentAttached.ColumnId%>");
        var options = {
            fields: {
                CreateFileUrl: {
                    validators: {
                        regexp: {
                            regexp: /^[^index][^_]*|^[^list][^_]*|^[^info][^_]*/i,
                            message: '<%="格式不正确".ToLang() %>'
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
                             actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "attachedrelease.aspx");
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
