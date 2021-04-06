<%@ Page Title="" Language="C#" MasterPageFile="~/Whir_System/Master/DialogMasterPage.master" AutoEventWireup="true" CodeFile="Share.aspx.cs" Inherits="Whir_System_ModuleMark_Common_Share" %>

<%@ Import Namespace="Whir.Language" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="dbshare-main">
        <div class="bdsharebuttonbox">
            <a href="#" class="bds_weixin" data-cmd="weixin" title="<%="分享到微信".ToLang() %>"></a>
            <a href="#" class="bds_sqq" data-cmd="sqq" title="<%="分享到QQ好友".ToLang() %>"></a>
            <a href="#" class="bds_tsina" data-cmd="tsina" title="<%="分享到新浪微博".ToLang() %>"></a>
            <a href="#" class="bds_fbook" data-cmd="fbook" title="<%="分享到Facebook".ToLang() %>"></a>
            <a href="#" class="bds_twi" data-cmd="twi" title="<%="分享到Twitter".ToLang() %>"></a>
        </div>
        <textarea style="display: none" name="_bdText"><%=Share.Title%></textarea>
        <textarea style="display: none" name="_bdDesc"><%=Share.Description%></textarea>
    </div>
    <style type="text/css">
        body{overflow: hidden !important;}
    </style>
    <script type="text/javascript">
        $(".bds_weixin").click(function () {
            var ifram = $("#dialogiframe", window.parent.document);
            ifram.width(300);
            ifram.height(300);

            setTimeout(close, 100);

            function close() {
                $(".bd_weixin_popup_close").click(function () {
                    var ifram = $("#dialogiframe", window.parent.document);
                    ifram.width(230);
                    ifram.height(65);
                });
            }
        });

        window._bd_share_config = {
            "common": {
                "bdSnsKey": {},
                "bdText": $("textarea[name=_bdText]").val(),
                "bdDesc": $("textarea[name=_bdDesc]").val(),
                "bdUrl": '<%=Share.Url%>',
                "bdPic": ReplaceImg('<%=Share.ImageUrl%>'),
                "bdMini": "2",
                "bdMiniList": false,
                "bdStyle": "0",
                "bdSize": "32"
            },
            "share": {}
        };
        with (document) 0[(getElementsByTagName('head')[0] || body).appendChild(createElement('script')).src = 'http://bdimg.share.baidu.com/static/api/js/share.js?v=89860593.js?cdnversion=' + ~(-new Date() / 36e5)];

        function ReplaceImg(img) {
            var imgs = img.split('/');
            var result = "";
            for (var i = 0; i < imgs.length; i++) {
                result += encodeURIComponent(imgs[i]) + "/";
            }
            if (result != "") {
                return result = result.substring(0, result.length - 1);
            }
            return result;
        }
    </script>
</asp:Content>

