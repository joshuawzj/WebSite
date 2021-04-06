<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="whir_system_Error" %>
<%@ Import Namespace="Whir.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=AppSettingUtil.GetString("ProductName").ToStr()%></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h3 style="color:Red;"><%=Msg %></h3>
    </div>
    </form>
</body>
</html>
