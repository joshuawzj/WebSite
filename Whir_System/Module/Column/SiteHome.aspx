<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="SiteHome.aspx.cs" Inherits="Whir_System_Module_Column_SiteHome" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap"  id="sitehome-form">
        <div class="space15">
        </div>
        <div class="panel">
         
            <div class="panel-body">
                 
                <ul class="nav nav-tabs">
                    <li><a href="ColumnList.aspx" aria-expanded="true"><%="栏目结构".ToLang()%></a></li>
                    <li class="active"><a href="#" data-toggle="tab" aria-expanded="true"><%="网站首页".ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/Site.aspx">
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="ParentId">
                            <%="站点名称：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <%=SiteInfo.SiteName%>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="ParentId">
                            <%="站点目录：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                           <%=SiteInfo.Path.IsEmpty()?"":"/" + SiteInfo.Path + "/"%>
                        </div>
                    </div>
                    <div class="form-group attr-edit-text">
                        <div class="col-md-2 text-right" for="CreateMode">
                            <%="生成模式：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <ul class="list">
                                <li>
                                    <input type="radio" id="CreateMode_No" name="CreateMode" value="0" />
                                    <label for="CreateMode_No">
                                         <%="不生成".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="CreateMode_Static" name="CreateMode" value="1" />
                                    <label for="CreateMode_Static">
                                         <%="生成静态".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="CreateMode_Dynamic" name="CreateMode" value="2" />
                                    <label for="CreateMode_Dynamic">
                                         <%="生成动态".ToLang()%></label>
                                </li>
                            </ul>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="DefaultTemp">
                            <%="首页模板：".ToLang()%></div>
                        <div class="col-md-10 ">
                            <div class="input-group ">
                                <input id="DefaultTemp" name="DefaultTemp" class="form-control" readonly="readonly"
                                    value="<%=SiteInfo.DefaultTemp%>" />
                                <span class="input-group-addon pointer" for="DefaultTemp" dialog-url="TemplateSelector.aspx">
                                     <%="选择".ToLang()%></span> <span class="input-group-addon pointer" onclick="$('#DefaultTemp').val('');$('#formEdit').data('bootstrapValidator').updateStatus('DefaultTemp', 'INVALID', 'notEmpty');">
                                         <%="清空".ToLang()%></span>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10 ">
                            <input type="hidden" name="SiteId" value="<%=CurrentSiteId%>" />
                            <input type="hidden" name="_action" value="Save" />
                            <button form-action="submit" form-success="back" class="btn btn-info">
                                <%="保存".ToLang()%></button>
                             <a class="btn btn-white" href="ColumnList.aspx"><%="返回".ToLang()%></a>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $("[name='CreateMode'][value='<%=SiteInfo.CreateMode%>']").prop("checked", "checked");

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
                //需对表单验证
                $("#formEdit").data('bootstrapValidator').updateStatus('DefaultTemp', 'NOT_VALIDATED', 'notEmpty');
                dialog.remove();
            }, 800);
        });

        //提交内容
        var options = {
            fields: {
                DefaultTemp: {
                    validators: {
                        notEmpty: {
                            message: '<%="请选择首页模板".ToLang() %>'
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
                             whir.toastr.success(response.Message);
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
