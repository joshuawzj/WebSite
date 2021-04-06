<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="Order_Edit.aspx.cs" Inherits="whir_system_Plugin_shop_order_Order_Edit" %>

<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <form runat="server" id="formTake">
        <div class="content-wrap">
            <div class="space15"></div>
            <div class="panel">
                <div class="panel-body ">
                    <ul class="nav nav-tabs">
                        <li><a href="OrderList.aspx" aria-expanded="true"><%="订单列表".ToLang()%></a></li>
                        <li class="active"><a href="<%=Request.Url%>" data-toggle="tab" aria-expanded="true"><%="订单详情".ToLang() %></a></li>
                    </ul>
                    <div class="space15"></div>
                    <div class="row form-horizontal">
                        <div class="col-md-6">
                            <div class="panel panel-default ">
                                <div class="panel-heading">
                                    <%="基本信息".ToLang()%>
                                </div>
                                <div class="panel-body">
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="订单编号：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 " style="min-height: 35px; margin-top: 5px;">
                                            <asp:Literal ID="ltOrderNo" runat="server"></asp:Literal>
                                            <input type="hidden" name="OrderID" id="OrderID" value="<%=ShopOrderInfo.OrderID %>" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="商品总额：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 " style="min-height: 35px; margin-top: 5px;">
                                            ￥<asp:Literal ID="ltProductAmount" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="支付状态：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 " style="min-height: 35px; margin-top: 5px;">
                                            <asp:Literal ID="ltPayState" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="订单状态：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 " style="min-height: 35px; margin-top: 5px;">
                                            <asp:Literal ID="ltStatus" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="购买会员：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 " style="min-height: 35px; margin-top: 5px;">
                                            <asp:Literal ID="ltMember" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="下单时间：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 " style="min-height: 35px; margin-top: 5px;">
                                            <asp:Literal ID="ltCreatedate" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="完成时间：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 " style="min-height: 35px; margin-top: 5px;">
                                            <asp:Literal ID="ltFinishDate" runat="server"></asp:Literal>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="panel panel-default ">
                                <div class="panel-heading">
                                    <%="收货信息".ToLang()%>
                                </div>
                                <div class="panel-body  ">

                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="收货人：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 ">
                                            <input id="TakeName" value="<%=ShopOrderInfo.TakeName %>" name="TakeName" class="form-control" required="true"
                                                maxlength="20" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="地址：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 ">
                                            <div class="input-group ">
                                                <input id="txtArea" name="Area" class="form-control" readonly="readonly" value="<%=Whir.Service.ServiceFactory.AreaService.GetParentsName(ShopOrderInfo.TakeRegion) %>" />
                                                <input type="hidden" name="TakeRegion" id="TakeRegion" value="<%=ShopOrderInfo.TakeRegion %>" required="true" />
                                                <span class="input-group-addon pointer" onclick="$('#txtArea').val('');$('#TakeRegion').val('');formRequiredValidator('txtArea',false); "><%="清空".ToLang()%></span>
                                                <span class="input-group-addon pointer" id="PickerArea" onclick="openAreaPicker();"><%="选择".ToLang()%></span>
                                            </div>
                                            <script type="text/javascript">
                                                $('#txtArea').click(function () {
                                                    openAreaPicker();
                                                });
                                                function openAreaPicker() {
                                                    var url = '<%=AppName%>whir_system/modulemark/common/area_select.aspx?arealevel=3&callback=setAreaPickerValue';
                                                    whir.dialog.frame('<%="选择地区".ToLang()%>', url, null, 400, 350);
                                                }
                                                function setAreaPickerValue(value, text) {
                                                    $('#txtArea').val(text);
                                                    $('#TakeRegion').val(value);
                                                    formRequiredValidator('txtArea', true);
                                                    whir.dialog.remove();
                                                }
                                            </script>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="街道信息：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 ">
                                            <input id="txtTakeAddress" value="<%=ShopOrderInfo.TakeAddress %>" name="TakeAddress" class="form-control" required="true"
                                                maxlength="100" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="手机号码：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 ">
                                            <input id="TakeMobile" value="<%=ShopOrderInfo.TakeMobile %>" name="TakeMobile" class="form-control"
                                                maxlength="12" required="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="固定电话：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 ">
                                            <input id="TakeTel" value="<%=ShopOrderInfo.TakeTel %>" name="TakeTel" class="form-control" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="邮箱地址：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 ">
                                            <input id="TakeEmail" value="<%=ShopOrderInfo.TakeEmail %>" name="TakeEmail" class="form-control"
                                                maxlength="100" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="邮政编码：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 ">
                                            <input id="TakePostcode" value="<%=ShopOrderInfo.TakePostcode %>" name="TakePostcode" class="form-control"
                                                maxlength="10" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-offset-3 col-md-9 ">
                                            <input type="button" class="btn btn-info" value="<%="保存".ToLang()%>" form-action="submit" />

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <%="商品信息".ToLang()%>
                                </div>
                                <div class="panel-body form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-12 ">
                                            <table class="Product_Infotable" cellpadding="0" cellspacing="0" border="0" width="100%">

                                                <tr class="th_second_title">
                                                    <th><%="订单商品".ToLang()%>
                                                    </th>
                                                    <th><%="商品名称".ToLang()%>
                                                    </th>
                                                    <th><%="金额".ToLang()%>
                                                    </th>
                                                    <th><%="数量".ToLang()%>
                                                    </th>
                                                    <th><%="小计".ToLang()%>
                                                    </th>
                                                </tr>
                                                <asp:Repeater ID="rpList" runat="server">
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td width="20%">
                                                                <a target="_blank" href="<%=AppName %>Shop/productinfo.aspx?proid=<%# Eval("ProID") %>&aids=<%# Eval("AttrValueIDs") %>">
                                                                    <img width="100" height="100" alt='<%#Eval("ProName") %>' src='<%=UploadFilePath %><%# Eval("ProImg") %>' />
                                                                </a>
                                                            </td>
                                                            <td width="50%">
                                                                <a target="_blank" href="<%=AppName %>Shop/productinfo.aspx?proid=<%# Eval("ProID") %>&aids=<%# Eval("AttrValueIDs") %>">
                                                                    <%# Eval("ProName") %>
                                                                    <br />
                                                                    <%# Eval("ProNo") %>
                                                                </a>
                                                            </td>
                                                            <td width="10%">
                                                                <font>￥<%# Eval("SaleAmount").ToDecimal().ToString("f2") %></font></td>
                                                            <td width="10%">
                                                                <%# Eval("Count") %>
                                                            </td>
                                                            <td width="10%">
                                                                <font>￥<%# (Eval("SaleAmount").ToDecimal()*Eval("Count").ToInt()).ToString("f2") %></font></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <%="支付及配送方式".ToLang()%>
                                </div>
                                <div class="panel-body form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="支付方式：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 ">
                                            <whir:DropDownList CssClass="form-control" ID="ddlPayment" runat="server">
                                            </whir:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3 control-label">
                                            <%="配送方式：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 ">
                                            <whir:DropDownList CssClass="form-control" ID="ddlCourier" runat="server">
                                            </whir:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-offset-3 col-md-9 ">
                                            <input type="button" class="btn btn-info" value="<%="保存".ToLang()%>" onclick="savePay();" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <%="结算信息".ToLang()%>
                                </div>
                                <div class="panel-body form-horizontal">
                                    <div class="form-group">
                                        <div class="col-md-3  control-label">
                                            <%="合计：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 " style="min-height: 35px; margin-top: 5px;">
                                            <div class="input-group ">
                                                <input id="TotalAmount" type="text" maxlength="8" readonly="readonly" max="99999999"
                                                    name="TotalAmount" class="form-control" value="<%=ShopOrderInfo.ProductAmount.ToString("f2")%>" />
                                                <span class="input-group-addon "><%="元".ToLang()%></span>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-md-3  control-label">
                                            <%="调整金额：".ToLang()%>
                                        </div>
                                        <div class="col-md-9" style="min-height: 35px; margin-top: 5px;">
                                            <div class="input-group ">
                                                <input id="DiscountAmount" type="text" maxlength="8" max="99999999"
                                                    name="DiscountAmount" class="form-control" value="<%=ShopOrderInfo.DiscountAmount.ToString("f2")%>"
                                                    onchange="discountAmount();" />
                                                <span class="input-group-addon "><%="元".ToLang()%></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3  control-label">
                                            <%="总金额：".ToLang()%>
                                        </div>
                                        <div class="col-md-9 " style="min-height: 35px; margin-top: 5px;">
                                            <div class="input-group ">
                                                <input id="PayAmount" type="text" readonly="readonly" maxlength="8" max="99999999"
                                                    name="PayAmount" class="form-control" value="<%=ShopOrderInfo.PayAmount.ToString("f2")%>" />
                                                <span class="input-group-addon "><%="元".ToLang()%></span>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-offset-3 col-md-9 ">
                                            <input type="button" class="btn btn-info" value="<%="保存".ToLang()%>" onclick="saveAmount();" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript">
         
        var options = {
            fields: {
                 TakeMobile: {
                    validators: {
                        regexp: {
                            regexp: /^0?(13[0-9]|14[5-9]|15[012356789]|166|17[0-8]|18[0-9]|19[8-9])[0-9]{8}$/i,
                            message: '<%="手机号码不正确".ToLang()%>'
                        }
                    }
                }, TakeEmail: {
                    validators: {
                        regexp: {
                            regexp: /^\w+([-+.]\w+)*@\w+([-.]\\w+)*\.\w+([-.]\w+)*$/i,
                            message: '<%="邮箱格式不正确".ToLang()%>'
                        }
                    }
                }  , TakePostcode: {
                    validators: {
                        regexp: {
                            regexp: /[1-9]\d{5}(?!\d)/i,
                            message: '<%="邮政编码格式不正确".ToLang()%>'
                        }
                    }
                }  , 
                submitButtons: '[form-action="submit"]'
            }
        }
 

        $("#formTake").Validator(options,saveTakeInfo);

        function saveTakeInfo() {
                var OrderID = $("#OrderID").val();
                var TakeName = $("#TakeName").val();
                var TakeMobile = $("#TakeMobile").val();
                var TakeTel = $("#TakeTel").val();
                var TakeRegion = $("#TakeRegion").val();
                var TakeAddress = $("#TakeAddress").val();
                var TakePostcode = $("#TakePostcode").val();
                var TakeEmail = $("#TakeEmail").val();
                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/OrderForm.aspx",
                        {
                            data: {
                                _action: "Save",
                                OrderID: OrderID,
                                TakeName: TakeName,
                                TakeMobile: TakeMobile,
                                TakeTel: TakeTel,
                                TakeRegion: TakeRegion,
                                TakeAddress: TakeAddress,
                                TakePostcode: TakePostcode,
                                TakeEmail: TakeEmail
                            },
                            success: function (response) {
                                whir.loading.remove();
                                if (response.Status == true) {
                                    whir.toastr.success(response.Message);
                                } else {
                                    whir.toastr.error(response.Message);
                                }
                            }
                        }
                    );
            }
            function saveAmount() {
                var OrderID = $("#OrderID").val();
                var TotalAmount = $("#TotalAmount").val();
                var DiscountAmount = $("#DiscountAmount").val();
                var PayAmount = $("#PayAmount").val();

                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/OrderForm.aspx",
                        {
                            data: {
                                _action: "SaveAmount",
                                OrderID: OrderID,
                                TotalAmount: TotalAmount,
                                DiscountAmount: DiscountAmount,
                                PayAmount: PayAmount
                            },
                            success: function (response) {
                                whir.loading.remove();
                                if (response.Status == true) {
                                    whir.toastr.success(response.Message);
                                } else {
                                    whir.toastr.error(response.Message);
                                }
                            }
                        }
                    );
            }
            function savePay() {
                var OrderID = $("#OrderID").val();
                var PaymentID = $("#<%=ddlPayment.ClientID%>").val();
                var CourierID = $("#<%=ddlCourier.ClientID%>").val();

                whir.ajax.post("<%=SysPath%>Handler/Plugin/shop/OrderForm.aspx",
                            {
                                data: {
                                    _action: "Save",
                                    OrderID: OrderID,
                                    PaymentID: PaymentID,
                                    CourierID: CourierID
                                },
                                success: function (response) {
                                    whir.loading.remove();
                                    if (response.Status == true) {
                                        whir.toastr.success(response.Message);
                                    } else {
                                        whir.toastr.error(response.Message);
                                    }
                                }
                            }
                        );
            }

            $(function () {
                var IsCanEdit=<%=IsCanEdit?"1":"0"%>;
                if(IsCanEdit=="0")
                {
                    $("#formTake input").each(function(i){ 
                        $(this).attr("disabled","disabled");
                    });
                }
                $(".pointer").unbind("click");
            });

            function discountAmount() {
                var discountAmount = $("#DiscountAmount").val();
                var totalAmount = $("#TotalAmount").val();
                $("#PayAmount").val(parseFloat(discountAmount)+parseFloat(totalAmount));
            }
    </script>

</asp:Content>
