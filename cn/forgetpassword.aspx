<%@ Page Language="C#" ValidateRequest="false" Inherits="FrontBasePage" %>
<%@ Import Namespace="Whir.Framework" %>
<%@ Import Namespace="Whir.Service" %>
<%@ Import Namespace="Whir.Label" %>
<script type="text/C#" runat="server">
/*

 *描述： 本文件由ezEIP模板发布引擎生成，请勿直接更改此文件！！ 

 *模板路径：C:\wwwroot\WebSite\cn\template\忘记密码.shtml

 *生成时间：2021-03-19 09:59:03

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
    <h1>忘记密码</h1> 
   <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_08.png"/>
      <input type="text" name="11238" id="11238" placeholder="登录名"/>
    </div>
	<div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_08.png"/>
      <input type="text" id="11243" name="11243"  placeholder="邮箱"/> 
    </div>
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_10.png"/>
      <input type="text" placeholder="验证码" id="code"/>
      <button onclick="send()">发送验证码</button>
    </div>
    <div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_12.png"/>
      <input id="11239" name="11239" type="password"  placeholder="密码（不少于8位）"/> 
    </div> 
	<div class="zc_inp">
      <img src="<%=Whir.Service.ServiceFactory.SiteInfoService.GetSitePath(PageSiteInfo)%>images/zc_12.png"/>
      <input id="renewpassword" name="renewpassword" type="password"  placeholder="确认密码"/> 
    </div> 
    <div class="zc_btn">
      <a href="#" id="ljzc" class="ljzc" onclick="sava()">确认修改</a>
    </div>
  </div>

</div>
</div>

<script>
    function send() { 
	
		var email = $("#11243").val();
        var reg = /^\w+([-+.]\w+)*@\w+([-.]\\w+)*\.\w+([-.]\w+)*$/;
        if (!reg.test(email)) { alert("邮箱格式不正确！"); return; }

        $.post("<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>ajax/AjaxData.aspx", { action: "sendcodeForget", email }, function (ref) {
            if (ref == "1") {
                alert("已发送验证码至您的邮箱，请进入邮箱查看！");
            } else {
                alert(ref);
            }
        });
    }
    function sava() {

		var loginname = $("#11238").val();
        if (loginname == "") { alert("请输入登录名！"); return; }

        var email = $("#11243").val();
        var reg = /^\w+([-+.]\w+)*@\w+([-.]\\w+)*\.\w+([-.]\w+)*$/;
        if (!reg.test(email)) { alert("邮箱格式不正确！"); return; } 
	
        var code = $("#code").val();
        if (code == "") { alert("请输入验证码！"); return; }

        var pwd = $("#11239").val();
        if (pwd == "") { alert("请输入密码！"); return; }
        if (pwd.length < 8) {
            alert("密码长度不少于8位！"); return;
        }
		
		var renewpassword = $("#renewpassword").val();
		if (renewpassword == "") { alert("请输入确认密码！"); return; }
		if (pwd != renewpassword) {  
			alert("确认新密码与新密码不一致！"); return; 
		} 


        $.post("<%=Whir.ezEIP.Web.SysManagePageBase.Insten.AppName %>ajax/AjaxData.aspx", { action:"forget", loginname,email,code, pwd,renewpassword }, function (ref) {
            if (ref == "1") {
                 alert("修改成功");
            } else {
                alert(ref);
                location.href = location.href;
            }
        });
        
    }
</script>
 
<!--top--> 
     <wtl:include runat="server"      filename="bottom.html"></wtl:include>
<!--top End--> 
</body>

</html>
