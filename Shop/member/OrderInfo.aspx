<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderInfo.aspx.cs" Inherits="Shop_member_OrderInfo" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Register Src="~/Shop/UserControl/Head.ascx" TagName="Head" TagPrefix="Shop" %>
<%@ Register Src="~/Shop/UserControl/Top.ascx" TagName="Top" TagPrefix="Shop" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader" TagPrefix="Shop" %>
<%@ Register src="~/Shop/UserControl/menu.ascx" tagname="MemberMenu" tagprefix="Shop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<Shop:Head runat="server" ID="shophead" />
</head>
<body>
    <form id="form1" runat="server">
<Shop:Top ID="Top" Runat="Server"></Shop:Top>

<!--类别 Start-->
<Shop:CategoryHeader ID="CategoryHeader" Runat="Server"></Shop:CategoryHeader>
<!--类别 End-->

<div class="ShopContain">
    <div class="Sidebar">
 <Shop:MemberMenu ID="MemberMenu" Runat="Server"></Shop:MemberMenu>
  </div>
  <div class="Main">
      
    <div class="Current"><span>当前位置：<a href="personal.aspx">会员中心</a> > <i>我的订单</i></span><b>我的订单</b></div>
      
      <!--Start-->
      <!--详细-->
      <div class="Order_txt">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="detail">
          <tr>
            <td>订单号：<b class="num"><asp:Literal runat="server" ID="OrderNo"></asp:Literal></b></td>
            <td>总金额：<b class="price"><asp:Literal runat="server" ID="PayAmount"></asp:Literal></b></td>
            <td>提交时间：<asp:Literal runat="server" ID="CreateDate"></asp:Literal></td>
          </tr>
          <tr>
            <td>订单状态：<asp:Literal runat="server" ID="OrderStatus"></asp:Literal></td>
            <td>支付状态：<asp:Literal runat="server" ID="PayStatus"></asp:Literal></td>
            <td>&nbsp;</td>
          </tr>
        </table>
      </div>
      <!--详细 End-->
      
      <!--物流跟踪 付款信息-->
      <div id="AutoTable1">
          <h2 class="tab">
              <span>物流跟踪</span>
              <span>付款信息</span>          
          </h2>
          <div class="Order_txt" name="AutoContent">
              <!--物流跟踪 end-->
              <div>
              <asp:Literal runat="server" ID="litCourier"></asp:Literal>
<%--
                <table style="display:none"  width="100%" border="0" cellspacing="0" cellpadding="0" class="logistic_pay">
                  <tr>
                    <th width="150">处理时间</th>
                    <th>处理信息</th>
                  </tr>
                  <asp:Repeater runat="server" ID="rptCourier"><ItemTemplate>
                  <tr>
                    <td>2013-01-25 15:30:50</td>
                    <td>创建订单</td>
                  </tr>
                  </ItemTemplate>
                  </asp:Repeater>
                </table>--%>
              </div>
              <!--物流跟踪 end-->
              <!--付款信息-->
              <div style="display:none;">
                <dl>
                支付方式：<asp:Literal runat="server" ID="PayTypeName"></asp:Literal><br />
                商品总额：<b class="f_red"><asp:Literal runat="server" ID="InfoProductAmount"></asp:Literal></b><br />
                <%--运　　费：<b class="f_red"><asp:Literal runat="server" ID="Carriage"></asp:Literal></b><br />--%>
                应付金额：<b class="f_red"><asp:Literal runat="server" ID="InfoPayAmount"></asp:Literal></b>
                </dl>
              </div>
              <!--付款信息 end-->
          </div>
      </div>
	<script type="text/javascript" src="../Scripts/tab.js"></script>
    <script type="text/javascript">
        AutoTables("AutoTable1");
    </script>
      <!--物流跟踪 付款信息 End-->


    <!--订单信息-->
    <div class="Order_txt">   
        <h4>订单信息</h4>
        <h5>收货信息</h5>
        <dl>
        收 货 人：<asp:Literal runat="server" ID="TakeName"></asp:Literal><br />
        地    址：<asp:Literal runat="server" ID="TakeRegion"></asp:Literal> <asp:Literal runat="server" ID="TakeAddress"></asp:Literal><br />
        手机号码：<asp:Literal runat="server" ID="TakeMobile"></asp:Literal>　　固定电话：<asp:Literal runat="server" ID="TakeTel"></asp:Literal><br />
        邮箱地址：<asp:Literal runat="server" ID="TakeEmail"></asp:Literal><br />
        邮政编码：<asp:Literal runat="server" ID="TakePostcode"></asp:Literal>
        </dl>
        <h5>支付及配送方式</h5>
        <dl>
        支付方式：<asp:Literal runat="server" ID="PayName"></asp:Literal><br />
        配送方式：<asp:Literal runat="server" ID="Courier"></asp:Literal>
        </dl>
    </div>
    <!--订单信息 End--> 
    
      <!--商品信息-->
    <div class="OrderTable">      
      <h4>商品信息</h4>      
      <table width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <th></th>
          <th>订单商品</th>
          <th>金额</th>
          <th>数量</th>
          <th>小计</th>
        </tr>
        <asp:Repeater runat="server" ID="rptOrderPro"> 
        <ItemTemplate>
        <tr>
          <td class="td_img"><a target="_blank" href="<%=AppName %>Shop/productinfo.aspx?proid=<%# Eval("ProID") %>&aids=<%# Eval("AttrValueIDs") %>">
          <img onerror="this.src='<%=AppName %>res/images/nofile.jpg'"
                    alt='<%#Eval("ProName") %>' src='<%=UploadFilePath %><%# Eval("ProImg") %>' />
                   </a>
          </td>
          <td class="td_name">
          <a target="_blank" href="<%=AppName %>Shop/productinfo.aspx?proid=<%# Eval("ProID") %>&aids=<%# Eval("AttrValueIDs") %>">
            <%# Eval("ProName")%><br />
            <span class="f_txt"><%# Eval("ProNO") %></span>
            </a>
          </td>
          <td width="120"><b class="f_price"><%# Eval("SaleAmount", "{0:C2}")%></b></td>
          <td width="70"><%# Eval("Count") %></td>
          <td width="120"><b class="f_price"><%# (Eval("SaleAmount").ToDecimal(0)*Eval("Count").ToDecimal(0)).ToString("C2")%></b></td>
        </tr>
        </ItemTemplate>
        </asp:Repeater>
      </table>
      </div>
      <!--商品信息 End--> 
    
    <!--结算信息-->
    <div class="Order_txt">   
        <h4>结算信息</h4>
        <dl>
            <ul class="list_total">
            <asp:Repeater runat="server" ID="rptSettleAccounts"> 
        <ItemTemplate>        
                <li><span>＋<%# (Eval("SaleAmount").ToDecimal(0)*Eval("Count").ToDecimal(0)).ToString("f2")%></span><%#Eval("ProName") %>×<%# Eval("Count") %>：</li>
                </ItemTemplate>        
                </asp:Repeater>
                <li><span><b><asp:Literal runat="server" ID="DiscountAmount"></asp:Literal></b></span>调整金额：</li>
                
            </ul>
            <h6><span><asp:Literal runat="server" ID="ProductAmount"></asp:Literal></span>总金额：</h6>
        </dl>
    </div>
    <!--结算信息 End-->  
      <!--End-->
      
      
  </div>
</div>
    </form>
</body>
</html>
