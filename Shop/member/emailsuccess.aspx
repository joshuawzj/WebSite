<%@ Page Language="C#" AutoEventWireup="true" CodeFile="emailsuccess.aspx.cs" Inherits="Shop_member_emailsuccess" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader"
    TagPrefix="uc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../css/shop_whir.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <!--共用头部-->
    <center>
        <img src="../images/header.jpg" /></center>
    <!--共用头部-->
        <uc2:CategoryHeader ID="CategoryHeader1" runat="server" />
    <div>
     <asp:Literal ID="ltUserName" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
