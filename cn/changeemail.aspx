<%@ Page Language="C#" ValidateRequest="false" Inherits="FrontBasePage" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server">
/*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn\template\修改邮箱.shtml

 *生成时间：2021-04-02 17:30:33

 */

public Whir.Domain.Column PageColumn ; 
public Whir.Domain.SiteInfo PageSiteInfo ; 
/*url变量*/ 
protected void Page_Load(object sender, EventArgs e){
    /*url变量赋值*/ 
    PageColumn = ServiceFactory.ColumnService.SingleOrDefault<Whir.Domain.Column>(0);
    ServiceFactory.ColumnService.CheckCanBrowseAspx();
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
<div class="registered" style="background: url(<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>uploadfiles/image/zc_02.jpg) no-repeat center center;background-size: cover;">
<div class="zc_box">
    
  <div class="zc_list">
    <h1>修改邮箱</h1> 
	
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_08.png"/>
      <input id="email1" name="email1"  placeholder="原邮箱"/> 
    </div> 
	
   
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_08.png"/>
      <input id="email2" name="email2"  placeholder="新邮箱"/> 
    </div> 
	<div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_08.png"/>
      <input id="email3" name="email3"   placeholder="确认邮箱"/> 
    </div> 
    <div class="zc_btn">
      <a href="#" id="ljzc" class="ljzc" onclick="sava()">确认修改</a>
    </div>
  </div>

</div>
</div>


<!--top--> 
     <wtl:include runat="server"      filename="bottom.html"></wtl:include>
<!--top End--> 
</body>

</html>
<script>
 
    function sava() {

        

        var email1 = $("#email1").val();
		var email2 = $("#email2").val();
		var email3 = $("#email3").val();
		
        var reg = /^\w+([-+.]\w+)*@\w+([-.]\\w+)*\.\w+([-.]\w+)*$/;
        if (!reg.test(email1)) { alert("原邮箱格式不正确！"); return; }
        if (!reg.test(email2)) { alert("新邮箱格式不正确！"); return; }
        if (!reg.test(email3)) { alert("确认邮箱格式不正确！"); return; }
		
		if (email2 != email3) {  
			alert("确认邮箱和新邮箱不一致"); return; 
		} 
		
        $.post("<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>ajax/AjaxData.aspx", { action:"modifyemail", email1, email2,email3 }, function (ref) {
            if (ref == "1") {
                 alert("修改成功");
				 location.href ="<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>yhzx/index.aspx";
            } else {
                alert(ref);
               location.href = location.href;
            }
        });
        
    }
</script>