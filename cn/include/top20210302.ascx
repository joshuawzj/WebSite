<%@ Control Language="C#" %>  
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server"> 
   
    /*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn/template/include/top20210302.html

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
 

<div class="Top002999">
  <div class="auto auto_1200">
    <div class="logo">
        <wtl:inforarea runat="server"  columnid="3"><ItemTemplate>
            <a href='<%=Whir.Service.ServiceFactory.SiteInfoService.GetSiteUrlLinkAspx("indexurl",PageSiteInfo)%>'>
                <img src="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.UploadFilePath %><%# Container.DataRow["logo"].ToReplace() %>">
            </a>
        </ItemTemplate></wtl:inforarea>
      
  </div>
   <a class="open-nav"><i></i><i></i><i></i></a>
   <div class="top-main">
       <a class="open-menu" rel="absolute"><i></i><i></i><i></i></a>
       <menu>
           <ul class="ul clearfix" id="menu" type="Vertical" rel="li-relative">
               <li><span><a href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>/yhzx/index.aspx">用户中心</a></span></li>
               <li><span><a href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>cn/zhuche.aspx">注册</a></span></li>
               <li><span><a href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>about/index.aspx">关于光盐财经</a></span> </li>
           </ul>
       </menu>
       <wtl:inforarea runat="server"  columnid="3"><ItemTemplate>
           <div class="gupiao">联系我们 <span><%# Container.DataRow["phone"].ToReplace() %></span></div>
       </ItemTemplate></wtl:inforarea>
   </div>
  </div>
</div>
<script>
var m="0" //选中
whirmenu.main("Vertical","en",m);
whirsearch.open(".open-search",".top-search");//打开搜索
whirsearch.search("#TopBtn","TopKey","请输入关键字","跳转链接")//搜索
whirOpen.one(".open-nav","body","pcmenu-show",".top-main");
</script>
<!--Top002999 end-->