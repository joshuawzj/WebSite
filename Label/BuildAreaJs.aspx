<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BuildAreaJs.aspx.cs" Inherits="label_BuildAreaJs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h1>地区js导出</h1>
        <asp:DropDownList ID="DropDownList1" runat="server">
            <asp:ListItem>中文JS</asp:ListItem>
            <asp:ListItem>英文JS</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="Button1" runat="server" Text="从数据库导出地区JS" OnClick="Button1_Click" />
    </div>
    </form>
</body>
</html>
