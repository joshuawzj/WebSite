<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="MenuEdit.aspx.cs" Inherits="Whir_System_Module_Developer_MenuEdit" %>

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
                    <li><a href="MenuList.aspx" aria-expanded="true"><%="菜单管理".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%="编辑菜单".ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Developer/Menu.aspx">
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="MenuName">
                                <%="菜单名称：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="MenuName" name="MenuName" value="<%=CurrentMenu.MenuName%>"
                                        class="form-control"
                                        required="true"
                                        maxlength="24"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="MenuIcon">
                                <%="菜单图标：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="MenuIcon" name="MenuIcon" value="<%=CurrentMenu.MenuIcon%>"
                                        class="form-control"  
                                        required="true"
                                        maxlength="24"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Sort">
                                <%="排序：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="Sort" name="Sort" value="<%=CurrentMenu.Sort%>"
                                       class="form-control" value="0"  
                                       required="true"/>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 text-right" for="Sort">
                                <%="是否显示：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                              <ul class="list" id="text1">
                                <li>
                                    <input type="radio" id="IsShow0" name="IsShow" value="1"  <%=CurrentMenu.IsShow?"checked='checked'":""%>"/>
                                    <label for="IsShow0"> <%="是".ToLang()%></label>
                                </li>
                                <li>
                                    <input type="radio" id="IsShow1" name="IsShow" value="0" <%=!CurrentMenu.IsShow?"checked='checked'":""%>"/>
                                    <label for="IsShow1"> <%="否".ToLang()%></label>
                                </li>
                            </ul>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Url">
                                <%="链接地址：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <input type="text" id="Url" name="Url" value="<%=CurrentMenu.Url??"#"%>"
                                       class="form-control"
                                       required="True"
                                       maxlength="128"/>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="MenuType">
                                <%="展现形式：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <select id="MenuType" name="MenuType" class="form-control">
                                    <option value="top"><%="上方图标导航".ToLang()%></option>
                                    <option value="left"><%="左侧菜单导航".ToLang()%></option>
                                    <option value="right"><%="右侧快捷导航".ToLang()%></option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="Target">
                                <%="打开方式：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <select id="Target" name="Target" class="form-control">
                                    <option value="child"><%="当前页面打开".ToLang()%></option>
                                    <option value="open"><%="新页面打开".ToLang()%></option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-2 control-label" for="ParentId">
                                <%="父菜单：".ToLang()%>
                            </div>
                            <div class="col-md-10 ">
                                <select id="ParentId" name="ParentId" class="form-control">
                                    <option value="0"><%="根目录".ToLang()%></option>
                                    <% foreach (var menu in MenuTreeList)
                                       {
                                    %>
                                    <option value="<%=menu.MenuId%>"><%=menu.MenuName%></option>
                                    <% } %>
                                </select>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-md-offset-2 col-md-10 ">
                                <input type="hidden" name="MenuId" value="<%=CurrentMenu.MenuId%>" />
                                <input type="hidden" name="CreateDate" value="<%=CurrentMenu.CreateDate.Value(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss")%>" />
                                <input type="hidden" name="CreateUser" value="<%=CurrentMenu.CreateUser??CurrentUserName%>" />
                                <input type="hidden" name="UpdateDate" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")%>" />
                                <input type="hidden" name="UpdateUser" value="<%=CurrentUserName%>" />
                                <input type="hidden" name="_action" value="Save" />
                                <div class="btn-group">
                                    <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                    <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                </div>
                                <a class="btn btn-white" href="MenuList.aspx"><%="返回".ToLang()%></a>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
            
        </div>
    </div>
    <script type="text/javascript">
        //绑定值
        $("[name='Target']").val("<%=CurrentMenu.Target%>");
        $("[name='MenuType']").val("<%=CurrentMenu.MenuType%>");
        $("[name='ParentId']").val("<%=CurrentMenu.ParentId%>");

        //提交内容
        $("[form-action='submit']").click(function () {
            var actionSuccess = $(this).attr("form-success");

            var $form = $("#formEdit");
            $form.post({
                success: function (response) {
                    if (response.Status == true) {
                        actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "MenuList.aspx?");

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
