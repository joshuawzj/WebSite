<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Error.aspx.cs" Inherits="whir_Error" %>

<%@ Import Namespace="Whir.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=AppSettingUtil.GetString("ProductName").ToStr()%></title>
    <style type='text/css'>
        body {
            font-size: 14px;
            font-family: 微软雅黑;
        }

        .title {
            width: 80%;
            margin: 20px auto;
            border: 1px solid gainsboro;
            padding: 20px;
            background-color: #F7F7F7;
            font-size: 16px;
            text-align: center;
            font-weight: bold;
            color: brown;
        }

        .content {
            width: 80%;
            margin: 20px auto;
            border: 1px solid gainsboro;
            padding: 20px;
            background-color: #F7F7F7;
        }

        .footer {
            width: 80%;
            margin: 20px auto;
            border: 1px solid gainsboro;
            padding: 20px;
            background-color: #F7F7F7;
            text-align: center;
            color: gray;
        }

        .err {
            color: red;
            padding: 10px;
        }
    </style>
</head>
<body>
    <div class='title'>
        系统请求错误
    </div>
    <div class='content'>
        <div class='err'>
            <div>错误代码：<%=Code %>
                <p />错误信息：<%=Msg %>  
                <p /><a href="/">点击跳转到网站首页！ </a></div>
        </div>
    </div>
    <div class='footer'>
        © 2018 万户网络. All Rights Reserved
    </div>


</body>
</html>

