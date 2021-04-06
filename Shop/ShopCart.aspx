<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopCart.aspx.cs" Inherits="Shop_ShopCart" %>
<%@ Register Src="~/Shop/UserControl/Head.ascx" TagName="Head" TagPrefix="Shop" %>
<%@ Register Src="~/Shop/UserControl/Top.ascx" TagName="Top" TagPrefix="Shop" %>
<%@ Register Src="~/Shop/UserControl/ShopCart.ascx" TagName="ShopCart" TagPrefix="Shop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<Shop:Head runat="server" ID="shophead" />
</head>
<body>
    <form id="form1" runat="server">
     <Shop:Top ID="Top" Runat="Server"></Shop:Top>
    <Shop:ShopCart ID="Cart" Runat="Server"></Shop:ShopCart>
    </form>
</body>
</html>
