<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Orders.aspx.cs" Inherits="Shop_member_Orders" EnableViewState="false" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Register Src="~/Shop/UserControl/Head.ascx" TagName="Head" TagPrefix="Shop" %>
<%@ Register Src="~/Shop/UserControl/Top.ascx" TagName="Top" TagPrefix="Shop" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader" TagPrefix="Shop" %>
<%@ Register src="~/Shop/UserControl/menu.ascx" tagname="MemberMenu" tagprefix="Shop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<Shop:Head runat="server" ID="shophead" />
<script type="text/javascript">
    jQuery(function () {

        jQuery(".noSel").click(function () {
            var oper = jQuery(this).attr("rel"); //订单状况
            window.location.href = 'orders.aspx?status=' + oper;
        })

        jQuery('.noSel[rel="<%=RequestUtil.Instance.GetQueryInt("status",0) %>"]').attr("class","show");
    })
</script>
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
      <h2 class="tab">
           <div style="float:right">商品名称/商品编号/订单编号：<asp:TextBox runat="server" ID="txtSearch"></asp:TextBox><asp:Button ID="btnSearch" Text="搜索" runat="server" OnClick="btnSearch_Click"  />
      </div>
          <span class="noSel" rel="0">全部订单</span>
          <span class="noSel" rel="1">进行中</span>
          <span class="noSel" rel="2">已完成</span>
          <span class="noSel" rel="3">已取消</span>

      </h2>
      <asp:PlaceHolder ID="NoDate" runat="server" Visible="false">
       <div class="OrderTable" style="text-align:center">
       没有符合条件的数据
       </div>
      </asp:PlaceHolder>

       <asp:Repeater runat="server" ID="rptOrder" 
         OnItemDataBound="rptOrder_ItemDataBound"> 
       <ItemTemplate>
      <div class="OrderTable">
      <h5 class="record"><span>总金额：<i><%# Eval("PayAmount", "{0:C2}")%></i>订单状态：<em><%# Eval("Status").ToInt(1)==1?"未完成":"已完成" %></em></span>订单号：<b><%# Eval("OrderNo") %></b>　　提交时间：<%# Eval("CreateDate","{0:yyyy-MM-dd HH:mm:ss}") %></h5>      
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
         <asp:Repeater runat="server" ID="rptOrderPro"> 
        <ItemTemplate>
          <tr>
            <td class="td_img"><a target="_blank" href="<%=AppName %>Shop/productinfo.aspx?proid=<%# Eval("ProID") %>&aids=<%# Eval("AttrValueIDs") %>">
            <img onerror="this.src='<%=AppName %>res/images/nofile.jpg'"
                    alt='<%#Eval("ProName") %>' src='<%=UploadFilePath %><%# Eval("ProImg") %>' /></a>
              </td>
            <td class="td_name">
            <a target="_blank" href="<%=AppName %>Shop/productinfo.aspx?proid=<%# Eval("ProID") %>&aids=<%# Eval("AttrValueIDs") %>"><%# Eval("ProName")%></a> <br />
            <span class="f_txt"><%# Eval("ProNO") %></span>
            </td>
            <td width="100"><b class="f_price"><%# Eval("SaleAmount", "{0:C2}")%></b></td>
            <td width="50"><%# Eval("Count") %></td>

             <%# GetOrderInofHtml(Container.ItemIndex, Eval("ProductAmount", "{0:C2}").ToStr(), Eval("PaymentID").ToStr(), Eval("IsPaid").ToStr(), Eval("TakeName").ToStr(), Eval("OrderID").ToStr(), Eval("IsCancel").ToStr(), Eval("ProLength").ToStr())%>
            
             </tr>
          </ItemTemplate>
          </asp:Repeater>
        </table>
      </div>
      </ItemTemplate> 
      </asp:Repeater>
      <!--End-->
      <!--Pages-->
        <div class="Pages">
        <wtl:Pager ID="pager1" runat="server" PageSize="3" Footer="5"></wtl:Pager>
        </div>
        <!--Pages-->
      
  </div>
</div>

    </form>
</body>
</html>
