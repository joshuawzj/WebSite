<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Admin_Edit.aspx.cs" Inherits="Whir_System_Module_Setting_Admin_Edit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Framework" %>
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
                    <li ><a  href="adminlist.aspx?type=<%=Whir.Framework.RequestUtil.Instance.GetQueryInt("type",0) %>&rolesid=<%=Whir.Framework.RequestUtil.Instance.GetQueryInt("rolesid",0) %>" aria-expanded="true"><%="管理员管理".ToLang()%></a></li>
                    <li class="active"><a data-toggle="tab" href="#single" aria-expanded="true"> <%= (UserId==0?"添加管理员":"编辑管理员").ToLang()%></a></li>
                  </ul>
                <br />
                 
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Module/Security/Admin.aspx">
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="LoginName">
                            <%="用户名：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="LoginName" name="LoginName" value="<%=CurrenUsers.LoginName %>"
                                class="form-control" required="true" minlength="4" maxlength="20" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Password">
                            <%="密码：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="password" id="Password" name="Password" value="" class="form-control"
                                minlength="6" maxlength="20" />
                                <span class="note_gray"> <%=AppSettingUtil.GetString("IsStrongPassword").ToBoolean(true)?"密码必须包含数字、字母、特殊符号，长度在6-20位".ToLang():"密码长度在6-20位".ToLang() %></span>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Password2">
                            <%="确认密码：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="password" id="Password2" name="Password2" value="" class="form-control"
                                minlength="6" maxlength="20" />
                         
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="Email">
                            <%="电子邮箱：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="Email" name="Email" value="<%=CurrenUsers.Email %>" class="form-control" maxlength="50" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-md-2 control-label" for="RealName">
                            <%="真实姓名：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <input type="text" id="RealName" name="RealName" value="<%=CurrenUsers.RealName %>"
                                class="form-control" required="true" maxlength="50" />
                        </div>
                    </div>
                    <div class="form-group" id="divRole">
                        <div class="col-md-2 control-label" for="RolesId">
                            <%="所属角色：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <select id="RolesId" name="RolesId" class="form-control" required >
                                <option value="">
                                    <%="==请选择==".ToLang()%></option>
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
                        <div class="col-md-2 control-label" for="Remarks">
                            <%="备注：".ToLang()%>
                        </div>
                        <div class="col-md-10 ">
                            <textarea id="Remarks" maxlength="1000" class="form-control" name="Remarks" rows="7"><%=CurrenUsers.Remarks %></textarea>
                        </div>
                    </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="UserId" value="<%=CurrenUsers.UserId %>" />
                                <input type="hidden" name="_action" value="Save" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <% if (PageMode == Whir.Service.EnumPageMode.Insert)
                                        { %>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                    <% } %>
                                </div>
                                <a class="btn btn-white" href="adminlist.aspx?type=<%=RequestUtil.Instance.GetQueryInt("type",0) %>&rolesid=<%=Whir.Framework.RequestUtil.Instance.GetQueryInt("rolesid",0) %>"><%="返回".ToLang()%></a>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        //绑定值
        $("#RolesId").val("<%=CurrenUsers.RolesId %>");
      
         <% if (PageMode == Whir.Service.EnumPageMode.Update)
          { %>
                 $("#LoginName").attr("disabled", true);
        <% } %>

        <% if (CurrenUsers.RolesId==1)
          { %>
        $("#RolesId").attr("disabled", true);
          <% } %>

        var regexp = /^[^\s]*$/, message = "<%="6-20字符,除空格外任意字符组合".ToLang() %>";
        <%if (AppSettingUtil.GetString("IsStrongPassword").ToBoolean(true)) {%>
            regexp = /^(?=.*[A-Za-z])(?=.*[0-9])(?=.*[^\sA-Za-z0-9])\S{6,20}$/;
            message = "<%="密码必须包含数字、字母、特殊符号，长度在6-20位".ToLang() %>";
        <%}%>
        var options = {
            fields: {
                LoginName: {
                    validators: {
                        
                        regexp: {
                            regexp: /^[a-zA-Z][a-zA-Z0-9_]{3,20}$/,
                            message: '<%="4-20字符,以字母开头的字母、数字、下划线组合".ToLang()%>'
                        }
                    }
                },
                Password: {
                    validators: {
                       regexp: {
                           regexp: regexp,
                           message: message
                       }
                        <%if(UserId==0){%>
                        ,notEmpty: {
                            message: '<%="请输入密码".ToLang() %>'
                        }
                          <%}%>
                    }
                }, 
                Password2: {
                    validators: {
                        regexp: {
                            regexp: regexp,
                            message: message
                        }, identical: {
                            field: 'Password',
                            message: '<%="两次密码不一致".ToLang() %>'
                        }
                        <%if(UserId==0){%>
                        ,notEmpty: {
                            message: '<%="请输入确认密码".ToLang() %>'
                        }
                    <%}%>
                    }
                },
               
                Email: {
                    validators: {
                        emailAddress: {
                            message: '<%="请输入正确的邮件地址".ToLang() %>'
                        }
                    }
                },
                RolesId: {
                    validators: {
                        min: -1,
                        message: "<%="请选择所属角色".ToLang() %>"
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
                             actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "AdminList.aspx");
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
