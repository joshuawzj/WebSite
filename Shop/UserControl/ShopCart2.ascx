<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ShopCart2.ascx.cs" Inherits="Shop_UserControl_ShopCart2" %>
<%@ Import Namespace="Whir.Framework" %>
<script type="text/javascript" src="<%=AppName %>res/js/area/AreaData_min_cn.js"></script>
<script type="text/javascript" src="<%=AppName %>res/js/area/Area.js"></script>
<script type="text/javascript">
    jQuery(function () {

        jQuery(":radio[name='PaymentIDs']").first().attr("checked", true);

        /*
        jQuery("#<%=TakeTel.ClientID %>").unbind("blur");
        jQuery("#<%=TakeMobile.ClientID %>").blur(function(){
        SettleAccounts_TakeTelClientValidate(jQuery(this).attr("id"), { Value: this.value, IsValid:true },true);
        });

       
        jQuery("#<%=TakeTel.ClientID %>").blur(function(){
        SettleAccounts_TakeTelClientValidate(jQuery(this).attr("id"), { Value: this.value, IsValid:true },false);
        });
        */

    })

</script>
<div class="ShopContain">
    <div class="MainBox">
        <!--Start-->
        <div class="myorder">
            <b></b>
        </div>
        <!--Content Start-->
        <!--地址-->
        <div class="ShopCart_txt">
            <h4>
                提交订单</h4>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="address">
                <tr>
                    <td class="name">
                        收 货 人
                    </td>
                    <td>
                        <whir:textbox id="TakeName" width="280px" cssclass="text" runat="server" required="True"
                            requirederrormessage="请填写收货人" maxlength="20" minlength="1"></whir:textbox>
                    </td>
                </tr>
                <tr>
                    <td class="name">
                        地 址：
                    </td>
                    <td id="td_selAddress">
                        <select id="shop_seachprov" onchange="changeComplexProvince(this.value, sub_array, 'shop_seachprov','shop_seachcity', 'shop_seachdistrict','<%=TakeRegion.ClientID %>');">
                        </select><select id="shop_seachcity" onchange="changeCity(this.value,'shop_seachcity','shop_seachdistrict','<%=TakeRegion.ClientID %>');"></select><span
                            id="shop_seachdistrict_div"><select id="shop_seachdistrict" onchange="setValue('shop_seachdistrict','<%=TakeRegion.ClientID %>')"></select>
                            <whir:textbox id="TakeRegion" width="280px" cssclass="text" style="display: none"
                                runat="server" required="True" message="请选择地址" requirederrormessage="请选择地址" maxlength="60"></whir:textbox>
                            <asp:HiddenField ID="hidArea" runat="server" />
                            <script type="text/javascript">

                                try {
                                    var areaPath = $("#<%=hidArea.ClientID %>").val().split(',');
                                    if (areaPath.length == 1) {
                                        initComplexArea('shop_seachprov', 'shop_seachcity', 'shop_seachdistrict', area_array, sub_array, sub_arr, areaPath[0]);
                                    } else if (areaPath.length == 2) {
                                        initComplexArea('shop_seachprov', 'shop_seachcity', 'shop_seachdistrict', area_array, sub_array, sub_arr, areaPath[0], areaPath[1]);
                                    }
                                    else {
                                        initComplexArea('shop_seachprov', 'shop_seachcity', 'shop_seachdistrict', area_array, sub_array, sub_arr, areaPath[0], areaPath[1], areaPath[2]);
                                    }

                                } catch (e) { }
                            </script>
                    </td>
                </tr>
                <tr>
                    <td class="name">
                        街道信息：
                    </td>
                    <td>
                        <whir:textbox id="TakeAddress" width="280px" cssclass="text" runat="server" required="True"
                            requirederrormessage="请填写街道信息" maxlength="100" minlength="1"></whir:textbox>
                    </td>
                </tr>
                <tr>
                    <td class="name">
                        手机号码：
                    </td>
                    <td>
                        <whir:textbox id="TakeMobile" width="280px" cssclass="text" runat="server" required="True"
                            requirederrormessage="请填写手机号码" regular="Custom" validationexpression="^0?(13[0-9]|14[5-9]|15[012356789]|166|17[0-8]|18[0-9]|19[8-9])[0-9]{8}$"
                            maxlength="13" errormessage="手机格式错误"></whir:textbox>
                    </td>
                </tr>
                <tr>
                    <td class="name">
                        固定电话：
                    </td>
                    <td>
                        <whir:textbox id="TakeTel" width="280px" cssclass="text" runat="server" required="True"
                            requirederrormessage="请填写固定电话" regular="Phone" errormessage="固定电话格式错误"></whir:textbox>
                    </td>
                </tr>
                <tr>
                    <td class="name">
                        邮箱地址：
                    </td>
                    <td>
                        <whir:textbox id="TakeEmail" width="280px" cssclass="text" runat="server" required="True"
                            regular="Email" message="*" requirederrormessage="请填写邮箱地址" maxlength="60"></whir:textbox>
                    </td>
                </tr>
                <tr>
                    <td class="name">
                        邮政编码：
                    </td>
                    <td>
                        <whir:textbox id="TakePostcode" width="280px" cssclass="text" runat="server" required="True"
                            regular="Zipcode" message="*" requirederrormessage="请填写邮政编码" maxlength="60"></whir:textbox>
                    </td>
                </tr>
                <tr class="pay">
                    <td class="name">
                        支付方式：
                    </td>
                    <td>
                        <asp:Literal ID="ltNoRecord" runat="server"></asp:Literal>
                        <asp:Repeater runat="server" ID="rpPayment">
                            <ItemTemplate>
                                <div>
                                    <input type="radio" name="PaymentIDs" id="radio_<%# Eval("id")%>" value="<%# Eval("id")%>" /><label
                                        for="radio_<%# Eval("id")%>"><%# Eval("name")%></label>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </td>
                </tr>
            </table>
        </div>
        <div class="CartProduct">
            <h4>
                <span><a href="<%=AppName+"Shop/ShopCart.aspx" %>">[返回修改商品信息]</a></span>商品信息</h4>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="mytable">
                <tr class="item">
                    <td width="120">
                    </td>
                    <td>
                        订单商品
                    </td>
                    <td width="150">
                        金额
                    </td>
                    <td width="100">
                        数量
                    </td>
                    <td width="100">
                        小计
                    </td>
                </tr>
                <asp:Repeater ID="rptShopCart" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <dt class="picture"><a href="<%=AppName %>Shop/productinfo.aspx?proid=<%# Eval("ProID") %>&aids=<%# Eval("AttrValueIDs") %>">
                                    <img onerror="this.src='<%=AppName %>res/images/nofile.jpg'" alt='<%#Eval("ProName") %>'
                                        src='<%=UploadFilePath %><%# Eval("ProImg") %>' />
                                </a></dt>
                            </td>
                            <td>
                                <dt class="name">
                                    <%# Eval("IsBuy").ToString()=="0"?"<span style='color:red' class='undercarriage'>[商品已下架]</span><br/>":"" %><a
                                        href="<%=AppName %>Shop/productinfo.aspx?proid=<%# Eval("ProID") %>&aids=<%# Eval("AttrValueIDs") %>"
                                        title='<%# Eval("ProName") %>'>
                                        <%# Eval("ProName") %>
                                        <%# Eval("AttrValueNames") %>
                                        <br />
                                        <%# Eval("ProNO") %>
                                    </a></dt>
                            </td>
                            <td>
                                价格：<font><%#string.Format("{0:C2}", Eval("CostAmount").ToDecimal(0))%></font>
                            </td>
                            <td>
                                <%# Convert.ToInt32(Eval("Qutity")) %>
                            </td>
                            <td>
                                <font>
                                    <%#string.Format("{0:C2}", Eval("CostAmount").ToDecimal(0) * Eval("Qutity").ToDecimal(0))%></font>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
        <!--购物车商品End-->
        <!--结算信息-->
        <div class="ShopCart_txt">
            <h4>
                结算信息</h4>
            <dl>
                <ul class="list_total">
                    <asp:Repeater ID="rptShopCartTotal" runat="server">
                        <ItemTemplate>
                            <li><span>＋<%#string.Format("{0:f2}", Eval("CostAmount").ToDecimal(0) * Eval("Qutity").ToDecimal(0))%></span><%# Eval("ProName") %>
                                <%# Eval("AttrValueNames") %>
                                ×<%# Eval("Qutity")%>：</li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
                <h6>
                    <span>
                        <%= string.Format("{0:C2}",Total) %></span>总金额：</h6>
            </dl>
        </div>
        <!--结算信息 End-->
        <div class="btnDiv">
            <input type="submit" name="btnSubmit" id="btnSubmit" value="提交订单" class="btn1" runat="server"
                onserverclick="btnSubmit_Click" onfocus="this.blur()" />
        </div>
        <!--Content End-->
    </div>
    <!--End-->
</div>
