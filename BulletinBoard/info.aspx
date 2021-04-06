<%@ Page Language="C#" ValidateRequest="false" Inherits="FrontBasePage" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server">
/*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn\template\新闻详情.shtml

 *生成时间：2021-04-02 17:26:03

 */

public Whir.Domain.Column PageColumn ; 
public Whir.Domain.SiteInfo PageSiteInfo ; 
/*url变量*/ 
protected void Page_Load(object sender, EventArgs e){
    /*url变量赋值*/ 
    PageColumn = ServiceFactory.ColumnService.SingleOrDefault<Whir.Domain.Column>(9);
    PageSiteInfo = ServiceFactory.SiteInfoService.SingleOrDefault<Whir.Domain.SiteInfo>(1);
    Whir.Label.LabelHelper.Instance.SerColumnAndSiteInfoToChildControl(this, PageSiteInfo, PageColumn); 
    DataBind();
}
</script><!DOCTYPE html>
<html lang="zh-cn">
<head>
<wtl:RemoveColor  runat="server"></wtl:RemoveColor>
<meta charset="utf-8">
    <wtl:include runat="server"      filename="head.html"></wtl:include>
	 <link href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>res/js/layer/skin/default/layer.css" rel="stylesheet" />
    <script src="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>res/js/layer/layer.js"></script>
</head>
<body>
<!--top--> 
        <wtl:include runat="server"      filename="top.html"></wtl:include>
<!--top End--> 
<div class="ban">
  <img src='<wtl:banner runat="server"     ></wtl:banner>'/>
</div>
<div class="zf">
  <div class="auto auto_1200">
     <wtl:inforarea runat="server" ><ItemTemplate><!--RighInfo002939 star-->
<div class="RighInfo002939">
  <div class="auto auto_1280">
      <div class="name">
         <h1><%# Container.DataRow["title"].ToReplace() %></h1>
          <div class="time"><time>发布时间：<%# Container.DataRow["CreateDate"].ToReplace().ToDateTime().ToString("yyyy-MM-dd") %></time> <div class="share"><em>分享到：</em><div class="bshare-custom"><a title="分享到QQ空间" class="bshare-qzone"></a><a title="分享到新浪微博" class="bshare-sinaminiblog"></a><a title="分享到人人网" class="bshare-renren"></a><a title="分享到腾讯微博" class="bshare-qqmb"></a><a title="分享到网易微博" class="bshare-neteasemb"></a><a title="更多平台" class="bshare-more bshare-more-icon more-style-addthis"></a><span class="BSHARE_COUNT bshare-share-count">0</span></div></div></div>
      </div>
      <article class="edit-info">
	  <wtl:if runat="server"      testtype="<%# CheckTime()%>" TestOperate="Equals"   testvalue="0">
	  <successTemplate>
	  	<script>
			layer.confirm('您尚未开通服务，或者身份已过期，请重新续费。', {
			  btn: ['确定','取消'] //按钮
			}, function(){
			  location.href='<%#Whir.Service.ServiceFactory.ColumnService.GetColumnListLink(4,true,3)%>';
			}, function(){
			  
			});
		</script>
	  </successTemplate>
	  <failureTemplate>
	  	<%# ((Whir.Label.Dynamic.DynamicDataContainer)((Whir.Label.Dynamic.DynamicDataContainer)Container).Parent.Parent).DataRow["content"].ToReplace() %>
	  </failureTemplate>
	  </wtl:if>
      </article>
      <dl class="info-page clearfix">
          
         <dd class="dot"> <wtl:Content runat="server"       Field="title" Type="prepage" lefttext="<b>上一篇：</b>" IsAutoLink="true"></wtl:Content></dd>
         <dd class="dot"> <wtl:Content runat="server"       Field="title" Type="nextpage" lefttext="<b>下一篇：</b>" IsAutoLink="true"></wtl:Content></dd>
      </dl>
   </div>
</div>
<!--RighInfo002939 end-->
  </ItemTemplate></wtl:inforarea>
  </div>
</div>

<!--bottom--> 
       <wtl:include runat="server"      filename="bottom.html"></wtl:include>
<!--bottom End--> 
</body>

</html>
