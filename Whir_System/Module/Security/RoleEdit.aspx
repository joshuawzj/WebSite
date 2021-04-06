<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="RoleEdit.aspx.cs" Inherits="Whir_System_Module_Setting_RoleEdit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="System.Collections.Generic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            
            <div class="panel-body">

                  <ul class="nav nav-tabs">
                    <li ><a  href="RoleList.aspx" aria-expanded="true"><%="角色管理".ToLang()%></a></li>
                    <li class="active"><a data-toggle="tab" href="#single" aria-expanded="true"> <%= (RoleId ==0?"添加角色":"编辑角色").ToLang()%></a></li>
                  </ul>
                <br />
 
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Module/Security/Roles.aspx">
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="ParentId">
                            <%="上级角色：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <select id="ParentId" name="ParentId" class="form-control"  >
                                 <% foreach (KeyValuePair<int, string> item in FirstSelect)
                                   { %>
                                <option value="<%= item.Key %>">
                                    <%= item.Value %></option>
                                <% } %>
                                <% foreach (KeyValuePair<int, string> item in Roles)
                                   { %>
                                <option value="<%= item.Key %>">
                                    <%= item.Value %></option>
                                <% } %>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="RoleName">
                            <%="角色名称：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="RoleName" name="RoleName" value="<%=CurrentRoles.RoleName %>"
                                class="form-control" required="true" minlength="1" maxlength="20" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Remarks">
                            <%="角色描述：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <textarea id="Remarks" name="Remarks" value="<%=CurrentRoles.Remarks %>" class="form-control"
                                maxlength="4000" rows="7"><%=CurrentRoles.Remarks %></textarea>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-offset-2 col-md-10 ">
                            <input type="hidden" name="RoleId" value="<%=CurrentRoles.RoleId %>" />
                            <input type="hidden" name="_action" value="Save" />
                            <div class="btn-group">
                                <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                <% if (PageMode == Whir.Service.EnumPageMode.Insert)
                                    { %>
                                <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                <% } %>
                            </div>
                            <a class="btn btn-white" href="rolelist.aspx"><%="返回".ToLang()%></a>
                        </div>
                    </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //绑定值
        $("#ParentId").val("<%=ParentRoleId>0?ParentRoleId:CurrentRoles.ParentId %>");
           <% if (CurrentRoles.RoleId == 1 || CurrentRoles.RoleId == 2) //root和admin 2个角色不允许有上级角色
          { %>
        $("#ParentId").attr("disabled", true);
        <% } %>

        var options = {
            fields: {
                RoleName: {
                    validators: {
                       
                        regexp: {
                            regexp: /^[\u4E00-\u9FA5A-Za-z0-9]+$/,
                            message: "<%="不能输入敏感字符，如：“*”、“/”".ToLang() %>"
                        }
                    }
                }
            }
        };
        $('#formEdit').Validator(options,
             function (el) {
                 var actionSuccess = el.attr("form-success");
                 var $form = $("#formEdit");
                 var parentId = $("#ParentId").val();
                 $form.post({
                     success: function (response) {
                         if (response.Status == true) {
                             actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "rolelist.aspx");
                         } else {
                             whir.toastr.error(response.Message);
                         }
                         whir.loading.remove();
                     },
                     error: function (response) {
                         whir.loading.remove();
                     }
                 });
                 return false;
             });
    </script>
</asp:Content>
