<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="ModelEdit.aspx.cs" Inherits="Whir_System_Module_Developer_ModelEdit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="ModelList.aspx" aria-expanded="true"><%="功能模型".ToLang()%></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%="添加功能模块".ToLang() %></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/Model.aspx">
                        <%if(ModelId==0){ %>
                        <div class="form-group">
                            <div class="col-md-2 control-label"  >
                                <%="选择模块：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                               <select id="ModuleMark" name="ModuleMark" class="form-control" required="true">
                                    <option value="">请选择</option>
                                    <% foreach (var module in AllModule.Where(p => p.ParentId == 0))
                                       {
                                            %>
                                    <option value="<%=module.ModuleId%>" <%=module.ModuleName==CurrentModel.ModelName? "selected='selected'":""%> ><%=module.ModuleName%></option>
                                    <%  }   %>
                                </select>
                            </div>
                        </div>
                        <%} %>
                        <div class="form-group">
                            <div class="col-md-2 control-label"  >
                                <%="功能模块名称：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="ModelName" name="ModelName" value="<%=CurrentModel.ModelName%>"
                                        class="form-control"
                                        required="true"
                                        maxlength="60"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label">
                                <%="功能模块描述：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <textarea type="text" id="ModelDesc" name="ModelDesc" class="form-control" rows="6"><%=CurrentModel.ModelDesc%></textarea>
                            </div>
                        </div>
                          <%if(ModelId==0){ %>
                        <div class="form-group">
                            <div class="col-md-2 control-label"  >
                                <%="数据表名：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <div class="input-group">
                                    <span class="input-group-addon">Whir_U_</span>
                                    <input type="text" id="TableName" name="TableName" 
                                        class="form-control" style="width: 200px;"
                                        value="<%=CurrentModel.TableName%>"
                                        required="true"
                                        maxlength="20" />
                                </div>
                            </div>
                        </div>
                        <%} %>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="ModelId" value="<%=CurrentModel.ModelId%>" />
                                <input type="hidden" name="CreateDate" value="<%=CurrentModel.CreateDate.Value(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")%>" />
                                <input type="hidden" name="CreateUser" value="<%=CurrentModel.CreateUser??CurrentUserName%>" />
                                <input type="hidden" name="UpdateDate" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>" />
                                <input type="hidden" name="UpdateUser" value="<%=CurrentUserName%>" />
                                <input type="hidden" name="_action" value="Save" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                </div>
                                <a class="btn btn-white" href="ModelList.aspx"><%="返回".ToLang()%></a>
                            </div>
                        </div>

                    </form>
                </div>
            </div>
        </div>
    </div>
   <script>
       //提交内容
       var options = {
           fields: {
               ModuleMark: {
                   validators: {
                       notEmpty: {
                           message: '<%="模块为必选项".ToLang() %>'
                       }
                   }
               },
               ModelName: {
                   validators: {
                       notEmpty: {
                           message: '<%="功能模块名称为必填项".ToLang() %>'
                       }
                   }
               },
               TableName: {
                   validators: {
                       notEmpty: {
                           message: '<%="数据表名为必填项".ToLang() %>'
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
                            actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "ModelList.aspx");
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
