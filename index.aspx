<%@ Page Language="C#" ValidateRequest="false" Inherits="FrontBasePage" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server">
/*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn\template\index.shtml

 *生成时间：2021-03-25 15:56:34

 */

public Whir.Domain.Column PageColumn ; 
public Whir.Domain.SiteInfo PageSiteInfo ; 
/*url变量*/ 
protected void Page_Load(object sender, EventArgs e){
    /*url变量赋值*/ 
    PageColumn = ServiceFactory.ColumnService.SingleOrDefault<Whir.Domain.Column>(0);
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
</head>
<body>

<!--top--> 
      <wtl:include runat="server"      filename="top.html"></wtl:include>
<!--top End--> 
<div class="Banner002625">
  <ul class="ul slides">
    <wtl:list runat="server"  columnid="7"><ItemTemplate>
    <li>
     <a href='<%# Container.DataRow["linkurl"].ToReplace() %>'></a>
     <div class="xycenterbox"><img src="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.UploadFilePath %><%# Container.DataRow["Imageurl"].ToReplace() %>"></div>
    <img src="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.UploadFilePath %><%# Container.DataRow["Imageurl2"].ToReplace() %>">
    <div class="ban_btn">
      <div class="ban_left">
        <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/dl.png"/>
        <a href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>cn/denglu.aspx">登录</a>
      </div>
      <div class="ban_right">
       <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc.png"/>
       <a href="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>cn/zhuche.aspx">注册</a>
     </div>
    </div>
    </li>
        </ItemTemplate></wtl:list>
  </ul>
  <script>
  window.onload=function() {
   $('.Banner002625').flexslider({
    slideshowSpeed: 6000
   });
 }
  </script>
 </div>
 <!--Banner002625 end-->
<!-- content -->
<div class="HomeMain">
  <div class="auto auto_1200">
    <div class="HomeMain_box">
      <div class="main_list">
          <wtl:list runat="server"  columnid="4"><ItemTemplate>
              <div class="main_li">
          <img src="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.UploadFilePath %><%# Container.DataRow["Imageurl"].ToReplace() %>"/>
          <h1><%# Container.DataRow["title"].ToReplace() %></h1>
          <p><%# Container.DataRow["content"].ToReplace() %></p>
          <ul>
              <wtl:list runat="server"  sql0='<%# Container.DataRow["whir_U_COntent_pid"].ToReplace() %>' sql="select Whir_U_Category_PId,CategoryName from Whir_U_Category  where isdel = 0 and Whir_U_Category_PId in (select value = substring(a.lable , b.number , charindex(',' , a.lable + ',' , b.number) - b.number)from (select lable from Whir_U_Content where typeid = 4 and Whir_U_Content_PId = {0} ) a join master..spt_values  b  on b.type='p' and b.number between 1 and len(a.lable) where substring(',' + a.lable , b.number , 1) = ',' )" ><ItemTemplate> 
                  <li><%# Container.DataRow["CategoryName"].ToReplace() %></li> 
            </ItemTemplate></wtl:list> 

       
          </ul>
          <div class="btn_a"><a href='<%# Container.PageUrl("") %>'>立即订阅</a></div>
        </div>
         </ItemTemplate></wtl:list>
        
        
      </div>
    </div>

    <div class="Home_bot">
      <div class="home_bot_left">
        <h2>真知灼见·管理风险</h2>
        <p>面向全球华人，普及财经常识，构筑风控防线，守卫财务安康</p>
      </div>
      <div class="home_bot_right">
		<wtl:list runat="server"  columnid="3"><ItemTemplate>
        <a href='<%# Container.DataRow["title"].ToReplace() %>' target="_blank"><img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/fx.png"/></a>
        <a href='<%# Container.DataRow["lxdh"].ToReplace() %>'  target="_blank"><img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/weibo.png"/></a>
		</ItemTemplate></wtl:list>
      </div>
    </div>

  </div>
</div>
<!-- content -->
<!--top--> 
       <wtl:include runat="server"      filename="bottom.html"></wtl:include>
<!--top End--> 
</body>
</html>
