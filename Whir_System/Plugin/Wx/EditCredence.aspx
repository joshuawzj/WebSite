<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="EditCredence.aspx.cs" Inherits="Whir_System_Plugin_Wx_EditCredence" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Import Namespace="Whir.Service" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        table td {
            border-bottom: 1px solid #EAEFF2;
        }
        table td {
            padding: 9px 4px;
        }
    </style>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
     <div class="content-wrap">
        <div class="space15"></div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="WxCredence.aspx" aria-expanded="true"><%="公众号管理".ToLang() %></a></li>
                    <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%=Credence.AppId==null? "添加公众号".ToLang():"编辑公众号".ToLang()%></a></li>
                </ul>
                <div class="space15"></div>
                <div class="form_center">
                    <div class="panel-body">
                        <div class="tab-content">
                            <div id="single" class="tab-pane active">
                                <form id="formEdit" enctype="multipart/form-data" runat="server" class="form-horizontal" form-url="">
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="OriginalId">
                                            <%="原始Id：".ToLang()%></div>
                                        <div class="col-md-10 ">
                                            <input type="text" id="OriginalId" name="OriginalId" value="<%=this.Credence.OriginalId %>"
                                                class="form-control" required="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="WxAccount">
                                            <%="微信号：".ToLang()%></div>
                                        <div class="col-md-10 ">
                                            <input type="text" id="Text1" name="WxAccount" value="<%=this.Credence.WxAccount %>"
                                                class="form-control" required="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="AppId">
                                            <%="AppId：".ToLang()%></div>
                                        <div class="col-md-10 ">
                                            <input type="text" id="AppId" name="AppId" value="<%=this.Credence.AppId %>"
                                                class="form-control" required="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="AppSecret">
                                            <%="AppSecret：".ToLang()%></div>
                                        <div class="col-md-10 ">
                                            <input type="text" id="AppSecret" name="AppSecret" value="<%=this.Credence.AppSecret %>"
                                                class="form-control" required="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="AppName">
                                            <%="AppName：".ToLang()%></div>
                                        <div class="col-md-10 ">
                                            <input type="text" id="AppName" name="AppName" value="<%=this.Credence.AppName %>"
                                                class="form-control" required="true" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-md-2 control-label" for="Token">
                                            <%="Token：".ToLang()%></div>
                                        <div class="col-md-10 ">
                                            <input type="text" id="Token" name="Token" value="<%=this.Credence.Token %>"
                                                class="form-control" required="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-offset-2 col-md-10 ">
                                            <input type="hidden" name="_action" value="SaveCredence" />
                                            <div class="btn-group">
                                                <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                                <% if (PageMode == EnumPageMode.Insert)
                                                { %>
                                                <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                                <% } %>
                                            </div>
                                            <a class="btn btn-white" href="WxCredence.aspx"><%="返回".ToLang()%></a>
                                        </div>
                                    </div>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">

        var options = {
            fields: {
                AppId: {
                    validators: {
                        notEmpty: {
                            message: '<%="AppId为必填项".ToLang() %>'
                        }
                    }
                },
                AppSecret: {
                    validators: {
                        notEmpty: {
                            message: '<%="AppSecret为必填项".ToLang() %>'
                        }
                    }
                },
                AppName: {
                    validators: {
                        notEmpty: {
                            message: '<%="AppName为必填项".ToLang() %>'
                        }
                    }
                },
                Token: {
                    validators: {
                        notEmpty: {
                            message: '<%="Token为必填项".ToLang() %>'
                        }
                    }
                }
            }
        };
        $("#formEdit").Validator(options,
             function (el) {
                 var actionSuccess = el.attr("form-success");
                 var $form = $("#formEdit");
                 $form.attr("form-url", "<%=SysPath%>Plugin/Wx/Ajax/Ajax.aspx");
                 $form.post({
                     success: function (response) {
                         if (response.Status == true) {
                             actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "WxCredence.aspx");
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

