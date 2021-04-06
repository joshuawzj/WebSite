<%@ Page Language="C#" AutoEventWireup="true" CodeFile="advisory.aspx.cs" Inherits="Shop_member_advisory" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Register src="~/Shop/UserControl/menu.ascx" tagname="menu" tagprefix="uc1" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <meta name="Author" content="万户网络设计制作" />
    <title>会员中心-我的咨询</title>
    <link href="../css/shop_whir.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <img src="../images/header.jpg" /></center>
    <uc2:categoryheader id="CategoryHeader1" runat="server" />
    <div class="ShopContain">
       <div class="Sidebar">
            <uc1:menu ID="menu1" runat="server" />
        </div>
        <div class="Main">
            <div class="Current">
                <span>当前位置：<a href="personal.aspx">会员中心</a> > <i>我的咨询</i></span><b>我的咨询</b></div>
            <!--Start-->
            <div class="Advisory_txt">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <asp:Repeater ID="rptProList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="img" valign="top" style="cursor:pointer;" onclick="location='../productinfo.aspx?proid=<%#Eval("ProID") %>';">
                                    <img src='<%#UploadFilePath+ Eval("ProImg") %>' onerror="this.src='<%=SysPath%>res/images/nopicture.jpg'" /><br />
                                    <i><%#Eval("ProName") %></i><br />
                                    <em><%#Eval("ProNO") %></em>
                                </td>
                                <td valign="top">
                                    <dl class="Q_txt">
                                        <span class="date"><%#Eval("CreateDate") %></span> <span class="name"><%#Eval("LoginName") %>：</span> <span
                                            class="txt"><%# Server.HtmlEncode(Eval("Consult").ToStr()) %> </span>
                                    </dl>
                                    <dl class="A_txt" style='<%#Eval("Reply")==null?"display:none;": ""%>'>
                                        <span class="date"><%#Eval("ReplyDate") %></span> <span class="name">回复：</span> <span
                                            class="txt"><%#Eval("Reply") %></span>
                                    </dl>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <!--End-->
            <!--Pages-->
            <div class="Pages">
                <wtl:pager id="pager1" runat="server" pagesize="3" footer="5"></wtl:pager>
            </div>
            <!--Pages-->
        </div>
    </div>
    </form>
</body>
</html>
