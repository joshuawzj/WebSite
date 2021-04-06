<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ShopCart2.aspx.cs" Inherits="Shop_ShopCart2" %>
<%@ Register Src="~/Shop/UserControl/Head.ascx" TagName="Head" TagPrefix="Shop" %>
<%@ Register Src="~/Shop/UserControl/Top.ascx" TagName="Top" TagPrefix="Shop" %>
<%@ Register Src="~/Shop/UserControl/ShopCart2.ascx" TagName="SettleAccounts" TagPrefix="Shop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<Shop:Head runat="server" ID="shophead" />
</head>
<body>
    <form id="form1" runat="server">
 <Shop:Top ID="Top" Runat="Server"></Shop:Top>
    <Shop:SettleAccounts ID="SettleAccounts" Runat="Server"></Shop:SettleAccounts>
    </form>
</body>
</html>
