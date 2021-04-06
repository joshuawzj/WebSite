<%@ Control Language="C#" %>  
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server"> 
   
    /*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn/template/include/head.html

 *生成时间：2021-04-06 09:46:25

 */


    
	/*url变量*/
    public Whir.Domain.Column PageColumn
    {
        get {
            if (ViewState["PageColumn"] == null)
            {
               // return null;
			   return new Whir.Domain.Column();
            }
            else
            {
                return ViewState["PageColumn"] as Whir.Domain.Column;
            }
        }
        set
        {
            ViewState["PageColumn"] = value;
        }
    }
	public Whir.Domain.SiteInfo PageSiteInfo
    {
        get {
            if (ViewState["PageSiteInfo"] == null)
            {
                return null;
            }
            else
            {
                return ViewState["PageSiteInfo"] as Whir.Domain.SiteInfo;
            }
        }
        set
        {
            ViewState["PageSiteInfo"] = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    { 
	    Whir.Label.LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn); 

 
		/*url变量赋值*/
        DataBind();
    } 
  
</script> 
 
<!--
                       (0 0)
   +=============oOO====(_)================+
   |   Powered By wanhu - www.wanhu.com.cn |
   |   Tel:400-888-0035  020-85575672      |
   |   Creation:2015.06.27                 |
   |   ezEip v4.1.0                        |
   +==========================oOO==========+
                      |__|__|
                       || ||
                      ooO Ooo
-->
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
<meta name="viewport" content="width=device-width, initial-scale=1">
<meta name="Author" content="万户网络">
<meta content="万户网络 www.wanhu.com.cn" name="design">
<wtl:seo runat="server"     ></wtl:seo>
<link rel="Shortcut icon" href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/ico/favicon.ico" />

<script src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>scripts/html5media/1.1.8/html5media.min.js"></script>
<script type="text/javascript" src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>scripts/jquery-3.3.1.min.js"></script>
<!--网页常用效果 s-->
<link href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>css/iconfont.css" rel="stylesheet" type="text/css"><!--字体图标-->
<link rel="stylesheet" type="text/css" href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>css/slick.css"/>
<script src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>scripts/jquery.slick.js"></script> 
<link rel="stylesheet" href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>css/magnific-popup.css">
<script src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>scripts/jquery.magnific-popup.min.js"></script> 
<link rel="stylesheet" href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>css/animate.css">
<script src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>scripts/wow.js"></script>

<script type="text/javascript" src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>scripts/jquery.flexslider-min.js"></script>
<script type="text/javascript" src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>scripts/a.whir_menu.2.0.js"></script>
<script type="text/javascript" src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>scripts/a.whir.search.js"></script>
<link rel="stylesheet" type="text/css" href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>css/responsive2.0.css" />
<link rel="stylesheet" type="text/css" href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>css/responsive.css" />
<script>
    new WOW().init();
	var url = window.location.href;
                if (url.indexOf("https") < 0) {
                    url = url.replace("http:", "https:");
                    window.location.replace(url);
                }
</script>
<!--网页常用效果 end-->
<link rel="stylesheet" type="text/css" href="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>css/style.css" />


<!--[if IE 6]>
<script type="text/javascript" src="scripts/iepng.js"></script>
<script type="text/javascript"> 
EvPNG.fix('img,dl,div,a,li'); </script>
<![endif]-->
<!--[if lt IE 9]>
  <script type="text/javascript" src="scripts/html5shiv.v3.72.min.js"></script>
  <script type="text/javascript" src="scripts/respond.min.js"></script>
<![endif]-->
<!--[if IE]>
<script>
(function(){if(!/*@cc_on!@*/0)return;var e = "header,footer,nav,article,section,address,aside".split(','),i=e.length;while(i--){document.createElement(e[i])}})()
</script>
<![endif]-->
