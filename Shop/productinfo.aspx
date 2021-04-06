<%@ Page Language="C#" AutoEventWireup="true" CodeFile="productinfo.aspx.cs" Inherits="Shop_productinfo" %>

<%@ Register Src="~/Shop/UserControl/ProCategoryMenu.ascx" TagName="ProCategoryMenu"
    TagPrefix="uc1" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <meta name="Author" content="万户网络设计制作" />
    <title>商品详细页面</title>
    <link href="css/shop_whir.css" rel="stylesheet" type="text/css" /> 
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <img src="images/header.jpg" /></center>
    <uc2:categoryheader id="CategoryHeader1" runat="server" />
    <div class="ShopContain">
        <div class="Sidebar">
            <uc1:procategorymenu id="ProCategoryMenu1" runat="server" />
        </div>
        <div class="MainPro">
            <div class="CurrentPro">
                <span>当前位置：<asp:Literal ID="ltCategory" runat="server"></asp:Literal></span></div>
            <!--筛选Start-->
            <div class="Pro_Info">
                <div id="preview">
                    <div class="jqzoom" id="spec-n1">
                        <asp:Literal ID="ltProImg" runat="server"></asp:Literal>
                    </div>
                    <div id="spec-n5">
                        <div id="spec-left">
                        </div>
                        <div id="spec-list">
                            <ul class="list-h">
                                <asp:Literal ID="ltImgs" runat="server"></asp:Literal>
                            </ul>
                        </div>
                        <div id="spec-right">
                        </div>
                    </div>
                </div>
                <div class="ProTxt">
                    <h5 class="Name">
                        <asp:Literal ID="ltProName" runat="server"></asp:Literal></h5>
                    <!--价格-->
                    <div class="Price">
                        <ul>
                            <li>
                                <dd>
                                    商品编号：</dd><dt><asp:Literal ID="ltProNO" runat="server"></asp:Literal></dt></li>
                            <li>
                                <dd>
                                    价格：</dd><dt><b><asp:Literal ID="ltCostAmount" runat="server"></asp:Literal></b></dt></li>
                        </ul>
                    </div>
                    <!--价格End-->
                    <!--参数-->
                    <div runat="server" id="divProAttr" class="Parameter">
                        <ul>
                            <asp:Literal ID="ltProAttr" runat="server"></asp:Literal>
                        </ul>
                        <script type="text/javascript">
                            $(document).ready(function () {

                                //绑定原有选择的规格
                                var AttrValueIDs = '<%=AttrValueIDs %>';
                                if (AttrValueIDs != '') {
                                    $(".Parameter a").each(function () {
                                        var avid = "," + $(this).attr("avid") + ",";
                                        if (("," + AttrValueIDs + ",").indexOf(avid) > -1) {
                                            $(this).removeClass().addClass("select").siblings().removeClass();
                                        }

                                    });
                                }
                                $(".Parameter a").click(function () {
                                    $(this).removeClass().addClass("select").siblings().removeClass();

                                    var avids = "";
                                    var rslt = true;
	                                $(".Parameter .select").each(function() {
		                                avids = avids + $(this).attr("avid") + ","
	                                });
                                    if (avids.length > 0) {
                                        avids = avids.substring(0, avids.length - 1);
                                    }
                                    $(".Parameter li").each(function () {
                                        rslt = rslt && $(this).find(".select").length > 0;
                                    });
                                    if (rslt) {
                                        location = changeURLPar(document.URL, 'aids',escape(avids));
                                    }
                                });
                            });
                            //设置url参数值，ref参数名,value新的参数值
                            function changeURLPar(url, ref, value) {
                                var str = "";
                                if (url.indexOf('?') != -1)
                                    str = url.substr(url.indexOf('?') + 1);
                                else
                                    return url + "?" + ref + "=" + value;
                                var returnurl = "";
                                var setparam = "";
                                var arr;
                                var modify = "0";

                                if (str.indexOf('&') != -1) {
                                    arr = str.split('&');

                                    for (i in arr) {
                                        if (arr[i].split('=')[0] == ref) {
                                            setparam = value;
                                            modify = "1";
                                        }
                                        else {
                                            setparam = arr[i].split('=')[1];
                                        }
                                        returnurl = returnurl + arr[i].split('=')[0] + "=" + setparam + "&";
                                    }

                                    returnurl = returnurl.substr(0, returnurl.length - 1);

                                    if (modify == "0")
                                        if (returnurl == str)
                                            returnurl = returnurl + "&" + ref + "=" + value;
                                }
                                else {
                                    if (str.indexOf('=') != -1) {
                                        arr = str.split('=');

                                        if (arr[0] == ref) {
                                            setparam = value;
                                            modify = "1";
                                        }
                                        else {
                                            setparam = arr[1];
                                        }
                                        returnurl = arr[0] + "=" + setparam;
                                        if (modify == "0")
                                            if (returnurl == str)
                                                returnurl = returnurl + "&" + ref + "=" + value;
                                    }
                                    else
                                        returnurl = ref + "=" + value;
                                }
                                return url.substr(0, url.indexOf('?')) + "?" + returnurl;
                            }
                        </script>
                        <div class="clear">
                        </div>
                    </div>
                    <!--参数End-->
                    <div runat="server" id="IsAllowBuy" class="Amount">
                        购买数量：<a style="cursor: pointer;" class="jia">-</a>
                        <input type="text" name="qutity" id="qutity" class="text" value="1" maxlength="6" />
                        <a style="cursor: pointer;" class="jian">+</a> 件<br />
                        <asp:Button ID="btnAddCart" runat="server" Text="" OnClick="btnAddCart_Click" CssClass="buy_btn" />
                        <!--<input type="submit" name="button2" id="button2" value="" class="finish_btn" />-->
                        <script type="text/javascript">
                            $(document).ready(function () {

                                $("#<%=btnAddCart.ClientID %>").click(function () {

                                    if ($(".Parameter li").length > 0 && $(".Parameter li").length != $(".Parameter .select").length) {
                                        alert("请选择完整商品规格");
                                        return false;
                                    } else {
                                        return true;
                                    }
                                });



                                $(".Amount .text").keypress(function (evt) {
                                    evt = (evt) ? evt : ((window.event) ? window.event : "") //兼容IE和Firefox获得keyBoardEvent对象
                                    var key = evt.keyCode ? evt.keyCode : evt.which; //兼容IE和Firefox获得keyBoardEvent对象的键值

                                    if (key < 48 || key > 59 || key == 48 && jQuery(this).val() == "0" && jQuery(this).val() == "")
                                        return false;
                                });

                                $(".Amount .text").blur(function () {
                                    var value = $(this).val();

                                    if (value == "" || value == "0") {
                                        value = 1;
                                        $(this).val("1");
                                    }

                                    if (parseInt(value, 10).toString() == "NaN") {
                                        jQuery(this).val("1");
                                    } else {
                                        jQuery(this).val(parseInt(value, 10));
                                    }

                                });
                                $(".Amount .jian").click(function () {

                                    $(".Amount .text").val((parseInt($(".Amount .text").val(), 10) + 1).toString());
                                });
                                $(".Amount .jia").click(function () {

                                    $(".Amount .text").val((parseInt($(".Amount .text").val(), 10) - 1) > 0 ? (parseInt($(".Amount .text").val(), 10) - 1).toString() : "1");
                                });
                            });
                        </script>
                    </div>
                </div>
            </div>
            <!--商品详情-->
            <h5 class="Pro_title">
                <b>商品详情</b></h5>
            <div class="Pro_detail">
                <asp:Literal ID="ltProDesc" runat="server"></asp:Literal>
            </div>
            <!--商品详情End-->
            <div class="Pro_advisory">
                <h5 class="Pro_title">
                    <b>商品咨询</b></h5>
                <div class="txt_advisory">
                    <asp:Repeater ID="rptConsultList" runat="server">
                        <ItemTemplate>
                            <dl class="Q_txt">
                                <span class="date">
                                    <%#Eval("CreateDate") %></span> <span class="name">
                                        <%#Eval("LoginName") %>：</span> <span class="txt">
                                            <%# Server.HtmlEncode(Eval("Consult").ToStr()) %></span>
                            </dl>
                            <dl class="A_txt" style='<%#Eval("Reply")==null?"display:none;": ""%>'>
                                <span class="date">
                                    <%#Eval("ReplyDate") %></span> <span class="name">回复：</span> <span class="txt">
                                        <%#Eval("Reply") %></span>
                            </dl>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
                <!--Pages-->
                <div class="Pages">
                    <wtl:pager id="pager1" runat="server" pagesize="2" footer="5"></wtl:pager>
                </div>
                <!--Pages-->
                <h5 class="Pro_title">
                    <b>发表咨询</b></h5>
                <div class="txt_form">
                <whir:textbox id="txtConsult" runat="server"  width="750" textmode="MultiLine" validationgroup="save1"
                                            height="85" maxlength="2000" tipctl="ConsultTip"></whir:textbox>
                    <br />
                    <span style="color: red; display: none;" id="ConsultTip">
                                        最多只能输入2000个字符</span>
                    <br />
                    <asp:Button ID="Button1" runat="server" Text="提 交" CssClass="btn" OnClick="Button1_Click" validationgroup="save1" />
                </div>
            </div>
            <!--筛选Start-->
            <!--筛选End-->
            <!--商品Start-->
            <!--商品End-->
        </div>
    </div>
    <script src="Scripts/p.pshow.js" type="text/javascript"></script>
    </form>
</body>
</html>
