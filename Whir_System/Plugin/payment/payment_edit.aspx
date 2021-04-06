<%@ Page Title="" Language="C#" MasterPageFile="~/whir_system/master/SystemMasterPage.master"
    AutoEventWireup="true" CodeFile="payment_edit.aspx.cs" Inherits="whir_system_Plugin_payment_payment_edit" %>
    <%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="mainbox">
        <dl class="title_column">
            <a href="paymentlist.aspx"><b><%="支付方式管理".ToLang() %></b></a> <em class="line"></em><a href="javascript:void(0)" class="aSelect">
                <b><%="编辑".ToLang() %></b></a>
        </dl>
         <div class="line_border">
        </div>
        <div class="All_table">
            <table width="100%" cellspacing="0" cellpadding="0" class="table2">
                <tr>
                    <td width="100" class="itemname" align="right">
                        <%="支付方式名称：".ToLang() %>
                    </td>
                    <td>
                        <whir:textbox id="Name" runat="server" width="300" cssclass="text_common" maxlength="50"
                            regular="Custom" validationexpression="[a-zA-Z0-9\u4e00-\u9fbb]{1,50}" ErrorCss="form_error" TipCss="form_tip" required="True"
                            errormessage="支付方式名称长度限制在50个字符以内，可输入中文、英文及数字。"></whir:textbox>
                        <span class="note_gray"><%="前台用户支付时显示的名称，例如支付宝，线下支付。".ToLang() %></span>
                    </td>
                </tr>
                <tr>
                    <td width="100" class="itemname" align="right">
                        <%="是否启用：".ToLang() %>
                    </td>
                    <td>
                        <asp:CheckBox ID="CheckBox1" runat="server" Text="开启" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td width="100" class="itemname" align="right">
                        <%="合作伙伴号：".ToLang() %>
                    </td>
                    <td>
                        <whir:textbox id="AccountNumber" runat="server" cssclass="text_common"  width="300" MaxLength="128"></whir:textbox>
                        <span class="note_gray"><%="接口公司给您的商户号，有些叫客户号。".ToLang() %></span>
                    </td>
                </tr>
                <tr runat="server" id="tr1">
                    <td width="100" class="itemname" align="right" runat="server" id="trAccount">
                        
                    </td>
                    <td>
                        <whir:textbox id="Account" runat="server" ErrorCss="form_error" TipCss="form_tip" required="True" cssclass="text_common"
                             width="300" MaxLength="64"></whir:textbox>
                        <span class="note_gray"><%="有些支付方式需要商户填写卖家账户才能支付,例如支付宝。".ToLang() %></span>
                    </td>
                </tr>
                <tr runat="server" id="tr2">
                    <td width="100" class="itemname" align="right" runat="server" id="trMd5Key">
                       
                    </td>
                    <td>
                        <whir:textbox id="Md5Key" runat="server" ErrorCss="form_error" TipCss="form_tip" required="True" cssclass="text_common"  width="300" MaxLength="64"></whir:textbox>
                        <span class="note_gray"><%="接口公司给您的私钥，有些叫MD5私钥。".ToLang() %></span>
                    </td>
                </tr>
                <tr runat="server" id="trAlipay" visible="false">
                    <td width="100" class="itemname" align="right">
                        <%="支付宝账户：".ToLang() %>
                    </td>
                    <td>
                        <whir:textbox id="txtAlipayAccount" ErrorCss="form_error" TipCss="form_tip" required="True" cssclass="text_common" runat="server"  width="300" MaxLength="64"></whir:textbox>
                        <span class="note_gray"><%="支付宝账号或卖家支付宝账户".ToLang() %></span>
                    </td>
                </tr>
                <tr runat="server" id="trurl1" visible="false">
                    <td width="100" class="itemname" align="right">
                        <%="返回地址：".ToLang() %>
                    </td>
                    <td>
                        <whir:textbox id="txtReturnUrl" runat="server" ErrorCss="form_error" TipCss="form_tip" required="True" cssclass="text_common"
                             width="300" ></whir:textbox>
                        <span class="note_gray"><%="支付完成后的返回地址，如http://www.***.com".ToLang() %></span>
                    </td>
                </tr>
                <tr runat="server" id="trurl2" visible="false">
                    <td width="100" class="itemname" align="right">
                        <%="异步通知地址：".ToLang() %>
                    </td>
                    <td>
                        <whir:textbox id="txtNotifyUrl" runat="server" ErrorCss="form_error" TipCss="form_tip" required="True" cssclass="text_common"
                             width="300"></whir:textbox>
                        <span class="note_gray"><%="支付完成的异步通知，如http://www.***.com".ToLang() %></span>
                    </td>
                </tr>
                <tr>
                    <td class="itemname" align="right">
                        <%="接口类型：".ToLang() %>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPaymentMode" runat="server" CssClass="select" Enabled="false">
                            <asp:ListItem Value="99bill" Text="快钱支付"></asp:ListItem>
                            <asp:ListItem Value="alipay" Text="支付宝"></asp:ListItem>
                            <asp:ListItem Value="alipayinstant" Text="支付宝实时到账"></asp:ListItem>
                            <asp:ListItem Value="chinabank" Text="网银在线"></asp:ListItem>
                            <asp:ListItem Value="chinapay" Text="银联在线"></asp:ListItem>
                            <asp:ListItem Value="cncard" Text="云网支付"></asp:ListItem>
                            <asp:ListItem Value="ipay" Text="中国在线支付网"></asp:ListItem>
                            <asp:ListItem Value="ips" Text="上海环迅"></asp:ListItem>
                            <asp:ListItem Value="tenpay" Text="财付通"></asp:ListItem>
                            <asp:ListItem Value="xpay" Text="易付通"></asp:ListItem>
                            <asp:ListItem Value="cod" Text="货到付款"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="itemname" align="right">
                        <%="描述：".ToLang() %>
                    </td>
                    <td>
                        <whir:textbox id="txtDescn" runat="server" width="300px" rows="4" textmode="MultiLine"
                            regular="Custom" validationexpression="[^<|>]*" ErrorCss="form_error" TipCss="form_tip" required="True" maxlength="255"
                            cssclass="input2" errormessage="长度限制在255个字符以内，不能出现“&lt;”、“&gt;”符号" height="101px"></whir:textbox>
                        <span class="note_gray"><%="长度限制在255个字符以内".ToLang() %> </span>
                    </td>
                </tr>
            </table>
            <div class="button_submit_div">
                <asp:LinkButton ID="btnSave" runat="server" CssClass="aLink" OnCommand="Save_Command"
                    CommandArgument="Save"> <em><img src="<%=SysPath%>res/images/button_submit_icon_1.gif" /></em><b><%="提交并返回".ToLang() %></b></asp:LinkButton>
                      <a class="aBack" href="paymentlist.aspx"><b><%="返回".ToLang() %></b></a>
            </div>
        </div>
    </div>
</asp:Content>
