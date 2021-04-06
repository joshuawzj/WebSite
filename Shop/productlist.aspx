<%@ Page Language="C#" AutoEventWireup="true" CodeFile="productlist.aspx.cs" Inherits="Shop_productlist" %>

<%@ Register Src="~/Shop/UserControl/ProCategoryMenu.ascx" TagName="ProCategoryMenu"
    TagPrefix="uc1" %>
<%@ Register Src="~/Shop/UserControl/CategoryHeader.ascx" TagName="CategoryHeader"
    TagPrefix="uc2" %>
<%@ Import Namespace="Whir.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="x-ua-compatible" content="ie=7" />
    <meta name="Author" content="万户网络设计制作" />
    <title>商品列表</title>
    <link href="css/shop_whir.css" rel="stylesheet" type="text/css" /> 
    <script type="text/javascript">
        //获取url参数的值：name是参数名
        function getQueryString(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) {
                return unescape(r[2]);
            }
            return null;
        }
        //分页：上一页，下一页
        function ChangePage(mark) {
            var page = getQueryString("page");
            if (page == null) {
                page = 1;
            }
            page = parseInt(page);
            var newPage = 0;
            //上一页
            if (mark == "pre") {
                newPage = (page > 1 ? (page - 1) : 1);
            }
            //下一页
            else {
                var PageCount = $(".SortDiv .fr").find("em").eq(0).html().split('/')[1];
                if (PageCount == null) {
                    PageCount = 1;
                }
                newPage = (page >= PageCount ? page : (page + 1));
            }
            window.location = changeURLPar(document.URL, "page", newPage);
        }
        //排序格式sale=asc&price=asc&sort=sale
        function ChangSort(name) {

            var url = document.URL;

            //将当前第一个排序改为选中的排序
            url = changeURLPar(url, "ob", name);

            //改变排序类型
            url = changeURLPar(url, name, (getQueryString(name) == null ? "asc" : (getQueryString(name) == "asc" ? "desc" : "asc")));

            window.location = url;
        }
        //设置url参数值，ref参数名,value新的参数值
        function changeURLPar(url, ref, value) 
        {
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
        //删除参数值
        function delQueStr(url, ref) {
            var str = "";
            if (url.indexOf('?') != -1) {
                str = url.substr(url.indexOf('?') + 1);
            }
            else {
                return url;
            }
            var arr = "";
            var returnurl = "";
            var setparam = "";
            if (str.indexOf('&') != -1) {
                arr = str.split('&');
                for (i in arr) {
                    if (arr[i].split('=')[0] != ref) {
                        returnurl = returnurl + arr[i].split('=')[0] + "=" + arr[i].split('=')[1] + "&";
                    }
                }
                return url.substr(0, url.indexOf('?')) + "?" + returnurl.substr(0, returnurl.length - 1);
            }
            else {
                arr = str.split('=');
                if (arr[0] == ref) {
                    return url.substr(0, url.indexOf('?'));
                }
                else {
                    return url;
                }
            }
        }
        $(document).ready(function () {
            //排序按钮样式
            $(".SortDiv .fl a").each(function () {
                var name = $(this).attr("mark");
                var c = ''; //样式名称
                var od = getQueryString(name);
                if (od == null) {
                    c = 'a_down_gray';
                }
                else {
                    if (od == "asc") {
                        c = 'a_up_red';
                    }
                    else {
                        c = 'a_down_red';
                    }
                    if (getQueryString("ob") == name) {
                        c = c + ' a_on';
                    }
                }
                $(this).removeClass().addClass(c);
            });
            //已筛选条件点击移除事件
            $(".FilterDiv .Filter a").click(function () {
                var svid = $(this).attr("svid");
                var url = document.URL;
                var svids = getQueryString("svids");
                if (svids == null) {
                    svids = '';
                }
                else {
                    svids = unescape(svids);
                }
                var list = svids.split(',');
                for (var i = 0; i < list.length; i++) {
                    if (list[i] == svid) {
                        list.splice(i, 1)
                    }
                }
                svids = list.toString();
                if (svids == '') {
                    url = delQueStr(url, "svids");
                }
                else {
                    url = changeURLPar(url, "svids", escape(svids));
                }
                window.location = url;
            });
            //筛选条件点击事件
            $(".FilterDiv dl a").click(function () {
                var svid = $(this).attr("svid");
                var url = document.URL;
                var svids = getQueryString("svids");
                if (svids == null) {
                    svids = '';
                }
                svid = svid.replace(/\'/g, "");
                if (("," + svids + ",").indexOf("," + svid + ",") == -1) {
                    if (svids != '') {
                        svids = svids + ",";
                    }
                    svids = svids + svid;
                }
                url = changeURLPar(url, "svids", escape(svids));
                window.location = url;
            });
            //筛选条件样式
            $(".FilterDiv dl a").each(function () {
                var svids = "," + (getQueryString("svids") == null ? "" : getQueryString("svids")) + ",";
                var svid = "," + $(this).attr("svid") + ",";
                svids = svids.replace(/\'/g, "");
                if (svids.indexOf(svid) > -1) {
                    $(this).removeClass().addClass("a_on");
                }
                else {
                    $(this).removeClass();
                }
            });

            //收缩筛选条件
            var sxHeigth = $(".Filter table tr td:last-child").find("div").css({ "overflow": "auto", "height": "auto" }).height();
         
            if (sxHeigth > 35) {
                $(".Filter table tr td:first-child").find("img").show();
            }
            else {
                $(".Filter table tr td:first-child").find("img").hide();
            }
            $(".Filter table tr td:first-child").find("img").click(function () {
                if ($(".Filter table tr td:first-child").find("img").attr("state") == "jian") {
                    $(".Filter table tr td:first-child").find("img").attr("state", "jia");
                    $(".Filter table tr td:first-child").find("img").attr("src", "images/2013_icon_jia.gif");
                    $(".Filter table tr td:last-child").find("div").css({ "overflow": "hidden", "height": "25px" });
                }
                else {
                    $(".Filter table tr td:first-child").find("img").attr("state", "jian");
                    $(".Filter table tr td:first-child").find("img").attr("src", "images/2013_icon_jian.gif");
                    $(".Filter table tr td:last-child").find("div").css({ "overflow": "auto", "height": "auto" });
                }
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <img src="images/header.jpg" /></center>
    <uc2:CategoryHeader ID="CategoryHeader1" runat="server" />
    <div class="ShopContain">
        <div class="Sidebar">
            <uc1:ProCategoryMenu ID="ProCategoryMenu1" runat="server" />
        </div>
        <div class="MainPro">
            <div class="CurrentPro">
                <span>当前位置：<asp:Literal ID="ltCategory" runat="server"></asp:Literal></span></div>
            <!--筛选Start-->
            <div class="FilterDiv">
                <div class="Filter">
                    <table border="0" cellspacing="0" cellpadding="0">
                        <tr>
                        <td style="width:14px; vertical-align:top;">
                        <img state="jian" src="images/2013_icon_jian.gif" />
                        </td>
                            <td class="name" valign="top">
                                筛选条件：
                            </td>
                            <td valign="top" >
                            <div>
                                <asp:Literal ID="ltChooseSearchValue" runat="server"></asp:Literal>
                             </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Literal ID="ltSearchValue" runat="server"></asp:Literal>
            </div>            
            <!--筛选End-->
            <!--筛选Start-->
            <div class="SortDiv">
                <span class="fl">排序 <a mark="oc" class="a_up_red a_on" onclick="ChangSort('oc');"><i>
                    销量</i></a> <a mark="price" class="a_up_red" onclick="ChangSort('price');"><i>价格</i></a>
                </span><span class="fr"><i>共
                    <asp:Literal ID="ltAllProCount" runat="server"></asp:Literal>个商品</i> <em>
                        <asp:Literal ID="ltPageTip" runat="server"></asp:Literal></em> <a style="cursor: pointer;"
                            onclick="ChangePage('pre');" class="a_prev">上一页</a> <a style="cursor: pointer;" onclick="ChangePage('next');"
                                class="a_next">下一页</a> </span>
            </div>
            <!--筛选End-->
            <!--商品Start-->
            <div class="ListProduct">
                <ul>
                    <asp:Repeater ID="rptProList" runat="server">
                        <ItemTemplate>
                            <li><span class="pic"><a href='productinfo.aspx?proid=<%#Eval("ProID") %>&categoryid=<%#Eval("CategoryID") %>' >
                                <img onerror="this.src='<%#Whir.Framework.WebUtil.Instance.AppPath()+Whir.Framework.AppSettingUtil.AppSettings["SystemPath"] %>res/images/nopicture.jpg'"
                                    alt='<%#Eval("ProName") %>' src='<%#Whir.Framework.WebUtil.Instance.AppPath()+Whir.Framework.AppSettingUtil.AppSettings["UploadFilePath"]+Eval("ProImg") %>' /></a></span>
                                <h5 class="name">
                                    <a href='productinfo.aspx?proid=<%#Eval("ProID") %>&categoryid=<%#Eval("CategoryID") %>' title='<%#Eval("ProName") %>'>
                                        <%#Eval("ProName").ToStr().Cut(45,"...") %></a></h5>
                                <i class="price">￥<%#Math.Round(Convert.ToInt32(Eval("AttrCount")) > 0 ? Convert.ToDecimal(Eval("AttrMinPrice")) : Convert.ToDecimal(Eval("CostAmount")), 2).ToString()%></i>
                            </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
            <!--商品End-->
            <!--Pages-->
            <div class="Pages">
                <wtl:Pager ID="pager1" runat="server" PageSize="12" Footer="5"></wtl:Pager>
            </div>
            <!--Pages-->
        </div>
    </div>
    </form>
</body>
</html>
