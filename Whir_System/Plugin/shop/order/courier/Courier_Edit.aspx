<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master" AutoEventWireup="true" CodeFile="courier_edit.aspx.cs" Inherits="whir_system_Plugin_shop_order_courier_courier_edit" %>

<%@ Import Namespace="Whir.Language" %>
<%@ Register Src="../../common/HeadContainer.ascx" TagName="HeadContainer" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <uc1:HeadContainer ID="HeadContainer2" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrap">
        <div class="space15">
        </div>
        <div class="panel">
            <div class="panel-body">
                <ul class="nav nav-tabs">
                    <li><a href="courierlist.aspx" aria-expanded="true"><%="配送方式管理".ToLang()%></a></li>
                    <li class="active"><a href="courier_edit.aspx" data-toggle="tab" aria-expanded="true"><%="添加配送方式".ToLang()%></a></li>
                </ul>
                <div class="panel-body">
                    <div id="single" class="tab-pane active">
                        <form id="formEdit" class="form-horizontal" form-url="<%=SysPath%>Handler/Plugin/shop/CourierForm.aspx">
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="CourierName">
                                    <%="配送方式名称：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input id="CourierName" name="CourierName" class="form-control" maxlength="30"
                                        value="<%=CurrentShopCourier.CourierName%>" required="true" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-2 control-label" for="Com">
                                    <%="快递公司代码：".ToLang()%>
                                </div>
                                <div class="col-md-8 ">
                                    <input id="Com" name="Com" class="form-control" maxlength="30"
                                        value="<%=CurrentShopCourier.Com%>" required="true" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-offset-2 col-md-10 ">
                                    <input type="hidden" name="_action" value="SaveCourier" />
                                    <input type="hidden" name="CourierID" value="<%=CourierID %>" />
                                    <div class="btn-group">
                                        <button form-action="submit" form-success="back" class="btn btn-info"><%="提交并返回".ToLang()%></button>
                                        <button form-action="submit" form-success="refresh" class="btn btn-info"><%="提交并继续".ToLang()%></button>
                                    </div>
                                    <a class="btn btn-white" href="courierlist.aspx"><%="返回".ToLang()%></a>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $('#formEdit').Validator(null,
        function (el) {
            var actionSuccess = el.attr("form-success");
            var $form = $("#formEdit");
            $form.post({
                success: function (response) {
                    if (response.Status == true) {
                        actionSuccess == "refresh" ? whir.toastr.success(response.Message, true, false) : whir.toastr.success(response.Message, true, false, "courierlist.aspx?courierid=<%= CourierID %>");

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

