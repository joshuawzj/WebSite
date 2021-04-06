<%@ Control Language="C#" %>  
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server"> 
   
    /*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn/template/include/bottom.html

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
 
<footer class="bottom">
  <div class="auto auto_1200">
    <wtl:inforarea runat="server"  columnid="3"><ItemTemplate>
        <%# Container.DataRow["content"].ToReplace() %>
    </ItemTemplate></wtl:inforarea>
  </div>
</footer>

<script type="text/javascript" src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>scripts/common.js"></script> 
<script type="text/javascript">
//整站无图处理
jQuery.each(jQuery("img"), function (i, n) { jQuery(n).error(function () { n.src = 'uploadfiles/nopic.jpg'; }); n.src = n.src; });
</script> 
<wtl:Service runat="server"     ></wtl:Service><wtl:Statis runat="server"     ></wtl:Statis>