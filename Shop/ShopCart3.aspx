<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopCart3.aspx.cs" Inherits="Shop_ShopCart3" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Register Src="~/Shop/UserControl/Head.ascx" TagName="Head" TagPrefix="Shop" %>
<%@ Register Src="~/Shop/UserControl/Top.ascx" TagName="Top" TagPrefix="Shop" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<Shop:Head runat="server" ID="shophead" />
</head>
<body>
    <form id="form1" runat="server">
 <Shop:Top ID="Top" Runat="Server"></Shop:Top>
 <div class="ShopContain">
   
  <div class="MainBox">
  
      
      <!--Start-->
      <div class="myorder"><b></b></div>
    
    
    <!--Content Start-->    
    
    <div class="ShopCart_success">
    <h5><asp:Literal runat="server" ID="litOrderNo"></asp:Literal></h5>
    <asp:PlaceHolder runat="server" ID="phTip">
    <p>您的订单已经完成，您可以在会员中心里查看到该订单的处理状态！<br />
    谢谢！<br />
     </asp:PlaceHolder>
     <input id="Button1" type="button" value=""  class="btn_Shopping" onClick="location.href='productlist.aspx'" onfocus="this.blur()"/>
     <input id="Button2" type="button" value="" class="btn_Member"  onClick="location.href='member/orderInfo.aspx?id=<%=RequestUtil.Instance.GetQueryInt("orderId",0) %>'"/>
    </div>
    
    <!--Content End-->     
    
    </div>
      <!--End-->
      
  </div>
    </form>
</body>
</html>
